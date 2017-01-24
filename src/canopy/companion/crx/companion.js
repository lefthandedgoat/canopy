"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.right = exports.left = exports.bottom = exports.top = exports.inputs = exports.border_padding = exports.border_width = exports.jq = exports.Result = exports.Element = exports.Self = undefined;

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

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
exports.cleanString = cleanString;
exports.lower = lower;
exports.howManyXPath = howManyXPath;
exports.howManyJQuery = howManyJQuery;
exports.suggestByXPathText = suggestByXPathText;
exports.suggestByCanopyText = suggestByCanopyText;
exports.suggestByName = suggestByName;
exports.suggestByPlaceholder = suggestByPlaceholder;
exports.suggestById = suggestById;
exports.suggestByValue = suggestByValue;
exports.suggestByCanopyValue = suggestByCanopyValue;
exports.suggestByClass = suggestByClass;
exports.suggestBySingleClass = suggestBySingleClass;
exports.suggestByHref = suggestByHref;
exports.suggestByTag = suggestByTag;
exports.suggest = suggest;
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

var _Seq = require("fable-core/umd/Seq");

var _List = require("fable-core/umd/List");

var _Set = require("fable-core/umd/Set");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

function _toConsumableArray(arr) { if (Array.isArray(arr)) { for (var i = 0, arr2 = Array(arr.length); i < arr.length; i++) { arr2[i] = arr[i]; } return arr2; } else { return Array.from(arr); } }

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var Self = exports.Self = function () {
  function Self(_self) {
    _classCallCheck(this, Self);

    this.Self = _self;
  }

  _createClass(Self, [{
    key: _Symbol3.default.reflection,
    value: function () {
      return {
        type: "Companion.Self",
        interfaces: ["FSharpRecord", "System.IEquatable"],
        properties: {
          Self: _Util.Any
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

var _Element = function () {
  function _Element(tag, _class, id, text, value, name, placeholder, href) {
    _classCallCheck(this, _Element);

    this.Tag = tag;
    this.Class = _class;
    this.Id = id;
    this.Text = text;
    this.Value = value;
    this.Name = name;
    this.Placeholder = placeholder;
    this.Href = href;
  }

  _createClass(_Element, [{
    key: _Symbol3.default.reflection,
    value: function () {
      return {
        type: "Companion.Element",
        interfaces: ["FSharpRecord", "System.IEquatable", "System.IComparable"],
        properties: {
          Tag: "string",
          Class: "string",
          Id: "string",
          Text: "string",
          Value: "string",
          Name: "string",
          Placeholder: "string",
          Href: "string"
        }
      };
    }
  }, {
    key: "Equals",
    value: function (other) {
      return (0, _Util.equalsRecords)(this, other);
    }
  }, {
    key: "CompareTo",
    value: function (other) {
      return (0, _Util.compareRecords)(this, other);
    }
  }]);

  return _Element;
}();

exports.Element = _Element;
(0, _Symbol2.setType)("Companion.Element", _Element);

var Result = exports.Result = function () {
  function Result(selector, readability, count) {
    _classCallCheck(this, Result);

    this.Selector = selector;
    this.Readability = readability;
    this.Count = count;
  }

  _createClass(Result, [{
    key: _Symbol3.default.reflection,
    value: function () {
      return {
        type: "Companion.Result",
        interfaces: ["FSharpRecord", "System.IEquatable", "System.IComparable"],
        properties: {
          Selector: "string",
          Readability: "number",
          Count: "number"
        }
      };
    }
  }, {
    key: "Equals",
    value: function (other) {
      return (0, _Util.equalsRecords)(this, other);
    }
  }, {
    key: "CompareTo",
    value: function (other) {
      return (0, _Util.compareRecords)(this, other);
    }
  }]);

  return Result;
}();

(0, _Symbol2.setType)("Companion.Result", Result);
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

function cleanString(value_1) {
  var value_2 = (0, _Util.toString)(value_1);
  var value_3 = (0, _String.replace)(value_2, "'", "\\'");
  var value_4 = value_3 === "undefined" ? "" : value_3;
  var value_5 = value_4.indexOf("\n") >= 0 ? "" : value_4;
  return value_5;
}

function lower(value_1) {
  return value_1.toLocaleLowerCase();
}

var border_width = exports.border_width = 5;
var border_padding = exports.border_padding = 2;

function howManyXPath(selector) {
  var result = document.evaluate(selector, document, null, 0, null);
  return (0, _Seq.toList)((0, _Seq.delay)(function () {
    return (0, _Seq.enumerateWhile)(function () {
      return result.iterateNext() != null;
    }, (0, _Seq.delay)(function () {
      return (0, _Seq.singleton)(1);
    }));
  })).length;
}

function howManyJQuery(selector) {
  return find(selector).length;
}

function suggestByXPathText(element) {
  if (element.Text !== "") {
    var _ret = function () {
      var selector = (0, _String.fsFormat)("//%s[text()='%s']")(function (x) {
        return x;
      })(element.Tag)(element.Text);
      return {
        v: function () {
          var Count = howManyXPath(selector);
          return new Result(selector, 1.3, Count);
        }()
      };
    }();

    if ((typeof _ret === "undefined" ? "undefined" : _typeof(_ret)) === "object") return _ret.v;
  }
}

function suggestByCanopyText(element) {
  if (element.Text !== "") {
    var _ret2 = function () {
      var selector = (0, _String.fsFormat)("//*[text()='%s']")(function (x) {
        return x;
      })(element.Text);
      return {
        v: function () {
          var Count = howManyXPath(selector);
          return new Result(element.Text, 0.5, Count);
        }()
      };
    }();

    if ((typeof _ret2 === "undefined" ? "undefined" : _typeof(_ret2)) === "object") return _ret2.v;
  }
}

function suggestByName(element) {
  if (element.Name !== "") {
    var _ret3 = function () {
      var selector = (0, _String.fsFormat)("[name='%s']")(function (x) {
        return x;
      })(element.Name);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 1.2, Count);
        }()
      };
    }();

    if ((typeof _ret3 === "undefined" ? "undefined" : _typeof(_ret3)) === "object") return _ret3.v;
  }
}

function suggestByPlaceholder(element) {
  if (element.Placeholder !== "") {
    var _ret4 = function () {
      var selector = (0, _String.fsFormat)("[placeholder='%s']")(function (x) {
        return x;
      })(element.Placeholder);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 1.2, Count);
        }()
      };
    }();

    if ((typeof _ret4 === "undefined" ? "undefined" : _typeof(_ret4)) === "object") return _ret4.v;
  }
}

function suggestById(element) {
  if (element.Id !== "") {
    var _ret5 = function () {
      var selector = (0, _String.fsFormat)("#%s")(function (x) {
        return x;
      })(element.Id);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 0.3, Count);
        }()
      };
    }();

    if ((typeof _ret5 === "undefined" ? "undefined" : _typeof(_ret5)) === "object") return _ret5.v;
  }
}

