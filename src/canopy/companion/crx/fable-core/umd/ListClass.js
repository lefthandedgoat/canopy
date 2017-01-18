(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Symbol", "./Util", "./Util", "./Util"], function (require, exports) {
    "use strict";
    var Symbol_1 = require("./Symbol");
    var Util_1 = require("./Util");
    var Util_2 = require("./Util");
    var Util_3 = require("./Util");
    function ofArray(args, base) {
        var acc = base || new List();
        for (var i = args.length - 1; i >= 0; i--) {
            acc = new List(args[i], acc);
        }
        return acc;
    }
    exports.ofArray = ofArray;
    var List = (function () {
        function List(head, tail) {
            this.head = head;
            this.tail = tail;
        }
        List.prototype.ToString = function () {
            return "[" + Array.from(this).map(Util_1.toString).join("; ") + "]";
        };
        List.prototype.Equals = function (x) {
            if (this === x) {
                return true;
            }
            else {
                var iter1 = this[Symbol.iterator](), iter2 = x[Symbol.iterator]();
                for (;;) {
                    var cur1 = iter1.next(), cur2 = iter2.next();
                    if (cur1.done)
                        return cur2.done ? true : false;
                    else if (cur2.done)
                        return false;
                    else if (!Util_2.equals(cur1.value, cur2.value))
                        return false;
                }
            }
        };
        List.prototype.CompareTo = function (x) {
            if (this === x) {
                return 0;
            }
            else {
                var acc = 0;
                var iter1 = this[Symbol.iterator](), iter2 = x[Symbol.iterator]();
                for (;;) {
                    var cur1 = iter1.next(), cur2 = iter2.next();
                    if (cur1.done)
                        return cur2.done ? acc : -1;
                    else if (cur2.done)
                        return 1;
                    else {
                        acc = Util_3.compare(cur1.value, cur2.value);
                        if (acc != 0)
                            return acc;
                    }
                }
            }
        };
        Object.defineProperty(List.prototype, "length", {
            get: function () {
                var cur = this, acc = 0;
                while (cur.tail != null) {
                    cur = cur.tail;
                    acc++;
                }
                return acc;
            },
            enumerable: true,
            configurable: true
        });
        List.prototype[Symbol.iterator] = function () {
            var cur = this;
            return {
                next: function () {
                    var tmp = cur;
                    cur = cur.tail;
                    return { done: tmp.tail == null, value: tmp.head };
                }
            };
        };
        List.prototype[Symbol_1.default.reflection] = function () {
            return {
                type: "Microsoft.FSharp.Collections.FSharpList",
                interfaces: ["System.IEquatable", "System.IComparable"]
            };
        };
        return List;
    }());
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = List;
});
