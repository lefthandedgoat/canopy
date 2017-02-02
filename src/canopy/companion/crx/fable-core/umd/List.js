import List from "./ListClass";
import { map as seqMap } from "./Seq";
import { fold as seqFold } from "./Seq";
import { foldBack as seqFoldBack } from "./Seq";
import { toList as seqToList } from "./Seq";
import { groupBy as mapGroupBy } from "./Map";
export default List;
export { ofArray } from "./ListClass";
export function append(xs, ys) {
    return seqFold(function (acc, x) { return new List(x, acc); }, ys, reverse(xs));
}
export function choose(f, xs) {
    var r = seqFold(function (acc, x) {
        var y = f(x);
        return y != null ? new List(y, acc) : acc;
    }, new List(), xs);
    return reverse(r);
}
export function collect(f, xs) {
    return seqFold(function (acc, x) { return append(acc, f(x)); }, new List(), xs);
}
export function concat(xs) {
    return collect(function (x) { return x; }, xs);
}
export function filter(f, xs) {
    return reverse(seqFold(function (acc, x) { return f(x) ? new List(x, acc) : acc; }, new List(), xs));
}
export function where(f, xs) {
    return filter(f, xs);
}
export function initialize(n, f) {
    if (n < 0) {
        throw new Error("List length must be non-negative");
    }
    var xs = new List();
    for (var i = 1; i <= n; i++) {
        xs = new List(f(n - i), xs);
    }
    return xs;
}
export function map(f, xs) {
    return reverse(seqFold(function (acc, x) { return new List(f(x), acc); }, new List(), xs));
}
export function mapIndexed(f, xs) {
    return reverse(seqFold(function (acc, x, i) { return new List(f(i, x), acc); }, new List(), xs));
}
export function partition(f, xs) {
    return seqFold(function (acc, x) {
        var lacc = acc[0], racc = acc[1];
        return f(x) ? [new List(x, lacc), racc] : [lacc, new List(x, racc)];
    }, [new List(), new List()], reverse(xs));
}
export function replicate(n, x) {
    return initialize(n, function () { return x; });
}
export function reverse(xs) {
    return seqFold(function (acc, x) { return new List(x, acc); }, new List(), xs);
}
export function singleton(x) {
    return new List(x, new List());
}
export function slice(lower, upper, xs) {
    var noLower = (lower == null);
    var noUpper = (upper == null);
    return reverse(seqFold(function (acc, x, i) { return (noLower || lower <= i) && (noUpper || i <= upper) ? new List(x, acc) : acc; }, new List(), xs));
}
export function unzip(xs) {
    return seqFoldBack(function (xy, acc) {
        return [new List(xy[0], acc[0]), new List(xy[1], acc[1])];
    }, xs, [new List(), new List()]);
}
export function unzip3(xs) {
    return seqFoldBack(function (xyz, acc) {
        return [new List(xyz[0], acc[0]), new List(xyz[1], acc[1]), new List(xyz[2], acc[2])];
    }, xs, [new List(), new List(), new List()]);
}
export function groupBy(f, xs) {
    return seqToList(seqMap(function (k) { return [k[0], seqToList(k[1])]; }, mapGroupBy(f, xs)));
}