function suggestByValue(element) {
  if (element.Value !== "") {
    var _ret6 = function () {
      var selector = (0, _String.fsFormat)("[value='%s']")(function (x) {
        return x;
      })(element.Value);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 1, Count);
        }()
      };
    }();

    if ((typeof _ret6 === "undefined" ? "undefined" : _typeof(_ret6)) === "object") return _ret6.v;
  }
}

function suggestByCanopyValue(element) {
  if (element.Value !== "") {
    var _ret7 = function () {
      var selector = (0, _String.fsFormat)("[value='%s']")(function (x) {
        return x;
      })(element.Value);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(element.Value, 0.5, Count);
        }()
      };
    }();

    if ((typeof _ret7 === "undefined" ? "undefined" : _typeof(_ret7)) === "object") return _ret7.v;
  }
}

function suggestByClass(element) {
  if (element.Class !== "") {
    var _ret8 = function () {
      var classes = (0, _String.fsFormat)(".%s")(function (x) {
        return x;
      })(_String.join.apply(undefined, ["."].concat(_toConsumableArray((0, _String.split)(element.Class, " ")))));
      return {
        v: function () {
          var Count = howManyJQuery(classes);
          return new Result(classes, 1.5, Count);
        }()
      };
    }();

    if ((typeof _ret8 === "undefined" ? "undefined" : _typeof(_ret8)) === "object") return _ret8.v;
  }
}

