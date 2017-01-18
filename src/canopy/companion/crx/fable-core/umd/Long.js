(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Symbol"], function (require, exports) {
    "use strict";
    var Symbol_1 = require("./Symbol");
    var Long = (function () {
        function Long(low, high, unsigned) {
            this.eq = this.equals;
            this.neq = this.notEquals;
            this.lt = this.lessThan;
            this.lte = this.lessThanOrEqual;
            this.gt = this.greaterThan;
            this.gte = this.greaterThanOrEqual;
            this.comp = this.compare;
            this.neg = this.negate;
            this.abs = this.absolute;
            this.sub = this.subtract;
            this.mul = this.multiply;
            this.div = this.divide;
            this.mod = this.modulo;
            this.shl = this.shiftLeft;
            this.shr = this.shiftRight;
            this.shru = this.shiftRightUnsigned;
            this.Equals = this.equals;
            this.CompareTo = this.compare;
            this.ToString = this.toString;
            this.low = low | 0;
            this.high = high | 0;
            this.unsigned = !!unsigned;
        }
        Long.prototype.toInt = function () {
            return this.unsigned ? this.low >>> 0 : this.low;
        };
        Long.prototype.toNumber = function () {
            if (this.unsigned)
                return ((this.high >>> 0) * TWO_PWR_32_DBL) + (this.low >>> 0);
            return this.high * TWO_PWR_32_DBL + (this.low >>> 0);
        };
        Long.prototype.toString = function (radix) {
            if (radix === void 0) { radix = 10; }
            radix = radix || 10;
            if (radix < 2 || 36 < radix)
                throw RangeError('radix');
            if (this.isZero())
                return '0';
            if (this.isNegative()) {
                if (this.eq(exports.MIN_VALUE)) {
                    var radixLong = fromNumber(radix), div = this.div(radixLong), rem1 = div.mul(radixLong).sub(this);
                    return div.toString(radix) + rem1.toInt().toString(radix);
                }
                else
                    return '-' + this.neg().toString(radix);
            }
            var radixToPower = fromNumber(pow_dbl(radix, 6), this.unsigned), rem = this;
            var result = '';
            while (true) {
                var remDiv = rem.div(radixToPower), intval = rem.sub(remDiv.mul(radixToPower)).toInt() >>> 0, digits = intval.toString(radix);
                rem = remDiv;
                if (rem.isZero())
                    return digits + result;
                else {
                    while (digits.length < 6)
                        digits = '0' + digits;
                    result = '' + digits + result;
                }
            }
        };
        Long.prototype.getHighBits = function () {
            return this.high;
        };
        Long.prototype.getHighBitsUnsigned = function () {
            return this.high >>> 0;
        };
        Long.prototype.getLowBits = function () {
            return this.low;
        };
        Long.prototype.getLowBitsUnsigned = function () {
            return this.low >>> 0;
        };
        Long.prototype.getNumBitsAbs = function () {
            if (this.isNegative())
                return this.eq(exports.MIN_VALUE) ? 64 : this.neg().getNumBitsAbs();
            var val = this.high != 0 ? this.high : this.low;
            for (var bit = 31; bit > 0; bit--)
                if ((val & (1 << bit)) != 0)
                    break;
            return this.high != 0 ? bit + 33 : bit + 1;
        };
        Long.prototype.isZero = function () {
            return this.high === 0 && this.low === 0;
        };
        Long.prototype.isNegative = function () {
            return !this.unsigned && this.high < 0;
        };
        Long.prototype.isPositive = function () {
            return this.unsigned || this.high >= 0;
        };
        Long.prototype.isOdd = function () {
            return (this.low & 1) === 1;
        };
        Long.prototype.isEven = function () {
            return (this.low & 1) === 0;
        };
        Long.prototype.equals = function (other) {
            if (!isLong(other))
                other = fromValue(other);
            if (this.unsigned !== other.unsigned && (this.high >>> 31) === 1 && (other.high >>> 31) === 1)
                return false;
            return this.high === other.high && this.low === other.low;
        };
        Long.prototype.notEquals = function (other) {
            return !this.eq(other);
        };
        Long.prototype.lessThan = function (other) {
            return this.comp(other) < 0;
        };
        Long.prototype.lessThanOrEqual = function (other) {
            return this.comp(other) <= 0;
        };
        Long.prototype.greaterThan = function (other) {
            return this.comp(other) > 0;
        };
        Long.prototype.greaterThanOrEqual = function (other) {
            return this.comp(other) >= 0;
        };
        Long.prototype.compare = function (other) {
            if (!isLong(other))
                other = fromValue(other);
            if (this.eq(other))
                return 0;
            var thisNeg = this.isNegative(), otherNeg = other.isNegative();
            if (thisNeg && !otherNeg)
                return -1;
            if (!thisNeg && otherNeg)
                return 1;
            if (!this.unsigned)
                return this.sub(other).isNegative() ? -1 : 1;
            return (other.high >>> 0) > (this.high >>> 0) || (other.high === this.high && (other.low >>> 0) > (this.low >>> 0)) ? -1 : 1;
        };
        Long.prototype.negate = function () {
            if (!this.unsigned && this.eq(exports.MIN_VALUE))
                return exports.MIN_VALUE;
            return this.not().add(exports.ONE);
        };
        Long.prototype.absolute = function () {
            if (!this.unsigned && this.isNegative())
                return this.negate();
            else
                return this;
        };
        Long.prototype.add = function (addend) {
            if (!isLong(addend))
                addend = fromValue(addend);
            var a48 = this.high >>> 16;
            var a32 = this.high & 0xFFFF;
            var a16 = this.low >>> 16;
            var a00 = this.low & 0xFFFF;
            var b48 = addend.high >>> 16;
            var b32 = addend.high & 0xFFFF;
            var b16 = addend.low >>> 16;
            var b00 = addend.low & 0xFFFF;
            var c48 = 0, c32 = 0, c16 = 0, c00 = 0;
            c00 += a00 + b00;
            c16 += c00 >>> 16;
            c00 &= 0xFFFF;
            c16 += a16 + b16;
            c32 += c16 >>> 16;
            c16 &= 0xFFFF;
            c32 += a32 + b32;
            c48 += c32 >>> 16;
            c32 &= 0xFFFF;
            c48 += a48 + b48;
            c48 &= 0xFFFF;
            return fromBits((c16 << 16) | c00, (c48 << 16) | c32, this.unsigned);
        };
        Long.prototype.subtract = function (subtrahend) {
            if (!isLong(subtrahend))
                subtrahend = fromValue(subtrahend);
            return this.add(subtrahend.neg());
        };
        Long.prototype.multiply = function (multiplier) {
            if (this.isZero())
                return exports.ZERO;
            if (!isLong(multiplier))
                multiplier = fromValue(multiplier);
            if (multiplier.isZero())
                return exports.ZERO;
            if (this.eq(exports.MIN_VALUE))
                return multiplier.isOdd() ? exports.MIN_VALUE : exports.ZERO;
            if (multiplier.eq(exports.MIN_VALUE))
                return this.isOdd() ? exports.MIN_VALUE : exports.ZERO;
            if (this.isNegative()) {
                if (multiplier.isNegative())
                    return this.neg().mul(multiplier.neg());
                else
                    return this.neg().mul(multiplier).neg();
            }
            else if (multiplier.isNegative())
                return this.mul(multiplier.neg()).neg();
            if (this.lt(TWO_PWR_24) && multiplier.lt(TWO_PWR_24))
                return fromNumber(this.toNumber() * multiplier.toNumber(), this.unsigned);
            var a48 = this.high >>> 16;
            var a32 = this.high & 0xFFFF;
            var a16 = this.low >>> 16;
            var a00 = this.low & 0xFFFF;
            var b48 = multiplier.high >>> 16;
            var b32 = multiplier.high & 0xFFFF;
            var b16 = multiplier.low >>> 16;
            var b00 = multiplier.low & 0xFFFF;
            var c48 = 0, c32 = 0, c16 = 0, c00 = 0;
            c00 += a00 * b00;
            c16 += c00 >>> 16;
            c00 &= 0xFFFF;
            c16 += a16 * b00;
            c32 += c16 >>> 16;
            c16 &= 0xFFFF;
            c16 += a00 * b16;
            c32 += c16 >>> 16;
            c16 &= 0xFFFF;
            c32 += a32 * b00;
            c48 += c32 >>> 16;
            c32 &= 0xFFFF;
            c32 += a16 * b16;
            c48 += c32 >>> 16;
            c32 &= 0xFFFF;
            c32 += a00 * b32;
            c48 += c32 >>> 16;
            c32 &= 0xFFFF;
            c48 += a48 * b00 + a32 * b16 + a16 * b32 + a00 * b48;
            c48 &= 0xFFFF;
            return fromBits((c16 << 16) | c00, (c48 << 16) | c32, this.unsigned);
        };
        Long.prototype.divide = function (divisor) {
            if (!isLong(divisor))
                divisor = fromValue(divisor);
            if (divisor.isZero())
                throw Error('division by zero');
            if (this.isZero())
                return this.unsigned ? exports.UZERO : exports.ZERO;
            var approx = 0, rem = exports.ZERO, res = exports.ZERO;
            if (!this.unsigned) {
                if (this.eq(exports.MIN_VALUE)) {
                    if (divisor.eq(exports.ONE) || divisor.eq(exports.NEG_ONE))
                        return exports.MIN_VALUE;
                    else if (divisor.eq(exports.MIN_VALUE))
                        return exports.ONE;
                    else {
                        var halfThis = this.shr(1);
                        var approx_1 = halfThis.div(divisor).shl(1);
                        if (approx_1.eq(exports.ZERO)) {
                            return divisor.isNegative() ? exports.ONE : exports.NEG_ONE;
                        }
                        else {
                            rem = this.sub(divisor.mul(approx_1));
                            res = approx_1.add(rem.div(divisor));
                            return res;
                        }
                    }
                }
                else if (divisor.eq(exports.MIN_VALUE))
                    return this.unsigned ? exports.UZERO : exports.ZERO;
                if (this.isNegative()) {
                    if (divisor.isNegative())
                        return this.neg().div(divisor.neg());
                    return this.neg().div(divisor).neg();
                }
                else if (divisor.isNegative())
                    return this.div(divisor.neg()).neg();
                res = exports.ZERO;
            }
            else {
                if (!divisor.unsigned)
                    divisor = divisor.toUnsigned();
                if (divisor.gt(this))
                    return exports.UZERO;
                if (divisor.gt(this.shru(1)))
                    return exports.UONE;
                res = exports.UZERO;
            }
            rem = this;
            while (rem.gte(divisor)) {
                approx = Math.max(1, Math.floor(rem.toNumber() / divisor.toNumber()));
                var log2 = Math.ceil(Math.log(approx) / Math.LN2), delta = (log2 <= 48) ? 1 : pow_dbl(2, log2 - 48), approxRes = fromNumber(approx), approxRem = approxRes.mul(divisor);
                while (approxRem.isNegative() || approxRem.gt(rem)) {
                    approx -= delta;
                    approxRes = fromNumber(approx, this.unsigned);
                    approxRem = approxRes.mul(divisor);
                }
                if (approxRes.isZero())
                    approxRes = exports.ONE;
                res = res.add(approxRes);
                rem = rem.sub(approxRem);
            }
            return res;
        };
        Long.prototype.modulo = function (divisor) {
            if (!isLong(divisor))
                divisor = fromValue(divisor);
            return this.sub(this.div(divisor).mul(divisor));
        };
        ;
        Long.prototype.not = function () {
            return fromBits(~this.low, ~this.high, this.unsigned);
        };
        ;
        Long.prototype.and = function (other) {
            if (!isLong(other))
                other = fromValue(other);
            return fromBits(this.low & other.low, this.high & other.high, this.unsigned);
        };
        Long.prototype.or = function (other) {
            if (!isLong(other))
                other = fromValue(other);
            return fromBits(this.low | other.low, this.high | other.high, this.unsigned);
        };
        Long.prototype.xor = function (other) {
            if (!isLong(other))
                other = fromValue(other);
            return fromBits(this.low ^ other.low, this.high ^ other.high, this.unsigned);
        };
        Long.prototype.shiftLeft = function (numBits) {
            if (isLong(numBits))
                numBits = numBits.toInt();
            numBits = numBits & 63;
            if (numBits === 0)
                return this;
            else if (numBits < 32)
                return fromBits(this.low << numBits, (this.high << numBits) | (this.low >>> (32 - numBits)), this.unsigned);
            else
                return fromBits(0, this.low << (numBits - 32), this.unsigned);
        };
        Long.prototype.shiftRight = function (numBits) {
            if (isLong(numBits))
                numBits = numBits.toInt();
            numBits = numBits & 63;
            if (numBits === 0)
                return this;
            else if (numBits < 32)
                return fromBits((this.low >>> numBits) | (this.high << (32 - numBits)), this.high >> numBits, this.unsigned);
            else
                return fromBits(this.high >> (numBits - 32), this.high >= 0 ? 0 : -1, this.unsigned);
        };
        Long.prototype.shiftRightUnsigned = function (numBits) {
            if (isLong(numBits))
                numBits = numBits.toInt();
            numBits = numBits & 63;
            if (numBits === 0)
                return this;
            else {
                var high = this.high;
                if (numBits < 32) {
                    var low = this.low;
                    return fromBits((low >>> numBits) | (high << (32 - numBits)), high >>> numBits, this.unsigned);
                }
                else if (numBits === 32)
                    return fromBits(high, 0, this.unsigned);
                else
                    return fromBits(high >>> (numBits - 32), 0, this.unsigned);
            }
        };
        Long.prototype.toSigned = function () {
            if (!this.unsigned)
                return this;
            return fromBits(this.low, this.high, false);
        };
        Long.prototype.toUnsigned = function () {
            if (this.unsigned)
                return this;
            return fromBits(this.low, this.high, true);
        };
        Long.prototype.toBytes = function (le) {
            return le ? this.toBytesLE() : this.toBytesBE();
        };
        Long.prototype.toBytesLE = function () {
            var hi = this.high, lo = this.low;
            return [
                lo & 0xff,
                (lo >>> 8) & 0xff,
                (lo >>> 16) & 0xff,
                (lo >>> 24) & 0xff,
                hi & 0xff,
                (hi >>> 8) & 0xff,
                (hi >>> 16) & 0xff,
                (hi >>> 24) & 0xff
            ];
        };
        Long.prototype.toBytesBE = function () {
            var hi = this.high, lo = this.low;
            return [
                (hi >>> 24) & 0xff,
                (hi >>> 16) & 0xff,
                (hi >>> 8) & 0xff,
                hi & 0xff,
                (lo >>> 24) & 0xff,
                (lo >>> 16) & 0xff,
                (lo >>> 8) & 0xff,
                lo & 0xff
            ];
        };
        Long.prototype[Symbol_1.default.reflection] = function () {
            return {
                type: "System.Int64",
                interfaces: ["FSharpRecord", "System.IComparable"],
                properties: {
                    low: "number",
                    high: "number",
                    unsigned: "boolean"
                }
            };
        };
        return Long;
    }());
    exports.Long = Long;
    var INT_CACHE = {};
    var UINT_CACHE = {};
    function isLong(obj) {
        return (obj && obj instanceof Long);
    }
    exports.isLong = isLong;
    function fromInt(value, unsigned) {
        if (unsigned === void 0) { unsigned = false; }
        var obj, cachedObj, cache;
        if (unsigned) {
            value >>>= 0;
            if (cache = (0 <= value && value < 256)) {
                cachedObj = UINT_CACHE[value];
                if (cachedObj)
                    return cachedObj;
            }
            obj = fromBits(value, (value | 0) < 0 ? -1 : 0, true);
            if (cache)
                UINT_CACHE[value] = obj;
            return obj;
        }
        else {
            value |= 0;
            if (cache = (-128 <= value && value < 128)) {
                cachedObj = INT_CACHE[value];
                if (cachedObj)
                    return cachedObj;
            }
            obj = fromBits(value, value < 0 ? -1 : 0, false);
            if (cache)
                INT_CACHE[value] = obj;
            return obj;
        }
    }
    exports.fromInt = fromInt;
    function fromNumber(value, unsigned) {
        if (unsigned === void 0) { unsigned = false; }
        if (isNaN(value) || !isFinite(value))
            return unsigned ? exports.UZERO : exports.ZERO;
        if (unsigned) {
            if (value < 0)
                return exports.UZERO;
            if (value >= TWO_PWR_64_DBL)
                return exports.MAX_UNSIGNED_VALUE;
        }
        else {
            if (value <= -TWO_PWR_63_DBL)
                return exports.MIN_VALUE;
            if (value + 1 >= TWO_PWR_63_DBL)
                return exports.MAX_VALUE;
        }
        if (value < 0)
            return fromNumber(-value, unsigned).neg();
        return fromBits((value % TWO_PWR_32_DBL) | 0, (value / TWO_PWR_32_DBL) | 0, unsigned);
    }
    exports.fromNumber = fromNumber;
    function fromBits(lowBits, highBits, unsigned) {
        return new Long(lowBits, highBits, unsigned);
    }
    exports.fromBits = fromBits;
    var pow_dbl = Math.pow;
    function fromString(str, unsigned, radix) {
        if (unsigned === void 0) { unsigned = false; }
        if (radix === void 0) { radix = 10; }
        if (str.length === 0)
            throw Error('empty string');
        if (str === "NaN" || str === "Infinity" || str === "+Infinity" || str === "-Infinity")
            return exports.ZERO;
        if (typeof unsigned === 'number') {
            radix = unsigned,
                unsigned = false;
        }
        else {
            unsigned = !!unsigned;
        }
        radix = radix || 10;
        if (radix < 2 || 36 < radix)
            throw RangeError('radix');
        var p = str.indexOf('-');
        if (p > 0)
            throw Error('interior hyphen');
        else if (p === 0) {
            return fromString(str.substring(1), unsigned, radix).neg();
        }
        var radixToPower = fromNumber(pow_dbl(radix, 8));
        var result = exports.ZERO;
        for (var i = 0; i < str.length; i += 8) {
            var size = Math.min(8, str.length - i), value = parseInt(str.substring(i, i + size), radix);
            if (size < 8) {
                var power = fromNumber(pow_dbl(radix, size));
                result = result.mul(power).add(fromNumber(value));
            }
            else {
                result = result.mul(radixToPower);
                result = result.add(fromNumber(value));
            }
        }
        result.unsigned = unsigned;
        return result;
    }
    exports.fromString = fromString;
    function fromValue(val) {
        if (val instanceof Long)
            return val;
        if (typeof val === 'number')
            return fromNumber(val);
        if (typeof val === 'string')
            return fromString(val);
        return fromBits(val.low, val.high, val.unsigned);
    }
    exports.fromValue = fromValue;
    var TWO_PWR_16_DBL = 1 << 16;
    var TWO_PWR_24_DBL = 1 << 24;
    var TWO_PWR_32_DBL = TWO_PWR_16_DBL * TWO_PWR_16_DBL;
    var TWO_PWR_64_DBL = TWO_PWR_32_DBL * TWO_PWR_32_DBL;
    var TWO_PWR_63_DBL = TWO_PWR_64_DBL / 2;
    var TWO_PWR_24 = fromInt(TWO_PWR_24_DBL);
    exports.ZERO = fromInt(0);
    exports.UZERO = fromInt(0, true);
    exports.ONE = fromInt(1);
    exports.UONE = fromInt(1, true);
    exports.NEG_ONE = fromInt(-1);
    exports.MAX_VALUE = fromBits(0xFFFFFFFF | 0, 0x7FFFFFFF | 0, false);
    exports.MAX_UNSIGNED_VALUE = fromBits(0xFFFFFFFF | 0, 0xFFFFFFFF | 0, true);
    exports.MIN_VALUE = fromBits(0, 0x80000000 | 0, false);
});
