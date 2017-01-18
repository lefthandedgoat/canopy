define(["exports", "fable-core/umd/Seq", "fable-core/umd/String"], function (exports, _Seq, _String) {
  "use strict";

  Object.defineProperty(exports, "__esModule", {
    value: true
  });
  exports.numbers = undefined;
  var numbers = exports.numbers = (0, _Seq.toList)((0, _Seq.range)(1, 13));
  (0, _Seq.iterate)(function (number) {
    (0, _String.fsFormat)("%i")(function (x) {
      console.log(x);
    })(number);
  }, numbers);
});
//# sourceMappingURL=companion.js.map