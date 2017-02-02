import FableSymbol from "./Symbol";
import { getType } from "./Symbol";
import List from "./List";
import { ofArray as listOfArray } from "./List";
import FableSet from "./Set";
import FableMap from "./Map";
import { create as mapCreate } from "./Map";
import { create as setCreate } from "./Set";
import { hasInterface } from "./Util";
import { getDefinition } from "./Util";
import { NonDeclaredType } from "./Util";
import { fold } from "./Seq";
import { resolveGeneric, getTypeFullName } from "./Reflection";
import { parse as dateParse } from "./Date";
import { fsFormat } from "./String";
export function toJson(o) {
    return JSON.stringify(o, function (k, v) {
        if (ArrayBuffer.isView(v)) {
            return Array.from(v);
        }
        else if (v != null && typeof v === "object") {
            var properties = typeof v[FableSymbol.reflection] === "function" ? v[FableSymbol.reflection]().properties : null;
            if (v instanceof List || v instanceof FableSet || v instanceof Set) {
                return Array.from(v);
            }
            else if (v instanceof FableMap || v instanceof Map) {
                var stringKeys_1 = null;
                return fold(function (o, kv) {
                    if (stringKeys_1 === null) {
                        stringKeys_1 = typeof kv[0] === "string";
                    }
                    o[stringKeys_1 ? kv[0] : toJson(kv[0])] = kv[1];
                    return o;
                }, {}, v);
            }
            else if (!hasInterface(v, "FSharpRecord") && properties) {
                return fold(function (o, prop) {
                    return o[prop] = v[prop], o;
                }, {}, Object.getOwnPropertyNames(properties));
            }
            else if (hasInterface(v, "FSharpUnion")) {
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
function combine(path1, path2) {
    return typeof path2 === "number"
        ? path1 + "[" + path2 + "]"
        : (path1 ? path1 + "." : "") + path2;
}
function isNullable(typ) {
    if (typeof typ === "string") {
        return typ !== "boolean" && typ !== "number";
    }
    else if (typ instanceof NonDeclaredType) {
        return typ.kind !== "Array" && typ.kind !== "Tuple";
    }
    else {
        var info = typeof typ.prototype[FableSymbol.reflection] === "function"
            ? typ.prototype[FableSymbol.reflection]() : null;
        return info ? info.nullable : true;
    }
}
function invalidate(val, typ, path) {
    throw new Error(fsFormat("%A", val) + " " + (path ? "(" + path + ")" : "") + " is not of type " + getTypeFullName(typ));
}
function needsInflate(enclosing) {
    var typ = enclosing.head;
    if (typeof typ === "string") {
        return false;
    }
    if (typ instanceof NonDeclaredType) {
        switch (typ.kind) {
            case "Option":
            case "Array":
                return typ.definition != null || needsInflate(new List(typ.generics, enclosing));
            case "Tuple":
                return typ.generics.some(function (x) {
                    return needsInflate(new List(x, enclosing));
                });
            case "GenericParam":
                return needsInflate(resolveGeneric(typ.definition, enclosing.tail));
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
    var ar = [], li = new List(), cur = val, inf = needsInflate(enclosing);
    while (cur.tail != null) {
        ar.push(inf ? inflate(cur.head, enclosing, path) : cur.head);
        cur = cur.tail;
    }
    ar.reverse();
    for (var i = 0; i < ar.length; i++) {
        li = new List(ar[i], li);
    }
    return li;
}
function inflate(val, typ, path) {
    var enclosing = null;
    if (typ instanceof List) {
        enclosing = typ;
        typ = typ.head;
    }
    else {
        enclosing = new List(typ, new List());
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
    else if (typ instanceof NonDeclaredType) {
        switch (typ.kind) {
            case "Unit":
                return null;
            case "Option":
                return inflate(val, new List(typ.generics, enclosing), path);
            case "Array":
                if (typ.definition != null) {
                    return new typ.definition(val);
                }
                else {
                    return inflateArray(val, new List(typ.generics, enclosing), path);
                }
            case "Tuple":
                return typ.generics.map(function (x, i) {
                    return inflate(val[i], new List(x, enclosing), combine(path, i));
                });
            case "GenericParam":
                return inflate(val, resolveGeneric(typ.definition, enclosing.tail), path);
            case "GenericType":
                var def = typ.definition;
                if (def === List) {
                    return Array.isArray(val)
                        ? listOfArray(inflateArray(val, resolveGeneric(0, enclosing), path))
                        : inflateList(val, resolveGeneric(0, enclosing), path);
                }
                if (def === FableSet) {
                    return setCreate(inflateArray(val, resolveGeneric(0, enclosing), path));
                }
                if (def === Set) {
                    return new Set(inflateArray(val, resolveGeneric(0, enclosing), path));
                }
                if (def === FableMap) {
                    return mapCreate(inflateMap(val, resolveGeneric(0, enclosing), resolveGeneric(1, enclosing), path));
                }
                if (def === Map) {
                    return new Map(inflateMap(val, resolveGeneric(0, enclosing), resolveGeneric(1, enclosing), path));
                }
                return inflate(val, new List(typ.definition, enclosing), path);
            default:
                return val;
        }
    }
    else if (typeof typ === "function") {
        if (typ === Date) {
            return dateParse(val);
        }
        var info = typeof typ.prototype[FableSymbol.reflection] === "function" ? typ.prototype[FableSymbol.reflection]() : {};
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
                        uFields.push(inflate(fields[i], new List(fieldTypes[i], enclosing), combine(path, i)));
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
                newObj[k] = inflate(val[k], new List(properties[k], enclosing), combine(path, k));
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
export { inflatePublic as inflate };
export function ofJson(json, genArgs) {
    return inflate(JSON.parse(json), genArgs ? genArgs.T : null, "");
}
export function toJsonWithTypeInfo(o) {
    return JSON.stringify(o, function (k, v) {
        if (ArrayBuffer.isView(v)) {
            return Array.from(v);
        }
        else if (v != null && typeof v === "object") {
            var typeName = typeof v[FableSymbol.reflection] === "function" ? v[FableSymbol.reflection]().type : null;
            if (v instanceof List || v instanceof FableSet || v instanceof Set) {
                return {
                    $type: typeName || "System.Collections.Generic.HashSet",
                    $values: Array.from(v)
                };
            }
            else if (v instanceof FableMap || v instanceof Map) {
                return fold(function (o, kv) { o[kv[0]] = kv[1]; return o; }, { $type: typeName || "System.Collections.Generic.Dictionary" }, v);
            }
            else if (typeName) {
                if (hasInterface(v, "FSharpUnion") || hasInterface(v, "FSharpRecord")) {
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
export function ofJsonWithTypeInfo(json, genArgs) {
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
                return listOfArray(v.$values);
            }
            else if (type == "Microsoft.FSharp.Collections.FSharpSet") {
                return setCreate(v.$values);
            }
            else if (type == "System.Collections.Generic.HashSet") {
                return new Set(v.$values);
            }
            else if (type == "Microsoft.FSharp.Collections.FSharpMap") {
                delete v.$type;
                return mapCreate(Object.getOwnPropertyNames(v)
                    .map(function (k) { return [k, v[k]]; }));
            }
            else if (type == "System.Collections.Generic.Dictionary") {
                delete v.$type;
                return new Map(Object.getOwnPropertyNames(v)
                    .map(function (k) { return [k, v[k]]; }));
            }
            else {
                var T = getType(type);
                if (T) {
                    delete v.$type;
                    return Object.assign(new T(), v);
                }
            }
        }
        else if (/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:[+-]\d{2}:\d{2}|Z)$/.test(v))
            return dateParse(v);
        else
            return v;
    });
    var expected = genArgs ? genArgs.T : null;
    if (parsed != null && typeof expected === "function"
        && !(parsed instanceof getDefinition(expected))) {
        throw new Error("JSON is not of type " + expected.name + ": " + json);
    }
    return parsed;
}
