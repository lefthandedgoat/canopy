import { defaultCancellationToken } from "./Async";
import { fromContinuations } from "./Async";
import { startImmediate } from "./Async";
var QueueCell = (function () {
    function QueueCell(message) {
        this.value = message;
    }
    return QueueCell;
}());
var MailboxQueue = (function () {
    function MailboxQueue() {
    }
    MailboxQueue.prototype.add = function (message) {
        var itCell = new QueueCell(message);
        if (this.firstAndLast) {
            this.firstAndLast[1].next = itCell;
            this.firstAndLast = [this.firstAndLast[0], itCell];
        }
        else
            this.firstAndLast = [itCell, itCell];
    };
    MailboxQueue.prototype.tryGet = function () {
        if (this.firstAndLast) {
            var value = this.firstAndLast[0].value;
            if (this.firstAndLast[0].next)
                this.firstAndLast = [this.firstAndLast[0].next, this.firstAndLast[1]];
            else
                delete this.firstAndLast;
            return value;
        }
        return void 0;
    };
    return MailboxQueue;
}());
var MailboxProcessor = (function () {
    function MailboxProcessor(body, cancellationToken) {
        this.body = body;
        this.cancellationToken = cancellationToken || defaultCancellationToken;
        this.messages = new MailboxQueue();
    }
    MailboxProcessor.prototype.__processEvents = function () {
        if (this.continuation) {
            var value = this.messages.tryGet();
            if (value) {
                var cont = this.continuation;
                delete this.continuation;
                cont(value);
            }
        }
    };
    MailboxProcessor.prototype.start = function () {
        startImmediate(this.body(this), this.cancellationToken);
    };
    MailboxProcessor.prototype.receive = function () {
        var _this = this;
        return fromContinuations(function (conts) {
            if (_this.continuation)
                throw new Error("Receive can only be called once!");
            _this.continuation = conts[0];
            _this.__processEvents();
        });
    };
    MailboxProcessor.prototype.post = function (message) {
        this.messages.add(message);
        this.__processEvents();
    };
    MailboxProcessor.prototype.postAndAsyncReply = function (buildMessage) {
        var result;
        var continuation;
        function checkCompletion() {
            if (result && continuation)
                continuation(result);
        }
        var reply = {
            reply: function (res) {
                result = res;
                checkCompletion();
            }
        };
        this.messages.add(buildMessage(reply));
        this.__processEvents();
        return fromContinuations(function (conts) {
            continuation = conts[0];
            checkCompletion();
        });
    };
    return MailboxProcessor;
}());
export default MailboxProcessor;
export function start(body, cancellationToken) {
    var mbox = new MailboxProcessor(body, cancellationToken);
    mbox.start();
    return mbox;
}
