(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./AsyncBuilder", "./AsyncBuilder", "./AsyncBuilder", "./AsyncBuilder", "./Choice", "./Choice", "./Seq"], function (require, exports) {
    "use strict";
    var AsyncBuilder_1 = require("./AsyncBuilder");
    var AsyncBuilder_2 = require("./AsyncBuilder");
    var AsyncBuilder_3 = require("./AsyncBuilder");
    var AsyncBuilder_4 = require("./AsyncBuilder");
    var Choice_1 = require("./Choice");
    var Choice_2 = require("./Choice");
    var Seq_1 = require("./Seq");
    var Async = (function () {
        function Async() {
        }
        return Async;
    }());
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = Async;
    function emptyContinuation(x) {
    }
    function awaitPromise(p) {
        return fromContinuations(function (conts) {
            return p.then(conts[0]).catch(function (err) {
                return (err == "cancelled" ? conts[2] : conts[1])(err);
            });
        });
    }
    exports.awaitPromise = awaitPromise;
    function cancellationToken() {
        return AsyncBuilder_2.protectedCont(function (ctx) { return ctx.onSuccess(ctx.cancelToken); });
    }
    exports.cancellationToken = cancellationToken;
    exports.defaultCancellationToken = { isCancelled: false };
    function catchAsync(work) {
        return AsyncBuilder_2.protectedCont(function (ctx) {
            work({
                onSuccess: function (x) { return ctx.onSuccess(Choice_1.choice1Of2(x)); },
                onError: function (ex) { return ctx.onSuccess(Choice_2.choice2Of2(ex)); },
                onCancel: ctx.onCancel,
                cancelToken: ctx.cancelToken,
                trampoline: ctx.trampoline
            });
        });
    }
    exports.catchAsync = catchAsync;
    function fromContinuations(f) {
        return AsyncBuilder_2.protectedCont(function (ctx) { return f([ctx.onSuccess, ctx.onError, ctx.onCancel]); });
    }
    exports.fromContinuations = fromContinuations;
    function ignore(computation) {
        return AsyncBuilder_3.protectedBind(computation, function (x) { return AsyncBuilder_4.protectedReturn(void 0); });
    }
    exports.ignore = ignore;
    function parallel(computations) {
        return awaitPromise(Promise.all(Seq_1.map(function (w) { return startAsPromise(w); }, computations)));
    }
    exports.parallel = parallel;
    function sleep(millisecondsDueTime) {
        return AsyncBuilder_2.protectedCont(function (ctx) {
            setTimeout(function () { return ctx.cancelToken.isCancelled ? ctx.onCancel("cancelled") : ctx.onSuccess(void 0); }, millisecondsDueTime);
        });
    }
    exports.sleep = sleep;
    function start(computation, cancellationToken) {
        return startWithContinuations(computation, cancellationToken);
    }
    exports.start = start;
    function startImmediate(computation, cancellationToken) {
        return start(computation, cancellationToken);
    }
    exports.startImmediate = startImmediate;
    function startWithContinuations(computation, continuation, exceptionContinuation, cancellationContinuation, cancelToken) {
        if (typeof continuation !== "function") {
            cancelToken = continuation;
            continuation = null;
        }
        var trampoline = new AsyncBuilder_1.Trampoline();
        computation({
            onSuccess: continuation ? continuation : emptyContinuation,
            onError: exceptionContinuation ? exceptionContinuation : emptyContinuation,
            onCancel: cancellationContinuation ? cancellationContinuation : emptyContinuation,
            cancelToken: cancelToken ? cancelToken : exports.defaultCancellationToken,
            trampoline: trampoline
        });
    }
    exports.startWithContinuations = startWithContinuations;
    function startAsPromise(computation, cancellationToken) {
        return new Promise(function (resolve, reject) {
            return startWithContinuations(computation, resolve, reject, reject, cancellationToken ? cancellationToken : exports.defaultCancellationToken);
        });
    }
    exports.startAsPromise = startAsPromise;
});
