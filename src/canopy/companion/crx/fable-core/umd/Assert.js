var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { equals } from "./Util";
var AssertionError = (function (_super) {
    __extends(AssertionError, _super);
    function AssertionError(msg, actual, expected) {
        var _this = _super.call(this, msg) || this;
        _this.actual = actual;
        _this.expected = expected;
        return _this;
    }
    return AssertionError;
}(Error));
export { AssertionError };
export function equal(actual, expected, msg) {
    if (!equals(actual, expected)) {
        throw new AssertionError(msg || "Expected: " + expected + " - Actual: " + actual, actual, expected);
    }
}
