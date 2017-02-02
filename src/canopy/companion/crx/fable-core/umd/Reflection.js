import { NonDeclaredType } from "./Util";
import List from "./List";
import FSymbol from "./Symbol";
export function resolveGeneric(idx, enclosing) {
    try {
        var t = enclosing.head;
        if (t.generics == null) {
            return resolveGeneric(idx, enclosing.tail);
        }
        else {
            var name_1 = typeof idx === "string"
                ? idx : Object.getOwnPropertyNames(t.generics)[idx];
            var resolved = t.generics[name_1];
            if (resolved == null) {
                return resolveGeneric(idx, enclosing.tail);
            }
            else if (resolved instanceof NonDeclaredType && resolved.kind === "GenericParam") {
                return resolveGeneric(resolved.definition, enclosing.tail);
            }
            else {
                return new List(resolved, enclosing);
            }
        }
    }
    catch (err) {
        throw new Error("Cannot resolve generic argument " + idx + ": " + err);
    }
}
export function getType(obj) {
    var t = typeof obj;
    switch (t) {
        case "boolean":
        case "number":
        case "string":
        case "function":
            return t;
        default:
            return Object.getPrototypeOf(obj).constructor;
    }
}
export function getTypeFullName(typ, option) {
    function trim(fullName, option) {
        if (typeof fullName !== "string") {
            return "unknown";
        }
        if (option === "name") {
            var i = fullName.lastIndexOf('.');
            return fullName.substr(i + 1);
        }
        if (option === "namespace") {
            var i = fullName.lastIndexOf('.');
            return i > -1 ? fullName.substr(0, i) : "";
        }
        return fullName;
    }
    if (typeof typ === "string") {
        return typ;
    }
    else if (typ instanceof NonDeclaredType) {
        switch (typ.kind) {
            case "Unit":
                return "unit";
            case "Option":
                return getTypeFullName(typ.generics, option) + " option";
            case "Array":
                return getTypeFullName(typ.generics, option) + "[]";
            case "Tuple":
                return typ.generics.map(function (x) { return getTypeFullName(x, option); }).join(" * ");
            case "GenericParam":
            case "Interface":
                return typ.definition;
            case "Any":
            default:
                return "unknown";
        }
    }
    else {
        var proto = typ.prototype;
        return trim(typeof proto[FSymbol.reflection] === "function"
            ? proto[FSymbol.reflection]().type : null, option);
    }
}
