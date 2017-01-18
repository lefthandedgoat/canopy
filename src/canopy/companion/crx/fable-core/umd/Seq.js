(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Util", "./Util", "./Array", "./ListClass"], function (require, exports) {
    "use strict";
    var Util_1 = require("./Util");
    var Util_2 = require("./Util");
    var Array_1 = require("./Array");
    var ListClass_1 = require("./ListClass");
    var Enumerator = (function () {
        function Enumerator(iter) {
            this.iter = iter;
        }
        Enumerator.prototype.MoveNext = function () {
            var cur = this.iter.next();
            this.current = cur.value;
            return !cur.done;
        };
        Object.defineProperty(Enumerator.prototype, "Current", {
            get: function () {
                return this.current;
            },
            enumerable: true,
            configurable: true
        });
        Enumerator.prototype.Reset = function () {
            throw new Error("JS iterators cannot be reset");
        };
        Enumerator.prototype.Dispose = function () { };
        return Enumerator;
    }());
    exports.Enumerator = Enumerator;
    function getEnumerator(o) {
        return typeof o.GetEnumerator === "function"
            ? o.GetEnumerator() : new Enumerator(o[Symbol.iterator]());
    }
    exports.getEnumerator = getEnumerator;
    function toIterator(en) {
        return {
            next: function () {
                return en.MoveNext()
                    ? { done: false, value: en.Current }
                    : { done: true, value: null };
            }
        };
    }
    exports.toIterator = toIterator;
    function __failIfNone(res) {
        if (res == null)
            throw new Error("Seq did not contain any matching element");
        return res;
    }
    function toList(xs) {
        return foldBack(function (x, acc) {
            return new ListClass_1.default(x, acc);
        }, xs, new ListClass_1.default());
    }
    exports.toList = toList;
    function ofList(xs) {
        return delay(function () { return unfold(function (x) { return x.tail != null ? [x.head, x.tail] : null; }, xs); });
    }
    exports.ofList = ofList;
    function ofArray(xs) {
        return delay(function () { return unfold(function (i) { return i < xs.length ? [xs[i], i + 1] : null; }, 0); });
    }
    exports.ofArray = ofArray;
    function append(xs, ys) {
        return delay(function () {
            var firstDone = false;
            var i = xs[Symbol.iterator]();
            var iters = [i, null];
            return unfold(function () {
                var cur;
                if (!firstDone) {
                    cur = iters[0].next();
                    if (!cur.done) {
                        return [cur.value, iters];
                    }
                    else {
                        firstDone = true;
                        iters = [null, ys[Symbol.iterator]()];
                    }
                }
                cur = iters[1].next();
                return !cur.done ? [cur.value, iters] : null;
            }, iters);
        });
    }
    exports.append = append;
    function average(xs) {
        var count = 1;
        var sum = reduce(function (acc, x) {
            count++;
            return acc + x;
        }, xs);
        return sum / count;
    }
    exports.average = average;
    function averageBy(f, xs) {
        var count = 1;
        var sum = reduce(function (acc, x) {
            count++;
            return (count === 2 ? f(acc) : acc) + f(x);
        }, xs);
        return sum / count;
    }
    exports.averageBy = averageBy;
    function concat(xs) {
        return delay(function () {
            var iter = xs[Symbol.iterator]();
            var output = null;
            return unfold(function (innerIter) {
                var hasFinished = false;
                while (!hasFinished) {
                    if (innerIter == null) {
                        var cur = iter.next();
                        if (!cur.done) {
                            innerIter = cur.value[Symbol.iterator]();
                        }
                        else {
                            hasFinished = true;
                        }
                    }
                    else {
                        var cur = innerIter.next();
                        if (!cur.done) {
                            output = cur.value;
                            hasFinished = true;
                        }
                        else {
                            innerIter = null;
                        }
                    }
                }
                return innerIter != null && output != null ? [output, innerIter] : null;
            }, null);
        });
    }
    exports.concat = concat;
    function collect(f, xs) {
        return concat(map(f, xs));
    }
    exports.collect = collect;
    function choose(f, xs) {
        var trySkipToNext = function (iter) {
            var cur = iter.next();
            if (!cur.done) {
                var y = f(cur.value);
                return y != null ? [y, iter] : trySkipToNext(iter);
            }
            return void 0;
        };
        return delay(function () {
            return unfold(function (iter) {
                return trySkipToNext(iter);
            }, xs[Symbol.iterator]());
        });
    }
    exports.choose = choose;
    function compareWith(f, xs, ys) {
        var nonZero = tryFind(function (i) { return i != 0; }, map2(function (x, y) { return f(x, y); }, xs, ys));
        return nonZero != null ? nonZero : count(xs) - count(ys);
    }
    exports.compareWith = compareWith;
    function delay(f) {
        return _a = {},
            _a[Symbol.iterator] = function () { return f()[Symbol.iterator](); },
            _a;
        var _a;
    }
    exports.delay = delay;
    function empty() {
        return unfold(function () { return void 0; });
    }
    exports.empty = empty;
    function enumerateWhile(cond, xs) {
        return concat(unfold(function () { return cond() ? [xs, true] : null; }));
    }
    exports.enumerateWhile = enumerateWhile;
    function enumerateThenFinally(xs, finalFn) {
        return delay(function () {
            var iter;
            try {
                iter = xs[Symbol.iterator]();
            }
            catch (err) {
                return void 0;
            }
            finally {
                finalFn();
            }
            return unfold(function (iter) {
                try {
                    var cur = iter.next();
                    return !cur.done ? [cur.value, iter] : null;
                }
                catch (err) {
                    return void 0;
                }
                finally {
                    finalFn();
                }
            }, iter);
        });
    }
    exports.enumerateThenFinally = enumerateThenFinally;
    function enumerateUsing(disp, work) {
        var isDisposed = false;
        var disposeOnce = function () {
            if (!isDisposed) {
                isDisposed = true;
                disp.Dispose();
            }
        };
        try {
            return enumerateThenFinally(work(disp), disposeOnce);
        }
        catch (err) {
            return void 0;
        }
        finally {
            disposeOnce();
        }
    }
    exports.enumerateUsing = enumerateUsing;
    function exactlyOne(xs) {
        var iter = xs[Symbol.iterator]();
        var fst = iter.next();
        if (fst.done)
            throw new Error("Seq was empty");
        var snd = iter.next();
        if (!snd.done)
            throw new Error("Seq had multiple items");
        return fst.value;
    }
    exports.exactlyOne = exactlyOne;
    function except(itemsToExclude, source) {
        var exclusionItems = Array.from(itemsToExclude);
        var testIsNotInExclusionItems = function (element) { return !exclusionItems.some(function (excludedItem) { return Util_1.equals(excludedItem, element); }); };
        return filter(testIsNotInExclusionItems, source);
    }
    exports.except = except;
    function exists(f, xs) {
        function aux(iter) {
            var cur = iter.next();
            return !cur.done && (f(cur.value) || aux(iter));
        }
        return aux(xs[Symbol.iterator]());
    }
    exports.exists = exists;
    function exists2(f, xs, ys) {
        function aux(iter1, iter2) {
            var cur1 = iter1.next(), cur2 = iter2.next();
            return !cur1.done && !cur2.done && (f(cur1.value, cur2.value) || aux(iter1, iter2));
        }
        return aux(xs[Symbol.iterator](), ys[Symbol.iterator]());
    }
    exports.exists2 = exists2;
    function filter(f, xs) {
        function trySkipToNext(iter) {
            var cur = iter.next();
            while (!cur.done) {
                if (f(cur.value)) {
                    return [cur.value, iter];
                }
                cur = iter.next();
            }
            return void 0;
        }
        return delay(function () { return unfold(trySkipToNext, xs[Symbol.iterator]()); });
    }
    exports.filter = filter;
    function where(f, xs) {
        return filter(f, xs);
    }
    exports.where = where;
    function fold(f, acc, xs) {
        if (Array.isArray(xs) || ArrayBuffer.isView(xs)) {
            return xs.reduce(f, acc);
        }
        else {
            var cur = void 0;
            for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
                cur = iter.next();
                if (cur.done)
                    break;
                acc = f(acc, cur.value, i);
            }
            return acc;
        }
    }
    exports.fold = fold;
    function foldBack(f, xs, acc) {
        var arr = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs : Array.from(xs);
        for (var i = arr.length - 1; i >= 0; i--) {
            acc = f(arr[i], acc, i);
        }
        return acc;
    }
    exports.foldBack = foldBack;
    function fold2(f, acc, xs, ys) {
        var iter1 = xs[Symbol.iterator](), iter2 = ys[Symbol.iterator]();
        var cur1, cur2;
        for (var i = 0;; i++) {
            cur1 = iter1.next();
            cur2 = iter2.next();
            if (cur1.done || cur2.done) {
                break;
            }
            acc = f(acc, cur1.value, cur2.value, i);
        }
        return acc;
    }
    exports.fold2 = fold2;
    function foldBack2(f, xs, ys, acc) {
        var ar1 = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs : Array.from(xs);
        var ar2 = Array.isArray(ys) || ArrayBuffer.isView(ys) ? ys : Array.from(ys);
        for (var i = ar1.length - 1; i >= 0; i--) {
            acc = f(ar1[i], ar2[i], acc, i);
        }
        return acc;
    }
    exports.foldBack2 = foldBack2;
    function forAll(f, xs) {
        return fold(function (acc, x) { return acc && f(x); }, true, xs);
    }
    exports.forAll = forAll;
    function forAll2(f, xs, ys) {
        return fold2(function (acc, x, y) { return acc && f(x, y); }, true, xs, ys);
    }
    exports.forAll2 = forAll2;
    function tryHead(xs) {
        var iter = xs[Symbol.iterator]();
        var cur = iter.next();
        return cur.done ? null : cur.value;
    }
    exports.tryHead = tryHead;
    function head(xs) {
        return __failIfNone(tryHead(xs));
    }
    exports.head = head;
    function initialize(n, f) {
        return delay(function () {
            return unfold(function (i) { return i < n ? [f(i), i + 1] : null; }, 0);
        });
    }
    exports.initialize = initialize;
    function initializeInfinite(f) {
        return delay(function () {
            return unfold(function (i) { return [f(i), i + 1]; }, 0);
        });
    }
    exports.initializeInfinite = initializeInfinite;
    function tryItem(i, xs) {
        if (i < 0)
            return null;
        if (Array.isArray(xs) || ArrayBuffer.isView(xs))
            return i < xs.length ? xs[i] : null;
        for (var j = 0, iter = xs[Symbol.iterator]();; j++) {
            var cur = iter.next();
            if (cur.done)
                return null;
            if (j === i)
                return cur.value;
        }
    }
    exports.tryItem = tryItem;
    function item(i, xs) {
        return __failIfNone(tryItem(i, xs));
    }
    exports.item = item;
    function iterate(f, xs) {
        fold(function (_, x) { return f(x); }, null, xs);
    }
    exports.iterate = iterate;
    function iterate2(f, xs, ys) {
        fold2(function (_, x, y) { return f(x, y); }, null, xs, ys);
    }
    exports.iterate2 = iterate2;
    function iterateIndexed(f, xs) {
        fold(function (_, x, i) { return f(i, x); }, null, xs);
    }
    exports.iterateIndexed = iterateIndexed;
    function iterateIndexed2(f, xs, ys) {
        fold2(function (_, x, y, i) { return f(i, x, y); }, null, xs, ys);
    }
    exports.iterateIndexed2 = iterateIndexed2;
    function isEmpty(xs) {
        var i = xs[Symbol.iterator]();
        return i.next().done;
    }
    exports.isEmpty = isEmpty;
    function tryLast(xs) {
        try {
            return reduce(function (_, x) { return x; }, xs);
        }
        catch (err) {
            return null;
        }
    }
    exports.tryLast = tryLast;
    function last(xs) {
        return __failIfNone(tryLast(xs));
    }
    exports.last = last;
    function count(xs) {
        return Array.isArray(xs) || ArrayBuffer.isView(xs)
            ? xs.length
            : fold(function (acc, x) { return acc + 1; }, 0, xs);
    }
    exports.count = count;
    function map(f, xs) {
        return delay(function () { return unfold(function (iter) {
            var cur = iter.next();
            return !cur.done ? [f(cur.value), iter] : null;
        }, xs[Symbol.iterator]()); });
    }
    exports.map = map;
    function mapIndexed(f, xs) {
        return delay(function () {
            var i = 0;
            return unfold(function (iter) {
                var cur = iter.next();
                return !cur.done ? [f(i++, cur.value), iter] : null;
            }, xs[Symbol.iterator]());
        });
    }
    exports.mapIndexed = mapIndexed;
    function map2(f, xs, ys) {
        return delay(function () {
            var iter1 = xs[Symbol.iterator]();
            var iter2 = ys[Symbol.iterator]();
            return unfold(function () {
                var cur1 = iter1.next(), cur2 = iter2.next();
                return !cur1.done && !cur2.done ? [f(cur1.value, cur2.value), null] : null;
            });
        });
    }
    exports.map2 = map2;
    function mapIndexed2(f, xs, ys) {
        return delay(function () {
            var i = 0;
            var iter1 = xs[Symbol.iterator]();
            var iter2 = ys[Symbol.iterator]();
            return unfold(function () {
                var cur1 = iter1.next(), cur2 = iter2.next();
                return !cur1.done && !cur2.done ? [f(i++, cur1.value, cur2.value), null] : null;
            });
        });
    }
    exports.mapIndexed2 = mapIndexed2;
    function map3(f, xs, ys, zs) {
        return delay(function () {
            var iter1 = xs[Symbol.iterator]();
            var iter2 = ys[Symbol.iterator]();
            var iter3 = zs[Symbol.iterator]();
            return unfold(function () {
                var cur1 = iter1.next(), cur2 = iter2.next(), cur3 = iter3.next();
                return !cur1.done && !cur2.done && !cur3.done ? [f(cur1.value, cur2.value, cur3.value), null] : null;
            });
        });
    }
    exports.map3 = map3;
    function mapFold(f, acc, xs) {
        var result = [];
        var r;
        var cur;
        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
            cur = iter.next();
            if (cur.done)
                break;
            _a = f(acc, cur.value), r = _a[0], acc = _a[1];
            result.push(r);
        }
        return [result, acc];
        var _a;
    }
    exports.mapFold = mapFold;
    function mapFoldBack(f, xs, acc) {
        var arr = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs : Array.from(xs);
        var result = [];
        var r;
        for (var i = arr.length - 1; i >= 0; i--) {
            _a = f(arr[i], acc), r = _a[0], acc = _a[1];
            result.push(r);
        }
        return [result, acc];
        var _a;
    }
    exports.mapFoldBack = mapFoldBack;
    function max(xs) {
        return reduce(function (acc, x) { return Util_2.compare(acc, x) === 1 ? acc : x; }, xs);
    }
    exports.max = max;
    function maxBy(f, xs) {
        return reduce(function (acc, x) { return Util_2.compare(f(acc), f(x)) === 1 ? acc : x; }, xs);
    }
    exports.maxBy = maxBy;
    function min(xs) {
        return reduce(function (acc, x) { return Util_2.compare(acc, x) === -1 ? acc : x; }, xs);
    }
    exports.min = min;
    function minBy(f, xs) {
        return reduce(function (acc, x) { return Util_2.compare(f(acc), f(x)) === -1 ? acc : x; }, xs);
    }
    exports.minBy = minBy;
    function pairwise(xs) {
        return skip(2, scan(function (last, next) { return [last[1], next]; }, [0, 0], xs));
    }
    exports.pairwise = pairwise;
    function permute(f, xs) {
        return ofArray(Array_1.permute(f, Array.from(xs)));
    }
    exports.permute = permute;
    function rangeStep(first, step, last) {
        if (step === 0)
            throw new Error("Step cannot be 0");
        return delay(function () { return unfold(function (x) { return step > 0 && x <= last || step < 0 && x >= last ? [x, x + step] : null; }, first); });
    }
    exports.rangeStep = rangeStep;
    function rangeChar(first, last) {
        return delay(function () { return unfold(function (x) { return x <= last ? [x, String.fromCharCode(x.charCodeAt(0) + 1)] : null; }, first); });
    }
    exports.rangeChar = rangeChar;
    function range(first, last) {
        return rangeStep(first, 1, last);
    }
    exports.range = range;
    function readOnly(xs) {
        return map(function (x) { return x; }, xs);
    }
    exports.readOnly = readOnly;
    function reduce(f, xs) {
        if (Array.isArray(xs) || ArrayBuffer.isView(xs))
            return xs.reduce(f);
        var iter = xs[Symbol.iterator]();
        var cur = iter.next();
        if (cur.done)
            throw new Error("Seq was empty");
        var acc = cur.value;
        for (;;) {
            cur = iter.next();
            if (cur.done)
                break;
            acc = f(acc, cur.value);
        }
        return acc;
    }
    exports.reduce = reduce;
    function reduceBack(f, xs) {
        var ar = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs : Array.from(xs);
        if (ar.length === 0)
            throw new Error("Seq was empty");
        var acc = ar[ar.length - 1];
        for (var i = ar.length - 2; i >= 0; i--)
            acc = f(ar[i], acc, i);
        return acc;
    }
    exports.reduceBack = reduceBack;
    function replicate(n, x) {
        return initialize(n, function () { return x; });
    }
    exports.replicate = replicate;
    function reverse(xs) {
        var ar = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs.slice(0) : Array.from(xs);
        return ofArray(ar.reverse());
    }
    exports.reverse = reverse;
    function scan(f, seed, xs) {
        return delay(function () {
            var iter = xs[Symbol.iterator]();
            return unfold(function (acc) {
                if (acc == null)
                    return [seed, seed];
                var cur = iter.next();
                if (!cur.done) {
                    acc = f(acc, cur.value);
                    return [acc, acc];
                }
                return void 0;
            }, null);
        });
    }
    exports.scan = scan;
    function scanBack(f, xs, seed) {
        return reverse(scan(function (acc, x) { return f(x, acc); }, seed, reverse(xs)));
    }
    exports.scanBack = scanBack;
    function singleton(x) {
        return unfold(function (x) { return x != null ? [x, null] : null; }, x);
    }
    exports.singleton = singleton;
    function skip(n, xs) {
        return _a = {},
            _a[Symbol.iterator] = function () {
                var iter = xs[Symbol.iterator]();
                for (var i = 1; i <= n; i++)
                    if (iter.next().done)
                        throw new Error("Seq has not enough elements");
                return iter;
            },
            _a;
        var _a;
    }
    exports.skip = skip;
    function skipWhile(f, xs) {
        return delay(function () {
            var hasPassed = false;
            return filter(function (x) { return hasPassed || (hasPassed = !f(x)); }, xs);
        });
    }
    exports.skipWhile = skipWhile;
    function sortWith(f, xs) {
        var ys = Array.from(xs);
        return ofArray(ys.sort(f));
    }
    exports.sortWith = sortWith;
    function sum(xs) {
        return fold(function (acc, x) { return acc + x; }, 0, xs);
    }
    exports.sum = sum;
    function sumBy(f, xs) {
        return fold(function (acc, x) { return acc + f(x); }, 0, xs);
    }
    exports.sumBy = sumBy;
    function tail(xs) {
        var iter = xs[Symbol.iterator]();
        var cur = iter.next();
        if (cur.done)
            throw new Error("Seq was empty");
        return _a = {},
            _a[Symbol.iterator] = function () { return iter; },
            _a;
        var _a;
    }
    exports.tail = tail;
    function take(n, xs, truncate) {
        if (truncate === void 0) { truncate = false; }
        return delay(function () {
            var iter = xs[Symbol.iterator]();
            return unfold(function (i) {
                if (i < n) {
                    var cur = iter.next();
                    if (!cur.done)
                        return [cur.value, i + 1];
                    if (!truncate)
                        throw new Error("Seq has not enough elements");
                }
                return void 0;
            }, 0);
        });
    }
    exports.take = take;
    function truncate(n, xs) {
        return take(n, xs, true);
    }
    exports.truncate = truncate;
    function takeWhile(f, xs) {
        return delay(function () {
            var iter = xs[Symbol.iterator]();
            return unfold(function (i) {
                var cur = iter.next();
                if (!cur.done && f(cur.value))
                    return [cur.value, null];
                return void 0;
            }, 0);
        });
    }
    exports.takeWhile = takeWhile;
    function tryFind(f, xs, defaultValue) {
        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
            var cur = iter.next();
            if (cur.done)
                return defaultValue === void 0 ? null : defaultValue;
            if (f(cur.value, i))
                return cur.value;
        }
    }
    exports.tryFind = tryFind;
    function find(f, xs) {
        return __failIfNone(tryFind(f, xs));
    }
    exports.find = find;
    function tryFindBack(f, xs, defaultValue) {
        var match = null;
        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
            var cur = iter.next();
            if (cur.done)
                return match === null ? (defaultValue === void 0 ? null : defaultValue) : match;
            if (f(cur.value, i))
                match = cur.value;
        }
    }
    exports.tryFindBack = tryFindBack;
    function findBack(f, xs) {
        return __failIfNone(tryFindBack(f, xs));
    }
    exports.findBack = findBack;
    function tryFindIndex(f, xs) {
        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
            var cur = iter.next();
            if (cur.done)
                return null;
            if (f(cur.value, i))
                return i;
        }
    }
    exports.tryFindIndex = tryFindIndex;
    function findIndex(f, xs) {
        return __failIfNone(tryFindIndex(f, xs));
    }
    exports.findIndex = findIndex;
    function tryFindIndexBack(f, xs) {
        var match = -1;
        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
            var cur = iter.next();
            if (cur.done)
                return match === -1 ? null : match;
            if (f(cur.value, i))
                match = i;
        }
    }
    exports.tryFindIndexBack = tryFindIndexBack;
    function findIndexBack(f, xs) {
        return __failIfNone(tryFindIndexBack(f, xs));
    }
    exports.findIndexBack = findIndexBack;
    function tryPick(f, xs) {
        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
            var cur = iter.next();
            if (cur.done)
                break;
            var y = f(cur.value, i);
            if (y != null)
                return y;
        }
        return void 0;
    }
    exports.tryPick = tryPick;
    function pick(f, xs) {
        return __failIfNone(tryPick(f, xs));
    }
    exports.pick = pick;
    function unfold(f, acc) {
        return _a = {},
            _a[Symbol.iterator] = function () {
                return {
                    next: function () {
                        var res = f(acc);
                        if (res != null) {
                            acc = res[1];
                            return { done: false, value: res[0] };
                        }
                        return { done: true };
                    }
                };
            },
            _a;
        var _a;
    }
    exports.unfold = unfold;
    function zip(xs, ys) {
        return map2(function (x, y) { return [x, y]; }, xs, ys);
    }
    exports.zip = zip;
    function zip3(xs, ys, zs) {
        return map3(function (x, y, z) { return [x, y, z]; }, xs, ys, zs);
    }
    exports.zip3 = zip3;
});
