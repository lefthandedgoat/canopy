export function createFromValue(v) {
    return new Lazy(function () { return v; });
}
var Lazy = (function () {
    function Lazy(factory) {
        this.factory = factory;
        this.isValueCreated = false;
    }
    Object.defineProperty(Lazy.prototype, "value", {
        get: function () {
            if (!this.isValueCreated) {
                this.createdValue = this.factory();
                this.isValueCreated = true;
            }
            return this.createdValue;
        },
        enumerable: true,
        configurable: true
    });
    return Lazy;
}());
export default Lazy;
