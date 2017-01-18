(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "../Long", "../Seq"], function (require, exports) {
    "use strict";
    var Long_1 = require("../Long");
    var Seq_1 = require("../Seq");
    function pow32(x, n) {
        if (n === 0) {
            return 1;
        }
        else if (n % 2 === 0) {
            return pow32(x * x, ~~(n / 2));
        }
        else {
            return x * pow32(x * x, ~~(n / 2));
        }
    }
    exports.pow32 = pow32;
    function leastBounding2Power(b) {
        var findBounding2Power = function (b_1) { return function (tp) { return function (i) {
            if (b_1 <= tp) {
                return [tp, i];
            }
            else {
                return findBounding2Power(b_1)(tp * 2)(i + 1);
            }
        }; }; };
        return findBounding2Power(b)(1)(0);
    }
    exports.leastBounding2Power = leastBounding2Power;
    exports.k = 27;
    exports.m = 15;
    exports.g = 31;
    exports.w = 440564289;
    exports.primeP = Long_1.fromBits(2013265921, 0, false);
    exports.maxBitsInsideFp = 30;
    exports.p = 2013265921;
    exports.p64 = Long_1.fromBits(2013265921, 0, true);
    function toInt(x) {
        return ~~x;
    }
    exports.toInt = toInt;
    function ofInt32(x) {
        return x >>> 0;
    }
    exports.ofInt32 = ofInt32;
    exports.mzero = 0;
    exports.mone = 1;
    exports.mtwo = 2;
    function mpow(x, n) {
        if (n === 0) {
            return exports.mone;
        }
        else if (n % 2 === 0) {
            return mpow(Long_1.fromNumber(x, true).mul(Long_1.fromNumber(x, true)).mod(exports.p64).toNumber() >>> 0, ~~(n / 2));
        }
        else {
            return Long_1.fromNumber(x, true).mul(Long_1.fromNumber(mpow(Long_1.fromNumber(x, true).mul(Long_1.fromNumber(x, true)).mod(exports.p64).toNumber() >>> 0, ~~(n / 2)), true)).mod(exports.p64).toNumber() >>> 0;
        }
    }
    exports.mpow = mpow;
    function mpowL(x, n) {
        if (n.Equals(Long_1.fromBits(0, 0, false))) {
            return exports.mone;
        }
        else if (n.mod(Long_1.fromBits(2, 0, false)).Equals(Long_1.fromBits(0, 0, false))) {
            return mpowL(Long_1.fromNumber(x, true).mul(Long_1.fromNumber(x, true)).mod(exports.p64).toNumber() >>> 0, n.div(Long_1.fromBits(2, 0, false)));
        }
        else {
            return Long_1.fromNumber(x, true).mul(Long_1.fromNumber(mpowL(Long_1.fromNumber(x, true).mul(Long_1.fromNumber(x, true)).mod(exports.p64).toNumber() >>> 0, n.div(Long_1.fromBits(2, 0, false))), true)).mod(exports.p64).toNumber() >>> 0;
        }
    }
    exports.mpowL = mpowL;
    function m2PowNthRoot(n) {
        return mpow(exports.w >>> 0, pow32(2, exports.k - n));
    }
    exports.m2PowNthRoot = m2PowNthRoot;
    function minv(x) {
        return mpowL(x, exports.primeP.sub(Long_1.fromBits(2, 0, false)));
    }
    exports.minv = minv;
    function computeFFT(lambda, mu, n, w_1, u, res, offset) {
        if (n === 1) {
            res[offset] = u[mu];
        }
        else {
            var halfN = ~~(n / 2);
            var ww = Long_1.fromNumber(w_1, true).mul(Long_1.fromNumber(w_1, true)).mod(exports.p64).toNumber() >>> 0;
            var offsetHalfN = offset + halfN;
            computeFFT(lambda * 2, mu, halfN, ww, u, res, offset);
            computeFFT(lambda * 2, lambda + mu, halfN, ww, u, res, offsetHalfN);
            var wj = exports.mone;
            for (var j = 0; j <= halfN - 1; j++) {
                var even = res[offset + j];
                var odd = res[offsetHalfN + j];
                res[offset + j] = (even + (Long_1.fromNumber(wj, true).mul(Long_1.fromNumber(odd, true)).mod(exports.p64).toNumber() >>> 0)) % exports.p;
                res[offsetHalfN + j] = (even + exports.p - (Long_1.fromNumber(wj, true).mul(Long_1.fromNumber(odd, true)).mod(exports.p64).toNumber() >>> 0)) % exports.p;
                wj = Long_1.fromNumber(w_1, true).mul(Long_1.fromNumber(wj, true)).mod(exports.p64).toNumber() >>> 0;
            }
        }
    }
    exports.computeFFT = computeFFT;
    function computFftInPlace(n, w_1, u) {
        var lambda = 1;
        var mu = 0;
        var res = Uint32Array.from(Seq_1.replicate(n, exports.mzero));
        var offset = 0;
        computeFFT(lambda, mu, n, w_1, u, res, offset);
        return res;
    }
    exports.computFftInPlace = computFftInPlace;
    function computeInverseFftInPlace(n, w_1, uT) {
        var bigKInv = minv(n >>> 0);
        return computFftInPlace(n, minv(w_1), uT).map(function (y) { return Long_1.fromNumber(bigKInv, true).mul(Long_1.fromNumber(y, true)).mod(exports.p64).toNumber() >>> 0; });
    }
    exports.computeInverseFftInPlace = computeInverseFftInPlace;
    exports.maxTwoPower = 29;
    exports.twoPowerTable = Int32Array.from(Seq_1.initialize(exports.maxTwoPower - 1, function (i) { return pow32(2, i); }));
    function computeFftPaddedPolynomialProduct(bigK, k_1, u, v) {
        var w_1 = m2PowNthRoot(k_1);
        var uT = computFftInPlace(bigK, w_1, u);
        var vT = computFftInPlace(bigK, w_1, v);
        var rT = Uint32Array.from(Seq_1.initialize(bigK, function (i) { return Long_1.fromNumber(uT[i], true).mul(Long_1.fromNumber(vT[i], true)).mod(exports.p64).toNumber() >>> 0; }));
        var r = computeInverseFftInPlace(bigK, w_1, rT);
        return r;
    }
    exports.computeFftPaddedPolynomialProduct = computeFftPaddedPolynomialProduct;
    function padTo(n, u) {
        var uBound = u.length;
        return Uint32Array.from(Seq_1.initialize(n, function (i) {
            if (i < uBound) {
                return ofInt32(u[i]);
            }
            else {
                return exports.mzero;
            }
        }));
    }
    exports.padTo = padTo;
    function computeFftPolynomialProduct(degu, u, degv, v) {
        var deguv = degu + degv;
        var bound = deguv + 1;
        var patternInput = leastBounding2Power(bound);
        var w_1 = m2PowNthRoot(patternInput[1]);
        var u_1 = padTo(patternInput[0], u);
        var v_1 = padTo(patternInput[0], v);
        var uT = computFftInPlace(patternInput[0], w_1, u_1);
        var vT = computFftInPlace(patternInput[0], w_1, v_1);
        var rT = Uint32Array.from(Seq_1.initialize(patternInput[0], function (i) { return Long_1.fromNumber(uT[i], true).mul(Long_1.fromNumber(vT[i], true)).mod(exports.p64).toNumber() >>> 0; }));
        var r = computeInverseFftInPlace(patternInput[0], w_1, rT);
        return Int32Array.from(Seq_1.map(function (x) { return toInt(x); }, r));
    }
    exports.computeFftPolynomialProduct = computeFftPolynomialProduct;
    exports.maxFp = (exports.p + exports.p - exports.mone) % exports.p;
});
