"use strict";

Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.right = exports.left = exports.bottom = exports.top = exports.inputs = exports.border_padding = exports.border_width = exports.jq = exports.Result = exports.SelectorType = exports.Element = exports.Self = undefined;

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
exports.hasDigit = hasDigit;
exports.hrefStopAtDigits = hrefStopAtDigits;
exports.hrefStopAtQueryString = hrefStopAtQueryString;
exports.hrefStopAtHash = hrefStopAtHash;
exports.typeToString = typeToString;
exports.applyXPath = applyXPath;
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
exports.suggestByHrefStopAtDigit = suggestByHrefStopAtDigit;
exports.suggestByHrefStopAtQueryString = suggestByHrefStopAtQueryString;
exports.suggestByHrefStopAtHash = suggestByHrefStopAtHash;
exports.suggestByTag = suggestByTag;
exports.suggest = suggest;
exports.green_border = green_border;
exports.createGreenBorders = createGreenBorders;
exports.border = border;
exports.createBorders = createBorders;
exports.result = result;
exports.mouseEnter = mouseEnter;
exports.blockClick = blockClick;
exports.mouseDown = mouseDown;

var _Symbol2 = require("fable-core/umd/Symbol");

var _Symbol3 = _interopRequireDefault(_Symbol2);

var _Util = require("fable-core/umd/Util");

var _jquery = require("jquery");

var _jquery2 = _interopRequireDefault(_jquery);

var _String = require("fable-core/umd/String");

var _RegExp = require("fable-core/umd/RegExp");

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

var SelectorType = exports.SelectorType = function () {
  function SelectorType(caseName, fields) {
    _classCallCheck(this, SelectorType);

    this.Case = caseName;
    this.Fields = fields;
  }

  _createClass(SelectorType, [{
    key: _Symbol3.default.reflection,
    value: function () {
      return {
        type: "Companion.SelectorType",
        interfaces: ["FSharpUnion", "System.IEquatable", "System.IComparable"],
        cases: {
          Canopy: [],
          Css: [],
          JQuery: [],
          XPath: []
        }
      };
    }
  }, {
    key: "Equals",
    value: function (other) {
      return (0, _Util.equalsUnions)(this, other);
    }
  }, {
    key: "CompareTo",
    value: function (other) {
      return (0, _Util.compareUnions)(this, other);
    }
  }]);

  return SelectorType;
}();

(0, _Symbol2.setType)("Companion.SelectorType", SelectorType);

var Result = exports.Result = function () {
  function Result(selector, readability, count, type, applySelector) {
    _classCallCheck(this, Result);

    this.Selector = selector;
    this.Readability = readability;
    this.Count = count;
    this.Type = type;
    this.ApplySelector = applySelector;
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
          Count: "number",
          Type: SelectorType,
          ApplySelector: "string"
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
  var value_2 = value_1;
  var value_3 = value_2 == null ? "" : value_2;
  var value_4 = value_3 === "undefined" ? "" : value_3;
  var value_5 = value_4.indexOf("\n") >= 0 ? "" : value_4;
  var value_6 = value_5.indexOf("\r\n") >= 0 ? "" : value_5;
  return value_6;
}

function lower(value_1) {
  return value_1.toLocaleLowerCase();
}

function hasDigit(value_1) {
  return (0, _RegExp.isMatch)(value_1, "\\d");
}

function hrefStopAtDigits(href) {
  var parts = (0, _String.split)(href, "/");

  var index = function (array) {
    return (0, _Seq.tryFindIndex)(function (value_1) {
      return hasDigit(value_1);
    }, array);
  }(parts);

  if (index == null) {
    return href;
  } else {
    return function (parts_1) {
      return _String.join.apply(undefined, ["/"].concat(_toConsumableArray(parts_1)));
    }(function (array) {
      return array.slice(0, index);
    }(parts));
  }
}

function hrefStopAtQueryString(href) {
  if (href.indexOf("?") >= 0) {
    return (0, _String.split)(href, "?")[0];
  } else {
    return href;
  }
}

function hrefStopAtHash(href) {
  if (href.indexOf("#") >= 0) {
    return (0, _String.split)(href, "#")[0];
  } else {
    return href;
  }
}

function typeToString(type_) {
  if (type_.Case === "Css") {
    return "css";
  } else if (type_.Case === "JQuery") {
    return "jQuery";
  } else if (type_.Case === "Canopy") {
    return "canopy";
  } else {
    return "xpath";
  }
}

var border_width = exports.border_width = 5;
var border_padding = exports.border_padding = 2;

function applyXPath(selector) {
  var result = document.evaluate(selector, document, null, 0, null);
  var element = result.iterateNext();
  return Array.from((0, _Seq.delay)(function () {
    return (0, _Seq.enumerateWhile)(function () {
      return element != null;
    }, (0, _Seq.delay)(function () {
      return (0, _Seq.append)((0, _Seq.singleton)(element), (0, _Seq.delay)(function () {
        element = result.iterateNext();
        return (0, _Seq.empty)();
      }));
    }));
  }));
}

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
          return new Result(selector, 1.3, Count, new SelectorType("XPath", []), selector);
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
          return new Result(element.Text, 0.5, Count, new SelectorType("Canopy", []), selector);
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
          return new Result(selector, 1.2, Count, new SelectorType("Css", []), selector);
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
          return new Result(selector, 1.2, Count, new SelectorType("Css", []), selector);
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
          return new Result(selector, 0.3, Count, new SelectorType("Css", []), selector);
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
          return new Result(selector, 1, Count, new SelectorType("Css", []), selector);
        }()
      };
    }();

    if ((typeof _ret6 === "undefined" ? "undefined" : _typeof(_ret6)) === "object") return _ret6.v;
  }
}

