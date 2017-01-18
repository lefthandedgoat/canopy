(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Event", "./Symbol"], function (require, exports) {
    "use strict";
    var Event_1 = require("./Event");
    var Symbol_1 = require("./Symbol");
    var Timer = (function () {
        function Timer(interval) {
            this.Interval = interval > 0 ? interval : 100;
            this.AutoReset = true;
            this._elapsed = new Event_1.default();
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
        Timer.prototype[Symbol_1.default.reflection] = function () {
            return {
                type: "System.Timers.Timer",
                interfaces: ["System.IDisposable"]
            };
        };
        return Timer;
    }());
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = Timer;
});
