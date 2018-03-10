import Event from "./Event";
import FSymbol from "./Symbol";
var Timer = (function () {
    function Timer(interval) {
        this.Interval = interval > 0 ? interval : 100;
        this.AutoReset = true;
        this._elapsed = new Event();
    }
    Object.defineProperty(Timer.prototype, "Elapsed", {
        get: function () {
            return this._elapsed;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Timer.prototype, "Enabled", {
        get: function () {
            return this._enabled;
        },
        set: function (x) {
            var _this = this;
            if (!this._isDisposed && this._enabled != x) {
                if (this._enabled = x) {
                    if (this.AutoReset) {
                        this._intervalId = setInterval(function () {
                            if (!_this.AutoReset)
                                _this.Enabled = false;
                            _this._elapsed.Trigger(new Date());
                        }, this.Interval);
                    }
                    else {
                        this._timeoutId = setTimeout(function () {
                            _this.Enabled = false;
                            _this._timeoutId = 0;
                            if (_this.AutoReset)
                                _this.Enabled = true;
                            _this._elapsed.Trigger(new Date());
                        }, this.Interval);
                    }
                }
                else {
                    if (this._timeoutId) {
                        clearTimeout(this._timeoutId);
                        this._timeoutId = 0;
                    }
                    if (this._intervalId) {
                        clearInterval(this._intervalId);
                        this._intervalId = 0;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Timer.prototype.Dispose = function () {
        this.Enabled = false;
        this._isDisposed = true;
    };
    Timer.prototype.Close = function () {
        this.Dispose();
    };
    Timer.prototype.Start = function () {
        this.Enabled = true;
    };
    Timer.prototype.Stop = function () {
        this.Enabled = false;
    };
    Timer.prototype[FSymbol.reflection] = function () {
        return {
            type: "System.Timers.Timer",
            interfaces: ["System.IDisposable"]
        };
    };
    return Timer;
}());
export default Timer;
