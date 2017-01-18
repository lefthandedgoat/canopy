(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
    }
})(["require", "exports", "./Util", "./List", "./Symbol"], function (require, exports) {
    "use strict";
    var Util_1 = require("./Util");
    var List_1 = require("./List");
    var Symbol_1 = require("./Symbol");
    function resolveGeneric(idx, enclosing) {
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
                else if (resolved instanceof Util_1.NonDeclaredType && resolved.kind === "GenericParam") {
                    return resolveGeneric(resolved.definition, enclosing.tail);
                }
                else {
                    return new List_1.default(resolved, enclosing);
                }
            }
        }
        catch (err) {
            throw new Error("Cannot resolve generic argument " + idx + ": " + err);
        }
    }
    exports.resolveGeneric = resolveGeneric;
    function getType(obj) {
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
    exports.getType = getType;
    function getTypeFullName(typ, option) {
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
        else if (typ instanceof Util_1.NonDeclaredType) {
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
            return trim(typeof proto[Symbol_1.default.reflection] === "function"
                ? proto[Symbol_1.default.reflection]().type : null, option);
        }
    }
    exports.getTypeFullName = getTypeFullName;
});
