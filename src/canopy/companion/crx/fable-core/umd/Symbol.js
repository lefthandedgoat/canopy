var fableGlobal = function () {
    var globalObj = typeof window !== "undefined" ? window
        : (typeof global !== "undefined" ? global
            : (typeof self !== "undefined" ? self : null));
    if (typeof globalObj.__FABLE_CORE__ === "undefined") {
        globalObj.__FABLE_CORE__ = {
            types: new Map(),
            symbols: {
                reflection: Symbol("reflection"),
            }
        };
    }
    return globalObj.__FABLE_CORE__;
}();
export function setType(fullName, cons) {
    fableGlobal.types.set(fullName, cons);
}
export function getType(fullName) {
    return fableGlobal.types.get(fullName);
}
export default (fableGlobal.symbols);
