import { createDisposable } from "./Util";
import { iterate as seqIterate } from "./Seq";
import { Observer } from "./Observable";
import { protect } from "./Observable";
var Event = (function () {
    function Event(_subscriber, delegates) {
        this._subscriber = _subscriber;
        this.delegates = delegates || new Array();
    }
    Event.prototype.Add = function (f) {
        this._addHandler(f);
    };
    Object.defineProperty(Event.prototype, "Publish", {
        get: function () {
            return this;
        },
        enumerable: true,
        configurable: true
    });
    Event.prototype.Trigger = function (value) {
        seqIterate(function (f) { return f(value); }, this.delegates);
    };
    Event.prototype._addHandler = function (f) {
        this.delegates.push(f);
    };
    Event.prototype._removeHandler = function (f) {
        var index = this.delegates.indexOf(f);
        if (index > -1)
            this.delegates.splice(index, 1);
    };
    Event.prototype.AddHandler = function (handler) {
        if (this._dotnetDelegates == null) {
            this._dotnetDelegates = new Map();
        }
        var f = function (x) { handler(null, x); };
        this._dotnetDelegates.set(handler, f);
        this._addHandler(f);
    };
    Event.prototype.RemoveHandler = function (handler) {
        if (this._dotnetDelegates != null) {
            var f = this._dotnetDelegates.get(handler);
            if (f != null) {
                this._dotnetDelegates.delete(handler);
                this._removeHandler(f);
            }
        }
    };
    Event.prototype._subscribeFromObserver = function (observer) {
        var _this = this;
        if (this._subscriber)
            return this._subscriber(observer);
        var callback = observer.OnNext;
        this._addHandler(callback);
        return createDisposable(function () { return _this._removeHandler(callback); });
    };
    Event.prototype._subscribeFromCallback = function (callback) {
        var _this = this;
        this._addHandler(callback);
        return createDisposable(function () { return _this._removeHandler(callback); });
    };
    Event.prototype.Subscribe = function (arg) {
        return typeof arg == "function"
            ? this._subscribeFromCallback(arg)
            : this._subscribeFromObserver(arg);
    };
    return Event;
}());
export default Event;
export function add(callback, sourceEvent) {
    sourceEvent.Subscribe(new Observer(callback));
}
export function choose(chooser, sourceEvent) {
    var source = sourceEvent;
    return new Event(function (observer) {
        return source.Subscribe(new Observer(function (t) {
            return protect(function () { return chooser(t); }, function (u) { if (u != null)
                observer.OnNext(u); }, observer.OnError);
        }, observer.OnError, observer.OnCompleted));
    }, source.delegates);
}
export function filter(predicate, sourceEvent) {
    return choose(function (x) { return predicate(x) ? x : null; }, sourceEvent);
}
export function map(mapping, sourceEvent) {
    var source = sourceEvent;
    return new Event(function (observer) {
        return source.Subscribe(new Observer(function (t) {
            return protect(function () { return mapping(t); }, observer.OnNext, observer.OnError);
        }, observer.OnError, observer.OnCompleted));
    }, source.delegates);
}
export function merge(event1, event2) {
    var source1 = event1;
    var source2 = event2;
    return new Event(function (observer) {
        var stopped = false, completed1 = false, completed2 = false;
        var h1 = source1.Subscribe(new Observer(function (v) { if (!stopped)
            observer.OnNext(v); }, function (e) {
            if (!stopped) {
                stopped = true;
                observer.OnError(e);
            }
        }, function () {
            if (!stopped) {
                completed1 = true;
                if (completed2) {
                    stopped = true;
                    observer.OnCompleted();
                }
            }
        }));
        var h2 = source2.Subscribe(new Observer(function (v) { if (!stopped)
            observer.OnNext(v); }, function (e) {
            if (!stopped) {
                stopped = true;
                observer.OnError(e);
            }
        }, function () {
            if (!stopped) {
                completed2 = true;
                if (completed1) {
                    stopped = true;
                    observer.OnCompleted();
                }
            }
        }));
        return createDisposable(function () {
            h1.Dispose();
            h2.Dispose();
        });
    }, source1.delegates.concat(source2.delegates));
}
export function pairwise(sourceEvent) {
    var source = sourceEvent;
    return new Event(function (observer) {
        var last = null;
        return source.Subscribe(new Observer(function (next) {
            if (last != null)
                observer.OnNext([last, next]);
            last = next;
        }, observer.OnError, observer.OnCompleted));
    }, source.delegates);
}
export function partition(predicate, sourceEvent) {
    return [filter(predicate, sourceEvent), filter(function (x) { return !predicate(x); }, sourceEvent)];
}
export function scan(collector, state, sourceEvent) {
    var source = sourceEvent;
    return new Event(function (observer) {
        return source.Subscribe(new Observer(function (t) {
            protect(function () { return collector(state, t); }, function (u) { state = u; observer.OnNext(u); }, observer.OnError);
        }, observer.OnError, observer.OnCompleted));
    }, source.delegates);
}
export function split(splitter, sourceEvent) {
    return [choose(function (v) { return splitter(v).valueIfChoice1; }, sourceEvent), choose(function (v) { return splitter(v).valueIfChoice2; }, sourceEvent)];
}
