(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./ListClass", "./Seq", "./Seq", "./Seq", "./Seq", "./Map", "./ListClass"], function (require, exports) {
    "use strict";
    var ListClass_1 = require("./ListClass");
    var Seq_1 = require("./Seq");
    var Seq_2 = require("./Seq");
    var Seq_3 = require("./Seq");
    var Seq_4 = require("./Seq");
    var Map_1 = require("./Map");
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = ListClass_1.default;
    var ListClass_2 = require("./ListClass");
    exports.ofArray = ListClass_2.ofArray;
    function append(xs, ys) {
        return Seq_2.fold(function (acc, x) { return new ListClass_1.default(x, acc); }, ys, reverse(xs));
    }
    exports.append = append;
    function choose(f, xs) {
        var r = Seq_2.fold(function (acc, x) {
            var y = f(x);
            return y != null ? new ListClass_1.default(y, acc) : acc;
        }, new ListClass_1.default(), xs);
        return reverse(r);
    }
    exports.choose = choose;
    function collect(f, xs) {
        return Seq_2.fold(function (acc, x) { return append(acc, f(x)); }, new ListClass_1.default(), xs);
    }
    exports.collect = collect;
    function concat(xs) {
        return collect(function (x) { return x; }, xs);
    }
    exports.concat = concat;
    function filter(f, xs) {
        return reverse(Seq_2.fold(function (acc, x) { return f(x) ? new ListClass_1.default(x, acc) : acc; }, new ListClass_1.default(), xs));
    }
    exports.filter = filter;
    function where(f, xs) {
        return filter(f, xs);
    }
    exports.where = where;
    function initialize(n, f) {
        if (n < 0) {
            throw new Error("List length must be non-negative");
        }
        var xs = new ListClass_1.default();
        for (var i = 1; i <= n; i++) {
            xs = new ListClass_1.default(f(n - i), xs);
        }
        return xs;
    }
    exports.initialize = initialize;
    function map(f, xs) {
        return reverse(Seq_2.fold(function (acc, x) { return new ListClass_1.default(f(x), acc); }, new ListClass_1.default(), xs));
    }
    exports.map = map;
    function mapIndexed(f, xs) {
        return reverse(Seq_2.fold(function (acc, x, i) { return new ListClass_1.default(f(i, x), acc); }, new ListClass_1.default(), xs));
    }
    exports.mapIndexed = mapIndexed;
    function partition(f, xs) {
        return Seq_2.fold(function (acc, x) {
            var lacc = acc[0], racc = acc[1];
            return f(x) ? [new ListClass_1.default(x, lacc), racc] : [lacc, new ListClass_1.default(x, racc)];
        }, [new ListClass_1.default(), new ListClass_1.default()], reverse(xs));
    }
    exports.partition = partition;
    function replicate(n, x) {
        return initialize(n, function () { return x; });
    }
    exports.replicate = replicate;
    function reverse(xs) {
        return Seq_2.fold(function (acc, x) { return new ListClass_1.default(x, acc); }, new ListClass_1.default(), xs);
    }
    exports.reverse = reverse;
    function singleton(x) {
        return new ListClass_1.default(x, new ListClass_1.default());
    }
    exports.singleton = singleton;
    function slice(lower, upper, xs) {
        var noLower = (lower == null);
        var noUpper = (upper == null);
        return reverse(Seq_2.fold(function (acc, x, i) { return (noLower || lower <= i) && (noUpper || i <= upper) ? new ListClass_1.default(x, acc) : acc; }, new ListClass_1.default(), xs));
    }
    exports.slice = slice;
    function unzip(xs) {
        return Seq_3.foldBack(function (xy, acc) {
            return [new ListClass_1.default(xy[0], acc[0]), new ListClass_1.default(xy[1], acc[1])];
        }, xs, [new ListClass_1.default(), new ListClass_1.default()]);
    }
    exports.unzip = unzip;
    function unzip3(xs) {
        return Seq_3.foldBack(function (xyz, acc) {
            return [new ListClass_1.default(xyz[0], acc[0]), new ListClass_1.default(xyz[1], acc[1]), new ListClass_1.default(xyz[2], acc[2])];
        }, xs, [new ListClass_1.default(), new ListClass_1.default(), new ListClass_1.default()]);
    }
    exports.unzip3 = unzip3;
    function groupBy(f, xs) {
        return Seq_4.toList(Seq_1.map(function (k) { return [k[0], Seq_4.toList(k[1])]; }, Map_1.groupBy(f, xs)));
    }
    exports.groupBy = groupBy;
});