function suggestBySingleClass(element) {
  if (element.Class !== "") {
    return (0, _List.ofArray)((0, _String.split)(element.Class, " ").map(function (class_) {
      return (0, _String.fsFormat)(".%s")(function (x) {
        return x;
      })(class_);
    }).map(function (class_) {
      return function () {
        var Count = howManyJQuery(class_);
        return new Result(class_, 1.2, Count);
      }();
    }));
  } else {
    return (0, _List.ofArray)([null]);
  }
}

function suggestByHref(element) {
  if (element.Href !== "") {
    var _ret9 = function () {
      var selector = (0, _String.fsFormat)("[href='%s']")(function (x) {
        return x;
      })(element.Href);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 1.2, Count);
        }()
      };
    }();

    if ((typeof _ret9 === "undefined" ? "undefined" : _typeof(_ret9)) === "object") return _ret9.v;
  }
}

function suggestByTag(element) {
  if (element.Tag !== "") {
    var _ret10 = function () {
      var selector = (0, _String.fsFormat)("%s")(function (x) {
        return x;
      })(element.Tag);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 1.2, Count);
        }()
      };
    }();

    if ((typeof _ret10 === "undefined" ? "undefined" : _typeof(_ret10)) === "object") return _ret10.v;
  }
}

function suggest(element) {
  return function (results) {
    if (results.length >= 5) {
      return (0, _Seq.toList)((0, _Seq.take)(5, results));
    } else {
      return results;
    }
  }((0, _Seq.toList)((0, _Seq.sortWith)(function (x, y) {
    return (0, _Util.compare)(function (tupledArg) {
      return [tupledArg[1].Count, tupledArg[0]];
    }(x), function (tupledArg) {
      return [tupledArg[1].Count, tupledArg[0]];
    }(y));
  }, (0, _List.map)(function (result) {
    return [result.Readability * result.Selector.length, result];
  }, (0, _Seq.toList)((0, _Set.distinctBy)(function (result) {
    return result.Selector;
  }, (0, _List.filter)(function (result) {
    return result.Count > 0;
  }, (0, _List.choose)(function (x) {
    return x;
  }, (0, _List.append)(suggestBySingleClass(element), (0, _List.ofArray)([suggestById(element), suggestByName(element), suggestByPlaceholder(element), suggestByCanopyText(element), suggestByXPathText(element), suggestByValue(element), suggestByCanopyValue(element), suggestByClass(element), suggestByHref(element), suggestByTag(element)]))))))))));
}

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
  var element = jq(event.data.Self);
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
  var _self = event.data.Self.get(0);

  var element = jq(_self);
  var tag_1 = tag(element);

  if (tag_1 !== "BODY" ? tag_1 !== "HTML" : false) {
    event.stopImmediatePropagation();
    blockClick(element);
    var element_1 = new _Element(lower(cleanString(_self.tagName)), cleanString(_self.className), cleanString(_self.id), cleanString(_self.textContext == null ? _self.innerText : _self.textContext), cleanString(_self.value), cleanString(_self.name), cleanString(_self.placeholder), cleanString(element.attr("href")));
    var suggestions = suggest(element_1);
    (0, _Seq.iterate)(function (tupledArg) {
      (0, _String.fsFormat)("score: %A / result %A")(function (x) {
        console.log(x);
      })(tupledArg[0])(tupledArg[1]);
    }, suggestions);
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