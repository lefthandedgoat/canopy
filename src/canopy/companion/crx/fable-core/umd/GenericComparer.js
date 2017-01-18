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
    var GenericComparer = (function () {
        function GenericComparer(f) {
            this.Compare = f || Util_1.compare;
        }
        GenericComparer.prototype[Symbol_1.default.reflection] = function () {
            return { interfaces: ["System.IComparer"] };
        };
        return GenericComparer;
    }());
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = GenericComparer;
});
