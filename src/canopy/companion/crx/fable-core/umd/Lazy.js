(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports"], function (require, exports) {
    "use strict";
    function createFromValue(v) {
        return new Lazy(function () { return v; });
    }
    exports.createFromValue = createFromValue;
    var Lazy = (function () {
        function Lazy(factory) {
            this.factory = factory;
            this.isValueCreated = false;
        }
        Object.defineProperty(Lazy.prototype, "value", {
            get: function () {
                if (!this.isValueCreated) {
                    this.createdValue = this.factory();
                    this.isValueCreated = true;
                }
                return this.createdValue;
            },
            enumerable: true,
            configurable: true
        });
        return Lazy;
    }());
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = Lazy;
});
