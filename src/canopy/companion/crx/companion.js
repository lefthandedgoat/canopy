"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.detail = exports.go = exports.jq = undefined;

var _jquery = require("jquery");

var _jquery2 = _interopRequireDefault(_jquery);

var _String = require("fable-core/umd/String");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var jq = exports.jq = _jquery2.default;
var go = exports.go = jq("#go");
var detail = exports.detail = jq(".detail");
detail.click(function (_arg1) {
  (0, _String.fsFormat)("I have been clicked!!")(function (x) {
    console.log(x);
  });
});
go.click(function (_arg2) {
  var selector = jq("#selector").val();
  var elements = jq(selector);
  elements.css("background-color", "red");
});