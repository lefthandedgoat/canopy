"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.right = exports.left = exports.bottom = exports.top = exports.inputs = exports.border_padding = exports.border_width = exports.jq = exports.Self = undefined;

var _createClass = function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; }();

exports.find = find;
exports.append = append;
exports.css = css;
exports.click = click;
exports.value = value;
exports.set = set;
exports.tag = tag;
exports.remove = remove;
exports.hide = hide;
exports.exists = exists;
exports.off = off;
exports.on = on;
exports.onEach = onEach;
exports.bool = bool;
exports.px = px;
exports.toInt = toInt;
exports.border = border;
exports.createBorders = createBorders;
exports.mouseEnter = mouseEnter;
exports.blockClick = blockClick;
exports.mouseDown = mouseDown;

var _Symbol2 = require("fable-core/umd/Symbol");

var _Symbol3 = _interopRequireDefault(_Symbol2);

var _Util = require("fable-core/umd/Util");

var _jquery = require("jquery");

var _jquery2 = _interopRequireDefault(_jquery);

var _String = require("fable-core/umd/String");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var Self = exports.Self = function () {
  function Self(_self) {
    _classCallCheck(this, Self);

    this.self = _self;
  }

  _createClass(Self, [{
    key: _Symbol3.default.reflection,
    value: function () {
      return {
        type: "Companion.Self",
        interfaces: ["FSharpRecord", "System.IEquatable"],
        properties: {
          self: _Util.Any
        }
      };
    }
  }, {
    key: "Equals",
    value: function (other) {
      return (0, _Util.equalsRecords)(this, other);
    }
  }]);

  return Self;
}();

(0, _Symbol2.setType)("Companion.Self", Self);
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

function set(selector, value_1) {
  find(selector).val(value_1);
}

function tag(element) {
  return (0, _Util.toString)(element.prop("tagName")).toLocaleUpperCase();
}

function remove(selector) {
  find(selector).remove();
}

function hide(selector) {
  find(selector).hide();
}

function exists(selector) {
  var value_1 = Number.parseInt((0, _String.fsFormat)("%O")(function (x) {
    return x;
  })(find(selector).length));
  return value_1 > 0;
}

function off(selector, event) {
  find(selector).off(event);
}

function on(element, event, f) {
  var element_1 = jq(element);
  element_1.on(event, new Self(element_1), f);
}

function onEach(selector, event, f) {
  var elements = find(selector);
  jq.each(elements, function (index, element) {
    on(element, event, f);
  });
}

function bool(whatever) {
  var matchValue = (0, _String.fsFormat)("%O")(function (x) {
    return x;
  })(whatever);

  if (matchValue === "true") {
    return true;
  } else {
    return false;
  }
}

function px(value_1) {
  return (0, _String.fsFormat)("%ipx")(function (x) {
    return x;
  })(value_1);
}

function toInt(value_1) {
  return Number.parseInt((0, _String.fsFormat)("%O")(function (x) {
    return x;
  })(value_1));
}

var border_width = exports.border_width = 5;
var border_padding = exports.border_padding = 2;
var inputs = exports.inputs = "\r\n<div id=\"canopy_companion\" class=\"canopy_companion_module\">\r\n  <input type=\"text\" id=\"selector\" class=\"canopy_companion_module\" value=\"\">\r\n  <input type=\"button\" id=\"go\" class=\"canopy_companion_module\" value=\"Go\">\r\n  <input type=\"button\" id=\"clear\" class=\"canopy_companion_module\" value=\"Clear\">\r\n  <input type=\"button\" id=\"close\" class=\"canopy_companion_module\" value=\"X\">\r\n</div>";
var top = exports.top = jq("<div>").addClass("canopy_companion_border").addClass("canopy_companion_border_top");
var bottom = exports.bottom = jq("<div>").addClass("canopy_companion_border").addClass("canopy_companion_border_bottom");
var left = exports.left = jq("<div>").addClass("canopy_companion_border").addClass("canopy_companion_border_left");
var right = exports.right = jq("<div>").addClass("canopy_companion_border").addClass("canopy_companion_border_right");

function border(position, heightValue, widthvalue, topValue, leftValue) {
  var element = find((0, _String.fsFormat)(".canopy_companion_border_%s")(function (x) {
    return x;
  })(position));
  element.css("height", px(heightValue)).css("width", px(widthvalue)).css("top", px(topValue)).css("left", px(leftValue)).show();
}

function createBorders(elements) {
  jq.each(elements, function (index, element) {
    var clone = jq(element);
    var position = clone.offset();
    var top_1 = toInt(position.top);
    var left_1 = toInt(position.left);
    var width = toInt(clone.outerWidth());
    var height = toInt(clone.outerHeight());
    border("top", border_width, width + border_padding * 2 + border_width * 2, top_1 - border_width - border_padding, left_1 - border_padding - border_width);
    border("bottom", border_width, width + border_padding * 2 + border_width * 2 - border_width, top_1 + height + border_padding, left_1 - border_padding - border_width);
    border("left", height + border_padding * 2, border_width, top_1 - border_padding, left_1 - border_padding - border_width);
    border("right", height + border_padding * 2, border_width, top_1 - border_padding, left_1 + width + border_padding);
  });
}

function mouseEnter(event) {
  var element = jq(event.data.self);
  var tag_1 = tag(element);

  if (tag_1 !== "BODY" ? tag_1 !== "HTML" : false) {
    event.stopImmediatePropagation();
    hide(".canopy_companion_border");
    createBorders(element);
  }
}

function blockClick(element) {
  var clone = jq(element);
  var position = clone.offset();
  var top_1 = toInt(position.top);
  var left_1 = toInt(position.left);
  var width = toInt(clone.outerWidth());
  var height = toInt(clone.outerHeight());
  var block = jq("<div>").css("position", "absolute").css("z-index", "9999999").css("width", px(width)).css("height", px(height)).css("top", px(top_1)).css("left", px(left_1)).css("background-color", "");
  append("body", block);
  window.setTimeout(function (_arg1) {
    return block.remove();
  }, 400);
}

function mouseDown(event) {
  var element = jq(event.data.self);
  var tag_1 = tag(element);

  if (tag_1 !== "BODY" ? tag_1 !== "HTML" : false) {
    event.stopImmediatePropagation();
    blockClick(element);
  }
}

if (!exists("#canopy_companion")) {
  append("body", top);
  append("body", bottom);
  append("body", left);
  append("body", right);
  onEach("*:not(.canopy_companion_module)", "mouseenter.canopy", function (event) {
    mouseEnter(event);
  });
  onEach("*:not(.canopy_companion_module)", "mousedown.canopy", function (event) {
    mouseDown(event);
  });
  append("body", inputs);
  click("#canopy_companion #go", function (_arg1) {
    var selector = value("#selector");
    hide(".canopy_companion_border");
    createBorders(find(selector));
  });
  click("#canopy_companion #clear", function (_arg2) {
    set("#selector", "");
    hide(".canopy_companion_border");
  });
  click("#canopy_companion #close", function (_arg3) {
    off("*:not(.canopy_companion_module)", "mouseenter.canopy");
    off("*:not(.canopy_companion_module)", "mousedown.canopy");
    remove(".canopy_companion_border");
    remove("#canopy_companion");
  });
}