function suggestByCanopyValue(element) {
  if (element.Value !== "") {
    var _ret7 = function () {
      var selector = (0, _String.fsFormat)("//*[@value='%s']")(function (x) {
        return x;
      })(element.Value);
      return {
        v: function () {
          var Count = howManyXPath(selector);
          return new Result(element.Value, 0.5, Count, new SelectorType("Canopy", []), selector);
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
          return new Result(classes, 1.5, Count, new SelectorType("Css", []), classes);
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
        return new Result(class_, 1.2, Count, new SelectorType("Css", []), class_);
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
          return new Result(selector, 1.2, Count, new SelectorType("Css", []), selector);
        }()
      };
    }();

    if ((typeof _ret9 === "undefined" ? "undefined" : _typeof(_ret9)) === "object") return _ret9.v;
  }
}

function suggestByHrefStopAtDigit(element) {
  var href = hrefStopAtDigits(element.Href);

  if (element.Href !== "" ? href !== element.Href : false) {
    var _ret10 = function () {
      var selector = (0, _String.fsFormat)("[href^='%s']")(function (x) {
        return x;
      })(href);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 1.3, Count, new SelectorType("Css", []), selector);
        }()
      };
    }();

    if ((typeof _ret10 === "undefined" ? "undefined" : _typeof(_ret10)) === "object") return _ret10.v;
  }
}

function suggestByHrefStopAtQueryString(element) {
  var href = hrefStopAtQueryString(element.Href);

  if (element.Href !== "" ? href !== element.Href : false) {
    var _ret11 = function () {
      var selector = (0, _String.fsFormat)("[href^='%s']")(function (x) {
        return x;
      })(href);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 1.3, Count, new SelectorType("Css", []), selector);
        }()
      };
    }();

    if ((typeof _ret11 === "undefined" ? "undefined" : _typeof(_ret11)) === "object") return _ret11.v;
  }
}

function suggestByHrefStopAtHash(element) {
  var href = hrefStopAtHash(element.Href);

  if (element.Href !== "" ? href !== element.Href : false) {
    var _ret12 = function () {
      var selector = (0, _String.fsFormat)("[href^='%s']")(function (x) {
        return x;
      })(href);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 1.3, Count, new SelectorType("Css", []), selector);
        }()
      };
    }();

    if ((typeof _ret12 === "undefined" ? "undefined" : _typeof(_ret12)) === "object") return _ret12.v;
  }
}

function suggestByTag(element) {
  if (element.Tag !== "") {
    var _ret13 = function () {
      var selector = (0, _String.fsFormat)("%s")(function (x) {
        return x;
      })(element.Tag);
      return {
        v: function () {
          var Count = howManyJQuery(selector);
          return new Result(selector, 1.2, Count, new SelectorType("Css", []), selector);
        }()
      };
    }();

    if ((typeof _ret13 === "undefined" ? "undefined" : _typeof(_ret13)) === "object") return _ret13.v;
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
  }, (0, _List.append)(suggestBySingleClass(element), (0, _List.ofArray)([suggestById(element), suggestByName(element), suggestByPlaceholder(element), suggestByCanopyText(element), suggestByXPathText(element), suggestByValue(element), suggestByCanopyValue(element), suggestByClass(element), suggestByHref(element), suggestByHrefStopAtDigit(element), suggestByHrefStopAtQueryString(element), suggestByHrefStopAtHash(element), suggestByTag(element)]))))))))));
}

var inputs = exports.inputs = "\r\n<div id=\"canopy_companion\" class=\"canopy_companion_module\">\r\n  <input type=\"button\" id=\"clear\" class=\"canopy_companion_module\" value=\"Clear\">\r\n  <input type=\"button\" id=\"close\" class=\"canopy_companion_module\" value=\"X\">\r\n</div>";
var top = exports.top = jq("<div>").addClass("canopy_companion_border canopy_companion_module").addClass("canopy_companion_border_top");
var bottom = exports.bottom = jq("<div>").addClass("canopy_companion_border canopy_companion_module").addClass("canopy_companion_border_bottom");
var left = exports.left = jq("<div>").addClass("canopy_companion_border canopy_companion_module").addClass("canopy_companion_border_left");
var right = exports.right = jq("<div>").addClass("canopy_companion_border canopy_companion_module").addClass("canopy_companion_border_right");

