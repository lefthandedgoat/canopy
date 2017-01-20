var Trampoline = (function () {
    function Trampoline() {
        this.callCount = 0;
    }
    Object.defineProperty(Trampoline, "maxTrampolineCallCount", {
        get: function () {
            return 2000;
        },
        enumerable: true,
        configurable: true
    });
    Trampoline.prototype.incrementAndCheck = function () {
        return this.callCount++ > Trampoline.maxTrampolineCallCount;
    };
    Trampoline.prototype.hijack = function (f) {
        this.callCount = 0;
        setTimeout(f, 0);
    };
    return Trampoline;
}());
export { Trampoline };
export function protectedCont(f) {
    return function (ctx) {
        if (ctx.cancelToken.isCancelled)
            ctx.onCancel("cancelled");
        else if (ctx.trampoline.incrementAndCheck())
            ctx.trampoline.hijack(function () {
                try {
                    f(ctx);
                }
                catch (err) {
                    ctx.onError(err);
                }
            });
        else
            try {
                f(ctx);
            }
            catch (err) {
                ctx.onError(err);
            }
    };
}
export function protectedBind(computation, binder) {
    return protectedCont(function (ctx) {
        computation({
            onSuccess: function (x) { return binder(x)(ctx); },
            onError: ctx.onError,
            onCancel: ctx.onCancel,
            cancelToken: ctx.cancelToken,
            trampoline: ctx.trampoline
        });
    });
}
export function protectedReturn(value) {
    return protectedCont(function (ctx) { return ctx.onSuccess(value); });
}
var AsyncBuilder = (function () {
    function AsyncBuilder() {
    }
    AsyncBuilder.prototype.Bind = function (computation, binder) {
        return protectedBind(computation, binder);
    };
    AsyncBuilder.prototype.Combine = function (computation1, computation2) {
        return this.Bind(computation1, function () { return computation2; });
    };
    AsyncBuilder.prototype.Delay = function (generator) {
        return protectedCont(function (ctx) { return generator()(ctx); });
    };
    AsyncBuilder.prototype.For = function (sequence, body) {
        var iter = sequence[Symbol.iterator]();
        var cur = iter.next();
        return this.While(function () { return !cur.done; }, this.Delay(function () {
            var res = body(cur.value);
            cur = iter.next();
            return res;
        }));
    };
    AsyncBuilder.prototype.Return = function (value) {
        return protectedReturn(value);
    };
    AsyncBuilder.prototype.ReturnFrom = function (computation) {
        return computation;
    };
    AsyncBuilder.prototype.TryFinally = function (computation, compensation) {
        return protectedCont(function (ctx) {
            computation({
                onSuccess: function (x) {
                    compensation();
                    ctx.onSuccess(x);
                },
                onError: function (x) {
                    compensation();
                    ctx.onError(x);
                },
                onCancel: function (x) {
                    compensation();
                    ctx.onCancel(x);
                },
                cancelToken: ctx.cancelToken,
                trampoline: ctx.trampoline
            });
        });
    };
    AsyncBuilder.prototype.TryWith = function (computation, catchHandler) {
        return protectedCont(function (ctx) {
            computation({
                onSuccess: ctx.onSuccess,
                onCancel: ctx.onCancel,
                cancelToken: ctx.cancelToken,
                trampoline: ctx.trampoline,
                onError: function (ex) {
                    try {
                        catchHandler(ex)(ctx);
                    }
                    catch (ex2) {
                        ctx.onError(ex2);
                    }
                }
            });
        });
    };
    AsyncBuilder.prototype.Using = function (resource, binder) {
        return this.TryFinally(binder(resource), function () { return resource.Dispose(); });
    };
    AsyncBuilder.prototype.While = function (guard, computation) {
        var _this = this;
        if (guard())
            return this.Bind(computation, function () { return _this.While(guard, computation); });
        else
            return this.Return(void 0);
    };
    AsyncBuilder.prototype.Zero = function () {
        return protectedCont(function (ctx) { return ctx.onSuccess(void 0); });
    };
    return AsyncBuilder;
}());
export { AsyncBuilder };
export var singleton = new AsyncBuilder();
