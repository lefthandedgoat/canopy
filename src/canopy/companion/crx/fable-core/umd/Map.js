(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./ListClass", "./ListClass", "./Util", "./Util", "./Util", "./GenericComparer", "./Symbol", "./Seq", "./Seq", "./Seq", "./Seq", "./Seq"], function (require, exports) {
    "use strict";
    var ListClass_1 = require("./ListClass");
    var ListClass_2 = require("./ListClass");
    var Util_1 = require("./Util");
    var Util_2 = require("./Util");
    var Util_3 = require("./Util");
    var GenericComparer_1 = require("./GenericComparer");
    var Symbol_1 = require("./Symbol");
    var Seq_1 = require("./Seq");
    var Seq_2 = require("./Seq");
    var Seq_3 = require("./Seq");
    var Seq_4 = require("./Seq");
    var Seq_5 = require("./Seq");
    function groupBy(f, xs) {
        var keys = [], iter = xs[Symbol.iterator]();
        var acc = create(), cur = iter.next();
        while (!cur.done) {
            var k = f(cur.value), vs = tryFind(k, acc);
            if (vs == null) {
                keys.push(k);
                acc = add(k, [cur.value], acc);
            }
            else {
                vs.push(cur.value);
            }
            cur = iter.next();
        }
        return keys.map(function (k) { return [k, acc.get(k)]; });
    }
    exports.groupBy = groupBy;
    function countBy(f, xs) {
        return groupBy(f, xs).map(function (kv) { return [kv[0], kv[1].length]; });
    }
    exports.countBy = countBy;
    var MapTree = (function () {
        function MapTree(caseName, fields) {
            this.Case = caseName;
            this.Fields = fields;
        }
        return MapTree;
    }());
    exports.MapTree = MapTree;
    function tree_sizeAux(acc, m) {
        return m.Case === "MapOne"
            ? acc + 1
            : m.Case === "MapNode"
                ? tree_sizeAux(tree_sizeAux(acc + 1, m.Fields[2]), m.Fields[3])
                : acc;
    }
    function tree_size(x) {
        return tree_sizeAux(0, x);
    }
    function tree_empty() {
        return new MapTree("MapEmpty", []);
    }
    function tree_height(_arg1) {
        return _arg1.Case === "MapOne" ? 1 : _arg1.Case === "MapNode" ? _arg1.Fields[4] : 0;
    }
    function tree_isEmpty(m) {
        return m.Case === "MapEmpty" ? true : false;
    }
    function tree_mk(l, k, v, r) {
        var matchValue = [l, r];
        var $target1 = function () {
            var hl = tree_height(l);
            var hr = tree_height(r);
            var m = hl < hr ? hr : hl;
            return new MapTree("MapNode", [k, v, l, r, m + 1]);
        };
        if (matchValue[0].Case === "MapEmpty") {
            if (matchValue[1].Case === "MapEmpty") {
                return new MapTree("MapOne", [k, v]);
            }
            else {
                return $target1();
            }
        }
        else {
            return $target1();
        }
    }
    ;
    function tree_rebalance(t1, k, v, t2) {
        var t1h = tree_height(t1);
        var t2h = tree_height(t2);
        if (t2h > t1h + 2) {
            if (t2.Case === "MapNode") {
                if (tree_height(t2.Fields[2]) > t1h + 1) {
                    if (t2.Fields[2].Case === "MapNode") {
                        return tree_mk(tree_mk(t1, k, v, t2.Fields[2].Fields[2]), t2.Fields[2].Fields[0], t2.Fields[2].Fields[1], tree_mk(t2.Fields[2].Fields[3], t2.Fields[0], t2.Fields[1], t2.Fields[3]));
                    }
                    else {
                        throw new Error("rebalance");
                    }
                }
                else {
                    return tree_mk(tree_mk(t1, k, v, t2.Fields[2]), t2.Fields[0], t2.Fields[1], t2.Fields[3]);
                }
            }
            else {
                throw new Error("rebalance");
            }
        }
        else {
            if (t1h > t2h + 2) {
                if (t1.Case === "MapNode") {
                    if (tree_height(t1.Fields[3]) > t2h + 1) {
                        if (t1.Fields[3].Case === "MapNode") {
                            return tree_mk(tree_mk(t1.Fields[2], t1.Fields[0], t1.Fields[1], t1.Fields[3].Fields[2]), t1.Fields[3].Fields[0], t1.Fields[3].Fields[1], tree_mk(t1.Fields[3].Fields[3], k, v, t2));
                        }
                        else {
                            throw new Error("rebalance");
                        }
                    }
                    else {
                        return tree_mk(t1.Fields[2], t1.Fields[0], t1.Fields[1], tree_mk(t1.Fields[3], k, v, t2));
                    }
                }
                else {
                    throw new Error("rebalance");
                }
            }
            else {
                return tree_mk(t1, k, v, t2);
            }
        }
    }
    function tree_add(comparer, k, v, m) {
        if (m.Case === "MapOne") {
            var c = comparer.Compare(k, m.Fields[0]);
            if (c < 0) {
                return new MapTree("MapNode", [k, v, new MapTree("MapEmpty", []), m, 2]);
            }
            else if (c === 0) {
                return new MapTree("MapOne", [k, v]);
            }
            return new MapTree("MapNode", [k, v, m, new MapTree("MapEmpty", []), 2]);
        }
        else if (m.Case === "MapNode") {
            var c = comparer.Compare(k, m.Fields[0]);
            if (c < 0) {
                return tree_rebalance(tree_add(comparer, k, v, m.Fields[2]), m.Fields[0], m.Fields[1], m.Fields[3]);
            }
            else if (c === 0) {
                return new MapTree("MapNode", [k, v, m.Fields[2], m.Fields[3], m.Fields[4]]);
            }
            return tree_rebalance(m.Fields[2], m.Fields[0], m.Fields[1], tree_add(comparer, k, v, m.Fields[3]));
        }
        return new MapTree("MapOne", [k, v]);
    }
    function tree_find(comparer, k, m) {
        var res = tree_tryFind(comparer, k, m);
        if (res != null)
            return res;
        throw new Error("key not found");
    }
    function tree_tryFind(comparer, k, m) {
        if (m.Case === "MapOne") {
            var c = comparer.Compare(k, m.Fields[0]);
            return c === 0 ? m.Fields[1] : null;
        }
        else if (m.Case === "MapNode") {
            var c = comparer.Compare(k, m.Fields[0]);
            if (c < 0) {
                return tree_tryFind(comparer, k, m.Fields[2]);
            }
            else {
                if (c === 0) {
                    return m.Fields[1];
                }
                else {
                    return tree_tryFind(comparer, k, m.Fields[3]);
                }
            }
        }
        return null;
    }
    function tree_partition1(comparer, f, k, v, acc1, acc2) {
        return f(k, v) ? [tree_add(comparer, k, v, acc1), acc2] : [acc1, tree_add(comparer, k, v, acc2)];
    }
    function tree_partitionAux(comparer, f, s, acc_0, acc_1) {
        var acc = [acc_0, acc_1];
        if (s.Case === "MapOne") {
            return tree_partition1(comparer, f, s.Fields[0], s.Fields[1], acc[0], acc[1]);
        }
        else if (s.Case === "MapNode") {
            var acc_2 = tree_partitionAux(comparer, f, s.Fields[3], acc[0], acc[1]);
            var acc_3 = tree_partition1(comparer, f, s.Fields[0], s.Fields[1], acc_2[0], acc_2[1]);
            return tree_partitionAux(comparer, f, s.Fields[2], acc_3[0], acc_3[1]);
        }
        return acc;
    }
    function tree_partition(comparer, f, s) {
        return tree_partitionAux(comparer, f, s, tree_empty(), tree_empty());
    }
    function tree_filter1(comparer, f, k, v, acc) {
        return f(k, v) ? tree_add(comparer, k, v, acc) : acc;
    }
    function tree_filterAux(comparer, f, s, acc) {
        return s.Case === "MapOne" ? tree_filter1(comparer, f, s.Fields[0], s.Fields[1], acc) : s.Case === "MapNode" ? tree_filterAux(comparer, f, s.Fields[3], tree_filter1(comparer, f, s.Fields[0], s.Fields[1], tree_filterAux(comparer, f, s.Fields[2], acc))) : acc;
    }
    function tree_filter(comparer, f, s) {
        return tree_filterAux(comparer, f, s, tree_empty());
    }
    function tree_spliceOutSuccessor(m) {
        if (m.Case === "MapOne") {
            return [m.Fields[0], m.Fields[1], new MapTree("MapEmpty", [])];
        }
        else if (m.Case === "MapNode") {
            if (m.Fields[2].Case === "MapEmpty") {
                return [m.Fields[0], m.Fields[1], m.Fields[3]];
            }
            else {
                var kvl = tree_spliceOutSuccessor(m.Fields[2]);
                return [kvl[0], kvl[1], tree_mk(kvl[2], m.Fields[0], m.Fields[1], m.Fields[3])];
            }
        }
        throw new Error("internal error: Map.spliceOutSuccessor");
    }
    function tree_remove(comparer, k, m) {
        if (m.Case === "MapOne") {
            var c = comparer.Compare(k, m.Fields[0]);
            if (c === 0) {
                return new MapTree("MapEmpty", []);
            }
            else {
                return m;
            }
        }
        else if (m.Case === "MapNode") {
            var c = comparer.Compare(k, m.Fields[0]);
            if (c < 0) {
                return tree_rebalance(tree_remove(comparer, k, m.Fields[2]), m.Fields[0], m.Fields[1], m.Fields[3]);
            }
            else {
                if (c === 0) {
                    var matchValue = [m.Fields[2], m.Fields[3]];
                    if (matchValue[0].Case === "MapEmpty") {
                        return m.Fields[3];
                    }
                    else {
                        if (matchValue[1].Case === "MapEmpty") {
                            return m.Fields[2];
                        }
                        else {
                            var patternInput = tree_spliceOutSuccessor(m.Fields[3]);
                            var sv = patternInput[1];
                            var sk = patternInput[0];
                            var r_ = patternInput[2];
                            return tree_mk(m.Fields[2], sk, sv, r_);
                        }
                    }
                }
                else {
                    return tree_rebalance(m.Fields[2], m.Fields[0], m.Fields[1], tree_remove(comparer, k, m.Fields[3]));
                }
            }
        }
        else {
            return tree_empty();
        }
    }
    function tree_mem(comparer, k, m) {
        if (m.Case === "MapOne") {
            return comparer.Compare(k, m.Fields[0]) === 0;
        }
        else if (m.Case === "MapNode") {
            var c = comparer.Compare(k, m.Fields[0]);
            if (c < 0) {
                return tree_mem(comparer, k, m.Fields[2]);
            }
            else {
                if (c === 0) {
                    return true;
                }
                else {
                    return tree_mem(comparer, k, m.Fields[3]);
                }
            }
        }
        else {
            return false;
        }
    }
    function tree_iter(f, m) {
        if (m.Case === "MapOne") {
            f(m.Fields[0], m.Fields[1]);
        }
        else if (m.Case === "MapNode") {
            tree_iter(f, m.Fields[2]);
            f(m.Fields[0], m.Fields[1]);
            tree_iter(f, m.Fields[3]);
        }
    }
    function tree_tryPick(f, m) {
        if (m.Case === "MapOne") {
            return f(m.Fields[0], m.Fields[1]);
        }
        else if (m.Case === "MapNode") {
            var matchValue = tree_tryPick(f, m.Fields[2]);
            if (matchValue == null) {
                var matchValue_1 = f(m.Fields[0], m.Fields[1]);
                if (matchValue_1 == null) {
                    return tree_tryPick(f, m.Fields[3]);
                }
                else {
                    var res = matchValue_1;
                    return res;
                }
            }
            else {
                var res = matchValue;
                return res;
            }
        }
        else {
            return null;
        }
    }
    function tree_exists(f, m) {
        return m.Case === "MapOne" ? f(m.Fields[0], m.Fields[1]) : m.Case === "MapNode" ? (tree_exists(f, m.Fields[2]) ? true : f(m.Fields[0], m.Fields[1])) ? true : tree_exists(f, m.Fields[3]) : false;
    }
    function tree_forall(f, m) {
        return m.Case === "MapOne" ? f(m.Fields[0], m.Fields[1]) : m.Case === "MapNode" ? (tree_forall(f, m.Fields[2]) ? f(m.Fields[0], m.Fields[1]) : false) ? tree_forall(f, m.Fields[3]) : false : true;
    }
    function tree_mapi(f, m) {
        return m.Case === "MapOne" ? new MapTree("MapOne", [m.Fields[0], f(m.Fields[0], m.Fields[1])]) : m.Case === "MapNode" ? new MapTree("MapNode", [m.Fields[0], f(m.Fields[0], m.Fields[1]), tree_mapi(f, m.Fields[2]), tree_mapi(f, m.Fields[3]), m.Fields[4]]) : tree_empty();
    }
    function tree_foldBack(f, m, x) {
        return m.Case === "MapOne" ? f(m.Fields[0], m.Fields[1], x) : m.Case === "MapNode" ? tree_foldBack(f, m.Fields[2], f(m.Fields[0], m.Fields[1], tree_foldBack(f, m.Fields[3], x))) : x;
    }
    function tree_fold(f, x, m) {
        return m.Case === "MapOne" ? f(x, m.Fields[0], m.Fields[1]) : m.Case === "MapNode" ? tree_fold(f, f(tree_fold(f, x, m.Fields[2]), m.Fields[0], m.Fields[1]), m.Fields[3]) : x;
    }
    function tree_mkFromEnumerator(comparer, acc, e) {
        var cur = e.next();
        while (!cur.done) {
            acc = tree_add(comparer, cur.value[0], cur.value[1], acc);
            cur = e.next();
        }
        return acc;
    }
    function tree_ofSeq(comparer, c) {
        var ie = c[Symbol.iterator]();
        return tree_mkFromEnumerator(comparer, tree_empty(), ie);
    }
    function tree_collapseLHS(stack) {
        if (stack.tail != null) {
            if (stack.head.Case === "MapOne") {
                return stack;
            }
            else if (stack.head.Case === "MapNode") {
                return tree_collapseLHS(ListClass_2.ofArray([
                    stack.head.Fields[2],
                    new MapTree("MapOne", [stack.head.Fields[0], stack.head.Fields[1]]),
                    stack.head.Fields[3]
                ], stack.tail));
            }
            else {
                return tree_collapseLHS(stack.tail);
            }
        }
        else {
            return new ListClass_1.default();
        }
    }
    function tree_mkIterator(s) {
        return { stack: tree_collapseLHS(new ListClass_1.default(s, new ListClass_1.default())), started: false };
    }
    function tree_moveNext(i) {
        function current(i) {
            if (i.stack.tail == null) {
                return null;
            }
            else if (i.stack.head.Case === "MapOne") {
                return [i.stack.head.Fields[0], i.stack.head.Fields[1]];
            }
            throw new Error("Please report error: Map iterator, unexpected stack for current");
        }
        if (i.started) {
            if (i.stack.tail == null) {
                return { done: true, value: null };
            }
            else {
                if (i.stack.head.Case === "MapOne") {
                    i.stack = tree_collapseLHS(i.stack.tail);
                    return {
                        done: i.stack.tail == null,
                        value: current(i)
                    };
                }
                else {
                    throw new Error("Please report error: Map iterator, unexpected stack for moveNext");
                }
            }
        }
        else {
            i.started = true;
            return {
                done: i.stack.tail == null,
                value: current(i)
            };
        }
        ;
    }
    var FableMap = (function () {
        function FableMap() {
        }
        FableMap.prototype.ToString = function () {
            return "map [" + Array.from(this).map(Util_1.toString).join("; ") + "]";
        };
        FableMap.prototype.Equals = function (m2) {
            return this.CompareTo(m2) === 0;
        };
        FableMap.prototype.CompareTo = function (m2) {
            var _this = this;
            return this === m2 ? 0 : Seq_5.compareWith(function (kvp1, kvp2) {
                var c = _this.comparer.Compare(kvp1[0], kvp2[0]);
                return c !== 0 ? c : Util_3.compare(kvp1[1], kvp2[1]);
            }, this, m2);
        };
        FableMap.prototype[Symbol.iterator] = function () {
            var i = tree_mkIterator(this.tree);
            return {
                next: function () { return tree_moveNext(i); }
            };
        };
        FableMap.prototype.entries = function () {
            return this[Symbol.iterator]();
        };
        FableMap.prototype.keys = function () {
            return Seq_1.map(function (kv) { return kv[0]; }, this);
        };
        FableMap.prototype.values = function () {
            return Seq_1.map(function (kv) { return kv[1]; }, this);
        };
        FableMap.prototype.get = function (k) {
            return tree_find(this.comparer, k, this.tree);
        };
        FableMap.prototype.has = function (k) {
            return tree_mem(this.comparer, k, this.tree);
        };
        FableMap.prototype.set = function (k, v) {
            throw new Error("not supported");
        };
        FableMap.prototype.delete = function (k) {
            throw new Error("not supported");
        };
        FableMap.prototype.clear = function () {
            throw new Error("not supported");
        };
        Object.defineProperty(FableMap.prototype, "size", {
            get: function () {
                return tree_size(this.tree);
            },
            enumerable: true,
            configurable: true
        });
        FableMap.prototype[Symbol_1.default.reflection] = function () {
            return {
                type: "Microsoft.FSharp.Collections.FSharpMap",
                interfaces: ["System.IEquatable", "System.IComparable", "System.Collections.Generic.IDictionary"]
            };
        };
        return FableMap;
    }());
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = FableMap;
    function from(comparer, tree) {
        var map = new FableMap();
        map.tree = tree;
        map.comparer = comparer || new GenericComparer_1.default();
        return map;
    }
    function create(ie, comparer) {
        comparer = comparer || new GenericComparer_1.default();
        return from(comparer, ie ? tree_ofSeq(comparer, ie) : tree_empty());
    }
    exports.create = create;
    function add(k, v, map) {
        return from(map.comparer, tree_add(map.comparer, k, v, map.tree));
    }
    exports.add = add;
    function remove(item, map) {
        return from(map.comparer, tree_remove(map.comparer, item, map.tree));
    }
    exports.remove = remove;
    function containsValue(v, map) {
        return Seq_2.fold(function (acc, k) { return acc || Util_2.equals(map.get(k), v); }, false, map.keys());
    }
    exports.containsValue = containsValue;
    function tryGetValue(map, key, defaultValue) {
        return map.has(key) ? [true, map.get(key)] : [false, defaultValue];
    }
    exports.tryGetValue = tryGetValue;
    function exists(f, map) {
        return tree_exists(f, map.tree);
    }
    exports.exists = exists;
    function find(k, map) {
        return tree_find(map.comparer, k, map.tree);
    }
    exports.find = find;
    function tryFind(k, map) {
        return tree_tryFind(map.comparer, k, map.tree);
    }
    exports.tryFind = tryFind;
    function filter(f, map) {
        return from(map.comparer, tree_filter(map.comparer, f, map.tree));
    }
    exports.filter = filter;
    function fold(f, seed, map) {
        return tree_fold(f, seed, map.tree);
    }
    exports.fold = fold;
    function foldBack(f, map, seed) {
        return tree_foldBack(f, map.tree, seed);
    }
    exports.foldBack = foldBack;
    function forAll(f, map) {
        return tree_forall(f, map.tree);
    }
    exports.forAll = forAll;
    function isEmpty(map) {
        return tree_isEmpty(map.tree);
    }
    exports.isEmpty = isEmpty;
    function iterate(f, map) {
        tree_iter(f, map.tree);
    }
    exports.iterate = iterate;
    function map(f, map) {
        return from(map.comparer, tree_mapi(f, map.tree));
    }
    exports.map = map;
    function partition(f, map) {
        var rs = tree_partition(map.comparer, f, map.tree);
        return [from(map.comparer, rs[0]), from(map.comparer, rs[1])];
    }
    exports.partition = partition;
    function findKey(f, map) {
        return Seq_3.pick(function (kv) { return f(kv[0], kv[1]) ? kv[0] : null; }, map);
    }
    exports.findKey = findKey;
    function tryFindKey(f, map) {
        return Seq_4.tryPick(function (kv) { return f(kv[0], kv[1]) ? kv[0] : null; }, map);
    }
    exports.tryFindKey = tryFindKey;
    function pick(f, map) {
        var res = tryPick(f, map);
        if (res != null)
            return res;
        throw new Error("key not found");
    }
    exports.pick = pick;
    function tryPick(f, map) {
        return tree_tryPick(f, map.tree);
    }
    exports.tryPick = tryPick;
});
