(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports"], function (require, exports) {
    "use strict";
    var fableGlobal = function () {
        var globalObj = typeof window !== "undefined" ? window
            : (typeof global !== "undefined" ? global
                : (typeof self !== "undefined" ? self : null));
        if (typeof globalObj.__FABLE_CORE__ === "undefined") {
            globalObj.__FABLE_CORE__ = {
                types: new Map(),
                symbols: {
                    reflection: Symbol("reflection"),
                }
            };
        }
        return globalObj.__FABLE_CORE__;
    }();
    function setType(fullName, cons) {
        fableGlobal.types.set(fullName, cons);
    }
    exports.setType = setType;
    function getType(fullName) {
        return fableGlobal.types.get(fullName);
    }
    exports.getType = getType;
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = (fableGlobal.symbols);
});
