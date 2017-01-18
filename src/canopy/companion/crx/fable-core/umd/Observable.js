(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Util", "./Symbol"], function (require, exports) {
    "use strict";
    var Util_1 = require("./Util");
    var Symbol_1 = require("./Symbol");
    var Observer = (function () {
        function Observer(onNext, onError, onCompleted) {
            this.OnNext = onNext;
            this.OnError = onError || (function (e) { });
            this.OnCompleted = onCompleted || function () { };
        }
        Observer.prototype[Symbol_1.default.reflection] = function () {
            return { interfaces: ["System.IObserver"] };
        };
        return Observer;
    }());
    exports.Observer = Observer;
    var Observable = (function () {
        function Observable(subscribe) {
            this.Subscribe = subscribe;
        }
        Observable.prototype[Symbol_1.default.reflection] = function () {
            return { interfaces: ["System.IObservable"] };
        };
        return Observable;
    }());
    function protect(f, succeed, fail) {
        try {
            return succeed(f());
        }
        catch (e) {
            fail(e);
        }
    }
    exports.protect = protect;
    function add(callback, source) {
        source.Subscribe(new Observer(callback));
    }
    exports.add = add;
    function choose(chooser, source) {
        return new Observable(function (observer) {
            return source.Subscribe(new Observer(function (t) {
                return protect(function () { return chooser(t); }, function (u) { if (u != null)
                    observer.OnNext(u); }, observer.OnError);
            }, observer.OnError, observer.OnCompleted));
        });
    }
    exports.choose = choose;
    function filter(predicate, source) {
        return choose(function (x) { return predicate(x) ? x : null; }, source);
    }
    exports.filter = filter;
    function map(mapping, source) {
        return new Observable(function (observer) {
            return source.Subscribe(new Observer(function (t) {
                protect(function () { return mapping(t); }, observer.OnNext, observer.OnError);
            }, observer.OnError, observer.OnCompleted));
        });
    }
    exports.map = map;
    function merge(source1, source2) {
        return new Observable(function (observer) {
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
            var h2 = source2.Subscribe(new Observer(function (v) { if (!stopped) {
                observer.OnNext(v);
            } }, function (e) {
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
            return Util_1.createDisposable(function () {
                h1.Dispose();
                h2.Dispose();
            });
        });
    }
    exports.merge = merge;
    function pairwise(source) {
        return new Observable(function (observer) {
            var last = null;
            return source.Subscribe(new Observer(function (next) {
                if (last != null)
                    observer.OnNext([last, next]);
                last = next;
            }, observer.OnError, observer.OnCompleted));
        });
    }
    exports.pairwise = pairwise;
    function partition(predicate, source) {
        return [filter(predicate, source), filter(function (x) { return !predicate(x); }, source)];
    }
    exports.partition = partition;
    function scan(collector, state, source) {
        return new Observable(function (observer) {
            return source.Subscribe(new Observer(function (t) {
                protect(function () { return collector(state, t); }, function (u) { state = u; observer.OnNext(u); }, observer.OnError);
            }, observer.OnError, observer.OnCompleted));
        });
    }
    exports.scan = scan;
    function split(splitter, source) {
        return [choose(function (v) { return splitter(v).valueIfChoice1; }, source), choose(function (v) { return splitter(v).valueIfChoice2; }, source)];
    }
    exports.split = split;
    function subscribe(callback, source) {
        return source.Subscribe(new Observer(callback));
    }
    exports.subscribe = subscribe;
});
