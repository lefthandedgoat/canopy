"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.go = exports.helper = exports.jq = undefined;

var _jquery = require("jquery");

var _jquery2 = _interopRequireDefault(_jquery);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var jq = exports.jq = _jquery2.default;
var helper = exports.helper = "\r\n<div id=\"canopy_companion\" style=\"position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;\">\r\n  <input type=\"text\" id=\"selector\" value=\"\">\r\n  <input type=\"button\" id=\"go\" value=\"Go\">\r\n</div>";
jq("body").append(helper);
var go = exports.go = jq("#go");
go.click(function (_arg1) {
  var selector = jq("#selector").val();
  var elements = jq(selector);
  elements.css("background-color", "red");
});