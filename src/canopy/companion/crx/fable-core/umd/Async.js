import { Trampoline } from "./AsyncBuilder";
import { protectedCont } from "./AsyncBuilder";
import { protectedBind } from "./AsyncBuilder";
import { protectedReturn } from "./AsyncBuilder";
import { choice1Of2 } from "./Choice";
import { choice2Of2 } from "./Choice";
import { map } from "./Seq";
var Async = (function () {
    function Async() {
    }
    return Async;
}());
export default Async;
function emptyContinuation(x) {
}
export function awaitPromise(p) {
    return fromContinuations(function (conts) {
        return p.then(conts[0]).catch(function (err) {
            return (err == "cancelled" ? conts[2] : conts[1])(err);
        });
    });
}
export function cancellationToken() {
    return protectedCont(function (ctx) { return ctx.onSuccess(ctx.cancelToken); });
}
export var defaultCancellationToken = { isCancelled: false };
export function catchAsync(work) {
    return protectedCont(function (ctx) {
        work({
            onSuccess: function (x) { return ctx.onSuccess(choice1Of2(x)); },
            onError: function (ex) { return ctx.onSuccess(choice2Of2(ex)); },
            onCancel: ctx.onCancel,
            cancelToken: ctx.cancelToken,
            trampoline: ctx.trampoline
        });
    });
}
export function fromContinuations(f) {
    return protectedCont(function (ctx) { return f([ctx.onSuccess, ctx.onError, ctx.onCancel]); });
}
export function ignore(computation) {
    return protectedBind(computation, function (x) { return protectedReturn(void 0); });
}
export function parallel(computations) {
    return awaitPromise(Promise.all(map(function (w) { return startAsPromise(w); }, computations)));
}
export function sleep(millisecondsDueTime) {
    return protectedCont(function (ctx) {
        setTimeout(function () { return ctx.cancelToken.isCancelled ? ctx.onCancel("cancelled") : ctx.onSuccess(void 0); }, millisecondsDueTime);
    });
}
export function start(computation, cancellationToken) {
    return startWithContinuations(computation, cancellationToken);
}
export function startImmediate(computation, cancellationToken) {
    return start(computation, cancellationToken);
}
export function startWithContinuations(computation, continuation, exceptionContinuation, cancellationContinuation, cancelToken) {
    if (typeof continuation !== "function") {
        cancelToken = continuation;
        continuation = null;
    }
    var trampoline = new Trampoline();
    computation({
        onSuccess: continuation ? continuation : emptyContinuation,
        onError: exceptionContinuation ? exceptionContinuation : emptyContinuation,
        onCancel: cancellationContinuation ? cancellationContinuation : emptyContinuation,
        cancelToken: cancelToken ? cancelToken : defaultCancellationToken,
        trampoline: trampoline
    });
}
export function startAsPromise(computation, cancellationToken) {
    return new Promise(function (resolve, reject) {
        return startWithContinuations(computation, resolve, reject, reject, cancellationToken ? cancellationToken : defaultCancellationToken);
    });
}
