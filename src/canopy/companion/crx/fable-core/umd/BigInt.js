(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Symbol", "./Symbol", "./Util", "./BigInt/BigNat", "./BigInt/BigNat", "./Seq", "./Long", "./String"], function (require, exports) {
    "use strict";
    var Symbol_1 = require("./Symbol");
    var Symbol_2 = require("./Symbol");
    var Util_1 = require("./Util");
    var BigNat_1 = require("./BigInt/BigNat");
    var BigNat_2 = require("./BigInt/BigNat");
    var Seq_1 = require("./Seq");
    var Long_1 = require("./Long");
    var String_1 = require("./String");
    var BigInteger = (function () {
        function BigInteger(signInt, v) {
            this.signInt = signInt;
            this.v = v;
        }
        BigInteger.prototype[Symbol_2.default.reflection] = function () {
            return {
                type: "System.Numerics.BigInteger",
                interfaces: ["FSharpRecord", "System.IComparable"],
                properties: {
                    signInt: "number",
                    v: BigNat_2.default
                }
            };
        };
        Object.defineProperty(BigInteger.prototype, "Sign", {
            get: function () {
                if (this.IsZero) {
                    return 0;
                }
                else {
                    return this.signInt;
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(BigInteger.prototype, "SignInt", {
            get: function () {
                return this.signInt;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(BigInteger.prototype, "V", {
            get: function () {
                return this.v;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(BigInteger.prototype, "IsZero", {
            get: function () {
                if (this.SignInt === 0) {
                    return true;
                }
                else {
                    return BigNat_1.isZero(this.V);
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(BigInteger.prototype, "IsOne", {
            get: function () {
                if (this.SignInt === 1) {
                    return BigNat_1.isOne(this.V);
                }
                else {
                    return false;
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(BigInteger.prototype, "StructuredDisplayString", {
            get: function () {
                return Util_1.toString(this);
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(BigInteger.prototype, "IsSmall", {
            get: function () {
                if (this.IsZero) {
                    return true;
                }
                else {
                    return BigNat_1.isSmall(this.V);
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(BigInteger.prototype, "IsNegative", {
            get: function () {
                if (this.SignInt === -1) {
                    return !this.IsZero;
                }
                else {
                    return false;
                }
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(BigInteger.prototype, "IsPositive", {
            get: function () {
                if (this.SignInt === 1) {
                    return !this.IsZero;
                }
                else {
                    return false;
                }
            },
            enumerable: true,
            configurable: true
        });
        BigInteger.prototype.CompareTo = function (obj) {
            if (obj instanceof BigInteger) {
                var that = obj;
                return compare(this, that);
            }
            else {
                throw new Error("the objects are not comparable" + '\nParameter name: ' + "obj");
            }
        };
        BigInteger.prototype.ToString = function () {
            var matchValue = this.SignInt;
            var $var19 = null;
            switch (matchValue) {
                case 1:
                    {
                        $var19 = BigNat_1.toString(this.V);
                        break;
                    }
                case -1:
                    {
                        if (BigNat_1.isZero(this.V)) {
                            $var19 = "0";
                        }
                        else {
                            $var19 = "-" + BigNat_1.toString(this.V);
                        }
                        break;
                    }
                case 0:
                    {
                        $var19 = "0";
                        break;
                    }
                default:
                    {
                        throw new Error("signs should be +/- 1 or 0");
                    }
            }
            return $var19;
        };
        BigInteger.prototype.Equals = function (obj) {
            if (obj instanceof BigInteger) {
                var that = obj;
                return op_Equality(this, that);
            }
            else {
                return false;
            }
        };
        BigInteger.prototype.GetHashCode = function () {
            return hash(this);
        };
        return BigInteger;
    }());
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = BigInteger;
    Symbol_1.setType("System.Numerics.BigInteger", BigInteger);
    var smallLim = 4096;
    var smallPosTab = Array.from(Seq_1.initialize(smallLim, function (n) { return BigNat_1.ofInt32(n); }));
    exports.one = fromInt32(1);
    exports.two = fromInt32(2);
    exports.zero = fromInt32(0);
    function fromInt32(n) {
        if (n >= 0) {
            return new BigInteger(1, nat(BigNat_1.ofInt32(n)));
        }
        else if (n === -2147483648) {
            return new BigInteger(-1, nat(BigNat_1.ofInt64(Long_1.fromNumber(n, false).neg())));
        }
        else {
            return new BigInteger(-1, nat(BigNat_1.ofInt32(-n)));
        }
    }
    exports.fromInt32 = fromInt32;
    function fromInt64(n) {
        if (n.CompareTo(Long_1.fromBits(0, 0, false)) >= 0) {
            return new BigInteger(1, nat(BigNat_1.ofInt64(n)));
        }
        else if (n.Equals(Long_1.fromBits(0, 2147483648, false))) {
            return new BigInteger(-1, nat(BigNat_1.add(BigNat_1.ofInt64(Long_1.fromBits(4294967295, 2147483647, false)), BigNat_1.one)));
        }
        else {
            return new BigInteger(-1, nat(BigNat_1.ofInt64(n.neg())));
        }
    }
    exports.fromInt64 = fromInt64;
    function nat(n) {
        if (BigNat_1.isSmall(n) ? BigNat_1.getSmall(n) < smallLim : false) {
            return smallPosTab[BigNat_1.getSmall(n)];
        }
        else {
            return n;
        }
    }
    exports.nat = nat;
    function create(s, n) {
        return new BigInteger(s, nat(n));
    }
    exports.create = create;
    function posn(n) {
        return new BigInteger(1, nat(n));
    }
    exports.posn = posn;
    function negn(n) {
        return new BigInteger(-1, nat(n));
    }
    exports.negn = negn;
    function op_Equality(x, y) {
        var matchValue = [x.SignInt, y.SignInt];
        var _target9 = function () {
            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
        };
        if (matchValue[0] === -1) {
            if (matchValue[1] === -1) {
                return BigNat_1.equal(x.V, y.V);
            }
            else if (matchValue[1] === 0) {
                return BigNat_1.isZero(x.V);
            }
            else if (matchValue[1] === 1) {
                if (BigNat_1.isZero(x.V)) {
                    return BigNat_1.isZero(y.V);
                }
                else {
                    return false;
                }
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 0) {
            if (matchValue[1] === -1) {
                return BigNat_1.isZero(y.V);
            }
            else if (matchValue[1] === 0) {
                return true;
            }
            else if (matchValue[1] === 1) {
                return BigNat_1.isZero(y.V);
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 1) {
            if (matchValue[1] === -1) {
                if (BigNat_1.isZero(x.V)) {
                    return BigNat_1.isZero(y.V);
                }
                else {
                    return false;
                }
            }
            else if (matchValue[1] === 0) {
                return BigNat_1.isZero(x.V);
            }
            else if (matchValue[1] === 1) {
                return BigNat_1.equal(x.V, y.V);
            }
            else {
                return _target9();
            }
        }
        else {
            return _target9();
        }
    }
    exports.op_Equality = op_Equality;
    function op_Inequality(x, y) {
        return !op_Equality(x, y);
    }
    exports.op_Inequality = op_Inequality;
    function op_LessThan(x, y) {
        var matchValue = [x.SignInt, y.SignInt];
        var _target9 = function () {
            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
        };
        if (matchValue[0] === -1) {
            if (matchValue[1] === -1) {
                return BigNat_1.lt(y.V, x.V);
            }
            else if (matchValue[1] === 0) {
                return !BigNat_1.isZero(x.V);
            }
            else if (matchValue[1] === 1) {
                if (!BigNat_1.isZero(x.V)) {
                    return true;
                }
                else {
                    return !BigNat_1.isZero(y.V);
                }
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 0) {
            if (matchValue[1] === -1) {
                return false;
            }
            else if (matchValue[1] === 0) {
                return false;
            }
            else if (matchValue[1] === 1) {
                return !BigNat_1.isZero(y.V);
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 1) {
            if (matchValue[1] === -1) {
                return false;
            }
            else if (matchValue[1] === 0) {
                return false;
            }
            else if (matchValue[1] === 1) {
                return BigNat_1.lt(x.V, y.V);
            }
            else {
                return _target9();
            }
        }
        else {
            return _target9();
        }
    }
    exports.op_LessThan = op_LessThan;
    function op_GreaterThan(x, y) {
        var matchValue = [x.SignInt, y.SignInt];
        var _target9 = function () {
            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
        };
        if (matchValue[0] === -1) {
            if (matchValue[1] === -1) {
                return BigNat_1.gt(y.V, x.V);
            }
            else if (matchValue[1] === 0) {
                return false;
            }
            else if (matchValue[1] === 1) {
                return false;
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 0) {
            if (matchValue[1] === -1) {
                return !BigNat_1.isZero(y.V);
            }
            else if (matchValue[1] === 0) {
                return false;
            }
            else if (matchValue[1] === 1) {
                return false;
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 1) {
            if (matchValue[1] === -1) {
                if (!BigNat_1.isZero(x.V)) {
                    return true;
                }
                else {
                    return !BigNat_1.isZero(y.V);
                }
            }
            else if (matchValue[1] === 0) {
                return !BigNat_1.isZero(x.V);
            }
            else if (matchValue[1] === 1) {
                return BigNat_1.gt(x.V, y.V);
            }
            else {
                return _target9();
            }
        }
        else {
            return _target9();
        }
    }
    exports.op_GreaterThan = op_GreaterThan;
    function compare(n, nn) {
        if (op_LessThan(n, nn)) {
            return -1;
        }
        else if (op_Equality(n, nn)) {
            return 0;
        }
        else {
            return 1;
        }
    }
    exports.compare = compare;
    function hash(z) {
        if (z.SignInt === 0) {
            return 1;
        }
        else {
            return z.SignInt + hash(z.V);
        }
    }
    exports.hash = hash;
    function op_UnaryNegation(z) {
        var matchValue = z.SignInt;
        if (matchValue === 0) {
            return exports.zero;
        }
        else {
            return create(-matchValue, z.V);
        }
    }
    exports.op_UnaryNegation = op_UnaryNegation;
    function scale(k, z) {
        if (z.SignInt === 0) {
            return exports.zero;
        }
        else if (k < 0) {
            return create(-z.SignInt, BigNat_1.scale(-k, z.V));
        }
        else {
            return create(z.SignInt, BigNat_1.scale(k, z.V));
        }
    }
    exports.scale = scale;
    function subnn(nx, ny) {
        if (BigNat_1.gte(nx, ny)) {
            return posn(BigNat_1.sub(nx, ny));
        }
        else {
            return negn(BigNat_1.sub(ny, nx));
        }
    }
    exports.subnn = subnn;
    function addnn(nx, ny) {
        return posn(BigNat_1.add(nx, ny));
    }
    exports.addnn = addnn;
    function op_Addition(x, y) {
        if (y.IsZero) {
            return x;
        }
        else if (x.IsZero) {
            return y;
        }
        else {
            var matchValue = [x.SignInt, y.SignInt];
            var _target4 = function () {
                throw new Error("signs should be +/- 1" + '\nParameter name: ' + "x");
            };
            if (matchValue[0] === -1) {
                if (matchValue[1] === -1) {
                    return op_UnaryNegation(addnn(x.V, y.V));
                }
                else if (matchValue[1] === 1) {
                    return subnn(y.V, x.V);
                }
                else {
                    return _target4();
                }
            }
            else if (matchValue[0] === 1) {
                if (matchValue[1] === -1) {
                    return subnn(x.V, y.V);
                }
                else if (matchValue[1] === 1) {
                    return addnn(x.V, y.V);
                }
                else {
                    return _target4();
                }
            }
            else {
                return _target4();
            }
        }
    }
    exports.op_Addition = op_Addition;
    function op_Subtraction(x, y) {
        if (y.IsZero) {
            return x;
        }
        else if (x.IsZero) {
            return op_UnaryNegation(y);
        }
        else {
            var matchValue = [x.SignInt, y.SignInt];
            var _target4 = function () {
                throw new Error("signs should be +/- 1" + '\nParameter name: ' + "x");
            };
            if (matchValue[0] === -1) {
                if (matchValue[1] === -1) {
                    return subnn(y.V, x.V);
                }
                else if (matchValue[1] === 1) {
                    return op_UnaryNegation(addnn(x.V, y.V));
                }
                else {
                    return _target4();
                }
            }
            else if (matchValue[0] === 1) {
                if (matchValue[1] === -1) {
                    return addnn(x.V, y.V);
                }
                else if (matchValue[1] === 1) {
                    return subnn(x.V, y.V);
                }
                else {
                    return _target4();
                }
            }
            else {
                return _target4();
            }
        }
    }
    exports.op_Subtraction = op_Subtraction;
    function op_Multiply(x, y) {
        if (x.IsZero) {
            return x;
        }
        else if (y.IsZero) {
            return y;
        }
        else if (x.IsOne) {
            return y;
        }
        else if (y.IsOne) {
            return x;
        }
        else {
            var m = BigNat_1.mul(x.V, y.V);
            return create(x.SignInt * y.SignInt, m);
        }
    }
    exports.op_Multiply = op_Multiply;
    function divRem(x, y) {
        if (y.IsZero) {
            throw new Error();
        }
        if (x.IsZero) {
            return [exports.zero, exports.zero];
        }
        else {
            var patternInput = BigNat_1.divmod(x.V, y.V);
            var matchValue = [x.SignInt, y.SignInt];
            var _target4 = function () {
                throw new Error("signs should be +/- 1" + '\nParameter name: ' + "x");
            };
            if (matchValue[0] === -1) {
                if (matchValue[1] === -1) {
                    return [posn(patternInput[0]), negn(patternInput[1])];
                }
                else if (matchValue[1] === 1) {
                    return [negn(patternInput[0]), negn(patternInput[1])];
                }
                else {
                    return _target4();
                }
            }
            else if (matchValue[0] === 1) {
                if (matchValue[1] === -1) {
                    return [negn(patternInput[0]), posn(patternInput[1])];
                }
                else if (matchValue[1] === 1) {
                    return [posn(patternInput[0]), posn(patternInput[1])];
                }
                else {
                    return _target4();
                }
            }
            else {
                return _target4();
            }
        }
    }
    exports.divRem = divRem;
    function op_Division(x, y) {
        return divRem(x, y)[0];
    }
    exports.op_Division = op_Division;
    function op_Modulus(x, y) {
        return divRem(x, y)[1];
    }
    exports.op_Modulus = op_Modulus;
    function op_RightShift(x, y) {
        return op_Division(x, pow(exports.two, y));
    }
    exports.op_RightShift = op_RightShift;
    function op_LeftShift(x, y) {
        return op_Multiply(x, pow(exports.two, y));
    }
    exports.op_LeftShift = op_LeftShift;
    function op_BitwiseAnd(x, y) {
        return posn(BigNat_1.bitAnd(x.V, y.V));
    }
    exports.op_BitwiseAnd = op_BitwiseAnd;
    function op_BitwiseOr(x, y) {
        return posn(BigNat_1.bitOr(x.V, y.V));
    }
    exports.op_BitwiseOr = op_BitwiseOr;
    function greatestCommonDivisor(x, y) {
        var matchValue = [x.SignInt, y.SignInt];
        if (matchValue[0] === 0) {
            if (matchValue[1] === 0) {
                return exports.zero;
            }
            else {
                return posn(y.V);
            }
        }
        else if (matchValue[1] === 0) {
            return posn(x.V);
        }
        else {
            return posn(BigNat_1.hcf(x.V, y.V));
        }
    }
    exports.greatestCommonDivisor = greatestCommonDivisor;
    function abs(x) {
        if (x.SignInt === -1) {
            return op_UnaryNegation(x);
        }
        else {
            return x;
        }
    }
    exports.abs = abs;
    function op_LessThanOrEqual(x, y) {
        var matchValue = [x.SignInt, y.SignInt];
        var _target9 = function () {
            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
        };
        if (matchValue[0] === -1) {
            if (matchValue[1] === -1) {
                return BigNat_1.lte(y.V, x.V);
            }
            else if (matchValue[1] === 0) {
                return true;
            }
            else if (matchValue[1] === 1) {
                return true;
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 0) {
            if (matchValue[1] === -1) {
                return BigNat_1.isZero(y.V);
            }
            else if (matchValue[1] === 0) {
                return true;
            }
            else if (matchValue[1] === 1) {
                return true;
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 1) {
            if (matchValue[1] === -1) {
                if (BigNat_1.isZero(x.V)) {
                    return BigNat_1.isZero(y.V);
                }
                else {
                    return false;
                }
            }
            else if (matchValue[1] === 0) {
                return BigNat_1.isZero(x.V);
            }
            else if (matchValue[1] === 1) {
                return BigNat_1.lte(x.V, y.V);
            }
            else {
                return _target9();
            }
        }
        else {
            return _target9();
        }
    }
    exports.op_LessThanOrEqual = op_LessThanOrEqual;
    function op_GreaterThanOrEqual(x, y) {
        var matchValue = [x.SignInt, y.SignInt];
        var _target9 = function () {
            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
        };
        if (matchValue[0] === -1) {
            if (matchValue[1] === -1) {
                return BigNat_1.gte(y.V, x.V);
            }
            else if (matchValue[1] === 0) {
                return BigNat_1.isZero(x.V);
            }
            else if (matchValue[1] === 1) {
                if (BigNat_1.isZero(x.V)) {
                    return BigNat_1.isZero(y.V);
                }
                else {
                    return false;
                }
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 0) {
            if (matchValue[1] === -1) {
                return true;
            }
            else if (matchValue[1] === 0) {
                return true;
            }
            else if (matchValue[1] === 1) {
                return BigNat_1.isZero(y.V);
            }
            else {
                return _target9();
            }
        }
        else if (matchValue[0] === 1) {
            if (matchValue[1] === -1) {
                return true;
            }
            else if (matchValue[1] === 0) {
                return true;
            }
            else if (matchValue[1] === 1) {
                return BigNat_1.gte(x.V, y.V);
            }
            else {
                return _target9();
            }
        }
        else {
            return _target9();
        }
    }
    exports.op_GreaterThanOrEqual = op_GreaterThanOrEqual;
    function op_Explicit_0(x) {
        if (x.IsZero) {
            return 0;
        }
        else {
            var u = BigNat_1.toUInt32(x.V);
            if (u <= 2147483647 >>> 0) {
                return x.SignInt * ~~u;
            }
            else if (x.SignInt === -1 ? u === 2147483647 + 1 >>> 0 : false) {
                return -2147483648;
            }
            else {
                throw new Error();
            }
        }
    }
    exports.op_Explicit_0 = op_Explicit_0;
    function op_Explicit_0(x) {
        if (x.IsZero) {
            return 0;
        }
        else {
            return BigNat_1.toUInt32(x.V);
        }
    }
    exports.op_Explicit_0 = op_Explicit_0;
    function op_Explicit_0(x) {
        if (x.IsZero) {
            return Long_1.fromBits(0, 0, false);
        }
        else {
            var u = BigNat_1.toUInt64(x.V);
            if (u.CompareTo(Long_1.fromNumber(Long_1.fromBits(4294967295, 2147483647, false).toNumber(), true)) <= 0) {
                return Long_1.fromNumber(x.SignInt, false).mul(Long_1.fromNumber(u.toNumber(), false));
            }
            else if (x.SignInt === -1 ? u.Equals(Long_1.fromNumber(Long_1.fromBits(4294967295, 2147483647, false).add(Long_1.fromBits(1, 0, false)).toNumber(), true)) : false) {
                return Long_1.fromBits(0, 2147483648, false);
            }
            else {
                throw new Error();
            }
        }
    }
    exports.op_Explicit_0 = op_Explicit_0;
    function op_Explicit_0(x) {
        if (x.IsZero) {
            return Long_1.fromBits(0, 0, true);
        }
        else {
            return BigNat_1.toUInt64(x.V);
        }
    }
    exports.op_Explicit_0 = op_Explicit_0;
    function op_Explicit_0(x) {
        var matchValue = x.SignInt;
        var $var20 = null;
        switch (matchValue) {
            case 1:
                {
                    $var20 = BigNat_1.toFloat(x.V);
                    break;
                }
            case -1:
                {
                    $var20 = -BigNat_1.toFloat(x.V);
                    break;
                }
            case 0:
                {
                    $var20 = 0;
                    break;
                }
            default:
                {
                    throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
                }
        }
        return $var20;
    }
    exports.op_Explicit_0 = op_Explicit_0;
    function parse(text) {
        if (text == null) {
            throw new Error("text");
        }
        var text_1 = String_1.trim(text, "both");
        var len = text_1.length;
        if (len === 0) {
            throw new Error();
        }
        var matchValue = [text_1[0], len];
        if (matchValue[0] === "+") {
            if (matchValue[1] === 1) {
                throw new Error();
            }
            else {
                return posn(BigNat_1.ofString(text_1.slice(1, len - 1 + 1)));
            }
        }
        else if (matchValue[0] === "-") {
            if (matchValue[1] === 1) {
                throw new Error();
            }
            else {
                return negn(BigNat_1.ofString(text_1.slice(1, len - 1 + 1)));
            }
        }
        else {
            return posn(BigNat_1.ofString(text_1));
        }
    }
    exports.parse = parse;
    function factorial(x) {
        if (x.IsNegative) {
            throw new Error("mustBeNonNegative" + '\nParameter name: ' + "x");
        }
        if (x.IsPositive) {
            return posn(BigNat_1.factorial(x.V));
        }
        else {
            return exports.one;
        }
    }
    exports.factorial = factorial;
    function op_UnaryPlus(n1) {
        return n1;
    }
    exports.op_UnaryPlus = op_UnaryPlus;
    function pow(x, y) {
        if (y < 0) {
            throw new Error("y");
        }
        var matchValue = [x.IsZero, y];
        if (matchValue[0]) {
            if (matchValue[1] === 0) {
                return exports.one;
            }
            else {
                return exports.zero;
            }
        }
        else {
            var yval = fromInt32(y);
            return create(BigNat_1.isZero(BigNat_1.rem(yval.V, BigNat_1.two)) ? 1 : x.SignInt, BigNat_1.pow(x.V, yval.V));
        }
    }
    exports.pow = pow;
});
