(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "../Symbol", "../Symbol", "../Util", "../Long", "../Seq", "./FFT", "../List", "../String"], function (require, exports) {
    "use strict";
    var Symbol_1 = require("../Symbol");
    var Symbol_2 = require("../Symbol");
    var Util_1 = require("../Util");
    var Long_1 = require("../Long");
    var Seq_1 = require("../Seq");
    var FFT_1 = require("./FFT");
    var List_1 = require("../List");
    var String_1 = require("../String");
    var BigNat = (function () {
        function BigNat(bound, digits) {
            this.bound = bound;
            this.digits = digits;
        }
        BigNat.prototype[Symbol_2.default.reflection] = function () {
            return {
                type: "Microsoft.FSharp.Math.BigNat",
                interfaces: ["FSharpRecord"],
                properties: {
                    bound: "number",
                    digits: Util_1.Array(Int32Array, true)
                }
            };
        };
        return BigNat;
    }());
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = BigNat;
    Symbol_1.setType("Microsoft.FSharp.Math.BigNat", BigNat);
    function bound(n) {
        return n.bound;
    }
    exports.bound = bound;
    function setBound(n, v) {
        n.bound = v;
    }
    exports.setBound = setBound;
    function coeff(n, i) {
        return n.digits[i];
    }
    exports.coeff = coeff;
    function coeff64(n, i) {
        return Long_1.fromNumber(coeff(n, i), false);
    }
    exports.coeff64 = coeff64;
    function setCoeff(n, i, v) {
        n.digits[i] = v;
    }
    exports.setCoeff = setCoeff;
    function pow64(x, n) {
        if (n === 0) {
            return Long_1.fromBits(1, 0, false);
        }
        else if (n % 2 === 0) {
            return pow64(x.mul(x), ~~(n / 2));
        }
        else {
            return x.mul(pow64(x.mul(x), ~~(n / 2)));
        }
    }
    exports.pow64 = pow64;
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
    function hash(n) {
        var res = 0;
        for (var i = 0; i <= n.bound - 1; i++) {
            res = n.digits[i] + (res << 3);
        }
        return res;
    }
    exports.hash = hash;
    function maxInt(a, b) {
        if (a < b) {
            return b;
        }
        else {
            return a;
        }
    }
    exports.maxInt = maxInt;
    function minInt(a, b) {
        if (a < b) {
            return a;
        }
        else {
            return b;
        }
    }
    exports.minInt = minInt;
    exports.baseBits = 24;
    exports.baseN = 16777216;
    exports.baseMask = 16777215;
    exports.baseNi64 = Long_1.fromBits(16777216, 0, false);
    exports.baseMaski64 = Long_1.fromBits(16777215, 0, false);
    exports.baseMaskU = Long_1.fromBits(16777215, 0, true);
    exports.baseMask32A = 16777215;
    exports.baseMask32B = 255;
    exports.baseShift32B = 24;
    exports.baseMask64A = 16777215;
    exports.baseMask64B = 16777215;
    exports.baseMask64C = 65535;
    exports.baseShift64B = 24;
    exports.baseShift64C = 48;
    function divbase(x) {
        return ~~(x >>> 0 >> exports.baseBits);
    }
    exports.divbase = divbase;
    function modbase(x) {
        return x & exports.baseMask;
    }
    exports.modbase = modbase;
    function createN(b) {
        return new BigNat(b, new Int32Array(b));
    }
    exports.createN = createN;
    function copyN(x) {
        return new BigNat(x.bound, x.digits.slice());
    }
    exports.copyN = copyN;
    function normN(n) {
        var findLeastBound = function (na) { return function (i) {
            if (i === -1 ? true : na[i] !== 0) {
                return i + 1;
            }
            else {
                return findLeastBound(na)(i - 1);
            }
        }; };
        var bound_1 = findLeastBound(n.digits)(n.bound - 1);
        n.bound = bound_1;
        return n;
    }
    exports.normN = normN;
    exports.boundInt = 2;
    exports.boundInt64 = 3;
    exports.boundBase = 1;
    function embed(x) {
        var x_1 = x < 0 ? 0 : x;
        if (x_1 < exports.baseN) {
            var r = createN(1);
            r.digits[0] = x_1;
            return normN(r);
        }
        else {
            var r = createN(exports.boundInt);
            for (var i = 0; i <= exports.boundInt - 1; i++) {
                r.digits[i] = ~~(x_1 / pow32(exports.baseN, i)) % exports.baseN;
            }
            return normN(r);
        }
    }
    exports.embed = embed;
    function embed64(x) {
        var x_1 = x.CompareTo(Long_1.fromBits(0, 0, false)) < 0 ? Long_1.fromBits(0, 0, false) : x;
        var r = createN(exports.boundInt64);
        for (var i = 0; i <= exports.boundInt64 - 1; i++) {
            r.digits[i] = ~~x_1.div(pow64(exports.baseNi64, i)).mod(exports.baseNi64).toNumber();
        }
        return normN(r);
    }
    exports.embed64 = embed64;
    function eval32(n) {
        if (n.bound === 1) {
            return n.digits[0];
        }
        else {
            var acc = 0;
            for (var i = n.bound - 1; i >= 0; i--) {
                acc = n.digits[i] + exports.baseN * acc;
            }
            return acc;
        }
    }
    exports.eval32 = eval32;
    function eval64(n) {
        if (n.bound === 1) {
            return Long_1.fromNumber(n.digits[0], false);
        }
        else {
            var acc = Long_1.fromBits(0, 0, false);
            for (var i = n.bound - 1; i >= 0; i--) {
                acc = Long_1.fromNumber(n.digits[i], false).add(exports.baseNi64.mul(acc));
            }
            return acc;
        }
    }
    exports.eval64 = eval64;
    exports.one = embed(1);
    exports.zero = embed(0);
    function restrictTo(d, n) {
        return new BigNat(minInt(d, n.bound), n.digits);
    }
    exports.restrictTo = restrictTo;
    function shiftUp(d, n) {
        var m = createN(n.bound + d);
        for (var i = 0; i <= n.bound - 1; i++) {
            m.digits[i + d] = n.digits[i];
        }
        return m;
    }
    exports.shiftUp = shiftUp;
    function shiftDown(d, n) {
        if (n.bound - d <= 0) {
            return exports.zero;
        }
        else {
            var m = createN(n.bound - d);
            for (var i = 0; i <= m.bound - 1; i++) {
                m.digits[i] = n.digits[i + d];
            }
            return m;
        }
    }
    exports.shiftDown = shiftDown;
    function degree(n) {
        return n.bound - 1;
    }
    exports.degree = degree;
    function addP(i, n, c, p, q, r) {
        if (i < n) {
            var x = (function () {
                var $var2 = i;
                var $var1 = p;
                if ($var2 < $var1.bound) {
                    return $var1.digits[$var2];
                }
                else {
                    return 0;
                }
            })() + (function () {
                var $var4 = i;
                var $var3 = q;
                if ($var4 < $var3.bound) {
                    return $var3.digits[$var4];
                }
                else {
                    return 0;
                }
            })() + c;
            r.digits[i] = modbase(x);
            var c_1 = divbase(x);
            addP(i + 1, n, c_1, p, q, r);
        }
    }
    exports.addP = addP;
    function add(p, q) {
        var rbound = 1 + maxInt(p.bound, q.bound);
        var r = createN(rbound);
        var carry = 0;
        addP(0, rbound, carry, p, q, r);
        return normN(r);
    }
    exports.add = add;
    function subP(i, n, c, p, q, r) {
        if (i < n) {
            var x = (function () {
                var $var6 = i;
                var $var5 = p;
                if ($var6 < $var5.bound) {
                    return $var5.digits[$var6];
                }
                else {
                    return 0;
                }
            })() - (function () {
                var $var8 = i;
                var $var7 = q;
                if ($var8 < $var7.bound) {
                    return $var7.digits[$var8];
                }
                else {
                    return 0;
                }
            })() + c;
            if (x > 0) {
                r.digits[i] = modbase(x);
                var c_1 = divbase(x);
                return subP(i + 1, n, c_1, p, q, r);
            }
            else {
                var x_1 = x + exports.baseN;
                r.digits[i] = modbase(x_1);
                var c_1 = divbase(x_1) - 1;
                return subP(i + 1, n, c_1, p, q, r);
            }
        }
        else {
            var underflow = c !== 0;
            return underflow;
        }
    }
    exports.subP = subP;
    function sub(p, q) {
        var rbound = maxInt(p.bound, q.bound);
        var r = createN(rbound);
        var carry = 0;
        var underflow = subP(0, rbound, carry, p, q, r);
        if (underflow) {
            return embed(0);
        }
        else {
            return normN(r);
        }
    }
    exports.sub = sub;
    function isZero(p) {
        return p.bound === 0;
    }
    exports.isZero = isZero;
    function IsZero(p) {
        return isZero(p);
    }
    exports.IsZero = IsZero;
    function isOne(p) {
        if (p.bound === 1) {
            return p.digits[0] === 1;
        }
        else {
            return false;
        }
    }
    exports.isOne = isOne;
    function equal(p, q) {
        if (p.bound === q.bound) {
            var check_1 = function (pa) { return function (qa) { return function (i) {
                if (i === -1) {
                    return true;
                }
                else if (pa[i] === qa[i]) {
                    return check_1(pa)(qa)(i - 1);
                }
                else {
                    return false;
                }
            }; }; };
            return check_1(p.digits)(q.digits)(p.bound - 1);
        }
        else {
            return false;
        }
    }
    exports.equal = equal;
    function shiftCompare(p, pn, q, qn) {
        if (p.bound + pn < q.bound + qn) {
            return -1;
        }
        else if (p.bound + pn > q.bound + pn) {
            return 1;
        }
        else {
            var check_2 = function (pa) { return function (qa) { return function (i) {
                if (i === -1) {
                    return 0;
                }
                else {
                    var pai = i < pn ? 0 : pa[i - pn];
                    var qai = i < qn ? 0 : qa[i - qn];
                    if (pai === qai) {
                        return check_2(pa)(qa)(i - 1);
                    }
                    else if (pai < qai) {
                        return -1;
                    }
                    else {
                        return 1;
                    }
                }
            }; }; };
            return check_2(p.digits)(q.digits)(p.bound + pn - 1);
        }
    }
    exports.shiftCompare = shiftCompare;
    function compare(p, q) {
        if (p.bound < q.bound) {
            return -1;
        }
        else if (p.bound > q.bound) {
            return 1;
        }
        else {
            var check_3 = function (pa) { return function (qa) { return function (i) {
                if (i === -1) {
                    return 0;
                }
                else if (pa[i] === qa[i]) {
                    return check_3(pa)(qa)(i - 1);
                }
                else if (pa[i] < qa[i]) {
                    return -1;
                }
                else {
                    return 1;
                }
            }; }; };
            return check_3(p.digits)(q.digits)(p.bound - 1);
        }
    }
    exports.compare = compare;
    function lt(p, q) {
        return compare(p, q) === -1;
    }
    exports.lt = lt;
    function gt(p, q) {
        return compare(p, q) === 1;
    }
    exports.gt = gt;
    function lte(p, q) {
        return compare(p, q) !== 1;
    }
    exports.lte = lte;
    function gte(p, q) {
        return compare(p, q) !== -1;
    }
    exports.gte = gte;
    function min(a, b) {
        if (lt(a, b)) {
            return a;
        }
        else {
            return b;
        }
    }
    exports.min = min;
    function max(a, b) {
        if (lt(a, b)) {
            return b;
        }
        else {
            return a;
        }
    }
    exports.max = max;
    function contributeArr(a, i, c) {
        var x = Long_1.fromNumber(a[i], false).add(c);
        var c_1 = x.div(exports.baseNi64);
        var x_1 = ~~x.and(exports.baseMaski64).toNumber();
        a[i] = x_1;
        if (c_1.CompareTo(Long_1.fromBits(0, 0, false)) > 0) {
            contributeArr(a, i + 1, c_1);
        }
    }
    exports.contributeArr = contributeArr;
    function scale(k, p) {
        var rbound = p.bound + exports.boundInt;
        var r = createN(rbound);
        var k_1 = Long_1.fromNumber(k, false);
        for (var i = 0; i <= p.bound - 1; i++) {
            var kpi = k_1.mul(Long_1.fromNumber(p.digits[i], false));
            contributeArr(r.digits, i, kpi);
        }
        return normN(r);
    }
    exports.scale = scale;
    function mulSchoolBookBothSmall(p, q) {
        var r = createN(2);
        var rak = Long_1.fromNumber(p, false).mul(Long_1.fromNumber(q, false));
        setCoeff(r, 0, ~~rak.and(exports.baseMaski64).toNumber());
        setCoeff(r, 1, ~~rak.div(exports.baseNi64).toNumber());
        return normN(r);
    }
    exports.mulSchoolBookBothSmall = mulSchoolBookBothSmall;
    function mulSchoolBookCarry(r, c, k) {
        if (c.CompareTo(Long_1.fromBits(0, 0, false)) > 0) {
            var rak = coeff64(r, k).add(c);
            setCoeff(r, k, ~~rak.and(exports.baseMaski64).toNumber());
            mulSchoolBookCarry(r, rak.div(exports.baseNi64), k + 1);
        }
    }
    exports.mulSchoolBookCarry = mulSchoolBookCarry;
    function mulSchoolBookOneSmall(p, q) {
        var bp = bound(p);
        var rbound = bp + 1;
        var r = createN(rbound);
        var q_1 = Long_1.fromNumber(q, false);
        var c = Long_1.fromBits(0, 0, false);
        for (var i = 0; i <= bp - 1; i++) {
            var rak = c.add(coeff64(r, i)).add(coeff64(p, i).mul(q_1));
            setCoeff(r, i, ~~rak.and(exports.baseMaski64).toNumber());
            c = rak.div(exports.baseNi64);
        }
        mulSchoolBookCarry(r, c, bp);
        return normN(r);
    }
    exports.mulSchoolBookOneSmall = mulSchoolBookOneSmall;
    function mulSchoolBookNeitherSmall(p, q) {
        var rbound = p.bound + q.bound;
        var r = createN(rbound);
        for (var i = 0; i <= p.bound - 1; i++) {
            var pai = Long_1.fromNumber(p.digits[i], false);
            var c = Long_1.fromBits(0, 0, false);
            var k = i;
            for (var j = 0; j <= q.bound - 1; j++) {
                var qaj = Long_1.fromNumber(q.digits[j], false);
                var rak = Long_1.fromNumber(r.digits[k], false).add(c).add(pai.mul(qaj));
                r.digits[k] = ~~rak.and(exports.baseMaski64).toNumber();
                c = rak.div(exports.baseNi64);
                k = k + 1;
            }
            mulSchoolBookCarry(r, c, k);
        }
        return normN(r);
    }
    exports.mulSchoolBookNeitherSmall = mulSchoolBookNeitherSmall;
    function mulSchoolBook(p, q) {
        var pSmall = bound(p) === 1;
        var qSmall = bound(q) === 1;
        if (pSmall ? qSmall : false) {
            return mulSchoolBookBothSmall(coeff(p, 0), coeff(q, 0));
        }
        else if (pSmall) {
            return mulSchoolBookOneSmall(q, coeff(p, 0));
        }
        else if (qSmall) {
            return mulSchoolBookOneSmall(p, coeff(q, 0));
        }
        else {
            return mulSchoolBookNeitherSmall(p, q);
        }
    }
    exports.mulSchoolBook = mulSchoolBook;
    var encoding = (function () {
        function encoding(bigL, twoToBigL, k, bigK, bigN, split, splits) {
            this.bigL = bigL;
            this.twoToBigL = twoToBigL;
            this.k = k;
            this.bigK = bigK;
            this.bigN = bigN;
            this.split = split;
            this.splits = splits;
        }
        encoding.prototype[Symbol_2.default.reflection] = function () {
            return {
                type: "Microsoft.FSharp.Math.BigNatModule.encoding",
                interfaces: ["FSharpRecord"],
                properties: {
                    bigL: "number",
                    twoToBigL: "number",
                    k: "number",
                    bigK: "number",
                    bigN: "number",
                    split: "number",
                    splits: Int32Array
                }
            };
        };
        return encoding;
    }());
    exports.encoding = encoding;
    Symbol_1.setType("Microsoft.FSharp.Math.BigNatModule.encoding", encoding);
    function mkEncoding(bigL, k, bigK, bigN) {
        return new encoding(bigL, pow32(2, bigL), k, bigK, bigN, ~~(exports.baseBits / bigL), Int32Array.from(Seq_1.initialize(~~(exports.baseBits / bigL), function (i) { return pow32(2, bigL * i); })));
    }
    exports.mkEncoding = mkEncoding;
    exports.table = [mkEncoding(1, 28, 268435456, 268435456), mkEncoding(2, 26, 67108864, 134217728), mkEncoding(3, 24, 16777216, 50331648), mkEncoding(4, 22, 4194304, 16777216), mkEncoding(5, 20, 1048576, 5242880), mkEncoding(6, 18, 262144, 1572864), mkEncoding(7, 16, 65536, 458752), mkEncoding(8, 14, 16384, 131072), mkEncoding(9, 12, 4096, 36864), mkEncoding(10, 10, 1024, 10240), mkEncoding(11, 8, 256, 2816), mkEncoding(12, 6, 64, 768), mkEncoding(13, 4, 16, 208)];
    function calculateTableTow(bigL) {
        var k = FFT_1.maxBitsInsideFp - 2 * bigL;
        var bigK = pow64(Long_1.fromBits(2, 0, false), k);
        var N = bigK.mul(Long_1.fromNumber(bigL, false));
        return [bigL, k, bigK, N];
    }
    exports.calculateTableTow = calculateTableTow;
    function encodingGivenResultBits(bitsRes) {
        var selectFrom = function (i) {
            if (i + 1 < exports.table.length ? bitsRes < exports.table[i + 1].bigN : false) {
                return selectFrom(i + 1);
            }
            else {
                return exports.table[i];
            }
        };
        if (bitsRes >= exports.table[0].bigN) {
            throw new Error("Product is huge, around 268435456 bits, beyond quickmul");
        }
        else {
            return selectFrom(0);
        }
    }
    exports.encodingGivenResultBits = encodingGivenResultBits;
    exports.bitmask = Int32Array.from(Seq_1.initialize(exports.baseBits, function (i) { return pow32(2, i) - 1; }));
    exports.twopowers = Int32Array.from(Seq_1.initialize(exports.baseBits, function (i) { return pow32(2, i); }));
    exports.twopowersI64 = Array.from(Seq_1.initialize(exports.baseBits, function (i) { return pow64(Long_1.fromBits(2, 0, false), i); }));
    function wordBits(word) {
        var hi = function (k) {
            if (k === 0) {
                return 0;
            }
            else if ((word & exports.twopowers[k - 1]) !== 0) {
                return k;
            }
            else {
                return hi(k - 1);
            }
        };
        return hi(exports.baseBits);
    }
    exports.wordBits = wordBits;
    function bits(u) {
        if (u.bound === 0) {
            return 0;
        }
        else {
            return degree(u) * exports.baseBits + wordBits(u.digits[degree(u)]);
        }
    }
    exports.bits = bits;
    function extractBits(n, enc, bi) {
        var bj = bi + enc.bigL - 1;
        var biw = ~~(bi / exports.baseBits);
        var bjw = ~~(bj / exports.baseBits);
        if (biw !== bjw) {
            var x = (function () {
                var $var10 = biw;
                var $var9 = n;
                if ($var10 < $var9.bound) {
                    return $var9.digits[$var10];
                }
                else {
                    return 0;
                }
            })();
            var y = (function () {
                var $var12 = bjw;
                var $var11 = n;
                if ($var12 < $var11.bound) {
                    return $var11.digits[$var12];
                }
                else {
                    return 0;
                }
            })();
            var xbit = bi % exports.baseBits;
            var nxbits = exports.baseBits - xbit;
            var x_1 = x >> xbit;
            var y_1 = y << nxbits;
            var x_2 = x_1 | y_1;
            var x_3 = x_2 & exports.bitmask[enc.bigL];
            return x_3;
        }
        else {
            var x = (function () {
                var $var14 = biw;
                var $var13 = n;
                if ($var14 < $var13.bound) {
                    return $var13.digits[$var14];
                }
                else {
                    return 0;
                }
            })();
            var xbit = bi % exports.baseBits;
            var x_1 = x >> xbit;
            var x_2 = x_1 & exports.bitmask[enc.bigL];
            return x_2;
        }
    }
    exports.extractBits = extractBits;
    function encodePoly(enc, n) {
        var poly = Uint32Array.from(Seq_1.replicate(enc.bigK, FFT_1.ofInt32(0)));
        var biMax = n.bound * exports.baseBits;
        var encoder = function (i) { return function (bi) {
            if (i === enc.bigK ? true : bi > biMax) { }
            else {
                var pi = extractBits(n, enc, bi);
                poly[i] = FFT_1.ofInt32(pi);
                var i_1 = i + 1;
                var bi_1 = bi + enc.bigL;
                encoder(i_1)(bi_1);
            }
        }; };
        encoder(0)(0);
        return poly;
    }
    exports.encodePoly = encodePoly;
    function decodeResultBits(enc, poly) {
        var n = 0;
        for (var i = 0; i <= poly.length - 1; i++) {
            if (poly[i] !== FFT_1.mzero) {
                n = i;
            }
        }
        var rbits = FFT_1.maxBitsInsideFp + enc.bigL * n + 1;
        return rbits + 1;
    }
    exports.decodeResultBits = decodeResultBits;
    function decodePoly(enc, poly) {
        var rbound = ~~(decodeResultBits(enc, poly) / exports.baseBits) + 1;
        var r = createN(rbound);
        var evaluate = function (i) { return function (j) { return function (d) {
            if (i === enc.bigK) { }
            else {
                if (j >= rbound) { }
                else {
                    var x = Long_1.fromNumber(FFT_1.toInt(poly[i]), false).mul(exports.twopowersI64[d]);
                    contributeArr(r.digits, j, x);
                }
                var i_1 = i + 1;
                var d_1 = d + enc.bigL;
                var patternInput = d_1 >= exports.baseBits ? [j + 1, d_1 - exports.baseBits] : [j, d_1];
                evaluate(i_1)(patternInput[0])(patternInput[1]);
            }
        }; }; };
        evaluate(0)(0)(0);
        return normN(r);
    }
    exports.decodePoly = decodePoly;
    function quickMulUsingFft(u, v) {
        var bitsRes = bits(u) + bits(v);
        var enc = encodingGivenResultBits(bitsRes);
        var upoly = encodePoly(enc, u);
        var vpoly = encodePoly(enc, v);
        var rpoly = FFT_1.computeFftPaddedPolynomialProduct(enc.bigK, enc.k, upoly, vpoly);
        var r = decodePoly(enc, rpoly);
        return normN(r);
    }
    exports.quickMulUsingFft = quickMulUsingFft;
    exports.minDigitsKaratsuba = 16;
    function recMulKaratsuba(mul, p, q) {
        var bp = p.bound;
        var bq = q.bound;
        var bmax = maxInt(bp, bq);
        if (bmax > exports.minDigitsKaratsuba) {
            var k = ~~(bmax / 2);
            var a0 = restrictTo(k, p);
            var a1 = shiftDown(k, p);
            var b0 = restrictTo(k, q);
            var b1 = shiftDown(k, q);
            var q0 = mul(a0)(b0);
            var q1 = mul(add(a0, a1))(add(b0, b1));
            var q2 = mul(a1)(b1);
            var p1 = sub(q1, add(q0, q2));
            var r = add(q0, shiftUp(k, add(p1, shiftUp(k, q2))));
            return r;
        }
        else {
            return mulSchoolBook(p, q);
        }
    }
    exports.recMulKaratsuba = recMulKaratsuba;
    function mulKaratsuba(x, y) {
        return recMulKaratsuba(function (x_1) { return function (y_1) { return mulKaratsuba(x_1, y_1); }; }, x, y);
    }
    exports.mulKaratsuba = mulKaratsuba;
    exports.productDigitsUpperSchoolBook = ~~(64000 / exports.baseBits);
    exports.singleDigitForceSchoolBook = ~~(32000 / exports.baseBits);
    exports.productDigitsUpperFft = ~~(exports.table[0].bigN / exports.baseBits);
    function mul(p, q) {
        return mulSchoolBook(p, q);
    }
    exports.mul = mul;
    function scaleSubInPlace(x, f, a, n) {
        var invariant = function (tupledArg) { };
        var patternInput = [x.digits, degree(x)];
        var patternInput_1 = [a.digits, degree(a)];
        var f_1 = Long_1.fromNumber(f, false);
        var j = 0;
        var z = f_1.mul(Long_1.fromNumber(patternInput_1[0][0], false));
        while (z.CompareTo(Long_1.fromBits(0, 0, false)) > 0 ? true : j < patternInput_1[1]) {
            if (j > patternInput[1]) {
                throw new Error("scaleSubInPlace: pre-condition did not apply, result would be -ve");
            }
            invariant([z, j, n]);
            var zLo = ~~z.and(exports.baseMaski64).toNumber();
            var zHi = z.div(exports.baseNi64);
            if (zLo <= patternInput[0][j + n]) {
                patternInput[0][j + n] = patternInput[0][j + n] - zLo;
            }
            else {
                patternInput[0][j + n] = patternInput[0][j + n] + (exports.baseN - zLo);
                zHi = zHi.add(Long_1.fromBits(1, 0, false));
            }
            if (j < patternInput_1[1]) {
                z = zHi.add(f_1.mul(Long_1.fromNumber(patternInput_1[0][j + 1], false)));
            }
            else {
                z = zHi;
            }
            j = j + 1;
        }
        normN(x);
    }
    exports.scaleSubInPlace = scaleSubInPlace;
    function scaleSub(x, f, a, n) {
        var freshx = add(x, exports.zero);
        scaleSubInPlace(freshx, f, a, n);
        return normN(freshx);
    }
    exports.scaleSub = scaleSub;
    function scaleAddInPlace(x, f, a, n) {
        var invariant = function (tupledArg) { };
        var patternInput = [x.digits, degree(x)];
        var patternInput_1 = [a.digits, degree(a)];
        var f_1 = Long_1.fromNumber(f, false);
        var j = 0;
        var z = f_1.mul(Long_1.fromNumber(patternInput_1[0][0], false));
        while (z.CompareTo(Long_1.fromBits(0, 0, false)) > 0 ? true : j < patternInput_1[1]) {
            if (j > patternInput[1]) {
                throw new Error("scaleSubInPlace: pre-condition did not apply, result would be -ve");
            }
            invariant([z, j, n]);
            var zLo = ~~z.and(exports.baseMaski64).toNumber();
            var zHi = z.div(exports.baseNi64);
            if (zLo < exports.baseN - patternInput[0][j + n]) {
                patternInput[0][j + n] = patternInput[0][j + n] + zLo;
            }
            else {
                patternInput[0][j + n] = zLo - (exports.baseN - patternInput[0][j + n]);
                zHi = zHi.add(Long_1.fromBits(1, 0, false));
            }
            if (j < patternInput_1[1]) {
                z = zHi.add(f_1.mul(Long_1.fromNumber(patternInput_1[0][j + 1], false)));
            }
            else {
                z = zHi;
            }
            j = j + 1;
        }
        normN(x);
    }
    exports.scaleAddInPlace = scaleAddInPlace;
    function scaleAdd(x, f, a, n) {
        var freshx = add(x, exports.zero);
        scaleAddInPlace(freshx, f, a, n);
        return normN(freshx);
    }
    exports.scaleAdd = scaleAdd;
    function removeFactor(x, a, n) {
        var patternInput = [degree(a), degree(x)];
        if (patternInput[1] < patternInput[0] + n) {
            return 0;
        }
        else {
            var patternInput_1 = [a.digits, x.digits];
            var f = patternInput[0] === 0
                ? patternInput[1] === n
                    ? ~~(patternInput_1[1][n] / patternInput_1[0][0])
                    : Long_1.fromNumber(patternInput_1[1][patternInput[1]], false).mul(exports.baseNi64).add(Long_1.fromNumber(patternInput_1[1][patternInput[1] - 1], false)).div(Long_1.fromNumber(patternInput_1[0][0], false)).toNumber()
                : patternInput[1] === patternInput[0] + n
                    ? ~~(patternInput_1[1][patternInput[1]] / (patternInput_1[0][patternInput[0]] + 1))
                    : Long_1.fromNumber(patternInput_1[1][patternInput[1]], false).mul(exports.baseNi64).add(Long_1.fromNumber(patternInput_1[1][patternInput[1] - 1], false)).div(Long_1.fromNumber(patternInput_1[0][patternInput[0]], false).add(Long_1.fromBits(1, 0, false))).toNumber();
            if (f === 0) {
                var lte_1 = shiftCompare(a, n, x, 0) !== 1;
                if (lte_1) {
                    return 1;
                }
                else {
                    return 0;
                }
            }
            else {
                return f;
            }
        }
    }
    exports.removeFactor = removeFactor;
    function divmod(b, a) {
        if (isZero(a)) {
            throw new Error();
        }
        else if (degree(b) < degree(a)) {
            return [exports.zero, b];
        }
        else {
            var x = copyN(b);
            var d = createN(degree(b) - degree(a) + 1 + 1);
            var p = degree(b);
            var m = degree(a);
            var n = p - m;
            var Invariant = function (tupledArg) { };
            var finished = false;
            while (!finished) {
                Invariant([d, x, n, p]);
                var f = removeFactor(x, a, n);
                if (f > 0) {
                    scaleSubInPlace(x, f, a, n);
                    scaleAddInPlace(d, f, exports.one, n);
                    Invariant([d, x, n, p]);
                }
                else {
                    if (f === 0) {
                        finished = n === 0;
                    }
                    else {
                        finished = false;
                    }
                    if (!finished) {
                        if (p === m + n) {
                            Invariant([d, x, n - 1, p]);
                            n = n - 1;
                        }
                        else {
                            Invariant([d, x, n - 1, p - 1]);
                            n = n - 1;
                            p = p - 1;
                        }
                    }
                }
            }
            return [normN(d), normN(x)];
        }
    }
    exports.divmod = divmod;
    function div(b, a) {
        return divmod(b, a)[0];
    }
    exports.div = div;
    function rem(b, a) {
        return divmod(b, a)[1];
    }
    exports.rem = rem;
    function bitAnd(a, b) {
        var rbound = minInt(a.bound, b.bound);
        var r = createN(rbound);
        for (var i = 0; i <= r.bound - 1; i++) {
            r.digits[i] = a.digits[i] & b.digits[i];
        }
        return normN(r);
    }
    exports.bitAnd = bitAnd;
    function bitOr(a, b) {
        var rbound = maxInt(a.bound, b.bound);
        var r = createN(rbound);
        for (var i = 0; i <= a.bound - 1; i++) {
            r.digits[i] = r.digits[i] | a.digits[i];
        }
        for (var i = 0; i <= b.bound - 1; i++) {
            r.digits[i] = r.digits[i] | b.digits[i];
        }
        return normN(r);
    }
    exports.bitOr = bitOr;
    function hcf(a, b) {
        var hcfloop = function (a_1) { return function (b_1) {
            if (equal(exports.zero, a_1)) {
                return b_1;
            }
            else {
                var patternInput = divmod(b_1, a_1);
                return hcfloop(patternInput[1])(a_1);
            }
        }; };
        if (lt(a, b)) {
            return hcfloop(a)(b);
        }
        else {
            return hcfloop(b)(a);
        }
    }
    exports.hcf = hcf;
    exports.two = embed(2);
    function powi(x, n) {
        var power = function (acc) { return function (x_1) { return function (n_1) {
            if (n_1 === 0) {
                return acc;
            }
            else if (n_1 % 2 === 0) {
                return power(acc)(mul(x_1, x_1))(~~(n_1 / 2));
            }
            else {
                return power(mul(x_1, acc))(mul(x_1, x_1))(~~(n_1 / 2));
            }
        }; }; };
        return power(exports.one)(x)(n);
    }
    exports.powi = powi;
    function pow(x, n) {
        var power = function (acc) { return function (x_1) { return function (n_1) {
            if (isZero(n_1)) {
                return acc;
            }
            else {
                var patternInput = divmod(n_1, exports.two);
                if (isZero(patternInput[1])) {
                    return power(acc)(mul(x_1, x_1))(patternInput[0]);
                }
                else {
                    return power(mul(x_1, acc))(mul(x_1, x_1))(patternInput[0]);
                }
            }
        }; }; };
        return power(exports.one)(x)(n);
    }
    exports.pow = pow;
    function toFloat(n) {
        var basef = exports.baseN;
        var evalFloat = function (acc) { return function (k) { return function (i) {
            if (i === n.bound) {
                return acc;
            }
            else {
                return evalFloat(acc + k * n.digits[i])(k * basef)(i + 1);
            }
        }; }; };
        return evalFloat(0)(1)(0);
    }
    exports.toFloat = toFloat;
    function ofInt32(n) {
        return embed(n);
    }
    exports.ofInt32 = ofInt32;
    function ofInt64(n) {
        return embed64(n);
    }
    exports.ofInt64 = ofInt64;
    function toUInt32(n) {
        var matchValue = n.bound;
        var $var15 = null;
        switch (matchValue) {
            case 0:
                {
                    $var15 = 0;
                    break;
                }
            case 1:
                {
                    $var15 = n.digits[0] >>> 0;
                    break;
                }
            case 2:
                {
                    {
                        var patternInput = [n.digits[0], n.digits[1]];
                        if (patternInput[1] > exports.baseMask32B) {
                            throw new Error();
                        }
                        $var15 = ((patternInput[0] & exports.baseMask32A) >>> 0) + ((patternInput[1] & exports.baseMask32B) >>> 0 << exports.baseShift32B);
                    }
                    break;
                }
            default:
                {
                    throw new Error();
                }
        }
        return $var15;
    }
    exports.toUInt32 = toUInt32;
    function toUInt64(n) {
        var matchValue = n.bound;
        var $var16 = null;
        switch (matchValue) {
            case 0:
                {
                    $var16 = Long_1.fromBits(0, 0, true);
                    break;
                }
            case 1:
                {
                    $var16 = Long_1.fromNumber(n.digits[0], true);
                    break;
                }
            case 2:
                {
                    {
                        var patternInput = [n.digits[0], n.digits[1]];
                        $var16 = Long_1.fromNumber(patternInput[0] & exports.baseMask64A, true).add(Long_1.fromNumber(patternInput[1] & exports.baseMask64B, true).shl(exports.baseShift64B));
                    }
                    break;
                }
            case 3:
                {
                    {
                        var patternInput = [n.digits[0], n.digits[1], n.digits[2]];
                        if (patternInput[2] > exports.baseMask64C) {
                            throw new Error();
                        }
                        $var16 = Long_1.fromNumber(patternInput[0] & exports.baseMask64A, true).add(Long_1.fromNumber(patternInput[1] & exports.baseMask64B, true).shl(exports.baseShift64B)).add(Long_1.fromNumber(patternInput[2] & exports.baseMask64C, true).shl(exports.baseShift64C));
                    }
                    break;
                }
            default:
                {
                    throw new Error();
                }
        }
        return $var16;
    }
    exports.toUInt64 = toUInt64;
    function toString(n) {
        var degn = degree(n);
        var route = function (prior) { return function (k) { return function (ten2k) {
            if (degree(ten2k) > degn) {
                return new List_1.default([k, ten2k], prior);
            }
            else {
                return route(new List_1.default([k, ten2k], prior))(k + 1)(mul(ten2k, ten2k));
            }
        }; }; };
        var kten2ks = route(new List_1.default())(0)(embed(10));
        var collect = function (isLeading) { return function (digits) { return function (n_1) { return function (_arg1) {
            if (_arg1.tail != null) {
                var ten2k = _arg1.head[1];
                var patternInput = divmod(n_1, ten2k);
                if (isLeading ? isZero(patternInput[0]) : false) {
                    var digits_1 = collect(isLeading)(digits)(patternInput[1])(_arg1.tail);
                    return digits_1;
                }
                else {
                    var digits_1 = collect(false)(digits)(patternInput[1])(_arg1.tail);
                    var digits_2 = collect(isLeading)(digits_1)(patternInput[0])(_arg1.tail);
                    return digits_2;
                }
            }
            else {
                var n_2 = eval32(n_1);
                if (isLeading ? n_2 === 0 : false) {
                    return digits;
                }
                else {
                    return new List_1.default(String(n_2), digits);
                }
            }
        }; }; }; };
        var digits = collect(true)(new List_1.default())(n)(kten2ks);
        if (digits.tail == null) {
            return "0";
        }
        else {
            return String_1.join.apply(void 0, [""].concat(Array.from(digits)));
        }
    }
    exports.toString = toString;
    function ofString(str) {
        var len = str.length;
        if (String_1.isNullOrEmpty(str)) {
            throw new Error("empty string" + '\nParameter name: ' + "str");
        }
        var ten = embed(10);
        var build = function (acc) { return function (i) {
            if (i === len) {
                return acc;
            }
            else {
                var c = str[i];
                var d = c.charCodeAt(0) - "0".charCodeAt(0);
                if (0 <= d ? d <= 9 : false) {
                    return build(add(mul(ten, acc), embed(d)))(i + 1);
                }
                else {
                    throw new Error();
                }
            }
        }; };
        return build(embed(0))(0);
    }
    exports.ofString = ofString;
    function isSmall(n) {
        return n.bound <= 1;
    }
    exports.isSmall = isSmall;
    function getSmall(n) {
        var $var18 = 0;
        var $var17 = n;
        if ($var18 < $var17.bound) {
            return $var17.digits[$var18];
        }
        else {
            return 0;
        }
    }
    exports.getSmall = getSmall;
    function factorial(n) {
        var productR = function (a) { return function (b) {
            if (equal(a, b)) {
                return a;
            }
            else {
                var m = div(add(a, b), ofInt32(2));
                return mul(productR(a)(m), productR(add(m, exports.one))(b));
            }
        }; };
        return productR(exports.one)(n);
    }
    exports.factorial = factorial;
});
