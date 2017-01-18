var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Util"], function (require, exports) {
    "use strict";
    var Util_1 = require("./Util");
    var AssertionError = (function (_super) {
        __extends(AssertionError, _super);
        function AssertionError(msg, actual, expected) {
            var _this = _super.call(this, msg) || this;
            _this.actual = actual;
            _this.expected = expected;
            return _this;
        }
        return AssertionError;
    }(Error));
    exports.AssertionError = AssertionError;
    function equal(actual, expected, msg) {
        if (!Util_1.equals(actual, expected)) {
            throw new AssertionError(msg || "Expected: " + expected + " - Actual: " + actual, actual, expected);
        }
    }
    exports.equal = equal;
});
