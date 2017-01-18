(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Symbol", "./Util", "./Util"], function (require, exports) {
    "use strict";
    var Symbol_1 = require("./Symbol");
    var Util_1 = require("./Util");
    var Util_2 = require("./Util");
    function choice1Of2(v) {
        return new Choice("Choice1Of2", [v]);
    }
    exports.choice1Of2 = choice1Of2;
    function choice2Of2(v) {
        return new Choice("Choice2Of2", [v]);
    }
    exports.choice2Of2 = choice2Of2;
    var Choice = (function () {
        function Choice(t, d) {
            this.Case = t;
            this.Fields = d;
        }
        Object.defineProperty(Choice.prototype, "valueIfChoice1", {
            get: function () {
                return this.Case === "Choice1Of2" ? this.Fields[0] : null;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Choice.prototype, "valueIfChoice2", {
            get: function () {
                return this.Case === "Choice2Of2" ? this.Fields[0] : null;
            },
            enumerable: true,
            configurable: true
        });
        Choice.prototype.Equals = function (other) {
            return Util_1.equalsUnions(this, other);
        };
        Choice.prototype.CompareTo = function (other) {
            return Util_2.compareUnions(this, other);
        };
        Choice.prototype[Symbol_1.default.reflection] = function () {
            return {
                type: "Microsoft.FSharp.Core.FSharpChoice",
                interfaces: ["FSharpUnion", "System.IEquatable", "System.IComparable"]
            };
        };
        return Choice;
    }());
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = Choice;
});
