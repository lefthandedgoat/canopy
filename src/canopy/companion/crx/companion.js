"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.inputs = exports.jq = undefined;
exports.find = find;
exports.append = append;
exports.css = css;
exports.click = click;
exports.value = value;
exports.remove = remove;
exports.killBorders = killBorders;

var _jquery = require("jquery");

var _jquery2 = _interopRequireDefault(_jquery);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var jq = exports.jq = _jquery2.default;

function find(selector) {
  return jq(selector);
}

function append(selector, element) {
  find(selector).append(element);
}

function css(selector, property, value) {
  find(selector).css(property, value);
}

function click(selector, f) {
  find(selector).click(f);
}

function value(selector) {
  return find(selector).val();
}

function remove(selector) {
  find(selector).remove();
}

var inputs = exports.inputs = "\r\n<div id=\"canopy_companion\" style=\"position: absolute; border: 1px solid black; bottom: 0px; right: 0px; margin: 3px; padding: 3px; background-color: white; z-index: 99999; font-size: 20px; font-family: monospace; font-weight: bold;\">\r\n  <input type=\"text\" id=\"selector\" value=\"\">\r\n  <input type=\"button\" id=\"go\" value=\"Go\">\r\n</div>";
append("body", inputs);
click("#go", function (_arg1) {
  var selector = value("#selector");
  css(selector, "background-color", "red");
});

function killBorders() {
  remove("#canopy_companion_border_top");
  remove("#canopy_companion_border_bottom");
  remove("#canopy_companion_border_left");
  remove("#canopy_companion_border_right");
}