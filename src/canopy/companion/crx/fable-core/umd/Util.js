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
    var NonDeclaredType = (function () {
        function NonDeclaredType(kind, definition, generics) {
            this.kind = kind;
            this.definition = definition;
            this.generics = generics;
        }
        NonDeclaredType.prototype.Equals = function (other) {
            if (this.kind === other.kind && this.definition === other.definition) {
                return typeof this.generics === "object"
                    ? equalsRecords(this.generics, other.generics)
                    : this.generics === other.generics;
            }
            return false;
        };
        return NonDeclaredType;
    }());
    exports.NonDeclaredType = NonDeclaredType;
    exports.Any = new NonDeclaredType("Any");
    exports.Unit = new NonDeclaredType("Unit");
    function Option(t) {
        return new NonDeclaredType("Option", null, t);
    }
    exports.Option = Option;
    function FableArray(t, isTypedArray) {
        if (isTypedArray === void 0) { isTypedArray = false; }
        var def = null, genArg = null;
        if (isTypedArray) {
            def = t;
        }
        else {
            genArg = t;
        }
        return new NonDeclaredType("Array", def, genArg);
    }
    exports.Array = FableArray;
    function Tuple(ts) {
        return new NonDeclaredType("Tuple", null, ts);
    }
    exports.Tuple = Tuple;
    function GenericParam(definition) {
        return new NonDeclaredType("GenericParam", definition);
    }
    exports.GenericParam = GenericParam;
    function Interface(definition) {
        return new NonDeclaredType("Interface", definition);
    }
    exports.Interface = Interface;
    function makeGeneric(typeDef, genArgs) {
        return new NonDeclaredType("GenericType", typeDef, genArgs);
    }
    exports.makeGeneric = makeGeneric;
    function isGeneric(typ) {
        return typ instanceof NonDeclaredType && typ.generics != null;
    }
    exports.isGeneric = isGeneric;
    function getDefinition(typ) {
        return isGeneric(typ) ? typ.definition : typ;
    }
    exports.getDefinition = getDefinition;
    function extendInfo(cons, info) {
        var parent = Object.getPrototypeOf(cons.prototype);
        if (typeof parent[Symbol_1.default.reflection] === "function") {
            var newInfo_1 = {}, parentInfo_1 = parent[Symbol_1.default.reflection]();
            Object.getOwnPropertyNames(info).forEach(function (k) {
                var i = info[k];
                if (typeof i === "object") {
                    newInfo_1[k] = Array.isArray(i)
                        ? (parentInfo_1[k] || []).concat(i)
                        : Object.assign(parentInfo_1[k] || {}, i);
                }
                else {
                    newInfo_1[k] = i;
                }
            });
            return newInfo_1;
        }
        return info;
    }
    exports.extendInfo = extendInfo;
    function hasInterface(obj, interfaceName) {
        if (interfaceName === "System.Collections.Generic.IEnumerable") {
            return typeof obj[Symbol.iterator] === "function";
        }
        else if (typeof obj[Symbol_1.default.reflection] === "function") {
            var interfaces = obj[Symbol_1.default.reflection]().interfaces;
            return Array.isArray(interfaces) && interfaces.indexOf(interfaceName) > -1;
        }
        return false;
    }
    exports.hasInterface = hasInterface;
    function isArray(obj) {
        return Array.isArray(obj) || ArrayBuffer.isView(obj);
    }
    exports.isArray = isArray;
    function getRestParams(args, idx) {
        for (var _len = args.length, restArgs = Array(_len > idx ? _len - idx : 0), _key = idx; _key < _len; _key++)
            restArgs[_key - idx] = args[_key];
        return restArgs;
    }
    exports.getRestParams = getRestParams;
    function toString(o) {
        return o != null && typeof o.ToString == "function" ? o.ToString() : String(o);
    }
    exports.toString = toString;
    function hash(x) {
        var s = JSON.stringify(x);
        var h = 5381, i = 0, len = s.length;
        while (i < len) {
            h = (h * 33) ^ s.charCodeAt(i++);
        }
        return h;
    }
    exports.hash = hash;
    function equals(x, y) {
        if (x === y)
            return true;
        else if (x == null)
            return y == null;
        else if (y == null)
            return false;
        else if (Object.getPrototypeOf(x) !== Object.getPrototypeOf(y))
            return false;
        else if (typeof x.Equals === "function")
            return x.Equals(y);
        else if (Array.isArray(x)) {
            if (x.length != y.length)
                return false;
            for (var i = 0; i < x.length; i++)
                if (!equals(x[i], y[i]))
                    return false;
            return true;
        }
        else if (ArrayBuffer.isView(x)) {
            if (x.byteLength !== y.byteLength)
                return false;
            var dv1 = new DataView(x.buffer), dv2 = new DataView(y.buffer);
            for (var i = 0; i < x.byteLength; i++)
                if (dv1.getUint8(i) !== dv2.getUint8(i))
                    return false;
            return true;
        }
        else if (x instanceof Date)
            return x.getTime() == y.getTime();
        else
            return false;
    }
    exports.equals = equals;
    function compare(x, y) {
        if (x === y)
            return 0;
        if (x == null)
            return y == null ? 0 : -1;
        else if (y == null)
            return 1;
        else if (Object.getPrototypeOf(x) !== Object.getPrototypeOf(y))
            return -1;
        else if (typeof x.CompareTo === "function")
            return x.CompareTo(y);
        else if (Array.isArray(x)) {
            if (x.length != y.length)
                return x.length < y.length ? -1 : 1;
            for (var i = 0, j = 0; i < x.length; i++)
                if ((j = compare(x[i], y[i])) !== 0)
                    return j;
            return 0;
        }
        else if (ArrayBuffer.isView(x)) {
            if (x.byteLength != y.byteLength)
                return x.byteLength < y.byteLength ? -1 : 1;
            var dv1 = new DataView(x.buffer), dv2 = new DataView(y.buffer);
            for (var i = 0, b1 = 0, b2 = 0; i < x.byteLength; i++) {
                b1 = dv1.getUint8(i), b2 = dv2.getUint8(i);
                if (b1 < b2)
                    return -1;
                if (b1 > b2)
                    return 1;
            }
            return 0;
        }
        else if (x instanceof Date)
            return compare(x.getTime(), y.getTime());
        else
            return x < y ? -1 : 1;
    }
    exports.compare = compare;
    function equalsRecords(x, y) {
        if (x === y) {
            return true;
        }
        else {
            var keys = Object.getOwnPropertyNames(x);
            for (var i = 0; i < keys.length; i++) {
                if (!equals(x[keys[i]], y[keys[i]]))
                    return false;
            }
            return true;
        }
    }
    exports.equalsRecords = equalsRecords;
    function compareRecords(x, y) {
        if (x === y) {
            return 0;
        }
        else {
            var keys = Object.getOwnPropertyNames(x);
            for (var i = 0; i < keys.length; i++) {
                var res = compare(x[keys[i]], y[keys[i]]);
                if (res !== 0)
                    return res;
            }
            return 0;
        }
    }
    exports.compareRecords = compareRecords;
    function equalsUnions(x, y) {
        if (x === y) {
            return true;
        }
        else if (x.Case !== y.Case) {
            return false;
        }
        else {
            for (var i = 0; i < x.Fields.length; i++) {
                if (!equals(x.Fields[i], y.Fields[i]))
                    return false;
            }
            return true;
        }
    }
    exports.equalsUnions = equalsUnions;
    function compareUnions(x, y) {
        if (x === y) {
            return 0;
        }
        else {
            var res = compare(x.Case, y.Case);
            if (res !== 0)
                return res;
            for (var i = 0; i < x.Fields.length; i++) {
                res = compare(x.Fields[i], y.Fields[i]);
                if (res !== 0)
                    return res;
            }
            return 0;
        }
    }
    exports.compareUnions = compareUnions;
    function createDisposable(f) {
        return _a = {
                Dispose: f
            },
            _a[Symbol_1.default.reflection] = function () { return { interfaces: ["System.IDisposable"] }; },
            _a;
        var _a;
    }
    exports.createDisposable = createDisposable;
    function createObj(fields) {
        var iter = fields[Symbol.iterator]();
        var cur = iter.next(), o = {};
        while (!cur.done) {
            o[cur.value[0]] = cur.value[1];
            cur = iter.next();
        }
        return o;
    }
    exports.createObj = createObj;
    function toPlainJsObj(source) {
        if (source != null && source.constructor != Object) {
            var target = {};
            var props = Object.getOwnPropertyNames(source);
            for (var i = 0; i < props.length; i++) {
                target[props[i]] = source[props[i]];
            }
            var proto = Object.getPrototypeOf(source);
            if (proto != null) {
                props = Object.getOwnPropertyNames(proto);
                for (var i = 0; i < props.length; i++) {
                    var prop = Object.getOwnPropertyDescriptor(proto, props[i]);
                    if (prop.value) {
                        target[props[i]] = prop.value;
                    }
                    else if (prop.get) {
                        target[props[i]] = prop.get.apply(source);
                    }
                }
            }
            return target;
        }
        else {
            return source;
        }
    }
    exports.toPlainJsObj = toPlainJsObj;
    function round(value, digits) {
        if (digits === void 0) { digits = 0; }
        var m = Math.pow(10, digits);
        var n = +(digits ? value * m : value).toFixed(8);
        var i = Math.floor(n), f = n - i;
        var e = 1e-8;
        var r = (f > 0.5 - e && f < 0.5 + e) ? ((i % 2 == 0) ? i : i + 1) : Math.round(n);
        return digits ? r / m : r;
    }
    exports.round = round;
    function randomNext(min, max) {
        return Math.floor(Math.random() * (max - min)) + min;
    }
    exports.randomNext = randomNext;
    function defaultArg(arg, defaultValue) {
        return arg == null ? defaultValue : arg;
    }
    exports.defaultArg = defaultArg;
});
