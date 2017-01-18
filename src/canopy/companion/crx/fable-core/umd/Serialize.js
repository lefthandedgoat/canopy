(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Symbol", "./Symbol", "./List", "./List", "./Set", "./Map", "./Map", "./Set", "./Util", "./Util", "./Util", "./Seq", "./Reflection", "./Date", "./String"], function (require, exports) {
    "use strict";
    var Symbol_1 = require("./Symbol");
    var Symbol_2 = require("./Symbol");
    var List_1 = require("./List");
    var List_2 = require("./List");
    var Set_1 = require("./Set");
    var Map_1 = require("./Map");
    var Map_2 = require("./Map");
    var Set_2 = require("./Set");
    var Util_1 = require("./Util");
    var Util_2 = require("./Util");
    var Util_3 = require("./Util");
    var Seq_1 = require("./Seq");
    var Reflection_1 = require("./Reflection");
    var Date_1 = require("./Date");
    var String_1 = require("./String");
    function toJson(o) {
        return JSON.stringify(o, function (k, v) {
            if (ArrayBuffer.isView(v)) {
                return Array.from(v);
            }
            else if (v != null && typeof v === "object") {
                var properties = typeof v[Symbol_1.default.reflection] === "function" ? v[Symbol_1.default.reflection]().properties : null;
                if (v instanceof List_1.default || v instanceof Set_1.default || v instanceof Set) {
                    return Array.from(v);
                }
                else if (v instanceof Map_1.default || v instanceof Map) {
                    return Seq_1.fold(function (o, kv) {
                        return o[toJson(kv[0])] = kv[1], o;
                    }, {}, v);
                }
                else if (!Util_1.hasInterface(v, "FSharpRecord") && properties) {
                    return Seq_1.fold(function (o, prop) {
                        return o[prop] = v[prop], o;
                    }, {}, Object.getOwnPropertyNames(properties));
                }
                else if (Util_1.hasInterface(v, "FSharpUnion")) {
                    if (!v.Fields || !v.Fields.length) {
                        return v.Case;
                    }
                    else if (v.Fields.length === 1) {
                        var fieldValue = typeof v.Fields[0] === 'undefined' ? null : v.Fields[0];
                        return _a = {}, _a[v.Case] = fieldValue, _a;
                    }
                    else {
                        return _b = {}, _b[v.Case] = v.Fields, _b;
                    }
                }
            }
            return v;
            var _a, _b;
        });
    }
    exports.toJson = toJson;
    function combine(path1, path2) {
        return typeof path2 === "number"
            ? path1 + "[" + path2 + "]"
            : (path1 ? path1 + "." : "") + path2;
    }
    function isNullable(typ) {
        if (typeof typ === "string") {
            return typ !== "boolean" && typ !== "number";
        }
        else if (typ instanceof Util_3.NonDeclaredType) {
            return typ.kind !== "Array" && typ.kind !== "Tuple";
        }
        else {
            var info = typeof typ.prototype[Symbol_1.default.reflection] === "function"
                ? typ.prototype[Symbol_1.default.reflection]() : null;
            return info ? info.nullable : true;
        }
    }
    function invalidate(val, typ, path) {
        throw new Error(String_1.fsFormat("%A", val) + " " + (path ? "(" + path + ")" : "") + " is not of type " + Reflection_1.getTypeFullName(typ));
    }
    function needsInflate(enclosing) {
        var typ = enclosing.head;
        if (typeof typ === "string") {
            return false;
        }
        if (typ instanceof Util_3.NonDeclaredType) {
            switch (typ.kind) {
                case "Option":
                case "Array":
                    return typ.definition != null || needsInflate(new List_1.default(typ.generics, enclosing));
                case "Tuple":
                    return typ.generics.some(function (x) {
                        return needsInflate(new List_1.default(x, enclosing));
                    });
                case "GenericParam":
                    return needsInflate(Reflection_1.resolveGeneric(typ.definition, enclosing.tail));
                case "GenericType":
                    return true;
                default:
                    return false;
            }
        }
        return true;
    }
    function inflateArray(arr, enclosing, path) {
        if (!Array.isArray) {
            invalidate(arr, "array", path);
        }
        return needsInflate(enclosing)
            ? arr.map(function (x, i) { return inflate(x, enclosing, combine(path, i)); })
            : arr;
    }
    function inflateMap(obj, keyEnclosing, valEnclosing, path) {
        var inflateKey = keyEnclosing.head !== "string";
        var inflateVal = needsInflate(valEnclosing);
        return Object
            .getOwnPropertyNames(obj)
            .map(function (k) {
            var key = inflateKey ? inflate(JSON.parse(k), keyEnclosing, combine(path, k)) : k;
            var val = inflateVal ? inflate(obj[k], valEnclosing, combine(path, k)) : obj[k];
            return [key, val];
        });
    }
    function inflateList(val, enclosing, path) {
        var ar = [], li = new List_1.default(), cur = val, inf = needsInflate(enclosing);
        while (cur.tail != null) {
            ar.push(inf ? inflate(cur.head, enclosing, path) : cur.head);
            cur = cur.tail;
        }
        ar.reverse();
        for (var i = 0; i < ar.length; i++) {
            li = new List_1.default(ar[i], li);
        }
        return li;
    }
    function inflate(val, typ, path) {
        var enclosing = null;
        if (typ instanceof List_1.default) {
            enclosing = typ;
            typ = typ.head;
        }
        else {
            enclosing = new List_1.default(typ, new List_1.default());
        }
        if (val == null) {
            if (!isNullable(typ)) {
                invalidate(val, typ, path);
            }
            return val;
        }
        else if (typeof typ === "string") {
            if ((typ === "boolean" || typ === "number" || typ === "string") && (typeof val !== typ)) {
                invalidate(val, typ, path);
            }
            return val;
        }
        else if (typ instanceof Util_3.NonDeclaredType) {
            switch (typ.kind) {
                case "Unit":
                    return null;
                case "Option":
                    return inflate(val, new List_1.default(typ.generics, enclosing), path);
                case "Array":
                    if (typ.definition != null) {
                        return new typ.definition(val);
                    }
                    else {
                        return inflateArray(val, new List_1.default(typ.generics, enclosing), path);
                    }
                case "Tuple":
                    return typ.generics.map(function (x, i) {
                        return inflate(val[i], new List_1.default(x, enclosing), combine(path, i));
                    });
                case "GenericParam":
                    return inflate(val, Reflection_1.resolveGeneric(typ.definition, enclosing.tail), path);
                case "GenericType":
                    var def = typ.definition;
                    if (def === List_1.default) {
                        return Array.isArray(val)
                            ? List_2.ofArray(inflateArray(val, Reflection_1.resolveGeneric(0, enclosing), path))
                            : inflateList(val, Reflection_1.resolveGeneric(0, enclosing), path);
                    }
                    if (def === Set_1.default) {
                        return Set_2.create(inflateArray(val, Reflection_1.resolveGeneric(0, enclosing), path));
                    }
                    if (def === Set) {
                        return new Set(inflateArray(val, Reflection_1.resolveGeneric(0, enclosing), path));
                    }
                    if (def === Map_1.default) {
                        return Map_2.create(inflateMap(val, Reflection_1.resolveGeneric(0, enclosing), Reflection_1.resolveGeneric(1, enclosing), path));
                    }
                    if (def === Map) {
                        return new Map(inflateMap(val, Reflection_1.resolveGeneric(0, enclosing), Reflection_1.resolveGeneric(1, enclosing), path));
                    }
                    return inflate(val, new List_1.default(typ.definition, enclosing), path);
                default:
                    return val;
            }
        }
        else if (typeof typ === "function") {
            if (typ === Date) {
                return Date_1.parse(val);
            }
            var info = typeof typ.prototype[Symbol_1.default.reflection] === "function" ? typ.prototype[Symbol_1.default.reflection]() : {};
            if (info.cases) {
                var uCase = void 0, uFields = [];
                if (typeof val === "string") {
                    uCase = val;
                }
                else if (typeof val.Case === "string" && Array.isArray(val.Fields)) {
                    uCase = val.Case;
                    uFields = val.Fields;
                }
                else {
                    var caseName = Object.getOwnPropertyNames(val)[0];
                    var fieldTypes = info.cases[caseName];
                    if (Array.isArray(fieldTypes)) {
                        var fields = fieldTypes.length > 1 ? val[caseName] : [val[caseName]];
                        uCase = caseName;
                        path = combine(path, caseName);
                        for (var i = 0; i < fieldTypes.length; i++) {
                            uFields.push(inflate(fields[i], new List_1.default(fieldTypes[i], enclosing), combine(path, i)));
                        }
                    }
                }
                if (uCase in info.cases === false) {
                    invalidate(val, typ, path);
                }
                return new typ(uCase, uFields);
            }
            if (info.properties) {
                var newObj = new typ();
                var properties = info.properties;
                var ks = Object.getOwnPropertyNames(properties);
                for (var i = 0; i < ks.length; i++) {
                    var k = ks[i];
                    newObj[k] = inflate(val[k], new List_1.default(properties[k], enclosing), combine(path, k));
                }
                return newObj;
            }
            return val;
        }
        throw new Error("Unexpected type when deserializing JSON: " + typ);
    }
    function inflatePublic(val, genArgs) {
        return inflate(val, genArgs ? genArgs.T : null, "");
    }
    exports.inflate = inflatePublic;
    function ofJson(json, genArgs) {
        return inflate(JSON.parse(json), genArgs ? genArgs.T : null, "");
    }
    exports.ofJson = ofJson;
    function toJsonWithTypeInfo(o) {
        return JSON.stringify(o, function (k, v) {
            if (ArrayBuffer.isView(v)) {
                return Array.from(v);
            }
            else if (v != null && typeof v === "object") {
                var typeName = typeof v[Symbol_1.default.reflection] === "function" ? v[Symbol_1.default.reflection]().type : null;
                if (v instanceof List_1.default || v instanceof Set_1.default || v instanceof Set) {
                    return {
                        $type: typeName || "System.Collections.Generic.HashSet",
                        $values: Array.from(v)
                    };
                }
                else if (v instanceof Map_1.default || v instanceof Map) {
                    return Seq_1.fold(function (o, kv) { o[kv[0]] = kv[1]; return o; }, { $type: typeName || "System.Collections.Generic.Dictionary" }, v);
                }
                else if (typeName) {
                    if (Util_1.hasInterface(v, "FSharpUnion") || Util_1.hasInterface(v, "FSharpRecord")) {
                        return Object.assign({ $type: typeName }, v);
                    }
                    else {
                        var proto = Object.getPrototypeOf(v), props = Object.getOwnPropertyNames(proto), o_1 = { $type: typeName };
                        for (var i = 0; i < props.length; i++) {
                            var prop = Object.getOwnPropertyDescriptor(proto, props[i]);
                            if (prop.get)
                                o_1[props[i]] = prop.get.apply(v);
                        }
                        return o_1;
                    }
                }
            }
            return v;
        });
    }
    exports.toJsonWithTypeInfo = toJsonWithTypeInfo;
    function ofJsonWithTypeInfo(json, genArgs) {
        var parsed = JSON.parse(json, function (k, v) {
            if (v == null)
                return v;
            else if (typeof v === "object" && typeof v.$type === "string") {
                var type = v.$type.replace('+', '.'), i = type.indexOf('`');
                if (i > -1) {
                    type = type.substr(0, i);
                }
                else {
                    i = type.indexOf(',');
                    type = i > -1 ? type.substr(0, i) : type;
                }
                if (type === "System.Collections.Generic.List" || (type.indexOf("[]") === type.length - 2)) {
                    return v.$values;
                }
                if (type === "Microsoft.FSharp.Collections.FSharpList") {
                    return List_2.ofArray(v.$values);
                }
                else if (type == "Microsoft.FSharp.Collections.FSharpSet") {
                    return Set_2.create(v.$values);
                }
                else if (type == "System.Collections.Generic.HashSet") {
                    return new Set(v.$values);
                }
                else if (type == "Microsoft.FSharp.Collections.FSharpMap") {
                    delete v.$type;
                    return Map_2.create(Object.getOwnPropertyNames(v)
                        .map(function (k) { return [k, v[k]]; }));
                }
                else if (type == "System.Collections.Generic.Dictionary") {
                    delete v.$type;
                    return new Map(Object.getOwnPropertyNames(v)
                        .map(function (k) { return [k, v[k]]; }));
                }
                else {
                    var T = Symbol_2.getType(type);
                    if (T) {
                        delete v.$type;
                        return Object.assign(new T(), v);
                    }
                }
            }
            else if (/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:[+-]\d{2}:\d{2}|Z)$/.test(v))
                return Date_1.parse(v);
            else
                return v;
        });
        var expected = genArgs ? genArgs.T : null;
        if (parsed != null && typeof expected === "function"
            && !(parsed instanceof Util_2.getDefinition(expected))) {
            throw new Error("JSON is not of type " + expected.name + ": " + json);
        }
        return parsed;
    }
    exports.ofJsonWithTypeInfo = ofJsonWithTypeInfo;
});