function green_border(heightValue, widthvalue, topValue, leftValue) {
  append("body", jq("<div>").addClass("canopy_companion_border").addClass("canopy_companion_border_green").addClass("canopy_companion_module").css("height", px(heightValue)).css("width", px(widthvalue)).css("top", px(topValue)).css("left", px(leftValue)));
}

function createGreenBorders(elements) {
  remove(".canopy_companion_border_green");
  jq.each(elements, function (index, element) {
    var clone = jq(element);
    var position = clone.offset();
    var top_1 = toInt(position.top);
    var left_1 = toInt(position.left);
    var width = toInt(clone.outerWidth());
    var height = toInt(clone.outerHeight());
    green_border(border_width, width + border_padding * 2 + border_width * 2, top_1 - border_width - border_padding, left_1 - border_padding - border_width);
    green_border(border_width, width + border_padding * 2 + border_width * 2 - border_width, top_1 + height + border_padding, left_1 - border_padding - border_width);
    green_border(height + border_padding * 2, border_width, top_1 - border_padding, left_1 - border_padding - border_width);
    green_border(height + border_padding * 2, border_width, top_1 - border_padding, left_1 + width + border_padding);
  });
}

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

function result(result_, index) {
  append("body", jq((0, _String.fsFormat)("<div>\r\n          selector: <span id=\"selector_%i\" class=\"canopy_companion_module\">\"%s\"</span> \r\n          count: %i \r\n          type: %A \r\n          <input class=\"canopy_companion_module\" type=\"button\" id=\"selector_copy_%i\" value=\"Copy\"> \r\n          <input class=\"canopy_companion_module\" type=\"button\" id=\"selector_apply_%i\" value=\"Apply\" data-selector=\"%s\" data-type=\"%s\">\r\n        </div>")(function (x) {
    return x;
  })(index)(result_.Selector)(result_.Count)(typeToString(result_.Type))(index)(index)(result_.ApplySelector)(typeToString(result_.Type))).addClass("canopy_companion_module").addClass("canopy_companion_result").css("bottom", px(40 + 33 * index)));
  find((0, _String.fsFormat)("#selector_copy_%i")(function (x) {
    return x;
  })(index)).on("click.canopy_selector_copy", function (_arg1) {
    var selector = (0, _Util.toString)(find((0, _String.fsFormat)("#selector_%i")(function (x) {
      return x;
    })(index)).text());
    append("body", jq("<textarea id='magic_textarea'>"));
    find("#magic_textarea").val(selector);
    find("#magic_textarea").select();
    document.execCommand("copy");
    remove("#magic_textarea");
  });
  find((0, _String.fsFormat)("#selector_apply_%i")(function (x) {
    return x;
  })(index)).on("click.canopy_selector_apply", function (_arg2) {
    var selector = (0, _Util.toString)(find((0, _String.fsFormat)("#selector_apply_%i")(function (x) {
      return x;
    })(index)).data("selector"));
    var type_ = (0, _Util.toString)(find((0, _String.fsFormat)("#selector_apply_%i")(function (x) {
      return x;
    })(index)).data("type"));

    var elements = function () {
      var $var1 = null;

      switch (type_) {
        case "css":
        case "jQuery":
          {
            $var1 = find((0, _String.fsFormat)("%s:not(.canopy_companion_module)")(function (x) {
              return x;
            })(selector));
            break;
          }

        case "canopy":
        case "xpath":
          {
            $var1 = applyXPath(selector);
            break;
          }

        default:
          {
            $var1 = null;
          }
      }

      return $var1;
    }();

    createGreenBorders(elements);
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
    var element_1 = new _Element(cleanString(lower(_self.tagName)), cleanString(_self.className), cleanString(_self.id), cleanString(_self.textContext == null ? _self.innerText : _self.textContext), cleanString(_self.value), cleanString(_self.name), cleanString(_self.placeholder), cleanString(element.attr("href")));
    remove(".canopy_companion_result");
    off("*", "click.selector_copy");
    off("*", "click.selector_apply");
    (0, _Seq.iterateIndexed)(function (index, tupledArg) {
      result(tupledArg[1], index);
    }, (0, _List.reverse)(suggest(element_1)));
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
  click("#canopy_companion #clear", function (_arg1) {
    off("*", "click.canopy_selector_apply");
    off("*", "click.canopy_selector_copy");
    remove(".canopy_companion_border_green");
    remove(".canopy_companion_result");
    hide(".canopy_companion_border");
  });
  click("#canopy_companion #close", function (_arg2) {
    off("*", "mouseenter.canopy");
    off("*", "mousedown.canopy");
    off("*", "click.canopy_selector_apply");
    off("*", "click.canopy_selector_copy");
    remove(".canopy_companion_border");
    remove(".canopy_companion_result");
    remove("#canopy_companion");
  });
}