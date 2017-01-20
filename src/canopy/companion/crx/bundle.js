/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId])
/******/ 			return installedModules[moduleId].exports;
/******/
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			exports: {},
/******/ 			id: moduleId,
/******/ 			loaded: false
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.loaded = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(0);
/******/ })
/************************************************************************/
/******/ ([
/* 0 */
/***/ function(module, exports, __webpack_require__) {

	module.exports = __webpack_require__(1);


/***/ },
/* 1 */
/***/ function(module, exports, __webpack_require__) {

	"use strict";
	
	Object.defineProperty(exports, "__esModule", {
	  value: true
	});
	exports.detail = exports.go = exports.jq = undefined;
	
	var _jquery = __webpack_require__(2);
	
	var _jquery2 = _interopRequireDefault(_jquery);
	
	var _String = __webpack_require__(4);
	
	var _Seq = __webpack_require__(29);
	
	function _interopRequireDefault(obj) {
	  return obj && obj.__esModule ? obj : { default: obj };
	}
	
	var jq = exports.jq = _jquery2.default;
	var go = exports.go = jq("#go");
	var detail = exports.detail = jq(".detail");
	detail.click(function (_arg1) {
	  (0, _String.fsFormat)("I have been clicked")(function (x) {
	    console.log(x);
	  });
	});
	go.click(function (_arg2) {
	  var selector = jq("#selector").val();
	  var elements = jq(selector);
	  elements.css("background-color", "red");
	});
	(0, _Seq.iterate)(function (i) {
	  (0, _String.fsFormat)("%i")(function (x) {
	    console.log(x);
	  })(i);
	}, (0, _Seq.toList)((0, _Seq.range)(1, 10)));

/***/ },
/* 2 */
/***/ function(module, exports, __webpack_require__) {

	var __WEBPACK_AMD_DEFINE_ARRAY__, __WEBPACK_AMD_DEFINE_RESULT__;/* WEBPACK VAR INJECTION */(function(module) {"use strict";
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	/*!
	 * jQuery JavaScript Library v3.1.1
	 * https://jquery.com/
	 *
	 * Includes Sizzle.js
	 * https://sizzlejs.com/
	 *
	 * Copyright jQuery Foundation and other contributors
	 * Released under the MIT license
	 * https://jquery.org/license
	 *
	 * Date: 2016-09-22T22:30Z
	 */
	(function (global, factory) {
	
		"use strict";
	
		if (( false ? "undefined" : _typeof(module)) === "object" && _typeof(module.exports) === "object") {
	
			// For CommonJS and CommonJS-like environments where a proper `window`
			// is present, execute the factory and get jQuery.
			// For environments that do not have a `window` with a `document`
			// (such as Node.js), expose a factory as module.exports.
			// This accentuates the need for the creation of a real `window`.
			// e.g. var jQuery = require("jquery")(window);
			// See ticket #14549 for more info.
			module.exports = global.document ? factory(global, true) : function (w) {
				if (!w.document) {
					throw new Error("jQuery requires a window with a document");
				}
				return factory(w);
			};
		} else {
			factory(global);
		}
	
		// Pass this if window is not defined yet
	})(typeof window !== "undefined" ? window : undefined, function (window, noGlobal) {
	
		// Edge <= 12 - 13+, Firefox <=18 - 45+, IE 10 - 11, Safari 5.1 - 9+, iOS 6 - 9.1
		// throw exceptions when non-strict code (e.g., ASP.NET 4.5) accesses strict mode
		// arguments.callee.caller (trac-13335). But as of jQuery 3.0 (2016), strict mode should be common
		// enough that all such attempts are guarded in a try block.
		"use strict";
	
		var arr = [];
	
		var document = window.document;
	
		var getProto = Object.getPrototypeOf;
	
		var _slice = arr.slice;
	
		var concat = arr.concat;
	
		var push = arr.push;
	
		var indexOf = arr.indexOf;
	
		var class2type = {};
	
		var toString = class2type.toString;
	
		var hasOwn = class2type.hasOwnProperty;
	
		var fnToString = hasOwn.toString;
	
		var ObjectFunctionString = fnToString.call(Object);
	
		var support = {};
	
		function DOMEval(code, doc) {
			doc = doc || document;
	
			var script = doc.createElement("script");
	
			script.text = code;
			doc.head.appendChild(script).parentNode.removeChild(script);
		}
		/* global Symbol */
		// Defining this global in .eslintrc.json would create a danger of using the global
		// unguarded in another place, it seems safer to define global only for this module
	
	
		var version = "3.1.1",
	
	
		// Define a local copy of jQuery
		jQuery = function jQuery(selector, context) {
	
			// The jQuery object is actually just the init constructor 'enhanced'
			// Need init if jQuery is called (just allow error to be thrown if not included)
			return new jQuery.fn.init(selector, context);
		},
	
	
		// Support: Android <=4.0 only
		// Make sure we trim BOM and NBSP
		rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g,
	
	
		// Matches dashed string for camelizing
		rmsPrefix = /^-ms-/,
		    rdashAlpha = /-([a-z])/g,
	
	
		// Used by jQuery.camelCase as callback to replace()
		fcamelCase = function fcamelCase(all, letter) {
			return letter.toUpperCase();
		};
	
		jQuery.fn = jQuery.prototype = {
	
			// The current version of jQuery being used
			jquery: version,
	
			constructor: jQuery,
	
			// The default length of a jQuery object is 0
			length: 0,
	
			toArray: function toArray() {
				return _slice.call(this);
			},
	
			// Get the Nth element in the matched element set OR
			// Get the whole matched element set as a clean array
			get: function get(num) {
	
				// Return all the elements in a clean array
				if (num == null) {
					return _slice.call(this);
				}
	
				// Return just the one element from the set
				return num < 0 ? this[num + this.length] : this[num];
			},
	
			// Take an array of elements and push it onto the stack
			// (returning the new matched element set)
			pushStack: function pushStack(elems) {
	
				// Build a new jQuery matched element set
				var ret = jQuery.merge(this.constructor(), elems);
	
				// Add the old object onto the stack (as a reference)
				ret.prevObject = this;
	
				// Return the newly-formed element set
				return ret;
			},
	
			// Execute a callback for every element in the matched set.
			each: function each(callback) {
				return jQuery.each(this, callback);
			},
	
			map: function map(callback) {
				return this.pushStack(jQuery.map(this, function (elem, i) {
					return callback.call(elem, i, elem);
				}));
			},
	
			slice: function slice() {
				return this.pushStack(_slice.apply(this, arguments));
			},
	
			first: function first() {
				return this.eq(0);
			},
	
			last: function last() {
				return this.eq(-1);
			},
	
			eq: function eq(i) {
				var len = this.length,
				    j = +i + (i < 0 ? len : 0);
				return this.pushStack(j >= 0 && j < len ? [this[j]] : []);
			},
	
			end: function end() {
				return this.prevObject || this.constructor();
			},
	
			// For internal use only.
			// Behaves like an Array's method, not like a jQuery method.
			push: push,
			sort: arr.sort,
			splice: arr.splice
		};
	
		jQuery.extend = jQuery.fn.extend = function () {
			var options,
			    name,
			    src,
			    copy,
			    copyIsArray,
			    clone,
			    target = arguments[0] || {},
			    i = 1,
			    length = arguments.length,
			    deep = false;
	
			// Handle a deep copy situation
			if (typeof target === "boolean") {
				deep = target;
	
				// Skip the boolean and the target
				target = arguments[i] || {};
				i++;
			}
	
			// Handle case when target is a string or something (possible in deep copy)
			if ((typeof target === "undefined" ? "undefined" : _typeof(target)) !== "object" && !jQuery.isFunction(target)) {
				target = {};
			}
	
			// Extend jQuery itself if only one argument is passed
			if (i === length) {
				target = this;
				i--;
			}
	
			for (; i < length; i++) {
	
				// Only deal with non-null/undefined values
				if ((options = arguments[i]) != null) {
	
					// Extend the base object
					for (name in options) {
						src = target[name];
						copy = options[name];
	
						// Prevent never-ending loop
						if (target === copy) {
							continue;
						}
	
						// Recurse if we're merging plain objects or arrays
						if (deep && copy && (jQuery.isPlainObject(copy) || (copyIsArray = jQuery.isArray(copy)))) {
	
							if (copyIsArray) {
								copyIsArray = false;
								clone = src && jQuery.isArray(src) ? src : [];
							} else {
								clone = src && jQuery.isPlainObject(src) ? src : {};
							}
	
							// Never move original objects, clone them
							target[name] = jQuery.extend(deep, clone, copy);
	
							// Don't bring in undefined values
						} else if (copy !== undefined) {
							target[name] = copy;
						}
					}
				}
			}
	
			// Return the modified object
			return target;
		};
	
		jQuery.extend({
	
			// Unique for each copy of jQuery on the page
			expando: "jQuery" + (version + Math.random()).replace(/\D/g, ""),
	
			// Assume jQuery is ready without the ready module
			isReady: true,
	
			error: function error(msg) {
				throw new Error(msg);
			},
	
			noop: function noop() {},
	
			isFunction: function isFunction(obj) {
				return jQuery.type(obj) === "function";
			},
	
			isArray: Array.isArray,
	
			isWindow: function isWindow(obj) {
				return obj != null && obj === obj.window;
			},
	
			isNumeric: function isNumeric(obj) {
	
				// As of jQuery 3.0, isNumeric is limited to
				// strings and numbers (primitives or objects)
				// that can be coerced to finite numbers (gh-2662)
				var type = jQuery.type(obj);
				return (type === "number" || type === "string") &&
	
				// parseFloat NaNs numeric-cast false positives ("")
				// ...but misinterprets leading-number strings, particularly hex literals ("0x...")
				// subtraction forces infinities to NaN
				!isNaN(obj - parseFloat(obj));
			},
	
			isPlainObject: function isPlainObject(obj) {
				var proto, Ctor;
	
				// Detect obvious negatives
				// Use toString instead of jQuery.type to catch host objects
				if (!obj || toString.call(obj) !== "[object Object]") {
					return false;
				}
	
				proto = getProto(obj);
	
				// Objects with no prototype (e.g., `Object.create( null )`) are plain
				if (!proto) {
					return true;
				}
	
				// Objects with prototype are plain iff they were constructed by a global Object function
				Ctor = hasOwn.call(proto, "constructor") && proto.constructor;
				return typeof Ctor === "function" && fnToString.call(Ctor) === ObjectFunctionString;
			},
	
			isEmptyObject: function isEmptyObject(obj) {
	
				/* eslint-disable no-unused-vars */
				// See https://github.com/eslint/eslint/issues/6125
				var name;
	
				for (name in obj) {
					return false;
				}
				return true;
			},
	
			type: function type(obj) {
				if (obj == null) {
					return obj + "";
				}
	
				// Support: Android <=2.3 only (functionish RegExp)
				return (typeof obj === "undefined" ? "undefined" : _typeof(obj)) === "object" || typeof obj === "function" ? class2type[toString.call(obj)] || "object" : typeof obj === "undefined" ? "undefined" : _typeof(obj);
			},
	
			// Evaluates a script in a global context
			globalEval: function globalEval(code) {
				DOMEval(code);
			},
	
			// Convert dashed to camelCase; used by the css and data modules
			// Support: IE <=9 - 11, Edge 12 - 13
			// Microsoft forgot to hump their vendor prefix (#9572)
			camelCase: function camelCase(string) {
				return string.replace(rmsPrefix, "ms-").replace(rdashAlpha, fcamelCase);
			},
	
			nodeName: function nodeName(elem, name) {
				return elem.nodeName && elem.nodeName.toLowerCase() === name.toLowerCase();
			},
	
			each: function each(obj, callback) {
				var length,
				    i = 0;
	
				if (isArrayLike(obj)) {
					length = obj.length;
					for (; i < length; i++) {
						if (callback.call(obj[i], i, obj[i]) === false) {
							break;
						}
					}
				} else {
					for (i in obj) {
						if (callback.call(obj[i], i, obj[i]) === false) {
							break;
						}
					}
				}
	
				return obj;
			},
	
			// Support: Android <=4.0 only
			trim: function trim(text) {
				return text == null ? "" : (text + "").replace(rtrim, "");
			},
	
			// results is for internal usage only
			makeArray: function makeArray(arr, results) {
				var ret = results || [];
	
				if (arr != null) {
					if (isArrayLike(Object(arr))) {
						jQuery.merge(ret, typeof arr === "string" ? [arr] : arr);
					} else {
						push.call(ret, arr);
					}
				}
	
				return ret;
			},
	
			inArray: function inArray(elem, arr, i) {
				return arr == null ? -1 : indexOf.call(arr, elem, i);
			},
	
			// Support: Android <=4.0 only, PhantomJS 1 only
			// push.apply(_, arraylike) throws on ancient WebKit
			merge: function merge(first, second) {
				var len = +second.length,
				    j = 0,
				    i = first.length;
	
				for (; j < len; j++) {
					first[i++] = second[j];
				}
	
				first.length = i;
	
				return first;
			},
	
			grep: function grep(elems, callback, invert) {
				var callbackInverse,
				    matches = [],
				    i = 0,
				    length = elems.length,
				    callbackExpect = !invert;
	
				// Go through the array, only saving the items
				// that pass the validator function
				for (; i < length; i++) {
					callbackInverse = !callback(elems[i], i);
					if (callbackInverse !== callbackExpect) {
						matches.push(elems[i]);
					}
				}
	
				return matches;
			},
	
			// arg is for internal usage only
			map: function map(elems, callback, arg) {
				var length,
				    value,
				    i = 0,
				    ret = [];
	
				// Go through the array, translating each of the items to their new values
				if (isArrayLike(elems)) {
					length = elems.length;
					for (; i < length; i++) {
						value = callback(elems[i], i, arg);
	
						if (value != null) {
							ret.push(value);
						}
					}
	
					// Go through every key on the object,
				} else {
					for (i in elems) {
						value = callback(elems[i], i, arg);
	
						if (value != null) {
							ret.push(value);
						}
					}
				}
	
				// Flatten any nested arrays
				return concat.apply([], ret);
			},
	
			// A global GUID counter for objects
			guid: 1,
	
			// Bind a function to a context, optionally partially applying any
			// arguments.
			proxy: function proxy(fn, context) {
				var tmp, args, proxy;
	
				if (typeof context === "string") {
					tmp = fn[context];
					context = fn;
					fn = tmp;
				}
	
				// Quick check to determine if target is callable, in the spec
				// this throws a TypeError, but we will just return undefined.
				if (!jQuery.isFunction(fn)) {
					return undefined;
				}
	
				// Simulated bind
				args = _slice.call(arguments, 2);
				proxy = function proxy() {
					return fn.apply(context || this, args.concat(_slice.call(arguments)));
				};
	
				// Set the guid of unique handler to the same of original handler, so it can be removed
				proxy.guid = fn.guid = fn.guid || jQuery.guid++;
	
				return proxy;
			},
	
			now: Date.now,
	
			// jQuery.support is not used in Core but other projects attach their
			// properties to it so it needs to exist.
			support: support
		});
	
		if (typeof Symbol === "function") {
			jQuery.fn[Symbol.iterator] = arr[Symbol.iterator];
		}
	
		// Populate the class2type map
		jQuery.each("Boolean Number String Function Array Date RegExp Object Error Symbol".split(" "), function (i, name) {
			class2type["[object " + name + "]"] = name.toLowerCase();
		});
	
		function isArrayLike(obj) {
	
			// Support: real iOS 8.2 only (not reproducible in simulator)
			// `in` check used to prevent JIT error (gh-2145)
			// hasOwn isn't used here due to false negatives
			// regarding Nodelist length in IE
			var length = !!obj && "length" in obj && obj.length,
			    type = jQuery.type(obj);
	
			if (type === "function" || jQuery.isWindow(obj)) {
				return false;
			}
	
			return type === "array" || length === 0 || typeof length === "number" && length > 0 && length - 1 in obj;
		}
		var Sizzle =
		/*!
	  * Sizzle CSS Selector Engine v2.3.3
	  * https://sizzlejs.com/
	  *
	  * Copyright jQuery Foundation and other contributors
	  * Released under the MIT license
	  * http://jquery.org/license
	  *
	  * Date: 2016-08-08
	  */
		function (window) {
	
			var i,
			    support,
			    Expr,
			    getText,
			    isXML,
			    tokenize,
			    compile,
			    select,
			    outermostContext,
			    sortInput,
			    hasDuplicate,
	
	
			// Local document vars
			setDocument,
			    document,
			    docElem,
			    documentIsHTML,
			    rbuggyQSA,
			    rbuggyMatches,
			    matches,
			    contains,
	
	
			// Instance-specific data
			expando = "sizzle" + 1 * new Date(),
			    preferredDoc = window.document,
			    dirruns = 0,
			    done = 0,
			    classCache = createCache(),
			    tokenCache = createCache(),
			    compilerCache = createCache(),
			    sortOrder = function sortOrder(a, b) {
				if (a === b) {
					hasDuplicate = true;
				}
				return 0;
			},
	
	
			// Instance methods
			hasOwn = {}.hasOwnProperty,
			    arr = [],
			    pop = arr.pop,
			    push_native = arr.push,
			    push = arr.push,
			    slice = arr.slice,
	
			// Use a stripped-down indexOf as it's faster than native
			// https://jsperf.com/thor-indexof-vs-for/5
			indexOf = function indexOf(list, elem) {
				var i = 0,
				    len = list.length;
				for (; i < len; i++) {
					if (list[i] === elem) {
						return i;
					}
				}
				return -1;
			},
			    booleans = "checked|selected|async|autofocus|autoplay|controls|defer|disabled|hidden|ismap|loop|multiple|open|readonly|required|scoped",
	
	
			// Regular expressions
	
			// http://www.w3.org/TR/css3-selectors/#whitespace
			whitespace = "[\\x20\\t\\r\\n\\f]",
	
	
			// http://www.w3.org/TR/CSS21/syndata.html#value-def-identifier
			identifier = "(?:\\\\.|[\\w-]|[^\0-\\xa0])+",
	
	
			// Attribute selectors: http://www.w3.org/TR/selectors/#attribute-selectors
			attributes = "\\[" + whitespace + "*(" + identifier + ")(?:" + whitespace +
			// Operator (capture 2)
			"*([*^$|!~]?=)" + whitespace +
			// "Attribute values must be CSS identifiers [capture 5] or strings [capture 3 or capture 4]"
			"*(?:'((?:\\\\.|[^\\\\'])*)'|\"((?:\\\\.|[^\\\\\"])*)\"|(" + identifier + "))|)" + whitespace + "*\\]",
			    pseudos = ":(" + identifier + ")(?:\\((" +
			// To reduce the number of selectors needing tokenize in the preFilter, prefer arguments:
			// 1. quoted (capture 3; capture 4 or capture 5)
			"('((?:\\\\.|[^\\\\'])*)'|\"((?:\\\\.|[^\\\\\"])*)\")|" +
			// 2. simple (capture 6)
			"((?:\\\\.|[^\\\\()[\\]]|" + attributes + ")*)|" +
			// 3. anything else (capture 2)
			".*" + ")\\)|)",
	
	
			// Leading and non-escaped trailing whitespace, capturing some non-whitespace characters preceding the latter
			rwhitespace = new RegExp(whitespace + "+", "g"),
			    rtrim = new RegExp("^" + whitespace + "+|((?:^|[^\\\\])(?:\\\\.)*)" + whitespace + "+$", "g"),
			    rcomma = new RegExp("^" + whitespace + "*," + whitespace + "*"),
			    rcombinators = new RegExp("^" + whitespace + "*([>+~]|" + whitespace + ")" + whitespace + "*"),
			    rattributeQuotes = new RegExp("=" + whitespace + "*([^\\]'\"]*?)" + whitespace + "*\\]", "g"),
			    rpseudo = new RegExp(pseudos),
			    ridentifier = new RegExp("^" + identifier + "$"),
			    matchExpr = {
				"ID": new RegExp("^#(" + identifier + ")"),
				"CLASS": new RegExp("^\\.(" + identifier + ")"),
				"TAG": new RegExp("^(" + identifier + "|[*])"),
				"ATTR": new RegExp("^" + attributes),
				"PSEUDO": new RegExp("^" + pseudos),
				"CHILD": new RegExp("^:(only|first|last|nth|nth-last)-(child|of-type)(?:\\(" + whitespace + "*(even|odd|(([+-]|)(\\d*)n|)" + whitespace + "*(?:([+-]|)" + whitespace + "*(\\d+)|))" + whitespace + "*\\)|)", "i"),
				"bool": new RegExp("^(?:" + booleans + ")$", "i"),
				// For use in libraries implementing .is()
				// We use this for POS matching in `select`
				"needsContext": new RegExp("^" + whitespace + "*[>+~]|:(even|odd|eq|gt|lt|nth|first|last)(?:\\(" + whitespace + "*((?:-\\d)?\\d*)" + whitespace + "*\\)|)(?=[^-]|$)", "i")
			},
			    rinputs = /^(?:input|select|textarea|button)$/i,
			    rheader = /^h\d$/i,
			    rnative = /^[^{]+\{\s*\[native \w/,
	
	
			// Easily-parseable/retrievable ID or TAG or CLASS selectors
			rquickExpr = /^(?:#([\w-]+)|(\w+)|\.([\w-]+))$/,
			    rsibling = /[+~]/,
	
	
			// CSS escapes
			// http://www.w3.org/TR/CSS21/syndata.html#escaped-characters
			runescape = new RegExp("\\\\([\\da-f]{1,6}" + whitespace + "?|(" + whitespace + ")|.)", "ig"),
			    funescape = function funescape(_, escaped, escapedWhitespace) {
				var high = "0x" + escaped - 0x10000;
				// NaN means non-codepoint
				// Support: Firefox<24
				// Workaround erroneous numeric interpretation of +"0x"
				return high !== high || escapedWhitespace ? escaped : high < 0 ?
				// BMP codepoint
				String.fromCharCode(high + 0x10000) :
				// Supplemental Plane codepoint (surrogate pair)
				String.fromCharCode(high >> 10 | 0xD800, high & 0x3FF | 0xDC00);
			},
	
	
			// CSS string/identifier serialization
			// https://drafts.csswg.org/cssom/#common-serializing-idioms
			rcssescape = /([\0-\x1f\x7f]|^-?\d)|^-$|[^\0-\x1f\x7f-\uFFFF\w-]/g,
			    fcssescape = function fcssescape(ch, asCodePoint) {
				if (asCodePoint) {
	
					// U+0000 NULL becomes U+FFFD REPLACEMENT CHARACTER
					if (ch === "\0") {
						return "\uFFFD";
					}
	
					// Control characters and (dependent upon position) numbers get escaped as code points
					return ch.slice(0, -1) + "\\" + ch.charCodeAt(ch.length - 1).toString(16) + " ";
				}
	
				// Other potentially-special ASCII characters get backslash-escaped
				return "\\" + ch;
			},
	
	
			// Used for iframes
			// See setDocument()
			// Removing the function wrapper causes a "Permission Denied"
			// error in IE
			unloadHandler = function unloadHandler() {
				setDocument();
			},
			    disabledAncestor = addCombinator(function (elem) {
				return elem.disabled === true && ("form" in elem || "label" in elem);
			}, { dir: "parentNode", next: "legend" });
	
			// Optimize for push.apply( _, NodeList )
			try {
				push.apply(arr = slice.call(preferredDoc.childNodes), preferredDoc.childNodes);
				// Support: Android<4.0
				// Detect silently failing push.apply
				arr[preferredDoc.childNodes.length].nodeType;
			} catch (e) {
				push = { apply: arr.length ?
	
					// Leverage slice if possible
					function (target, els) {
						push_native.apply(target, slice.call(els));
					} :
	
					// Support: IE<9
					// Otherwise append directly
					function (target, els) {
						var j = target.length,
						    i = 0;
						// Can't trust NodeList.length
						while (target[j++] = els[i++]) {}
						target.length = j - 1;
					}
				};
			}
	
			function Sizzle(selector, context, results, seed) {
				var m,
				    i,
				    elem,
				    nid,
				    match,
				    groups,
				    newSelector,
				    newContext = context && context.ownerDocument,
	
	
				// nodeType defaults to 9, since context defaults to document
				nodeType = context ? context.nodeType : 9;
	
				results = results || [];
	
				// Return early from calls with invalid selector or context
				if (typeof selector !== "string" || !selector || nodeType !== 1 && nodeType !== 9 && nodeType !== 11) {
	
					return results;
				}
	
				// Try to shortcut find operations (as opposed to filters) in HTML documents
				if (!seed) {
	
					if ((context ? context.ownerDocument || context : preferredDoc) !== document) {
						setDocument(context);
					}
					context = context || document;
	
					if (documentIsHTML) {
	
						// If the selector is sufficiently simple, try using a "get*By*" DOM method
						// (excepting DocumentFragment context, where the methods don't exist)
						if (nodeType !== 11 && (match = rquickExpr.exec(selector))) {
	
							// ID selector
							if (m = match[1]) {
	
								// Document context
								if (nodeType === 9) {
									if (elem = context.getElementById(m)) {
	
										// Support: IE, Opera, Webkit
										// TODO: identify versions
										// getElementById can match elements by name instead of ID
										if (elem.id === m) {
											results.push(elem);
											return results;
										}
									} else {
										return results;
									}
	
									// Element context
								} else {
	
									// Support: IE, Opera, Webkit
									// TODO: identify versions
									// getElementById can match elements by name instead of ID
									if (newContext && (elem = newContext.getElementById(m)) && contains(context, elem) && elem.id === m) {
	
										results.push(elem);
										return results;
									}
								}
	
								// Type selector
							} else if (match[2]) {
								push.apply(results, context.getElementsByTagName(selector));
								return results;
	
								// Class selector
							} else if ((m = match[3]) && support.getElementsByClassName && context.getElementsByClassName) {
	
								push.apply(results, context.getElementsByClassName(m));
								return results;
							}
						}
	
						// Take advantage of querySelectorAll
						if (support.qsa && !compilerCache[selector + " "] && (!rbuggyQSA || !rbuggyQSA.test(selector))) {
	
							if (nodeType !== 1) {
								newContext = context;
								newSelector = selector;
	
								// qSA looks outside Element context, which is not what we want
								// Thanks to Andrew Dupont for this workaround technique
								// Support: IE <=8
								// Exclude object elements
							} else if (context.nodeName.toLowerCase() !== "object") {
	
								// Capture the context ID, setting it first if necessary
								if (nid = context.getAttribute("id")) {
									nid = nid.replace(rcssescape, fcssescape);
								} else {
									context.setAttribute("id", nid = expando);
								}
	
								// Prefix every selector in the list
								groups = tokenize(selector);
								i = groups.length;
								while (i--) {
									groups[i] = "#" + nid + " " + toSelector(groups[i]);
								}
								newSelector = groups.join(",");
	
								// Expand context for sibling selectors
								newContext = rsibling.test(selector) && testContext(context.parentNode) || context;
							}
	
							if (newSelector) {
								try {
									push.apply(results, newContext.querySelectorAll(newSelector));
									return results;
								} catch (qsaError) {} finally {
									if (nid === expando) {
										context.removeAttribute("id");
									}
								}
							}
						}
					}
				}
	
				// All others
				return select(selector.replace(rtrim, "$1"), context, results, seed);
			}
	
			/**
	   * Create key-value caches of limited size
	   * @returns {function(string, object)} Returns the Object data after storing it on itself with
	   *	property name the (space-suffixed) string and (if the cache is larger than Expr.cacheLength)
	   *	deleting the oldest entry
	   */
			function createCache() {
				var keys = [];
	
				function cache(key, value) {
					// Use (key + " ") to avoid collision with native prototype properties (see Issue #157)
					if (keys.push(key + " ") > Expr.cacheLength) {
						// Only keep the most recent entries
						delete cache[keys.shift()];
					}
					return cache[key + " "] = value;
				}
				return cache;
			}
	
			/**
	   * Mark a function for special use by Sizzle
	   * @param {Function} fn The function to mark
	   */
			function markFunction(fn) {
				fn[expando] = true;
				return fn;
			}
	
			/**
	   * Support testing using an element
	   * @param {Function} fn Passed the created element and returns a boolean result
	   */
			function assert(fn) {
				var el = document.createElement("fieldset");
	
				try {
					return !!fn(el);
				} catch (e) {
					return false;
				} finally {
					// Remove from its parent by default
					if (el.parentNode) {
						el.parentNode.removeChild(el);
					}
					// release memory in IE
					el = null;
				}
			}
	
			/**
	   * Adds the same handler for all of the specified attrs
	   * @param {String} attrs Pipe-separated list of attributes
	   * @param {Function} handler The method that will be applied
	   */
			function addHandle(attrs, handler) {
				var arr = attrs.split("|"),
				    i = arr.length;
	
				while (i--) {
					Expr.attrHandle[arr[i]] = handler;
				}
			}
	
			/**
	   * Checks document order of two siblings
	   * @param {Element} a
	   * @param {Element} b
	   * @returns {Number} Returns less than 0 if a precedes b, greater than 0 if a follows b
	   */
			function siblingCheck(a, b) {
				var cur = b && a,
				    diff = cur && a.nodeType === 1 && b.nodeType === 1 && a.sourceIndex - b.sourceIndex;
	
				// Use IE sourceIndex if available on both nodes
				if (diff) {
					return diff;
				}
	
				// Check if b follows a
				if (cur) {
					while (cur = cur.nextSibling) {
						if (cur === b) {
							return -1;
						}
					}
				}
	
				return a ? 1 : -1;
			}
	
			/**
	   * Returns a function to use in pseudos for input types
	   * @param {String} type
	   */
			function createInputPseudo(type) {
				return function (elem) {
					var name = elem.nodeName.toLowerCase();
					return name === "input" && elem.type === type;
				};
			}
	
			/**
	   * Returns a function to use in pseudos for buttons
	   * @param {String} type
	   */
			function createButtonPseudo(type) {
				return function (elem) {
					var name = elem.nodeName.toLowerCase();
					return (name === "input" || name === "button") && elem.type === type;
				};
			}
	
			/**
	   * Returns a function to use in pseudos for :enabled/:disabled
	   * @param {Boolean} disabled true for :disabled; false for :enabled
	   */
			function createDisabledPseudo(disabled) {
	
				// Known :disabled false positives: fieldset[disabled] > legend:nth-of-type(n+2) :can-disable
				return function (elem) {
	
					// Only certain elements can match :enabled or :disabled
					// https://html.spec.whatwg.org/multipage/scripting.html#selector-enabled
					// https://html.spec.whatwg.org/multipage/scripting.html#selector-disabled
					if ("form" in elem) {
	
						// Check for inherited disabledness on relevant non-disabled elements:
						// * listed form-associated elements in a disabled fieldset
						//   https://html.spec.whatwg.org/multipage/forms.html#category-listed
						//   https://html.spec.whatwg.org/multipage/forms.html#concept-fe-disabled
						// * option elements in a disabled optgroup
						//   https://html.spec.whatwg.org/multipage/forms.html#concept-option-disabled
						// All such elements have a "form" property.
						if (elem.parentNode && elem.disabled === false) {
	
							// Option elements defer to a parent optgroup if present
							if ("label" in elem) {
								if ("label" in elem.parentNode) {
									return elem.parentNode.disabled === disabled;
								} else {
									return elem.disabled === disabled;
								}
							}
	
							// Support: IE 6 - 11
							// Use the isDisabled shortcut property to check for disabled fieldset ancestors
							return elem.isDisabled === disabled ||
	
							// Where there is no isDisabled, check manually
							/* jshint -W018 */
							elem.isDisabled !== !disabled && disabledAncestor(elem) === disabled;
						}
	
						return elem.disabled === disabled;
	
						// Try to winnow out elements that can't be disabled before trusting the disabled property.
						// Some victims get caught in our net (label, legend, menu, track), but it shouldn't
						// even exist on them, let alone have a boolean value.
					} else if ("label" in elem) {
						return elem.disabled === disabled;
					}
	
					// Remaining elements are neither :enabled nor :disabled
					return false;
				};
			}
	
			/**
	   * Returns a function to use in pseudos for positionals
	   * @param {Function} fn
	   */
			function createPositionalPseudo(fn) {
				return markFunction(function (argument) {
					argument = +argument;
					return markFunction(function (seed, matches) {
						var j,
						    matchIndexes = fn([], seed.length, argument),
						    i = matchIndexes.length;
	
						// Match elements found at the specified indexes
						while (i--) {
							if (seed[j = matchIndexes[i]]) {
								seed[j] = !(matches[j] = seed[j]);
							}
						}
					});
				});
			}
	
			/**
	   * Checks a node for validity as a Sizzle context
	   * @param {Element|Object=} context
	   * @returns {Element|Object|Boolean} The input node if acceptable, otherwise a falsy value
	   */
			function testContext(context) {
				return context && typeof context.getElementsByTagName !== "undefined" && context;
			}
	
			// Expose support vars for convenience
			support = Sizzle.support = {};
	
			/**
	   * Detects XML nodes
	   * @param {Element|Object} elem An element or a document
	   * @returns {Boolean} True iff elem is a non-HTML XML node
	   */
			isXML = Sizzle.isXML = function (elem) {
				// documentElement is verified for cases where it doesn't yet exist
				// (such as loading iframes in IE - #4833)
				var documentElement = elem && (elem.ownerDocument || elem).documentElement;
				return documentElement ? documentElement.nodeName !== "HTML" : false;
			};
	
			/**
	   * Sets document-related variables once based on the current document
	   * @param {Element|Object} [doc] An element or document object to use to set the document
	   * @returns {Object} Returns the current document
	   */
			setDocument = Sizzle.setDocument = function (node) {
				var hasCompare,
				    subWindow,
				    doc = node ? node.ownerDocument || node : preferredDoc;
	
				// Return early if doc is invalid or already selected
				if (doc === document || doc.nodeType !== 9 || !doc.documentElement) {
					return document;
				}
	
				// Update global variables
				document = doc;
				docElem = document.documentElement;
				documentIsHTML = !isXML(document);
	
				// Support: IE 9-11, Edge
				// Accessing iframe documents after unload throws "permission denied" errors (jQuery #13936)
				if (preferredDoc !== document && (subWindow = document.defaultView) && subWindow.top !== subWindow) {
	
					// Support: IE 11, Edge
					if (subWindow.addEventListener) {
						subWindow.addEventListener("unload", unloadHandler, false);
	
						// Support: IE 9 - 10 only
					} else if (subWindow.attachEvent) {
						subWindow.attachEvent("onunload", unloadHandler);
					}
				}
	
				/* Attributes
	   ---------------------------------------------------------------------- */
	
				// Support: IE<8
				// Verify that getAttribute really returns attributes and not properties
				// (excepting IE8 booleans)
				support.attributes = assert(function (el) {
					el.className = "i";
					return !el.getAttribute("className");
				});
	
				/* getElement(s)By*
	   ---------------------------------------------------------------------- */
	
				// Check if getElementsByTagName("*") returns only elements
				support.getElementsByTagName = assert(function (el) {
					el.appendChild(document.createComment(""));
					return !el.getElementsByTagName("*").length;
				});
	
				// Support: IE<9
				support.getElementsByClassName = rnative.test(document.getElementsByClassName);
	
				// Support: IE<10
				// Check if getElementById returns elements by name
				// The broken getElementById methods don't pick up programmatically-set names,
				// so use a roundabout getElementsByName test
				support.getById = assert(function (el) {
					docElem.appendChild(el).id = expando;
					return !document.getElementsByName || !document.getElementsByName(expando).length;
				});
	
				// ID filter and find
				if (support.getById) {
					Expr.filter["ID"] = function (id) {
						var attrId = id.replace(runescape, funescape);
						return function (elem) {
							return elem.getAttribute("id") === attrId;
						};
					};
					Expr.find["ID"] = function (id, context) {
						if (typeof context.getElementById !== "undefined" && documentIsHTML) {
							var elem = context.getElementById(id);
							return elem ? [elem] : [];
						}
					};
				} else {
					Expr.filter["ID"] = function (id) {
						var attrId = id.replace(runescape, funescape);
						return function (elem) {
							var node = typeof elem.getAttributeNode !== "undefined" && elem.getAttributeNode("id");
							return node && node.value === attrId;
						};
					};
	
					// Support: IE 6 - 7 only
					// getElementById is not reliable as a find shortcut
					Expr.find["ID"] = function (id, context) {
						if (typeof context.getElementById !== "undefined" && documentIsHTML) {
							var node,
							    i,
							    elems,
							    elem = context.getElementById(id);
	
							if (elem) {
	
								// Verify the id attribute
								node = elem.getAttributeNode("id");
								if (node && node.value === id) {
									return [elem];
								}
	
								// Fall back on getElementsByName
								elems = context.getElementsByName(id);
								i = 0;
								while (elem = elems[i++]) {
									node = elem.getAttributeNode("id");
									if (node && node.value === id) {
										return [elem];
									}
								}
							}
	
							return [];
						}
					};
				}
	
				// Tag
				Expr.find["TAG"] = support.getElementsByTagName ? function (tag, context) {
					if (typeof context.getElementsByTagName !== "undefined") {
						return context.getElementsByTagName(tag);
	
						// DocumentFragment nodes don't have gEBTN
					} else if (support.qsa) {
						return context.querySelectorAll(tag);
					}
				} : function (tag, context) {
					var elem,
					    tmp = [],
					    i = 0,
	
					// By happy coincidence, a (broken) gEBTN appears on DocumentFragment nodes too
					results = context.getElementsByTagName(tag);
	
					// Filter out possible comments
					if (tag === "*") {
						while (elem = results[i++]) {
							if (elem.nodeType === 1) {
								tmp.push(elem);
							}
						}
	
						return tmp;
					}
					return results;
				};
	
				// Class
				Expr.find["CLASS"] = support.getElementsByClassName && function (className, context) {
					if (typeof context.getElementsByClassName !== "undefined" && documentIsHTML) {
						return context.getElementsByClassName(className);
					}
				};
	
				/* QSA/matchesSelector
	   ---------------------------------------------------------------------- */
	
				// QSA and matchesSelector support
	
				// matchesSelector(:active) reports false when true (IE9/Opera 11.5)
				rbuggyMatches = [];
	
				// qSa(:focus) reports false when true (Chrome 21)
				// We allow this because of a bug in IE8/9 that throws an error
				// whenever `document.activeElement` is accessed on an iframe
				// So, we allow :focus to pass through QSA all the time to avoid the IE error
				// See https://bugs.jquery.com/ticket/13378
				rbuggyQSA = [];
	
				if (support.qsa = rnative.test(document.querySelectorAll)) {
					// Build QSA regex
					// Regex strategy adopted from Diego Perini
					assert(function (el) {
						// Select is set to empty string on purpose
						// This is to test IE's treatment of not explicitly
						// setting a boolean content attribute,
						// since its presence should be enough
						// https://bugs.jquery.com/ticket/12359
						docElem.appendChild(el).innerHTML = "<a id='" + expando + "'></a>" + "<select id='" + expando + "-\r\\' msallowcapture=''>" + "<option selected=''></option></select>";
	
						// Support: IE8, Opera 11-12.16
						// Nothing should be selected when empty strings follow ^= or $= or *=
						// The test attribute must be unknown in Opera but "safe" for WinRT
						// https://msdn.microsoft.com/en-us/library/ie/hh465388.aspx#attribute_section
						if (el.querySelectorAll("[msallowcapture^='']").length) {
							rbuggyQSA.push("[*^$]=" + whitespace + "*(?:''|\"\")");
						}
	
						// Support: IE8
						// Boolean attributes and "value" are not treated correctly
						if (!el.querySelectorAll("[selected]").length) {
							rbuggyQSA.push("\\[" + whitespace + "*(?:value|" + booleans + ")");
						}
	
						// Support: Chrome<29, Android<4.4, Safari<7.0+, iOS<7.0+, PhantomJS<1.9.8+
						if (!el.querySelectorAll("[id~=" + expando + "-]").length) {
							rbuggyQSA.push("~=");
						}
	
						// Webkit/Opera - :checked should return selected option elements
						// http://www.w3.org/TR/2011/REC-css3-selectors-20110929/#checked
						// IE8 throws error here and will not see later tests
						if (!el.querySelectorAll(":checked").length) {
							rbuggyQSA.push(":checked");
						}
	
						// Support: Safari 8+, iOS 8+
						// https://bugs.webkit.org/show_bug.cgi?id=136851
						// In-page `selector#id sibling-combinator selector` fails
						if (!el.querySelectorAll("a#" + expando + "+*").length) {
							rbuggyQSA.push(".#.+[+~]");
						}
					});
	
					assert(function (el) {
						el.innerHTML = "<a href='' disabled='disabled'></a>" + "<select disabled='disabled'><option/></select>";
	
						// Support: Windows 8 Native Apps
						// The type and name attributes are restricted during .innerHTML assignment
						var input = document.createElement("input");
						input.setAttribute("type", "hidden");
						el.appendChild(input).setAttribute("name", "D");
	
						// Support: IE8
						// Enforce case-sensitivity of name attribute
						if (el.querySelectorAll("[name=d]").length) {
							rbuggyQSA.push("name" + whitespace + "*[*^$|!~]?=");
						}
	
						// FF 3.5 - :enabled/:disabled and hidden elements (hidden elements are still enabled)
						// IE8 throws error here and will not see later tests
						if (el.querySelectorAll(":enabled").length !== 2) {
							rbuggyQSA.push(":enabled", ":disabled");
						}
	
						// Support: IE9-11+
						// IE's :disabled selector does not pick up the children of disabled fieldsets
						docElem.appendChild(el).disabled = true;
						if (el.querySelectorAll(":disabled").length !== 2) {
							rbuggyQSA.push(":enabled", ":disabled");
						}
	
						// Opera 10-11 does not throw on post-comma invalid pseudos
						el.querySelectorAll("*,:x");
						rbuggyQSA.push(",.*:");
					});
				}
	
				if (support.matchesSelector = rnative.test(matches = docElem.matches || docElem.webkitMatchesSelector || docElem.mozMatchesSelector || docElem.oMatchesSelector || docElem.msMatchesSelector)) {
	
					assert(function (el) {
						// Check to see if it's possible to do matchesSelector
						// on a disconnected node (IE 9)
						support.disconnectedMatch = matches.call(el, "*");
	
						// This should fail with an exception
						// Gecko does not error, returns false instead
						matches.call(el, "[s!='']:x");
						rbuggyMatches.push("!=", pseudos);
					});
				}
	
				rbuggyQSA = rbuggyQSA.length && new RegExp(rbuggyQSA.join("|"));
				rbuggyMatches = rbuggyMatches.length && new RegExp(rbuggyMatches.join("|"));
	
				/* Contains
	   ---------------------------------------------------------------------- */
				hasCompare = rnative.test(docElem.compareDocumentPosition);
	
				// Element contains another
				// Purposefully self-exclusive
				// As in, an element does not contain itself
				contains = hasCompare || rnative.test(docElem.contains) ? function (a, b) {
					var adown = a.nodeType === 9 ? a.documentElement : a,
					    bup = b && b.parentNode;
					return a === bup || !!(bup && bup.nodeType === 1 && (adown.contains ? adown.contains(bup) : a.compareDocumentPosition && a.compareDocumentPosition(bup) & 16));
				} : function (a, b) {
					if (b) {
						while (b = b.parentNode) {
							if (b === a) {
								return true;
							}
						}
					}
					return false;
				};
	
				/* Sorting
	   ---------------------------------------------------------------------- */
	
				// Document order sorting
				sortOrder = hasCompare ? function (a, b) {
	
					// Flag for duplicate removal
					if (a === b) {
						hasDuplicate = true;
						return 0;
					}
	
					// Sort on method existence if only one input has compareDocumentPosition
					var compare = !a.compareDocumentPosition - !b.compareDocumentPosition;
					if (compare) {
						return compare;
					}
	
					// Calculate position if both inputs belong to the same document
					compare = (a.ownerDocument || a) === (b.ownerDocument || b) ? a.compareDocumentPosition(b) :
	
					// Otherwise we know they are disconnected
					1;
	
					// Disconnected nodes
					if (compare & 1 || !support.sortDetached && b.compareDocumentPosition(a) === compare) {
	
						// Choose the first element that is related to our preferred document
						if (a === document || a.ownerDocument === preferredDoc && contains(preferredDoc, a)) {
							return -1;
						}
						if (b === document || b.ownerDocument === preferredDoc && contains(preferredDoc, b)) {
							return 1;
						}
	
						// Maintain original order
						return sortInput ? indexOf(sortInput, a) - indexOf(sortInput, b) : 0;
					}
	
					return compare & 4 ? -1 : 1;
				} : function (a, b) {
					// Exit early if the nodes are identical
					if (a === b) {
						hasDuplicate = true;
						return 0;
					}
	
					var cur,
					    i = 0,
					    aup = a.parentNode,
					    bup = b.parentNode,
					    ap = [a],
					    bp = [b];
	
					// Parentless nodes are either documents or disconnected
					if (!aup || !bup) {
						return a === document ? -1 : b === document ? 1 : aup ? -1 : bup ? 1 : sortInput ? indexOf(sortInput, a) - indexOf(sortInput, b) : 0;
	
						// If the nodes are siblings, we can do a quick check
					} else if (aup === bup) {
						return siblingCheck(a, b);
					}
	
					// Otherwise we need full lists of their ancestors for comparison
					cur = a;
					while (cur = cur.parentNode) {
						ap.unshift(cur);
					}
					cur = b;
					while (cur = cur.parentNode) {
						bp.unshift(cur);
					}
	
					// Walk down the tree looking for a discrepancy
					while (ap[i] === bp[i]) {
						i++;
					}
	
					return i ?
					// Do a sibling check if the nodes have a common ancestor
					siblingCheck(ap[i], bp[i]) :
	
					// Otherwise nodes in our document sort first
					ap[i] === preferredDoc ? -1 : bp[i] === preferredDoc ? 1 : 0;
				};
	
				return document;
			};
	
			Sizzle.matches = function (expr, elements) {
				return Sizzle(expr, null, null, elements);
			};
	
			Sizzle.matchesSelector = function (elem, expr) {
				// Set document vars if needed
				if ((elem.ownerDocument || elem) !== document) {
					setDocument(elem);
				}
	
				// Make sure that attribute selectors are quoted
				expr = expr.replace(rattributeQuotes, "='$1']");
	
				if (support.matchesSelector && documentIsHTML && !compilerCache[expr + " "] && (!rbuggyMatches || !rbuggyMatches.test(expr)) && (!rbuggyQSA || !rbuggyQSA.test(expr))) {
	
					try {
						var ret = matches.call(elem, expr);
	
						// IE 9's matchesSelector returns false on disconnected nodes
						if (ret || support.disconnectedMatch ||
						// As well, disconnected nodes are said to be in a document
						// fragment in IE 9
						elem.document && elem.document.nodeType !== 11) {
							return ret;
						}
					} catch (e) {}
				}
	
				return Sizzle(expr, document, null, [elem]).length > 0;
			};
	
			Sizzle.contains = function (context, elem) {
				// Set document vars if needed
				if ((context.ownerDocument || context) !== document) {
					setDocument(context);
				}
				return contains(context, elem);
			};
	
			Sizzle.attr = function (elem, name) {
				// Set document vars if needed
				if ((elem.ownerDocument || elem) !== document) {
					setDocument(elem);
				}
	
				var fn = Expr.attrHandle[name.toLowerCase()],
	
				// Don't get fooled by Object.prototype properties (jQuery #13807)
				val = fn && hasOwn.call(Expr.attrHandle, name.toLowerCase()) ? fn(elem, name, !documentIsHTML) : undefined;
	
				return val !== undefined ? val : support.attributes || !documentIsHTML ? elem.getAttribute(name) : (val = elem.getAttributeNode(name)) && val.specified ? val.value : null;
			};
	
			Sizzle.escape = function (sel) {
				return (sel + "").replace(rcssescape, fcssescape);
			};
	
			Sizzle.error = function (msg) {
				throw new Error("Syntax error, unrecognized expression: " + msg);
			};
	
			/**
	   * Document sorting and removing duplicates
	   * @param {ArrayLike} results
	   */
			Sizzle.uniqueSort = function (results) {
				var elem,
				    duplicates = [],
				    j = 0,
				    i = 0;
	
				// Unless we *know* we can detect duplicates, assume their presence
				hasDuplicate = !support.detectDuplicates;
				sortInput = !support.sortStable && results.slice(0);
				results.sort(sortOrder);
	
				if (hasDuplicate) {
					while (elem = results[i++]) {
						if (elem === results[i]) {
							j = duplicates.push(i);
						}
					}
					while (j--) {
						results.splice(duplicates[j], 1);
					}
				}
	
				// Clear input after sorting to release objects
				// See https://github.com/jquery/sizzle/pull/225
				sortInput = null;
	
				return results;
			};
	
			/**
	   * Utility function for retrieving the text value of an array of DOM nodes
	   * @param {Array|Element} elem
	   */
			getText = Sizzle.getText = function (elem) {
				var node,
				    ret = "",
				    i = 0,
				    nodeType = elem.nodeType;
	
				if (!nodeType) {
					// If no nodeType, this is expected to be an array
					while (node = elem[i++]) {
						// Do not traverse comment nodes
						ret += getText(node);
					}
				} else if (nodeType === 1 || nodeType === 9 || nodeType === 11) {
					// Use textContent for elements
					// innerText usage removed for consistency of new lines (jQuery #11153)
					if (typeof elem.textContent === "string") {
						return elem.textContent;
					} else {
						// Traverse its children
						for (elem = elem.firstChild; elem; elem = elem.nextSibling) {
							ret += getText(elem);
						}
					}
				} else if (nodeType === 3 || nodeType === 4) {
					return elem.nodeValue;
				}
				// Do not include comment or processing instruction nodes
	
				return ret;
			};
	
			Expr = Sizzle.selectors = {
	
				// Can be adjusted by the user
				cacheLength: 50,
	
				createPseudo: markFunction,
	
				match: matchExpr,
	
				attrHandle: {},
	
				find: {},
	
				relative: {
					">": { dir: "parentNode", first: true },
					" ": { dir: "parentNode" },
					"+": { dir: "previousSibling", first: true },
					"~": { dir: "previousSibling" }
				},
	
				preFilter: {
					"ATTR": function ATTR(match) {
						match[1] = match[1].replace(runescape, funescape);
	
						// Move the given value to match[3] whether quoted or unquoted
						match[3] = (match[3] || match[4] || match[5] || "").replace(runescape, funescape);
	
						if (match[2] === "~=") {
							match[3] = " " + match[3] + " ";
						}
	
						return match.slice(0, 4);
					},
	
					"CHILD": function CHILD(match) {
						/* matches from matchExpr["CHILD"]
	     	1 type (only|nth|...)
	     	2 what (child|of-type)
	     	3 argument (even|odd|\d*|\d*n([+-]\d+)?|...)
	     	4 xn-component of xn+y argument ([+-]?\d*n|)
	     	5 sign of xn-component
	     	6 x of xn-component
	     	7 sign of y-component
	     	8 y of y-component
	     */
						match[1] = match[1].toLowerCase();
	
						if (match[1].slice(0, 3) === "nth") {
							// nth-* requires argument
							if (!match[3]) {
								Sizzle.error(match[0]);
							}
	
							// numeric x and y parameters for Expr.filter.CHILD
							// remember that false/true cast respectively to 0/1
							match[4] = +(match[4] ? match[5] + (match[6] || 1) : 2 * (match[3] === "even" || match[3] === "odd"));
							match[5] = +(match[7] + match[8] || match[3] === "odd");
	
							// other types prohibit arguments
						} else if (match[3]) {
							Sizzle.error(match[0]);
						}
	
						return match;
					},
	
					"PSEUDO": function PSEUDO(match) {
						var excess,
						    unquoted = !match[6] && match[2];
	
						if (matchExpr["CHILD"].test(match[0])) {
							return null;
						}
	
						// Accept quoted arguments as-is
						if (match[3]) {
							match[2] = match[4] || match[5] || "";
	
							// Strip excess characters from unquoted arguments
						} else if (unquoted && rpseudo.test(unquoted) && (
						// Get excess from tokenize (recursively)
						excess = tokenize(unquoted, true)) && (
						// advance to the next closing parenthesis
						excess = unquoted.indexOf(")", unquoted.length - excess) - unquoted.length)) {
	
							// excess is a negative index
							match[0] = match[0].slice(0, excess);
							match[2] = unquoted.slice(0, excess);
						}
	
						// Return only captures needed by the pseudo filter method (type and argument)
						return match.slice(0, 3);
					}
				},
	
				filter: {
	
					"TAG": function TAG(nodeNameSelector) {
						var nodeName = nodeNameSelector.replace(runescape, funescape).toLowerCase();
						return nodeNameSelector === "*" ? function () {
							return true;
						} : function (elem) {
							return elem.nodeName && elem.nodeName.toLowerCase() === nodeName;
						};
					},
	
					"CLASS": function CLASS(className) {
						var pattern = classCache[className + " "];
	
						return pattern || (pattern = new RegExp("(^|" + whitespace + ")" + className + "(" + whitespace + "|$)")) && classCache(className, function (elem) {
							return pattern.test(typeof elem.className === "string" && elem.className || typeof elem.getAttribute !== "undefined" && elem.getAttribute("class") || "");
						});
					},
	
					"ATTR": function ATTR(name, operator, check) {
						return function (elem) {
							var result = Sizzle.attr(elem, name);
	
							if (result == null) {
								return operator === "!=";
							}
							if (!operator) {
								return true;
							}
	
							result += "";
	
							return operator === "=" ? result === check : operator === "!=" ? result !== check : operator === "^=" ? check && result.indexOf(check) === 0 : operator === "*=" ? check && result.indexOf(check) > -1 : operator === "$=" ? check && result.slice(-check.length) === check : operator === "~=" ? (" " + result.replace(rwhitespace, " ") + " ").indexOf(check) > -1 : operator === "|=" ? result === check || result.slice(0, check.length + 1) === check + "-" : false;
						};
					},
	
					"CHILD": function CHILD(type, what, argument, first, last) {
						var simple = type.slice(0, 3) !== "nth",
						    forward = type.slice(-4) !== "last",
						    ofType = what === "of-type";
	
						return first === 1 && last === 0 ?
	
						// Shortcut for :nth-*(n)
						function (elem) {
							return !!elem.parentNode;
						} : function (elem, context, xml) {
							var cache,
							    uniqueCache,
							    outerCache,
							    node,
							    nodeIndex,
							    start,
							    dir = simple !== forward ? "nextSibling" : "previousSibling",
							    parent = elem.parentNode,
							    name = ofType && elem.nodeName.toLowerCase(),
							    useCache = !xml && !ofType,
							    diff = false;
	
							if (parent) {
	
								// :(first|last|only)-(child|of-type)
								if (simple) {
									while (dir) {
										node = elem;
										while (node = node[dir]) {
											if (ofType ? node.nodeName.toLowerCase() === name : node.nodeType === 1) {
	
												return false;
											}
										}
										// Reverse direction for :only-* (if we haven't yet done so)
										start = dir = type === "only" && !start && "nextSibling";
									}
									return true;
								}
	
								start = [forward ? parent.firstChild : parent.lastChild];
	
								// non-xml :nth-child(...) stores cache data on `parent`
								if (forward && useCache) {
	
									// Seek `elem` from a previously-cached index
	
									// ...in a gzip-friendly way
									node = parent;
									outerCache = node[expando] || (node[expando] = {});
	
									// Support: IE <9 only
									// Defend against cloned attroperties (jQuery gh-1709)
									uniqueCache = outerCache[node.uniqueID] || (outerCache[node.uniqueID] = {});
	
									cache = uniqueCache[type] || [];
									nodeIndex = cache[0] === dirruns && cache[1];
									diff = nodeIndex && cache[2];
									node = nodeIndex && parent.childNodes[nodeIndex];
	
									while (node = ++nodeIndex && node && node[dir] || (
	
									// Fallback to seeking `elem` from the start
									diff = nodeIndex = 0) || start.pop()) {
	
										// When found, cache indexes on `parent` and break
										if (node.nodeType === 1 && ++diff && node === elem) {
											uniqueCache[type] = [dirruns, nodeIndex, diff];
											break;
										}
									}
								} else {
									// Use previously-cached element index if available
									if (useCache) {
										// ...in a gzip-friendly way
										node = elem;
										outerCache = node[expando] || (node[expando] = {});
	
										// Support: IE <9 only
										// Defend against cloned attroperties (jQuery gh-1709)
										uniqueCache = outerCache[node.uniqueID] || (outerCache[node.uniqueID] = {});
	
										cache = uniqueCache[type] || [];
										nodeIndex = cache[0] === dirruns && cache[1];
										diff = nodeIndex;
									}
	
									// xml :nth-child(...)
									// or :nth-last-child(...) or :nth(-last)?-of-type(...)
									if (diff === false) {
										// Use the same loop as above to seek `elem` from the start
										while (node = ++nodeIndex && node && node[dir] || (diff = nodeIndex = 0) || start.pop()) {
	
											if ((ofType ? node.nodeName.toLowerCase() === name : node.nodeType === 1) && ++diff) {
	
												// Cache the index of each encountered element
												if (useCache) {
													outerCache = node[expando] || (node[expando] = {});
	
													// Support: IE <9 only
													// Defend against cloned attroperties (jQuery gh-1709)
													uniqueCache = outerCache[node.uniqueID] || (outerCache[node.uniqueID] = {});
	
													uniqueCache[type] = [dirruns, diff];
												}
	
												if (node === elem) {
													break;
												}
											}
										}
									}
								}
	
								// Incorporate the offset, then check against cycle size
								diff -= last;
								return diff === first || diff % first === 0 && diff / first >= 0;
							}
						};
					},
	
					"PSEUDO": function PSEUDO(pseudo, argument) {
						// pseudo-class names are case-insensitive
						// http://www.w3.org/TR/selectors/#pseudo-classes
						// Prioritize by case sensitivity in case custom pseudos are added with uppercase letters
						// Remember that setFilters inherits from pseudos
						var args,
						    fn = Expr.pseudos[pseudo] || Expr.setFilters[pseudo.toLowerCase()] || Sizzle.error("unsupported pseudo: " + pseudo);
	
						// The user may use createPseudo to indicate that
						// arguments are needed to create the filter function
						// just as Sizzle does
						if (fn[expando]) {
							return fn(argument);
						}
	
						// But maintain support for old signatures
						if (fn.length > 1) {
							args = [pseudo, pseudo, "", argument];
							return Expr.setFilters.hasOwnProperty(pseudo.toLowerCase()) ? markFunction(function (seed, matches) {
								var idx,
								    matched = fn(seed, argument),
								    i = matched.length;
								while (i--) {
									idx = indexOf(seed, matched[i]);
									seed[idx] = !(matches[idx] = matched[i]);
								}
							}) : function (elem) {
								return fn(elem, 0, args);
							};
						}
	
						return fn;
					}
				},
	
				pseudos: {
					// Potentially complex pseudos
					"not": markFunction(function (selector) {
						// Trim the selector passed to compile
						// to avoid treating leading and trailing
						// spaces as combinators
						var input = [],
						    results = [],
						    matcher = compile(selector.replace(rtrim, "$1"));
	
						return matcher[expando] ? markFunction(function (seed, matches, context, xml) {
							var elem,
							    unmatched = matcher(seed, null, xml, []),
							    i = seed.length;
	
							// Match elements unmatched by `matcher`
							while (i--) {
								if (elem = unmatched[i]) {
									seed[i] = !(matches[i] = elem);
								}
							}
						}) : function (elem, context, xml) {
							input[0] = elem;
							matcher(input, null, xml, results);
							// Don't keep the element (issue #299)
							input[0] = null;
							return !results.pop();
						};
					}),
	
					"has": markFunction(function (selector) {
						return function (elem) {
							return Sizzle(selector, elem).length > 0;
						};
					}),
	
					"contains": markFunction(function (text) {
						text = text.replace(runescape, funescape);
						return function (elem) {
							return (elem.textContent || elem.innerText || getText(elem)).indexOf(text) > -1;
						};
					}),
	
					// "Whether an element is represented by a :lang() selector
					// is based solely on the element's language value
					// being equal to the identifier C,
					// or beginning with the identifier C immediately followed by "-".
					// The matching of C against the element's language value is performed case-insensitively.
					// The identifier C does not have to be a valid language name."
					// http://www.w3.org/TR/selectors/#lang-pseudo
					"lang": markFunction(function (lang) {
						// lang value must be a valid identifier
						if (!ridentifier.test(lang || "")) {
							Sizzle.error("unsupported lang: " + lang);
						}
						lang = lang.replace(runescape, funescape).toLowerCase();
						return function (elem) {
							var elemLang;
							do {
								if (elemLang = documentIsHTML ? elem.lang : elem.getAttribute("xml:lang") || elem.getAttribute("lang")) {
	
									elemLang = elemLang.toLowerCase();
									return elemLang === lang || elemLang.indexOf(lang + "-") === 0;
								}
							} while ((elem = elem.parentNode) && elem.nodeType === 1);
							return false;
						};
					}),
	
					// Miscellaneous
					"target": function target(elem) {
						var hash = window.location && window.location.hash;
						return hash && hash.slice(1) === elem.id;
					},
	
					"root": function root(elem) {
						return elem === docElem;
					},
	
					"focus": function focus(elem) {
						return elem === document.activeElement && (!document.hasFocus || document.hasFocus()) && !!(elem.type || elem.href || ~elem.tabIndex);
					},
	
					// Boolean properties
					"enabled": createDisabledPseudo(false),
					"disabled": createDisabledPseudo(true),
	
					"checked": function checked(elem) {
						// In CSS3, :checked should return both checked and selected elements
						// http://www.w3.org/TR/2011/REC-css3-selectors-20110929/#checked
						var nodeName = elem.nodeName.toLowerCase();
						return nodeName === "input" && !!elem.checked || nodeName === "option" && !!elem.selected;
					},
	
					"selected": function selected(elem) {
						// Accessing this property makes selected-by-default
						// options in Safari work properly
						if (elem.parentNode) {
							elem.parentNode.selectedIndex;
						}
	
						return elem.selected === true;
					},
	
					// Contents
					"empty": function empty(elem) {
						// http://www.w3.org/TR/selectors/#empty-pseudo
						// :empty is negated by element (1) or content nodes (text: 3; cdata: 4; entity ref: 5),
						//   but not by others (comment: 8; processing instruction: 7; etc.)
						// nodeType < 6 works because attributes (2) do not appear as children
						for (elem = elem.firstChild; elem; elem = elem.nextSibling) {
							if (elem.nodeType < 6) {
								return false;
							}
						}
						return true;
					},
	
					"parent": function parent(elem) {
						return !Expr.pseudos["empty"](elem);
					},
	
					// Element/input types
					"header": function header(elem) {
						return rheader.test(elem.nodeName);
					},
	
					"input": function input(elem) {
						return rinputs.test(elem.nodeName);
					},
	
					"button": function button(elem) {
						var name = elem.nodeName.toLowerCase();
						return name === "input" && elem.type === "button" || name === "button";
					},
	
					"text": function text(elem) {
						var attr;
						return elem.nodeName.toLowerCase() === "input" && elem.type === "text" && (
	
						// Support: IE<8
						// New HTML5 attribute values (e.g., "search") appear with elem.type === "text"
						(attr = elem.getAttribute("type")) == null || attr.toLowerCase() === "text");
					},
	
					// Position-in-collection
					"first": createPositionalPseudo(function () {
						return [0];
					}),
	
					"last": createPositionalPseudo(function (matchIndexes, length) {
						return [length - 1];
					}),
	
					"eq": createPositionalPseudo(function (matchIndexes, length, argument) {
						return [argument < 0 ? argument + length : argument];
					}),
	
					"even": createPositionalPseudo(function (matchIndexes, length) {
						var i = 0;
						for (; i < length; i += 2) {
							matchIndexes.push(i);
						}
						return matchIndexes;
					}),
	
					"odd": createPositionalPseudo(function (matchIndexes, length) {
						var i = 1;
						for (; i < length; i += 2) {
							matchIndexes.push(i);
						}
						return matchIndexes;
					}),
	
					"lt": createPositionalPseudo(function (matchIndexes, length, argument) {
						var i = argument < 0 ? argument + length : argument;
						for (; --i >= 0;) {
							matchIndexes.push(i);
						}
						return matchIndexes;
					}),
	
					"gt": createPositionalPseudo(function (matchIndexes, length, argument) {
						var i = argument < 0 ? argument + length : argument;
						for (; ++i < length;) {
							matchIndexes.push(i);
						}
						return matchIndexes;
					})
				}
			};
	
			Expr.pseudos["nth"] = Expr.pseudos["eq"];
	
			// Add button/input type pseudos
			for (i in { radio: true, checkbox: true, file: true, password: true, image: true }) {
				Expr.pseudos[i] = createInputPseudo(i);
			}
			for (i in { submit: true, reset: true }) {
				Expr.pseudos[i] = createButtonPseudo(i);
			}
	
			// Easy API for creating new setFilters
			function setFilters() {}
			setFilters.prototype = Expr.filters = Expr.pseudos;
			Expr.setFilters = new setFilters();
	
			tokenize = Sizzle.tokenize = function (selector, parseOnly) {
				var matched,
				    match,
				    tokens,
				    type,
				    soFar,
				    groups,
				    preFilters,
				    cached = tokenCache[selector + " "];
	
				if (cached) {
					return parseOnly ? 0 : cached.slice(0);
				}
	
				soFar = selector;
				groups = [];
				preFilters = Expr.preFilter;
	
				while (soFar) {
	
					// Comma and first run
					if (!matched || (match = rcomma.exec(soFar))) {
						if (match) {
							// Don't consume trailing commas as valid
							soFar = soFar.slice(match[0].length) || soFar;
						}
						groups.push(tokens = []);
					}
	
					matched = false;
	
					// Combinators
					if (match = rcombinators.exec(soFar)) {
						matched = match.shift();
						tokens.push({
							value: matched,
							// Cast descendant combinators to space
							type: match[0].replace(rtrim, " ")
						});
						soFar = soFar.slice(matched.length);
					}
	
					// Filters
					for (type in Expr.filter) {
						if ((match = matchExpr[type].exec(soFar)) && (!preFilters[type] || (match = preFilters[type](match)))) {
							matched = match.shift();
							tokens.push({
								value: matched,
								type: type,
								matches: match
							});
							soFar = soFar.slice(matched.length);
						}
					}
	
					if (!matched) {
						break;
					}
				}
	
				// Return the length of the invalid excess
				// if we're just parsing
				// Otherwise, throw an error or return tokens
				return parseOnly ? soFar.length : soFar ? Sizzle.error(selector) :
				// Cache the tokens
				tokenCache(selector, groups).slice(0);
			};
	
			function toSelector(tokens) {
				var i = 0,
				    len = tokens.length,
				    selector = "";
				for (; i < len; i++) {
					selector += tokens[i].value;
				}
				return selector;
			}
	
			function addCombinator(matcher, combinator, base) {
				var dir = combinator.dir,
				    skip = combinator.next,
				    key = skip || dir,
				    checkNonElements = base && key === "parentNode",
				    doneName = done++;
	
				return combinator.first ?
				// Check against closest ancestor/preceding element
				function (elem, context, xml) {
					while (elem = elem[dir]) {
						if (elem.nodeType === 1 || checkNonElements) {
							return matcher(elem, context, xml);
						}
					}
					return false;
				} :
	
				// Check against all ancestor/preceding elements
				function (elem, context, xml) {
					var oldCache,
					    uniqueCache,
					    outerCache,
					    newCache = [dirruns, doneName];
	
					// We can't set arbitrary data on XML nodes, so they don't benefit from combinator caching
					if (xml) {
						while (elem = elem[dir]) {
							if (elem.nodeType === 1 || checkNonElements) {
								if (matcher(elem, context, xml)) {
									return true;
								}
							}
						}
					} else {
						while (elem = elem[dir]) {
							if (elem.nodeType === 1 || checkNonElements) {
								outerCache = elem[expando] || (elem[expando] = {});
	
								// Support: IE <9 only
								// Defend against cloned attroperties (jQuery gh-1709)
								uniqueCache = outerCache[elem.uniqueID] || (outerCache[elem.uniqueID] = {});
	
								if (skip && skip === elem.nodeName.toLowerCase()) {
									elem = elem[dir] || elem;
								} else if ((oldCache = uniqueCache[key]) && oldCache[0] === dirruns && oldCache[1] === doneName) {
	
									// Assign to newCache so results back-propagate to previous elements
									return newCache[2] = oldCache[2];
								} else {
									// Reuse newcache so results back-propagate to previous elements
									uniqueCache[key] = newCache;
	
									// A match means we're done; a fail means we have to keep checking
									if (newCache[2] = matcher(elem, context, xml)) {
										return true;
									}
								}
							}
						}
					}
					return false;
				};
			}
	
			function elementMatcher(matchers) {
				return matchers.length > 1 ? function (elem, context, xml) {
					var i = matchers.length;
					while (i--) {
						if (!matchers[i](elem, context, xml)) {
							return false;
						}
					}
					return true;
				} : matchers[0];
			}
	
			function multipleContexts(selector, contexts, results) {
				var i = 0,
				    len = contexts.length;
				for (; i < len; i++) {
					Sizzle(selector, contexts[i], results);
				}
				return results;
			}
	
			function condense(unmatched, map, filter, context, xml) {
				var elem,
				    newUnmatched = [],
				    i = 0,
				    len = unmatched.length,
				    mapped = map != null;
	
				for (; i < len; i++) {
					if (elem = unmatched[i]) {
						if (!filter || filter(elem, context, xml)) {
							newUnmatched.push(elem);
							if (mapped) {
								map.push(i);
							}
						}
					}
				}
	
				return newUnmatched;
			}
	
			function setMatcher(preFilter, selector, matcher, postFilter, postFinder, postSelector) {
				if (postFilter && !postFilter[expando]) {
					postFilter = setMatcher(postFilter);
				}
				if (postFinder && !postFinder[expando]) {
					postFinder = setMatcher(postFinder, postSelector);
				}
				return markFunction(function (seed, results, context, xml) {
					var temp,
					    i,
					    elem,
					    preMap = [],
					    postMap = [],
					    preexisting = results.length,
	
	
					// Get initial elements from seed or context
					elems = seed || multipleContexts(selector || "*", context.nodeType ? [context] : context, []),
	
	
					// Prefilter to get matcher input, preserving a map for seed-results synchronization
					matcherIn = preFilter && (seed || !selector) ? condense(elems, preMap, preFilter, context, xml) : elems,
					    matcherOut = matcher ?
					// If we have a postFinder, or filtered seed, or non-seed postFilter or preexisting results,
					postFinder || (seed ? preFilter : preexisting || postFilter) ?
	
					// ...intermediate processing is necessary
					[] :
	
					// ...otherwise use results directly
					results : matcherIn;
	
					// Find primary matches
					if (matcher) {
						matcher(matcherIn, matcherOut, context, xml);
					}
	
					// Apply postFilter
					if (postFilter) {
						temp = condense(matcherOut, postMap);
						postFilter(temp, [], context, xml);
	
						// Un-match failing elements by moving them back to matcherIn
						i = temp.length;
						while (i--) {
							if (elem = temp[i]) {
								matcherOut[postMap[i]] = !(matcherIn[postMap[i]] = elem);
							}
						}
					}
	
					if (seed) {
						if (postFinder || preFilter) {
							if (postFinder) {
								// Get the final matcherOut by condensing this intermediate into postFinder contexts
								temp = [];
								i = matcherOut.length;
								while (i--) {
									if (elem = matcherOut[i]) {
										// Restore matcherIn since elem is not yet a final match
										temp.push(matcherIn[i] = elem);
									}
								}
								postFinder(null, matcherOut = [], temp, xml);
							}
	
							// Move matched elements from seed to results to keep them synchronized
							i = matcherOut.length;
							while (i--) {
								if ((elem = matcherOut[i]) && (temp = postFinder ? indexOf(seed, elem) : preMap[i]) > -1) {
	
									seed[temp] = !(results[temp] = elem);
								}
							}
						}
	
						// Add elements to results, through postFinder if defined
					} else {
						matcherOut = condense(matcherOut === results ? matcherOut.splice(preexisting, matcherOut.length) : matcherOut);
						if (postFinder) {
							postFinder(null, results, matcherOut, xml);
						} else {
							push.apply(results, matcherOut);
						}
					}
				});
			}
	
			function matcherFromTokens(tokens) {
				var checkContext,
				    matcher,
				    j,
				    len = tokens.length,
				    leadingRelative = Expr.relative[tokens[0].type],
				    implicitRelative = leadingRelative || Expr.relative[" "],
				    i = leadingRelative ? 1 : 0,
	
	
				// The foundational matcher ensures that elements are reachable from top-level context(s)
				matchContext = addCombinator(function (elem) {
					return elem === checkContext;
				}, implicitRelative, true),
				    matchAnyContext = addCombinator(function (elem) {
					return indexOf(checkContext, elem) > -1;
				}, implicitRelative, true),
				    matchers = [function (elem, context, xml) {
					var ret = !leadingRelative && (xml || context !== outermostContext) || ((checkContext = context).nodeType ? matchContext(elem, context, xml) : matchAnyContext(elem, context, xml));
					// Avoid hanging onto element (issue #299)
					checkContext = null;
					return ret;
				}];
	
				for (; i < len; i++) {
					if (matcher = Expr.relative[tokens[i].type]) {
						matchers = [addCombinator(elementMatcher(matchers), matcher)];
					} else {
						matcher = Expr.filter[tokens[i].type].apply(null, tokens[i].matches);
	
						// Return special upon seeing a positional matcher
						if (matcher[expando]) {
							// Find the next relative operator (if any) for proper handling
							j = ++i;
							for (; j < len; j++) {
								if (Expr.relative[tokens[j].type]) {
									break;
								}
							}
							return setMatcher(i > 1 && elementMatcher(matchers), i > 1 && toSelector(
							// If the preceding token was a descendant combinator, insert an implicit any-element `*`
							tokens.slice(0, i - 1).concat({ value: tokens[i - 2].type === " " ? "*" : "" })).replace(rtrim, "$1"), matcher, i < j && matcherFromTokens(tokens.slice(i, j)), j < len && matcherFromTokens(tokens = tokens.slice(j)), j < len && toSelector(tokens));
						}
						matchers.push(matcher);
					}
				}
	
				return elementMatcher(matchers);
			}
	
			function matcherFromGroupMatchers(elementMatchers, setMatchers) {
				var bySet = setMatchers.length > 0,
				    byElement = elementMatchers.length > 0,
				    superMatcher = function superMatcher(seed, context, xml, results, outermost) {
					var elem,
					    j,
					    matcher,
					    matchedCount = 0,
					    i = "0",
					    unmatched = seed && [],
					    setMatched = [],
					    contextBackup = outermostContext,
	
					// We must always have either seed elements or outermost context
					elems = seed || byElement && Expr.find["TAG"]("*", outermost),
	
					// Use integer dirruns iff this is the outermost matcher
					dirrunsUnique = dirruns += contextBackup == null ? 1 : Math.random() || 0.1,
					    len = elems.length;
	
					if (outermost) {
						outermostContext = context === document || context || outermost;
					}
	
					// Add elements passing elementMatchers directly to results
					// Support: IE<9, Safari
					// Tolerate NodeList properties (IE: "length"; Safari: <number>) matching elements by id
					for (; i !== len && (elem = elems[i]) != null; i++) {
						if (byElement && elem) {
							j = 0;
							if (!context && elem.ownerDocument !== document) {
								setDocument(elem);
								xml = !documentIsHTML;
							}
							while (matcher = elementMatchers[j++]) {
								if (matcher(elem, context || document, xml)) {
									results.push(elem);
									break;
								}
							}
							if (outermost) {
								dirruns = dirrunsUnique;
							}
						}
	
						// Track unmatched elements for set filters
						if (bySet) {
							// They will have gone through all possible matchers
							if (elem = !matcher && elem) {
								matchedCount--;
							}
	
							// Lengthen the array for every element, matched or not
							if (seed) {
								unmatched.push(elem);
							}
						}
					}
	
					// `i` is now the count of elements visited above, and adding it to `matchedCount`
					// makes the latter nonnegative.
					matchedCount += i;
	
					// Apply set filters to unmatched elements
					// NOTE: This can be skipped if there are no unmatched elements (i.e., `matchedCount`
					// equals `i`), unless we didn't visit _any_ elements in the above loop because we have
					// no element matchers and no seed.
					// Incrementing an initially-string "0" `i` allows `i` to remain a string only in that
					// case, which will result in a "00" `matchedCount` that differs from `i` but is also
					// numerically zero.
					if (bySet && i !== matchedCount) {
						j = 0;
						while (matcher = setMatchers[j++]) {
							matcher(unmatched, setMatched, context, xml);
						}
	
						if (seed) {
							// Reintegrate element matches to eliminate the need for sorting
							if (matchedCount > 0) {
								while (i--) {
									if (!(unmatched[i] || setMatched[i])) {
										setMatched[i] = pop.call(results);
									}
								}
							}
	
							// Discard index placeholder values to get only actual matches
							setMatched = condense(setMatched);
						}
	
						// Add matches to results
						push.apply(results, setMatched);
	
						// Seedless set matches succeeding multiple successful matchers stipulate sorting
						if (outermost && !seed && setMatched.length > 0 && matchedCount + setMatchers.length > 1) {
	
							Sizzle.uniqueSort(results);
						}
					}
	
					// Override manipulation of globals by nested matchers
					if (outermost) {
						dirruns = dirrunsUnique;
						outermostContext = contextBackup;
					}
	
					return unmatched;
				};
	
				return bySet ? markFunction(superMatcher) : superMatcher;
			}
	
			compile = Sizzle.compile = function (selector, match /* Internal Use Only */) {
				var i,
				    setMatchers = [],
				    elementMatchers = [],
				    cached = compilerCache[selector + " "];
	
				if (!cached) {
					// Generate a function of recursive functions that can be used to check each element
					if (!match) {
						match = tokenize(selector);
					}
					i = match.length;
					while (i--) {
						cached = matcherFromTokens(match[i]);
						if (cached[expando]) {
							setMatchers.push(cached);
						} else {
							elementMatchers.push(cached);
						}
					}
	
					// Cache the compiled function
					cached = compilerCache(selector, matcherFromGroupMatchers(elementMatchers, setMatchers));
	
					// Save selector and tokenization
					cached.selector = selector;
				}
				return cached;
			};
	
			/**
	   * A low-level selection function that works with Sizzle's compiled
	   *  selector functions
	   * @param {String|Function} selector A selector or a pre-compiled
	   *  selector function built with Sizzle.compile
	   * @param {Element} context
	   * @param {Array} [results]
	   * @param {Array} [seed] A set of elements to match against
	   */
			select = Sizzle.select = function (selector, context, results, seed) {
				var i,
				    tokens,
				    token,
				    type,
				    find,
				    compiled = typeof selector === "function" && selector,
				    match = !seed && tokenize(selector = compiled.selector || selector);
	
				results = results || [];
	
				// Try to minimize operations if there is only one selector in the list and no seed
				// (the latter of which guarantees us context)
				if (match.length === 1) {
	
					// Reduce context if the leading compound selector is an ID
					tokens = match[0] = match[0].slice(0);
					if (tokens.length > 2 && (token = tokens[0]).type === "ID" && context.nodeType === 9 && documentIsHTML && Expr.relative[tokens[1].type]) {
	
						context = (Expr.find["ID"](token.matches[0].replace(runescape, funescape), context) || [])[0];
						if (!context) {
							return results;
	
							// Precompiled matchers will still verify ancestry, so step up a level
						} else if (compiled) {
							context = context.parentNode;
						}
	
						selector = selector.slice(tokens.shift().value.length);
					}
	
					// Fetch a seed set for right-to-left matching
					i = matchExpr["needsContext"].test(selector) ? 0 : tokens.length;
					while (i--) {
						token = tokens[i];
	
						// Abort if we hit a combinator
						if (Expr.relative[type = token.type]) {
							break;
						}
						if (find = Expr.find[type]) {
							// Search, expanding context for leading sibling combinators
							if (seed = find(token.matches[0].replace(runescape, funescape), rsibling.test(tokens[0].type) && testContext(context.parentNode) || context)) {
	
								// If seed is empty or no tokens remain, we can return early
								tokens.splice(i, 1);
								selector = seed.length && toSelector(tokens);
								if (!selector) {
									push.apply(results, seed);
									return results;
								}
	
								break;
							}
						}
					}
				}
	
				// Compile and execute a filtering function if one is not provided
				// Provide `match` to avoid retokenization if we modified the selector above
				(compiled || compile(selector, match))(seed, context, !documentIsHTML, results, !context || rsibling.test(selector) && testContext(context.parentNode) || context);
				return results;
			};
	
			// One-time assignments
	
			// Sort stability
			support.sortStable = expando.split("").sort(sortOrder).join("") === expando;
	
			// Support: Chrome 14-35+
			// Always assume duplicates if they aren't passed to the comparison function
			support.detectDuplicates = !!hasDuplicate;
	
			// Initialize against the default document
			setDocument();
	
			// Support: Webkit<537.32 - Safari 6.0.3/Chrome 25 (fixed in Chrome 27)
			// Detached nodes confoundingly follow *each other*
			support.sortDetached = assert(function (el) {
				// Should return 1, but returns 4 (following)
				return el.compareDocumentPosition(document.createElement("fieldset")) & 1;
			});
	
			// Support: IE<8
			// Prevent attribute/property "interpolation"
			// https://msdn.microsoft.com/en-us/library/ms536429%28VS.85%29.aspx
			if (!assert(function (el) {
				el.innerHTML = "<a href='#'></a>";
				return el.firstChild.getAttribute("href") === "#";
			})) {
				addHandle("type|href|height|width", function (elem, name, isXML) {
					if (!isXML) {
						return elem.getAttribute(name, name.toLowerCase() === "type" ? 1 : 2);
					}
				});
			}
	
			// Support: IE<9
			// Use defaultValue in place of getAttribute("value")
			if (!support.attributes || !assert(function (el) {
				el.innerHTML = "<input/>";
				el.firstChild.setAttribute("value", "");
				return el.firstChild.getAttribute("value") === "";
			})) {
				addHandle("value", function (elem, name, isXML) {
					if (!isXML && elem.nodeName.toLowerCase() === "input") {
						return elem.defaultValue;
					}
				});
			}
	
			// Support: IE<9
			// Use getAttributeNode to fetch booleans when getAttribute lies
			if (!assert(function (el) {
				return el.getAttribute("disabled") == null;
			})) {
				addHandle(booleans, function (elem, name, isXML) {
					var val;
					if (!isXML) {
						return elem[name] === true ? name.toLowerCase() : (val = elem.getAttributeNode(name)) && val.specified ? val.value : null;
					}
				});
			}
	
			return Sizzle;
		}(window);
	
		jQuery.find = Sizzle;
		jQuery.expr = Sizzle.selectors;
	
		// Deprecated
		jQuery.expr[":"] = jQuery.expr.pseudos;
		jQuery.uniqueSort = jQuery.unique = Sizzle.uniqueSort;
		jQuery.text = Sizzle.getText;
		jQuery.isXMLDoc = Sizzle.isXML;
		jQuery.contains = Sizzle.contains;
		jQuery.escapeSelector = Sizzle.escape;
	
		var dir = function dir(elem, _dir, until) {
			var matched = [],
			    truncate = until !== undefined;
	
			while ((elem = elem[_dir]) && elem.nodeType !== 9) {
				if (elem.nodeType === 1) {
					if (truncate && jQuery(elem).is(until)) {
						break;
					}
					matched.push(elem);
				}
			}
			return matched;
		};
	
		var _siblings = function _siblings(n, elem) {
			var matched = [];
	
			for (; n; n = n.nextSibling) {
				if (n.nodeType === 1 && n !== elem) {
					matched.push(n);
				}
			}
	
			return matched;
		};
	
		var rneedsContext = jQuery.expr.match.needsContext;
	
		var rsingleTag = /^<([a-z][^\/\0>:\x20\t\r\n\f]*)[\x20\t\r\n\f]*\/?>(?:<\/\1>|)$/i;
	
		var risSimple = /^.[^:#\[\.,]*$/;
	
		// Implement the identical functionality for filter and not
		function winnow(elements, qualifier, not) {
			if (jQuery.isFunction(qualifier)) {
				return jQuery.grep(elements, function (elem, i) {
					return !!qualifier.call(elem, i, elem) !== not;
				});
			}
	
			// Single element
			if (qualifier.nodeType) {
				return jQuery.grep(elements, function (elem) {
					return elem === qualifier !== not;
				});
			}
	
			// Arraylike of elements (jQuery, arguments, Array)
			if (typeof qualifier !== "string") {
				return jQuery.grep(elements, function (elem) {
					return indexOf.call(qualifier, elem) > -1 !== not;
				});
			}
	
			// Simple selector that can be filtered directly, removing non-Elements
			if (risSimple.test(qualifier)) {
				return jQuery.filter(qualifier, elements, not);
			}
	
			// Complex selector, compare the two sets, removing non-Elements
			qualifier = jQuery.filter(qualifier, elements);
			return jQuery.grep(elements, function (elem) {
				return indexOf.call(qualifier, elem) > -1 !== not && elem.nodeType === 1;
			});
		}
	
		jQuery.filter = function (expr, elems, not) {
			var elem = elems[0];
	
			if (not) {
				expr = ":not(" + expr + ")";
			}
	
			if (elems.length === 1 && elem.nodeType === 1) {
				return jQuery.find.matchesSelector(elem, expr) ? [elem] : [];
			}
	
			return jQuery.find.matches(expr, jQuery.grep(elems, function (elem) {
				return elem.nodeType === 1;
			}));
		};
	
		jQuery.fn.extend({
			find: function find(selector) {
				var i,
				    ret,
				    len = this.length,
				    self = this;
	
				if (typeof selector !== "string") {
					return this.pushStack(jQuery(selector).filter(function () {
						for (i = 0; i < len; i++) {
							if (jQuery.contains(self[i], this)) {
								return true;
							}
						}
					}));
				}
	
				ret = this.pushStack([]);
	
				for (i = 0; i < len; i++) {
					jQuery.find(selector, self[i], ret);
				}
	
				return len > 1 ? jQuery.uniqueSort(ret) : ret;
			},
			filter: function filter(selector) {
				return this.pushStack(winnow(this, selector || [], false));
			},
			not: function not(selector) {
				return this.pushStack(winnow(this, selector || [], true));
			},
			is: function is(selector) {
				return !!winnow(this,
	
				// If this is a positional/relative selector, check membership in the returned set
				// so $("p:first").is("p:last") won't return true for a doc with two "p".
				typeof selector === "string" && rneedsContext.test(selector) ? jQuery(selector) : selector || [], false).length;
			}
		});
	
		// Initialize a jQuery object
	
	
		// A central reference to the root jQuery(document)
		var rootjQuery,
	
	
		// A simple way to check for HTML strings
		// Prioritize #id over <tag> to avoid XSS via location.hash (#9521)
		// Strict HTML recognition (#11290: must start with <)
		// Shortcut simple #id case for speed
		rquickExpr = /^(?:\s*(<[\w\W]+>)[^>]*|#([\w-]+))$/,
		    init = jQuery.fn.init = function (selector, context, root) {
			var match, elem;
	
			// HANDLE: $(""), $(null), $(undefined), $(false)
			if (!selector) {
				return this;
			}
	
			// Method init() accepts an alternate rootjQuery
			// so migrate can support jQuery.sub (gh-2101)
			root = root || rootjQuery;
	
			// Handle HTML strings
			if (typeof selector === "string") {
				if (selector[0] === "<" && selector[selector.length - 1] === ">" && selector.length >= 3) {
	
					// Assume that strings that start and end with <> are HTML and skip the regex check
					match = [null, selector, null];
				} else {
					match = rquickExpr.exec(selector);
				}
	
				// Match html or make sure no context is specified for #id
				if (match && (match[1] || !context)) {
	
					// HANDLE: $(html) -> $(array)
					if (match[1]) {
						context = context instanceof jQuery ? context[0] : context;
	
						// Option to run scripts is true for back-compat
						// Intentionally let the error be thrown if parseHTML is not present
						jQuery.merge(this, jQuery.parseHTML(match[1], context && context.nodeType ? context.ownerDocument || context : document, true));
	
						// HANDLE: $(html, props)
						if (rsingleTag.test(match[1]) && jQuery.isPlainObject(context)) {
							for (match in context) {
	
								// Properties of context are called as methods if possible
								if (jQuery.isFunction(this[match])) {
									this[match](context[match]);
	
									// ...and otherwise set as attributes
								} else {
									this.attr(match, context[match]);
								}
							}
						}
	
						return this;
	
						// HANDLE: $(#id)
					} else {
						elem = document.getElementById(match[2]);
	
						if (elem) {
	
							// Inject the element directly into the jQuery object
							this[0] = elem;
							this.length = 1;
						}
						return this;
					}
	
					// HANDLE: $(expr, $(...))
				} else if (!context || context.jquery) {
					return (context || root).find(selector);
	
					// HANDLE: $(expr, context)
					// (which is just equivalent to: $(context).find(expr)
				} else {
					return this.constructor(context).find(selector);
				}
	
				// HANDLE: $(DOMElement)
			} else if (selector.nodeType) {
				this[0] = selector;
				this.length = 1;
				return this;
	
				// HANDLE: $(function)
				// Shortcut for document ready
			} else if (jQuery.isFunction(selector)) {
				return root.ready !== undefined ? root.ready(selector) :
	
				// Execute immediately if ready is not present
				selector(jQuery);
			}
	
			return jQuery.makeArray(selector, this);
		};
	
		// Give the init function the jQuery prototype for later instantiation
		init.prototype = jQuery.fn;
	
		// Initialize central reference
		rootjQuery = jQuery(document);
	
		var rparentsprev = /^(?:parents|prev(?:Until|All))/,
	
	
		// Methods guaranteed to produce a unique set when starting from a unique set
		guaranteedUnique = {
			children: true,
			contents: true,
			next: true,
			prev: true
		};
	
		jQuery.fn.extend({
			has: function has(target) {
				var targets = jQuery(target, this),
				    l = targets.length;
	
				return this.filter(function () {
					var i = 0;
					for (; i < l; i++) {
						if (jQuery.contains(this, targets[i])) {
							return true;
						}
					}
				});
			},
	
			closest: function closest(selectors, context) {
				var cur,
				    i = 0,
				    l = this.length,
				    matched = [],
				    targets = typeof selectors !== "string" && jQuery(selectors);
	
				// Positional selectors never match, since there's no _selection_ context
				if (!rneedsContext.test(selectors)) {
					for (; i < l; i++) {
						for (cur = this[i]; cur && cur !== context; cur = cur.parentNode) {
	
							// Always skip document fragments
							if (cur.nodeType < 11 && (targets ? targets.index(cur) > -1 :
	
							// Don't pass non-elements to Sizzle
							cur.nodeType === 1 && jQuery.find.matchesSelector(cur, selectors))) {
	
								matched.push(cur);
								break;
							}
						}
					}
				}
	
				return this.pushStack(matched.length > 1 ? jQuery.uniqueSort(matched) : matched);
			},
	
			// Determine the position of an element within the set
			index: function index(elem) {
	
				// No argument, return index in parent
				if (!elem) {
					return this[0] && this[0].parentNode ? this.first().prevAll().length : -1;
				}
	
				// Index in selector
				if (typeof elem === "string") {
					return indexOf.call(jQuery(elem), this[0]);
				}
	
				// Locate the position of the desired element
				return indexOf.call(this,
	
				// If it receives a jQuery object, the first element is used
				elem.jquery ? elem[0] : elem);
			},
	
			add: function add(selector, context) {
				return this.pushStack(jQuery.uniqueSort(jQuery.merge(this.get(), jQuery(selector, context))));
			},
	
			addBack: function addBack(selector) {
				return this.add(selector == null ? this.prevObject : this.prevObject.filter(selector));
			}
		});
	
		function sibling(cur, dir) {
			while ((cur = cur[dir]) && cur.nodeType !== 1) {}
			return cur;
		}
	
		jQuery.each({
			parent: function parent(elem) {
				var parent = elem.parentNode;
				return parent && parent.nodeType !== 11 ? parent : null;
			},
			parents: function parents(elem) {
				return dir(elem, "parentNode");
			},
			parentsUntil: function parentsUntil(elem, i, until) {
				return dir(elem, "parentNode", until);
			},
			next: function next(elem) {
				return sibling(elem, "nextSibling");
			},
			prev: function prev(elem) {
				return sibling(elem, "previousSibling");
			},
			nextAll: function nextAll(elem) {
				return dir(elem, "nextSibling");
			},
			prevAll: function prevAll(elem) {
				return dir(elem, "previousSibling");
			},
			nextUntil: function nextUntil(elem, i, until) {
				return dir(elem, "nextSibling", until);
			},
			prevUntil: function prevUntil(elem, i, until) {
				return dir(elem, "previousSibling", until);
			},
			siblings: function siblings(elem) {
				return _siblings((elem.parentNode || {}).firstChild, elem);
			},
			children: function children(elem) {
				return _siblings(elem.firstChild);
			},
			contents: function contents(elem) {
				return elem.contentDocument || jQuery.merge([], elem.childNodes);
			}
		}, function (name, fn) {
			jQuery.fn[name] = function (until, selector) {
				var matched = jQuery.map(this, fn, until);
	
				if (name.slice(-5) !== "Until") {
					selector = until;
				}
	
				if (selector && typeof selector === "string") {
					matched = jQuery.filter(selector, matched);
				}
	
				if (this.length > 1) {
	
					// Remove duplicates
					if (!guaranteedUnique[name]) {
						jQuery.uniqueSort(matched);
					}
	
					// Reverse order for parents* and prev-derivatives
					if (rparentsprev.test(name)) {
						matched.reverse();
					}
				}
	
				return this.pushStack(matched);
			};
		});
		var rnothtmlwhite = /[^\x20\t\r\n\f]+/g;
	
		// Convert String-formatted options into Object-formatted ones
		function createOptions(options) {
			var object = {};
			jQuery.each(options.match(rnothtmlwhite) || [], function (_, flag) {
				object[flag] = true;
			});
			return object;
		}
	
		/*
	  * Create a callback list using the following parameters:
	  *
	  *	options: an optional list of space-separated options that will change how
	  *			the callback list behaves or a more traditional option object
	  *
	  * By default a callback list will act like an event callback list and can be
	  * "fired" multiple times.
	  *
	  * Possible options:
	  *
	  *	once:			will ensure the callback list can only be fired once (like a Deferred)
	  *
	  *	memory:			will keep track of previous values and will call any callback added
	  *					after the list has been fired right away with the latest "memorized"
	  *					values (like a Deferred)
	  *
	  *	unique:			will ensure a callback can only be added once (no duplicate in the list)
	  *
	  *	stopOnFalse:	interrupt callings when a callback returns false
	  *
	  */
		jQuery.Callbacks = function (options) {
	
			// Convert options from String-formatted to Object-formatted if needed
			// (we check in cache first)
			options = typeof options === "string" ? createOptions(options) : jQuery.extend({}, options);
	
			var // Flag to know if list is currently firing
			firing,
	
	
			// Last fire value for non-forgettable lists
			memory,
	
	
			// Flag to know if list was already fired
			_fired,
	
	
			// Flag to prevent firing
			_locked,
	
	
			// Actual callback list
			list = [],
	
	
			// Queue of execution data for repeatable lists
			queue = [],
	
	
			// Index of currently firing callback (modified by add/remove as needed)
			firingIndex = -1,
	
	
			// Fire callbacks
			fire = function fire() {
	
				// Enforce single-firing
				_locked = options.once;
	
				// Execute callbacks for all pending executions,
				// respecting firingIndex overrides and runtime changes
				_fired = firing = true;
				for (; queue.length; firingIndex = -1) {
					memory = queue.shift();
					while (++firingIndex < list.length) {
	
						// Run callback and check for early termination
						if (list[firingIndex].apply(memory[0], memory[1]) === false && options.stopOnFalse) {
	
							// Jump to end and forget the data so .add doesn't re-fire
							firingIndex = list.length;
							memory = false;
						}
					}
				}
	
				// Forget the data if we're done with it
				if (!options.memory) {
					memory = false;
				}
	
				firing = false;
	
				// Clean up if we're done firing for good
				if (_locked) {
	
					// Keep an empty list if we have data for future add calls
					if (memory) {
						list = [];
	
						// Otherwise, this object is spent
					} else {
						list = "";
					}
				}
			},
	
	
			// Actual Callbacks object
			self = {
	
				// Add a callback or a collection of callbacks to the list
				add: function add() {
					if (list) {
	
						// If we have memory from a past run, we should fire after adding
						if (memory && !firing) {
							firingIndex = list.length - 1;
							queue.push(memory);
						}
	
						(function add(args) {
							jQuery.each(args, function (_, arg) {
								if (jQuery.isFunction(arg)) {
									if (!options.unique || !self.has(arg)) {
										list.push(arg);
									}
								} else if (arg && arg.length && jQuery.type(arg) !== "string") {
	
									// Inspect recursively
									add(arg);
								}
							});
						})(arguments);
	
						if (memory && !firing) {
							fire();
						}
					}
					return this;
				},
	
				// Remove a callback from the list
				remove: function remove() {
					jQuery.each(arguments, function (_, arg) {
						var index;
						while ((index = jQuery.inArray(arg, list, index)) > -1) {
							list.splice(index, 1);
	
							// Handle firing indexes
							if (index <= firingIndex) {
								firingIndex--;
							}
						}
					});
					return this;
				},
	
				// Check if a given callback is in the list.
				// If no argument is given, return whether or not list has callbacks attached.
				has: function has(fn) {
					return fn ? jQuery.inArray(fn, list) > -1 : list.length > 0;
				},
	
				// Remove all callbacks from the list
				empty: function empty() {
					if (list) {
						list = [];
					}
					return this;
				},
	
				// Disable .fire and .add
				// Abort any current/pending executions
				// Clear all callbacks and values
				disable: function disable() {
					_locked = queue = [];
					list = memory = "";
					return this;
				},
				disabled: function disabled() {
					return !list;
				},
	
				// Disable .fire
				// Also disable .add unless we have memory (since it would have no effect)
				// Abort any pending executions
				lock: function lock() {
					_locked = queue = [];
					if (!memory && !firing) {
						list = memory = "";
					}
					return this;
				},
				locked: function locked() {
					return !!_locked;
				},
	
				// Call all callbacks with the given context and arguments
				fireWith: function fireWith(context, args) {
					if (!_locked) {
						args = args || [];
						args = [context, args.slice ? args.slice() : args];
						queue.push(args);
						if (!firing) {
							fire();
						}
					}
					return this;
				},
	
				// Call all the callbacks with the given arguments
				fire: function fire() {
					self.fireWith(this, arguments);
					return this;
				},
	
				// To know if the callbacks have already been called at least once
				fired: function fired() {
					return !!_fired;
				}
			};
	
			return self;
		};
	
		function Identity(v) {
			return v;
		}
		function Thrower(ex) {
			throw ex;
		}
	
		function adoptValue(value, resolve, reject) {
			var method;
	
			try {
	
				// Check for promise aspect first to privilege synchronous behavior
				if (value && jQuery.isFunction(method = value.promise)) {
					method.call(value).done(resolve).fail(reject);
	
					// Other thenables
				} else if (value && jQuery.isFunction(method = value.then)) {
					method.call(value, resolve, reject);
	
					// Other non-thenables
				} else {
	
					// Support: Android 4.0 only
					// Strict mode functions invoked without .call/.apply get global-object context
					resolve.call(undefined, value);
				}
	
				// For Promises/A+, convert exceptions into rejections
				// Since jQuery.when doesn't unwrap thenables, we can skip the extra checks appearing in
				// Deferred#then to conditionally suppress rejection.
			} catch (value) {
	
				// Support: Android 4.0 only
				// Strict mode functions invoked without .call/.apply get global-object context
				reject.call(undefined, value);
			}
		}
	
		jQuery.extend({
	
			Deferred: function Deferred(func) {
				var tuples = [
	
				// action, add listener, callbacks,
				// ... .then handlers, argument index, [final state]
				["notify", "progress", jQuery.Callbacks("memory"), jQuery.Callbacks("memory"), 2], ["resolve", "done", jQuery.Callbacks("once memory"), jQuery.Callbacks("once memory"), 0, "resolved"], ["reject", "fail", jQuery.Callbacks("once memory"), jQuery.Callbacks("once memory"), 1, "rejected"]],
				    _state = "pending",
				    _promise = {
					state: function state() {
						return _state;
					},
					always: function always() {
						deferred.done(arguments).fail(arguments);
						return this;
					},
					"catch": function _catch(fn) {
						return _promise.then(null, fn);
					},
	
					// Keep pipe for back-compat
					pipe: function pipe() /* fnDone, fnFail, fnProgress */{
						var fns = arguments;
	
						return jQuery.Deferred(function (newDefer) {
							jQuery.each(tuples, function (i, tuple) {
	
								// Map tuples (progress, done, fail) to arguments (done, fail, progress)
								var fn = jQuery.isFunction(fns[tuple[4]]) && fns[tuple[4]];
	
								// deferred.progress(function() { bind to newDefer or newDefer.notify })
								// deferred.done(function() { bind to newDefer or newDefer.resolve })
								// deferred.fail(function() { bind to newDefer or newDefer.reject })
								deferred[tuple[1]](function () {
									var returned = fn && fn.apply(this, arguments);
									if (returned && jQuery.isFunction(returned.promise)) {
										returned.promise().progress(newDefer.notify).done(newDefer.resolve).fail(newDefer.reject);
									} else {
										newDefer[tuple[0] + "With"](this, fn ? [returned] : arguments);
									}
								});
							});
							fns = null;
						}).promise();
					},
					then: function then(onFulfilled, onRejected, onProgress) {
						var maxDepth = 0;
						function resolve(depth, deferred, handler, special) {
							return function () {
								var that = this,
								    args = arguments,
								    mightThrow = function mightThrow() {
									var returned, then;
	
									// Support: Promises/A+ section 2.3.3.3.3
									// https://promisesaplus.com/#point-59
									// Ignore double-resolution attempts
									if (depth < maxDepth) {
										return;
									}
	
									returned = handler.apply(that, args);
	
									// Support: Promises/A+ section 2.3.1
									// https://promisesaplus.com/#point-48
									if (returned === deferred.promise()) {
										throw new TypeError("Thenable self-resolution");
									}
	
									// Support: Promises/A+ sections 2.3.3.1, 3.5
									// https://promisesaplus.com/#point-54
									// https://promisesaplus.com/#point-75
									// Retrieve `then` only once
									then = returned && (
	
									// Support: Promises/A+ section 2.3.4
									// https://promisesaplus.com/#point-64
									// Only check objects and functions for thenability
									(typeof returned === "undefined" ? "undefined" : _typeof(returned)) === "object" || typeof returned === "function") && returned.then;
	
									// Handle a returned thenable
									if (jQuery.isFunction(then)) {
	
										// Special processors (notify) just wait for resolution
										if (special) {
											then.call(returned, resolve(maxDepth, deferred, Identity, special), resolve(maxDepth, deferred, Thrower, special));
	
											// Normal processors (resolve) also hook into progress
										} else {
	
											// ...and disregard older resolution values
											maxDepth++;
	
											then.call(returned, resolve(maxDepth, deferred, Identity, special), resolve(maxDepth, deferred, Thrower, special), resolve(maxDepth, deferred, Identity, deferred.notifyWith));
										}
	
										// Handle all other returned values
									} else {
	
										// Only substitute handlers pass on context
										// and multiple values (non-spec behavior)
										if (handler !== Identity) {
											that = undefined;
											args = [returned];
										}
	
										// Process the value(s)
										// Default process is resolve
										(special || deferred.resolveWith)(that, args);
									}
								},
	
	
								// Only normal processors (resolve) catch and reject exceptions
								process = special ? mightThrow : function () {
									try {
										mightThrow();
									} catch (e) {
	
										if (jQuery.Deferred.exceptionHook) {
											jQuery.Deferred.exceptionHook(e, process.stackTrace);
										}
	
										// Support: Promises/A+ section 2.3.3.3.4.1
										// https://promisesaplus.com/#point-61
										// Ignore post-resolution exceptions
										if (depth + 1 >= maxDepth) {
	
											// Only substitute handlers pass on context
											// and multiple values (non-spec behavior)
											if (handler !== Thrower) {
												that = undefined;
												args = [e];
											}
	
											deferred.rejectWith(that, args);
										}
									}
								};
	
								// Support: Promises/A+ section 2.3.3.3.1
								// https://promisesaplus.com/#point-57
								// Re-resolve promises immediately to dodge false rejection from
								// subsequent errors
								if (depth) {
									process();
								} else {
	
									// Call an optional hook to record the stack, in case of exception
									// since it's otherwise lost when execution goes async
									if (jQuery.Deferred.getStackHook) {
										process.stackTrace = jQuery.Deferred.getStackHook();
									}
									window.setTimeout(process);
								}
							};
						}
	
						return jQuery.Deferred(function (newDefer) {
	
							// progress_handlers.add( ... )
							tuples[0][3].add(resolve(0, newDefer, jQuery.isFunction(onProgress) ? onProgress : Identity, newDefer.notifyWith));
	
							// fulfilled_handlers.add( ... )
							tuples[1][3].add(resolve(0, newDefer, jQuery.isFunction(onFulfilled) ? onFulfilled : Identity));
	
							// rejected_handlers.add( ... )
							tuples[2][3].add(resolve(0, newDefer, jQuery.isFunction(onRejected) ? onRejected : Thrower));
						}).promise();
					},
	
					// Get a promise for this deferred
					// If obj is provided, the promise aspect is added to the object
					promise: function promise(obj) {
						return obj != null ? jQuery.extend(obj, _promise) : _promise;
					}
				},
				    deferred = {};
	
				// Add list-specific methods
				jQuery.each(tuples, function (i, tuple) {
					var list = tuple[2],
					    stateString = tuple[5];
	
					// promise.progress = list.add
					// promise.done = list.add
					// promise.fail = list.add
					_promise[tuple[1]] = list.add;
	
					// Handle state
					if (stateString) {
						list.add(function () {
	
							// state = "resolved" (i.e., fulfilled)
							// state = "rejected"
							_state = stateString;
						},
	
						// rejected_callbacks.disable
						// fulfilled_callbacks.disable
						tuples[3 - i][2].disable,
	
						// progress_callbacks.lock
						tuples[0][2].lock);
					}
	
					// progress_handlers.fire
					// fulfilled_handlers.fire
					// rejected_handlers.fire
					list.add(tuple[3].fire);
	
					// deferred.notify = function() { deferred.notifyWith(...) }
					// deferred.resolve = function() { deferred.resolveWith(...) }
					// deferred.reject = function() { deferred.rejectWith(...) }
					deferred[tuple[0]] = function () {
						deferred[tuple[0] + "With"](this === deferred ? undefined : this, arguments);
						return this;
					};
	
					// deferred.notifyWith = list.fireWith
					// deferred.resolveWith = list.fireWith
					// deferred.rejectWith = list.fireWith
					deferred[tuple[0] + "With"] = list.fireWith;
				});
	
				// Make the deferred a promise
				_promise.promise(deferred);
	
				// Call given func if any
				if (func) {
					func.call(deferred, deferred);
				}
	
				// All done!
				return deferred;
			},
	
			// Deferred helper
			when: function when(singleValue) {
				var
	
				// count of uncompleted subordinates
				remaining = arguments.length,
	
	
				// count of unprocessed arguments
				i = remaining,
	
	
				// subordinate fulfillment data
				resolveContexts = Array(i),
				    resolveValues = _slice.call(arguments),
	
	
				// the master Deferred
				master = jQuery.Deferred(),
	
	
				// subordinate callback factory
				updateFunc = function updateFunc(i) {
					return function (value) {
						resolveContexts[i] = this;
						resolveValues[i] = arguments.length > 1 ? _slice.call(arguments) : value;
						if (! --remaining) {
							master.resolveWith(resolveContexts, resolveValues);
						}
					};
				};
	
				// Single- and empty arguments are adopted like Promise.resolve
				if (remaining <= 1) {
					adoptValue(singleValue, master.done(updateFunc(i)).resolve, master.reject);
	
					// Use .then() to unwrap secondary thenables (cf. gh-3000)
					if (master.state() === "pending" || jQuery.isFunction(resolveValues[i] && resolveValues[i].then)) {
	
						return master.then();
					}
				}
	
				// Multiple arguments are aggregated like Promise.all array elements
				while (i--) {
					adoptValue(resolveValues[i], updateFunc(i), master.reject);
				}
	
				return master.promise();
			}
		});
	
		// These usually indicate a programmer mistake during development,
		// warn about them ASAP rather than swallowing them by default.
		var rerrorNames = /^(Eval|Internal|Range|Reference|Syntax|Type|URI)Error$/;
	
		jQuery.Deferred.exceptionHook = function (error, stack) {
	
			// Support: IE 8 - 9 only
			// Console exists when dev tools are open, which can happen at any time
			if (window.console && window.console.warn && error && rerrorNames.test(error.name)) {
				window.console.warn("jQuery.Deferred exception: " + error.message, error.stack, stack);
			}
		};
	
		jQuery.readyException = function (error) {
			window.setTimeout(function () {
				throw error;
			});
		};
	
		// The deferred used on DOM ready
		var readyList = jQuery.Deferred();
	
		jQuery.fn.ready = function (fn) {
	
			readyList.then(fn)
	
			// Wrap jQuery.readyException in a function so that the lookup
			// happens at the time of error handling instead of callback
			// registration.
			.catch(function (error) {
				jQuery.readyException(error);
			});
	
			return this;
		};
	
		jQuery.extend({
	
			// Is the DOM ready to be used? Set to true once it occurs.
			isReady: false,
	
			// A counter to track how many items to wait for before
			// the ready event fires. See #6781
			readyWait: 1,
	
			// Hold (or release) the ready event
			holdReady: function holdReady(hold) {
				if (hold) {
					jQuery.readyWait++;
				} else {
					jQuery.ready(true);
				}
			},
	
			// Handle when the DOM is ready
			ready: function ready(wait) {
	
				// Abort if there are pending holds or we're already ready
				if (wait === true ? --jQuery.readyWait : jQuery.isReady) {
					return;
				}
	
				// Remember that the DOM is ready
				jQuery.isReady = true;
	
				// If a normal DOM Ready event fired, decrement, and wait if need be
				if (wait !== true && --jQuery.readyWait > 0) {
					return;
				}
	
				// If there are functions bound, to execute
				readyList.resolveWith(document, [jQuery]);
			}
		});
	
		jQuery.ready.then = readyList.then;
	
		// The ready event handler and self cleanup method
		function completed() {
			document.removeEventListener("DOMContentLoaded", completed);
			window.removeEventListener("load", completed);
			jQuery.ready();
		}
	
		// Catch cases where $(document).ready() is called
		// after the browser event has already occurred.
		// Support: IE <=9 - 10 only
		// Older IE sometimes signals "interactive" too soon
		if (document.readyState === "complete" || document.readyState !== "loading" && !document.documentElement.doScroll) {
	
			// Handle it asynchronously to allow scripts the opportunity to delay ready
			window.setTimeout(jQuery.ready);
		} else {
	
			// Use the handy event callback
			document.addEventListener("DOMContentLoaded", completed);
	
			// A fallback to window.onload, that will always work
			window.addEventListener("load", completed);
		}
	
		// Multifunctional method to get and set values of a collection
		// The value/s can optionally be executed if it's a function
		var access = function access(elems, fn, key, value, chainable, emptyGet, raw) {
			var i = 0,
			    len = elems.length,
			    bulk = key == null;
	
			// Sets many values
			if (jQuery.type(key) === "object") {
				chainable = true;
				for (i in key) {
					access(elems, fn, i, key[i], true, emptyGet, raw);
				}
	
				// Sets one value
			} else if (value !== undefined) {
				chainable = true;
	
				if (!jQuery.isFunction(value)) {
					raw = true;
				}
	
				if (bulk) {
	
					// Bulk operations run against the entire set
					if (raw) {
						fn.call(elems, value);
						fn = null;
	
						// ...except when executing function values
					} else {
						bulk = fn;
						fn = function fn(elem, key, value) {
							return bulk.call(jQuery(elem), value);
						};
					}
				}
	
				if (fn) {
					for (; i < len; i++) {
						fn(elems[i], key, raw ? value : value.call(elems[i], i, fn(elems[i], key)));
					}
				}
			}
	
			if (chainable) {
				return elems;
			}
	
			// Gets
			if (bulk) {
				return fn.call(elems);
			}
	
			return len ? fn(elems[0], key) : emptyGet;
		};
		var acceptData = function acceptData(owner) {
	
			// Accepts only:
			//  - Node
			//    - Node.ELEMENT_NODE
			//    - Node.DOCUMENT_NODE
			//  - Object
			//    - Any
			return owner.nodeType === 1 || owner.nodeType === 9 || !+owner.nodeType;
		};
	
		function Data() {
			this.expando = jQuery.expando + Data.uid++;
		}
	
		Data.uid = 1;
	
		Data.prototype = {
	
			cache: function cache(owner) {
	
				// Check if the owner object already has a cache
				var value = owner[this.expando];
	
				// If not, create one
				if (!value) {
					value = {};
	
					// We can accept data for non-element nodes in modern browsers,
					// but we should not, see #8335.
					// Always return an empty object.
					if (acceptData(owner)) {
	
						// If it is a node unlikely to be stringify-ed or looped over
						// use plain assignment
						if (owner.nodeType) {
							owner[this.expando] = value;
	
							// Otherwise secure it in a non-enumerable property
							// configurable must be true to allow the property to be
							// deleted when data is removed
						} else {
							Object.defineProperty(owner, this.expando, {
								value: value,
								configurable: true
							});
						}
					}
				}
	
				return value;
			},
			set: function set(owner, data, value) {
				var prop,
				    cache = this.cache(owner);
	
				// Handle: [ owner, key, value ] args
				// Always use camelCase key (gh-2257)
				if (typeof data === "string") {
					cache[jQuery.camelCase(data)] = value;
	
					// Handle: [ owner, { properties } ] args
				} else {
	
					// Copy the properties one-by-one to the cache object
					for (prop in data) {
						cache[jQuery.camelCase(prop)] = data[prop];
					}
				}
				return cache;
			},
			get: function get(owner, key) {
				return key === undefined ? this.cache(owner) :
	
				// Always use camelCase key (gh-2257)
				owner[this.expando] && owner[this.expando][jQuery.camelCase(key)];
			},
			access: function access(owner, key, value) {
	
				// In cases where either:
				//
				//   1. No key was specified
				//   2. A string key was specified, but no value provided
				//
				// Take the "read" path and allow the get method to determine
				// which value to return, respectively either:
				//
				//   1. The entire cache object
				//   2. The data stored at the key
				//
				if (key === undefined || key && typeof key === "string" && value === undefined) {
	
					return this.get(owner, key);
				}
	
				// When the key is not a string, or both a key and value
				// are specified, set or extend (existing objects) with either:
				//
				//   1. An object of properties
				//   2. A key and value
				//
				this.set(owner, key, value);
	
				// Since the "set" path can have two possible entry points
				// return the expected data based on which path was taken[*]
				return value !== undefined ? value : key;
			},
			remove: function remove(owner, key) {
				var i,
				    cache = owner[this.expando];
	
				if (cache === undefined) {
					return;
				}
	
				if (key !== undefined) {
	
					// Support array or space separated string of keys
					if (jQuery.isArray(key)) {
	
						// If key is an array of keys...
						// We always set camelCase keys, so remove that.
						key = key.map(jQuery.camelCase);
					} else {
						key = jQuery.camelCase(key);
	
						// If a key with the spaces exists, use it.
						// Otherwise, create an array by matching non-whitespace
						key = key in cache ? [key] : key.match(rnothtmlwhite) || [];
					}
	
					i = key.length;
	
					while (i--) {
						delete cache[key[i]];
					}
				}
	
				// Remove the expando if there's no more data
				if (key === undefined || jQuery.isEmptyObject(cache)) {
	
					// Support: Chrome <=35 - 45
					// Webkit & Blink performance suffers when deleting properties
					// from DOM nodes, so set to undefined instead
					// https://bugs.chromium.org/p/chromium/issues/detail?id=378607 (bug restricted)
					if (owner.nodeType) {
						owner[this.expando] = undefined;
					} else {
						delete owner[this.expando];
					}
				}
			},
			hasData: function hasData(owner) {
				var cache = owner[this.expando];
				return cache !== undefined && !jQuery.isEmptyObject(cache);
			}
		};
		var dataPriv = new Data();
	
		var dataUser = new Data();
	
		//	Implementation Summary
		//
		//	1. Enforce API surface and semantic compatibility with 1.9.x branch
		//	2. Improve the module's maintainability by reducing the storage
		//		paths to a single mechanism.
		//	3. Use the same single mechanism to support "private" and "user" data.
		//	4. _Never_ expose "private" data to user code (TODO: Drop _data, _removeData)
		//	5. Avoid exposing implementation details on user objects (eg. expando properties)
		//	6. Provide a clear path for implementation upgrade to WeakMap in 2014
	
		var rbrace = /^(?:\{[\w\W]*\}|\[[\w\W]*\])$/,
		    rmultiDash = /[A-Z]/g;
	
		function getData(data) {
			if (data === "true") {
				return true;
			}
	
			if (data === "false") {
				return false;
			}
	
			if (data === "null") {
				return null;
			}
	
			// Only convert to a number if it doesn't change the string
			if (data === +data + "") {
				return +data;
			}
	
			if (rbrace.test(data)) {
				return JSON.parse(data);
			}
	
			return data;
		}
	
		function dataAttr(elem, key, data) {
			var name;
	
			// If nothing was found internally, try to fetch any
			// data from the HTML5 data-* attribute
			if (data === undefined && elem.nodeType === 1) {
				name = "data-" + key.replace(rmultiDash, "-$&").toLowerCase();
				data = elem.getAttribute(name);
	
				if (typeof data === "string") {
					try {
						data = getData(data);
					} catch (e) {}
	
					// Make sure we set the data so it isn't changed later
					dataUser.set(elem, key, data);
				} else {
					data = undefined;
				}
			}
			return data;
		}
	
		jQuery.extend({
			hasData: function hasData(elem) {
				return dataUser.hasData(elem) || dataPriv.hasData(elem);
			},
	
			data: function data(elem, name, _data) {
				return dataUser.access(elem, name, _data);
			},
	
			removeData: function removeData(elem, name) {
				dataUser.remove(elem, name);
			},
	
			// TODO: Now that all calls to _data and _removeData have been replaced
			// with direct calls to dataPriv methods, these can be deprecated.
			_data: function _data(elem, name, data) {
				return dataPriv.access(elem, name, data);
			},
	
			_removeData: function _removeData(elem, name) {
				dataPriv.remove(elem, name);
			}
		});
	
		jQuery.fn.extend({
			data: function data(key, value) {
				var i,
				    name,
				    data,
				    elem = this[0],
				    attrs = elem && elem.attributes;
	
				// Gets all values
				if (key === undefined) {
					if (this.length) {
						data = dataUser.get(elem);
	
						if (elem.nodeType === 1 && !dataPriv.get(elem, "hasDataAttrs")) {
							i = attrs.length;
							while (i--) {
	
								// Support: IE 11 only
								// The attrs elements can be null (#14894)
								if (attrs[i]) {
									name = attrs[i].name;
									if (name.indexOf("data-") === 0) {
										name = jQuery.camelCase(name.slice(5));
										dataAttr(elem, name, data[name]);
									}
								}
							}
							dataPriv.set(elem, "hasDataAttrs", true);
						}
					}
	
					return data;
				}
	
				// Sets multiple values
				if ((typeof key === "undefined" ? "undefined" : _typeof(key)) === "object") {
					return this.each(function () {
						dataUser.set(this, key);
					});
				}
	
				return access(this, function (value) {
					var data;
	
					// The calling jQuery object (element matches) is not empty
					// (and therefore has an element appears at this[ 0 ]) and the
					// `value` parameter was not undefined. An empty jQuery object
					// will result in `undefined` for elem = this[ 0 ] which will
					// throw an exception if an attempt to read a data cache is made.
					if (elem && value === undefined) {
	
						// Attempt to get data from the cache
						// The key will always be camelCased in Data
						data = dataUser.get(elem, key);
						if (data !== undefined) {
							return data;
						}
	
						// Attempt to "discover" the data in
						// HTML5 custom data-* attrs
						data = dataAttr(elem, key);
						if (data !== undefined) {
							return data;
						}
	
						// We tried really hard, but the data doesn't exist.
						return;
					}
	
					// Set the data...
					this.each(function () {
	
						// We always store the camelCased key
						dataUser.set(this, key, value);
					});
				}, null, value, arguments.length > 1, null, true);
			},
	
			removeData: function removeData(key) {
				return this.each(function () {
					dataUser.remove(this, key);
				});
			}
		});
	
		jQuery.extend({
			queue: function queue(elem, type, data) {
				var queue;
	
				if (elem) {
					type = (type || "fx") + "queue";
					queue = dataPriv.get(elem, type);
	
					// Speed up dequeue by getting out quickly if this is just a lookup
					if (data) {
						if (!queue || jQuery.isArray(data)) {
							queue = dataPriv.access(elem, type, jQuery.makeArray(data));
						} else {
							queue.push(data);
						}
					}
					return queue || [];
				}
			},
	
			dequeue: function dequeue(elem, type) {
				type = type || "fx";
	
				var queue = jQuery.queue(elem, type),
				    startLength = queue.length,
				    fn = queue.shift(),
				    hooks = jQuery._queueHooks(elem, type),
				    next = function next() {
					jQuery.dequeue(elem, type);
				};
	
				// If the fx queue is dequeued, always remove the progress sentinel
				if (fn === "inprogress") {
					fn = queue.shift();
					startLength--;
				}
	
				if (fn) {
	
					// Add a progress sentinel to prevent the fx queue from being
					// automatically dequeued
					if (type === "fx") {
						queue.unshift("inprogress");
					}
	
					// Clear up the last queue stop function
					delete hooks.stop;
					fn.call(elem, next, hooks);
				}
	
				if (!startLength && hooks) {
					hooks.empty.fire();
				}
			},
	
			// Not public - generate a queueHooks object, or return the current one
			_queueHooks: function _queueHooks(elem, type) {
				var key = type + "queueHooks";
				return dataPriv.get(elem, key) || dataPriv.access(elem, key, {
					empty: jQuery.Callbacks("once memory").add(function () {
						dataPriv.remove(elem, [type + "queue", key]);
					})
				});
			}
		});
	
		jQuery.fn.extend({
			queue: function queue(type, data) {
				var setter = 2;
	
				if (typeof type !== "string") {
					data = type;
					type = "fx";
					setter--;
				}
	
				if (arguments.length < setter) {
					return jQuery.queue(this[0], type);
				}
	
				return data === undefined ? this : this.each(function () {
					var queue = jQuery.queue(this, type, data);
	
					// Ensure a hooks for this queue
					jQuery._queueHooks(this, type);
	
					if (type === "fx" && queue[0] !== "inprogress") {
						jQuery.dequeue(this, type);
					}
				});
			},
			dequeue: function dequeue(type) {
				return this.each(function () {
					jQuery.dequeue(this, type);
				});
			},
			clearQueue: function clearQueue(type) {
				return this.queue(type || "fx", []);
			},
	
			// Get a promise resolved when queues of a certain type
			// are emptied (fx is the type by default)
			promise: function promise(type, obj) {
				var tmp,
				    count = 1,
				    defer = jQuery.Deferred(),
				    elements = this,
				    i = this.length,
				    resolve = function resolve() {
					if (! --count) {
						defer.resolveWith(elements, [elements]);
					}
				};
	
				if (typeof type !== "string") {
					obj = type;
					type = undefined;
				}
				type = type || "fx";
	
				while (i--) {
					tmp = dataPriv.get(elements[i], type + "queueHooks");
					if (tmp && tmp.empty) {
						count++;
						tmp.empty.add(resolve);
					}
				}
				resolve();
				return defer.promise(obj);
			}
		});
		var pnum = /[+-]?(?:\d*\.|)\d+(?:[eE][+-]?\d+|)/.source;
	
		var rcssNum = new RegExp("^(?:([+-])=|)(" + pnum + ")([a-z%]*)$", "i");
	
		var cssExpand = ["Top", "Right", "Bottom", "Left"];
	
		var isHiddenWithinTree = function isHiddenWithinTree(elem, el) {
	
			// isHiddenWithinTree might be called from jQuery#filter function;
			// in that case, element will be second argument
			elem = el || elem;
	
			// Inline style trumps all
			return elem.style.display === "none" || elem.style.display === "" &&
	
			// Otherwise, check computed style
			// Support: Firefox <=43 - 45
			// Disconnected elements can have computed display: none, so first confirm that elem is
			// in the document.
			jQuery.contains(elem.ownerDocument, elem) && jQuery.css(elem, "display") === "none";
		};
	
		var swap = function swap(elem, options, callback, args) {
			var ret,
			    name,
			    old = {};
	
			// Remember the old values, and insert the new ones
			for (name in options) {
				old[name] = elem.style[name];
				elem.style[name] = options[name];
			}
	
			ret = callback.apply(elem, args || []);
	
			// Revert the old values
			for (name in options) {
				elem.style[name] = old[name];
			}
	
			return ret;
		};
	
		function adjustCSS(elem, prop, valueParts, tween) {
			var adjusted,
			    scale = 1,
			    maxIterations = 20,
			    currentValue = tween ? function () {
				return tween.cur();
			} : function () {
				return jQuery.css(elem, prop, "");
			},
			    initial = currentValue(),
			    unit = valueParts && valueParts[3] || (jQuery.cssNumber[prop] ? "" : "px"),
	
	
			// Starting value computation is required for potential unit mismatches
			initialInUnit = (jQuery.cssNumber[prop] || unit !== "px" && +initial) && rcssNum.exec(jQuery.css(elem, prop));
	
			if (initialInUnit && initialInUnit[3] !== unit) {
	
				// Trust units reported by jQuery.css
				unit = unit || initialInUnit[3];
	
				// Make sure we update the tween properties later on
				valueParts = valueParts || [];
	
				// Iteratively approximate from a nonzero starting point
				initialInUnit = +initial || 1;
	
				do {
	
					// If previous iteration zeroed out, double until we get *something*.
					// Use string for doubling so we don't accidentally see scale as unchanged below
					scale = scale || ".5";
	
					// Adjust and apply
					initialInUnit = initialInUnit / scale;
					jQuery.style(elem, prop, initialInUnit + unit);
	
					// Update scale, tolerating zero or NaN from tween.cur()
					// Break the loop if scale is unchanged or perfect, or if we've just had enough.
				} while (scale !== (scale = currentValue() / initial) && scale !== 1 && --maxIterations);
			}
	
			if (valueParts) {
				initialInUnit = +initialInUnit || +initial || 0;
	
				// Apply relative offset (+=/-=) if specified
				adjusted = valueParts[1] ? initialInUnit + (valueParts[1] + 1) * valueParts[2] : +valueParts[2];
				if (tween) {
					tween.unit = unit;
					tween.start = initialInUnit;
					tween.end = adjusted;
				}
			}
			return adjusted;
		}
	
		var defaultDisplayMap = {};
	
		function getDefaultDisplay(elem) {
			var temp,
			    doc = elem.ownerDocument,
			    nodeName = elem.nodeName,
			    display = defaultDisplayMap[nodeName];
	
			if (display) {
				return display;
			}
	
			temp = doc.body.appendChild(doc.createElement(nodeName));
			display = jQuery.css(temp, "display");
	
			temp.parentNode.removeChild(temp);
	
			if (display === "none") {
				display = "block";
			}
			defaultDisplayMap[nodeName] = display;
	
			return display;
		}
	
		function showHide(elements, show) {
			var display,
			    elem,
			    values = [],
			    index = 0,
			    length = elements.length;
	
			// Determine new display value for elements that need to change
			for (; index < length; index++) {
				elem = elements[index];
				if (!elem.style) {
					continue;
				}
	
				display = elem.style.display;
				if (show) {
	
					// Since we force visibility upon cascade-hidden elements, an immediate (and slow)
					// check is required in this first loop unless we have a nonempty display value (either
					// inline or about-to-be-restored)
					if (display === "none") {
						values[index] = dataPriv.get(elem, "display") || null;
						if (!values[index]) {
							elem.style.display = "";
						}
					}
					if (elem.style.display === "" && isHiddenWithinTree(elem)) {
						values[index] = getDefaultDisplay(elem);
					}
				} else {
					if (display !== "none") {
						values[index] = "none";
	
						// Remember what we're overwriting
						dataPriv.set(elem, "display", display);
					}
				}
			}
	
			// Set the display of the elements in a second loop to avoid constant reflow
			for (index = 0; index < length; index++) {
				if (values[index] != null) {
					elements[index].style.display = values[index];
				}
			}
	
			return elements;
		}
	
		jQuery.fn.extend({
			show: function show() {
				return showHide(this, true);
			},
			hide: function hide() {
				return showHide(this);
			},
			toggle: function toggle(state) {
				if (typeof state === "boolean") {
					return state ? this.show() : this.hide();
				}
	
				return this.each(function () {
					if (isHiddenWithinTree(this)) {
						jQuery(this).show();
					} else {
						jQuery(this).hide();
					}
				});
			}
		});
		var rcheckableType = /^(?:checkbox|radio)$/i;
	
		var rtagName = /<([a-z][^\/\0>\x20\t\r\n\f]+)/i;
	
		var rscriptType = /^$|\/(?:java|ecma)script/i;
	
		// We have to close these tags to support XHTML (#13200)
		var wrapMap = {
	
			// Support: IE <=9 only
			option: [1, "<select multiple='multiple'>", "</select>"],
	
			// XHTML parsers do not magically insert elements in the
			// same way that tag soup parsers do. So we cannot shorten
			// this by omitting <tbody> or other required elements.
			thead: [1, "<table>", "</table>"],
			col: [2, "<table><colgroup>", "</colgroup></table>"],
			tr: [2, "<table><tbody>", "</tbody></table>"],
			td: [3, "<table><tbody><tr>", "</tr></tbody></table>"],
	
			_default: [0, "", ""]
		};
	
		// Support: IE <=9 only
		wrapMap.optgroup = wrapMap.option;
	
		wrapMap.tbody = wrapMap.tfoot = wrapMap.colgroup = wrapMap.caption = wrapMap.thead;
		wrapMap.th = wrapMap.td;
	
		function getAll(context, tag) {
	
			// Support: IE <=9 - 11 only
			// Use typeof to avoid zero-argument method invocation on host objects (#15151)
			var ret;
	
			if (typeof context.getElementsByTagName !== "undefined") {
				ret = context.getElementsByTagName(tag || "*");
			} else if (typeof context.querySelectorAll !== "undefined") {
				ret = context.querySelectorAll(tag || "*");
			} else {
				ret = [];
			}
	
			if (tag === undefined || tag && jQuery.nodeName(context, tag)) {
				return jQuery.merge([context], ret);
			}
	
			return ret;
		}
	
		// Mark scripts as having already been evaluated
		function setGlobalEval(elems, refElements) {
			var i = 0,
			    l = elems.length;
	
			for (; i < l; i++) {
				dataPriv.set(elems[i], "globalEval", !refElements || dataPriv.get(refElements[i], "globalEval"));
			}
		}
	
		var rhtml = /<|&#?\w+;/;
	
		function buildFragment(elems, context, scripts, selection, ignored) {
			var elem,
			    tmp,
			    tag,
			    wrap,
			    contains,
			    j,
			    fragment = context.createDocumentFragment(),
			    nodes = [],
			    i = 0,
			    l = elems.length;
	
			for (; i < l; i++) {
				elem = elems[i];
	
				if (elem || elem === 0) {
	
					// Add nodes directly
					if (jQuery.type(elem) === "object") {
	
						// Support: Android <=4.0 only, PhantomJS 1 only
						// push.apply(_, arraylike) throws on ancient WebKit
						jQuery.merge(nodes, elem.nodeType ? [elem] : elem);
	
						// Convert non-html into a text node
					} else if (!rhtml.test(elem)) {
						nodes.push(context.createTextNode(elem));
	
						// Convert html into DOM nodes
					} else {
						tmp = tmp || fragment.appendChild(context.createElement("div"));
	
						// Deserialize a standard representation
						tag = (rtagName.exec(elem) || ["", ""])[1].toLowerCase();
						wrap = wrapMap[tag] || wrapMap._default;
						tmp.innerHTML = wrap[1] + jQuery.htmlPrefilter(elem) + wrap[2];
	
						// Descend through wrappers to the right content
						j = wrap[0];
						while (j--) {
							tmp = tmp.lastChild;
						}
	
						// Support: Android <=4.0 only, PhantomJS 1 only
						// push.apply(_, arraylike) throws on ancient WebKit
						jQuery.merge(nodes, tmp.childNodes);
	
						// Remember the top-level container
						tmp = fragment.firstChild;
	
						// Ensure the created nodes are orphaned (#12392)
						tmp.textContent = "";
					}
				}
			}
	
			// Remove wrapper from fragment
			fragment.textContent = "";
	
			i = 0;
			while (elem = nodes[i++]) {
	
				// Skip elements already in the context collection (trac-4087)
				if (selection && jQuery.inArray(elem, selection) > -1) {
					if (ignored) {
						ignored.push(elem);
					}
					continue;
				}
	
				contains = jQuery.contains(elem.ownerDocument, elem);
	
				// Append to fragment
				tmp = getAll(fragment.appendChild(elem), "script");
	
				// Preserve script evaluation history
				if (contains) {
					setGlobalEval(tmp);
				}
	
				// Capture executables
				if (scripts) {
					j = 0;
					while (elem = tmp[j++]) {
						if (rscriptType.test(elem.type || "")) {
							scripts.push(elem);
						}
					}
				}
			}
	
			return fragment;
		}
	
		(function () {
			var fragment = document.createDocumentFragment(),
			    div = fragment.appendChild(document.createElement("div")),
			    input = document.createElement("input");
	
			// Support: Android 4.0 - 4.3 only
			// Check state lost if the name is set (#11217)
			// Support: Windows Web Apps (WWA)
			// `name` and `type` must use .setAttribute for WWA (#14901)
			input.setAttribute("type", "radio");
			input.setAttribute("checked", "checked");
			input.setAttribute("name", "t");
	
			div.appendChild(input);
	
			// Support: Android <=4.1 only
			// Older WebKit doesn't clone checked state correctly in fragments
			support.checkClone = div.cloneNode(true).cloneNode(true).lastChild.checked;
	
			// Support: IE <=11 only
			// Make sure textarea (and checkbox) defaultValue is properly cloned
			div.innerHTML = "<textarea>x</textarea>";
			support.noCloneChecked = !!div.cloneNode(true).lastChild.defaultValue;
		})();
		var documentElement = document.documentElement;
	
		var rkeyEvent = /^key/,
		    rmouseEvent = /^(?:mouse|pointer|contextmenu|drag|drop)|click/,
		    rtypenamespace = /^([^.]*)(?:\.(.+)|)/;
	
		function returnTrue() {
			return true;
		}
	
		function returnFalse() {
			return false;
		}
	
		// Support: IE <=9 only
		// See #13393 for more info
		function safeActiveElement() {
			try {
				return document.activeElement;
			} catch (err) {}
		}
	
		function _on(elem, types, selector, data, fn, one) {
			var origFn, type;
	
			// Types can be a map of types/handlers
			if ((typeof types === "undefined" ? "undefined" : _typeof(types)) === "object") {
	
				// ( types-Object, selector, data )
				if (typeof selector !== "string") {
	
					// ( types-Object, data )
					data = data || selector;
					selector = undefined;
				}
				for (type in types) {
					_on(elem, type, selector, data, types[type], one);
				}
				return elem;
			}
	
			if (data == null && fn == null) {
	
				// ( types, fn )
				fn = selector;
				data = selector = undefined;
			} else if (fn == null) {
				if (typeof selector === "string") {
	
					// ( types, selector, fn )
					fn = data;
					data = undefined;
				} else {
	
					// ( types, data, fn )
					fn = data;
					data = selector;
					selector = undefined;
				}
			}
			if (fn === false) {
				fn = returnFalse;
			} else if (!fn) {
				return elem;
			}
	
			if (one === 1) {
				origFn = fn;
				fn = function fn(event) {
	
					// Can use an empty set, since event contains the info
					jQuery().off(event);
					return origFn.apply(this, arguments);
				};
	
				// Use same guid so caller can remove using origFn
				fn.guid = origFn.guid || (origFn.guid = jQuery.guid++);
			}
			return elem.each(function () {
				jQuery.event.add(this, types, fn, data, selector);
			});
		}
	
		/*
	  * Helper functions for managing events -- not part of the public interface.
	  * Props to Dean Edwards' addEvent library for many of the ideas.
	  */
		jQuery.event = {
	
			global: {},
	
			add: function add(elem, types, handler, data, selector) {
	
				var handleObjIn,
				    eventHandle,
				    tmp,
				    events,
				    t,
				    handleObj,
				    special,
				    handlers,
				    type,
				    namespaces,
				    origType,
				    elemData = dataPriv.get(elem);
	
				// Don't attach events to noData or text/comment nodes (but allow plain objects)
				if (!elemData) {
					return;
				}
	
				// Caller can pass in an object of custom data in lieu of the handler
				if (handler.handler) {
					handleObjIn = handler;
					handler = handleObjIn.handler;
					selector = handleObjIn.selector;
				}
	
				// Ensure that invalid selectors throw exceptions at attach time
				// Evaluate against documentElement in case elem is a non-element node (e.g., document)
				if (selector) {
					jQuery.find.matchesSelector(documentElement, selector);
				}
	
				// Make sure that the handler has a unique ID, used to find/remove it later
				if (!handler.guid) {
					handler.guid = jQuery.guid++;
				}
	
				// Init the element's event structure and main handler, if this is the first
				if (!(events = elemData.events)) {
					events = elemData.events = {};
				}
				if (!(eventHandle = elemData.handle)) {
					eventHandle = elemData.handle = function (e) {
	
						// Discard the second event of a jQuery.event.trigger() and
						// when an event is called after a page has unloaded
						return typeof jQuery !== "undefined" && jQuery.event.triggered !== e.type ? jQuery.event.dispatch.apply(elem, arguments) : undefined;
					};
				}
	
				// Handle multiple events separated by a space
				types = (types || "").match(rnothtmlwhite) || [""];
				t = types.length;
				while (t--) {
					tmp = rtypenamespace.exec(types[t]) || [];
					type = origType = tmp[1];
					namespaces = (tmp[2] || "").split(".").sort();
	
					// There *must* be a type, no attaching namespace-only handlers
					if (!type) {
						continue;
					}
	
					// If event changes its type, use the special event handlers for the changed type
					special = jQuery.event.special[type] || {};
	
					// If selector defined, determine special event api type, otherwise given type
					type = (selector ? special.delegateType : special.bindType) || type;
	
					// Update special based on newly reset type
					special = jQuery.event.special[type] || {};
	
					// handleObj is passed to all event handlers
					handleObj = jQuery.extend({
						type: type,
						origType: origType,
						data: data,
						handler: handler,
						guid: handler.guid,
						selector: selector,
						needsContext: selector && jQuery.expr.match.needsContext.test(selector),
						namespace: namespaces.join(".")
					}, handleObjIn);
	
					// Init the event handler queue if we're the first
					if (!(handlers = events[type])) {
						handlers = events[type] = [];
						handlers.delegateCount = 0;
	
						// Only use addEventListener if the special events handler returns false
						if (!special.setup || special.setup.call(elem, data, namespaces, eventHandle) === false) {
	
							if (elem.addEventListener) {
								elem.addEventListener(type, eventHandle);
							}
						}
					}
	
					if (special.add) {
						special.add.call(elem, handleObj);
	
						if (!handleObj.handler.guid) {
							handleObj.handler.guid = handler.guid;
						}
					}
	
					// Add to the element's handler list, delegates in front
					if (selector) {
						handlers.splice(handlers.delegateCount++, 0, handleObj);
					} else {
						handlers.push(handleObj);
					}
	
					// Keep track of which events have ever been used, for event optimization
					jQuery.event.global[type] = true;
				}
			},
	
			// Detach an event or set of events from an element
			remove: function remove(elem, types, handler, selector, mappedTypes) {
	
				var j,
				    origCount,
				    tmp,
				    events,
				    t,
				    handleObj,
				    special,
				    handlers,
				    type,
				    namespaces,
				    origType,
				    elemData = dataPriv.hasData(elem) && dataPriv.get(elem);
	
				if (!elemData || !(events = elemData.events)) {
					return;
				}
	
				// Once for each type.namespace in types; type may be omitted
				types = (types || "").match(rnothtmlwhite) || [""];
				t = types.length;
				while (t--) {
					tmp = rtypenamespace.exec(types[t]) || [];
					type = origType = tmp[1];
					namespaces = (tmp[2] || "").split(".").sort();
	
					// Unbind all events (on this namespace, if provided) for the element
					if (!type) {
						for (type in events) {
							jQuery.event.remove(elem, type + types[t], handler, selector, true);
						}
						continue;
					}
	
					special = jQuery.event.special[type] || {};
					type = (selector ? special.delegateType : special.bindType) || type;
					handlers = events[type] || [];
					tmp = tmp[2] && new RegExp("(^|\\.)" + namespaces.join("\\.(?:.*\\.|)") + "(\\.|$)");
	
					// Remove matching events
					origCount = j = handlers.length;
					while (j--) {
						handleObj = handlers[j];
	
						if ((mappedTypes || origType === handleObj.origType) && (!handler || handler.guid === handleObj.guid) && (!tmp || tmp.test(handleObj.namespace)) && (!selector || selector === handleObj.selector || selector === "**" && handleObj.selector)) {
							handlers.splice(j, 1);
	
							if (handleObj.selector) {
								handlers.delegateCount--;
							}
							if (special.remove) {
								special.remove.call(elem, handleObj);
							}
						}
					}
	
					// Remove generic event handler if we removed something and no more handlers exist
					// (avoids potential for endless recursion during removal of special event handlers)
					if (origCount && !handlers.length) {
						if (!special.teardown || special.teardown.call(elem, namespaces, elemData.handle) === false) {
	
							jQuery.removeEvent(elem, type, elemData.handle);
						}
	
						delete events[type];
					}
				}
	
				// Remove data and the expando if it's no longer used
				if (jQuery.isEmptyObject(events)) {
					dataPriv.remove(elem, "handle events");
				}
			},
	
			dispatch: function dispatch(nativeEvent) {
	
				// Make a writable jQuery.Event from the native event object
				var event = jQuery.event.fix(nativeEvent);
	
				var i,
				    j,
				    ret,
				    matched,
				    handleObj,
				    handlerQueue,
				    args = new Array(arguments.length),
				    handlers = (dataPriv.get(this, "events") || {})[event.type] || [],
				    special = jQuery.event.special[event.type] || {};
	
				// Use the fix-ed jQuery.Event rather than the (read-only) native event
				args[0] = event;
	
				for (i = 1; i < arguments.length; i++) {
					args[i] = arguments[i];
				}
	
				event.delegateTarget = this;
	
				// Call the preDispatch hook for the mapped type, and let it bail if desired
				if (special.preDispatch && special.preDispatch.call(this, event) === false) {
					return;
				}
	
				// Determine handlers
				handlerQueue = jQuery.event.handlers.call(this, event, handlers);
	
				// Run delegates first; they may want to stop propagation beneath us
				i = 0;
				while ((matched = handlerQueue[i++]) && !event.isPropagationStopped()) {
					event.currentTarget = matched.elem;
	
					j = 0;
					while ((handleObj = matched.handlers[j++]) && !event.isImmediatePropagationStopped()) {
	
						// Triggered event must either 1) have no namespace, or 2) have namespace(s)
						// a subset or equal to those in the bound event (both can have no namespace).
						if (!event.rnamespace || event.rnamespace.test(handleObj.namespace)) {
	
							event.handleObj = handleObj;
							event.data = handleObj.data;
	
							ret = ((jQuery.event.special[handleObj.origType] || {}).handle || handleObj.handler).apply(matched.elem, args);
	
							if (ret !== undefined) {
								if ((event.result = ret) === false) {
									event.preventDefault();
									event.stopPropagation();
								}
							}
						}
					}
				}
	
				// Call the postDispatch hook for the mapped type
				if (special.postDispatch) {
					special.postDispatch.call(this, event);
				}
	
				return event.result;
			},
	
			handlers: function handlers(event, _handlers) {
				var i,
				    handleObj,
				    sel,
				    matchedHandlers,
				    matchedSelectors,
				    handlerQueue = [],
				    delegateCount = _handlers.delegateCount,
				    cur = event.target;
	
				// Find delegate handlers
				if (delegateCount &&
	
				// Support: IE <=9
				// Black-hole SVG <use> instance trees (trac-13180)
				cur.nodeType &&
	
				// Support: Firefox <=42
				// Suppress spec-violating clicks indicating a non-primary pointer button (trac-3861)
				// https://www.w3.org/TR/DOM-Level-3-Events/#event-type-click
				// Support: IE 11 only
				// ...but not arrow key "clicks" of radio inputs, which can have `button` -1 (gh-2343)
				!(event.type === "click" && event.button >= 1)) {
	
					for (; cur !== this; cur = cur.parentNode || this) {
	
						// Don't check non-elements (#13208)
						// Don't process clicks on disabled elements (#6911, #8165, #11382, #11764)
						if (cur.nodeType === 1 && !(event.type === "click" && cur.disabled === true)) {
							matchedHandlers = [];
							matchedSelectors = {};
							for (i = 0; i < delegateCount; i++) {
								handleObj = _handlers[i];
	
								// Don't conflict with Object.prototype properties (#13203)
								sel = handleObj.selector + " ";
	
								if (matchedSelectors[sel] === undefined) {
									matchedSelectors[sel] = handleObj.needsContext ? jQuery(sel, this).index(cur) > -1 : jQuery.find(sel, this, null, [cur]).length;
								}
								if (matchedSelectors[sel]) {
									matchedHandlers.push(handleObj);
								}
							}
							if (matchedHandlers.length) {
								handlerQueue.push({ elem: cur, handlers: matchedHandlers });
							}
						}
					}
				}
	
				// Add the remaining (directly-bound) handlers
				cur = this;
				if (delegateCount < _handlers.length) {
					handlerQueue.push({ elem: cur, handlers: _handlers.slice(delegateCount) });
				}
	
				return handlerQueue;
			},
	
			addProp: function addProp(name, hook) {
				Object.defineProperty(jQuery.Event.prototype, name, {
					enumerable: true,
					configurable: true,
	
					get: jQuery.isFunction(hook) ? function () {
						if (this.originalEvent) {
							return hook(this.originalEvent);
						}
					} : function () {
						if (this.originalEvent) {
							return this.originalEvent[name];
						}
					},
	
					set: function set(value) {
						Object.defineProperty(this, name, {
							enumerable: true,
							configurable: true,
							writable: true,
							value: value
						});
					}
				});
			},
	
			fix: function fix(originalEvent) {
				return originalEvent[jQuery.expando] ? originalEvent : new jQuery.Event(originalEvent);
			},
	
			special: {
				load: {
	
					// Prevent triggered image.load events from bubbling to window.load
					noBubble: true
				},
				focus: {
	
					// Fire native event if possible so blur/focus sequence is correct
					trigger: function trigger() {
						if (this !== safeActiveElement() && this.focus) {
							this.focus();
							return false;
						}
					},
					delegateType: "focusin"
				},
				blur: {
					trigger: function trigger() {
						if (this === safeActiveElement() && this.blur) {
							this.blur();
							return false;
						}
					},
					delegateType: "focusout"
				},
				click: {
	
					// For checkbox, fire native event so checked state will be right
					trigger: function trigger() {
						if (this.type === "checkbox" && this.click && jQuery.nodeName(this, "input")) {
							this.click();
							return false;
						}
					},
	
					// For cross-browser consistency, don't fire native .click() on links
					_default: function _default(event) {
						return jQuery.nodeName(event.target, "a");
					}
				},
	
				beforeunload: {
					postDispatch: function postDispatch(event) {
	
						// Support: Firefox 20+
						// Firefox doesn't alert if the returnValue field is not set.
						if (event.result !== undefined && event.originalEvent) {
							event.originalEvent.returnValue = event.result;
						}
					}
				}
			}
		};
	
		jQuery.removeEvent = function (elem, type, handle) {
	
			// This "if" is needed for plain objects
			if (elem.removeEventListener) {
				elem.removeEventListener(type, handle);
			}
		};
	
		jQuery.Event = function (src, props) {
	
			// Allow instantiation without the 'new' keyword
			if (!(this instanceof jQuery.Event)) {
				return new jQuery.Event(src, props);
			}
	
			// Event object
			if (src && src.type) {
				this.originalEvent = src;
				this.type = src.type;
	
				// Events bubbling up the document may have been marked as prevented
				// by a handler lower down the tree; reflect the correct value.
				this.isDefaultPrevented = src.defaultPrevented || src.defaultPrevented === undefined &&
	
				// Support: Android <=2.3 only
				src.returnValue === false ? returnTrue : returnFalse;
	
				// Create target properties
				// Support: Safari <=6 - 7 only
				// Target should not be a text node (#504, #13143)
				this.target = src.target && src.target.nodeType === 3 ? src.target.parentNode : src.target;
	
				this.currentTarget = src.currentTarget;
				this.relatedTarget = src.relatedTarget;
	
				// Event type
			} else {
				this.type = src;
			}
	
			// Put explicitly provided properties onto the event object
			if (props) {
				jQuery.extend(this, props);
			}
	
			// Create a timestamp if incoming event doesn't have one
			this.timeStamp = src && src.timeStamp || jQuery.now();
	
			// Mark it as fixed
			this[jQuery.expando] = true;
		};
	
		// jQuery.Event is based on DOM3 Events as specified by the ECMAScript Language Binding
		// https://www.w3.org/TR/2003/WD-DOM-Level-3-Events-20030331/ecma-script-binding.html
		jQuery.Event.prototype = {
			constructor: jQuery.Event,
			isDefaultPrevented: returnFalse,
			isPropagationStopped: returnFalse,
			isImmediatePropagationStopped: returnFalse,
			isSimulated: false,
	
			preventDefault: function preventDefault() {
				var e = this.originalEvent;
	
				this.isDefaultPrevented = returnTrue;
	
				if (e && !this.isSimulated) {
					e.preventDefault();
				}
			},
			stopPropagation: function stopPropagation() {
				var e = this.originalEvent;
	
				this.isPropagationStopped = returnTrue;
	
				if (e && !this.isSimulated) {
					e.stopPropagation();
				}
			},
			stopImmediatePropagation: function stopImmediatePropagation() {
				var e = this.originalEvent;
	
				this.isImmediatePropagationStopped = returnTrue;
	
				if (e && !this.isSimulated) {
					e.stopImmediatePropagation();
				}
	
				this.stopPropagation();
			}
		};
	
		// Includes all common event props including KeyEvent and MouseEvent specific props
		jQuery.each({
			altKey: true,
			bubbles: true,
			cancelable: true,
			changedTouches: true,
			ctrlKey: true,
			detail: true,
			eventPhase: true,
			metaKey: true,
			pageX: true,
			pageY: true,
			shiftKey: true,
			view: true,
			"char": true,
			charCode: true,
			key: true,
			keyCode: true,
			button: true,
			buttons: true,
			clientX: true,
			clientY: true,
			offsetX: true,
			offsetY: true,
			pointerId: true,
			pointerType: true,
			screenX: true,
			screenY: true,
			targetTouches: true,
			toElement: true,
			touches: true,
	
			which: function which(event) {
				var button = event.button;
	
				// Add which for key events
				if (event.which == null && rkeyEvent.test(event.type)) {
					return event.charCode != null ? event.charCode : event.keyCode;
				}
	
				// Add which for click: 1 === left; 2 === middle; 3 === right
				if (!event.which && button !== undefined && rmouseEvent.test(event.type)) {
					if (button & 1) {
						return 1;
					}
	
					if (button & 2) {
						return 3;
					}
	
					if (button & 4) {
						return 2;
					}
	
					return 0;
				}
	
				return event.which;
			}
		}, jQuery.event.addProp);
	
		// Create mouseenter/leave events using mouseover/out and event-time checks
		// so that event delegation works in jQuery.
		// Do the same for pointerenter/pointerleave and pointerover/pointerout
		//
		// Support: Safari 7 only
		// Safari sends mouseenter too often; see:
		// https://bugs.chromium.org/p/chromium/issues/detail?id=470258
		// for the description of the bug (it existed in older Chrome versions as well).
		jQuery.each({
			mouseenter: "mouseover",
			mouseleave: "mouseout",
			pointerenter: "pointerover",
			pointerleave: "pointerout"
		}, function (orig, fix) {
			jQuery.event.special[orig] = {
				delegateType: fix,
				bindType: fix,
	
				handle: function handle(event) {
					var ret,
					    target = this,
					    related = event.relatedTarget,
					    handleObj = event.handleObj;
	
					// For mouseenter/leave call the handler if related is outside the target.
					// NB: No relatedTarget if the mouse left/entered the browser window
					if (!related || related !== target && !jQuery.contains(target, related)) {
						event.type = handleObj.origType;
						ret = handleObj.handler.apply(this, arguments);
						event.type = fix;
					}
					return ret;
				}
			};
		});
	
		jQuery.fn.extend({
	
			on: function on(types, selector, data, fn) {
				return _on(this, types, selector, data, fn);
			},
			one: function one(types, selector, data, fn) {
				return _on(this, types, selector, data, fn, 1);
			},
			off: function off(types, selector, fn) {
				var handleObj, type;
				if (types && types.preventDefault && types.handleObj) {
	
					// ( event )  dispatched jQuery.Event
					handleObj = types.handleObj;
					jQuery(types.delegateTarget).off(handleObj.namespace ? handleObj.origType + "." + handleObj.namespace : handleObj.origType, handleObj.selector, handleObj.handler);
					return this;
				}
				if ((typeof types === "undefined" ? "undefined" : _typeof(types)) === "object") {
	
					// ( types-object [, selector] )
					for (type in types) {
						this.off(type, selector, types[type]);
					}
					return this;
				}
				if (selector === false || typeof selector === "function") {
	
					// ( types [, fn] )
					fn = selector;
					selector = undefined;
				}
				if (fn === false) {
					fn = returnFalse;
				}
				return this.each(function () {
					jQuery.event.remove(this, types, fn, selector);
				});
			}
		});
	
		var
	
		/* eslint-disable max-len */
	
		// See https://github.com/eslint/eslint/issues/3229
		rxhtmlTag = /<(?!area|br|col|embed|hr|img|input|link|meta|param)(([a-z][^\/\0>\x20\t\r\n\f]*)[^>]*)\/>/gi,
	
	
		/* eslint-enable */
	
		// Support: IE <=10 - 11, Edge 12 - 13
		// In IE/Edge using regex groups here causes severe slowdowns.
		// See https://connect.microsoft.com/IE/feedback/details/1736512/
		rnoInnerhtml = /<script|<style|<link/i,
	
	
		// checked="checked" or checked
		rchecked = /checked\s*(?:[^=]|=\s*.checked.)/i,
		    rscriptTypeMasked = /^true\/(.*)/,
		    rcleanScript = /^\s*<!(?:\[CDATA\[|--)|(?:\]\]|--)>\s*$/g;
	
		function manipulationTarget(elem, content) {
			if (jQuery.nodeName(elem, "table") && jQuery.nodeName(content.nodeType !== 11 ? content : content.firstChild, "tr")) {
	
				return elem.getElementsByTagName("tbody")[0] || elem;
			}
	
			return elem;
		}
	
		// Replace/restore the type attribute of script elements for safe DOM manipulation
		function disableScript(elem) {
			elem.type = (elem.getAttribute("type") !== null) + "/" + elem.type;
			return elem;
		}
		function restoreScript(elem) {
			var match = rscriptTypeMasked.exec(elem.type);
	
			if (match) {
				elem.type = match[1];
			} else {
				elem.removeAttribute("type");
			}
	
			return elem;
		}
	
		function cloneCopyEvent(src, dest) {
			var i, l, type, pdataOld, pdataCur, udataOld, udataCur, events;
	
			if (dest.nodeType !== 1) {
				return;
			}
	
			// 1. Copy private data: events, handlers, etc.
			if (dataPriv.hasData(src)) {
				pdataOld = dataPriv.access(src);
				pdataCur = dataPriv.set(dest, pdataOld);
				events = pdataOld.events;
	
				if (events) {
					delete pdataCur.handle;
					pdataCur.events = {};
	
					for (type in events) {
						for (i = 0, l = events[type].length; i < l; i++) {
							jQuery.event.add(dest, type, events[type][i]);
						}
					}
				}
			}
	
			// 2. Copy user data
			if (dataUser.hasData(src)) {
				udataOld = dataUser.access(src);
				udataCur = jQuery.extend({}, udataOld);
	
				dataUser.set(dest, udataCur);
			}
		}
	
		// Fix IE bugs, see support tests
		function fixInput(src, dest) {
			var nodeName = dest.nodeName.toLowerCase();
	
			// Fails to persist the checked state of a cloned checkbox or radio button.
			if (nodeName === "input" && rcheckableType.test(src.type)) {
				dest.checked = src.checked;
	
				// Fails to return the selected option to the default selected state when cloning options
			} else if (nodeName === "input" || nodeName === "textarea") {
				dest.defaultValue = src.defaultValue;
			}
		}
	
		function domManip(collection, args, callback, ignored) {
	
			// Flatten any nested arrays
			args = concat.apply([], args);
	
			var fragment,
			    first,
			    scripts,
			    hasScripts,
			    node,
			    doc,
			    i = 0,
			    l = collection.length,
			    iNoClone = l - 1,
			    value = args[0],
			    isFunction = jQuery.isFunction(value);
	
			// We can't cloneNode fragments that contain checked, in WebKit
			if (isFunction || l > 1 && typeof value === "string" && !support.checkClone && rchecked.test(value)) {
				return collection.each(function (index) {
					var self = collection.eq(index);
					if (isFunction) {
						args[0] = value.call(this, index, self.html());
					}
					domManip(self, args, callback, ignored);
				});
			}
	
			if (l) {
				fragment = buildFragment(args, collection[0].ownerDocument, false, collection, ignored);
				first = fragment.firstChild;
	
				if (fragment.childNodes.length === 1) {
					fragment = first;
				}
	
				// Require either new content or an interest in ignored elements to invoke the callback
				if (first || ignored) {
					scripts = jQuery.map(getAll(fragment, "script"), disableScript);
					hasScripts = scripts.length;
	
					// Use the original fragment for the last item
					// instead of the first because it can end up
					// being emptied incorrectly in certain situations (#8070).
					for (; i < l; i++) {
						node = fragment;
	
						if (i !== iNoClone) {
							node = jQuery.clone(node, true, true);
	
							// Keep references to cloned scripts for later restoration
							if (hasScripts) {
	
								// Support: Android <=4.0 only, PhantomJS 1 only
								// push.apply(_, arraylike) throws on ancient WebKit
								jQuery.merge(scripts, getAll(node, "script"));
							}
						}
	
						callback.call(collection[i], node, i);
					}
	
					if (hasScripts) {
						doc = scripts[scripts.length - 1].ownerDocument;
	
						// Reenable scripts
						jQuery.map(scripts, restoreScript);
	
						// Evaluate executable scripts on first document insertion
						for (i = 0; i < hasScripts; i++) {
							node = scripts[i];
							if (rscriptType.test(node.type || "") && !dataPriv.access(node, "globalEval") && jQuery.contains(doc, node)) {
	
								if (node.src) {
	
									// Optional AJAX dependency, but won't run scripts if not present
									if (jQuery._evalUrl) {
										jQuery._evalUrl(node.src);
									}
								} else {
									DOMEval(node.textContent.replace(rcleanScript, ""), doc);
								}
							}
						}
					}
				}
			}
	
			return collection;
		}
	
		function _remove(elem, selector, keepData) {
			var node,
			    nodes = selector ? jQuery.filter(selector, elem) : elem,
			    i = 0;
	
			for (; (node = nodes[i]) != null; i++) {
				if (!keepData && node.nodeType === 1) {
					jQuery.cleanData(getAll(node));
				}
	
				if (node.parentNode) {
					if (keepData && jQuery.contains(node.ownerDocument, node)) {
						setGlobalEval(getAll(node, "script"));
					}
					node.parentNode.removeChild(node);
				}
			}
	
			return elem;
		}
	
		jQuery.extend({
			htmlPrefilter: function htmlPrefilter(html) {
				return html.replace(rxhtmlTag, "<$1></$2>");
			},
	
			clone: function clone(elem, dataAndEvents, deepDataAndEvents) {
				var i,
				    l,
				    srcElements,
				    destElements,
				    clone = elem.cloneNode(true),
				    inPage = jQuery.contains(elem.ownerDocument, elem);
	
				// Fix IE cloning issues
				if (!support.noCloneChecked && (elem.nodeType === 1 || elem.nodeType === 11) && !jQuery.isXMLDoc(elem)) {
	
					// We eschew Sizzle here for performance reasons: https://jsperf.com/getall-vs-sizzle/2
					destElements = getAll(clone);
					srcElements = getAll(elem);
	
					for (i = 0, l = srcElements.length; i < l; i++) {
						fixInput(srcElements[i], destElements[i]);
					}
				}
	
				// Copy the events from the original to the clone
				if (dataAndEvents) {
					if (deepDataAndEvents) {
						srcElements = srcElements || getAll(elem);
						destElements = destElements || getAll(clone);
	
						for (i = 0, l = srcElements.length; i < l; i++) {
							cloneCopyEvent(srcElements[i], destElements[i]);
						}
					} else {
						cloneCopyEvent(elem, clone);
					}
				}
	
				// Preserve script evaluation history
				destElements = getAll(clone, "script");
				if (destElements.length > 0) {
					setGlobalEval(destElements, !inPage && getAll(elem, "script"));
				}
	
				// Return the cloned set
				return clone;
			},
	
			cleanData: function cleanData(elems) {
				var data,
				    elem,
				    type,
				    special = jQuery.event.special,
				    i = 0;
	
				for (; (elem = elems[i]) !== undefined; i++) {
					if (acceptData(elem)) {
						if (data = elem[dataPriv.expando]) {
							if (data.events) {
								for (type in data.events) {
									if (special[type]) {
										jQuery.event.remove(elem, type);
	
										// This is a shortcut to avoid jQuery.event.remove's overhead
									} else {
										jQuery.removeEvent(elem, type, data.handle);
									}
								}
							}
	
							// Support: Chrome <=35 - 45+
							// Assign undefined instead of using delete, see Data#remove
							elem[dataPriv.expando] = undefined;
						}
						if (elem[dataUser.expando]) {
	
							// Support: Chrome <=35 - 45+
							// Assign undefined instead of using delete, see Data#remove
							elem[dataUser.expando] = undefined;
						}
					}
				}
			}
		});
	
		jQuery.fn.extend({
			detach: function detach(selector) {
				return _remove(this, selector, true);
			},
	
			remove: function remove(selector) {
				return _remove(this, selector);
			},
	
			text: function text(value) {
				return access(this, function (value) {
					return value === undefined ? jQuery.text(this) : this.empty().each(function () {
						if (this.nodeType === 1 || this.nodeType === 11 || this.nodeType === 9) {
							this.textContent = value;
						}
					});
				}, null, value, arguments.length);
			},
	
			append: function append() {
				return domManip(this, arguments, function (elem) {
					if (this.nodeType === 1 || this.nodeType === 11 || this.nodeType === 9) {
						var target = manipulationTarget(this, elem);
						target.appendChild(elem);
					}
				});
			},
	
			prepend: function prepend() {
				return domManip(this, arguments, function (elem) {
					if (this.nodeType === 1 || this.nodeType === 11 || this.nodeType === 9) {
						var target = manipulationTarget(this, elem);
						target.insertBefore(elem, target.firstChild);
					}
				});
			},
	
			before: function before() {
				return domManip(this, arguments, function (elem) {
					if (this.parentNode) {
						this.parentNode.insertBefore(elem, this);
					}
				});
			},
	
			after: function after() {
				return domManip(this, arguments, function (elem) {
					if (this.parentNode) {
						this.parentNode.insertBefore(elem, this.nextSibling);
					}
				});
			},
	
			empty: function empty() {
				var elem,
				    i = 0;
	
				for (; (elem = this[i]) != null; i++) {
					if (elem.nodeType === 1) {
	
						// Prevent memory leaks
						jQuery.cleanData(getAll(elem, false));
	
						// Remove any remaining nodes
						elem.textContent = "";
					}
				}
	
				return this;
			},
	
			clone: function clone(dataAndEvents, deepDataAndEvents) {
				dataAndEvents = dataAndEvents == null ? false : dataAndEvents;
				deepDataAndEvents = deepDataAndEvents == null ? dataAndEvents : deepDataAndEvents;
	
				return this.map(function () {
					return jQuery.clone(this, dataAndEvents, deepDataAndEvents);
				});
			},
	
			html: function html(value) {
				return access(this, function (value) {
					var elem = this[0] || {},
					    i = 0,
					    l = this.length;
	
					if (value === undefined && elem.nodeType === 1) {
						return elem.innerHTML;
					}
	
					// See if we can take a shortcut and just use innerHTML
					if (typeof value === "string" && !rnoInnerhtml.test(value) && !wrapMap[(rtagName.exec(value) || ["", ""])[1].toLowerCase()]) {
	
						value = jQuery.htmlPrefilter(value);
	
						try {
							for (; i < l; i++) {
								elem = this[i] || {};
	
								// Remove element nodes and prevent memory leaks
								if (elem.nodeType === 1) {
									jQuery.cleanData(getAll(elem, false));
									elem.innerHTML = value;
								}
							}
	
							elem = 0;
	
							// If using innerHTML throws an exception, use the fallback method
						} catch (e) {}
					}
	
					if (elem) {
						this.empty().append(value);
					}
				}, null, value, arguments.length);
			},
	
			replaceWith: function replaceWith() {
				var ignored = [];
	
				// Make the changes, replacing each non-ignored context element with the new content
				return domManip(this, arguments, function (elem) {
					var parent = this.parentNode;
	
					if (jQuery.inArray(this, ignored) < 0) {
						jQuery.cleanData(getAll(this));
						if (parent) {
							parent.replaceChild(elem, this);
						}
					}
	
					// Force callback invocation
				}, ignored);
			}
		});
	
		jQuery.each({
			appendTo: "append",
			prependTo: "prepend",
			insertBefore: "before",
			insertAfter: "after",
			replaceAll: "replaceWith"
		}, function (name, original) {
			jQuery.fn[name] = function (selector) {
				var elems,
				    ret = [],
				    insert = jQuery(selector),
				    last = insert.length - 1,
				    i = 0;
	
				for (; i <= last; i++) {
					elems = i === last ? this : this.clone(true);
					jQuery(insert[i])[original](elems);
	
					// Support: Android <=4.0 only, PhantomJS 1 only
					// .get() because push.apply(_, arraylike) throws on ancient WebKit
					push.apply(ret, elems.get());
				}
	
				return this.pushStack(ret);
			};
		});
		var rmargin = /^margin/;
	
		var rnumnonpx = new RegExp("^(" + pnum + ")(?!px)[a-z%]+$", "i");
	
		var getStyles = function getStyles(elem) {
	
			// Support: IE <=11 only, Firefox <=30 (#15098, #14150)
			// IE throws on elements created in popups
			// FF meanwhile throws on frame elements through "defaultView.getComputedStyle"
			var view = elem.ownerDocument.defaultView;
	
			if (!view || !view.opener) {
				view = window;
			}
	
			return view.getComputedStyle(elem);
		};
	
		(function () {
	
			// Executing both pixelPosition & boxSizingReliable tests require only one layout
			// so they're executed at the same time to save the second computation.
			function computeStyleTests() {
	
				// This is a singleton, we need to execute it only once
				if (!div) {
					return;
				}
	
				div.style.cssText = "box-sizing:border-box;" + "position:relative;display:block;" + "margin:auto;border:1px;padding:1px;" + "top:1%;width:50%";
				div.innerHTML = "";
				documentElement.appendChild(container);
	
				var divStyle = window.getComputedStyle(div);
				pixelPositionVal = divStyle.top !== "1%";
	
				// Support: Android 4.0 - 4.3 only, Firefox <=3 - 44
				reliableMarginLeftVal = divStyle.marginLeft === "2px";
				boxSizingReliableVal = divStyle.width === "4px";
	
				// Support: Android 4.0 - 4.3 only
				// Some styles come back with percentage values, even though they shouldn't
				div.style.marginRight = "50%";
				pixelMarginRightVal = divStyle.marginRight === "4px";
	
				documentElement.removeChild(container);
	
				// Nullify the div so it wouldn't be stored in the memory and
				// it will also be a sign that checks already performed
				div = null;
			}
	
			var pixelPositionVal,
			    boxSizingReliableVal,
			    pixelMarginRightVal,
			    reliableMarginLeftVal,
			    container = document.createElement("div"),
			    div = document.createElement("div");
	
			// Finish early in limited (non-browser) environments
			if (!div.style) {
				return;
			}
	
			// Support: IE <=9 - 11 only
			// Style of cloned element affects source element cloned (#8908)
			div.style.backgroundClip = "content-box";
			div.cloneNode(true).style.backgroundClip = "";
			support.clearCloneStyle = div.style.backgroundClip === "content-box";
	
			container.style.cssText = "border:0;width:8px;height:0;top:0;left:-9999px;" + "padding:0;margin-top:1px;position:absolute";
			container.appendChild(div);
	
			jQuery.extend(support, {
				pixelPosition: function pixelPosition() {
					computeStyleTests();
					return pixelPositionVal;
				},
				boxSizingReliable: function boxSizingReliable() {
					computeStyleTests();
					return boxSizingReliableVal;
				},
				pixelMarginRight: function pixelMarginRight() {
					computeStyleTests();
					return pixelMarginRightVal;
				},
				reliableMarginLeft: function reliableMarginLeft() {
					computeStyleTests();
					return reliableMarginLeftVal;
				}
			});
		})();
	
		function curCSS(elem, name, computed) {
			var width,
			    minWidth,
			    maxWidth,
			    ret,
			    style = elem.style;
	
			computed = computed || getStyles(elem);
	
			// Support: IE <=9 only
			// getPropertyValue is only needed for .css('filter') (#12537)
			if (computed) {
				ret = computed.getPropertyValue(name) || computed[name];
	
				if (ret === "" && !jQuery.contains(elem.ownerDocument, elem)) {
					ret = jQuery.style(elem, name);
				}
	
				// A tribute to the "awesome hack by Dean Edwards"
				// Android Browser returns percentage for some values,
				// but width seems to be reliably pixels.
				// This is against the CSSOM draft spec:
				// https://drafts.csswg.org/cssom/#resolved-values
				if (!support.pixelMarginRight() && rnumnonpx.test(ret) && rmargin.test(name)) {
	
					// Remember the original values
					width = style.width;
					minWidth = style.minWidth;
					maxWidth = style.maxWidth;
	
					// Put in the new values to get a computed value out
					style.minWidth = style.maxWidth = style.width = ret;
					ret = computed.width;
	
					// Revert the changed values
					style.width = width;
					style.minWidth = minWidth;
					style.maxWidth = maxWidth;
				}
			}
	
			return ret !== undefined ?
	
			// Support: IE <=9 - 11 only
			// IE returns zIndex value as an integer.
			ret + "" : ret;
		}
	
		function addGetHookIf(conditionFn, hookFn) {
	
			// Define the hook, we'll check on the first run if it's really needed.
			return {
				get: function get() {
					if (conditionFn()) {
	
						// Hook not needed (or it's not possible to use it due
						// to missing dependency), remove it.
						delete this.get;
						return;
					}
	
					// Hook needed; redefine it so that the support test is not executed again.
					return (this.get = hookFn).apply(this, arguments);
				}
			};
		}
	
		var
	
		// Swappable if display is none or starts with table
		// except "table", "table-cell", or "table-caption"
		// See here for display values: https://developer.mozilla.org/en-US/docs/CSS/display
		rdisplayswap = /^(none|table(?!-c[ea]).+)/,
		    cssShow = { position: "absolute", visibility: "hidden", display: "block" },
		    cssNormalTransform = {
			letterSpacing: "0",
			fontWeight: "400"
		},
		    cssPrefixes = ["Webkit", "Moz", "ms"],
		    emptyStyle = document.createElement("div").style;
	
		// Return a css property mapped to a potentially vendor prefixed property
		function vendorPropName(name) {
	
			// Shortcut for names that are not vendor prefixed
			if (name in emptyStyle) {
				return name;
			}
	
			// Check for vendor prefixed names
			var capName = name[0].toUpperCase() + name.slice(1),
			    i = cssPrefixes.length;
	
			while (i--) {
				name = cssPrefixes[i] + capName;
				if (name in emptyStyle) {
					return name;
				}
			}
		}
	
		function setPositiveNumber(elem, value, subtract) {
	
			// Any relative (+/-) values have already been
			// normalized at this point
			var matches = rcssNum.exec(value);
			return matches ?
	
			// Guard against undefined "subtract", e.g., when used as in cssHooks
			Math.max(0, matches[2] - (subtract || 0)) + (matches[3] || "px") : value;
		}
	
		function augmentWidthOrHeight(elem, name, extra, isBorderBox, styles) {
			var i,
			    val = 0;
	
			// If we already have the right measurement, avoid augmentation
			if (extra === (isBorderBox ? "border" : "content")) {
				i = 4;
	
				// Otherwise initialize for horizontal or vertical properties
			} else {
				i = name === "width" ? 1 : 0;
			}
	
			for (; i < 4; i += 2) {
	
				// Both box models exclude margin, so add it if we want it
				if (extra === "margin") {
					val += jQuery.css(elem, extra + cssExpand[i], true, styles);
				}
	
				if (isBorderBox) {
	
					// border-box includes padding, so remove it if we want content
					if (extra === "content") {
						val -= jQuery.css(elem, "padding" + cssExpand[i], true, styles);
					}
	
					// At this point, extra isn't border nor margin, so remove border
					if (extra !== "margin") {
						val -= jQuery.css(elem, "border" + cssExpand[i] + "Width", true, styles);
					}
				} else {
	
					// At this point, extra isn't content, so add padding
					val += jQuery.css(elem, "padding" + cssExpand[i], true, styles);
	
					// At this point, extra isn't content nor padding, so add border
					if (extra !== "padding") {
						val += jQuery.css(elem, "border" + cssExpand[i] + "Width", true, styles);
					}
				}
			}
	
			return val;
		}
	
		function getWidthOrHeight(elem, name, extra) {
	
			// Start with offset property, which is equivalent to the border-box value
			var val,
			    valueIsBorderBox = true,
			    styles = getStyles(elem),
			    isBorderBox = jQuery.css(elem, "boxSizing", false, styles) === "border-box";
	
			// Support: IE <=11 only
			// Running getBoundingClientRect on a disconnected node
			// in IE throws an error.
			if (elem.getClientRects().length) {
				val = elem.getBoundingClientRect()[name];
			}
	
			// Some non-html elements return undefined for offsetWidth, so check for null/undefined
			// svg - https://bugzilla.mozilla.org/show_bug.cgi?id=649285
			// MathML - https://bugzilla.mozilla.org/show_bug.cgi?id=491668
			if (val <= 0 || val == null) {
	
				// Fall back to computed then uncomputed css if necessary
				val = curCSS(elem, name, styles);
				if (val < 0 || val == null) {
					val = elem.style[name];
				}
	
				// Computed unit is not pixels. Stop here and return.
				if (rnumnonpx.test(val)) {
					return val;
				}
	
				// Check for style in case a browser which returns unreliable values
				// for getComputedStyle silently falls back to the reliable elem.style
				valueIsBorderBox = isBorderBox && (support.boxSizingReliable() || val === elem.style[name]);
	
				// Normalize "", auto, and prepare for extra
				val = parseFloat(val) || 0;
			}
	
			// Use the active box-sizing model to add/subtract irrelevant styles
			return val + augmentWidthOrHeight(elem, name, extra || (isBorderBox ? "border" : "content"), valueIsBorderBox, styles) + "px";
		}
	
		jQuery.extend({
	
			// Add in style property hooks for overriding the default
			// behavior of getting and setting a style property
			cssHooks: {
				opacity: {
					get: function get(elem, computed) {
						if (computed) {
	
							// We should always get a number back from opacity
							var ret = curCSS(elem, "opacity");
							return ret === "" ? "1" : ret;
						}
					}
				}
			},
	
			// Don't automatically add "px" to these possibly-unitless properties
			cssNumber: {
				"animationIterationCount": true,
				"columnCount": true,
				"fillOpacity": true,
				"flexGrow": true,
				"flexShrink": true,
				"fontWeight": true,
				"lineHeight": true,
				"opacity": true,
				"order": true,
				"orphans": true,
				"widows": true,
				"zIndex": true,
				"zoom": true
			},
	
			// Add in properties whose names you wish to fix before
			// setting or getting the value
			cssProps: {
				"float": "cssFloat"
			},
	
			// Get and set the style property on a DOM Node
			style: function style(elem, name, value, extra) {
	
				// Don't set styles on text and comment nodes
				if (!elem || elem.nodeType === 3 || elem.nodeType === 8 || !elem.style) {
					return;
				}
	
				// Make sure that we're working with the right name
				var ret,
				    type,
				    hooks,
				    origName = jQuery.camelCase(name),
				    style = elem.style;
	
				name = jQuery.cssProps[origName] || (jQuery.cssProps[origName] = vendorPropName(origName) || origName);
	
				// Gets hook for the prefixed version, then unprefixed version
				hooks = jQuery.cssHooks[name] || jQuery.cssHooks[origName];
	
				// Check if we're setting a value
				if (value !== undefined) {
					type = typeof value === "undefined" ? "undefined" : _typeof(value);
	
					// Convert "+=" or "-=" to relative numbers (#7345)
					if (type === "string" && (ret = rcssNum.exec(value)) && ret[1]) {
						value = adjustCSS(elem, name, ret);
	
						// Fixes bug #9237
						type = "number";
					}
	
					// Make sure that null and NaN values aren't set (#7116)
					if (value == null || value !== value) {
						return;
					}
	
					// If a number was passed in, add the unit (except for certain CSS properties)
					if (type === "number") {
						value += ret && ret[3] || (jQuery.cssNumber[origName] ? "" : "px");
					}
	
					// background-* props affect original clone's values
					if (!support.clearCloneStyle && value === "" && name.indexOf("background") === 0) {
						style[name] = "inherit";
					}
	
					// If a hook was provided, use that value, otherwise just set the specified value
					if (!hooks || !("set" in hooks) || (value = hooks.set(elem, value, extra)) !== undefined) {
	
						style[name] = value;
					}
				} else {
	
					// If a hook was provided get the non-computed value from there
					if (hooks && "get" in hooks && (ret = hooks.get(elem, false, extra)) !== undefined) {
	
						return ret;
					}
	
					// Otherwise just get the value from the style object
					return style[name];
				}
			},
	
			css: function css(elem, name, extra, styles) {
				var val,
				    num,
				    hooks,
				    origName = jQuery.camelCase(name);
	
				// Make sure that we're working with the right name
				name = jQuery.cssProps[origName] || (jQuery.cssProps[origName] = vendorPropName(origName) || origName);
	
				// Try prefixed name followed by the unprefixed name
				hooks = jQuery.cssHooks[name] || jQuery.cssHooks[origName];
	
				// If a hook was provided get the computed value from there
				if (hooks && "get" in hooks) {
					val = hooks.get(elem, true, extra);
				}
	
				// Otherwise, if a way to get the computed value exists, use that
				if (val === undefined) {
					val = curCSS(elem, name, styles);
				}
	
				// Convert "normal" to computed value
				if (val === "normal" && name in cssNormalTransform) {
					val = cssNormalTransform[name];
				}
	
				// Make numeric if forced or a qualifier was provided and val looks numeric
				if (extra === "" || extra) {
					num = parseFloat(val);
					return extra === true || isFinite(num) ? num || 0 : val;
				}
				return val;
			}
		});
	
		jQuery.each(["height", "width"], function (i, name) {
			jQuery.cssHooks[name] = {
				get: function get(elem, computed, extra) {
					if (computed) {
	
						// Certain elements can have dimension info if we invisibly show them
						// but it must have a current display style that would benefit
						return rdisplayswap.test(jQuery.css(elem, "display")) && (
	
						// Support: Safari 8+
						// Table columns in Safari have non-zero offsetWidth & zero
						// getBoundingClientRect().width unless display is changed.
						// Support: IE <=11 only
						// Running getBoundingClientRect on a disconnected node
						// in IE throws an error.
						!elem.getClientRects().length || !elem.getBoundingClientRect().width) ? swap(elem, cssShow, function () {
							return getWidthOrHeight(elem, name, extra);
						}) : getWidthOrHeight(elem, name, extra);
					}
				},
	
				set: function set(elem, value, extra) {
					var matches,
					    styles = extra && getStyles(elem),
					    subtract = extra && augmentWidthOrHeight(elem, name, extra, jQuery.css(elem, "boxSizing", false, styles) === "border-box", styles);
	
					// Convert to pixels if value adjustment is needed
					if (subtract && (matches = rcssNum.exec(value)) && (matches[3] || "px") !== "px") {
	
						elem.style[name] = value;
						value = jQuery.css(elem, name);
					}
	
					return setPositiveNumber(elem, value, subtract);
				}
			};
		});
	
		jQuery.cssHooks.marginLeft = addGetHookIf(support.reliableMarginLeft, function (elem, computed) {
			if (computed) {
				return (parseFloat(curCSS(elem, "marginLeft")) || elem.getBoundingClientRect().left - swap(elem, { marginLeft: 0 }, function () {
					return elem.getBoundingClientRect().left;
				})) + "px";
			}
		});
	
		// These hooks are used by animate to expand properties
		jQuery.each({
			margin: "",
			padding: "",
			border: "Width"
		}, function (prefix, suffix) {
			jQuery.cssHooks[prefix + suffix] = {
				expand: function expand(value) {
					var i = 0,
					    expanded = {},
	
	
					// Assumes a single number if not a string
					parts = typeof value === "string" ? value.split(" ") : [value];
	
					for (; i < 4; i++) {
						expanded[prefix + cssExpand[i] + suffix] = parts[i] || parts[i - 2] || parts[0];
					}
	
					return expanded;
				}
			};
	
			if (!rmargin.test(prefix)) {
				jQuery.cssHooks[prefix + suffix].set = setPositiveNumber;
			}
		});
	
		jQuery.fn.extend({
			css: function css(name, value) {
				return access(this, function (elem, name, value) {
					var styles,
					    len,
					    map = {},
					    i = 0;
	
					if (jQuery.isArray(name)) {
						styles = getStyles(elem);
						len = name.length;
	
						for (; i < len; i++) {
							map[name[i]] = jQuery.css(elem, name[i], false, styles);
						}
	
						return map;
					}
	
					return value !== undefined ? jQuery.style(elem, name, value) : jQuery.css(elem, name);
				}, name, value, arguments.length > 1);
			}
		});
	
		function Tween(elem, options, prop, end, easing) {
			return new Tween.prototype.init(elem, options, prop, end, easing);
		}
		jQuery.Tween = Tween;
	
		Tween.prototype = {
			constructor: Tween,
			init: function init(elem, options, prop, end, easing, unit) {
				this.elem = elem;
				this.prop = prop;
				this.easing = easing || jQuery.easing._default;
				this.options = options;
				this.start = this.now = this.cur();
				this.end = end;
				this.unit = unit || (jQuery.cssNumber[prop] ? "" : "px");
			},
			cur: function cur() {
				var hooks = Tween.propHooks[this.prop];
	
				return hooks && hooks.get ? hooks.get(this) : Tween.propHooks._default.get(this);
			},
			run: function run(percent) {
				var eased,
				    hooks = Tween.propHooks[this.prop];
	
				if (this.options.duration) {
					this.pos = eased = jQuery.easing[this.easing](percent, this.options.duration * percent, 0, 1, this.options.duration);
				} else {
					this.pos = eased = percent;
				}
				this.now = (this.end - this.start) * eased + this.start;
	
				if (this.options.step) {
					this.options.step.call(this.elem, this.now, this);
				}
	
				if (hooks && hooks.set) {
					hooks.set(this);
				} else {
					Tween.propHooks._default.set(this);
				}
				return this;
			}
		};
	
		Tween.prototype.init.prototype = Tween.prototype;
	
		Tween.propHooks = {
			_default: {
				get: function get(tween) {
					var result;
	
					// Use a property on the element directly when it is not a DOM element,
					// or when there is no matching style property that exists.
					if (tween.elem.nodeType !== 1 || tween.elem[tween.prop] != null && tween.elem.style[tween.prop] == null) {
						return tween.elem[tween.prop];
					}
	
					// Passing an empty string as a 3rd parameter to .css will automatically
					// attempt a parseFloat and fallback to a string if the parse fails.
					// Simple values such as "10px" are parsed to Float;
					// complex values such as "rotate(1rad)" are returned as-is.
					result = jQuery.css(tween.elem, tween.prop, "");
	
					// Empty strings, null, undefined and "auto" are converted to 0.
					return !result || result === "auto" ? 0 : result;
				},
				set: function set(tween) {
	
					// Use step hook for back compat.
					// Use cssHook if its there.
					// Use .style if available and use plain properties where available.
					if (jQuery.fx.step[tween.prop]) {
						jQuery.fx.step[tween.prop](tween);
					} else if (tween.elem.nodeType === 1 && (tween.elem.style[jQuery.cssProps[tween.prop]] != null || jQuery.cssHooks[tween.prop])) {
						jQuery.style(tween.elem, tween.prop, tween.now + tween.unit);
					} else {
						tween.elem[tween.prop] = tween.now;
					}
				}
			}
		};
	
		// Support: IE <=9 only
		// Panic based approach to setting things on disconnected nodes
		Tween.propHooks.scrollTop = Tween.propHooks.scrollLeft = {
			set: function set(tween) {
				if (tween.elem.nodeType && tween.elem.parentNode) {
					tween.elem[tween.prop] = tween.now;
				}
			}
		};
	
		jQuery.easing = {
			linear: function linear(p) {
				return p;
			},
			swing: function swing(p) {
				return 0.5 - Math.cos(p * Math.PI) / 2;
			},
			_default: "swing"
		};
	
		jQuery.fx = Tween.prototype.init;
	
		// Back compat <1.8 extension point
		jQuery.fx.step = {};
	
		var fxNow,
		    timerId,
		    rfxtypes = /^(?:toggle|show|hide)$/,
		    rrun = /queueHooks$/;
	
		function raf() {
			if (timerId) {
				window.requestAnimationFrame(raf);
				jQuery.fx.tick();
			}
		}
	
		// Animations created synchronously will run synchronously
		function createFxNow() {
			window.setTimeout(function () {
				fxNow = undefined;
			});
			return fxNow = jQuery.now();
		}
	
		// Generate parameters to create a standard animation
		function genFx(type, includeWidth) {
			var which,
			    i = 0,
			    attrs = { height: type };
	
			// If we include width, step value is 1 to do all cssExpand values,
			// otherwise step value is 2 to skip over Left and Right
			includeWidth = includeWidth ? 1 : 0;
			for (; i < 4; i += 2 - includeWidth) {
				which = cssExpand[i];
				attrs["margin" + which] = attrs["padding" + which] = type;
			}
	
			if (includeWidth) {
				attrs.opacity = attrs.width = type;
			}
	
			return attrs;
		}
	
		function createTween(value, prop, animation) {
			var tween,
			    collection = (Animation.tweeners[prop] || []).concat(Animation.tweeners["*"]),
			    index = 0,
			    length = collection.length;
			for (; index < length; index++) {
				if (tween = collection[index].call(animation, prop, value)) {
	
					// We're done with this property
					return tween;
				}
			}
		}
	
		function defaultPrefilter(elem, props, opts) {
			var prop,
			    value,
			    toggle,
			    hooks,
			    oldfire,
			    propTween,
			    restoreDisplay,
			    display,
			    isBox = "width" in props || "height" in props,
			    anim = this,
			    orig = {},
			    style = elem.style,
			    hidden = elem.nodeType && isHiddenWithinTree(elem),
			    dataShow = dataPriv.get(elem, "fxshow");
	
			// Queue-skipping animations hijack the fx hooks
			if (!opts.queue) {
				hooks = jQuery._queueHooks(elem, "fx");
				if (hooks.unqueued == null) {
					hooks.unqueued = 0;
					oldfire = hooks.empty.fire;
					hooks.empty.fire = function () {
						if (!hooks.unqueued) {
							oldfire();
						}
					};
				}
				hooks.unqueued++;
	
				anim.always(function () {
	
					// Ensure the complete handler is called before this completes
					anim.always(function () {
						hooks.unqueued--;
						if (!jQuery.queue(elem, "fx").length) {
							hooks.empty.fire();
						}
					});
				});
			}
	
			// Detect show/hide animations
			for (prop in props) {
				value = props[prop];
				if (rfxtypes.test(value)) {
					delete props[prop];
					toggle = toggle || value === "toggle";
					if (value === (hidden ? "hide" : "show")) {
	
						// Pretend to be hidden if this is a "show" and
						// there is still data from a stopped show/hide
						if (value === "show" && dataShow && dataShow[prop] !== undefined) {
							hidden = true;
	
							// Ignore all other no-op show/hide data
						} else {
							continue;
						}
					}
					orig[prop] = dataShow && dataShow[prop] || jQuery.style(elem, prop);
				}
			}
	
			// Bail out if this is a no-op like .hide().hide()
			propTween = !jQuery.isEmptyObject(props);
			if (!propTween && jQuery.isEmptyObject(orig)) {
				return;
			}
	
			// Restrict "overflow" and "display" styles during box animations
			if (isBox && elem.nodeType === 1) {
	
				// Support: IE <=9 - 11, Edge 12 - 13
				// Record all 3 overflow attributes because IE does not infer the shorthand
				// from identically-valued overflowX and overflowY
				opts.overflow = [style.overflow, style.overflowX, style.overflowY];
	
				// Identify a display type, preferring old show/hide data over the CSS cascade
				restoreDisplay = dataShow && dataShow.display;
				if (restoreDisplay == null) {
					restoreDisplay = dataPriv.get(elem, "display");
				}
				display = jQuery.css(elem, "display");
				if (display === "none") {
					if (restoreDisplay) {
						display = restoreDisplay;
					} else {
	
						// Get nonempty value(s) by temporarily forcing visibility
						showHide([elem], true);
						restoreDisplay = elem.style.display || restoreDisplay;
						display = jQuery.css(elem, "display");
						showHide([elem]);
					}
				}
	
				// Animate inline elements as inline-block
				if (display === "inline" || display === "inline-block" && restoreDisplay != null) {
					if (jQuery.css(elem, "float") === "none") {
	
						// Restore the original display value at the end of pure show/hide animations
						if (!propTween) {
							anim.done(function () {
								style.display = restoreDisplay;
							});
							if (restoreDisplay == null) {
								display = style.display;
								restoreDisplay = display === "none" ? "" : display;
							}
						}
						style.display = "inline-block";
					}
				}
			}
	
			if (opts.overflow) {
				style.overflow = "hidden";
				anim.always(function () {
					style.overflow = opts.overflow[0];
					style.overflowX = opts.overflow[1];
					style.overflowY = opts.overflow[2];
				});
			}
	
			// Implement show/hide animations
			propTween = false;
			for (prop in orig) {
	
				// General show/hide setup for this element animation
				if (!propTween) {
					if (dataShow) {
						if ("hidden" in dataShow) {
							hidden = dataShow.hidden;
						}
					} else {
						dataShow = dataPriv.access(elem, "fxshow", { display: restoreDisplay });
					}
	
					// Store hidden/visible for toggle so `.stop().toggle()` "reverses"
					if (toggle) {
						dataShow.hidden = !hidden;
					}
	
					// Show elements before animating them
					if (hidden) {
						showHide([elem], true);
					}
	
					/* eslint-disable no-loop-func */
	
					anim.done(function () {
	
						/* eslint-enable no-loop-func */
	
						// The final step of a "hide" animation is actually hiding the element
						if (!hidden) {
							showHide([elem]);
						}
						dataPriv.remove(elem, "fxshow");
						for (prop in orig) {
							jQuery.style(elem, prop, orig[prop]);
						}
					});
				}
	
				// Per-property setup
				propTween = createTween(hidden ? dataShow[prop] : 0, prop, anim);
				if (!(prop in dataShow)) {
					dataShow[prop] = propTween.start;
					if (hidden) {
						propTween.end = propTween.start;
						propTween.start = 0;
					}
				}
			}
		}
	
		function propFilter(props, specialEasing) {
			var index, name, easing, value, hooks;
	
			// camelCase, specialEasing and expand cssHook pass
			for (index in props) {
				name = jQuery.camelCase(index);
				easing = specialEasing[name];
				value = props[index];
				if (jQuery.isArray(value)) {
					easing = value[1];
					value = props[index] = value[0];
				}
	
				if (index !== name) {
					props[name] = value;
					delete props[index];
				}
	
				hooks = jQuery.cssHooks[name];
				if (hooks && "expand" in hooks) {
					value = hooks.expand(value);
					delete props[name];
	
					// Not quite $.extend, this won't overwrite existing keys.
					// Reusing 'index' because we have the correct "name"
					for (index in value) {
						if (!(index in props)) {
							props[index] = value[index];
							specialEasing[index] = easing;
						}
					}
				} else {
					specialEasing[name] = easing;
				}
			}
		}
	
		function Animation(elem, properties, options) {
			var result,
			    stopped,
			    index = 0,
			    length = Animation.prefilters.length,
			    deferred = jQuery.Deferred().always(function () {
	
				// Don't match elem in the :animated selector
				delete tick.elem;
			}),
			    tick = function tick() {
				if (stopped) {
					return false;
				}
				var currentTime = fxNow || createFxNow(),
				    remaining = Math.max(0, animation.startTime + animation.duration - currentTime),
	
	
				// Support: Android 2.3 only
				// Archaic crash bug won't allow us to use `1 - ( 0.5 || 0 )` (#12497)
				temp = remaining / animation.duration || 0,
				    percent = 1 - temp,
				    index = 0,
				    length = animation.tweens.length;
	
				for (; index < length; index++) {
					animation.tweens[index].run(percent);
				}
	
				deferred.notifyWith(elem, [animation, percent, remaining]);
	
				if (percent < 1 && length) {
					return remaining;
				} else {
					deferred.resolveWith(elem, [animation]);
					return false;
				}
			},
			    animation = deferred.promise({
				elem: elem,
				props: jQuery.extend({}, properties),
				opts: jQuery.extend(true, {
					specialEasing: {},
					easing: jQuery.easing._default
				}, options),
				originalProperties: properties,
				originalOptions: options,
				startTime: fxNow || createFxNow(),
				duration: options.duration,
				tweens: [],
				createTween: function createTween(prop, end) {
					var tween = jQuery.Tween(elem, animation.opts, prop, end, animation.opts.specialEasing[prop] || animation.opts.easing);
					animation.tweens.push(tween);
					return tween;
				},
				stop: function stop(gotoEnd) {
					var index = 0,
	
	
					// If we are going to the end, we want to run all the tweens
					// otherwise we skip this part
					length = gotoEnd ? animation.tweens.length : 0;
					if (stopped) {
						return this;
					}
					stopped = true;
					for (; index < length; index++) {
						animation.tweens[index].run(1);
					}
	
					// Resolve when we played the last frame; otherwise, reject
					if (gotoEnd) {
						deferred.notifyWith(elem, [animation, 1, 0]);
						deferred.resolveWith(elem, [animation, gotoEnd]);
					} else {
						deferred.rejectWith(elem, [animation, gotoEnd]);
					}
					return this;
				}
			}),
			    props = animation.props;
	
			propFilter(props, animation.opts.specialEasing);
	
			for (; index < length; index++) {
				result = Animation.prefilters[index].call(animation, elem, props, animation.opts);
				if (result) {
					if (jQuery.isFunction(result.stop)) {
						jQuery._queueHooks(animation.elem, animation.opts.queue).stop = jQuery.proxy(result.stop, result);
					}
					return result;
				}
			}
	
			jQuery.map(props, createTween, animation);
	
			if (jQuery.isFunction(animation.opts.start)) {
				animation.opts.start.call(elem, animation);
			}
	
			jQuery.fx.timer(jQuery.extend(tick, {
				elem: elem,
				anim: animation,
				queue: animation.opts.queue
			}));
	
			// attach callbacks from options
			return animation.progress(animation.opts.progress).done(animation.opts.done, animation.opts.complete).fail(animation.opts.fail).always(animation.opts.always);
		}
	
		jQuery.Animation = jQuery.extend(Animation, {
	
			tweeners: {
				"*": [function (prop, value) {
					var tween = this.createTween(prop, value);
					adjustCSS(tween.elem, prop, rcssNum.exec(value), tween);
					return tween;
				}]
			},
	
			tweener: function tweener(props, callback) {
				if (jQuery.isFunction(props)) {
					callback = props;
					props = ["*"];
				} else {
					props = props.match(rnothtmlwhite);
				}
	
				var prop,
				    index = 0,
				    length = props.length;
	
				for (; index < length; index++) {
					prop = props[index];
					Animation.tweeners[prop] = Animation.tweeners[prop] || [];
					Animation.tweeners[prop].unshift(callback);
				}
			},
	
			prefilters: [defaultPrefilter],
	
			prefilter: function prefilter(callback, prepend) {
				if (prepend) {
					Animation.prefilters.unshift(callback);
				} else {
					Animation.prefilters.push(callback);
				}
			}
		});
	
		jQuery.speed = function (speed, easing, fn) {
			var opt = speed && (typeof speed === "undefined" ? "undefined" : _typeof(speed)) === "object" ? jQuery.extend({}, speed) : {
				complete: fn || !fn && easing || jQuery.isFunction(speed) && speed,
				duration: speed,
				easing: fn && easing || easing && !jQuery.isFunction(easing) && easing
			};
	
			// Go to the end state if fx are off or if document is hidden
			if (jQuery.fx.off || document.hidden) {
				opt.duration = 0;
			} else {
				if (typeof opt.duration !== "number") {
					if (opt.duration in jQuery.fx.speeds) {
						opt.duration = jQuery.fx.speeds[opt.duration];
					} else {
						opt.duration = jQuery.fx.speeds._default;
					}
				}
			}
	
			// Normalize opt.queue - true/undefined/null -> "fx"
			if (opt.queue == null || opt.queue === true) {
				opt.queue = "fx";
			}
	
			// Queueing
			opt.old = opt.complete;
	
			opt.complete = function () {
				if (jQuery.isFunction(opt.old)) {
					opt.old.call(this);
				}
	
				if (opt.queue) {
					jQuery.dequeue(this, opt.queue);
				}
			};
	
			return opt;
		};
	
		jQuery.fn.extend({
			fadeTo: function fadeTo(speed, to, easing, callback) {
	
				// Show any hidden elements after setting opacity to 0
				return this.filter(isHiddenWithinTree).css("opacity", 0).show()
	
				// Animate to the value specified
				.end().animate({ opacity: to }, speed, easing, callback);
			},
			animate: function animate(prop, speed, easing, callback) {
				var empty = jQuery.isEmptyObject(prop),
				    optall = jQuery.speed(speed, easing, callback),
				    doAnimation = function doAnimation() {
	
					// Operate on a copy of prop so per-property easing won't be lost
					var anim = Animation(this, jQuery.extend({}, prop), optall);
	
					// Empty animations, or finishing resolves immediately
					if (empty || dataPriv.get(this, "finish")) {
						anim.stop(true);
					}
				};
				doAnimation.finish = doAnimation;
	
				return empty || optall.queue === false ? this.each(doAnimation) : this.queue(optall.queue, doAnimation);
			},
			stop: function stop(type, clearQueue, gotoEnd) {
				var stopQueue = function stopQueue(hooks) {
					var stop = hooks.stop;
					delete hooks.stop;
					stop(gotoEnd);
				};
	
				if (typeof type !== "string") {
					gotoEnd = clearQueue;
					clearQueue = type;
					type = undefined;
				}
				if (clearQueue && type !== false) {
					this.queue(type || "fx", []);
				}
	
				return this.each(function () {
					var dequeue = true,
					    index = type != null && type + "queueHooks",
					    timers = jQuery.timers,
					    data = dataPriv.get(this);
	
					if (index) {
						if (data[index] && data[index].stop) {
							stopQueue(data[index]);
						}
					} else {
						for (index in data) {
							if (data[index] && data[index].stop && rrun.test(index)) {
								stopQueue(data[index]);
							}
						}
					}
	
					for (index = timers.length; index--;) {
						if (timers[index].elem === this && (type == null || timers[index].queue === type)) {
	
							timers[index].anim.stop(gotoEnd);
							dequeue = false;
							timers.splice(index, 1);
						}
					}
	
					// Start the next in the queue if the last step wasn't forced.
					// Timers currently will call their complete callbacks, which
					// will dequeue but only if they were gotoEnd.
					if (dequeue || !gotoEnd) {
						jQuery.dequeue(this, type);
					}
				});
			},
			finish: function finish(type) {
				if (type !== false) {
					type = type || "fx";
				}
				return this.each(function () {
					var index,
					    data = dataPriv.get(this),
					    queue = data[type + "queue"],
					    hooks = data[type + "queueHooks"],
					    timers = jQuery.timers,
					    length = queue ? queue.length : 0;
	
					// Enable finishing flag on private data
					data.finish = true;
	
					// Empty the queue first
					jQuery.queue(this, type, []);
	
					if (hooks && hooks.stop) {
						hooks.stop.call(this, true);
					}
	
					// Look for any active animations, and finish them
					for (index = timers.length; index--;) {
						if (timers[index].elem === this && timers[index].queue === type) {
							timers[index].anim.stop(true);
							timers.splice(index, 1);
						}
					}
	
					// Look for any animations in the old queue and finish them
					for (index = 0; index < length; index++) {
						if (queue[index] && queue[index].finish) {
							queue[index].finish.call(this);
						}
					}
	
					// Turn off finishing flag
					delete data.finish;
				});
			}
		});
	
		jQuery.each(["toggle", "show", "hide"], function (i, name) {
			var cssFn = jQuery.fn[name];
			jQuery.fn[name] = function (speed, easing, callback) {
				return speed == null || typeof speed === "boolean" ? cssFn.apply(this, arguments) : this.animate(genFx(name, true), speed, easing, callback);
			};
		});
	
		// Generate shortcuts for custom animations
		jQuery.each({
			slideDown: genFx("show"),
			slideUp: genFx("hide"),
			slideToggle: genFx("toggle"),
			fadeIn: { opacity: "show" },
			fadeOut: { opacity: "hide" },
			fadeToggle: { opacity: "toggle" }
		}, function (name, props) {
			jQuery.fn[name] = function (speed, easing, callback) {
				return this.animate(props, speed, easing, callback);
			};
		});
	
		jQuery.timers = [];
		jQuery.fx.tick = function () {
			var timer,
			    i = 0,
			    timers = jQuery.timers;
	
			fxNow = jQuery.now();
	
			for (; i < timers.length; i++) {
				timer = timers[i];
	
				// Checks the timer has not already been removed
				if (!timer() && timers[i] === timer) {
					timers.splice(i--, 1);
				}
			}
	
			if (!timers.length) {
				jQuery.fx.stop();
			}
			fxNow = undefined;
		};
	
		jQuery.fx.timer = function (timer) {
			jQuery.timers.push(timer);
			if (timer()) {
				jQuery.fx.start();
			} else {
				jQuery.timers.pop();
			}
		};
	
		jQuery.fx.interval = 13;
		jQuery.fx.start = function () {
			if (!timerId) {
				timerId = window.requestAnimationFrame ? window.requestAnimationFrame(raf) : window.setInterval(jQuery.fx.tick, jQuery.fx.interval);
			}
		};
	
		jQuery.fx.stop = function () {
			if (window.cancelAnimationFrame) {
				window.cancelAnimationFrame(timerId);
			} else {
				window.clearInterval(timerId);
			}
	
			timerId = null;
		};
	
		jQuery.fx.speeds = {
			slow: 600,
			fast: 200,
	
			// Default speed
			_default: 400
		};
	
		// Based off of the plugin by Clint Helfers, with permission.
		// https://web.archive.org/web/20100324014747/http://blindsignals.com/index.php/2009/07/jquery-delay/
		jQuery.fn.delay = function (time, type) {
			time = jQuery.fx ? jQuery.fx.speeds[time] || time : time;
			type = type || "fx";
	
			return this.queue(type, function (next, hooks) {
				var timeout = window.setTimeout(next, time);
				hooks.stop = function () {
					window.clearTimeout(timeout);
				};
			});
		};
	
		(function () {
			var input = document.createElement("input"),
			    select = document.createElement("select"),
			    opt = select.appendChild(document.createElement("option"));
	
			input.type = "checkbox";
	
			// Support: Android <=4.3 only
			// Default value for a checkbox should be "on"
			support.checkOn = input.value !== "";
	
			// Support: IE <=11 only
			// Must access selectedIndex to make default options select
			support.optSelected = opt.selected;
	
			// Support: IE <=11 only
			// An input loses its value after becoming a radio
			input = document.createElement("input");
			input.value = "t";
			input.type = "radio";
			support.radioValue = input.value === "t";
		})();
	
		var boolHook,
		    attrHandle = jQuery.expr.attrHandle;
	
		jQuery.fn.extend({
			attr: function attr(name, value) {
				return access(this, jQuery.attr, name, value, arguments.length > 1);
			},
	
			removeAttr: function removeAttr(name) {
				return this.each(function () {
					jQuery.removeAttr(this, name);
				});
			}
		});
	
		jQuery.extend({
			attr: function attr(elem, name, value) {
				var ret,
				    hooks,
				    nType = elem.nodeType;
	
				// Don't get/set attributes on text, comment and attribute nodes
				if (nType === 3 || nType === 8 || nType === 2) {
					return;
				}
	
				// Fallback to prop when attributes are not supported
				if (typeof elem.getAttribute === "undefined") {
					return jQuery.prop(elem, name, value);
				}
	
				// Attribute hooks are determined by the lowercase version
				// Grab necessary hook if one is defined
				if (nType !== 1 || !jQuery.isXMLDoc(elem)) {
					hooks = jQuery.attrHooks[name.toLowerCase()] || (jQuery.expr.match.bool.test(name) ? boolHook : undefined);
				}
	
				if (value !== undefined) {
					if (value === null) {
						jQuery.removeAttr(elem, name);
						return;
					}
	
					if (hooks && "set" in hooks && (ret = hooks.set(elem, value, name)) !== undefined) {
						return ret;
					}
	
					elem.setAttribute(name, value + "");
					return value;
				}
	
				if (hooks && "get" in hooks && (ret = hooks.get(elem, name)) !== null) {
					return ret;
				}
	
				ret = jQuery.find.attr(elem, name);
	
				// Non-existent attributes return null, we normalize to undefined
				return ret == null ? undefined : ret;
			},
	
			attrHooks: {
				type: {
					set: function set(elem, value) {
						if (!support.radioValue && value === "radio" && jQuery.nodeName(elem, "input")) {
							var val = elem.value;
							elem.setAttribute("type", value);
							if (val) {
								elem.value = val;
							}
							return value;
						}
					}
				}
			},
	
			removeAttr: function removeAttr(elem, value) {
				var name,
				    i = 0,
	
	
				// Attribute names can contain non-HTML whitespace characters
				// https://html.spec.whatwg.org/multipage/syntax.html#attributes-2
				attrNames = value && value.match(rnothtmlwhite);
	
				if (attrNames && elem.nodeType === 1) {
					while (name = attrNames[i++]) {
						elem.removeAttribute(name);
					}
				}
			}
		});
	
		// Hooks for boolean attributes
		boolHook = {
			set: function set(elem, value, name) {
				if (value === false) {
	
					// Remove boolean attributes when set to false
					jQuery.removeAttr(elem, name);
				} else {
					elem.setAttribute(name, name);
				}
				return name;
			}
		};
	
		jQuery.each(jQuery.expr.match.bool.source.match(/\w+/g), function (i, name) {
			var getter = attrHandle[name] || jQuery.find.attr;
	
			attrHandle[name] = function (elem, name, isXML) {
				var ret,
				    handle,
				    lowercaseName = name.toLowerCase();
	
				if (!isXML) {
	
					// Avoid an infinite loop by temporarily removing this function from the getter
					handle = attrHandle[lowercaseName];
					attrHandle[lowercaseName] = ret;
					ret = getter(elem, name, isXML) != null ? lowercaseName : null;
					attrHandle[lowercaseName] = handle;
				}
				return ret;
			};
		});
	
		var rfocusable = /^(?:input|select|textarea|button)$/i,
		    rclickable = /^(?:a|area)$/i;
	
		jQuery.fn.extend({
			prop: function prop(name, value) {
				return access(this, jQuery.prop, name, value, arguments.length > 1);
			},
	
			removeProp: function removeProp(name) {
				return this.each(function () {
					delete this[jQuery.propFix[name] || name];
				});
			}
		});
	
		jQuery.extend({
			prop: function prop(elem, name, value) {
				var ret,
				    hooks,
				    nType = elem.nodeType;
	
				// Don't get/set properties on text, comment and attribute nodes
				if (nType === 3 || nType === 8 || nType === 2) {
					return;
				}
	
				if (nType !== 1 || !jQuery.isXMLDoc(elem)) {
	
					// Fix name and attach hooks
					name = jQuery.propFix[name] || name;
					hooks = jQuery.propHooks[name];
				}
	
				if (value !== undefined) {
					if (hooks && "set" in hooks && (ret = hooks.set(elem, value, name)) !== undefined) {
						return ret;
					}
	
					return elem[name] = value;
				}
	
				if (hooks && "get" in hooks && (ret = hooks.get(elem, name)) !== null) {
					return ret;
				}
	
				return elem[name];
			},
	
			propHooks: {
				tabIndex: {
					get: function get(elem) {
	
						// Support: IE <=9 - 11 only
						// elem.tabIndex doesn't always return the
						// correct value when it hasn't been explicitly set
						// https://web.archive.org/web/20141116233347/http://fluidproject.org/blog/2008/01/09/getting-setting-and-removing-tabindex-values-with-javascript/
						// Use proper attribute retrieval(#12072)
						var tabindex = jQuery.find.attr(elem, "tabindex");
	
						if (tabindex) {
							return parseInt(tabindex, 10);
						}
	
						if (rfocusable.test(elem.nodeName) || rclickable.test(elem.nodeName) && elem.href) {
							return 0;
						}
	
						return -1;
					}
				}
			},
	
			propFix: {
				"for": "htmlFor",
				"class": "className"
			}
		});
	
		// Support: IE <=11 only
		// Accessing the selectedIndex property
		// forces the browser to respect setting selected
		// on the option
		// The getter ensures a default option is selected
		// when in an optgroup
		// eslint rule "no-unused-expressions" is disabled for this code
		// since it considers such accessions noop
		if (!support.optSelected) {
			jQuery.propHooks.selected = {
				get: function get(elem) {
	
					/* eslint no-unused-expressions: "off" */
	
					var parent = elem.parentNode;
					if (parent && parent.parentNode) {
						parent.parentNode.selectedIndex;
					}
					return null;
				},
				set: function set(elem) {
	
					/* eslint no-unused-expressions: "off" */
	
					var parent = elem.parentNode;
					if (parent) {
						parent.selectedIndex;
	
						if (parent.parentNode) {
							parent.parentNode.selectedIndex;
						}
					}
				}
			};
		}
	
		jQuery.each(["tabIndex", "readOnly", "maxLength", "cellSpacing", "cellPadding", "rowSpan", "colSpan", "useMap", "frameBorder", "contentEditable"], function () {
			jQuery.propFix[this.toLowerCase()] = this;
		});
	
		// Strip and collapse whitespace according to HTML spec
		// https://html.spec.whatwg.org/multipage/infrastructure.html#strip-and-collapse-whitespace
		function stripAndCollapse(value) {
			var tokens = value.match(rnothtmlwhite) || [];
			return tokens.join(" ");
		}
	
		function getClass(elem) {
			return elem.getAttribute && elem.getAttribute("class") || "";
		}
	
		jQuery.fn.extend({
			addClass: function addClass(value) {
				var classes,
				    elem,
				    cur,
				    curValue,
				    clazz,
				    j,
				    finalValue,
				    i = 0;
	
				if (jQuery.isFunction(value)) {
					return this.each(function (j) {
						jQuery(this).addClass(value.call(this, j, getClass(this)));
					});
				}
	
				if (typeof value === "string" && value) {
					classes = value.match(rnothtmlwhite) || [];
	
					while (elem = this[i++]) {
						curValue = getClass(elem);
						cur = elem.nodeType === 1 && " " + stripAndCollapse(curValue) + " ";
	
						if (cur) {
							j = 0;
							while (clazz = classes[j++]) {
								if (cur.indexOf(" " + clazz + " ") < 0) {
									cur += clazz + " ";
								}
							}
	
							// Only assign if different to avoid unneeded rendering.
							finalValue = stripAndCollapse(cur);
							if (curValue !== finalValue) {
								elem.setAttribute("class", finalValue);
							}
						}
					}
				}
	
				return this;
			},
	
			removeClass: function removeClass(value) {
				var classes,
				    elem,
				    cur,
				    curValue,
				    clazz,
				    j,
				    finalValue,
				    i = 0;
	
				if (jQuery.isFunction(value)) {
					return this.each(function (j) {
						jQuery(this).removeClass(value.call(this, j, getClass(this)));
					});
				}
	
				if (!arguments.length) {
					return this.attr("class", "");
				}
	
				if (typeof value === "string" && value) {
					classes = value.match(rnothtmlwhite) || [];
	
					while (elem = this[i++]) {
						curValue = getClass(elem);
	
						// This expression is here for better compressibility (see addClass)
						cur = elem.nodeType === 1 && " " + stripAndCollapse(curValue) + " ";
	
						if (cur) {
							j = 0;
							while (clazz = classes[j++]) {
	
								// Remove *all* instances
								while (cur.indexOf(" " + clazz + " ") > -1) {
									cur = cur.replace(" " + clazz + " ", " ");
								}
							}
	
							// Only assign if different to avoid unneeded rendering.
							finalValue = stripAndCollapse(cur);
							if (curValue !== finalValue) {
								elem.setAttribute("class", finalValue);
							}
						}
					}
				}
	
				return this;
			},
	
			toggleClass: function toggleClass(value, stateVal) {
				var type = typeof value === "undefined" ? "undefined" : _typeof(value);
	
				if (typeof stateVal === "boolean" && type === "string") {
					return stateVal ? this.addClass(value) : this.removeClass(value);
				}
	
				if (jQuery.isFunction(value)) {
					return this.each(function (i) {
						jQuery(this).toggleClass(value.call(this, i, getClass(this), stateVal), stateVal);
					});
				}
	
				return this.each(function () {
					var className, i, self, classNames;
	
					if (type === "string") {
	
						// Toggle individual class names
						i = 0;
						self = jQuery(this);
						classNames = value.match(rnothtmlwhite) || [];
	
						while (className = classNames[i++]) {
	
							// Check each className given, space separated list
							if (self.hasClass(className)) {
								self.removeClass(className);
							} else {
								self.addClass(className);
							}
						}
	
						// Toggle whole class name
					} else if (value === undefined || type === "boolean") {
						className = getClass(this);
						if (className) {
	
							// Store className if set
							dataPriv.set(this, "__className__", className);
						}
	
						// If the element has a class name or if we're passed `false`,
						// then remove the whole classname (if there was one, the above saved it).
						// Otherwise bring back whatever was previously saved (if anything),
						// falling back to the empty string if nothing was stored.
						if (this.setAttribute) {
							this.setAttribute("class", className || value === false ? "" : dataPriv.get(this, "__className__") || "");
						}
					}
				});
			},
	
			hasClass: function hasClass(selector) {
				var className,
				    elem,
				    i = 0;
	
				className = " " + selector + " ";
				while (elem = this[i++]) {
					if (elem.nodeType === 1 && (" " + stripAndCollapse(getClass(elem)) + " ").indexOf(className) > -1) {
						return true;
					}
				}
	
				return false;
			}
		});
	
		var rreturn = /\r/g;
	
		jQuery.fn.extend({
			val: function val(value) {
				var hooks,
				    ret,
				    isFunction,
				    elem = this[0];
	
				if (!arguments.length) {
					if (elem) {
						hooks = jQuery.valHooks[elem.type] || jQuery.valHooks[elem.nodeName.toLowerCase()];
	
						if (hooks && "get" in hooks && (ret = hooks.get(elem, "value")) !== undefined) {
							return ret;
						}
	
						ret = elem.value;
	
						// Handle most common string cases
						if (typeof ret === "string") {
							return ret.replace(rreturn, "");
						}
	
						// Handle cases where value is null/undef or number
						return ret == null ? "" : ret;
					}
	
					return;
				}
	
				isFunction = jQuery.isFunction(value);
	
				return this.each(function (i) {
					var val;
	
					if (this.nodeType !== 1) {
						return;
					}
	
					if (isFunction) {
						val = value.call(this, i, jQuery(this).val());
					} else {
						val = value;
					}
	
					// Treat null/undefined as ""; convert numbers to string
					if (val == null) {
						val = "";
					} else if (typeof val === "number") {
						val += "";
					} else if (jQuery.isArray(val)) {
						val = jQuery.map(val, function (value) {
							return value == null ? "" : value + "";
						});
					}
	
					hooks = jQuery.valHooks[this.type] || jQuery.valHooks[this.nodeName.toLowerCase()];
	
					// If set returns undefined, fall back to normal setting
					if (!hooks || !("set" in hooks) || hooks.set(this, val, "value") === undefined) {
						this.value = val;
					}
				});
			}
		});
	
		jQuery.extend({
			valHooks: {
				option: {
					get: function get(elem) {
	
						var val = jQuery.find.attr(elem, "value");
						return val != null ? val :
	
						// Support: IE <=10 - 11 only
						// option.text throws exceptions (#14686, #14858)
						// Strip and collapse whitespace
						// https://html.spec.whatwg.org/#strip-and-collapse-whitespace
						stripAndCollapse(jQuery.text(elem));
					}
				},
				select: {
					get: function get(elem) {
						var value,
						    option,
						    i,
						    options = elem.options,
						    index = elem.selectedIndex,
						    one = elem.type === "select-one",
						    values = one ? null : [],
						    max = one ? index + 1 : options.length;
	
						if (index < 0) {
							i = max;
						} else {
							i = one ? index : 0;
						}
	
						// Loop through all the selected options
						for (; i < max; i++) {
							option = options[i];
	
							// Support: IE <=9 only
							// IE8-9 doesn't update selected after form reset (#2551)
							if ((option.selected || i === index) &&
	
							// Don't return options that are disabled or in a disabled optgroup
							!option.disabled && (!option.parentNode.disabled || !jQuery.nodeName(option.parentNode, "optgroup"))) {
	
								// Get the specific value for the option
								value = jQuery(option).val();
	
								// We don't need an array for one selects
								if (one) {
									return value;
								}
	
								// Multi-Selects return an array
								values.push(value);
							}
						}
	
						return values;
					},
	
					set: function set(elem, value) {
						var optionSet,
						    option,
						    options = elem.options,
						    values = jQuery.makeArray(value),
						    i = options.length;
	
						while (i--) {
							option = options[i];
	
							/* eslint-disable no-cond-assign */
	
							if (option.selected = jQuery.inArray(jQuery.valHooks.option.get(option), values) > -1) {
								optionSet = true;
							}
	
							/* eslint-enable no-cond-assign */
						}
	
						// Force browsers to behave consistently when non-matching value is set
						if (!optionSet) {
							elem.selectedIndex = -1;
						}
						return values;
					}
				}
			}
		});
	
		// Radios and checkboxes getter/setter
		jQuery.each(["radio", "checkbox"], function () {
			jQuery.valHooks[this] = {
				set: function set(elem, value) {
					if (jQuery.isArray(value)) {
						return elem.checked = jQuery.inArray(jQuery(elem).val(), value) > -1;
					}
				}
			};
			if (!support.checkOn) {
				jQuery.valHooks[this].get = function (elem) {
					return elem.getAttribute("value") === null ? "on" : elem.value;
				};
			}
		});
	
		// Return jQuery for attributes-only inclusion
	
	
		var rfocusMorph = /^(?:focusinfocus|focusoutblur)$/;
	
		jQuery.extend(jQuery.event, {
	
			trigger: function trigger(event, data, elem, onlyHandlers) {
	
				var i,
				    cur,
				    tmp,
				    bubbleType,
				    ontype,
				    handle,
				    special,
				    eventPath = [elem || document],
				    type = hasOwn.call(event, "type") ? event.type : event,
				    namespaces = hasOwn.call(event, "namespace") ? event.namespace.split(".") : [];
	
				cur = tmp = elem = elem || document;
	
				// Don't do events on text and comment nodes
				if (elem.nodeType === 3 || elem.nodeType === 8) {
					return;
				}
	
				// focus/blur morphs to focusin/out; ensure we're not firing them right now
				if (rfocusMorph.test(type + jQuery.event.triggered)) {
					return;
				}
	
				if (type.indexOf(".") > -1) {
	
					// Namespaced trigger; create a regexp to match event type in handle()
					namespaces = type.split(".");
					type = namespaces.shift();
					namespaces.sort();
				}
				ontype = type.indexOf(":") < 0 && "on" + type;
	
				// Caller can pass in a jQuery.Event object, Object, or just an event type string
				event = event[jQuery.expando] ? event : new jQuery.Event(type, (typeof event === "undefined" ? "undefined" : _typeof(event)) === "object" && event);
	
				// Trigger bitmask: & 1 for native handlers; & 2 for jQuery (always true)
				event.isTrigger = onlyHandlers ? 2 : 3;
				event.namespace = namespaces.join(".");
				event.rnamespace = event.namespace ? new RegExp("(^|\\.)" + namespaces.join("\\.(?:.*\\.|)") + "(\\.|$)") : null;
	
				// Clean up the event in case it is being reused
				event.result = undefined;
				if (!event.target) {
					event.target = elem;
				}
	
				// Clone any incoming data and prepend the event, creating the handler arg list
				data = data == null ? [event] : jQuery.makeArray(data, [event]);
	
				// Allow special events to draw outside the lines
				special = jQuery.event.special[type] || {};
				if (!onlyHandlers && special.trigger && special.trigger.apply(elem, data) === false) {
					return;
				}
	
				// Determine event propagation path in advance, per W3C events spec (#9951)
				// Bubble up to document, then to window; watch for a global ownerDocument var (#9724)
				if (!onlyHandlers && !special.noBubble && !jQuery.isWindow(elem)) {
	
					bubbleType = special.delegateType || type;
					if (!rfocusMorph.test(bubbleType + type)) {
						cur = cur.parentNode;
					}
					for (; cur; cur = cur.parentNode) {
						eventPath.push(cur);
						tmp = cur;
					}
	
					// Only add window if we got to document (e.g., not plain obj or detached DOM)
					if (tmp === (elem.ownerDocument || document)) {
						eventPath.push(tmp.defaultView || tmp.parentWindow || window);
					}
				}
	
				// Fire handlers on the event path
				i = 0;
				while ((cur = eventPath[i++]) && !event.isPropagationStopped()) {
	
					event.type = i > 1 ? bubbleType : special.bindType || type;
	
					// jQuery handler
					handle = (dataPriv.get(cur, "events") || {})[event.type] && dataPriv.get(cur, "handle");
					if (handle) {
						handle.apply(cur, data);
					}
	
					// Native handler
					handle = ontype && cur[ontype];
					if (handle && handle.apply && acceptData(cur)) {
						event.result = handle.apply(cur, data);
						if (event.result === false) {
							event.preventDefault();
						}
					}
				}
				event.type = type;
	
				// If nobody prevented the default action, do it now
				if (!onlyHandlers && !event.isDefaultPrevented()) {
	
					if ((!special._default || special._default.apply(eventPath.pop(), data) === false) && acceptData(elem)) {
	
						// Call a native DOM method on the target with the same name as the event.
						// Don't do default actions on window, that's where global variables be (#6170)
						if (ontype && jQuery.isFunction(elem[type]) && !jQuery.isWindow(elem)) {
	
							// Don't re-trigger an onFOO event when we call its FOO() method
							tmp = elem[ontype];
	
							if (tmp) {
								elem[ontype] = null;
							}
	
							// Prevent re-triggering of the same event, since we already bubbled it above
							jQuery.event.triggered = type;
							elem[type]();
							jQuery.event.triggered = undefined;
	
							if (tmp) {
								elem[ontype] = tmp;
							}
						}
					}
				}
	
				return event.result;
			},
	
			// Piggyback on a donor event to simulate a different one
			// Used only for `focus(in | out)` events
			simulate: function simulate(type, elem, event) {
				var e = jQuery.extend(new jQuery.Event(), event, {
					type: type,
					isSimulated: true
				});
	
				jQuery.event.trigger(e, null, elem);
			}
	
		});
	
		jQuery.fn.extend({
	
			trigger: function trigger(type, data) {
				return this.each(function () {
					jQuery.event.trigger(type, data, this);
				});
			},
			triggerHandler: function triggerHandler(type, data) {
				var elem = this[0];
				if (elem) {
					return jQuery.event.trigger(type, data, elem, true);
				}
			}
		});
	
		jQuery.each(("blur focus focusin focusout resize scroll click dblclick " + "mousedown mouseup mousemove mouseover mouseout mouseenter mouseleave " + "change select submit keydown keypress keyup contextmenu").split(" "), function (i, name) {
	
			// Handle event binding
			jQuery.fn[name] = function (data, fn) {
				return arguments.length > 0 ? this.on(name, null, data, fn) : this.trigger(name);
			};
		});
	
		jQuery.fn.extend({
			hover: function hover(fnOver, fnOut) {
				return this.mouseenter(fnOver).mouseleave(fnOut || fnOver);
			}
		});
	
		support.focusin = "onfocusin" in window;
	
		// Support: Firefox <=44
		// Firefox doesn't have focus(in | out) events
		// Related ticket - https://bugzilla.mozilla.org/show_bug.cgi?id=687787
		//
		// Support: Chrome <=48 - 49, Safari <=9.0 - 9.1
		// focus(in | out) events fire after focus & blur events,
		// which is spec violation - http://www.w3.org/TR/DOM-Level-3-Events/#events-focusevent-event-order
		// Related ticket - https://bugs.chromium.org/p/chromium/issues/detail?id=449857
		if (!support.focusin) {
			jQuery.each({ focus: "focusin", blur: "focusout" }, function (orig, fix) {
	
				// Attach a single capturing handler on the document while someone wants focusin/focusout
				var handler = function handler(event) {
					jQuery.event.simulate(fix, event.target, jQuery.event.fix(event));
				};
	
				jQuery.event.special[fix] = {
					setup: function setup() {
						var doc = this.ownerDocument || this,
						    attaches = dataPriv.access(doc, fix);
	
						if (!attaches) {
							doc.addEventListener(orig, handler, true);
						}
						dataPriv.access(doc, fix, (attaches || 0) + 1);
					},
					teardown: function teardown() {
						var doc = this.ownerDocument || this,
						    attaches = dataPriv.access(doc, fix) - 1;
	
						if (!attaches) {
							doc.removeEventListener(orig, handler, true);
							dataPriv.remove(doc, fix);
						} else {
							dataPriv.access(doc, fix, attaches);
						}
					}
				};
			});
		}
		var location = window.location;
	
		var nonce = jQuery.now();
	
		var rquery = /\?/;
	
		// Cross-browser xml parsing
		jQuery.parseXML = function (data) {
			var xml;
			if (!data || typeof data !== "string") {
				return null;
			}
	
			// Support: IE 9 - 11 only
			// IE throws on parseFromString with invalid input.
			try {
				xml = new window.DOMParser().parseFromString(data, "text/xml");
			} catch (e) {
				xml = undefined;
			}
	
			if (!xml || xml.getElementsByTagName("parsererror").length) {
				jQuery.error("Invalid XML: " + data);
			}
			return xml;
		};
	
		var rbracket = /\[\]$/,
		    rCRLF = /\r?\n/g,
		    rsubmitterTypes = /^(?:submit|button|image|reset|file)$/i,
		    rsubmittable = /^(?:input|select|textarea|keygen)/i;
	
		function buildParams(prefix, obj, traditional, add) {
			var name;
	
			if (jQuery.isArray(obj)) {
	
				// Serialize array item.
				jQuery.each(obj, function (i, v) {
					if (traditional || rbracket.test(prefix)) {
	
						// Treat each array item as a scalar.
						add(prefix, v);
					} else {
	
						// Item is non-scalar (array or object), encode its numeric index.
						buildParams(prefix + "[" + ((typeof v === "undefined" ? "undefined" : _typeof(v)) === "object" && v != null ? i : "") + "]", v, traditional, add);
					}
				});
			} else if (!traditional && jQuery.type(obj) === "object") {
	
				// Serialize object item.
				for (name in obj) {
					buildParams(prefix + "[" + name + "]", obj[name], traditional, add);
				}
			} else {
	
				// Serialize scalar item.
				add(prefix, obj);
			}
		}
	
		// Serialize an array of form elements or a set of
		// key/values into a query string
		jQuery.param = function (a, traditional) {
			var prefix,
			    s = [],
			    add = function add(key, valueOrFunction) {
	
				// If value is a function, invoke it and use its return value
				var value = jQuery.isFunction(valueOrFunction) ? valueOrFunction() : valueOrFunction;
	
				s[s.length] = encodeURIComponent(key) + "=" + encodeURIComponent(value == null ? "" : value);
			};
	
			// If an array was passed in, assume that it is an array of form elements.
			if (jQuery.isArray(a) || a.jquery && !jQuery.isPlainObject(a)) {
	
				// Serialize the form elements
				jQuery.each(a, function () {
					add(this.name, this.value);
				});
			} else {
	
				// If traditional, encode the "old" way (the way 1.3.2 or older
				// did it), otherwise encode params recursively.
				for (prefix in a) {
					buildParams(prefix, a[prefix], traditional, add);
				}
			}
	
			// Return the resulting serialization
			return s.join("&");
		};
	
		jQuery.fn.extend({
			serialize: function serialize() {
				return jQuery.param(this.serializeArray());
			},
			serializeArray: function serializeArray() {
				return this.map(function () {
	
					// Can add propHook for "elements" to filter or add form elements
					var elements = jQuery.prop(this, "elements");
					return elements ? jQuery.makeArray(elements) : this;
				}).filter(function () {
					var type = this.type;
	
					// Use .is( ":disabled" ) so that fieldset[disabled] works
					return this.name && !jQuery(this).is(":disabled") && rsubmittable.test(this.nodeName) && !rsubmitterTypes.test(type) && (this.checked || !rcheckableType.test(type));
				}).map(function (i, elem) {
					var val = jQuery(this).val();
	
					if (val == null) {
						return null;
					}
	
					if (jQuery.isArray(val)) {
						return jQuery.map(val, function (val) {
							return { name: elem.name, value: val.replace(rCRLF, "\r\n") };
						});
					}
	
					return { name: elem.name, value: val.replace(rCRLF, "\r\n") };
				}).get();
			}
		});
	
		var r20 = /%20/g,
		    rhash = /#.*$/,
		    rantiCache = /([?&])_=[^&]*/,
		    rheaders = /^(.*?):[ \t]*([^\r\n]*)$/mg,
	
	
		// #7653, #8125, #8152: local protocol detection
		rlocalProtocol = /^(?:about|app|app-storage|.+-extension|file|res|widget):$/,
		    rnoContent = /^(?:GET|HEAD)$/,
		    rprotocol = /^\/\//,
	
	
		/* Prefilters
	  * 1) They are useful to introduce custom dataTypes (see ajax/jsonp.js for an example)
	  * 2) These are called:
	  *    - BEFORE asking for a transport
	  *    - AFTER param serialization (s.data is a string if s.processData is true)
	  * 3) key is the dataType
	  * 4) the catchall symbol "*" can be used
	  * 5) execution will start with transport dataType and THEN continue down to "*" if needed
	  */
		prefilters = {},
	
	
		/* Transports bindings
	  * 1) key is the dataType
	  * 2) the catchall symbol "*" can be used
	  * 3) selection will start with transport dataType and THEN go to "*" if needed
	  */
		transports = {},
	
	
		// Avoid comment-prolog char sequence (#10098); must appease lint and evade compression
		allTypes = "*/".concat("*"),
	
	
		// Anchor tag for parsing the document origin
		originAnchor = document.createElement("a");
		originAnchor.href = location.href;
	
		// Base "constructor" for jQuery.ajaxPrefilter and jQuery.ajaxTransport
		function addToPrefiltersOrTransports(structure) {
	
			// dataTypeExpression is optional and defaults to "*"
			return function (dataTypeExpression, func) {
	
				if (typeof dataTypeExpression !== "string") {
					func = dataTypeExpression;
					dataTypeExpression = "*";
				}
	
				var dataType,
				    i = 0,
				    dataTypes = dataTypeExpression.toLowerCase().match(rnothtmlwhite) || [];
	
				if (jQuery.isFunction(func)) {
	
					// For each dataType in the dataTypeExpression
					while (dataType = dataTypes[i++]) {
	
						// Prepend if requested
						if (dataType[0] === "+") {
							dataType = dataType.slice(1) || "*";
							(structure[dataType] = structure[dataType] || []).unshift(func);
	
							// Otherwise append
						} else {
							(structure[dataType] = structure[dataType] || []).push(func);
						}
					}
				}
			};
		}
	
		// Base inspection function for prefilters and transports
		function inspectPrefiltersOrTransports(structure, options, originalOptions, jqXHR) {
	
			var inspected = {},
			    seekingTransport = structure === transports;
	
			function inspect(dataType) {
				var selected;
				inspected[dataType] = true;
				jQuery.each(structure[dataType] || [], function (_, prefilterOrFactory) {
					var dataTypeOrTransport = prefilterOrFactory(options, originalOptions, jqXHR);
					if (typeof dataTypeOrTransport === "string" && !seekingTransport && !inspected[dataTypeOrTransport]) {
	
						options.dataTypes.unshift(dataTypeOrTransport);
						inspect(dataTypeOrTransport);
						return false;
					} else if (seekingTransport) {
						return !(selected = dataTypeOrTransport);
					}
				});
				return selected;
			}
	
			return inspect(options.dataTypes[0]) || !inspected["*"] && inspect("*");
		}
	
		// A special extend for ajax options
		// that takes "flat" options (not to be deep extended)
		// Fixes #9887
		function ajaxExtend(target, src) {
			var key,
			    deep,
			    flatOptions = jQuery.ajaxSettings.flatOptions || {};
	
			for (key in src) {
				if (src[key] !== undefined) {
					(flatOptions[key] ? target : deep || (deep = {}))[key] = src[key];
				}
			}
			if (deep) {
				jQuery.extend(true, target, deep);
			}
	
			return target;
		}
	
		/* Handles responses to an ajax request:
	  * - finds the right dataType (mediates between content-type and expected dataType)
	  * - returns the corresponding response
	  */
		function ajaxHandleResponses(s, jqXHR, responses) {
	
			var ct,
			    type,
			    finalDataType,
			    firstDataType,
			    contents = s.contents,
			    dataTypes = s.dataTypes;
	
			// Remove auto dataType and get content-type in the process
			while (dataTypes[0] === "*") {
				dataTypes.shift();
				if (ct === undefined) {
					ct = s.mimeType || jqXHR.getResponseHeader("Content-Type");
				}
			}
	
			// Check if we're dealing with a known content-type
			if (ct) {
				for (type in contents) {
					if (contents[type] && contents[type].test(ct)) {
						dataTypes.unshift(type);
						break;
					}
				}
			}
	
			// Check to see if we have a response for the expected dataType
			if (dataTypes[0] in responses) {
				finalDataType = dataTypes[0];
			} else {
	
				// Try convertible dataTypes
				for (type in responses) {
					if (!dataTypes[0] || s.converters[type + " " + dataTypes[0]]) {
						finalDataType = type;
						break;
					}
					if (!firstDataType) {
						firstDataType = type;
					}
				}
	
				// Or just use first one
				finalDataType = finalDataType || firstDataType;
			}
	
			// If we found a dataType
			// We add the dataType to the list if needed
			// and return the corresponding response
			if (finalDataType) {
				if (finalDataType !== dataTypes[0]) {
					dataTypes.unshift(finalDataType);
				}
				return responses[finalDataType];
			}
		}
	
		/* Chain conversions given the request and the original response
	  * Also sets the responseXXX fields on the jqXHR instance
	  */
		function ajaxConvert(s, response, jqXHR, isSuccess) {
			var conv2,
			    current,
			    conv,
			    tmp,
			    prev,
			    converters = {},
	
	
			// Work with a copy of dataTypes in case we need to modify it for conversion
			dataTypes = s.dataTypes.slice();
	
			// Create converters map with lowercased keys
			if (dataTypes[1]) {
				for (conv in s.converters) {
					converters[conv.toLowerCase()] = s.converters[conv];
				}
			}
	
			current = dataTypes.shift();
	
			// Convert to each sequential dataType
			while (current) {
	
				if (s.responseFields[current]) {
					jqXHR[s.responseFields[current]] = response;
				}
	
				// Apply the dataFilter if provided
				if (!prev && isSuccess && s.dataFilter) {
					response = s.dataFilter(response, s.dataType);
				}
	
				prev = current;
				current = dataTypes.shift();
	
				if (current) {
	
					// There's only work to do if current dataType is non-auto
					if (current === "*") {
	
						current = prev;
	
						// Convert response if prev dataType is non-auto and differs from current
					} else if (prev !== "*" && prev !== current) {
	
						// Seek a direct converter
						conv = converters[prev + " " + current] || converters["* " + current];
	
						// If none found, seek a pair
						if (!conv) {
							for (conv2 in converters) {
	
								// If conv2 outputs current
								tmp = conv2.split(" ");
								if (tmp[1] === current) {
	
									// If prev can be converted to accepted input
									conv = converters[prev + " " + tmp[0]] || converters["* " + tmp[0]];
									if (conv) {
	
										// Condense equivalence converters
										if (conv === true) {
											conv = converters[conv2];
	
											// Otherwise, insert the intermediate dataType
										} else if (converters[conv2] !== true) {
											current = tmp[0];
											dataTypes.unshift(tmp[1]);
										}
										break;
									}
								}
							}
						}
	
						// Apply converter (if not an equivalence)
						if (conv !== true) {
	
							// Unless errors are allowed to bubble, catch and return them
							if (conv && s.throws) {
								response = conv(response);
							} else {
								try {
									response = conv(response);
								} catch (e) {
									return {
										state: "parsererror",
										error: conv ? e : "No conversion from " + prev + " to " + current
									};
								}
							}
						}
					}
				}
			}
	
			return { state: "success", data: response };
		}
	
		jQuery.extend({
	
			// Counter for holding the number of active queries
			active: 0,
	
			// Last-Modified header cache for next request
			lastModified: {},
			etag: {},
	
			ajaxSettings: {
				url: location.href,
				type: "GET",
				isLocal: rlocalProtocol.test(location.protocol),
				global: true,
				processData: true,
				async: true,
				contentType: "application/x-www-form-urlencoded; charset=UTF-8",
	
				/*
	   timeout: 0,
	   data: null,
	   dataType: null,
	   username: null,
	   password: null,
	   cache: null,
	   throws: false,
	   traditional: false,
	   headers: {},
	   */
	
				accepts: {
					"*": allTypes,
					text: "text/plain",
					html: "text/html",
					xml: "application/xml, text/xml",
					json: "application/json, text/javascript"
				},
	
				contents: {
					xml: /\bxml\b/,
					html: /\bhtml/,
					json: /\bjson\b/
				},
	
				responseFields: {
					xml: "responseXML",
					text: "responseText",
					json: "responseJSON"
				},
	
				// Data converters
				// Keys separate source (or catchall "*") and destination types with a single space
				converters: {
	
					// Convert anything to text
					"* text": String,
	
					// Text to html (true = no transformation)
					"text html": true,
	
					// Evaluate text as a json expression
					"text json": JSON.parse,
	
					// Parse text as xml
					"text xml": jQuery.parseXML
				},
	
				// For options that shouldn't be deep extended:
				// you can add your own custom options here if
				// and when you create one that shouldn't be
				// deep extended (see ajaxExtend)
				flatOptions: {
					url: true,
					context: true
				}
			},
	
			// Creates a full fledged settings object into target
			// with both ajaxSettings and settings fields.
			// If target is omitted, writes into ajaxSettings.
			ajaxSetup: function ajaxSetup(target, settings) {
				return settings ?
	
				// Building a settings object
				ajaxExtend(ajaxExtend(target, jQuery.ajaxSettings), settings) :
	
				// Extending ajaxSettings
				ajaxExtend(jQuery.ajaxSettings, target);
			},
	
			ajaxPrefilter: addToPrefiltersOrTransports(prefilters),
			ajaxTransport: addToPrefiltersOrTransports(transports),
	
			// Main method
			ajax: function ajax(url, options) {
	
				// If url is an object, simulate pre-1.5 signature
				if ((typeof url === "undefined" ? "undefined" : _typeof(url)) === "object") {
					options = url;
					url = undefined;
				}
	
				// Force options to be an object
				options = options || {};
	
				var transport,
	
	
				// URL without anti-cache param
				cacheURL,
	
	
				// Response headers
				responseHeadersString,
				    responseHeaders,
	
	
				// timeout handle
				timeoutTimer,
	
	
				// Url cleanup var
				urlAnchor,
	
	
				// Request state (becomes false upon send and true upon completion)
				completed,
	
	
				// To know if global events are to be dispatched
				fireGlobals,
	
	
				// Loop variable
				i,
	
	
				// uncached part of the url
				uncached,
	
	
				// Create the final options object
				s = jQuery.ajaxSetup({}, options),
	
	
				// Callbacks context
				callbackContext = s.context || s,
	
	
				// Context for global events is callbackContext if it is a DOM node or jQuery collection
				globalEventContext = s.context && (callbackContext.nodeType || callbackContext.jquery) ? jQuery(callbackContext) : jQuery.event,
	
	
				// Deferreds
				deferred = jQuery.Deferred(),
				    completeDeferred = jQuery.Callbacks("once memory"),
	
	
				// Status-dependent callbacks
				_statusCode = s.statusCode || {},
	
	
				// Headers (they are sent all at once)
				requestHeaders = {},
				    requestHeadersNames = {},
	
	
				// Default abort message
				strAbort = "canceled",
	
	
				// Fake xhr
				jqXHR = {
					readyState: 0,
	
					// Builds headers hashtable if needed
					getResponseHeader: function getResponseHeader(key) {
						var match;
						if (completed) {
							if (!responseHeaders) {
								responseHeaders = {};
								while (match = rheaders.exec(responseHeadersString)) {
									responseHeaders[match[1].toLowerCase()] = match[2];
								}
							}
							match = responseHeaders[key.toLowerCase()];
						}
						return match == null ? null : match;
					},
	
					// Raw string
					getAllResponseHeaders: function getAllResponseHeaders() {
						return completed ? responseHeadersString : null;
					},
	
					// Caches the header
					setRequestHeader: function setRequestHeader(name, value) {
						if (completed == null) {
							name = requestHeadersNames[name.toLowerCase()] = requestHeadersNames[name.toLowerCase()] || name;
							requestHeaders[name] = value;
						}
						return this;
					},
	
					// Overrides response content-type header
					overrideMimeType: function overrideMimeType(type) {
						if (completed == null) {
							s.mimeType = type;
						}
						return this;
					},
	
					// Status-dependent callbacks
					statusCode: function statusCode(map) {
						var code;
						if (map) {
							if (completed) {
	
								// Execute the appropriate callbacks
								jqXHR.always(map[jqXHR.status]);
							} else {
	
								// Lazy-add the new callbacks in a way that preserves old ones
								for (code in map) {
									_statusCode[code] = [_statusCode[code], map[code]];
								}
							}
						}
						return this;
					},
	
					// Cancel the request
					abort: function abort(statusText) {
						var finalText = statusText || strAbort;
						if (transport) {
							transport.abort(finalText);
						}
						done(0, finalText);
						return this;
					}
				};
	
				// Attach deferreds
				deferred.promise(jqXHR);
	
				// Add protocol if not provided (prefilters might expect it)
				// Handle falsy url in the settings object (#10093: consistency with old signature)
				// We also use the url parameter if available
				s.url = ((url || s.url || location.href) + "").replace(rprotocol, location.protocol + "//");
	
				// Alias method option to type as per ticket #12004
				s.type = options.method || options.type || s.method || s.type;
	
				// Extract dataTypes list
				s.dataTypes = (s.dataType || "*").toLowerCase().match(rnothtmlwhite) || [""];
	
				// A cross-domain request is in order when the origin doesn't match the current origin.
				if (s.crossDomain == null) {
					urlAnchor = document.createElement("a");
	
					// Support: IE <=8 - 11, Edge 12 - 13
					// IE throws exception on accessing the href property if url is malformed,
					// e.g. http://example.com:80x/
					try {
						urlAnchor.href = s.url;
	
						// Support: IE <=8 - 11 only
						// Anchor's host property isn't correctly set when s.url is relative
						urlAnchor.href = urlAnchor.href;
						s.crossDomain = originAnchor.protocol + "//" + originAnchor.host !== urlAnchor.protocol + "//" + urlAnchor.host;
					} catch (e) {
	
						// If there is an error parsing the URL, assume it is crossDomain,
						// it can be rejected by the transport if it is invalid
						s.crossDomain = true;
					}
				}
	
				// Convert data if not already a string
				if (s.data && s.processData && typeof s.data !== "string") {
					s.data = jQuery.param(s.data, s.traditional);
				}
	
				// Apply prefilters
				inspectPrefiltersOrTransports(prefilters, s, options, jqXHR);
	
				// If request was aborted inside a prefilter, stop there
				if (completed) {
					return jqXHR;
				}
	
				// We can fire global events as of now if asked to
				// Don't fire events if jQuery.event is undefined in an AMD-usage scenario (#15118)
				fireGlobals = jQuery.event && s.global;
	
				// Watch for a new set of requests
				if (fireGlobals && jQuery.active++ === 0) {
					jQuery.event.trigger("ajaxStart");
				}
	
				// Uppercase the type
				s.type = s.type.toUpperCase();
	
				// Determine if request has content
				s.hasContent = !rnoContent.test(s.type);
	
				// Save the URL in case we're toying with the If-Modified-Since
				// and/or If-None-Match header later on
				// Remove hash to simplify url manipulation
				cacheURL = s.url.replace(rhash, "");
	
				// More options handling for requests with no content
				if (!s.hasContent) {
	
					// Remember the hash so we can put it back
					uncached = s.url.slice(cacheURL.length);
	
					// If data is available, append data to url
					if (s.data) {
						cacheURL += (rquery.test(cacheURL) ? "&" : "?") + s.data;
	
						// #9682: remove data so that it's not used in an eventual retry
						delete s.data;
					}
	
					// Add or update anti-cache param if needed
					if (s.cache === false) {
						cacheURL = cacheURL.replace(rantiCache, "$1");
						uncached = (rquery.test(cacheURL) ? "&" : "?") + "_=" + nonce++ + uncached;
					}
	
					// Put hash and anti-cache on the URL that will be requested (gh-1732)
					s.url = cacheURL + uncached;
	
					// Change '%20' to '+' if this is encoded form body content (gh-2658)
				} else if (s.data && s.processData && (s.contentType || "").indexOf("application/x-www-form-urlencoded") === 0) {
					s.data = s.data.replace(r20, "+");
				}
	
				// Set the If-Modified-Since and/or If-None-Match header, if in ifModified mode.
				if (s.ifModified) {
					if (jQuery.lastModified[cacheURL]) {
						jqXHR.setRequestHeader("If-Modified-Since", jQuery.lastModified[cacheURL]);
					}
					if (jQuery.etag[cacheURL]) {
						jqXHR.setRequestHeader("If-None-Match", jQuery.etag[cacheURL]);
					}
				}
	
				// Set the correct header, if data is being sent
				if (s.data && s.hasContent && s.contentType !== false || options.contentType) {
					jqXHR.setRequestHeader("Content-Type", s.contentType);
				}
	
				// Set the Accepts header for the server, depending on the dataType
				jqXHR.setRequestHeader("Accept", s.dataTypes[0] && s.accepts[s.dataTypes[0]] ? s.accepts[s.dataTypes[0]] + (s.dataTypes[0] !== "*" ? ", " + allTypes + "; q=0.01" : "") : s.accepts["*"]);
	
				// Check for headers option
				for (i in s.headers) {
					jqXHR.setRequestHeader(i, s.headers[i]);
				}
	
				// Allow custom headers/mimetypes and early abort
				if (s.beforeSend && (s.beforeSend.call(callbackContext, jqXHR, s) === false || completed)) {
	
					// Abort if not done already and return
					return jqXHR.abort();
				}
	
				// Aborting is no longer a cancellation
				strAbort = "abort";
	
				// Install callbacks on deferreds
				completeDeferred.add(s.complete);
				jqXHR.done(s.success);
				jqXHR.fail(s.error);
	
				// Get transport
				transport = inspectPrefiltersOrTransports(transports, s, options, jqXHR);
	
				// If no transport, we auto-abort
				if (!transport) {
					done(-1, "No Transport");
				} else {
					jqXHR.readyState = 1;
	
					// Send global event
					if (fireGlobals) {
						globalEventContext.trigger("ajaxSend", [jqXHR, s]);
					}
	
					// If request was aborted inside ajaxSend, stop there
					if (completed) {
						return jqXHR;
					}
	
					// Timeout
					if (s.async && s.timeout > 0) {
						timeoutTimer = window.setTimeout(function () {
							jqXHR.abort("timeout");
						}, s.timeout);
					}
	
					try {
						completed = false;
						transport.send(requestHeaders, done);
					} catch (e) {
	
						// Rethrow post-completion exceptions
						if (completed) {
							throw e;
						}
	
						// Propagate others as results
						done(-1, e);
					}
				}
	
				// Callback for when everything is done
				function done(status, nativeStatusText, responses, headers) {
					var isSuccess,
					    success,
					    error,
					    response,
					    modified,
					    statusText = nativeStatusText;
	
					// Ignore repeat invocations
					if (completed) {
						return;
					}
	
					completed = true;
	
					// Clear timeout if it exists
					if (timeoutTimer) {
						window.clearTimeout(timeoutTimer);
					}
	
					// Dereference transport for early garbage collection
					// (no matter how long the jqXHR object will be used)
					transport = undefined;
	
					// Cache response headers
					responseHeadersString = headers || "";
	
					// Set readyState
					jqXHR.readyState = status > 0 ? 4 : 0;
	
					// Determine if successful
					isSuccess = status >= 200 && status < 300 || status === 304;
	
					// Get response data
					if (responses) {
						response = ajaxHandleResponses(s, jqXHR, responses);
					}
	
					// Convert no matter what (that way responseXXX fields are always set)
					response = ajaxConvert(s, response, jqXHR, isSuccess);
	
					// If successful, handle type chaining
					if (isSuccess) {
	
						// Set the If-Modified-Since and/or If-None-Match header, if in ifModified mode.
						if (s.ifModified) {
							modified = jqXHR.getResponseHeader("Last-Modified");
							if (modified) {
								jQuery.lastModified[cacheURL] = modified;
							}
							modified = jqXHR.getResponseHeader("etag");
							if (modified) {
								jQuery.etag[cacheURL] = modified;
							}
						}
	
						// if no content
						if (status === 204 || s.type === "HEAD") {
							statusText = "nocontent";
	
							// if not modified
						} else if (status === 304) {
							statusText = "notmodified";
	
							// If we have data, let's convert it
						} else {
							statusText = response.state;
							success = response.data;
							error = response.error;
							isSuccess = !error;
						}
					} else {
	
						// Extract error from statusText and normalize for non-aborts
						error = statusText;
						if (status || !statusText) {
							statusText = "error";
							if (status < 0) {
								status = 0;
							}
						}
					}
	
					// Set data for the fake xhr object
					jqXHR.status = status;
					jqXHR.statusText = (nativeStatusText || statusText) + "";
	
					// Success/Error
					if (isSuccess) {
						deferred.resolveWith(callbackContext, [success, statusText, jqXHR]);
					} else {
						deferred.rejectWith(callbackContext, [jqXHR, statusText, error]);
					}
	
					// Status-dependent callbacks
					jqXHR.statusCode(_statusCode);
					_statusCode = undefined;
	
					if (fireGlobals) {
						globalEventContext.trigger(isSuccess ? "ajaxSuccess" : "ajaxError", [jqXHR, s, isSuccess ? success : error]);
					}
	
					// Complete
					completeDeferred.fireWith(callbackContext, [jqXHR, statusText]);
	
					if (fireGlobals) {
						globalEventContext.trigger("ajaxComplete", [jqXHR, s]);
	
						// Handle the global AJAX counter
						if (! --jQuery.active) {
							jQuery.event.trigger("ajaxStop");
						}
					}
				}
	
				return jqXHR;
			},
	
			getJSON: function getJSON(url, data, callback) {
				return jQuery.get(url, data, callback, "json");
			},
	
			getScript: function getScript(url, callback) {
				return jQuery.get(url, undefined, callback, "script");
			}
		});
	
		jQuery.each(["get", "post"], function (i, method) {
			jQuery[method] = function (url, data, callback, type) {
	
				// Shift arguments if data argument was omitted
				if (jQuery.isFunction(data)) {
					type = type || callback;
					callback = data;
					data = undefined;
				}
	
				// The url can be an options object (which then must have .url)
				return jQuery.ajax(jQuery.extend({
					url: url,
					type: method,
					dataType: type,
					data: data,
					success: callback
				}, jQuery.isPlainObject(url) && url));
			};
		});
	
		jQuery._evalUrl = function (url) {
			return jQuery.ajax({
				url: url,
	
				// Make this explicit, since user can override this through ajaxSetup (#11264)
				type: "GET",
				dataType: "script",
				cache: true,
				async: false,
				global: false,
				"throws": true
			});
		};
	
		jQuery.fn.extend({
			wrapAll: function wrapAll(html) {
				var wrap;
	
				if (this[0]) {
					if (jQuery.isFunction(html)) {
						html = html.call(this[0]);
					}
	
					// The elements to wrap the target around
					wrap = jQuery(html, this[0].ownerDocument).eq(0).clone(true);
	
					if (this[0].parentNode) {
						wrap.insertBefore(this[0]);
					}
	
					wrap.map(function () {
						var elem = this;
	
						while (elem.firstElementChild) {
							elem = elem.firstElementChild;
						}
	
						return elem;
					}).append(this);
				}
	
				return this;
			},
	
			wrapInner: function wrapInner(html) {
				if (jQuery.isFunction(html)) {
					return this.each(function (i) {
						jQuery(this).wrapInner(html.call(this, i));
					});
				}
	
				return this.each(function () {
					var self = jQuery(this),
					    contents = self.contents();
	
					if (contents.length) {
						contents.wrapAll(html);
					} else {
						self.append(html);
					}
				});
			},
	
			wrap: function wrap(html) {
				var isFunction = jQuery.isFunction(html);
	
				return this.each(function (i) {
					jQuery(this).wrapAll(isFunction ? html.call(this, i) : html);
				});
			},
	
			unwrap: function unwrap(selector) {
				this.parent(selector).not("body").each(function () {
					jQuery(this).replaceWith(this.childNodes);
				});
				return this;
			}
		});
	
		jQuery.expr.pseudos.hidden = function (elem) {
			return !jQuery.expr.pseudos.visible(elem);
		};
		jQuery.expr.pseudos.visible = function (elem) {
			return !!(elem.offsetWidth || elem.offsetHeight || elem.getClientRects().length);
		};
	
		jQuery.ajaxSettings.xhr = function () {
			try {
				return new window.XMLHttpRequest();
			} catch (e) {}
		};
	
		var xhrSuccessStatus = {
	
			// File protocol always yields status code 0, assume 200
			0: 200,
	
			// Support: IE <=9 only
			// #1450: sometimes IE returns 1223 when it should be 204
			1223: 204
		},
		    xhrSupported = jQuery.ajaxSettings.xhr();
	
		support.cors = !!xhrSupported && "withCredentials" in xhrSupported;
		support.ajax = xhrSupported = !!xhrSupported;
	
		jQuery.ajaxTransport(function (options) {
			var _callback, errorCallback;
	
			// Cross domain only allowed if supported through XMLHttpRequest
			if (support.cors || xhrSupported && !options.crossDomain) {
				return {
					send: function send(headers, complete) {
						var i,
						    xhr = options.xhr();
	
						xhr.open(options.type, options.url, options.async, options.username, options.password);
	
						// Apply custom fields if provided
						if (options.xhrFields) {
							for (i in options.xhrFields) {
								xhr[i] = options.xhrFields[i];
							}
						}
	
						// Override mime type if needed
						if (options.mimeType && xhr.overrideMimeType) {
							xhr.overrideMimeType(options.mimeType);
						}
	
						// X-Requested-With header
						// For cross-domain requests, seeing as conditions for a preflight are
						// akin to a jigsaw puzzle, we simply never set it to be sure.
						// (it can always be set on a per-request basis or even using ajaxSetup)
						// For same-domain requests, won't change header if already provided.
						if (!options.crossDomain && !headers["X-Requested-With"]) {
							headers["X-Requested-With"] = "XMLHttpRequest";
						}
	
						// Set headers
						for (i in headers) {
							xhr.setRequestHeader(i, headers[i]);
						}
	
						// Callback
						_callback = function callback(type) {
							return function () {
								if (_callback) {
									_callback = errorCallback = xhr.onload = xhr.onerror = xhr.onabort = xhr.onreadystatechange = null;
	
									if (type === "abort") {
										xhr.abort();
									} else if (type === "error") {
	
										// Support: IE <=9 only
										// On a manual native abort, IE9 throws
										// errors on any property access that is not readyState
										if (typeof xhr.status !== "number") {
											complete(0, "error");
										} else {
											complete(
	
											// File: protocol always yields status 0; see #8605, #14207
											xhr.status, xhr.statusText);
										}
									} else {
										complete(xhrSuccessStatus[xhr.status] || xhr.status, xhr.statusText,
	
										// Support: IE <=9 only
										// IE9 has no XHR2 but throws on binary (trac-11426)
										// For XHR2 non-text, let the caller handle it (gh-2498)
										(xhr.responseType || "text") !== "text" || typeof xhr.responseText !== "string" ? { binary: xhr.response } : { text: xhr.responseText }, xhr.getAllResponseHeaders());
									}
								}
							};
						};
	
						// Listen to events
						xhr.onload = _callback();
						errorCallback = xhr.onerror = _callback("error");
	
						// Support: IE 9 only
						// Use onreadystatechange to replace onabort
						// to handle uncaught aborts
						if (xhr.onabort !== undefined) {
							xhr.onabort = errorCallback;
						} else {
							xhr.onreadystatechange = function () {
	
								// Check readyState before timeout as it changes
								if (xhr.readyState === 4) {
	
									// Allow onerror to be called first,
									// but that will not handle a native abort
									// Also, save errorCallback to a variable
									// as xhr.onerror cannot be accessed
									window.setTimeout(function () {
										if (_callback) {
											errorCallback();
										}
									});
								}
							};
						}
	
						// Create the abort callback
						_callback = _callback("abort");
	
						try {
	
							// Do send the request (this may raise an exception)
							xhr.send(options.hasContent && options.data || null);
						} catch (e) {
	
							// #14683: Only rethrow if this hasn't been notified as an error yet
							if (_callback) {
								throw e;
							}
						}
					},
	
					abort: function abort() {
						if (_callback) {
							_callback();
						}
					}
				};
			}
		});
	
		// Prevent auto-execution of scripts when no explicit dataType was provided (See gh-2432)
		jQuery.ajaxPrefilter(function (s) {
			if (s.crossDomain) {
				s.contents.script = false;
			}
		});
	
		// Install script dataType
		jQuery.ajaxSetup({
			accepts: {
				script: "text/javascript, application/javascript, " + "application/ecmascript, application/x-ecmascript"
			},
			contents: {
				script: /\b(?:java|ecma)script\b/
			},
			converters: {
				"text script": function textScript(text) {
					jQuery.globalEval(text);
					return text;
				}
			}
		});
	
		// Handle cache's special case and crossDomain
		jQuery.ajaxPrefilter("script", function (s) {
			if (s.cache === undefined) {
				s.cache = false;
			}
			if (s.crossDomain) {
				s.type = "GET";
			}
		});
	
		// Bind script tag hack transport
		jQuery.ajaxTransport("script", function (s) {
	
			// This transport only deals with cross domain requests
			if (s.crossDomain) {
				var script, _callback2;
				return {
					send: function send(_, complete) {
						script = jQuery("<script>").prop({
							charset: s.scriptCharset,
							src: s.url
						}).on("load error", _callback2 = function callback(evt) {
							script.remove();
							_callback2 = null;
							if (evt) {
								complete(evt.type === "error" ? 404 : 200, evt.type);
							}
						});
	
						// Use native DOM manipulation to avoid our domManip AJAX trickery
						document.head.appendChild(script[0]);
					},
					abort: function abort() {
						if (_callback2) {
							_callback2();
						}
					}
				};
			}
		});
	
		var oldCallbacks = [],
		    rjsonp = /(=)\?(?=&|$)|\?\?/;
	
		// Default jsonp settings
		jQuery.ajaxSetup({
			jsonp: "callback",
			jsonpCallback: function jsonpCallback() {
				var callback = oldCallbacks.pop() || jQuery.expando + "_" + nonce++;
				this[callback] = true;
				return callback;
			}
		});
	
		// Detect, normalize options and install callbacks for jsonp requests
		jQuery.ajaxPrefilter("json jsonp", function (s, originalSettings, jqXHR) {
	
			var callbackName,
			    overwritten,
			    responseContainer,
			    jsonProp = s.jsonp !== false && (rjsonp.test(s.url) ? "url" : typeof s.data === "string" && (s.contentType || "").indexOf("application/x-www-form-urlencoded") === 0 && rjsonp.test(s.data) && "data");
	
			// Handle iff the expected data type is "jsonp" or we have a parameter to set
			if (jsonProp || s.dataTypes[0] === "jsonp") {
	
				// Get callback name, remembering preexisting value associated with it
				callbackName = s.jsonpCallback = jQuery.isFunction(s.jsonpCallback) ? s.jsonpCallback() : s.jsonpCallback;
	
				// Insert callback into url or form data
				if (jsonProp) {
					s[jsonProp] = s[jsonProp].replace(rjsonp, "$1" + callbackName);
				} else if (s.jsonp !== false) {
					s.url += (rquery.test(s.url) ? "&" : "?") + s.jsonp + "=" + callbackName;
				}
	
				// Use data converter to retrieve json after script execution
				s.converters["script json"] = function () {
					if (!responseContainer) {
						jQuery.error(callbackName + " was not called");
					}
					return responseContainer[0];
				};
	
				// Force json dataType
				s.dataTypes[0] = "json";
	
				// Install callback
				overwritten = window[callbackName];
				window[callbackName] = function () {
					responseContainer = arguments;
				};
	
				// Clean-up function (fires after converters)
				jqXHR.always(function () {
	
					// If previous value didn't exist - remove it
					if (overwritten === undefined) {
						jQuery(window).removeProp(callbackName);
	
						// Otherwise restore preexisting value
					} else {
						window[callbackName] = overwritten;
					}
	
					// Save back as free
					if (s[callbackName]) {
	
						// Make sure that re-using the options doesn't screw things around
						s.jsonpCallback = originalSettings.jsonpCallback;
	
						// Save the callback name for future use
						oldCallbacks.push(callbackName);
					}
	
					// Call if it was a function and we have a response
					if (responseContainer && jQuery.isFunction(overwritten)) {
						overwritten(responseContainer[0]);
					}
	
					responseContainer = overwritten = undefined;
				});
	
				// Delegate to script
				return "script";
			}
		});
	
		// Support: Safari 8 only
		// In Safari 8 documents created via document.implementation.createHTMLDocument
		// collapse sibling forms: the second one becomes a child of the first one.
		// Because of that, this security measure has to be disabled in Safari 8.
		// https://bugs.webkit.org/show_bug.cgi?id=137337
		support.createHTMLDocument = function () {
			var body = document.implementation.createHTMLDocument("").body;
			body.innerHTML = "<form></form><form></form>";
			return body.childNodes.length === 2;
		}();
	
		// Argument "data" should be string of html
		// context (optional): If specified, the fragment will be created in this context,
		// defaults to document
		// keepScripts (optional): If true, will include scripts passed in the html string
		jQuery.parseHTML = function (data, context, keepScripts) {
			if (typeof data !== "string") {
				return [];
			}
			if (typeof context === "boolean") {
				keepScripts = context;
				context = false;
			}
	
			var base, parsed, scripts;
	
			if (!context) {
	
				// Stop scripts or inline event handlers from being executed immediately
				// by using document.implementation
				if (support.createHTMLDocument) {
					context = document.implementation.createHTMLDocument("");
	
					// Set the base href for the created document
					// so any parsed elements with URLs
					// are based on the document's URL (gh-2965)
					base = context.createElement("base");
					base.href = document.location.href;
					context.head.appendChild(base);
				} else {
					context = document;
				}
			}
	
			parsed = rsingleTag.exec(data);
			scripts = !keepScripts && [];
	
			// Single tag
			if (parsed) {
				return [context.createElement(parsed[1])];
			}
	
			parsed = buildFragment([data], context, scripts);
	
			if (scripts && scripts.length) {
				jQuery(scripts).remove();
			}
	
			return jQuery.merge([], parsed.childNodes);
		};
	
		/**
	  * Load a url into a page
	  */
		jQuery.fn.load = function (url, params, callback) {
			var selector,
			    type,
			    response,
			    self = this,
			    off = url.indexOf(" ");
	
			if (off > -1) {
				selector = stripAndCollapse(url.slice(off));
				url = url.slice(0, off);
			}
	
			// If it's a function
			if (jQuery.isFunction(params)) {
	
				// We assume that it's the callback
				callback = params;
				params = undefined;
	
				// Otherwise, build a param string
			} else if (params && (typeof params === "undefined" ? "undefined" : _typeof(params)) === "object") {
				type = "POST";
			}
	
			// If we have elements to modify, make the request
			if (self.length > 0) {
				jQuery.ajax({
					url: url,
	
					// If "type" variable is undefined, then "GET" method will be used.
					// Make value of this field explicit since
					// user can override it through ajaxSetup method
					type: type || "GET",
					dataType: "html",
					data: params
				}).done(function (responseText) {
	
					// Save response for use in complete callback
					response = arguments;
	
					self.html(selector ?
	
					// If a selector was specified, locate the right elements in a dummy div
					// Exclude scripts to avoid IE 'Permission Denied' errors
					jQuery("<div>").append(jQuery.parseHTML(responseText)).find(selector) :
	
					// Otherwise use the full result
					responseText);
	
					// If the request succeeds, this function gets "data", "status", "jqXHR"
					// but they are ignored because response was set above.
					// If it fails, this function gets "jqXHR", "status", "error"
				}).always(callback && function (jqXHR, status) {
					self.each(function () {
						callback.apply(this, response || [jqXHR.responseText, status, jqXHR]);
					});
				});
			}
	
			return this;
		};
	
		// Attach a bunch of functions for handling common AJAX events
		jQuery.each(["ajaxStart", "ajaxStop", "ajaxComplete", "ajaxError", "ajaxSuccess", "ajaxSend"], function (i, type) {
			jQuery.fn[type] = function (fn) {
				return this.on(type, fn);
			};
		});
	
		jQuery.expr.pseudos.animated = function (elem) {
			return jQuery.grep(jQuery.timers, function (fn) {
				return elem === fn.elem;
			}).length;
		};
	
		/**
	  * Gets a window from an element
	  */
		function getWindow(elem) {
			return jQuery.isWindow(elem) ? elem : elem.nodeType === 9 && elem.defaultView;
		}
	
		jQuery.offset = {
			setOffset: function setOffset(elem, options, i) {
				var curPosition,
				    curLeft,
				    curCSSTop,
				    curTop,
				    curOffset,
				    curCSSLeft,
				    calculatePosition,
				    position = jQuery.css(elem, "position"),
				    curElem = jQuery(elem),
				    props = {};
	
				// Set position first, in-case top/left are set even on static elem
				if (position === "static") {
					elem.style.position = "relative";
				}
	
				curOffset = curElem.offset();
				curCSSTop = jQuery.css(elem, "top");
				curCSSLeft = jQuery.css(elem, "left");
				calculatePosition = (position === "absolute" || position === "fixed") && (curCSSTop + curCSSLeft).indexOf("auto") > -1;
	
				// Need to be able to calculate position if either
				// top or left is auto and position is either absolute or fixed
				if (calculatePosition) {
					curPosition = curElem.position();
					curTop = curPosition.top;
					curLeft = curPosition.left;
				} else {
					curTop = parseFloat(curCSSTop) || 0;
					curLeft = parseFloat(curCSSLeft) || 0;
				}
	
				if (jQuery.isFunction(options)) {
	
					// Use jQuery.extend here to allow modification of coordinates argument (gh-1848)
					options = options.call(elem, i, jQuery.extend({}, curOffset));
				}
	
				if (options.top != null) {
					props.top = options.top - curOffset.top + curTop;
				}
				if (options.left != null) {
					props.left = options.left - curOffset.left + curLeft;
				}
	
				if ("using" in options) {
					options.using.call(elem, props);
				} else {
					curElem.css(props);
				}
			}
		};
	
		jQuery.fn.extend({
			offset: function offset(options) {
	
				// Preserve chaining for setter
				if (arguments.length) {
					return options === undefined ? this : this.each(function (i) {
						jQuery.offset.setOffset(this, options, i);
					});
				}
	
				var docElem,
				    win,
				    rect,
				    doc,
				    elem = this[0];
	
				if (!elem) {
					return;
				}
	
				// Support: IE <=11 only
				// Running getBoundingClientRect on a
				// disconnected node in IE throws an error
				if (!elem.getClientRects().length) {
					return { top: 0, left: 0 };
				}
	
				rect = elem.getBoundingClientRect();
	
				// Make sure element is not hidden (display: none)
				if (rect.width || rect.height) {
					doc = elem.ownerDocument;
					win = getWindow(doc);
					docElem = doc.documentElement;
	
					return {
						top: rect.top + win.pageYOffset - docElem.clientTop,
						left: rect.left + win.pageXOffset - docElem.clientLeft
					};
				}
	
				// Return zeros for disconnected and hidden elements (gh-2310)
				return rect;
			},
	
			position: function position() {
				if (!this[0]) {
					return;
				}
	
				var offsetParent,
				    offset,
				    elem = this[0],
				    parentOffset = { top: 0, left: 0 };
	
				// Fixed elements are offset from window (parentOffset = {top:0, left: 0},
				// because it is its only offset parent
				if (jQuery.css(elem, "position") === "fixed") {
	
					// Assume getBoundingClientRect is there when computed position is fixed
					offset = elem.getBoundingClientRect();
				} else {
	
					// Get *real* offsetParent
					offsetParent = this.offsetParent();
	
					// Get correct offsets
					offset = this.offset();
					if (!jQuery.nodeName(offsetParent[0], "html")) {
						parentOffset = offsetParent.offset();
					}
	
					// Add offsetParent borders
					parentOffset = {
						top: parentOffset.top + jQuery.css(offsetParent[0], "borderTopWidth", true),
						left: parentOffset.left + jQuery.css(offsetParent[0], "borderLeftWidth", true)
					};
				}
	
				// Subtract parent offsets and element margins
				return {
					top: offset.top - parentOffset.top - jQuery.css(elem, "marginTop", true),
					left: offset.left - parentOffset.left - jQuery.css(elem, "marginLeft", true)
				};
			},
	
			// This method will return documentElement in the following cases:
			// 1) For the element inside the iframe without offsetParent, this method will return
			//    documentElement of the parent window
			// 2) For the hidden or detached element
			// 3) For body or html element, i.e. in case of the html node - it will return itself
			//
			// but those exceptions were never presented as a real life use-cases
			// and might be considered as more preferable results.
			//
			// This logic, however, is not guaranteed and can change at any point in the future
			offsetParent: function offsetParent() {
				return this.map(function () {
					var offsetParent = this.offsetParent;
	
					while (offsetParent && jQuery.css(offsetParent, "position") === "static") {
						offsetParent = offsetParent.offsetParent;
					}
	
					return offsetParent || documentElement;
				});
			}
		});
	
		// Create scrollLeft and scrollTop methods
		jQuery.each({ scrollLeft: "pageXOffset", scrollTop: "pageYOffset" }, function (method, prop) {
			var top = "pageYOffset" === prop;
	
			jQuery.fn[method] = function (val) {
				return access(this, function (elem, method, val) {
					var win = getWindow(elem);
	
					if (val === undefined) {
						return win ? win[prop] : elem[method];
					}
	
					if (win) {
						win.scrollTo(!top ? val : win.pageXOffset, top ? val : win.pageYOffset);
					} else {
						elem[method] = val;
					}
				}, method, val, arguments.length);
			};
		});
	
		// Support: Safari <=7 - 9.1, Chrome <=37 - 49
		// Add the top/left cssHooks using jQuery.fn.position
		// Webkit bug: https://bugs.webkit.org/show_bug.cgi?id=29084
		// Blink bug: https://bugs.chromium.org/p/chromium/issues/detail?id=589347
		// getComputedStyle returns percent when specified for top/left/bottom/right;
		// rather than make the css module depend on the offset module, just check for it here
		jQuery.each(["top", "left"], function (i, prop) {
			jQuery.cssHooks[prop] = addGetHookIf(support.pixelPosition, function (elem, computed) {
				if (computed) {
					computed = curCSS(elem, prop);
	
					// If curCSS returns percentage, fallback to offset
					return rnumnonpx.test(computed) ? jQuery(elem).position()[prop] + "px" : computed;
				}
			});
		});
	
		// Create innerHeight, innerWidth, height, width, outerHeight and outerWidth methods
		jQuery.each({ Height: "height", Width: "width" }, function (name, type) {
			jQuery.each({ padding: "inner" + name, content: type, "": "outer" + name }, function (defaultExtra, funcName) {
	
				// Margin is only for outerHeight, outerWidth
				jQuery.fn[funcName] = function (margin, value) {
					var chainable = arguments.length && (defaultExtra || typeof margin !== "boolean"),
					    extra = defaultExtra || (margin === true || value === true ? "margin" : "border");
	
					return access(this, function (elem, type, value) {
						var doc;
	
						if (jQuery.isWindow(elem)) {
	
							// $( window ).outerWidth/Height return w/h including scrollbars (gh-1729)
							return funcName.indexOf("outer") === 0 ? elem["inner" + name] : elem.document.documentElement["client" + name];
						}
	
						// Get document width or height
						if (elem.nodeType === 9) {
							doc = elem.documentElement;
	
							// Either scroll[Width/Height] or offset[Width/Height] or client[Width/Height],
							// whichever is greatest
							return Math.max(elem.body["scroll" + name], doc["scroll" + name], elem.body["offset" + name], doc["offset" + name], doc["client" + name]);
						}
	
						return value === undefined ?
	
						// Get width or height on the element, requesting but not forcing parseFloat
						jQuery.css(elem, type, extra) :
	
						// Set width or height on the element
						jQuery.style(elem, type, value, extra);
					}, type, chainable ? margin : undefined, chainable);
				};
			});
		});
	
		jQuery.fn.extend({
	
			bind: function bind(types, data, fn) {
				return this.on(types, null, data, fn);
			},
			unbind: function unbind(types, fn) {
				return this.off(types, null, fn);
			},
	
			delegate: function delegate(selector, types, data, fn) {
				return this.on(types, selector, data, fn);
			},
			undelegate: function undelegate(selector, types, fn) {
	
				// ( namespace ) or ( selector, types [, fn] )
				return arguments.length === 1 ? this.off(selector, "**") : this.off(types, selector || "**", fn);
			}
		});
	
		jQuery.parseJSON = JSON.parse;
	
		// Register as a named AMD module, since jQuery can be concatenated with other
		// files that may use define, but not via a proper concatenation script that
		// understands anonymous AMD modules. A named AMD is safest and most robust
		// way to register. Lowercase jquery is used because AMD module names are
		// derived from file names, and jQuery is normally delivered in a lowercase
		// file name. Do this after creating the global so that if an AMD module wants
		// to call noConflict to hide this version of jQuery, it will work.
	
		// Note that for maximum portability, libraries that are not jQuery should
		// declare themselves as anonymous modules, and avoid setting a global if an
		// AMD loader is present. jQuery is a special case. For more information, see
		// https://github.com/jrburke/requirejs/wiki/Updating-existing-libraries#wiki-anon
	
		if (true) {
			!(__WEBPACK_AMD_DEFINE_ARRAY__ = [], __WEBPACK_AMD_DEFINE_RESULT__ = function () {
				return jQuery;
			}.apply(exports, __WEBPACK_AMD_DEFINE_ARRAY__), __WEBPACK_AMD_DEFINE_RESULT__ !== undefined && (module.exports = __WEBPACK_AMD_DEFINE_RESULT__));
		}
	
		var
	
		// Map over jQuery in case of overwrite
		_jQuery = window.jQuery,
	
	
		// Map over the $ in case of overwrite
		_$ = window.$;
	
		jQuery.noConflict = function (deep) {
			if (window.$ === jQuery) {
				window.$ = _$;
			}
	
			if (deep && window.jQuery === jQuery) {
				window.jQuery = _jQuery;
			}
	
			return jQuery;
		};
	
		// Expose jQuery and $ identifiers, even in AMD
		// (#7102#comment:10, https://github.com/jquery/jquery/pull/557)
		// and CommonJS for browser emulators (#13566)
		if (!noGlobal) {
			window.jQuery = window.$ = jQuery;
		}
	
		return jQuery;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 3 */
/***/ function(module, exports) {

	module.exports = function(module) {
		if(!module.webpackPolyfill) {
			module.deprecate = function() {};
			module.paths = [];
			// module.parent = undefined by default
			module.children = [];
			module.webpackPolyfill = 1;
		}
		return module;
	}


/***/ },
/* 4 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Util", "./Util", "./RegExp", "./Date", "./Date", "./Date", "./Date", "./Date", "./Date"], function (require, exports) {
	    "use strict";
	
	    var Util_1 = require("./Util");
	    var Util_2 = require("./Util");
	    var RegExp_1 = require("./RegExp");
	    var Date_1 = require("./Date");
	    var Date_2 = require("./Date");
	    var Date_3 = require("./Date");
	    var Date_4 = require("./Date");
	    var Date_5 = require("./Date");
	    var Date_6 = require("./Date");
	    var fsFormatRegExp = /(^|[^%])%([0+ ]*)(-?\d+)?(?:\.(\d+))?(\w)/;
	    var formatRegExp = /\{(\d+)(,-?\d+)?(?:\:(.+?))?\}/g;
	    var StringComparison = {
	        CurrentCulture: 0,
	        CurrentCultureIgnoreCase: 1,
	        InvariantCulture: 2,
	        InvariantCultureIgnoreCase: 3,
	        Ordinal: 4,
	        OrdinalIgnoreCase: 5
	    };
	    function cmp(x, y, ic) {
	        function isIgnoreCase(i) {
	            return i === true || i === StringComparison.CurrentCultureIgnoreCase || i === StringComparison.InvariantCultureIgnoreCase || i === StringComparison.OrdinalIgnoreCase;
	        }
	        function isOrdinal(i) {
	            return i === StringComparison.Ordinal || i === StringComparison.OrdinalIgnoreCase;
	        }
	        if (x == null) return y == null ? 0 : -1;
	        if (y == null) return 1;
	        if (isOrdinal(ic)) {
	            if (isIgnoreCase(ic)) {
	                x = x.toLowerCase();
	                y = y.toLowerCase();
	            }
	            return x === y ? 0 : x < y ? -1 : 1;
	        } else {
	            if (isIgnoreCase(ic)) {
	                x = x.toLocaleLowerCase();
	                y = y.toLocaleLowerCase();
	            }
	            return x.localeCompare(y);
	        }
	    }
	    function compare() {
	        var args = [];
	        for (var _i = 0; _i < arguments.length; _i++) {
	            args[_i] = arguments[_i];
	        }
	        switch (args.length) {
	            case 2:
	                return cmp(args[0], args[1], false);
	            case 3:
	                return cmp(args[0], args[1], args[2]);
	            case 4:
	                return cmp(args[0], args[1], args[2] === true);
	            case 5:
	                return cmp(args[0].substr(args[1], args[4]), args[2].substr(args[3], args[4]), false);
	            case 6:
	                return cmp(args[0].substr(args[1], args[4]), args[2].substr(args[3], args[4]), args[5]);
	            case 7:
	                return cmp(args[0].substr(args[1], args[4]), args[2].substr(args[3], args[4]), args[5] === true);
	            default:
	                throw new Error("String.compare: Unsupported number of parameters");
	        }
	    }
	    exports.compare = compare;
	    function compareTo(x, y) {
	        return cmp(x, y, false);
	    }
	    exports.compareTo = compareTo;
	    function indexOfAny(str, anyOf) {
	        var args = [];
	        for (var _i = 2; _i < arguments.length; _i++) {
	            args[_i - 2] = arguments[_i];
	        }
	        if (str == null || str === "") return -1;
	        var startIndex = args.length > 0 ? args[0] : 0;
	        if (startIndex < 0) throw new Error("String.indexOfAny: Start index cannot be negative");
	        var length = args.length > 1 ? args[1] : str.length - startIndex;
	        if (length < 0) throw new Error("String.indexOfAny: Length cannot be negative");
	        if (length > str.length - startIndex) throw new Error("String.indexOfAny: Invalid startIndex and length");
	        str = str.substr(startIndex, length);
	        for (var _a = 0, anyOf_1 = anyOf; _a < anyOf_1.length; _a++) {
	            var c = anyOf_1[_a];
	            var index = str.indexOf(c);
	            if (index > -1) return index + startIndex;
	        }
	        return -1;
	    }
	    exports.indexOfAny = indexOfAny;
	    function toHex(value) {
	        return value < 0 ? "ff" + (16777215 - (Math.abs(value) - 1)).toString(16) : value.toString(16);
	    }
	    function fsFormat(str) {
	        var args = [];
	        for (var _i = 1; _i < arguments.length; _i++) {
	            args[_i - 1] = arguments[_i];
	        }
	        var _cont;
	        function isObject(x) {
	            return x !== null && (typeof x === 'undefined' ? 'undefined' : _typeof(x)) === "object" && !(x instanceof Number) && !(x instanceof String) && !(x instanceof Boolean);
	        }
	        function formatOnce(str, rep) {
	            return str.replace(fsFormatRegExp, function (_, prefix, flags, pad, precision, format) {
	                switch (format) {
	                    case "f":
	                    case "F":
	                        rep = rep.toFixed(precision || 6);
	                        break;
	                    case "g":
	                    case "G":
	                        rep = rep.toPrecision(precision);
	                        break;
	                    case "e":
	                    case "E":
	                        rep = rep.toExponential(precision);
	                        break;
	                    case "O":
	                        rep = Util_1.toString(rep);
	                        break;
	                    case "A":
	                        try {
	                            rep = JSON.stringify(rep, function (k, v) {
	                                return v && v[Symbol.iterator] && !Array.isArray(v) && isObject(v) ? Array.from(v) : v && typeof v.ToString === "function" ? Util_1.toString(v) : v;
	                            });
	                        } catch (err) {
	                            rep = "{" + Object.getOwnPropertyNames(rep).map(function (k) {
	                                return k + ": " + String(rep[k]);
	                            }).join(", ") + "}";
	                        }
	                        break;
	                    case "x":
	                        rep = toHex(Number(rep));
	                        break;
	                    case "X":
	                        rep = toHex(Number(rep)).toUpperCase();
	                        break;
	                }
	                var plusPrefix = flags.indexOf("+") >= 0 && parseInt(rep) >= 0;
	                if (!isNaN(pad = parseInt(pad))) {
	                    var ch = pad >= 0 && flags.indexOf("0") >= 0 ? "0" : " ";
	                    rep = padLeft(rep, Math.abs(pad) - (plusPrefix ? 1 : 0), ch, pad < 0);
	                }
	                var once = prefix + (plusPrefix ? "+" + rep : rep);
	                return once.replace(/%/g, "%%");
	            });
	        }
	        function makeFn(str) {
	            return function (rep) {
	                var str2 = formatOnce(str, rep);
	                return fsFormatRegExp.test(str2) ? makeFn(str2) : _cont(str2.replace(/%%/g, "%"));
	            };
	        }
	        if (args.length === 0) {
	            return function (cont) {
	                _cont = cont;
	                return fsFormatRegExp.test(str) ? makeFn(str) : _cont(str);
	            };
	        } else {
	            for (var i = 0; i < args.length; i++) {
	                str = formatOnce(str, args[i]);
	            }
	            return str.replace(/%%/g, "%");
	        }
	    }
	    exports.fsFormat = fsFormat;
	    function format(str) {
	        var args = [];
	        for (var _i = 1; _i < arguments.length; _i++) {
	            args[_i - 1] = arguments[_i];
	        }
	        return str.replace(formatRegExp, function (match, idx, pad, format) {
	            var rep = args[idx],
	                padSymbol = " ";
	            if (typeof rep === "number") {
	                switch ((format || "").substring(0, 1)) {
	                    case "f":
	                    case "F":
	                        rep = format.length > 1 ? rep.toFixed(format.substring(1)) : rep.toFixed(2);
	                        break;
	                    case "g":
	                    case "G":
	                        rep = format.length > 1 ? rep.toPrecision(format.substring(1)) : rep.toPrecision();
	                        break;
	                    case "e":
	                    case "E":
	                        rep = format.length > 1 ? rep.toExponential(format.substring(1)) : rep.toExponential();
	                        break;
	                    case "p":
	                    case "P":
	                        rep = (format.length > 1 ? (rep * 100).toFixed(format.substring(1)) : (rep * 100).toFixed(2)) + " %";
	                        break;
	                    case "x":
	                        rep = toHex(Number(rep));
	                        break;
	                    case "X":
	                        rep = toHex(Number(rep)).toUpperCase();
	                        break;
	                    default:
	                        var m = /^(0+)(\.0+)?$/.exec(format);
	                        if (m != null) {
	                            var decs = 0;
	                            if (m[2] != null) rep = rep.toFixed(decs = m[2].length - 1);
	                            pad = "," + (m[1].length + (decs ? decs + 1 : 0)).toString();
	                            padSymbol = "0";
	                        } else if (format) {
	                            rep = format;
	                        }
	                }
	            } else if (rep instanceof Date) {
	                if (format.length === 1) {
	                    switch (format) {
	                        case "D":
	                            rep = rep.toDateString();
	                            break;
	                        case "T":
	                            rep = rep.toLocaleTimeString();
	                            break;
	                        case "d":
	                            rep = rep.toLocaleDateString();
	                            break;
	                        case "t":
	                            rep = rep.toLocaleTimeString().replace(/:\d\d(?!:)/, "");
	                            break;
	                        case "o":
	                        case "O":
	                            if (rep.kind === 2) {
	                                var offset = rep.getTimezoneOffset() * -1;
	                                rep = format("{0:yyyy-MM-dd}T{0:HH:mm}:{1:00.000}{2}{3:00}:{4:00}", rep, Date_1.second(rep), offset >= 0 ? "+" : "-", ~~(offset / 60), offset % 60);
	                            } else {
	                                rep = rep.toISOString();
	                            }
	                    }
	                } else {
	                    rep = format.replace(/\w+/g, function (match2) {
	                        var rep2 = match2;
	                        switch (match2.substring(0, 1)) {
	                            case "y":
	                                rep2 = match2.length < 4 ? Date_6.year(rep) % 100 : Date_6.year(rep);
	                                break;
	                            case "h":
	                                rep2 = rep.getHours() > 12 ? Date_3.hour(rep) % 12 : Date_3.hour(rep);
	                                break;
	                            case "M":
	                                rep2 = Date_5.month(rep);
	                                break;
	                            case "d":
	                                rep2 = Date_4.day(rep);
	                                break;
	                            case "H":
	                                rep2 = Date_3.hour(rep);
	                                break;
	                            case "m":
	                                rep2 = Date_2.minute(rep);
	                                break;
	                            case "s":
	                                rep2 = Date_1.second(rep);
	                                break;
	                        }
	                        if (rep2 !== match2 && rep2 < 10 && match2.length > 1) {
	                            rep2 = "0" + rep2;
	                        }
	                        return rep2;
	                    });
	                }
	            }
	            if (!isNaN(pad = parseInt((pad || "").substring(1)))) {
	                rep = padLeft(rep, Math.abs(pad), padSymbol, pad < 0);
	            }
	            return rep;
	        });
	    }
	    exports.format = format;
	    function endsWith(str, search) {
	        var idx = str.lastIndexOf(search);
	        return idx >= 0 && idx == str.length - search.length;
	    }
	    exports.endsWith = endsWith;
	    function initialize(n, f) {
	        if (n < 0) throw new Error("String length must be non-negative");
	        var xs = new Array(n);
	        for (var i = 0; i < n; i++) {
	            xs[i] = f(i);
	        }return xs.join("");
	    }
	    exports.initialize = initialize;
	    function insert(str, startIndex, value) {
	        if (startIndex < 0 || startIndex > str.length) {
	            throw new Error("startIndex is negative or greater than the length of this instance.");
	        }
	        return str.substring(0, startIndex) + value + str.substring(startIndex);
	    }
	    exports.insert = insert;
	    function isNullOrEmpty(str) {
	        return typeof str !== "string" || str.length == 0;
	    }
	    exports.isNullOrEmpty = isNullOrEmpty;
	    function isNullOrWhiteSpace(str) {
	        return typeof str !== "string" || /^\s*$/.test(str);
	    }
	    exports.isNullOrWhiteSpace = isNullOrWhiteSpace;
	    function join(delimiter, xs) {
	        xs = typeof xs == "string" ? Util_2.getRestParams(arguments, 1) : xs;
	        return (Array.isArray(xs) ? xs : Array.from(xs)).join(delimiter);
	    }
	    exports.join = join;
	    function newGuid() {
	        var uuid = "";
	        for (var i = 0; i < 32; i++) {
	            var random = Math.random() * 16 | 0;
	            if (i === 8 || i === 12 || i === 16 || i === 20) uuid += "-";
	            uuid += (i === 12 ? 4 : i === 16 ? random & 3 | 8 : random).toString(16);
	        }
	        return uuid;
	    }
	    exports.newGuid = newGuid;
	    function padLeft(str, len, ch, isRight) {
	        ch = ch || " ";
	        str = String(str);
	        len = len - str.length;
	        for (var i = -1; ++i < len;) {
	            str = isRight ? str + ch : ch + str;
	        }return str;
	    }
	    exports.padLeft = padLeft;
	    function padRight(str, len, ch) {
	        return padLeft(str, len, ch, true);
	    }
	    exports.padRight = padRight;
	    function remove(str, startIndex, count) {
	        if (startIndex >= str.length) {
	            throw new Error("startIndex must be less than length of string");
	        }
	        if (typeof count === "number" && startIndex + count > str.length) {
	            throw new Error("Index and count must refer to a location within the string.");
	        }
	        return str.slice(0, startIndex) + (typeof count === "number" ? str.substr(startIndex + count) : "");
	    }
	    exports.remove = remove;
	    function replace(str, search, replace) {
	        return str.replace(new RegExp(RegExp_1.escape(search), "g"), replace);
	    }
	    exports.replace = replace;
	    function replicate(n, x) {
	        return initialize(n, function () {
	            return x;
	        });
	    }
	    exports.replicate = replicate;
	    function split(str, splitters, count, removeEmpty) {
	        count = typeof count == "number" ? count : null;
	        removeEmpty = typeof removeEmpty == "number" ? removeEmpty : null;
	        if (count < 0) throw new Error("Count cannot be less than zero");
	        if (count === 0) return [];
	        splitters = Array.isArray(splitters) ? splitters : Util_2.getRestParams(arguments, 1);
	        splitters = splitters.map(function (x) {
	            return RegExp_1.escape(x);
	        });
	        splitters = splitters.length > 0 ? splitters : [" "];
	        var m;
	        var i = 0;
	        var splits = [];
	        var reg = new RegExp(splitters.join("|"), "g");
	        while ((count == null || count > 1) && (m = reg.exec(str)) !== null) {
	            if (!removeEmpty || m.index - i > 0) {
	                count = count != null ? count - 1 : count;
	                splits.push(str.substring(i, m.index));
	            }
	            i = reg.lastIndex;
	        }
	        if (!removeEmpty || str.length - i > 0) splits.push(str.substring(i));
	        return splits;
	    }
	    exports.split = split;
	    function trim(str, side) {
	        var chars = [];
	        for (var _i = 2; _i < arguments.length; _i++) {
	            chars[_i - 2] = arguments[_i];
	        }
	        if (side == "both" && chars.length == 0) return str.trim();
	        if (side == "start" || side == "both") {
	            var reg = chars.length == 0 ? /^\s+/ : new RegExp("^[" + RegExp_1.escape(chars.join("")) + "]+");
	            str = str.replace(reg, "");
	        }
	        if (side == "end" || side == "both") {
	            var reg = chars.length == 0 ? /\s+$/ : new RegExp("[" + RegExp_1.escape(chars.join("")) + "]+$");
	            str = str.replace(reg, "");
	        }
	        return str;
	    }
	    exports.trim = trim;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 5 */
/***/ function(module, exports, __webpack_require__) {

	var map = {
		"./Array": 6,
		"./Array.js": 6,
		"./Assert": 8,
		"./Assert.js": 8,
		"./Async": 9,
		"./Async.js": 9,
		"./AsyncBuilder": 10,
		"./AsyncBuilder.js": 10,
		"./BigInt": 11,
		"./BigInt.js": 11,
		"./BigInt/BigNat": 12,
		"./BigInt/BigNat.js": 12,
		"./BigInt/FFT": 14,
		"./BigInt/FFT.js": 14,
		"./BitConverter": 15,
		"./BitConverter.js": 15,
		"./Choice": 16,
		"./Choice.js": 16,
		"./Date": 17,
		"./Date.js": 17,
		"./Event": 18,
		"./Event.js": 18,
		"./GenericComparer": 19,
		"./GenericComparer.js": 19,
		"./Lazy": 20,
		"./Lazy.js": 20,
		"./List": 21,
		"./List.js": 21,
		"./ListClass": 22,
		"./ListClass.js": 22,
		"./Long": 23,
		"./Long.js": 23,
		"./MailboxProcessor": 24,
		"./MailboxProcessor.js": 24,
		"./Map": 25,
		"./Map.js": 25,
		"./Observable": 26,
		"./Observable.js": 26,
		"./Reflection": 27,
		"./Reflection.js": 27,
		"./RegExp": 28,
		"./RegExp.js": 28,
		"./Seq": 29,
		"./Seq.js": 29,
		"./Serialize": 30,
		"./Serialize.js": 30,
		"./Set": 31,
		"./Set.js": 31,
		"./String": 4,
		"./String.js": 4,
		"./Symbol": 32,
		"./Symbol.js": 32,
		"./TimeSpan": 33,
		"./TimeSpan.js": 33,
		"./Timer": 34,
		"./Timer.js": 34,
		"./Util": 35,
		"./Util.js": 35
	};
	function webpackContext(req) {
		return __webpack_require__(webpackContextResolve(req));
	};
	function webpackContextResolve(req) {
		return map[req] || (function() { throw new Error("Cannot find module '" + req + "'.") }());
	};
	webpackContext.keys = function webpackContextKeys() {
		return Object.keys(map);
	};
	webpackContext.resolve = webpackContextResolve;
	module.exports = webpackContext;
	webpackContext.id = 5;


/***/ },
/* 6 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports"], function (require, exports) {
	    "use strict";
	
	    function addRangeInPlace(range, xs) {
	        var iter = range[Symbol.iterator]();
	        var cur = iter.next();
	        while (!cur.done) {
	            xs.push(cur.value);
	            cur = iter.next();
	        }
	    }
	    exports.addRangeInPlace = addRangeInPlace;
	    function copyTo(source, sourceIndex, target, targetIndex, count) {
	        while (count--) {
	            target[targetIndex++] = source[sourceIndex++];
	        }
	    }
	    exports.copyTo = copyTo;
	    function partition(f, xs) {
	        var ys = [],
	            zs = [],
	            j = 0,
	            k = 0;
	        for (var i = 0; i < xs.length; i++) {
	            if (f(xs[i])) ys[j++] = xs[i];else zs[k++] = xs[i];
	        }return [ys, zs];
	    }
	    exports.partition = partition;
	    function permute(f, xs) {
	        var ys = xs.map(function () {
	            return null;
	        });
	        var checkFlags = new Array(xs.length);
	        for (var i = 0; i < xs.length; i++) {
	            var j = f(i);
	            if (j < 0 || j >= xs.length) throw new Error("Not a valid permutation");
	            ys[j] = xs[i];
	            checkFlags[j] = 1;
	        }
	        for (var i = 0; i < xs.length; i++) {
	            if (checkFlags[i] != 1) throw new Error("Not a valid permutation");
	        }return ys;
	    }
	    exports.permute = permute;
	    function removeInPlace(item, xs) {
	        var i = xs.indexOf(item);
	        if (i > -1) {
	            xs.splice(i, 1);
	            return true;
	        }
	        return false;
	    }
	    exports.removeInPlace = removeInPlace;
	    function setSlice(target, lower, upper, source) {
	        var length = (upper || target.length - 1) - lower;
	        if (ArrayBuffer.isView(target) && source.length <= length) target.set(source, lower);else for (var i = lower | 0, j = 0; j <= length; i++, j++) {
	            target[i] = source[j];
	        }
	    }
	    exports.setSlice = setSlice;
	    function sortInPlaceBy(f, xs, dir) {
	        if (dir === void 0) {
	            dir = 1;
	        }
	        return xs.sort(function (x, y) {
	            x = f(x);
	            y = f(y);
	            return (x < y ? -1 : x == y ? 0 : 1) * dir;
	        });
	    }
	    exports.sortInPlaceBy = sortInPlaceBy;
	    function unzip(xs) {
	        var bs = new Array(xs.length),
	            cs = new Array(xs.length);
	        for (var i = 0; i < xs.length; i++) {
	            bs[i] = xs[i][0];
	            cs[i] = xs[i][1];
	        }
	        return [bs, cs];
	    }
	    exports.unzip = unzip;
	    function unzip3(xs) {
	        var bs = new Array(xs.length),
	            cs = new Array(xs.length),
	            ds = new Array(xs.length);
	        for (var i = 0; i < xs.length; i++) {
	            bs[i] = xs[i][0];
	            cs[i] = xs[i][1];
	            ds[i] = xs[i][2];
	        }
	        return [bs, cs, ds];
	    }
	    exports.unzip3 = unzip3;
	    function getSubArray(xs, startIndex, count) {
	        return xs.slice(startIndex, startIndex + count);
	    }
	    exports.getSubArray = getSubArray;
	    function fill(target, targetIndex, count, value) {
	        target.fill(value, targetIndex, targetIndex + count);
	    }
	    exports.fill = fill;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 7 */
/***/ function(module, exports) {

	module.exports = function() { throw new Error("define cannot be used indirect"); };


/***/ },
/* 8 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	var __extends = undefined && undefined.__extends || function (d, b) {
	    for (var p in b) {
	        if (b.hasOwnProperty(p)) d[p] = b[p];
	    }function __() {
	        this.constructor = d;
	    }
	    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
	};
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Util"], function (require, exports) {
	    "use strict";
	
	    var Util_1 = require("./Util");
	    var AssertionError = function (_super) {
	        __extends(AssertionError, _super);
	        function AssertionError(msg, actual, expected) {
	            var _this = _super.call(this, msg) || this;
	            _this.actual = actual;
	            _this.expected = expected;
	            return _this;
	        }
	        return AssertionError;
	    }(Error);
	    exports.AssertionError = AssertionError;
	    function equal(actual, expected, msg) {
	        if (!Util_1.equals(actual, expected)) {
	            throw new AssertionError(msg || "Expected: " + expected + " - Actual: " + actual, actual, expected);
	        }
	    }
	    exports.equal = equal;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 9 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./AsyncBuilder", "./AsyncBuilder", "./AsyncBuilder", "./AsyncBuilder", "./Choice", "./Choice", "./Seq"], function (require, exports) {
	    "use strict";
	
	    var AsyncBuilder_1 = require("./AsyncBuilder");
	    var AsyncBuilder_2 = require("./AsyncBuilder");
	    var AsyncBuilder_3 = require("./AsyncBuilder");
	    var AsyncBuilder_4 = require("./AsyncBuilder");
	    var Choice_1 = require("./Choice");
	    var Choice_2 = require("./Choice");
	    var Seq_1 = require("./Seq");
	    var Async = function () {
	        function Async() {}
	        return Async;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = Async;
	    function emptyContinuation(x) {}
	    function awaitPromise(p) {
	        return fromContinuations(function (conts) {
	            return p.then(conts[0]).catch(function (err) {
	                return (err == "cancelled" ? conts[2] : conts[1])(err);
	            });
	        });
	    }
	    exports.awaitPromise = awaitPromise;
	    function cancellationToken() {
	        return AsyncBuilder_2.protectedCont(function (ctx) {
	            return ctx.onSuccess(ctx.cancelToken);
	        });
	    }
	    exports.cancellationToken = cancellationToken;
	    exports.defaultCancellationToken = { isCancelled: false };
	    function catchAsync(work) {
	        return AsyncBuilder_2.protectedCont(function (ctx) {
	            work({
	                onSuccess: function onSuccess(x) {
	                    return ctx.onSuccess(Choice_1.choice1Of2(x));
	                },
	                onError: function onError(ex) {
	                    return ctx.onSuccess(Choice_2.choice2Of2(ex));
	                },
	                onCancel: ctx.onCancel,
	                cancelToken: ctx.cancelToken,
	                trampoline: ctx.trampoline
	            });
	        });
	    }
	    exports.catchAsync = catchAsync;
	    function fromContinuations(f) {
	        return AsyncBuilder_2.protectedCont(function (ctx) {
	            return f([ctx.onSuccess, ctx.onError, ctx.onCancel]);
	        });
	    }
	    exports.fromContinuations = fromContinuations;
	    function ignore(computation) {
	        return AsyncBuilder_3.protectedBind(computation, function (x) {
	            return AsyncBuilder_4.protectedReturn(void 0);
	        });
	    }
	    exports.ignore = ignore;
	    function parallel(computations) {
	        return awaitPromise(Promise.all(Seq_1.map(function (w) {
	            return startAsPromise(w);
	        }, computations)));
	    }
	    exports.parallel = parallel;
	    function sleep(millisecondsDueTime) {
	        return AsyncBuilder_2.protectedCont(function (ctx) {
	            setTimeout(function () {
	                return ctx.cancelToken.isCancelled ? ctx.onCancel("cancelled") : ctx.onSuccess(void 0);
	            }, millisecondsDueTime);
	        });
	    }
	    exports.sleep = sleep;
	    function start(computation, cancellationToken) {
	        return startWithContinuations(computation, cancellationToken);
	    }
	    exports.start = start;
	    function startImmediate(computation, cancellationToken) {
	        return start(computation, cancellationToken);
	    }
	    exports.startImmediate = startImmediate;
	    function startWithContinuations(computation, continuation, exceptionContinuation, cancellationContinuation, cancelToken) {
	        if (typeof continuation !== "function") {
	            cancelToken = continuation;
	            continuation = null;
	        }
	        var trampoline = new AsyncBuilder_1.Trampoline();
	        computation({
	            onSuccess: continuation ? continuation : emptyContinuation,
	            onError: exceptionContinuation ? exceptionContinuation : emptyContinuation,
	            onCancel: cancellationContinuation ? cancellationContinuation : emptyContinuation,
	            cancelToken: cancelToken ? cancelToken : exports.defaultCancellationToken,
	            trampoline: trampoline
	        });
	    }
	    exports.startWithContinuations = startWithContinuations;
	    function startAsPromise(computation, cancellationToken) {
	        return new Promise(function (resolve, reject) {
	            return startWithContinuations(computation, resolve, reject, reject, cancellationToken ? cancellationToken : exports.defaultCancellationToken);
	        });
	    }
	    exports.startAsPromise = startAsPromise;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 10 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports"], function (require, exports) {
	    "use strict";
	
	    var Trampoline = function () {
	        function Trampoline() {
	            this.callCount = 0;
	        }
	        Object.defineProperty(Trampoline, "maxTrampolineCallCount", {
	            get: function get() {
	                return 2000;
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Trampoline.prototype.incrementAndCheck = function () {
	            return this.callCount++ > Trampoline.maxTrampolineCallCount;
	        };
	        Trampoline.prototype.hijack = function (f) {
	            this.callCount = 0;
	            setTimeout(f, 0);
	        };
	        return Trampoline;
	    }();
	    exports.Trampoline = Trampoline;
	    function protectedCont(f) {
	        return function (ctx) {
	            if (ctx.cancelToken.isCancelled) ctx.onCancel("cancelled");else if (ctx.trampoline.incrementAndCheck()) ctx.trampoline.hijack(function () {
	                try {
	                    f(ctx);
	                } catch (err) {
	                    ctx.onError(err);
	                }
	            });else try {
	                f(ctx);
	            } catch (err) {
	                ctx.onError(err);
	            }
	        };
	    }
	    exports.protectedCont = protectedCont;
	    function protectedBind(computation, binder) {
	        return protectedCont(function (ctx) {
	            computation({
	                onSuccess: function onSuccess(x) {
	                    return binder(x)(ctx);
	                },
	                onError: ctx.onError,
	                onCancel: ctx.onCancel,
	                cancelToken: ctx.cancelToken,
	                trampoline: ctx.trampoline
	            });
	        });
	    }
	    exports.protectedBind = protectedBind;
	    function protectedReturn(value) {
	        return protectedCont(function (ctx) {
	            return ctx.onSuccess(value);
	        });
	    }
	    exports.protectedReturn = protectedReturn;
	    var AsyncBuilder = function () {
	        function AsyncBuilder() {}
	        AsyncBuilder.prototype.Bind = function (computation, binder) {
	            return protectedBind(computation, binder);
	        };
	        AsyncBuilder.prototype.Combine = function (computation1, computation2) {
	            return this.Bind(computation1, function () {
	                return computation2;
	            });
	        };
	        AsyncBuilder.prototype.Delay = function (generator) {
	            return protectedCont(function (ctx) {
	                return generator()(ctx);
	            });
	        };
	        AsyncBuilder.prototype.For = function (sequence, body) {
	            var iter = sequence[Symbol.iterator]();
	            var cur = iter.next();
	            return this.While(function () {
	                return !cur.done;
	            }, this.Delay(function () {
	                var res = body(cur.value);
	                cur = iter.next();
	                return res;
	            }));
	        };
	        AsyncBuilder.prototype.Return = function (value) {
	            return protectedReturn(value);
	        };
	        AsyncBuilder.prototype.ReturnFrom = function (computation) {
	            return computation;
	        };
	        AsyncBuilder.prototype.TryFinally = function (computation, compensation) {
	            return protectedCont(function (ctx) {
	                computation({
	                    onSuccess: function onSuccess(x) {
	                        compensation();
	                        ctx.onSuccess(x);
	                    },
	                    onError: function onError(x) {
	                        compensation();
	                        ctx.onError(x);
	                    },
	                    onCancel: function onCancel(x) {
	                        compensation();
	                        ctx.onCancel(x);
	                    },
	                    cancelToken: ctx.cancelToken,
	                    trampoline: ctx.trampoline
	                });
	            });
	        };
	        AsyncBuilder.prototype.TryWith = function (computation, catchHandler) {
	            return protectedCont(function (ctx) {
	                computation({
	                    onSuccess: ctx.onSuccess,
	                    onCancel: ctx.onCancel,
	                    cancelToken: ctx.cancelToken,
	                    trampoline: ctx.trampoline,
	                    onError: function onError(ex) {
	                        try {
	                            catchHandler(ex)(ctx);
	                        } catch (ex2) {
	                            ctx.onError(ex2);
	                        }
	                    }
	                });
	            });
	        };
	        AsyncBuilder.prototype.Using = function (resource, binder) {
	            return this.TryFinally(binder(resource), function () {
	                return resource.Dispose();
	            });
	        };
	        AsyncBuilder.prototype.While = function (guard, computation) {
	            var _this = this;
	            if (guard()) return this.Bind(computation, function () {
	                return _this.While(guard, computation);
	            });else return this.Return(void 0);
	        };
	        AsyncBuilder.prototype.Zero = function () {
	            return protectedCont(function (ctx) {
	                return ctx.onSuccess(void 0);
	            });
	        };
	        return AsyncBuilder;
	    }();
	    exports.AsyncBuilder = AsyncBuilder;
	    exports.singleton = new AsyncBuilder();
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 11 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Symbol", "./Symbol", "./Util", "./BigInt/BigNat", "./BigInt/BigNat", "./Seq", "./Long", "./String"], function (require, exports) {
	    "use strict";
	
	    var Symbol_1 = require("./Symbol");
	    var Symbol_2 = require("./Symbol");
	    var Util_1 = require("./Util");
	    var BigNat_1 = require("./BigInt/BigNat");
	    var BigNat_2 = require("./BigInt/BigNat");
	    var Seq_1 = require("./Seq");
	    var Long_1 = require("./Long");
	    var String_1 = require("./String");
	    var BigInteger = function () {
	        function BigInteger(signInt, v) {
	            this.signInt = signInt;
	            this.v = v;
	        }
	        BigInteger.prototype[Symbol_2.default.reflection] = function () {
	            return {
	                type: "System.Numerics.BigInteger",
	                interfaces: ["FSharpRecord", "System.IComparable"],
	                properties: {
	                    signInt: "number",
	                    v: BigNat_2.default
	                }
	            };
	        };
	        Object.defineProperty(BigInteger.prototype, "Sign", {
	            get: function get() {
	                if (this.IsZero) {
	                    return 0;
	                } else {
	                    return this.signInt;
	                }
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(BigInteger.prototype, "SignInt", {
	            get: function get() {
	                return this.signInt;
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(BigInteger.prototype, "V", {
	            get: function get() {
	                return this.v;
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(BigInteger.prototype, "IsZero", {
	            get: function get() {
	                if (this.SignInt === 0) {
	                    return true;
	                } else {
	                    return BigNat_1.isZero(this.V);
	                }
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(BigInteger.prototype, "IsOne", {
	            get: function get() {
	                if (this.SignInt === 1) {
	                    return BigNat_1.isOne(this.V);
	                } else {
	                    return false;
	                }
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(BigInteger.prototype, "StructuredDisplayString", {
	            get: function get() {
	                return Util_1.toString(this);
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(BigInteger.prototype, "IsSmall", {
	            get: function get() {
	                if (this.IsZero) {
	                    return true;
	                } else {
	                    return BigNat_1.isSmall(this.V);
	                }
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(BigInteger.prototype, "IsNegative", {
	            get: function get() {
	                if (this.SignInt === -1) {
	                    return !this.IsZero;
	                } else {
	                    return false;
	                }
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(BigInteger.prototype, "IsPositive", {
	            get: function get() {
	                if (this.SignInt === 1) {
	                    return !this.IsZero;
	                } else {
	                    return false;
	                }
	            },
	            enumerable: true,
	            configurable: true
	        });
	        BigInteger.prototype.CompareTo = function (obj) {
	            if (obj instanceof BigInteger) {
	                var that = obj;
	                return compare(this, that);
	            } else {
	                throw new Error("the objects are not comparable" + '\nParameter name: ' + "obj");
	            }
	        };
	        BigInteger.prototype.ToString = function () {
	            var matchValue = this.SignInt;
	            var $var19 = null;
	            switch (matchValue) {
	                case 1:
	                    {
	                        $var19 = BigNat_1.toString(this.V);
	                        break;
	                    }
	                case -1:
	                    {
	                        if (BigNat_1.isZero(this.V)) {
	                            $var19 = "0";
	                        } else {
	                            $var19 = "-" + BigNat_1.toString(this.V);
	                        }
	                        break;
	                    }
	                case 0:
	                    {
	                        $var19 = "0";
	                        break;
	                    }
	                default:
	                    {
	                        throw new Error("signs should be +/- 1 or 0");
	                    }
	            }
	            return $var19;
	        };
	        BigInteger.prototype.Equals = function (obj) {
	            if (obj instanceof BigInteger) {
	                var that = obj;
	                return op_Equality(this, that);
	            } else {
	                return false;
	            }
	        };
	        BigInteger.prototype.GetHashCode = function () {
	            return hash(this);
	        };
	        return BigInteger;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = BigInteger;
	    Symbol_1.setType("System.Numerics.BigInteger", BigInteger);
	    var smallLim = 4096;
	    var smallPosTab = Array.from(Seq_1.initialize(smallLim, function (n) {
	        return BigNat_1.ofInt32(n);
	    }));
	    exports.one = fromInt32(1);
	    exports.two = fromInt32(2);
	    exports.zero = fromInt32(0);
	    function fromInt32(n) {
	        if (n >= 0) {
	            return new BigInteger(1, nat(BigNat_1.ofInt32(n)));
	        } else if (n === -2147483648) {
	            return new BigInteger(-1, nat(BigNat_1.ofInt64(Long_1.fromNumber(n, false).neg())));
	        } else {
	            return new BigInteger(-1, nat(BigNat_1.ofInt32(-n)));
	        }
	    }
	    exports.fromInt32 = fromInt32;
	    function fromInt64(n) {
	        if (n.CompareTo(Long_1.fromBits(0, 0, false)) >= 0) {
	            return new BigInteger(1, nat(BigNat_1.ofInt64(n)));
	        } else if (n.Equals(Long_1.fromBits(0, 2147483648, false))) {
	            return new BigInteger(-1, nat(BigNat_1.add(BigNat_1.ofInt64(Long_1.fromBits(4294967295, 2147483647, false)), BigNat_1.one)));
	        } else {
	            return new BigInteger(-1, nat(BigNat_1.ofInt64(n.neg())));
	        }
	    }
	    exports.fromInt64 = fromInt64;
	    function nat(n) {
	        if (BigNat_1.isSmall(n) ? BigNat_1.getSmall(n) < smallLim : false) {
	            return smallPosTab[BigNat_1.getSmall(n)];
	        } else {
	            return n;
	        }
	    }
	    exports.nat = nat;
	    function create(s, n) {
	        return new BigInteger(s, nat(n));
	    }
	    exports.create = create;
	    function posn(n) {
	        return new BigInteger(1, nat(n));
	    }
	    exports.posn = posn;
	    function negn(n) {
	        return new BigInteger(-1, nat(n));
	    }
	    exports.negn = negn;
	    function op_Equality(x, y) {
	        var matchValue = [x.SignInt, y.SignInt];
	        var _target9 = function _target9() {
	            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
	        };
	        if (matchValue[0] === -1) {
	            if (matchValue[1] === -1) {
	                return BigNat_1.equal(x.V, y.V);
	            } else if (matchValue[1] === 0) {
	                return BigNat_1.isZero(x.V);
	            } else if (matchValue[1] === 1) {
	                if (BigNat_1.isZero(x.V)) {
	                    return BigNat_1.isZero(y.V);
	                } else {
	                    return false;
	                }
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 0) {
	            if (matchValue[1] === -1) {
	                return BigNat_1.isZero(y.V);
	            } else if (matchValue[1] === 0) {
	                return true;
	            } else if (matchValue[1] === 1) {
	                return BigNat_1.isZero(y.V);
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 1) {
	            if (matchValue[1] === -1) {
	                if (BigNat_1.isZero(x.V)) {
	                    return BigNat_1.isZero(y.V);
	                } else {
	                    return false;
	                }
	            } else if (matchValue[1] === 0) {
	                return BigNat_1.isZero(x.V);
	            } else if (matchValue[1] === 1) {
	                return BigNat_1.equal(x.V, y.V);
	            } else {
	                return _target9();
	            }
	        } else {
	            return _target9();
	        }
	    }
	    exports.op_Equality = op_Equality;
	    function op_Inequality(x, y) {
	        return !op_Equality(x, y);
	    }
	    exports.op_Inequality = op_Inequality;
	    function op_LessThan(x, y) {
	        var matchValue = [x.SignInt, y.SignInt];
	        var _target9 = function _target9() {
	            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
	        };
	        if (matchValue[0] === -1) {
	            if (matchValue[1] === -1) {
	                return BigNat_1.lt(y.V, x.V);
	            } else if (matchValue[1] === 0) {
	                return !BigNat_1.isZero(x.V);
	            } else if (matchValue[1] === 1) {
	                if (!BigNat_1.isZero(x.V)) {
	                    return true;
	                } else {
	                    return !BigNat_1.isZero(y.V);
	                }
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 0) {
	            if (matchValue[1] === -1) {
	                return false;
	            } else if (matchValue[1] === 0) {
	                return false;
	            } else if (matchValue[1] === 1) {
	                return !BigNat_1.isZero(y.V);
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 1) {
	            if (matchValue[1] === -1) {
	                return false;
	            } else if (matchValue[1] === 0) {
	                return false;
	            } else if (matchValue[1] === 1) {
	                return BigNat_1.lt(x.V, y.V);
	            } else {
	                return _target9();
	            }
	        } else {
	            return _target9();
	        }
	    }
	    exports.op_LessThan = op_LessThan;
	    function op_GreaterThan(x, y) {
	        var matchValue = [x.SignInt, y.SignInt];
	        var _target9 = function _target9() {
	            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
	        };
	        if (matchValue[0] === -1) {
	            if (matchValue[1] === -1) {
	                return BigNat_1.gt(y.V, x.V);
	            } else if (matchValue[1] === 0) {
	                return false;
	            } else if (matchValue[1] === 1) {
	                return false;
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 0) {
	            if (matchValue[1] === -1) {
	                return !BigNat_1.isZero(y.V);
	            } else if (matchValue[1] === 0) {
	                return false;
	            } else if (matchValue[1] === 1) {
	                return false;
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 1) {
	            if (matchValue[1] === -1) {
	                if (!BigNat_1.isZero(x.V)) {
	                    return true;
	                } else {
	                    return !BigNat_1.isZero(y.V);
	                }
	            } else if (matchValue[1] === 0) {
	                return !BigNat_1.isZero(x.V);
	            } else if (matchValue[1] === 1) {
	                return BigNat_1.gt(x.V, y.V);
	            } else {
	                return _target9();
	            }
	        } else {
	            return _target9();
	        }
	    }
	    exports.op_GreaterThan = op_GreaterThan;
	    function compare(n, nn) {
	        if (op_LessThan(n, nn)) {
	            return -1;
	        } else if (op_Equality(n, nn)) {
	            return 0;
	        } else {
	            return 1;
	        }
	    }
	    exports.compare = compare;
	    function hash(z) {
	        if (z.SignInt === 0) {
	            return 1;
	        } else {
	            return z.SignInt + hash(z.V);
	        }
	    }
	    exports.hash = hash;
	    function op_UnaryNegation(z) {
	        var matchValue = z.SignInt;
	        if (matchValue === 0) {
	            return exports.zero;
	        } else {
	            return create(-matchValue, z.V);
	        }
	    }
	    exports.op_UnaryNegation = op_UnaryNegation;
	    function scale(k, z) {
	        if (z.SignInt === 0) {
	            return exports.zero;
	        } else if (k < 0) {
	            return create(-z.SignInt, BigNat_1.scale(-k, z.V));
	        } else {
	            return create(z.SignInt, BigNat_1.scale(k, z.V));
	        }
	    }
	    exports.scale = scale;
	    function subnn(nx, ny) {
	        if (BigNat_1.gte(nx, ny)) {
	            return posn(BigNat_1.sub(nx, ny));
	        } else {
	            return negn(BigNat_1.sub(ny, nx));
	        }
	    }
	    exports.subnn = subnn;
	    function addnn(nx, ny) {
	        return posn(BigNat_1.add(nx, ny));
	    }
	    exports.addnn = addnn;
	    function op_Addition(x, y) {
	        if (y.IsZero) {
	            return x;
	        } else if (x.IsZero) {
	            return y;
	        } else {
	            var matchValue = [x.SignInt, y.SignInt];
	            var _target4 = function _target4() {
	                throw new Error("signs should be +/- 1" + '\nParameter name: ' + "x");
	            };
	            if (matchValue[0] === -1) {
	                if (matchValue[1] === -1) {
	                    return op_UnaryNegation(addnn(x.V, y.V));
	                } else if (matchValue[1] === 1) {
	                    return subnn(y.V, x.V);
	                } else {
	                    return _target4();
	                }
	            } else if (matchValue[0] === 1) {
	                if (matchValue[1] === -1) {
	                    return subnn(x.V, y.V);
	                } else if (matchValue[1] === 1) {
	                    return addnn(x.V, y.V);
	                } else {
	                    return _target4();
	                }
	            } else {
	                return _target4();
	            }
	        }
	    }
	    exports.op_Addition = op_Addition;
	    function op_Subtraction(x, y) {
	        if (y.IsZero) {
	            return x;
	        } else if (x.IsZero) {
	            return op_UnaryNegation(y);
	        } else {
	            var matchValue = [x.SignInt, y.SignInt];
	            var _target4 = function _target4() {
	                throw new Error("signs should be +/- 1" + '\nParameter name: ' + "x");
	            };
	            if (matchValue[0] === -1) {
	                if (matchValue[1] === -1) {
	                    return subnn(y.V, x.V);
	                } else if (matchValue[1] === 1) {
	                    return op_UnaryNegation(addnn(x.V, y.V));
	                } else {
	                    return _target4();
	                }
	            } else if (matchValue[0] === 1) {
	                if (matchValue[1] === -1) {
	                    return addnn(x.V, y.V);
	                } else if (matchValue[1] === 1) {
	                    return subnn(x.V, y.V);
	                } else {
	                    return _target4();
	                }
	            } else {
	                return _target4();
	            }
	        }
	    }
	    exports.op_Subtraction = op_Subtraction;
	    function op_Multiply(x, y) {
	        if (x.IsZero) {
	            return x;
	        } else if (y.IsZero) {
	            return y;
	        } else if (x.IsOne) {
	            return y;
	        } else if (y.IsOne) {
	            return x;
	        } else {
	            var m = BigNat_1.mul(x.V, y.V);
	            return create(x.SignInt * y.SignInt, m);
	        }
	    }
	    exports.op_Multiply = op_Multiply;
	    function divRem(x, y) {
	        if (y.IsZero) {
	            throw new Error();
	        }
	        if (x.IsZero) {
	            return [exports.zero, exports.zero];
	        } else {
	            var patternInput = BigNat_1.divmod(x.V, y.V);
	            var matchValue = [x.SignInt, y.SignInt];
	            var _target4 = function _target4() {
	                throw new Error("signs should be +/- 1" + '\nParameter name: ' + "x");
	            };
	            if (matchValue[0] === -1) {
	                if (matchValue[1] === -1) {
	                    return [posn(patternInput[0]), negn(patternInput[1])];
	                } else if (matchValue[1] === 1) {
	                    return [negn(patternInput[0]), negn(patternInput[1])];
	                } else {
	                    return _target4();
	                }
	            } else if (matchValue[0] === 1) {
	                if (matchValue[1] === -1) {
	                    return [negn(patternInput[0]), posn(patternInput[1])];
	                } else if (matchValue[1] === 1) {
	                    return [posn(patternInput[0]), posn(patternInput[1])];
	                } else {
	                    return _target4();
	                }
	            } else {
	                return _target4();
	            }
	        }
	    }
	    exports.divRem = divRem;
	    function op_Division(x, y) {
	        return divRem(x, y)[0];
	    }
	    exports.op_Division = op_Division;
	    function op_Modulus(x, y) {
	        return divRem(x, y)[1];
	    }
	    exports.op_Modulus = op_Modulus;
	    function op_RightShift(x, y) {
	        return op_Division(x, pow(exports.two, y));
	    }
	    exports.op_RightShift = op_RightShift;
	    function op_LeftShift(x, y) {
	        return op_Multiply(x, pow(exports.two, y));
	    }
	    exports.op_LeftShift = op_LeftShift;
	    function op_BitwiseAnd(x, y) {
	        return posn(BigNat_1.bitAnd(x.V, y.V));
	    }
	    exports.op_BitwiseAnd = op_BitwiseAnd;
	    function op_BitwiseOr(x, y) {
	        return posn(BigNat_1.bitOr(x.V, y.V));
	    }
	    exports.op_BitwiseOr = op_BitwiseOr;
	    function greatestCommonDivisor(x, y) {
	        var matchValue = [x.SignInt, y.SignInt];
	        if (matchValue[0] === 0) {
	            if (matchValue[1] === 0) {
	                return exports.zero;
	            } else {
	                return posn(y.V);
	            }
	        } else if (matchValue[1] === 0) {
	            return posn(x.V);
	        } else {
	            return posn(BigNat_1.hcf(x.V, y.V));
	        }
	    }
	    exports.greatestCommonDivisor = greatestCommonDivisor;
	    function abs(x) {
	        if (x.SignInt === -1) {
	            return op_UnaryNegation(x);
	        } else {
	            return x;
	        }
	    }
	    exports.abs = abs;
	    function op_LessThanOrEqual(x, y) {
	        var matchValue = [x.SignInt, y.SignInt];
	        var _target9 = function _target9() {
	            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
	        };
	        if (matchValue[0] === -1) {
	            if (matchValue[1] === -1) {
	                return BigNat_1.lte(y.V, x.V);
	            } else if (matchValue[1] === 0) {
	                return true;
	            } else if (matchValue[1] === 1) {
	                return true;
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 0) {
	            if (matchValue[1] === -1) {
	                return BigNat_1.isZero(y.V);
	            } else if (matchValue[1] === 0) {
	                return true;
	            } else if (matchValue[1] === 1) {
	                return true;
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 1) {
	            if (matchValue[1] === -1) {
	                if (BigNat_1.isZero(x.V)) {
	                    return BigNat_1.isZero(y.V);
	                } else {
	                    return false;
	                }
	            } else if (matchValue[1] === 0) {
	                return BigNat_1.isZero(x.V);
	            } else if (matchValue[1] === 1) {
	                return BigNat_1.lte(x.V, y.V);
	            } else {
	                return _target9();
	            }
	        } else {
	            return _target9();
	        }
	    }
	    exports.op_LessThanOrEqual = op_LessThanOrEqual;
	    function op_GreaterThanOrEqual(x, y) {
	        var matchValue = [x.SignInt, y.SignInt];
	        var _target9 = function _target9() {
	            throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
	        };
	        if (matchValue[0] === -1) {
	            if (matchValue[1] === -1) {
	                return BigNat_1.gte(y.V, x.V);
	            } else if (matchValue[1] === 0) {
	                return BigNat_1.isZero(x.V);
	            } else if (matchValue[1] === 1) {
	                if (BigNat_1.isZero(x.V)) {
	                    return BigNat_1.isZero(y.V);
	                } else {
	                    return false;
	                }
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 0) {
	            if (matchValue[1] === -1) {
	                return true;
	            } else if (matchValue[1] === 0) {
	                return true;
	            } else if (matchValue[1] === 1) {
	                return BigNat_1.isZero(y.V);
	            } else {
	                return _target9();
	            }
	        } else if (matchValue[0] === 1) {
	            if (matchValue[1] === -1) {
	                return true;
	            } else if (matchValue[1] === 0) {
	                return true;
	            } else if (matchValue[1] === 1) {
	                return BigNat_1.gte(x.V, y.V);
	            } else {
	                return _target9();
	            }
	        } else {
	            return _target9();
	        }
	    }
	    exports.op_GreaterThanOrEqual = op_GreaterThanOrEqual;
	    function op_Explicit_0(x) {
	        if (x.IsZero) {
	            return 0;
	        } else {
	            var u = BigNat_1.toUInt32(x.V);
	            if (u <= 2147483647 >>> 0) {
	                return x.SignInt * ~~u;
	            } else if (x.SignInt === -1 ? u === 2147483647 + 1 >>> 0 : false) {
	                return -2147483648;
	            } else {
	                throw new Error();
	            }
	        }
	    }
	    exports.op_Explicit_0 = op_Explicit_0;
	    function op_Explicit_0(x) {
	        if (x.IsZero) {
	            return 0;
	        } else {
	            return BigNat_1.toUInt32(x.V);
	        }
	    }
	    exports.op_Explicit_0 = op_Explicit_0;
	    function op_Explicit_0(x) {
	        if (x.IsZero) {
	            return Long_1.fromBits(0, 0, false);
	        } else {
	            var u = BigNat_1.toUInt64(x.V);
	            if (u.CompareTo(Long_1.fromNumber(Long_1.fromBits(4294967295, 2147483647, false).toNumber(), true)) <= 0) {
	                return Long_1.fromNumber(x.SignInt, false).mul(Long_1.fromNumber(u.toNumber(), false));
	            } else if (x.SignInt === -1 ? u.Equals(Long_1.fromNumber(Long_1.fromBits(4294967295, 2147483647, false).add(Long_1.fromBits(1, 0, false)).toNumber(), true)) : false) {
	                return Long_1.fromBits(0, 2147483648, false);
	            } else {
	                throw new Error();
	            }
	        }
	    }
	    exports.op_Explicit_0 = op_Explicit_0;
	    function op_Explicit_0(x) {
	        if (x.IsZero) {
	            return Long_1.fromBits(0, 0, true);
	        } else {
	            return BigNat_1.toUInt64(x.V);
	        }
	    }
	    exports.op_Explicit_0 = op_Explicit_0;
	    function op_Explicit_0(x) {
	        var matchValue = x.SignInt;
	        var $var20 = null;
	        switch (matchValue) {
	            case 1:
	                {
	                    $var20 = BigNat_1.toFloat(x.V);
	                    break;
	                }
	            case -1:
	                {
	                    $var20 = -BigNat_1.toFloat(x.V);
	                    break;
	                }
	            case 0:
	                {
	                    $var20 = 0;
	                    break;
	                }
	            default:
	                {
	                    throw new Error("signs should be +/- 1 or 0" + '\nParameter name: ' + "x");
	                }
	        }
	        return $var20;
	    }
	    exports.op_Explicit_0 = op_Explicit_0;
	    function parse(text) {
	        if (text == null) {
	            throw new Error("text");
	        }
	        var text_1 = String_1.trim(text, "both");
	        var len = text_1.length;
	        if (len === 0) {
	            throw new Error();
	        }
	        var matchValue = [text_1[0], len];
	        if (matchValue[0] === "+") {
	            if (matchValue[1] === 1) {
	                throw new Error();
	            } else {
	                return posn(BigNat_1.ofString(text_1.slice(1, len - 1 + 1)));
	            }
	        } else if (matchValue[0] === "-") {
	            if (matchValue[1] === 1) {
	                throw new Error();
	            } else {
	                return negn(BigNat_1.ofString(text_1.slice(1, len - 1 + 1)));
	            }
	        } else {
	            return posn(BigNat_1.ofString(text_1));
	        }
	    }
	    exports.parse = parse;
	    function factorial(x) {
	        if (x.IsNegative) {
	            throw new Error("mustBeNonNegative" + '\nParameter name: ' + "x");
	        }
	        if (x.IsPositive) {
	            return posn(BigNat_1.factorial(x.V));
	        } else {
	            return exports.one;
	        }
	    }
	    exports.factorial = factorial;
	    function op_UnaryPlus(n1) {
	        return n1;
	    }
	    exports.op_UnaryPlus = op_UnaryPlus;
	    function pow(x, y) {
	        if (y < 0) {
	            throw new Error("y");
	        }
	        var matchValue = [x.IsZero, y];
	        if (matchValue[0]) {
	            if (matchValue[1] === 0) {
	                return exports.one;
	            } else {
	                return exports.zero;
	            }
	        } else {
	            var yval = fromInt32(y);
	            return create(BigNat_1.isZero(BigNat_1.rem(yval.V, BigNat_1.two)) ? 1 : x.SignInt, BigNat_1.pow(x.V, yval.V));
	        }
	    }
	    exports.pow = pow;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 12 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(13), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "../Symbol", "../Symbol", "../Util", "../Long", "../Seq", "./FFT", "../List", "../String"], function (require, exports) {
	    "use strict";
	
	    var Symbol_1 = require("../Symbol");
	    var Symbol_2 = require("../Symbol");
	    var Util_1 = require("../Util");
	    var Long_1 = require("../Long");
	    var Seq_1 = require("../Seq");
	    var FFT_1 = require("./FFT");
	    var List_1 = require("../List");
	    var String_1 = require("../String");
	    var BigNat = function () {
	        function BigNat(bound, digits) {
	            this.bound = bound;
	            this.digits = digits;
	        }
	        BigNat.prototype[Symbol_2.default.reflection] = function () {
	            return {
	                type: "Microsoft.FSharp.Math.BigNat",
	                interfaces: ["FSharpRecord"],
	                properties: {
	                    bound: "number",
	                    digits: Util_1.Array(Int32Array, true)
	                }
	            };
	        };
	        return BigNat;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = BigNat;
	    Symbol_1.setType("Microsoft.FSharp.Math.BigNat", BigNat);
	    function bound(n) {
	        return n.bound;
	    }
	    exports.bound = bound;
	    function setBound(n, v) {
	        n.bound = v;
	    }
	    exports.setBound = setBound;
	    function coeff(n, i) {
	        return n.digits[i];
	    }
	    exports.coeff = coeff;
	    function coeff64(n, i) {
	        return Long_1.fromNumber(coeff(n, i), false);
	    }
	    exports.coeff64 = coeff64;
	    function setCoeff(n, i, v) {
	        n.digits[i] = v;
	    }
	    exports.setCoeff = setCoeff;
	    function pow64(x, n) {
	        if (n === 0) {
	            return Long_1.fromBits(1, 0, false);
	        } else if (n % 2 === 0) {
	            return pow64(x.mul(x), ~~(n / 2));
	        } else {
	            return x.mul(pow64(x.mul(x), ~~(n / 2)));
	        }
	    }
	    exports.pow64 = pow64;
	    function pow32(x, n) {
	        if (n === 0) {
	            return 1;
	        } else if (n % 2 === 0) {
	            return pow32(x * x, ~~(n / 2));
	        } else {
	            return x * pow32(x * x, ~~(n / 2));
	        }
	    }
	    exports.pow32 = pow32;
	    function hash(n) {
	        var res = 0;
	        for (var i = 0; i <= n.bound - 1; i++) {
	            res = n.digits[i] + (res << 3);
	        }
	        return res;
	    }
	    exports.hash = hash;
	    function maxInt(a, b) {
	        if (a < b) {
	            return b;
	        } else {
	            return a;
	        }
	    }
	    exports.maxInt = maxInt;
	    function minInt(a, b) {
	        if (a < b) {
	            return a;
	        } else {
	            return b;
	        }
	    }
	    exports.minInt = minInt;
	    exports.baseBits = 24;
	    exports.baseN = 16777216;
	    exports.baseMask = 16777215;
	    exports.baseNi64 = Long_1.fromBits(16777216, 0, false);
	    exports.baseMaski64 = Long_1.fromBits(16777215, 0, false);
	    exports.baseMaskU = Long_1.fromBits(16777215, 0, true);
	    exports.baseMask32A = 16777215;
	    exports.baseMask32B = 255;
	    exports.baseShift32B = 24;
	    exports.baseMask64A = 16777215;
	    exports.baseMask64B = 16777215;
	    exports.baseMask64C = 65535;
	    exports.baseShift64B = 24;
	    exports.baseShift64C = 48;
	    function divbase(x) {
	        return ~~(x >>> 0 >> exports.baseBits);
	    }
	    exports.divbase = divbase;
	    function modbase(x) {
	        return x & exports.baseMask;
	    }
	    exports.modbase = modbase;
	    function createN(b) {
	        return new BigNat(b, new Int32Array(b));
	    }
	    exports.createN = createN;
	    function copyN(x) {
	        return new BigNat(x.bound, x.digits.slice());
	    }
	    exports.copyN = copyN;
	    function normN(n) {
	        var findLeastBound = function findLeastBound(na) {
	            return function (i) {
	                if (i === -1 ? true : na[i] !== 0) {
	                    return i + 1;
	                } else {
	                    return findLeastBound(na)(i - 1);
	                }
	            };
	        };
	        var bound_1 = findLeastBound(n.digits)(n.bound - 1);
	        n.bound = bound_1;
	        return n;
	    }
	    exports.normN = normN;
	    exports.boundInt = 2;
	    exports.boundInt64 = 3;
	    exports.boundBase = 1;
	    function embed(x) {
	        var x_1 = x < 0 ? 0 : x;
	        if (x_1 < exports.baseN) {
	            var r = createN(1);
	            r.digits[0] = x_1;
	            return normN(r);
	        } else {
	            var r = createN(exports.boundInt);
	            for (var i = 0; i <= exports.boundInt - 1; i++) {
	                r.digits[i] = ~~(x_1 / pow32(exports.baseN, i)) % exports.baseN;
	            }
	            return normN(r);
	        }
	    }
	    exports.embed = embed;
	    function embed64(x) {
	        var x_1 = x.CompareTo(Long_1.fromBits(0, 0, false)) < 0 ? Long_1.fromBits(0, 0, false) : x;
	        var r = createN(exports.boundInt64);
	        for (var i = 0; i <= exports.boundInt64 - 1; i++) {
	            r.digits[i] = ~~x_1.div(pow64(exports.baseNi64, i)).mod(exports.baseNi64).toNumber();
	        }
	        return normN(r);
	    }
	    exports.embed64 = embed64;
	    function eval32(n) {
	        if (n.bound === 1) {
	            return n.digits[0];
	        } else {
	            var acc = 0;
	            for (var i = n.bound - 1; i >= 0; i--) {
	                acc = n.digits[i] + exports.baseN * acc;
	            }
	            return acc;
	        }
	    }
	    exports.eval32 = eval32;
	    function eval64(n) {
	        if (n.bound === 1) {
	            return Long_1.fromNumber(n.digits[0], false);
	        } else {
	            var acc = Long_1.fromBits(0, 0, false);
	            for (var i = n.bound - 1; i >= 0; i--) {
	                acc = Long_1.fromNumber(n.digits[i], false).add(exports.baseNi64.mul(acc));
	            }
	            return acc;
	        }
	    }
	    exports.eval64 = eval64;
	    exports.one = embed(1);
	    exports.zero = embed(0);
	    function restrictTo(d, n) {
	        return new BigNat(minInt(d, n.bound), n.digits);
	    }
	    exports.restrictTo = restrictTo;
	    function shiftUp(d, n) {
	        var m = createN(n.bound + d);
	        for (var i = 0; i <= n.bound - 1; i++) {
	            m.digits[i + d] = n.digits[i];
	        }
	        return m;
	    }
	    exports.shiftUp = shiftUp;
	    function shiftDown(d, n) {
	        if (n.bound - d <= 0) {
	            return exports.zero;
	        } else {
	            var m = createN(n.bound - d);
	            for (var i = 0; i <= m.bound - 1; i++) {
	                m.digits[i] = n.digits[i + d];
	            }
	            return m;
	        }
	    }
	    exports.shiftDown = shiftDown;
	    function degree(n) {
	        return n.bound - 1;
	    }
	    exports.degree = degree;
	    function addP(i, n, c, p, q, r) {
	        if (i < n) {
	            var x = function () {
	                var $var2 = i;
	                var $var1 = p;
	                if ($var2 < $var1.bound) {
	                    return $var1.digits[$var2];
	                } else {
	                    return 0;
	                }
	            }() + function () {
	                var $var4 = i;
	                var $var3 = q;
	                if ($var4 < $var3.bound) {
	                    return $var3.digits[$var4];
	                } else {
	                    return 0;
	                }
	            }() + c;
	            r.digits[i] = modbase(x);
	            var c_1 = divbase(x);
	            addP(i + 1, n, c_1, p, q, r);
	        }
	    }
	    exports.addP = addP;
	    function add(p, q) {
	        var rbound = 1 + maxInt(p.bound, q.bound);
	        var r = createN(rbound);
	        var carry = 0;
	        addP(0, rbound, carry, p, q, r);
	        return normN(r);
	    }
	    exports.add = add;
	    function subP(i, n, c, p, q, r) {
	        if (i < n) {
	            var x = function () {
	                var $var6 = i;
	                var $var5 = p;
	                if ($var6 < $var5.bound) {
	                    return $var5.digits[$var6];
	                } else {
	                    return 0;
	                }
	            }() - function () {
	                var $var8 = i;
	                var $var7 = q;
	                if ($var8 < $var7.bound) {
	                    return $var7.digits[$var8];
	                } else {
	                    return 0;
	                }
	            }() + c;
	            if (x > 0) {
	                r.digits[i] = modbase(x);
	                var c_1 = divbase(x);
	                return subP(i + 1, n, c_1, p, q, r);
	            } else {
	                var x_1 = x + exports.baseN;
	                r.digits[i] = modbase(x_1);
	                var c_1 = divbase(x_1) - 1;
	                return subP(i + 1, n, c_1, p, q, r);
	            }
	        } else {
	            var underflow = c !== 0;
	            return underflow;
	        }
	    }
	    exports.subP = subP;
	    function sub(p, q) {
	        var rbound = maxInt(p.bound, q.bound);
	        var r = createN(rbound);
	        var carry = 0;
	        var underflow = subP(0, rbound, carry, p, q, r);
	        if (underflow) {
	            return embed(0);
	        } else {
	            return normN(r);
	        }
	    }
	    exports.sub = sub;
	    function isZero(p) {
	        return p.bound === 0;
	    }
	    exports.isZero = isZero;
	    function IsZero(p) {
	        return isZero(p);
	    }
	    exports.IsZero = IsZero;
	    function isOne(p) {
	        if (p.bound === 1) {
	            return p.digits[0] === 1;
	        } else {
	            return false;
	        }
	    }
	    exports.isOne = isOne;
	    function equal(p, q) {
	        if (p.bound === q.bound) {
	            var check_1 = function check_1(pa) {
	                return function (qa) {
	                    return function (i) {
	                        if (i === -1) {
	                            return true;
	                        } else if (pa[i] === qa[i]) {
	                            return check_1(pa)(qa)(i - 1);
	                        } else {
	                            return false;
	                        }
	                    };
	                };
	            };
	            return check_1(p.digits)(q.digits)(p.bound - 1);
	        } else {
	            return false;
	        }
	    }
	    exports.equal = equal;
	    function shiftCompare(p, pn, q, qn) {
	        if (p.bound + pn < q.bound + qn) {
	            return -1;
	        } else if (p.bound + pn > q.bound + pn) {
	            return 1;
	        } else {
	            var check_2 = function check_2(pa) {
	                return function (qa) {
	                    return function (i) {
	                        if (i === -1) {
	                            return 0;
	                        } else {
	                            var pai = i < pn ? 0 : pa[i - pn];
	                            var qai = i < qn ? 0 : qa[i - qn];
	                            if (pai === qai) {
	                                return check_2(pa)(qa)(i - 1);
	                            } else if (pai < qai) {
	                                return -1;
	                            } else {
	                                return 1;
	                            }
	                        }
	                    };
	                };
	            };
	            return check_2(p.digits)(q.digits)(p.bound + pn - 1);
	        }
	    }
	    exports.shiftCompare = shiftCompare;
	    function compare(p, q) {
	        if (p.bound < q.bound) {
	            return -1;
	        } else if (p.bound > q.bound) {
	            return 1;
	        } else {
	            var check_3 = function check_3(pa) {
	                return function (qa) {
	                    return function (i) {
	                        if (i === -1) {
	                            return 0;
	                        } else if (pa[i] === qa[i]) {
	                            return check_3(pa)(qa)(i - 1);
	                        } else if (pa[i] < qa[i]) {
	                            return -1;
	                        } else {
	                            return 1;
	                        }
	                    };
	                };
	            };
	            return check_3(p.digits)(q.digits)(p.bound - 1);
	        }
	    }
	    exports.compare = compare;
	    function lt(p, q) {
	        return compare(p, q) === -1;
	    }
	    exports.lt = lt;
	    function gt(p, q) {
	        return compare(p, q) === 1;
	    }
	    exports.gt = gt;
	    function lte(p, q) {
	        return compare(p, q) !== 1;
	    }
	    exports.lte = lte;
	    function gte(p, q) {
	        return compare(p, q) !== -1;
	    }
	    exports.gte = gte;
	    function min(a, b) {
	        if (lt(a, b)) {
	            return a;
	        } else {
	            return b;
	        }
	    }
	    exports.min = min;
	    function max(a, b) {
	        if (lt(a, b)) {
	            return b;
	        } else {
	            return a;
	        }
	    }
	    exports.max = max;
	    function contributeArr(a, i, c) {
	        var x = Long_1.fromNumber(a[i], false).add(c);
	        var c_1 = x.div(exports.baseNi64);
	        var x_1 = ~~x.and(exports.baseMaski64).toNumber();
	        a[i] = x_1;
	        if (c_1.CompareTo(Long_1.fromBits(0, 0, false)) > 0) {
	            contributeArr(a, i + 1, c_1);
	        }
	    }
	    exports.contributeArr = contributeArr;
	    function scale(k, p) {
	        var rbound = p.bound + exports.boundInt;
	        var r = createN(rbound);
	        var k_1 = Long_1.fromNumber(k, false);
	        for (var i = 0; i <= p.bound - 1; i++) {
	            var kpi = k_1.mul(Long_1.fromNumber(p.digits[i], false));
	            contributeArr(r.digits, i, kpi);
	        }
	        return normN(r);
	    }
	    exports.scale = scale;
	    function mulSchoolBookBothSmall(p, q) {
	        var r = createN(2);
	        var rak = Long_1.fromNumber(p, false).mul(Long_1.fromNumber(q, false));
	        setCoeff(r, 0, ~~rak.and(exports.baseMaski64).toNumber());
	        setCoeff(r, 1, ~~rak.div(exports.baseNi64).toNumber());
	        return normN(r);
	    }
	    exports.mulSchoolBookBothSmall = mulSchoolBookBothSmall;
	    function mulSchoolBookCarry(r, c, k) {
	        if (c.CompareTo(Long_1.fromBits(0, 0, false)) > 0) {
	            var rak = coeff64(r, k).add(c);
	            setCoeff(r, k, ~~rak.and(exports.baseMaski64).toNumber());
	            mulSchoolBookCarry(r, rak.div(exports.baseNi64), k + 1);
	        }
	    }
	    exports.mulSchoolBookCarry = mulSchoolBookCarry;
	    function mulSchoolBookOneSmall(p, q) {
	        var bp = bound(p);
	        var rbound = bp + 1;
	        var r = createN(rbound);
	        var q_1 = Long_1.fromNumber(q, false);
	        var c = Long_1.fromBits(0, 0, false);
	        for (var i = 0; i <= bp - 1; i++) {
	            var rak = c.add(coeff64(r, i)).add(coeff64(p, i).mul(q_1));
	            setCoeff(r, i, ~~rak.and(exports.baseMaski64).toNumber());
	            c = rak.div(exports.baseNi64);
	        }
	        mulSchoolBookCarry(r, c, bp);
	        return normN(r);
	    }
	    exports.mulSchoolBookOneSmall = mulSchoolBookOneSmall;
	    function mulSchoolBookNeitherSmall(p, q) {
	        var rbound = p.bound + q.bound;
	        var r = createN(rbound);
	        for (var i = 0; i <= p.bound - 1; i++) {
	            var pai = Long_1.fromNumber(p.digits[i], false);
	            var c = Long_1.fromBits(0, 0, false);
	            var k = i;
	            for (var j = 0; j <= q.bound - 1; j++) {
	                var qaj = Long_1.fromNumber(q.digits[j], false);
	                var rak = Long_1.fromNumber(r.digits[k], false).add(c).add(pai.mul(qaj));
	                r.digits[k] = ~~rak.and(exports.baseMaski64).toNumber();
	                c = rak.div(exports.baseNi64);
	                k = k + 1;
	            }
	            mulSchoolBookCarry(r, c, k);
	        }
	        return normN(r);
	    }
	    exports.mulSchoolBookNeitherSmall = mulSchoolBookNeitherSmall;
	    function mulSchoolBook(p, q) {
	        var pSmall = bound(p) === 1;
	        var qSmall = bound(q) === 1;
	        if (pSmall ? qSmall : false) {
	            return mulSchoolBookBothSmall(coeff(p, 0), coeff(q, 0));
	        } else if (pSmall) {
	            return mulSchoolBookOneSmall(q, coeff(p, 0));
	        } else if (qSmall) {
	            return mulSchoolBookOneSmall(p, coeff(q, 0));
	        } else {
	            return mulSchoolBookNeitherSmall(p, q);
	        }
	    }
	    exports.mulSchoolBook = mulSchoolBook;
	    var encoding = function () {
	        function encoding(bigL, twoToBigL, k, bigK, bigN, split, splits) {
	            this.bigL = bigL;
	            this.twoToBigL = twoToBigL;
	            this.k = k;
	            this.bigK = bigK;
	            this.bigN = bigN;
	            this.split = split;
	            this.splits = splits;
	        }
	        encoding.prototype[Symbol_2.default.reflection] = function () {
	            return {
	                type: "Microsoft.FSharp.Math.BigNatModule.encoding",
	                interfaces: ["FSharpRecord"],
	                properties: {
	                    bigL: "number",
	                    twoToBigL: "number",
	                    k: "number",
	                    bigK: "number",
	                    bigN: "number",
	                    split: "number",
	                    splits: Int32Array
	                }
	            };
	        };
	        return encoding;
	    }();
	    exports.encoding = encoding;
	    Symbol_1.setType("Microsoft.FSharp.Math.BigNatModule.encoding", encoding);
	    function mkEncoding(bigL, k, bigK, bigN) {
	        return new encoding(bigL, pow32(2, bigL), k, bigK, bigN, ~~(exports.baseBits / bigL), Int32Array.from(Seq_1.initialize(~~(exports.baseBits / bigL), function (i) {
	            return pow32(2, bigL * i);
	        })));
	    }
	    exports.mkEncoding = mkEncoding;
	    exports.table = [mkEncoding(1, 28, 268435456, 268435456), mkEncoding(2, 26, 67108864, 134217728), mkEncoding(3, 24, 16777216, 50331648), mkEncoding(4, 22, 4194304, 16777216), mkEncoding(5, 20, 1048576, 5242880), mkEncoding(6, 18, 262144, 1572864), mkEncoding(7, 16, 65536, 458752), mkEncoding(8, 14, 16384, 131072), mkEncoding(9, 12, 4096, 36864), mkEncoding(10, 10, 1024, 10240), mkEncoding(11, 8, 256, 2816), mkEncoding(12, 6, 64, 768), mkEncoding(13, 4, 16, 208)];
	    function calculateTableTow(bigL) {
	        var k = FFT_1.maxBitsInsideFp - 2 * bigL;
	        var bigK = pow64(Long_1.fromBits(2, 0, false), k);
	        var N = bigK.mul(Long_1.fromNumber(bigL, false));
	        return [bigL, k, bigK, N];
	    }
	    exports.calculateTableTow = calculateTableTow;
	    function encodingGivenResultBits(bitsRes) {
	        var selectFrom = function selectFrom(i) {
	            if (i + 1 < exports.table.length ? bitsRes < exports.table[i + 1].bigN : false) {
	                return selectFrom(i + 1);
	            } else {
	                return exports.table[i];
	            }
	        };
	        if (bitsRes >= exports.table[0].bigN) {
	            throw new Error("Product is huge, around 268435456 bits, beyond quickmul");
	        } else {
	            return selectFrom(0);
	        }
	    }
	    exports.encodingGivenResultBits = encodingGivenResultBits;
	    exports.bitmask = Int32Array.from(Seq_1.initialize(exports.baseBits, function (i) {
	        return pow32(2, i) - 1;
	    }));
	    exports.twopowers = Int32Array.from(Seq_1.initialize(exports.baseBits, function (i) {
	        return pow32(2, i);
	    }));
	    exports.twopowersI64 = Array.from(Seq_1.initialize(exports.baseBits, function (i) {
	        return pow64(Long_1.fromBits(2, 0, false), i);
	    }));
	    function wordBits(word) {
	        var hi = function hi(k) {
	            if (k === 0) {
	                return 0;
	            } else if ((word & exports.twopowers[k - 1]) !== 0) {
	                return k;
	            } else {
	                return hi(k - 1);
	            }
	        };
	        return hi(exports.baseBits);
	    }
	    exports.wordBits = wordBits;
	    function bits(u) {
	        if (u.bound === 0) {
	            return 0;
	        } else {
	            return degree(u) * exports.baseBits + wordBits(u.digits[degree(u)]);
	        }
	    }
	    exports.bits = bits;
	    function extractBits(n, enc, bi) {
	        var bj = bi + enc.bigL - 1;
	        var biw = ~~(bi / exports.baseBits);
	        var bjw = ~~(bj / exports.baseBits);
	        if (biw !== bjw) {
	            var x = function () {
	                var $var10 = biw;
	                var $var9 = n;
	                if ($var10 < $var9.bound) {
	                    return $var9.digits[$var10];
	                } else {
	                    return 0;
	                }
	            }();
	            var y = function () {
	                var $var12 = bjw;
	                var $var11 = n;
	                if ($var12 < $var11.bound) {
	                    return $var11.digits[$var12];
	                } else {
	                    return 0;
	                }
	            }();
	            var xbit = bi % exports.baseBits;
	            var nxbits = exports.baseBits - xbit;
	            var x_1 = x >> xbit;
	            var y_1 = y << nxbits;
	            var x_2 = x_1 | y_1;
	            var x_3 = x_2 & exports.bitmask[enc.bigL];
	            return x_3;
	        } else {
	            var x = function () {
	                var $var14 = biw;
	                var $var13 = n;
	                if ($var14 < $var13.bound) {
	                    return $var13.digits[$var14];
	                } else {
	                    return 0;
	                }
	            }();
	            var xbit = bi % exports.baseBits;
	            var x_1 = x >> xbit;
	            var x_2 = x_1 & exports.bitmask[enc.bigL];
	            return x_2;
	        }
	    }
	    exports.extractBits = extractBits;
	    function encodePoly(enc, n) {
	        var poly = Uint32Array.from(Seq_1.replicate(enc.bigK, FFT_1.ofInt32(0)));
	        var biMax = n.bound * exports.baseBits;
	        var encoder = function encoder(i) {
	            return function (bi) {
	                if (i === enc.bigK ? true : bi > biMax) {} else {
	                    var pi = extractBits(n, enc, bi);
	                    poly[i] = FFT_1.ofInt32(pi);
	                    var i_1 = i + 1;
	                    var bi_1 = bi + enc.bigL;
	                    encoder(i_1)(bi_1);
	                }
	            };
	        };
	        encoder(0)(0);
	        return poly;
	    }
	    exports.encodePoly = encodePoly;
	    function decodeResultBits(enc, poly) {
	        var n = 0;
	        for (var i = 0; i <= poly.length - 1; i++) {
	            if (poly[i] !== FFT_1.mzero) {
	                n = i;
	            }
	        }
	        var rbits = FFT_1.maxBitsInsideFp + enc.bigL * n + 1;
	        return rbits + 1;
	    }
	    exports.decodeResultBits = decodeResultBits;
	    function decodePoly(enc, poly) {
	        var rbound = ~~(decodeResultBits(enc, poly) / exports.baseBits) + 1;
	        var r = createN(rbound);
	        var evaluate = function evaluate(i) {
	            return function (j) {
	                return function (d) {
	                    if (i === enc.bigK) {} else {
	                        if (j >= rbound) {} else {
	                            var x = Long_1.fromNumber(FFT_1.toInt(poly[i]), false).mul(exports.twopowersI64[d]);
	                            contributeArr(r.digits, j, x);
	                        }
	                        var i_1 = i + 1;
	                        var d_1 = d + enc.bigL;
	                        var patternInput = d_1 >= exports.baseBits ? [j + 1, d_1 - exports.baseBits] : [j, d_1];
	                        evaluate(i_1)(patternInput[0])(patternInput[1]);
	                    }
	                };
	            };
	        };
	        evaluate(0)(0)(0);
	        return normN(r);
	    }
	    exports.decodePoly = decodePoly;
	    function quickMulUsingFft(u, v) {
	        var bitsRes = bits(u) + bits(v);
	        var enc = encodingGivenResultBits(bitsRes);
	        var upoly = encodePoly(enc, u);
	        var vpoly = encodePoly(enc, v);
	        var rpoly = FFT_1.computeFftPaddedPolynomialProduct(enc.bigK, enc.k, upoly, vpoly);
	        var r = decodePoly(enc, rpoly);
	        return normN(r);
	    }
	    exports.quickMulUsingFft = quickMulUsingFft;
	    exports.minDigitsKaratsuba = 16;
	    function recMulKaratsuba(mul, p, q) {
	        var bp = p.bound;
	        var bq = q.bound;
	        var bmax = maxInt(bp, bq);
	        if (bmax > exports.minDigitsKaratsuba) {
	            var k = ~~(bmax / 2);
	            var a0 = restrictTo(k, p);
	            var a1 = shiftDown(k, p);
	            var b0 = restrictTo(k, q);
	            var b1 = shiftDown(k, q);
	            var q0 = mul(a0)(b0);
	            var q1 = mul(add(a0, a1))(add(b0, b1));
	            var q2 = mul(a1)(b1);
	            var p1 = sub(q1, add(q0, q2));
	            var r = add(q0, shiftUp(k, add(p1, shiftUp(k, q2))));
	            return r;
	        } else {
	            return mulSchoolBook(p, q);
	        }
	    }
	    exports.recMulKaratsuba = recMulKaratsuba;
	    function mulKaratsuba(x, y) {
	        return recMulKaratsuba(function (x_1) {
	            return function (y_1) {
	                return mulKaratsuba(x_1, y_1);
	            };
	        }, x, y);
	    }
	    exports.mulKaratsuba = mulKaratsuba;
	    exports.productDigitsUpperSchoolBook = ~~(64000 / exports.baseBits);
	    exports.singleDigitForceSchoolBook = ~~(32000 / exports.baseBits);
	    exports.productDigitsUpperFft = ~~(exports.table[0].bigN / exports.baseBits);
	    function mul(p, q) {
	        return mulSchoolBook(p, q);
	    }
	    exports.mul = mul;
	    function scaleSubInPlace(x, f, a, n) {
	        var invariant = function invariant(tupledArg) {};
	        var patternInput = [x.digits, degree(x)];
	        var patternInput_1 = [a.digits, degree(a)];
	        var f_1 = Long_1.fromNumber(f, false);
	        var j = 0;
	        var z = f_1.mul(Long_1.fromNumber(patternInput_1[0][0], false));
	        while (z.CompareTo(Long_1.fromBits(0, 0, false)) > 0 ? true : j < patternInput_1[1]) {
	            if (j > patternInput[1]) {
	                throw new Error("scaleSubInPlace: pre-condition did not apply, result would be -ve");
	            }
	            invariant([z, j, n]);
	            var zLo = ~~z.and(exports.baseMaski64).toNumber();
	            var zHi = z.div(exports.baseNi64);
	            if (zLo <= patternInput[0][j + n]) {
	                patternInput[0][j + n] = patternInput[0][j + n] - zLo;
	            } else {
	                patternInput[0][j + n] = patternInput[0][j + n] + (exports.baseN - zLo);
	                zHi = zHi.add(Long_1.fromBits(1, 0, false));
	            }
	            if (j < patternInput_1[1]) {
	                z = zHi.add(f_1.mul(Long_1.fromNumber(patternInput_1[0][j + 1], false)));
	            } else {
	                z = zHi;
	            }
	            j = j + 1;
	        }
	        normN(x);
	    }
	    exports.scaleSubInPlace = scaleSubInPlace;
	    function scaleSub(x, f, a, n) {
	        var freshx = add(x, exports.zero);
	        scaleSubInPlace(freshx, f, a, n);
	        return normN(freshx);
	    }
	    exports.scaleSub = scaleSub;
	    function scaleAddInPlace(x, f, a, n) {
	        var invariant = function invariant(tupledArg) {};
	        var patternInput = [x.digits, degree(x)];
	        var patternInput_1 = [a.digits, degree(a)];
	        var f_1 = Long_1.fromNumber(f, false);
	        var j = 0;
	        var z = f_1.mul(Long_1.fromNumber(patternInput_1[0][0], false));
	        while (z.CompareTo(Long_1.fromBits(0, 0, false)) > 0 ? true : j < patternInput_1[1]) {
	            if (j > patternInput[1]) {
	                throw new Error("scaleSubInPlace: pre-condition did not apply, result would be -ve");
	            }
	            invariant([z, j, n]);
	            var zLo = ~~z.and(exports.baseMaski64).toNumber();
	            var zHi = z.div(exports.baseNi64);
	            if (zLo < exports.baseN - patternInput[0][j + n]) {
	                patternInput[0][j + n] = patternInput[0][j + n] + zLo;
	            } else {
	                patternInput[0][j + n] = zLo - (exports.baseN - patternInput[0][j + n]);
	                zHi = zHi.add(Long_1.fromBits(1, 0, false));
	            }
	            if (j < patternInput_1[1]) {
	                z = zHi.add(f_1.mul(Long_1.fromNumber(patternInput_1[0][j + 1], false)));
	            } else {
	                z = zHi;
	            }
	            j = j + 1;
	        }
	        normN(x);
	    }
	    exports.scaleAddInPlace = scaleAddInPlace;
	    function scaleAdd(x, f, a, n) {
	        var freshx = add(x, exports.zero);
	        scaleAddInPlace(freshx, f, a, n);
	        return normN(freshx);
	    }
	    exports.scaleAdd = scaleAdd;
	    function removeFactor(x, a, n) {
	        var patternInput = [degree(a), degree(x)];
	        if (patternInput[1] < patternInput[0] + n) {
	            return 0;
	        } else {
	            var patternInput_1 = [a.digits, x.digits];
	            var f = patternInput[0] === 0 ? patternInput[1] === n ? ~~(patternInput_1[1][n] / patternInput_1[0][0]) : Long_1.fromNumber(patternInput_1[1][patternInput[1]], false).mul(exports.baseNi64).add(Long_1.fromNumber(patternInput_1[1][patternInput[1] - 1], false)).div(Long_1.fromNumber(patternInput_1[0][0], false)).toNumber() : patternInput[1] === patternInput[0] + n ? ~~(patternInput_1[1][patternInput[1]] / (patternInput_1[0][patternInput[0]] + 1)) : Long_1.fromNumber(patternInput_1[1][patternInput[1]], false).mul(exports.baseNi64).add(Long_1.fromNumber(patternInput_1[1][patternInput[1] - 1], false)).div(Long_1.fromNumber(patternInput_1[0][patternInput[0]], false).add(Long_1.fromBits(1, 0, false))).toNumber();
	            if (f === 0) {
	                var lte_1 = shiftCompare(a, n, x, 0) !== 1;
	                if (lte_1) {
	                    return 1;
	                } else {
	                    return 0;
	                }
	            } else {
	                return f;
	            }
	        }
	    }
	    exports.removeFactor = removeFactor;
	    function divmod(b, a) {
	        if (isZero(a)) {
	            throw new Error();
	        } else if (degree(b) < degree(a)) {
	            return [exports.zero, b];
	        } else {
	            var x = copyN(b);
	            var d = createN(degree(b) - degree(a) + 1 + 1);
	            var p = degree(b);
	            var m = degree(a);
	            var n = p - m;
	            var Invariant = function Invariant(tupledArg) {};
	            var finished = false;
	            while (!finished) {
	                Invariant([d, x, n, p]);
	                var f = removeFactor(x, a, n);
	                if (f > 0) {
	                    scaleSubInPlace(x, f, a, n);
	                    scaleAddInPlace(d, f, exports.one, n);
	                    Invariant([d, x, n, p]);
	                } else {
	                    if (f === 0) {
	                        finished = n === 0;
	                    } else {
	                        finished = false;
	                    }
	                    if (!finished) {
	                        if (p === m + n) {
	                            Invariant([d, x, n - 1, p]);
	                            n = n - 1;
	                        } else {
	                            Invariant([d, x, n - 1, p - 1]);
	                            n = n - 1;
	                            p = p - 1;
	                        }
	                    }
	                }
	            }
	            return [normN(d), normN(x)];
	        }
	    }
	    exports.divmod = divmod;
	    function div(b, a) {
	        return divmod(b, a)[0];
	    }
	    exports.div = div;
	    function rem(b, a) {
	        return divmod(b, a)[1];
	    }
	    exports.rem = rem;
	    function bitAnd(a, b) {
	        var rbound = minInt(a.bound, b.bound);
	        var r = createN(rbound);
	        for (var i = 0; i <= r.bound - 1; i++) {
	            r.digits[i] = a.digits[i] & b.digits[i];
	        }
	        return normN(r);
	    }
	    exports.bitAnd = bitAnd;
	    function bitOr(a, b) {
	        var rbound = maxInt(a.bound, b.bound);
	        var r = createN(rbound);
	        for (var i = 0; i <= a.bound - 1; i++) {
	            r.digits[i] = r.digits[i] | a.digits[i];
	        }
	        for (var i = 0; i <= b.bound - 1; i++) {
	            r.digits[i] = r.digits[i] | b.digits[i];
	        }
	        return normN(r);
	    }
	    exports.bitOr = bitOr;
	    function hcf(a, b) {
	        var hcfloop = function hcfloop(a_1) {
	            return function (b_1) {
	                if (equal(exports.zero, a_1)) {
	                    return b_1;
	                } else {
	                    var patternInput = divmod(b_1, a_1);
	                    return hcfloop(patternInput[1])(a_1);
	                }
	            };
	        };
	        if (lt(a, b)) {
	            return hcfloop(a)(b);
	        } else {
	            return hcfloop(b)(a);
	        }
	    }
	    exports.hcf = hcf;
	    exports.two = embed(2);
	    function powi(x, n) {
	        var power = function power(acc) {
	            return function (x_1) {
	                return function (n_1) {
	                    if (n_1 === 0) {
	                        return acc;
	                    } else if (n_1 % 2 === 0) {
	                        return power(acc)(mul(x_1, x_1))(~~(n_1 / 2));
	                    } else {
	                        return power(mul(x_1, acc))(mul(x_1, x_1))(~~(n_1 / 2));
	                    }
	                };
	            };
	        };
	        return power(exports.one)(x)(n);
	    }
	    exports.powi = powi;
	    function pow(x, n) {
	        var power = function power(acc) {
	            return function (x_1) {
	                return function (n_1) {
	                    if (isZero(n_1)) {
	                        return acc;
	                    } else {
	                        var patternInput = divmod(n_1, exports.two);
	                        if (isZero(patternInput[1])) {
	                            return power(acc)(mul(x_1, x_1))(patternInput[0]);
	                        } else {
	                            return power(mul(x_1, acc))(mul(x_1, x_1))(patternInput[0]);
	                        }
	                    }
	                };
	            };
	        };
	        return power(exports.one)(x)(n);
	    }
	    exports.pow = pow;
	    function toFloat(n) {
	        var basef = exports.baseN;
	        var evalFloat = function evalFloat(acc) {
	            return function (k) {
	                return function (i) {
	                    if (i === n.bound) {
	                        return acc;
	                    } else {
	                        return evalFloat(acc + k * n.digits[i])(k * basef)(i + 1);
	                    }
	                };
	            };
	        };
	        return evalFloat(0)(1)(0);
	    }
	    exports.toFloat = toFloat;
	    function ofInt32(n) {
	        return embed(n);
	    }
	    exports.ofInt32 = ofInt32;
	    function ofInt64(n) {
	        return embed64(n);
	    }
	    exports.ofInt64 = ofInt64;
	    function toUInt32(n) {
	        var matchValue = n.bound;
	        var $var15 = null;
	        switch (matchValue) {
	            case 0:
	                {
	                    $var15 = 0;
	                    break;
	                }
	            case 1:
	                {
	                    $var15 = n.digits[0] >>> 0;
	                    break;
	                }
	            case 2:
	                {
	                    {
	                        var patternInput = [n.digits[0], n.digits[1]];
	                        if (patternInput[1] > exports.baseMask32B) {
	                            throw new Error();
	                        }
	                        $var15 = ((patternInput[0] & exports.baseMask32A) >>> 0) + ((patternInput[1] & exports.baseMask32B) >>> 0 << exports.baseShift32B);
	                    }
	                    break;
	                }
	            default:
	                {
	                    throw new Error();
	                }
	        }
	        return $var15;
	    }
	    exports.toUInt32 = toUInt32;
	    function toUInt64(n) {
	        var matchValue = n.bound;
	        var $var16 = null;
	        switch (matchValue) {
	            case 0:
	                {
	                    $var16 = Long_1.fromBits(0, 0, true);
	                    break;
	                }
	            case 1:
	                {
	                    $var16 = Long_1.fromNumber(n.digits[0], true);
	                    break;
	                }
	            case 2:
	                {
	                    {
	                        var patternInput = [n.digits[0], n.digits[1]];
	                        $var16 = Long_1.fromNumber(patternInput[0] & exports.baseMask64A, true).add(Long_1.fromNumber(patternInput[1] & exports.baseMask64B, true).shl(exports.baseShift64B));
	                    }
	                    break;
	                }
	            case 3:
	                {
	                    {
	                        var patternInput = [n.digits[0], n.digits[1], n.digits[2]];
	                        if (patternInput[2] > exports.baseMask64C) {
	                            throw new Error();
	                        }
	                        $var16 = Long_1.fromNumber(patternInput[0] & exports.baseMask64A, true).add(Long_1.fromNumber(patternInput[1] & exports.baseMask64B, true).shl(exports.baseShift64B)).add(Long_1.fromNumber(patternInput[2] & exports.baseMask64C, true).shl(exports.baseShift64C));
	                    }
	                    break;
	                }
	            default:
	                {
	                    throw new Error();
	                }
	        }
	        return $var16;
	    }
	    exports.toUInt64 = toUInt64;
	    function toString(n) {
	        var degn = degree(n);
	        var route = function route(prior) {
	            return function (k) {
	                return function (ten2k) {
	                    if (degree(ten2k) > degn) {
	                        return new List_1.default([k, ten2k], prior);
	                    } else {
	                        return route(new List_1.default([k, ten2k], prior))(k + 1)(mul(ten2k, ten2k));
	                    }
	                };
	            };
	        };
	        var kten2ks = route(new List_1.default())(0)(embed(10));
	        var collect = function collect(isLeading) {
	            return function (digits) {
	                return function (n_1) {
	                    return function (_arg1) {
	                        if (_arg1.tail != null) {
	                            var ten2k = _arg1.head[1];
	                            var patternInput = divmod(n_1, ten2k);
	                            if (isLeading ? isZero(patternInput[0]) : false) {
	                                var digits_1 = collect(isLeading)(digits)(patternInput[1])(_arg1.tail);
	                                return digits_1;
	                            } else {
	                                var digits_1 = collect(false)(digits)(patternInput[1])(_arg1.tail);
	                                var digits_2 = collect(isLeading)(digits_1)(patternInput[0])(_arg1.tail);
	                                return digits_2;
	                            }
	                        } else {
	                            var n_2 = eval32(n_1);
	                            if (isLeading ? n_2 === 0 : false) {
	                                return digits;
	                            } else {
	                                return new List_1.default(String(n_2), digits);
	                            }
	                        }
	                    };
	                };
	            };
	        };
	        var digits = collect(true)(new List_1.default())(n)(kten2ks);
	        if (digits.tail == null) {
	            return "0";
	        } else {
	            return String_1.join.apply(void 0, [""].concat(Array.from(digits)));
	        }
	    }
	    exports.toString = toString;
	    function ofString(str) {
	        var len = str.length;
	        if (String_1.isNullOrEmpty(str)) {
	            throw new Error("empty string" + '\nParameter name: ' + "str");
	        }
	        var ten = embed(10);
	        var build = function build(acc) {
	            return function (i) {
	                if (i === len) {
	                    return acc;
	                } else {
	                    var c = str[i];
	                    var d = c.charCodeAt(0) - "0".charCodeAt(0);
	                    if (0 <= d ? d <= 9 : false) {
	                        return build(add(mul(ten, acc), embed(d)))(i + 1);
	                    } else {
	                        throw new Error();
	                    }
	                }
	            };
	        };
	        return build(embed(0))(0);
	    }
	    exports.ofString = ofString;
	    function isSmall(n) {
	        return n.bound <= 1;
	    }
	    exports.isSmall = isSmall;
	    function getSmall(n) {
	        var $var18 = 0;
	        var $var17 = n;
	        if ($var18 < $var17.bound) {
	            return $var17.digits[$var18];
	        } else {
	            return 0;
	        }
	    }
	    exports.getSmall = getSmall;
	    function factorial(n) {
	        var productR = function productR(a) {
	            return function (b) {
	                if (equal(a, b)) {
	                    return a;
	                } else {
	                    var m = div(add(a, b), ofInt32(2));
	                    return mul(productR(a)(m), productR(add(m, exports.one))(b));
	                }
	            };
	        };
	        return productR(exports.one)(n);
	    }
	    exports.factorial = factorial;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 13 */
/***/ function(module, exports, __webpack_require__) {

	var map = {
		"./BigNat": 12,
		"./BigNat.js": 12,
		"./FFT": 14,
		"./FFT.js": 14
	};
	function webpackContext(req) {
		return __webpack_require__(webpackContextResolve(req));
	};
	function webpackContextResolve(req) {
		return map[req] || (function() { throw new Error("Cannot find module '" + req + "'.") }());
	};
	webpackContext.keys = function webpackContextKeys() {
		return Object.keys(map);
	};
	webpackContext.resolve = webpackContextResolve;
	module.exports = webpackContext;
	webpackContext.id = 13;


/***/ },
/* 14 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(13), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "../Long", "../Seq"], function (require, exports) {
	    "use strict";
	
	    var Long_1 = require("../Long");
	    var Seq_1 = require("../Seq");
	    function pow32(x, n) {
	        if (n === 0) {
	            return 1;
	        } else if (n % 2 === 0) {
	            return pow32(x * x, ~~(n / 2));
	        } else {
	            return x * pow32(x * x, ~~(n / 2));
	        }
	    }
	    exports.pow32 = pow32;
	    function leastBounding2Power(b) {
	        var findBounding2Power = function findBounding2Power(b_1) {
	            return function (tp) {
	                return function (i) {
	                    if (b_1 <= tp) {
	                        return [tp, i];
	                    } else {
	                        return findBounding2Power(b_1)(tp * 2)(i + 1);
	                    }
	                };
	            };
	        };
	        return findBounding2Power(b)(1)(0);
	    }
	    exports.leastBounding2Power = leastBounding2Power;
	    exports.k = 27;
	    exports.m = 15;
	    exports.g = 31;
	    exports.w = 440564289;
	    exports.primeP = Long_1.fromBits(2013265921, 0, false);
	    exports.maxBitsInsideFp = 30;
	    exports.p = 2013265921;
	    exports.p64 = Long_1.fromBits(2013265921, 0, true);
	    function toInt(x) {
	        return ~~x;
	    }
	    exports.toInt = toInt;
	    function ofInt32(x) {
	        return x >>> 0;
	    }
	    exports.ofInt32 = ofInt32;
	    exports.mzero = 0;
	    exports.mone = 1;
	    exports.mtwo = 2;
	    function mpow(x, n) {
	        if (n === 0) {
	            return exports.mone;
	        } else if (n % 2 === 0) {
	            return mpow(Long_1.fromNumber(x, true).mul(Long_1.fromNumber(x, true)).mod(exports.p64).toNumber() >>> 0, ~~(n / 2));
	        } else {
	            return Long_1.fromNumber(x, true).mul(Long_1.fromNumber(mpow(Long_1.fromNumber(x, true).mul(Long_1.fromNumber(x, true)).mod(exports.p64).toNumber() >>> 0, ~~(n / 2)), true)).mod(exports.p64).toNumber() >>> 0;
	        }
	    }
	    exports.mpow = mpow;
	    function mpowL(x, n) {
	        if (n.Equals(Long_1.fromBits(0, 0, false))) {
	            return exports.mone;
	        } else if (n.mod(Long_1.fromBits(2, 0, false)).Equals(Long_1.fromBits(0, 0, false))) {
	            return mpowL(Long_1.fromNumber(x, true).mul(Long_1.fromNumber(x, true)).mod(exports.p64).toNumber() >>> 0, n.div(Long_1.fromBits(2, 0, false)));
	        } else {
	            return Long_1.fromNumber(x, true).mul(Long_1.fromNumber(mpowL(Long_1.fromNumber(x, true).mul(Long_1.fromNumber(x, true)).mod(exports.p64).toNumber() >>> 0, n.div(Long_1.fromBits(2, 0, false))), true)).mod(exports.p64).toNumber() >>> 0;
	        }
	    }
	    exports.mpowL = mpowL;
	    function m2PowNthRoot(n) {
	        return mpow(exports.w >>> 0, pow32(2, exports.k - n));
	    }
	    exports.m2PowNthRoot = m2PowNthRoot;
	    function minv(x) {
	        return mpowL(x, exports.primeP.sub(Long_1.fromBits(2, 0, false)));
	    }
	    exports.minv = minv;
	    function computeFFT(lambda, mu, n, w_1, u, res, offset) {
	        if (n === 1) {
	            res[offset] = u[mu];
	        } else {
	            var halfN = ~~(n / 2);
	            var ww = Long_1.fromNumber(w_1, true).mul(Long_1.fromNumber(w_1, true)).mod(exports.p64).toNumber() >>> 0;
	            var offsetHalfN = offset + halfN;
	            computeFFT(lambda * 2, mu, halfN, ww, u, res, offset);
	            computeFFT(lambda * 2, lambda + mu, halfN, ww, u, res, offsetHalfN);
	            var wj = exports.mone;
	            for (var j = 0; j <= halfN - 1; j++) {
	                var even = res[offset + j];
	                var odd = res[offsetHalfN + j];
	                res[offset + j] = (even + (Long_1.fromNumber(wj, true).mul(Long_1.fromNumber(odd, true)).mod(exports.p64).toNumber() >>> 0)) % exports.p;
	                res[offsetHalfN + j] = (even + exports.p - (Long_1.fromNumber(wj, true).mul(Long_1.fromNumber(odd, true)).mod(exports.p64).toNumber() >>> 0)) % exports.p;
	                wj = Long_1.fromNumber(w_1, true).mul(Long_1.fromNumber(wj, true)).mod(exports.p64).toNumber() >>> 0;
	            }
	        }
	    }
	    exports.computeFFT = computeFFT;
	    function computFftInPlace(n, w_1, u) {
	        var lambda = 1;
	        var mu = 0;
	        var res = Uint32Array.from(Seq_1.replicate(n, exports.mzero));
	        var offset = 0;
	        computeFFT(lambda, mu, n, w_1, u, res, offset);
	        return res;
	    }
	    exports.computFftInPlace = computFftInPlace;
	    function computeInverseFftInPlace(n, w_1, uT) {
	        var bigKInv = minv(n >>> 0);
	        return computFftInPlace(n, minv(w_1), uT).map(function (y) {
	            return Long_1.fromNumber(bigKInv, true).mul(Long_1.fromNumber(y, true)).mod(exports.p64).toNumber() >>> 0;
	        });
	    }
	    exports.computeInverseFftInPlace = computeInverseFftInPlace;
	    exports.maxTwoPower = 29;
	    exports.twoPowerTable = Int32Array.from(Seq_1.initialize(exports.maxTwoPower - 1, function (i) {
	        return pow32(2, i);
	    }));
	    function computeFftPaddedPolynomialProduct(bigK, k_1, u, v) {
	        var w_1 = m2PowNthRoot(k_1);
	        var uT = computFftInPlace(bigK, w_1, u);
	        var vT = computFftInPlace(bigK, w_1, v);
	        var rT = Uint32Array.from(Seq_1.initialize(bigK, function (i) {
	            return Long_1.fromNumber(uT[i], true).mul(Long_1.fromNumber(vT[i], true)).mod(exports.p64).toNumber() >>> 0;
	        }));
	        var r = computeInverseFftInPlace(bigK, w_1, rT);
	        return r;
	    }
	    exports.computeFftPaddedPolynomialProduct = computeFftPaddedPolynomialProduct;
	    function padTo(n, u) {
	        var uBound = u.length;
	        return Uint32Array.from(Seq_1.initialize(n, function (i) {
	            if (i < uBound) {
	                return ofInt32(u[i]);
	            } else {
	                return exports.mzero;
	            }
	        }));
	    }
	    exports.padTo = padTo;
	    function computeFftPolynomialProduct(degu, u, degv, v) {
	        var deguv = degu + degv;
	        var bound = deguv + 1;
	        var patternInput = leastBounding2Power(bound);
	        var w_1 = m2PowNthRoot(patternInput[1]);
	        var u_1 = padTo(patternInput[0], u);
	        var v_1 = padTo(patternInput[0], v);
	        var uT = computFftInPlace(patternInput[0], w_1, u_1);
	        var vT = computFftInPlace(patternInput[0], w_1, v_1);
	        var rT = Uint32Array.from(Seq_1.initialize(patternInput[0], function (i) {
	            return Long_1.fromNumber(uT[i], true).mul(Long_1.fromNumber(vT[i], true)).mod(exports.p64).toNumber() >>> 0;
	        }));
	        var r = computeInverseFftInPlace(patternInput[0], w_1, rT);
	        return Int32Array.from(Seq_1.map(function (x) {
	            return toInt(x);
	        }, r));
	    }
	    exports.computeFftPolynomialProduct = computeFftPolynomialProduct;
	    exports.maxFp = (exports.p + exports.p - exports.mone) % exports.p;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 15 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Long"], function (require, exports) {
	    "use strict";
	
	    var Long = require("./Long");
	    var littleEndian = true;
	    function isLittleEndian() {
	        return littleEndian;
	    }
	    exports.isLittleEndian = isLittleEndian;
	    function getBytesBoolean(value) {
	        var bytes = new Uint8Array(1);
	        new DataView(bytes.buffer).setUint8(0, value ? 1 : 0);
	        return bytes;
	    }
	    exports.getBytesBoolean = getBytesBoolean;
	    function getBytesChar(value) {
	        var bytes = new Uint8Array(2);
	        new DataView(bytes.buffer).setUint16(0, value.charCodeAt(0), littleEndian);
	        return bytes;
	    }
	    exports.getBytesChar = getBytesChar;
	    function getBytesInt16(value) {
	        var bytes = new Uint8Array(2);
	        new DataView(bytes.buffer).setInt16(0, value, littleEndian);
	        return bytes;
	    }
	    exports.getBytesInt16 = getBytesInt16;
	    function getBytesInt32(value) {
	        var bytes = new Uint8Array(4);
	        new DataView(bytes.buffer).setInt32(0, value, littleEndian);
	        return bytes;
	    }
	    exports.getBytesInt32 = getBytesInt32;
	    function getBytesInt64(value) {
	        var bytes = new Uint8Array(8);
	        new DataView(bytes.buffer).setInt32(littleEndian ? 0 : 4, value.getLowBits(), littleEndian);
	        new DataView(bytes.buffer).setInt32(littleEndian ? 4 : 0, value.getHighBits(), littleEndian);
	        return bytes;
	    }
	    exports.getBytesInt64 = getBytesInt64;
	    function getBytesUInt16(value) {
	        var bytes = new Uint8Array(2);
	        new DataView(bytes.buffer).setUint16(0, value, littleEndian);
	        return bytes;
	    }
	    exports.getBytesUInt16 = getBytesUInt16;
	    function getBytesUInt32(value) {
	        var bytes = new Uint8Array(4);
	        new DataView(bytes.buffer).setUint32(0, value, littleEndian);
	        return bytes;
	    }
	    exports.getBytesUInt32 = getBytesUInt32;
	    function getBytesUInt64(value) {
	        var bytes = new Uint8Array(8);
	        new DataView(bytes.buffer).setUint32(littleEndian ? 0 : 4, value.getLowBitsUnsigned(), littleEndian);
	        new DataView(bytes.buffer).setUint32(littleEndian ? 4 : 0, value.getHighBitsUnsigned(), littleEndian);
	        return bytes;
	    }
	    exports.getBytesUInt64 = getBytesUInt64;
	    function getBytesSingle(value) {
	        var bytes = new Uint8Array(4);
	        new DataView(bytes.buffer).setFloat32(0, value, littleEndian);
	        return bytes;
	    }
	    exports.getBytesSingle = getBytesSingle;
	    function getBytesDouble(value) {
	        var bytes = new Uint8Array(8);
	        new DataView(bytes.buffer).setFloat64(0, value, littleEndian);
	        return bytes;
	    }
	    exports.getBytesDouble = getBytesDouble;
	    function int64BitsToDouble(value) {
	        var buffer = new ArrayBuffer(8);
	        new DataView(buffer).setInt32(littleEndian ? 0 : 4, value.getLowBits(), littleEndian);
	        new DataView(buffer).setInt32(littleEndian ? 4 : 0, value.getHighBits(), littleEndian);
	        return new DataView(buffer).getFloat64(0, littleEndian);
	    }
	    exports.int64BitsToDouble = int64BitsToDouble;
	    function doubleToInt64Bits(value) {
	        var buffer = new ArrayBuffer(8);
	        new DataView(buffer).setFloat64(0, value, littleEndian);
	        var lowBits = new DataView(buffer).getInt32(littleEndian ? 0 : 4, littleEndian);
	        var highBits = new DataView(buffer).getInt32(littleEndian ? 4 : 0, littleEndian);
	        return Long.fromBits(lowBits, highBits, false);
	    }
	    exports.doubleToInt64Bits = doubleToInt64Bits;
	    function toBoolean(bytes, offset) {
	        return new DataView(bytes.buffer).getUint8(offset) === 1 ? true : false;
	    }
	    exports.toBoolean = toBoolean;
	    function toChar(bytes, offset) {
	        var code = new DataView(bytes.buffer).getUint16(offset, littleEndian);
	        return String.fromCharCode(code);
	    }
	    exports.toChar = toChar;
	    function toInt16(bytes, offset) {
	        return new DataView(bytes.buffer).getInt16(offset, littleEndian);
	    }
	    exports.toInt16 = toInt16;
	    function toInt32(bytes, offset) {
	        return new DataView(bytes.buffer).getInt32(offset, littleEndian);
	    }
	    exports.toInt32 = toInt32;
	    function toInt64(bytes, offset) {
	        var lowBits = new DataView(bytes.buffer).getInt32(offset + (littleEndian ? 0 : 4), littleEndian);
	        var highBits = new DataView(bytes.buffer).getInt32(offset + (littleEndian ? 4 : 0), littleEndian);
	        return Long.fromBits(lowBits, highBits, false);
	    }
	    exports.toInt64 = toInt64;
	    function toUInt16(bytes, offset) {
	        return new DataView(bytes.buffer).getUint16(offset, littleEndian);
	    }
	    exports.toUInt16 = toUInt16;
	    function toUInt32(bytes, offset) {
	        return new DataView(bytes.buffer).getUint32(offset, littleEndian);
	    }
	    exports.toUInt32 = toUInt32;
	    function toUInt64(bytes, offset) {
	        var lowBits = new DataView(bytes.buffer).getUint32(offset + (littleEndian ? 0 : 4), littleEndian);
	        var highBits = new DataView(bytes.buffer).getUint32(offset + (littleEndian ? 4 : 0), littleEndian);
	        return Long.fromBits(lowBits, highBits, true);
	    }
	    exports.toUInt64 = toUInt64;
	    function toSingle(bytes, offset) {
	        return new DataView(bytes.buffer).getFloat32(offset, littleEndian);
	    }
	    exports.toSingle = toSingle;
	    function toDouble(bytes, offset) {
	        return new DataView(bytes.buffer).getFloat64(offset, littleEndian);
	    }
	    exports.toDouble = toDouble;
	    function toString(bytes, offset, count) {
	        var ar = bytes;
	        if (typeof offset !== "undefined" && typeof count !== "undefined") ar = bytes.subarray(offset, offset + count);else if (typeof offset !== "undefined") ar = bytes.subarray(offset);
	        return Array.from(ar).map(function (b) {
	            return ("0" + b.toString(16)).slice(-2);
	        }).join("-");
	    }
	    exports.toString = toString;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 16 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Symbol", "./Util", "./Util"], function (require, exports) {
	    "use strict";
	
	    var Symbol_1 = require("./Symbol");
	    var Util_1 = require("./Util");
	    var Util_2 = require("./Util");
	    function choice1Of2(v) {
	        return new Choice("Choice1Of2", [v]);
	    }
	    exports.choice1Of2 = choice1Of2;
	    function choice2Of2(v) {
	        return new Choice("Choice2Of2", [v]);
	    }
	    exports.choice2Of2 = choice2Of2;
	    var Choice = function () {
	        function Choice(t, d) {
	            this.Case = t;
	            this.Fields = d;
	        }
	        Object.defineProperty(Choice.prototype, "valueIfChoice1", {
	            get: function get() {
	                return this.Case === "Choice1Of2" ? this.Fields[0] : null;
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(Choice.prototype, "valueIfChoice2", {
	            get: function get() {
	                return this.Case === "Choice2Of2" ? this.Fields[0] : null;
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Choice.prototype.Equals = function (other) {
	            return Util_1.equalsUnions(this, other);
	        };
	        Choice.prototype.CompareTo = function (other) {
	            return Util_2.compareUnions(this, other);
	        };
	        Choice.prototype[Symbol_1.default.reflection] = function () {
	            return {
	                type: "Microsoft.FSharp.Core.FSharpChoice",
	                interfaces: ["FSharpUnion", "System.IEquatable", "System.IComparable"]
	            };
	        };
	        return Choice;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = Choice;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 17 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./TimeSpan", "./Util", "./Long"], function (require, exports) {
	    "use strict";
	
	    var TimeSpan_1 = require("./TimeSpan");
	    var Util_1 = require("./Util");
	    var Long = require("./Long");
	    function minValue() {
	        return parse(-8640000000000000, 1);
	    }
	    exports.minValue = minValue;
	    function maxValue() {
	        return parse(8640000000000000, 1);
	    }
	    exports.maxValue = maxValue;
	    function parse(v, kind) {
	        if (kind == null) {
	            kind = typeof v == "string" && v.slice(-1) == "Z" ? 1 : 2;
	        }
	        var date = v == null ? new Date() : new Date(v);
	        if (kind === 2) {
	            date.kind = kind;
	        }
	        if (isNaN(date.getTime())) {
	            throw new Error("The string is not a valid Date.");
	        }
	        return date;
	    }
	    exports.parse = parse;
	    function tryParse(v) {
	        try {
	            return [true, parse(v)];
	        } catch (_err) {
	            return [false, minValue()];
	        }
	    }
	    exports.tryParse = tryParse;
	    function create(year, month, day, h, m, s, ms, kind) {
	        if (h === void 0) {
	            h = 0;
	        }
	        if (m === void 0) {
	            m = 0;
	        }
	        if (s === void 0) {
	            s = 0;
	        }
	        if (ms === void 0) {
	            ms = 0;
	        }
	        if (kind === void 0) {
	            kind = 2;
	        }
	        var date;
	        if (kind === 2) {
	            date = new Date(year, month - 1, day, h, m, s, ms);
	            date.kind = kind;
	        } else {
	            date = new Date(Date.UTC(year, month - 1, day, h, m, s, ms));
	        }
	        if (isNaN(date.getTime())) {
	            throw new Error("The parameters describe an unrepresentable Date.");
	        }
	        return date;
	    }
	    exports.create = create;
	    function now() {
	        return parse();
	    }
	    exports.now = now;
	    function utcNow() {
	        return parse(null, 1);
	    }
	    exports.utcNow = utcNow;
	    function today() {
	        return date(now());
	    }
	    exports.today = today;
	    function isLeapYear(year) {
	        return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
	    }
	    exports.isLeapYear = isLeapYear;
	    function daysInMonth(year, month) {
	        return month == 2 ? isLeapYear(year) ? 29 : 28 : month >= 8 ? month % 2 == 0 ? 31 : 30 : month % 2 == 0 ? 30 : 31;
	    }
	    exports.daysInMonth = daysInMonth;
	    function toUniversalTime(d) {
	        return d.kind === 2 ? new Date(d.getTime()) : d;
	    }
	    exports.toUniversalTime = toUniversalTime;
	    function toLocalTime(d) {
	        if (d.kind === 2) {
	            return d;
	        } else {
	            var d2 = new Date(d.getTime());
	            d2.kind = 2;
	            return d2;
	        }
	    }
	    exports.toLocalTime = toLocalTime;
	    function timeOfDay(d) {
	        return TimeSpan_1.create(0, hour(d), minute(d), second(d), millisecond(d));
	    }
	    exports.timeOfDay = timeOfDay;
	    function date(d) {
	        return create(year(d), month(d), day(d), 0, 0, 0, 0, d.kind || 1);
	    }
	    exports.date = date;
	    function kind(d) {
	        return d.kind || 1;
	    }
	    exports.kind = kind;
	    function day(d) {
	        return d.kind === 2 ? d.getDate() : d.getUTCDate();
	    }
	    exports.day = day;
	    function hour(d) {
	        return d.kind === 2 ? d.getHours() : d.getUTCHours();
	    }
	    exports.hour = hour;
	    function millisecond(d) {
	        return d.kind === 2 ? d.getMilliseconds() : d.getUTCMilliseconds();
	    }
	    exports.millisecond = millisecond;
	    function minute(d) {
	        return d.kind === 2 ? d.getMinutes() : d.getUTCMinutes();
	    }
	    exports.minute = minute;
	    function month(d) {
	        return (d.kind === 2 ? d.getMonth() : d.getUTCMonth()) + 1;
	    }
	    exports.month = month;
	    function second(d) {
	        return d.kind === 2 ? d.getSeconds() : d.getUTCSeconds();
	    }
	    exports.second = second;
	    function year(d) {
	        return d.kind === 2 ? d.getFullYear() : d.getUTCFullYear();
	    }
	    exports.year = year;
	    function dayOfWeek(d) {
	        return d.kind === 2 ? d.getDay() : d.getUTCDay();
	    }
	    exports.dayOfWeek = dayOfWeek;
	    function ticks(d) {
	        return Long.fromNumber(d.getTime()).add(62135596800000).sub(d.kind == 2 ? d.getTimezoneOffset() * 60 * 1000 : 0).mul(10000);
	    }
	    exports.ticks = ticks;
	    function toBinary(d) {
	        return ticks(d);
	    }
	    exports.toBinary = toBinary;
	    function dayOfYear(d) {
	        var _year = year(d);
	        var _month = month(d);
	        var _day = day(d);
	        for (var i = 1; i < _month; i++) {
	            _day += daysInMonth(_year, i);
	        }return _day;
	    }
	    exports.dayOfYear = dayOfYear;
	    function add(d, ts) {
	        return parse(d.getTime() + ts, d.kind || 1);
	    }
	    exports.add = add;
	    function addDays(d, v) {
	        return parse(d.getTime() + v * 86400000, d.kind || 1);
	    }
	    exports.addDays = addDays;
	    function addHours(d, v) {
	        return parse(d.getTime() + v * 3600000, d.kind || 1);
	    }
	    exports.addHours = addHours;
	    function addMinutes(d, v) {
	        return parse(d.getTime() + v * 60000, d.kind || 1);
	    }
	    exports.addMinutes = addMinutes;
	    function addSeconds(d, v) {
	        return parse(d.getTime() + v * 1000, d.kind || 1);
	    }
	    exports.addSeconds = addSeconds;
	    function addMilliseconds(d, v) {
	        return parse(d.getTime() + v, d.kind || 1);
	    }
	    exports.addMilliseconds = addMilliseconds;
	    function addTicks(d, t) {
	        return parse(Long.fromNumber(d.getTime()).add(t.div(10000)).toNumber(), d.kind || 1);
	    }
	    exports.addTicks = addTicks;
	    function addYears(d, v) {
	        var newMonth = month(d);
	        var newYear = year(d) + v;
	        var _daysInMonth = daysInMonth(newYear, newMonth);
	        var newDay = Math.min(_daysInMonth, day(d));
	        return create(newYear, newMonth, newDay, hour(d), minute(d), second(d), millisecond(d), d.kind || 1);
	    }
	    exports.addYears = addYears;
	    function addMonths(d, v) {
	        var newMonth = month(d) + v;
	        var newMonth_ = 0;
	        var yearOffset = 0;
	        if (newMonth > 12) {
	            newMonth_ = newMonth % 12;
	            yearOffset = Math.floor(newMonth / 12);
	            newMonth = newMonth_;
	        } else if (newMonth < 1) {
	            newMonth_ = 12 + newMonth % 12;
	            yearOffset = Math.floor(newMonth / 12) + (newMonth_ == 12 ? -1 : 0);
	            newMonth = newMonth_;
	        }
	        var newYear = year(d) + yearOffset;
	        var _daysInMonth = daysInMonth(newYear, newMonth);
	        var newDay = Math.min(_daysInMonth, day(d));
	        return create(newYear, newMonth, newDay, hour(d), minute(d), second(d), millisecond(d), d.kind || 1);
	    }
	    exports.addMonths = addMonths;
	    function subtract(d, that) {
	        return typeof that == "number" ? parse(d.getTime() - that, d.kind || 1) : d.getTime() - that.getTime();
	    }
	    exports.subtract = subtract;
	    function toLongDateString(d) {
	        return d.toDateString();
	    }
	    exports.toLongDateString = toLongDateString;
	    function toShortDateString(d) {
	        return d.toLocaleDateString();
	    }
	    exports.toShortDateString = toShortDateString;
	    function toLongTimeString(d) {
	        return d.toLocaleTimeString();
	    }
	    exports.toLongTimeString = toLongTimeString;
	    function toShortTimeString(d) {
	        return d.toLocaleTimeString().replace(/:\d\d(?!:)/, "");
	    }
	    exports.toShortTimeString = toShortTimeString;
	    function equals(d1, d2) {
	        return d1.getTime() == d2.getTime();
	    }
	    exports.equals = equals;
	    function compare(x, y) {
	        return Util_1.compare(x, y);
	    }
	    exports.compare = compare;
	    function compareTo(x, y) {
	        return Util_1.compare(x, y);
	    }
	    exports.compareTo = compareTo;
	    function op_Addition(x, y) {
	        return add(x, y);
	    }
	    exports.op_Addition = op_Addition;
	    function op_Subtraction(x, y) {
	        return subtract(x, y);
	    }
	    exports.op_Subtraction = op_Subtraction;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 18 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Util", "./Seq", "./Observable", "./Observable"], function (require, exports) {
	    "use strict";
	
	    var Util_1 = require("./Util");
	    var Seq_1 = require("./Seq");
	    var Observable_1 = require("./Observable");
	    var Observable_2 = require("./Observable");
	    var Event = function () {
	        function Event(_subscriber, delegates) {
	            this._subscriber = _subscriber;
	            this.delegates = delegates || new Array();
	        }
	        Event.prototype.Add = function (f) {
	            this._addHandler(f);
	        };
	        Object.defineProperty(Event.prototype, "Publish", {
	            get: function get() {
	                return this;
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Event.prototype.Trigger = function (value) {
	            Seq_1.iterate(function (f) {
	                return f(value);
	            }, this.delegates);
	        };
	        Event.prototype._addHandler = function (f) {
	            this.delegates.push(f);
	        };
	        Event.prototype._removeHandler = function (f) {
	            var index = this.delegates.indexOf(f);
	            if (index > -1) this.delegates.splice(index, 1);
	        };
	        Event.prototype.AddHandler = function (handler) {
	            if (this._dotnetDelegates == null) {
	                this._dotnetDelegates = new Map();
	            }
	            var f = function f(x) {
	                handler(null, x);
	            };
	            this._dotnetDelegates.set(handler, f);
	            this._addHandler(f);
	        };
	        Event.prototype.RemoveHandler = function (handler) {
	            if (this._dotnetDelegates != null) {
	                var f = this._dotnetDelegates.get(handler);
	                if (f != null) {
	                    this._dotnetDelegates.delete(handler);
	                    this._removeHandler(f);
	                }
	            }
	        };
	        Event.prototype._subscribeFromObserver = function (observer) {
	            var _this = this;
	            if (this._subscriber) return this._subscriber(observer);
	            var callback = observer.OnNext;
	            this._addHandler(callback);
	            return Util_1.createDisposable(function () {
	                return _this._removeHandler(callback);
	            });
	        };
	        Event.prototype._subscribeFromCallback = function (callback) {
	            var _this = this;
	            this._addHandler(callback);
	            return Util_1.createDisposable(function () {
	                return _this._removeHandler(callback);
	            });
	        };
	        Event.prototype.Subscribe = function (arg) {
	            return typeof arg == "function" ? this._subscribeFromCallback(arg) : this._subscribeFromObserver(arg);
	        };
	        return Event;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = Event;
	    function add(callback, sourceEvent) {
	        sourceEvent.Subscribe(new Observable_1.Observer(callback));
	    }
	    exports.add = add;
	    function choose(chooser, sourceEvent) {
	        var source = sourceEvent;
	        return new Event(function (observer) {
	            return source.Subscribe(new Observable_1.Observer(function (t) {
	                return Observable_2.protect(function () {
	                    return chooser(t);
	                }, function (u) {
	                    if (u != null) observer.OnNext(u);
	                }, observer.OnError);
	            }, observer.OnError, observer.OnCompleted));
	        }, source.delegates);
	    }
	    exports.choose = choose;
	    function filter(predicate, sourceEvent) {
	        return choose(function (x) {
	            return predicate(x) ? x : null;
	        }, sourceEvent);
	    }
	    exports.filter = filter;
	    function map(mapping, sourceEvent) {
	        var source = sourceEvent;
	        return new Event(function (observer) {
	            return source.Subscribe(new Observable_1.Observer(function (t) {
	                return Observable_2.protect(function () {
	                    return mapping(t);
	                }, observer.OnNext, observer.OnError);
	            }, observer.OnError, observer.OnCompleted));
	        }, source.delegates);
	    }
	    exports.map = map;
	    function merge(event1, event2) {
	        var source1 = event1;
	        var source2 = event2;
	        return new Event(function (observer) {
	            var stopped = false,
	                completed1 = false,
	                completed2 = false;
	            var h1 = source1.Subscribe(new Observable_1.Observer(function (v) {
	                if (!stopped) observer.OnNext(v);
	            }, function (e) {
	                if (!stopped) {
	                    stopped = true;
	                    observer.OnError(e);
	                }
	            }, function () {
	                if (!stopped) {
	                    completed1 = true;
	                    if (completed2) {
	                        stopped = true;
	                        observer.OnCompleted();
	                    }
	                }
	            }));
	            var h2 = source2.Subscribe(new Observable_1.Observer(function (v) {
	                if (!stopped) observer.OnNext(v);
	            }, function (e) {
	                if (!stopped) {
	                    stopped = true;
	                    observer.OnError(e);
	                }
	            }, function () {
	                if (!stopped) {
	                    completed2 = true;
	                    if (completed1) {
	                        stopped = true;
	                        observer.OnCompleted();
	                    }
	                }
	            }));
	            return Util_1.createDisposable(function () {
	                h1.Dispose();
	                h2.Dispose();
	            });
	        }, source1.delegates.concat(source2.delegates));
	    }
	    exports.merge = merge;
	    function pairwise(sourceEvent) {
	        var source = sourceEvent;
	        return new Event(function (observer) {
	            var last = null;
	            return source.Subscribe(new Observable_1.Observer(function (next) {
	                if (last != null) observer.OnNext([last, next]);
	                last = next;
	            }, observer.OnError, observer.OnCompleted));
	        }, source.delegates);
	    }
	    exports.pairwise = pairwise;
	    function partition(predicate, sourceEvent) {
	        return [filter(predicate, sourceEvent), filter(function (x) {
	            return !predicate(x);
	        }, sourceEvent)];
	    }
	    exports.partition = partition;
	    function scan(collector, state, sourceEvent) {
	        var source = sourceEvent;
	        return new Event(function (observer) {
	            return source.Subscribe(new Observable_1.Observer(function (t) {
	                Observable_2.protect(function () {
	                    return collector(state, t);
	                }, function (u) {
	                    state = u;observer.OnNext(u);
	                }, observer.OnError);
	            }, observer.OnError, observer.OnCompleted));
	        }, source.delegates);
	    }
	    exports.scan = scan;
	    function split(splitter, sourceEvent) {
	        return [choose(function (v) {
	            return splitter(v).valueIfChoice1;
	        }, sourceEvent), choose(function (v) {
	            return splitter(v).valueIfChoice2;
	        }, sourceEvent)];
	    }
	    exports.split = split;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 19 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Util", "./Symbol"], function (require, exports) {
	    "use strict";
	
	    var Util_1 = require("./Util");
	    var Symbol_1 = require("./Symbol");
	    var GenericComparer = function () {
	        function GenericComparer(f) {
	            this.Compare = f || Util_1.compare;
	        }
	        GenericComparer.prototype[Symbol_1.default.reflection] = function () {
	            return { interfaces: ["System.IComparer"] };
	        };
	        return GenericComparer;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = GenericComparer;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 20 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports"], function (require, exports) {
	    "use strict";
	
	    function createFromValue(v) {
	        return new Lazy(function () {
	            return v;
	        });
	    }
	    exports.createFromValue = createFromValue;
	    var Lazy = function () {
	        function Lazy(factory) {
	            this.factory = factory;
	            this.isValueCreated = false;
	        }
	        Object.defineProperty(Lazy.prototype, "value", {
	            get: function get() {
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
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = Lazy;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 21 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./ListClass", "./Seq", "./Seq", "./Seq", "./Seq", "./Map", "./ListClass"], function (require, exports) {
	    "use strict";
	
	    var ListClass_1 = require("./ListClass");
	    var Seq_1 = require("./Seq");
	    var Seq_2 = require("./Seq");
	    var Seq_3 = require("./Seq");
	    var Seq_4 = require("./Seq");
	    var Map_1 = require("./Map");
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = ListClass_1.default;
	    var ListClass_2 = require("./ListClass");
	    exports.ofArray = ListClass_2.ofArray;
	    function append(xs, ys) {
	        return Seq_2.fold(function (acc, x) {
	            return new ListClass_1.default(x, acc);
	        }, ys, reverse(xs));
	    }
	    exports.append = append;
	    function choose(f, xs) {
	        var r = Seq_2.fold(function (acc, x) {
	            var y = f(x);
	            return y != null ? new ListClass_1.default(y, acc) : acc;
	        }, new ListClass_1.default(), xs);
	        return reverse(r);
	    }
	    exports.choose = choose;
	    function collect(f, xs) {
	        return Seq_2.fold(function (acc, x) {
	            return append(acc, f(x));
	        }, new ListClass_1.default(), xs);
	    }
	    exports.collect = collect;
	    function concat(xs) {
	        return collect(function (x) {
	            return x;
	        }, xs);
	    }
	    exports.concat = concat;
	    function filter(f, xs) {
	        return reverse(Seq_2.fold(function (acc, x) {
	            return f(x) ? new ListClass_1.default(x, acc) : acc;
	        }, new ListClass_1.default(), xs));
	    }
	    exports.filter = filter;
	    function where(f, xs) {
	        return filter(f, xs);
	    }
	    exports.where = where;
	    function initialize(n, f) {
	        if (n < 0) {
	            throw new Error("List length must be non-negative");
	        }
	        var xs = new ListClass_1.default();
	        for (var i = 1; i <= n; i++) {
	            xs = new ListClass_1.default(f(n - i), xs);
	        }
	        return xs;
	    }
	    exports.initialize = initialize;
	    function map(f, xs) {
	        return reverse(Seq_2.fold(function (acc, x) {
	            return new ListClass_1.default(f(x), acc);
	        }, new ListClass_1.default(), xs));
	    }
	    exports.map = map;
	    function mapIndexed(f, xs) {
	        return reverse(Seq_2.fold(function (acc, x, i) {
	            return new ListClass_1.default(f(i, x), acc);
	        }, new ListClass_1.default(), xs));
	    }
	    exports.mapIndexed = mapIndexed;
	    function partition(f, xs) {
	        return Seq_2.fold(function (acc, x) {
	            var lacc = acc[0],
	                racc = acc[1];
	            return f(x) ? [new ListClass_1.default(x, lacc), racc] : [lacc, new ListClass_1.default(x, racc)];
	        }, [new ListClass_1.default(), new ListClass_1.default()], reverse(xs));
	    }
	    exports.partition = partition;
	    function replicate(n, x) {
	        return initialize(n, function () {
	            return x;
	        });
	    }
	    exports.replicate = replicate;
	    function reverse(xs) {
	        return Seq_2.fold(function (acc, x) {
	            return new ListClass_1.default(x, acc);
	        }, new ListClass_1.default(), xs);
	    }
	    exports.reverse = reverse;
	    function singleton(x) {
	        return new ListClass_1.default(x, new ListClass_1.default());
	    }
	    exports.singleton = singleton;
	    function slice(lower, upper, xs) {
	        var noLower = lower == null;
	        var noUpper = upper == null;
	        return reverse(Seq_2.fold(function (acc, x, i) {
	            return (noLower || lower <= i) && (noUpper || i <= upper) ? new ListClass_1.default(x, acc) : acc;
	        }, new ListClass_1.default(), xs));
	    }
	    exports.slice = slice;
	    function unzip(xs) {
	        return Seq_3.foldBack(function (xy, acc) {
	            return [new ListClass_1.default(xy[0], acc[0]), new ListClass_1.default(xy[1], acc[1])];
	        }, xs, [new ListClass_1.default(), new ListClass_1.default()]);
	    }
	    exports.unzip = unzip;
	    function unzip3(xs) {
	        return Seq_3.foldBack(function (xyz, acc) {
	            return [new ListClass_1.default(xyz[0], acc[0]), new ListClass_1.default(xyz[1], acc[1]), new ListClass_1.default(xyz[2], acc[2])];
	        }, xs, [new ListClass_1.default(), new ListClass_1.default(), new ListClass_1.default()]);
	    }
	    exports.unzip3 = unzip3;
	    function groupBy(f, xs) {
	        return Seq_4.toList(Seq_1.map(function (k) {
	            return [k[0], Seq_4.toList(k[1])];
	        }, Map_1.groupBy(f, xs)));
	    }
	    exports.groupBy = groupBy;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 22 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Symbol", "./Util", "./Util", "./Util"], function (require, exports) {
	    "use strict";
	
	    var Symbol_1 = require("./Symbol");
	    var Util_1 = require("./Util");
	    var Util_2 = require("./Util");
	    var Util_3 = require("./Util");
	    function ofArray(args, base) {
	        var acc = base || new List();
	        for (var i = args.length - 1; i >= 0; i--) {
	            acc = new List(args[i], acc);
	        }
	        return acc;
	    }
	    exports.ofArray = ofArray;
	    var List = function () {
	        function List(head, tail) {
	            this.head = head;
	            this.tail = tail;
	        }
	        List.prototype.ToString = function () {
	            return "[" + Array.from(this).map(Util_1.toString).join("; ") + "]";
	        };
	        List.prototype.Equals = function (x) {
	            if (this === x) {
	                return true;
	            } else {
	                var iter1 = this[Symbol.iterator](),
	                    iter2 = x[Symbol.iterator]();
	                for (;;) {
	                    var cur1 = iter1.next(),
	                        cur2 = iter2.next();
	                    if (cur1.done) return cur2.done ? true : false;else if (cur2.done) return false;else if (!Util_2.equals(cur1.value, cur2.value)) return false;
	                }
	            }
	        };
	        List.prototype.CompareTo = function (x) {
	            if (this === x) {
	                return 0;
	            } else {
	                var acc = 0;
	                var iter1 = this[Symbol.iterator](),
	                    iter2 = x[Symbol.iterator]();
	                for (;;) {
	                    var cur1 = iter1.next(),
	                        cur2 = iter2.next();
	                    if (cur1.done) return cur2.done ? acc : -1;else if (cur2.done) return 1;else {
	                        acc = Util_3.compare(cur1.value, cur2.value);
	                        if (acc != 0) return acc;
	                    }
	                }
	            }
	        };
	        Object.defineProperty(List.prototype, "length", {
	            get: function get() {
	                var cur = this,
	                    acc = 0;
	                while (cur.tail != null) {
	                    cur = cur.tail;
	                    acc++;
	                }
	                return acc;
	            },
	            enumerable: true,
	            configurable: true
	        });
	        List.prototype[Symbol.iterator] = function () {
	            var cur = this;
	            return {
	                next: function next() {
	                    var tmp = cur;
	                    cur = cur.tail;
	                    return { done: tmp.tail == null, value: tmp.head };
	                }
	            };
	        };
	        List.prototype[Symbol_1.default.reflection] = function () {
	            return {
	                type: "Microsoft.FSharp.Collections.FSharpList",
	                interfaces: ["System.IEquatable", "System.IComparable"]
	            };
	        };
	        return List;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = List;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 23 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Symbol"], function (require, exports) {
	    "use strict";
	
	    var Symbol_1 = require("./Symbol");
	    var Long = function () {
	        function Long(low, high, unsigned) {
	            this.eq = this.equals;
	            this.neq = this.notEquals;
	            this.lt = this.lessThan;
	            this.lte = this.lessThanOrEqual;
	            this.gt = this.greaterThan;
	            this.gte = this.greaterThanOrEqual;
	            this.comp = this.compare;
	            this.neg = this.negate;
	            this.abs = this.absolute;
	            this.sub = this.subtract;
	            this.mul = this.multiply;
	            this.div = this.divide;
	            this.mod = this.modulo;
	            this.shl = this.shiftLeft;
	            this.shr = this.shiftRight;
	            this.shru = this.shiftRightUnsigned;
	            this.Equals = this.equals;
	            this.CompareTo = this.compare;
	            this.ToString = this.toString;
	            this.low = low | 0;
	            this.high = high | 0;
	            this.unsigned = !!unsigned;
	        }
	        Long.prototype.toInt = function () {
	            return this.unsigned ? this.low >>> 0 : this.low;
	        };
	        Long.prototype.toNumber = function () {
	            if (this.unsigned) return (this.high >>> 0) * TWO_PWR_32_DBL + (this.low >>> 0);
	            return this.high * TWO_PWR_32_DBL + (this.low >>> 0);
	        };
	        Long.prototype.toString = function (radix) {
	            if (radix === void 0) {
	                radix = 10;
	            }
	            radix = radix || 10;
	            if (radix < 2 || 36 < radix) throw RangeError('radix');
	            if (this.isZero()) return '0';
	            if (this.isNegative()) {
	                if (this.eq(exports.MIN_VALUE)) {
	                    var radixLong = fromNumber(radix),
	                        div = this.div(radixLong),
	                        rem1 = div.mul(radixLong).sub(this);
	                    return div.toString(radix) + rem1.toInt().toString(radix);
	                } else return '-' + this.neg().toString(radix);
	            }
	            var radixToPower = fromNumber(pow_dbl(radix, 6), this.unsigned),
	                rem = this;
	            var result = '';
	            while (true) {
	                var remDiv = rem.div(radixToPower),
	                    intval = rem.sub(remDiv.mul(radixToPower)).toInt() >>> 0,
	                    digits = intval.toString(radix);
	                rem = remDiv;
	                if (rem.isZero()) return digits + result;else {
	                    while (digits.length < 6) {
	                        digits = '0' + digits;
	                    }result = '' + digits + result;
	                }
	            }
	        };
	        Long.prototype.getHighBits = function () {
	            return this.high;
	        };
	        Long.prototype.getHighBitsUnsigned = function () {
	            return this.high >>> 0;
	        };
	        Long.prototype.getLowBits = function () {
	            return this.low;
	        };
	        Long.prototype.getLowBitsUnsigned = function () {
	            return this.low >>> 0;
	        };
	        Long.prototype.getNumBitsAbs = function () {
	            if (this.isNegative()) return this.eq(exports.MIN_VALUE) ? 64 : this.neg().getNumBitsAbs();
	            var val = this.high != 0 ? this.high : this.low;
	            for (var bit = 31; bit > 0; bit--) {
	                if ((val & 1 << bit) != 0) break;
	            }return this.high != 0 ? bit + 33 : bit + 1;
	        };
	        Long.prototype.isZero = function () {
	            return this.high === 0 && this.low === 0;
	        };
	        Long.prototype.isNegative = function () {
	            return !this.unsigned && this.high < 0;
	        };
	        Long.prototype.isPositive = function () {
	            return this.unsigned || this.high >= 0;
	        };
	        Long.prototype.isOdd = function () {
	            return (this.low & 1) === 1;
	        };
	        Long.prototype.isEven = function () {
	            return (this.low & 1) === 0;
	        };
	        Long.prototype.equals = function (other) {
	            if (!isLong(other)) other = fromValue(other);
	            if (this.unsigned !== other.unsigned && this.high >>> 31 === 1 && other.high >>> 31 === 1) return false;
	            return this.high === other.high && this.low === other.low;
	        };
	        Long.prototype.notEquals = function (other) {
	            return !this.eq(other);
	        };
	        Long.prototype.lessThan = function (other) {
	            return this.comp(other) < 0;
	        };
	        Long.prototype.lessThanOrEqual = function (other) {
	            return this.comp(other) <= 0;
	        };
	        Long.prototype.greaterThan = function (other) {
	            return this.comp(other) > 0;
	        };
	        Long.prototype.greaterThanOrEqual = function (other) {
	            return this.comp(other) >= 0;
	        };
	        Long.prototype.compare = function (other) {
	            if (!isLong(other)) other = fromValue(other);
	            if (this.eq(other)) return 0;
	            var thisNeg = this.isNegative(),
	                otherNeg = other.isNegative();
	            if (thisNeg && !otherNeg) return -1;
	            if (!thisNeg && otherNeg) return 1;
	            if (!this.unsigned) return this.sub(other).isNegative() ? -1 : 1;
	            return other.high >>> 0 > this.high >>> 0 || other.high === this.high && other.low >>> 0 > this.low >>> 0 ? -1 : 1;
	        };
	        Long.prototype.negate = function () {
	            if (!this.unsigned && this.eq(exports.MIN_VALUE)) return exports.MIN_VALUE;
	            return this.not().add(exports.ONE);
	        };
	        Long.prototype.absolute = function () {
	            if (!this.unsigned && this.isNegative()) return this.negate();else return this;
	        };
	        Long.prototype.add = function (addend) {
	            if (!isLong(addend)) addend = fromValue(addend);
	            var a48 = this.high >>> 16;
	            var a32 = this.high & 0xFFFF;
	            var a16 = this.low >>> 16;
	            var a00 = this.low & 0xFFFF;
	            var b48 = addend.high >>> 16;
	            var b32 = addend.high & 0xFFFF;
	            var b16 = addend.low >>> 16;
	            var b00 = addend.low & 0xFFFF;
	            var c48 = 0,
	                c32 = 0,
	                c16 = 0,
	                c00 = 0;
	            c00 += a00 + b00;
	            c16 += c00 >>> 16;
	            c00 &= 0xFFFF;
	            c16 += a16 + b16;
	            c32 += c16 >>> 16;
	            c16 &= 0xFFFF;
	            c32 += a32 + b32;
	            c48 += c32 >>> 16;
	            c32 &= 0xFFFF;
	            c48 += a48 + b48;
	            c48 &= 0xFFFF;
	            return fromBits(c16 << 16 | c00, c48 << 16 | c32, this.unsigned);
	        };
	        Long.prototype.subtract = function (subtrahend) {
	            if (!isLong(subtrahend)) subtrahend = fromValue(subtrahend);
	            return this.add(subtrahend.neg());
	        };
	        Long.prototype.multiply = function (multiplier) {
	            if (this.isZero()) return exports.ZERO;
	            if (!isLong(multiplier)) multiplier = fromValue(multiplier);
	            if (multiplier.isZero()) return exports.ZERO;
	            if (this.eq(exports.MIN_VALUE)) return multiplier.isOdd() ? exports.MIN_VALUE : exports.ZERO;
	            if (multiplier.eq(exports.MIN_VALUE)) return this.isOdd() ? exports.MIN_VALUE : exports.ZERO;
	            if (this.isNegative()) {
	                if (multiplier.isNegative()) return this.neg().mul(multiplier.neg());else return this.neg().mul(multiplier).neg();
	            } else if (multiplier.isNegative()) return this.mul(multiplier.neg()).neg();
	            if (this.lt(TWO_PWR_24) && multiplier.lt(TWO_PWR_24)) return fromNumber(this.toNumber() * multiplier.toNumber(), this.unsigned);
	            var a48 = this.high >>> 16;
	            var a32 = this.high & 0xFFFF;
	            var a16 = this.low >>> 16;
	            var a00 = this.low & 0xFFFF;
	            var b48 = multiplier.high >>> 16;
	            var b32 = multiplier.high & 0xFFFF;
	            var b16 = multiplier.low >>> 16;
	            var b00 = multiplier.low & 0xFFFF;
	            var c48 = 0,
	                c32 = 0,
	                c16 = 0,
	                c00 = 0;
	            c00 += a00 * b00;
	            c16 += c00 >>> 16;
	            c00 &= 0xFFFF;
	            c16 += a16 * b00;
	            c32 += c16 >>> 16;
	            c16 &= 0xFFFF;
	            c16 += a00 * b16;
	            c32 += c16 >>> 16;
	            c16 &= 0xFFFF;
	            c32 += a32 * b00;
	            c48 += c32 >>> 16;
	            c32 &= 0xFFFF;
	            c32 += a16 * b16;
	            c48 += c32 >>> 16;
	            c32 &= 0xFFFF;
	            c32 += a00 * b32;
	            c48 += c32 >>> 16;
	            c32 &= 0xFFFF;
	            c48 += a48 * b00 + a32 * b16 + a16 * b32 + a00 * b48;
	            c48 &= 0xFFFF;
	            return fromBits(c16 << 16 | c00, c48 << 16 | c32, this.unsigned);
	        };
	        Long.prototype.divide = function (divisor) {
	            if (!isLong(divisor)) divisor = fromValue(divisor);
	            if (divisor.isZero()) throw Error('division by zero');
	            if (this.isZero()) return this.unsigned ? exports.UZERO : exports.ZERO;
	            var approx = 0,
	                rem = exports.ZERO,
	                res = exports.ZERO;
	            if (!this.unsigned) {
	                if (this.eq(exports.MIN_VALUE)) {
	                    if (divisor.eq(exports.ONE) || divisor.eq(exports.NEG_ONE)) return exports.MIN_VALUE;else if (divisor.eq(exports.MIN_VALUE)) return exports.ONE;else {
	                        var halfThis = this.shr(1);
	                        var approx_1 = halfThis.div(divisor).shl(1);
	                        if (approx_1.eq(exports.ZERO)) {
	                            return divisor.isNegative() ? exports.ONE : exports.NEG_ONE;
	                        } else {
	                            rem = this.sub(divisor.mul(approx_1));
	                            res = approx_1.add(rem.div(divisor));
	                            return res;
	                        }
	                    }
	                } else if (divisor.eq(exports.MIN_VALUE)) return this.unsigned ? exports.UZERO : exports.ZERO;
	                if (this.isNegative()) {
	                    if (divisor.isNegative()) return this.neg().div(divisor.neg());
	                    return this.neg().div(divisor).neg();
	                } else if (divisor.isNegative()) return this.div(divisor.neg()).neg();
	                res = exports.ZERO;
	            } else {
	                if (!divisor.unsigned) divisor = divisor.toUnsigned();
	                if (divisor.gt(this)) return exports.UZERO;
	                if (divisor.gt(this.shru(1))) return exports.UONE;
	                res = exports.UZERO;
	            }
	            rem = this;
	            while (rem.gte(divisor)) {
	                approx = Math.max(1, Math.floor(rem.toNumber() / divisor.toNumber()));
	                var log2 = Math.ceil(Math.log(approx) / Math.LN2),
	                    delta = log2 <= 48 ? 1 : pow_dbl(2, log2 - 48),
	                    approxRes = fromNumber(approx),
	                    approxRem = approxRes.mul(divisor);
	                while (approxRem.isNegative() || approxRem.gt(rem)) {
	                    approx -= delta;
	                    approxRes = fromNumber(approx, this.unsigned);
	                    approxRem = approxRes.mul(divisor);
	                }
	                if (approxRes.isZero()) approxRes = exports.ONE;
	                res = res.add(approxRes);
	                rem = rem.sub(approxRem);
	            }
	            return res;
	        };
	        Long.prototype.modulo = function (divisor) {
	            if (!isLong(divisor)) divisor = fromValue(divisor);
	            return this.sub(this.div(divisor).mul(divisor));
	        };
	        ;
	        Long.prototype.not = function () {
	            return fromBits(~this.low, ~this.high, this.unsigned);
	        };
	        ;
	        Long.prototype.and = function (other) {
	            if (!isLong(other)) other = fromValue(other);
	            return fromBits(this.low & other.low, this.high & other.high, this.unsigned);
	        };
	        Long.prototype.or = function (other) {
	            if (!isLong(other)) other = fromValue(other);
	            return fromBits(this.low | other.low, this.high | other.high, this.unsigned);
	        };
	        Long.prototype.xor = function (other) {
	            if (!isLong(other)) other = fromValue(other);
	            return fromBits(this.low ^ other.low, this.high ^ other.high, this.unsigned);
	        };
	        Long.prototype.shiftLeft = function (numBits) {
	            if (isLong(numBits)) numBits = numBits.toInt();
	            numBits = numBits & 63;
	            if (numBits === 0) return this;else if (numBits < 32) return fromBits(this.low << numBits, this.high << numBits | this.low >>> 32 - numBits, this.unsigned);else return fromBits(0, this.low << numBits - 32, this.unsigned);
	        };
	        Long.prototype.shiftRight = function (numBits) {
	            if (isLong(numBits)) numBits = numBits.toInt();
	            numBits = numBits & 63;
	            if (numBits === 0) return this;else if (numBits < 32) return fromBits(this.low >>> numBits | this.high << 32 - numBits, this.high >> numBits, this.unsigned);else return fromBits(this.high >> numBits - 32, this.high >= 0 ? 0 : -1, this.unsigned);
	        };
	        Long.prototype.shiftRightUnsigned = function (numBits) {
	            if (isLong(numBits)) numBits = numBits.toInt();
	            numBits = numBits & 63;
	            if (numBits === 0) return this;else {
	                var high = this.high;
	                if (numBits < 32) {
	                    var low = this.low;
	                    return fromBits(low >>> numBits | high << 32 - numBits, high >>> numBits, this.unsigned);
	                } else if (numBits === 32) return fromBits(high, 0, this.unsigned);else return fromBits(high >>> numBits - 32, 0, this.unsigned);
	            }
	        };
	        Long.prototype.toSigned = function () {
	            if (!this.unsigned) return this;
	            return fromBits(this.low, this.high, false);
	        };
	        Long.prototype.toUnsigned = function () {
	            if (this.unsigned) return this;
	            return fromBits(this.low, this.high, true);
	        };
	        Long.prototype.toBytes = function (le) {
	            return le ? this.toBytesLE() : this.toBytesBE();
	        };
	        Long.prototype.toBytesLE = function () {
	            var hi = this.high,
	                lo = this.low;
	            return [lo & 0xff, lo >>> 8 & 0xff, lo >>> 16 & 0xff, lo >>> 24 & 0xff, hi & 0xff, hi >>> 8 & 0xff, hi >>> 16 & 0xff, hi >>> 24 & 0xff];
	        };
	        Long.prototype.toBytesBE = function () {
	            var hi = this.high,
	                lo = this.low;
	            return [hi >>> 24 & 0xff, hi >>> 16 & 0xff, hi >>> 8 & 0xff, hi & 0xff, lo >>> 24 & 0xff, lo >>> 16 & 0xff, lo >>> 8 & 0xff, lo & 0xff];
	        };
	        Long.prototype[Symbol_1.default.reflection] = function () {
	            return {
	                type: "System.Int64",
	                interfaces: ["FSharpRecord", "System.IComparable"],
	                properties: {
	                    low: "number",
	                    high: "number",
	                    unsigned: "boolean"
	                }
	            };
	        };
	        return Long;
	    }();
	    exports.Long = Long;
	    var INT_CACHE = {};
	    var UINT_CACHE = {};
	    function isLong(obj) {
	        return obj && obj instanceof Long;
	    }
	    exports.isLong = isLong;
	    function fromInt(value, unsigned) {
	        if (unsigned === void 0) {
	            unsigned = false;
	        }
	        var obj, cachedObj, cache;
	        if (unsigned) {
	            value >>>= 0;
	            if (cache = 0 <= value && value < 256) {
	                cachedObj = UINT_CACHE[value];
	                if (cachedObj) return cachedObj;
	            }
	            obj = fromBits(value, (value | 0) < 0 ? -1 : 0, true);
	            if (cache) UINT_CACHE[value] = obj;
	            return obj;
	        } else {
	            value |= 0;
	            if (cache = -128 <= value && value < 128) {
	                cachedObj = INT_CACHE[value];
	                if (cachedObj) return cachedObj;
	            }
	            obj = fromBits(value, value < 0 ? -1 : 0, false);
	            if (cache) INT_CACHE[value] = obj;
	            return obj;
	        }
	    }
	    exports.fromInt = fromInt;
	    function fromNumber(value, unsigned) {
	        if (unsigned === void 0) {
	            unsigned = false;
	        }
	        if (isNaN(value) || !isFinite(value)) return unsigned ? exports.UZERO : exports.ZERO;
	        if (unsigned) {
	            if (value < 0) return exports.UZERO;
	            if (value >= TWO_PWR_64_DBL) return exports.MAX_UNSIGNED_VALUE;
	        } else {
	            if (value <= -TWO_PWR_63_DBL) return exports.MIN_VALUE;
	            if (value + 1 >= TWO_PWR_63_DBL) return exports.MAX_VALUE;
	        }
	        if (value < 0) return fromNumber(-value, unsigned).neg();
	        return fromBits(value % TWO_PWR_32_DBL | 0, value / TWO_PWR_32_DBL | 0, unsigned);
	    }
	    exports.fromNumber = fromNumber;
	    function fromBits(lowBits, highBits, unsigned) {
	        return new Long(lowBits, highBits, unsigned);
	    }
	    exports.fromBits = fromBits;
	    var pow_dbl = Math.pow;
	    function fromString(str, unsigned, radix) {
	        if (unsigned === void 0) {
	            unsigned = false;
	        }
	        if (radix === void 0) {
	            radix = 10;
	        }
	        if (str.length === 0) throw Error('empty string');
	        if (str === "NaN" || str === "Infinity" || str === "+Infinity" || str === "-Infinity") return exports.ZERO;
	        if (typeof unsigned === 'number') {
	            radix = unsigned, unsigned = false;
	        } else {
	            unsigned = !!unsigned;
	        }
	        radix = radix || 10;
	        if (radix < 2 || 36 < radix) throw RangeError('radix');
	        var p = str.indexOf('-');
	        if (p > 0) throw Error('interior hyphen');else if (p === 0) {
	            return fromString(str.substring(1), unsigned, radix).neg();
	        }
	        var radixToPower = fromNumber(pow_dbl(radix, 8));
	        var result = exports.ZERO;
	        for (var i = 0; i < str.length; i += 8) {
	            var size = Math.min(8, str.length - i),
	                value = parseInt(str.substring(i, i + size), radix);
	            if (size < 8) {
	                var power = fromNumber(pow_dbl(radix, size));
	                result = result.mul(power).add(fromNumber(value));
	            } else {
	                result = result.mul(radixToPower);
	                result = result.add(fromNumber(value));
	            }
	        }
	        result.unsigned = unsigned;
	        return result;
	    }
	    exports.fromString = fromString;
	    function fromValue(val) {
	        if (val instanceof Long) return val;
	        if (typeof val === 'number') return fromNumber(val);
	        if (typeof val === 'string') return fromString(val);
	        return fromBits(val.low, val.high, val.unsigned);
	    }
	    exports.fromValue = fromValue;
	    var TWO_PWR_16_DBL = 1 << 16;
	    var TWO_PWR_24_DBL = 1 << 24;
	    var TWO_PWR_32_DBL = TWO_PWR_16_DBL * TWO_PWR_16_DBL;
	    var TWO_PWR_64_DBL = TWO_PWR_32_DBL * TWO_PWR_32_DBL;
	    var TWO_PWR_63_DBL = TWO_PWR_64_DBL / 2;
	    var TWO_PWR_24 = fromInt(TWO_PWR_24_DBL);
	    exports.ZERO = fromInt(0);
	    exports.UZERO = fromInt(0, true);
	    exports.ONE = fromInt(1);
	    exports.UONE = fromInt(1, true);
	    exports.NEG_ONE = fromInt(-1);
	    exports.MAX_VALUE = fromBits(0xFFFFFFFF | 0, 0x7FFFFFFF | 0, false);
	    exports.MAX_UNSIGNED_VALUE = fromBits(0xFFFFFFFF | 0, 0xFFFFFFFF | 0, true);
	    exports.MIN_VALUE = fromBits(0, 0x80000000 | 0, false);
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 24 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Async", "./Async", "./Async"], function (require, exports) {
	    "use strict";
	
	    var Async_1 = require("./Async");
	    var Async_2 = require("./Async");
	    var Async_3 = require("./Async");
	    var QueueCell = function () {
	        function QueueCell(message) {
	            this.value = message;
	        }
	        return QueueCell;
	    }();
	    var MailboxQueue = function () {
	        function MailboxQueue() {}
	        MailboxQueue.prototype.add = function (message) {
	            var itCell = new QueueCell(message);
	            if (this.firstAndLast) {
	                this.firstAndLast[1].next = itCell;
	                this.firstAndLast = [this.firstAndLast[0], itCell];
	            } else this.firstAndLast = [itCell, itCell];
	        };
	        MailboxQueue.prototype.tryGet = function () {
	            if (this.firstAndLast) {
	                var value = this.firstAndLast[0].value;
	                if (this.firstAndLast[0].next) this.firstAndLast = [this.firstAndLast[0].next, this.firstAndLast[1]];else delete this.firstAndLast;
	                return value;
	            }
	            return void 0;
	        };
	        return MailboxQueue;
	    }();
	    var MailboxProcessor = function () {
	        function MailboxProcessor(body, cancellationToken) {
	            this.body = body;
	            this.cancellationToken = cancellationToken || Async_1.defaultCancellationToken;
	            this.messages = new MailboxQueue();
	        }
	        MailboxProcessor.prototype.__processEvents = function () {
	            if (this.continuation) {
	                var value = this.messages.tryGet();
	                if (value) {
	                    var cont = this.continuation;
	                    delete this.continuation;
	                    cont(value);
	                }
	            }
	        };
	        MailboxProcessor.prototype.start = function () {
	            Async_3.startImmediate(this.body(this), this.cancellationToken);
	        };
	        MailboxProcessor.prototype.receive = function () {
	            var _this = this;
	            return Async_2.fromContinuations(function (conts) {
	                if (_this.continuation) throw new Error("Receive can only be called once!");
	                _this.continuation = conts[0];
	                _this.__processEvents();
	            });
	        };
	        MailboxProcessor.prototype.post = function (message) {
	            this.messages.add(message);
	            this.__processEvents();
	        };
	        MailboxProcessor.prototype.postAndAsyncReply = function (buildMessage) {
	            var result;
	            var continuation;
	            function checkCompletion() {
	                if (result && continuation) continuation(result);
	            }
	            var reply = {
	                reply: function reply(res) {
	                    result = res;
	                    checkCompletion();
	                }
	            };
	            this.messages.add(buildMessage(reply));
	            this.__processEvents();
	            return Async_2.fromContinuations(function (conts) {
	                continuation = conts[0];
	                checkCompletion();
	            });
	        };
	        return MailboxProcessor;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = MailboxProcessor;
	    function start(body, cancellationToken) {
	        var mbox = new MailboxProcessor(body, cancellationToken);
	        mbox.start();
	        return mbox;
	    }
	    exports.start = start;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 25 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./ListClass", "./ListClass", "./Util", "./Util", "./Util", "./GenericComparer", "./Symbol", "./Seq", "./Seq", "./Seq", "./Seq", "./Seq"], function (require, exports) {
	    "use strict";
	
	    var ListClass_1 = require("./ListClass");
	    var ListClass_2 = require("./ListClass");
	    var Util_1 = require("./Util");
	    var Util_2 = require("./Util");
	    var Util_3 = require("./Util");
	    var GenericComparer_1 = require("./GenericComparer");
	    var Symbol_1 = require("./Symbol");
	    var Seq_1 = require("./Seq");
	    var Seq_2 = require("./Seq");
	    var Seq_3 = require("./Seq");
	    var Seq_4 = require("./Seq");
	    var Seq_5 = require("./Seq");
	    function groupBy(f, xs) {
	        var keys = [],
	            iter = xs[Symbol.iterator]();
	        var acc = create(),
	            cur = iter.next();
	        while (!cur.done) {
	            var k = f(cur.value),
	                vs = tryFind(k, acc);
	            if (vs == null) {
	                keys.push(k);
	                acc = add(k, [cur.value], acc);
	            } else {
	                vs.push(cur.value);
	            }
	            cur = iter.next();
	        }
	        return keys.map(function (k) {
	            return [k, acc.get(k)];
	        });
	    }
	    exports.groupBy = groupBy;
	    function countBy(f, xs) {
	        return groupBy(f, xs).map(function (kv) {
	            return [kv[0], kv[1].length];
	        });
	    }
	    exports.countBy = countBy;
	    var MapTree = function () {
	        function MapTree(caseName, fields) {
	            this.Case = caseName;
	            this.Fields = fields;
	        }
	        return MapTree;
	    }();
	    exports.MapTree = MapTree;
	    function tree_sizeAux(acc, m) {
	        return m.Case === "MapOne" ? acc + 1 : m.Case === "MapNode" ? tree_sizeAux(tree_sizeAux(acc + 1, m.Fields[2]), m.Fields[3]) : acc;
	    }
	    function tree_size(x) {
	        return tree_sizeAux(0, x);
	    }
	    function tree_empty() {
	        return new MapTree("MapEmpty", []);
	    }
	    function tree_height(_arg1) {
	        return _arg1.Case === "MapOne" ? 1 : _arg1.Case === "MapNode" ? _arg1.Fields[4] : 0;
	    }
	    function tree_isEmpty(m) {
	        return m.Case === "MapEmpty" ? true : false;
	    }
	    function tree_mk(l, k, v, r) {
	        var matchValue = [l, r];
	        var $target1 = function $target1() {
	            var hl = tree_height(l);
	            var hr = tree_height(r);
	            var m = hl < hr ? hr : hl;
	            return new MapTree("MapNode", [k, v, l, r, m + 1]);
	        };
	        if (matchValue[0].Case === "MapEmpty") {
	            if (matchValue[1].Case === "MapEmpty") {
	                return new MapTree("MapOne", [k, v]);
	            } else {
	                return $target1();
	            }
	        } else {
	            return $target1();
	        }
	    }
	    ;
	    function tree_rebalance(t1, k, v, t2) {
	        var t1h = tree_height(t1);
	        var t2h = tree_height(t2);
	        if (t2h > t1h + 2) {
	            if (t2.Case === "MapNode") {
	                if (tree_height(t2.Fields[2]) > t1h + 1) {
	                    if (t2.Fields[2].Case === "MapNode") {
	                        return tree_mk(tree_mk(t1, k, v, t2.Fields[2].Fields[2]), t2.Fields[2].Fields[0], t2.Fields[2].Fields[1], tree_mk(t2.Fields[2].Fields[3], t2.Fields[0], t2.Fields[1], t2.Fields[3]));
	                    } else {
	                        throw new Error("rebalance");
	                    }
	                } else {
	                    return tree_mk(tree_mk(t1, k, v, t2.Fields[2]), t2.Fields[0], t2.Fields[1], t2.Fields[3]);
	                }
	            } else {
	                throw new Error("rebalance");
	            }
	        } else {
	            if (t1h > t2h + 2) {
	                if (t1.Case === "MapNode") {
	                    if (tree_height(t1.Fields[3]) > t2h + 1) {
	                        if (t1.Fields[3].Case === "MapNode") {
	                            return tree_mk(tree_mk(t1.Fields[2], t1.Fields[0], t1.Fields[1], t1.Fields[3].Fields[2]), t1.Fields[3].Fields[0], t1.Fields[3].Fields[1], tree_mk(t1.Fields[3].Fields[3], k, v, t2));
	                        } else {
	                            throw new Error("rebalance");
	                        }
	                    } else {
	                        return tree_mk(t1.Fields[2], t1.Fields[0], t1.Fields[1], tree_mk(t1.Fields[3], k, v, t2));
	                    }
	                } else {
	                    throw new Error("rebalance");
	                }
	            } else {
	                return tree_mk(t1, k, v, t2);
	            }
	        }
	    }
	    function tree_add(comparer, k, v, m) {
	        if (m.Case === "MapOne") {
	            var c = comparer.Compare(k, m.Fields[0]);
	            if (c < 0) {
	                return new MapTree("MapNode", [k, v, new MapTree("MapEmpty", []), m, 2]);
	            } else if (c === 0) {
	                return new MapTree("MapOne", [k, v]);
	            }
	            return new MapTree("MapNode", [k, v, m, new MapTree("MapEmpty", []), 2]);
	        } else if (m.Case === "MapNode") {
	            var c = comparer.Compare(k, m.Fields[0]);
	            if (c < 0) {
	                return tree_rebalance(tree_add(comparer, k, v, m.Fields[2]), m.Fields[0], m.Fields[1], m.Fields[3]);
	            } else if (c === 0) {
	                return new MapTree("MapNode", [k, v, m.Fields[2], m.Fields[3], m.Fields[4]]);
	            }
	            return tree_rebalance(m.Fields[2], m.Fields[0], m.Fields[1], tree_add(comparer, k, v, m.Fields[3]));
	        }
	        return new MapTree("MapOne", [k, v]);
	    }
	    function tree_find(comparer, k, m) {
	        var res = tree_tryFind(comparer, k, m);
	        if (res != null) return res;
	        throw new Error("key not found");
	    }
	    function tree_tryFind(comparer, k, m) {
	        if (m.Case === "MapOne") {
	            var c = comparer.Compare(k, m.Fields[0]);
	            return c === 0 ? m.Fields[1] : null;
	        } else if (m.Case === "MapNode") {
	            var c = comparer.Compare(k, m.Fields[0]);
	            if (c < 0) {
	                return tree_tryFind(comparer, k, m.Fields[2]);
	            } else {
	                if (c === 0) {
	                    return m.Fields[1];
	                } else {
	                    return tree_tryFind(comparer, k, m.Fields[3]);
	                }
	            }
	        }
	        return null;
	    }
	    function tree_partition1(comparer, f, k, v, acc1, acc2) {
	        return f(k, v) ? [tree_add(comparer, k, v, acc1), acc2] : [acc1, tree_add(comparer, k, v, acc2)];
	    }
	    function tree_partitionAux(comparer, f, s, acc_0, acc_1) {
	        var acc = [acc_0, acc_1];
	        if (s.Case === "MapOne") {
	            return tree_partition1(comparer, f, s.Fields[0], s.Fields[1], acc[0], acc[1]);
	        } else if (s.Case === "MapNode") {
	            var acc_2 = tree_partitionAux(comparer, f, s.Fields[3], acc[0], acc[1]);
	            var acc_3 = tree_partition1(comparer, f, s.Fields[0], s.Fields[1], acc_2[0], acc_2[1]);
	            return tree_partitionAux(comparer, f, s.Fields[2], acc_3[0], acc_3[1]);
	        }
	        return acc;
	    }
	    function tree_partition(comparer, f, s) {
	        return tree_partitionAux(comparer, f, s, tree_empty(), tree_empty());
	    }
	    function tree_filter1(comparer, f, k, v, acc) {
	        return f(k, v) ? tree_add(comparer, k, v, acc) : acc;
	    }
	    function tree_filterAux(comparer, f, s, acc) {
	        return s.Case === "MapOne" ? tree_filter1(comparer, f, s.Fields[0], s.Fields[1], acc) : s.Case === "MapNode" ? tree_filterAux(comparer, f, s.Fields[3], tree_filter1(comparer, f, s.Fields[0], s.Fields[1], tree_filterAux(comparer, f, s.Fields[2], acc))) : acc;
	    }
	    function tree_filter(comparer, f, s) {
	        return tree_filterAux(comparer, f, s, tree_empty());
	    }
	    function tree_spliceOutSuccessor(m) {
	        if (m.Case === "MapOne") {
	            return [m.Fields[0], m.Fields[1], new MapTree("MapEmpty", [])];
	        } else if (m.Case === "MapNode") {
	            if (m.Fields[2].Case === "MapEmpty") {
	                return [m.Fields[0], m.Fields[1], m.Fields[3]];
	            } else {
	                var kvl = tree_spliceOutSuccessor(m.Fields[2]);
	                return [kvl[0], kvl[1], tree_mk(kvl[2], m.Fields[0], m.Fields[1], m.Fields[3])];
	            }
	        }
	        throw new Error("internal error: Map.spliceOutSuccessor");
	    }
	    function tree_remove(comparer, k, m) {
	        if (m.Case === "MapOne") {
	            var c = comparer.Compare(k, m.Fields[0]);
	            if (c === 0) {
	                return new MapTree("MapEmpty", []);
	            } else {
	                return m;
	            }
	        } else if (m.Case === "MapNode") {
	            var c = comparer.Compare(k, m.Fields[0]);
	            if (c < 0) {
	                return tree_rebalance(tree_remove(comparer, k, m.Fields[2]), m.Fields[0], m.Fields[1], m.Fields[3]);
	            } else {
	                if (c === 0) {
	                    var matchValue = [m.Fields[2], m.Fields[3]];
	                    if (matchValue[0].Case === "MapEmpty") {
	                        return m.Fields[3];
	                    } else {
	                        if (matchValue[1].Case === "MapEmpty") {
	                            return m.Fields[2];
	                        } else {
	                            var patternInput = tree_spliceOutSuccessor(m.Fields[3]);
	                            var sv = patternInput[1];
	                            var sk = patternInput[0];
	                            var r_ = patternInput[2];
	                            return tree_mk(m.Fields[2], sk, sv, r_);
	                        }
	                    }
	                } else {
	                    return tree_rebalance(m.Fields[2], m.Fields[0], m.Fields[1], tree_remove(comparer, k, m.Fields[3]));
	                }
	            }
	        } else {
	            return tree_empty();
	        }
	    }
	    function tree_mem(comparer, k, m) {
	        if (m.Case === "MapOne") {
	            return comparer.Compare(k, m.Fields[0]) === 0;
	        } else if (m.Case === "MapNode") {
	            var c = comparer.Compare(k, m.Fields[0]);
	            if (c < 0) {
	                return tree_mem(comparer, k, m.Fields[2]);
	            } else {
	                if (c === 0) {
	                    return true;
	                } else {
	                    return tree_mem(comparer, k, m.Fields[3]);
	                }
	            }
	        } else {
	            return false;
	        }
	    }
	    function tree_iter(f, m) {
	        if (m.Case === "MapOne") {
	            f(m.Fields[0], m.Fields[1]);
	        } else if (m.Case === "MapNode") {
	            tree_iter(f, m.Fields[2]);
	            f(m.Fields[0], m.Fields[1]);
	            tree_iter(f, m.Fields[3]);
	        }
	    }
	    function tree_tryPick(f, m) {
	        if (m.Case === "MapOne") {
	            return f(m.Fields[0], m.Fields[1]);
	        } else if (m.Case === "MapNode") {
	            var matchValue = tree_tryPick(f, m.Fields[2]);
	            if (matchValue == null) {
	                var matchValue_1 = f(m.Fields[0], m.Fields[1]);
	                if (matchValue_1 == null) {
	                    return tree_tryPick(f, m.Fields[3]);
	                } else {
	                    var res = matchValue_1;
	                    return res;
	                }
	            } else {
	                var res = matchValue;
	                return res;
	            }
	        } else {
	            return null;
	        }
	    }
	    function tree_exists(f, m) {
	        return m.Case === "MapOne" ? f(m.Fields[0], m.Fields[1]) : m.Case === "MapNode" ? (tree_exists(f, m.Fields[2]) ? true : f(m.Fields[0], m.Fields[1])) ? true : tree_exists(f, m.Fields[3]) : false;
	    }
	    function tree_forall(f, m) {
	        return m.Case === "MapOne" ? f(m.Fields[0], m.Fields[1]) : m.Case === "MapNode" ? (tree_forall(f, m.Fields[2]) ? f(m.Fields[0], m.Fields[1]) : false) ? tree_forall(f, m.Fields[3]) : false : true;
	    }
	    function tree_mapi(f, m) {
	        return m.Case === "MapOne" ? new MapTree("MapOne", [m.Fields[0], f(m.Fields[0], m.Fields[1])]) : m.Case === "MapNode" ? new MapTree("MapNode", [m.Fields[0], f(m.Fields[0], m.Fields[1]), tree_mapi(f, m.Fields[2]), tree_mapi(f, m.Fields[3]), m.Fields[4]]) : tree_empty();
	    }
	    function tree_foldBack(f, m, x) {
	        return m.Case === "MapOne" ? f(m.Fields[0], m.Fields[1], x) : m.Case === "MapNode" ? tree_foldBack(f, m.Fields[2], f(m.Fields[0], m.Fields[1], tree_foldBack(f, m.Fields[3], x))) : x;
	    }
	    function tree_fold(f, x, m) {
	        return m.Case === "MapOne" ? f(x, m.Fields[0], m.Fields[1]) : m.Case === "MapNode" ? tree_fold(f, f(tree_fold(f, x, m.Fields[2]), m.Fields[0], m.Fields[1]), m.Fields[3]) : x;
	    }
	    function tree_mkFromEnumerator(comparer, acc, e) {
	        var cur = e.next();
	        while (!cur.done) {
	            acc = tree_add(comparer, cur.value[0], cur.value[1], acc);
	            cur = e.next();
	        }
	        return acc;
	    }
	    function tree_ofSeq(comparer, c) {
	        var ie = c[Symbol.iterator]();
	        return tree_mkFromEnumerator(comparer, tree_empty(), ie);
	    }
	    function tree_collapseLHS(stack) {
	        if (stack.tail != null) {
	            if (stack.head.Case === "MapOne") {
	                return stack;
	            } else if (stack.head.Case === "MapNode") {
	                return tree_collapseLHS(ListClass_2.ofArray([stack.head.Fields[2], new MapTree("MapOne", [stack.head.Fields[0], stack.head.Fields[1]]), stack.head.Fields[3]], stack.tail));
	            } else {
	                return tree_collapseLHS(stack.tail);
	            }
	        } else {
	            return new ListClass_1.default();
	        }
	    }
	    function tree_mkIterator(s) {
	        return { stack: tree_collapseLHS(new ListClass_1.default(s, new ListClass_1.default())), started: false };
	    }
	    function tree_moveNext(i) {
	        function current(i) {
	            if (i.stack.tail == null) {
	                return null;
	            } else if (i.stack.head.Case === "MapOne") {
	                return [i.stack.head.Fields[0], i.stack.head.Fields[1]];
	            }
	            throw new Error("Please report error: Map iterator, unexpected stack for current");
	        }
	        if (i.started) {
	            if (i.stack.tail == null) {
	                return { done: true, value: null };
	            } else {
	                if (i.stack.head.Case === "MapOne") {
	                    i.stack = tree_collapseLHS(i.stack.tail);
	                    return {
	                        done: i.stack.tail == null,
	                        value: current(i)
	                    };
	                } else {
	                    throw new Error("Please report error: Map iterator, unexpected stack for moveNext");
	                }
	            }
	        } else {
	            i.started = true;
	            return {
	                done: i.stack.tail == null,
	                value: current(i)
	            };
	        }
	        ;
	    }
	    var FableMap = function () {
	        function FableMap() {}
	        FableMap.prototype.ToString = function () {
	            return "map [" + Array.from(this).map(Util_1.toString).join("; ") + "]";
	        };
	        FableMap.prototype.Equals = function (m2) {
	            return this.CompareTo(m2) === 0;
	        };
	        FableMap.prototype.CompareTo = function (m2) {
	            var _this = this;
	            return this === m2 ? 0 : Seq_5.compareWith(function (kvp1, kvp2) {
	                var c = _this.comparer.Compare(kvp1[0], kvp2[0]);
	                return c !== 0 ? c : Util_3.compare(kvp1[1], kvp2[1]);
	            }, this, m2);
	        };
	        FableMap.prototype[Symbol.iterator] = function () {
	            var i = tree_mkIterator(this.tree);
	            return {
	                next: function next() {
	                    return tree_moveNext(i);
	                }
	            };
	        };
	        FableMap.prototype.entries = function () {
	            return this[Symbol.iterator]();
	        };
	        FableMap.prototype.keys = function () {
	            return Seq_1.map(function (kv) {
	                return kv[0];
	            }, this);
	        };
	        FableMap.prototype.values = function () {
	            return Seq_1.map(function (kv) {
	                return kv[1];
	            }, this);
	        };
	        FableMap.prototype.get = function (k) {
	            return tree_find(this.comparer, k, this.tree);
	        };
	        FableMap.prototype.has = function (k) {
	            return tree_mem(this.comparer, k, this.tree);
	        };
	        FableMap.prototype.set = function (k, v) {
	            throw new Error("not supported");
	        };
	        FableMap.prototype.delete = function (k) {
	            throw new Error("not supported");
	        };
	        FableMap.prototype.clear = function () {
	            throw new Error("not supported");
	        };
	        Object.defineProperty(FableMap.prototype, "size", {
	            get: function get() {
	                return tree_size(this.tree);
	            },
	            enumerable: true,
	            configurable: true
	        });
	        FableMap.prototype[Symbol_1.default.reflection] = function () {
	            return {
	                type: "Microsoft.FSharp.Collections.FSharpMap",
	                interfaces: ["System.IEquatable", "System.IComparable", "System.Collections.Generic.IDictionary"]
	            };
	        };
	        return FableMap;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = FableMap;
	    function from(comparer, tree) {
	        var map = new FableMap();
	        map.tree = tree;
	        map.comparer = comparer || new GenericComparer_1.default();
	        return map;
	    }
	    function create(ie, comparer) {
	        comparer = comparer || new GenericComparer_1.default();
	        return from(comparer, ie ? tree_ofSeq(comparer, ie) : tree_empty());
	    }
	    exports.create = create;
	    function add(k, v, map) {
	        return from(map.comparer, tree_add(map.comparer, k, v, map.tree));
	    }
	    exports.add = add;
	    function remove(item, map) {
	        return from(map.comparer, tree_remove(map.comparer, item, map.tree));
	    }
	    exports.remove = remove;
	    function containsValue(v, map) {
	        return Seq_2.fold(function (acc, k) {
	            return acc || Util_2.equals(map.get(k), v);
	        }, false, map.keys());
	    }
	    exports.containsValue = containsValue;
	    function tryGetValue(map, key, defaultValue) {
	        return map.has(key) ? [true, map.get(key)] : [false, defaultValue];
	    }
	    exports.tryGetValue = tryGetValue;
	    function exists(f, map) {
	        return tree_exists(f, map.tree);
	    }
	    exports.exists = exists;
	    function find(k, map) {
	        return tree_find(map.comparer, k, map.tree);
	    }
	    exports.find = find;
	    function tryFind(k, map) {
	        return tree_tryFind(map.comparer, k, map.tree);
	    }
	    exports.tryFind = tryFind;
	    function filter(f, map) {
	        return from(map.comparer, tree_filter(map.comparer, f, map.tree));
	    }
	    exports.filter = filter;
	    function fold(f, seed, map) {
	        return tree_fold(f, seed, map.tree);
	    }
	    exports.fold = fold;
	    function foldBack(f, map, seed) {
	        return tree_foldBack(f, map.tree, seed);
	    }
	    exports.foldBack = foldBack;
	    function forAll(f, map) {
	        return tree_forall(f, map.tree);
	    }
	    exports.forAll = forAll;
	    function isEmpty(map) {
	        return tree_isEmpty(map.tree);
	    }
	    exports.isEmpty = isEmpty;
	    function iterate(f, map) {
	        tree_iter(f, map.tree);
	    }
	    exports.iterate = iterate;
	    function map(f, map) {
	        return from(map.comparer, tree_mapi(f, map.tree));
	    }
	    exports.map = map;
	    function partition(f, map) {
	        var rs = tree_partition(map.comparer, f, map.tree);
	        return [from(map.comparer, rs[0]), from(map.comparer, rs[1])];
	    }
	    exports.partition = partition;
	    function findKey(f, map) {
	        return Seq_3.pick(function (kv) {
	            return f(kv[0], kv[1]) ? kv[0] : null;
	        }, map);
	    }
	    exports.findKey = findKey;
	    function tryFindKey(f, map) {
	        return Seq_4.tryPick(function (kv) {
	            return f(kv[0], kv[1]) ? kv[0] : null;
	        }, map);
	    }
	    exports.tryFindKey = tryFindKey;
	    function pick(f, map) {
	        var res = tryPick(f, map);
	        if (res != null) return res;
	        throw new Error("key not found");
	    }
	    exports.pick = pick;
	    function tryPick(f, map) {
	        return tree_tryPick(f, map.tree);
	    }
	    exports.tryPick = tryPick;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 26 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Util", "./Symbol"], function (require, exports) {
	    "use strict";
	
	    var Util_1 = require("./Util");
	    var Symbol_1 = require("./Symbol");
	    var Observer = function () {
	        function Observer(onNext, onError, onCompleted) {
	            this.OnNext = onNext;
	            this.OnError = onError || function (e) {};
	            this.OnCompleted = onCompleted || function () {};
	        }
	        Observer.prototype[Symbol_1.default.reflection] = function () {
	            return { interfaces: ["System.IObserver"] };
	        };
	        return Observer;
	    }();
	    exports.Observer = Observer;
	    var Observable = function () {
	        function Observable(subscribe) {
	            this.Subscribe = subscribe;
	        }
	        Observable.prototype[Symbol_1.default.reflection] = function () {
	            return { interfaces: ["System.IObservable"] };
	        };
	        return Observable;
	    }();
	    function protect(f, succeed, fail) {
	        try {
	            return succeed(f());
	        } catch (e) {
	            fail(e);
	        }
	    }
	    exports.protect = protect;
	    function add(callback, source) {
	        source.Subscribe(new Observer(callback));
	    }
	    exports.add = add;
	    function choose(chooser, source) {
	        return new Observable(function (observer) {
	            return source.Subscribe(new Observer(function (t) {
	                return protect(function () {
	                    return chooser(t);
	                }, function (u) {
	                    if (u != null) observer.OnNext(u);
	                }, observer.OnError);
	            }, observer.OnError, observer.OnCompleted));
	        });
	    }
	    exports.choose = choose;
	    function filter(predicate, source) {
	        return choose(function (x) {
	            return predicate(x) ? x : null;
	        }, source);
	    }
	    exports.filter = filter;
	    function map(mapping, source) {
	        return new Observable(function (observer) {
	            return source.Subscribe(new Observer(function (t) {
	                protect(function () {
	                    return mapping(t);
	                }, observer.OnNext, observer.OnError);
	            }, observer.OnError, observer.OnCompleted));
	        });
	    }
	    exports.map = map;
	    function merge(source1, source2) {
	        return new Observable(function (observer) {
	            var stopped = false,
	                completed1 = false,
	                completed2 = false;
	            var h1 = source1.Subscribe(new Observer(function (v) {
	                if (!stopped) observer.OnNext(v);
	            }, function (e) {
	                if (!stopped) {
	                    stopped = true;
	                    observer.OnError(e);
	                }
	            }, function () {
	                if (!stopped) {
	                    completed1 = true;
	                    if (completed2) {
	                        stopped = true;
	                        observer.OnCompleted();
	                    }
	                }
	            }));
	            var h2 = source2.Subscribe(new Observer(function (v) {
	                if (!stopped) {
	                    observer.OnNext(v);
	                }
	            }, function (e) {
	                if (!stopped) {
	                    stopped = true;
	                    observer.OnError(e);
	                }
	            }, function () {
	                if (!stopped) {
	                    completed2 = true;
	                    if (completed1) {
	                        stopped = true;
	                        observer.OnCompleted();
	                    }
	                }
	            }));
	            return Util_1.createDisposable(function () {
	                h1.Dispose();
	                h2.Dispose();
	            });
	        });
	    }
	    exports.merge = merge;
	    function pairwise(source) {
	        return new Observable(function (observer) {
	            var last = null;
	            return source.Subscribe(new Observer(function (next) {
	                if (last != null) observer.OnNext([last, next]);
	                last = next;
	            }, observer.OnError, observer.OnCompleted));
	        });
	    }
	    exports.pairwise = pairwise;
	    function partition(predicate, source) {
	        return [filter(predicate, source), filter(function (x) {
	            return !predicate(x);
	        }, source)];
	    }
	    exports.partition = partition;
	    function scan(collector, state, source) {
	        return new Observable(function (observer) {
	            return source.Subscribe(new Observer(function (t) {
	                protect(function () {
	                    return collector(state, t);
	                }, function (u) {
	                    state = u;observer.OnNext(u);
	                }, observer.OnError);
	            }, observer.OnError, observer.OnCompleted));
	        });
	    }
	    exports.scan = scan;
	    function split(splitter, source) {
	        return [choose(function (v) {
	            return splitter(v).valueIfChoice1;
	        }, source), choose(function (v) {
	            return splitter(v).valueIfChoice2;
	        }, source)];
	    }
	    exports.split = split;
	    function subscribe(callback, source) {
	        return source.Subscribe(new Observer(callback));
	    }
	    exports.subscribe = subscribe;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 27 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Util", "./List", "./Symbol"], function (require, exports) {
	    "use strict";
	
	    var Util_1 = require("./Util");
	    var List_1 = require("./List");
	    var Symbol_1 = require("./Symbol");
	    function resolveGeneric(idx, enclosing) {
	        try {
	            var t = enclosing.head;
	            if (t.generics == null) {
	                return resolveGeneric(idx, enclosing.tail);
	            } else {
	                var name_1 = typeof idx === "string" ? idx : Object.getOwnPropertyNames(t.generics)[idx];
	                var resolved = t.generics[name_1];
	                if (resolved == null) {
	                    return resolveGeneric(idx, enclosing.tail);
	                } else if (resolved instanceof Util_1.NonDeclaredType && resolved.kind === "GenericParam") {
	                    return resolveGeneric(resolved.definition, enclosing.tail);
	                } else {
	                    return new List_1.default(resolved, enclosing);
	                }
	            }
	        } catch (err) {
	            throw new Error("Cannot resolve generic argument " + idx + ": " + err);
	        }
	    }
	    exports.resolveGeneric = resolveGeneric;
	    function getType(obj) {
	        var t = typeof obj === 'undefined' ? 'undefined' : _typeof(obj);
	        switch (t) {
	            case "boolean":
	            case "number":
	            case "string":
	            case "function":
	                return t;
	            default:
	                return Object.getPrototypeOf(obj).constructor;
	        }
	    }
	    exports.getType = getType;
	    function getTypeFullName(typ, option) {
	        function trim(fullName, option) {
	            if (typeof fullName !== "string") {
	                return "unknown";
	            }
	            if (option === "name") {
	                var i = fullName.lastIndexOf('.');
	                return fullName.substr(i + 1);
	            }
	            if (option === "namespace") {
	                var i = fullName.lastIndexOf('.');
	                return i > -1 ? fullName.substr(0, i) : "";
	            }
	            return fullName;
	        }
	        if (typeof typ === "string") {
	            return typ;
	        } else if (typ instanceof Util_1.NonDeclaredType) {
	            switch (typ.kind) {
	                case "Unit":
	                    return "unit";
	                case "Option":
	                    return getTypeFullName(typ.generics, option) + " option";
	                case "Array":
	                    return getTypeFullName(typ.generics, option) + "[]";
	                case "Tuple":
	                    return typ.generics.map(function (x) {
	                        return getTypeFullName(x, option);
	                    }).join(" * ");
	                case "GenericParam":
	                case "Interface":
	                    return typ.definition;
	                case "Any":
	                default:
	                    return "unknown";
	            }
	        } else {
	            var proto = typ.prototype;
	            return trim(typeof proto[Symbol_1.default.reflection] === "function" ? proto[Symbol_1.default.reflection]().type : null, option);
	        }
	    }
	    exports.getTypeFullName = getTypeFullName;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 28 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports"], function (require, exports) {
	    "use strict";
	
	    function create(pattern, options) {
	        var flags = "g";
	        flags += options & 1 ? "i" : "";
	        flags += options & 2 ? "m" : "";
	        return new RegExp(pattern, flags);
	    }
	    exports.create = create;
	    function escape(str) {
	        return str.replace(/[\-\[\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
	    }
	    exports.escape = escape;
	    function unescape(str) {
	        return str.replace(/\\([\-\[\/\{\}\(\)\*\+\?\.\\\^\$\|])/g, "$1");
	    }
	    exports.unescape = unescape;
	    function isMatch(str, pattern, options) {
	        if (options === void 0) {
	            options = 0;
	        }
	        var reg = str instanceof RegExp ? (reg = str, str = pattern, reg.lastIndex = options, reg) : reg = create(pattern, options);
	        return reg.test(str);
	    }
	    exports.isMatch = isMatch;
	    function match(str, pattern, options) {
	        if (options === void 0) {
	            options = 0;
	        }
	        var reg = str instanceof RegExp ? (reg = str, str = pattern, reg.lastIndex = options, reg) : reg = create(pattern, options);
	        return reg.exec(str);
	    }
	    exports.match = match;
	    function matches(str, pattern, options) {
	        if (options === void 0) {
	            options = 0;
	        }
	        var reg = str instanceof RegExp ? (reg = str, str = pattern, reg.lastIndex = options, reg) : reg = create(pattern, options);
	        if (!reg.global) throw new Error("Non-global RegExp");
	        var m;
	        var matches = [];
	        while ((m = reg.exec(str)) !== null) {
	            matches.push(m);
	        }return matches;
	    }
	    exports.matches = matches;
	    function options(reg) {
	        var options = 256;
	        options |= reg.ignoreCase ? 1 : 0;
	        options |= reg.multiline ? 2 : 0;
	        return options;
	    }
	    exports.options = options;
	    function replace(reg, input, replacement, limit, offset) {
	        if (offset === void 0) {
	            offset = 0;
	        }
	        function replacer() {
	            var res = arguments[0];
	            if (limit !== 0) {
	                limit--;
	                var match_1 = [];
	                var len = arguments.length;
	                for (var i = 0; i < len - 2; i++) {
	                    match_1.push(arguments[i]);
	                }match_1.index = arguments[len - 2];
	                match_1.input = arguments[len - 1];
	                res = replacement(match_1);
	            }
	            return res;
	        }
	        if (typeof reg == "string") {
	            var tmp = reg;
	            reg = create(input, limit);
	            input = tmp;
	            limit = undefined;
	        }
	        if (typeof replacement == "function") {
	            limit = limit == null ? -1 : limit;
	            return input.substring(0, offset) + input.substring(offset).replace(reg, replacer);
	        } else {
	            if (limit != null) {
	                var m = void 0;
	                var sub1 = input.substring(offset);
	                var _matches = matches(reg, sub1);
	                var sub2 = matches.length > limit ? (m = _matches[limit - 1], sub1.substring(0, m.index + m[0].length)) : sub1;
	                return input.substring(0, offset) + sub2.replace(reg, replacement) + input.substring(offset + sub2.length);
	            } else {
	                return input.replace(reg, replacement);
	            }
	        }
	    }
	    exports.replace = replace;
	    function split(reg, input, limit, offset) {
	        if (offset === void 0) {
	            offset = 0;
	        }
	        if (typeof reg == "string") {
	            var tmp = reg;
	            reg = create(input, limit);
	            input = tmp;
	            limit = undefined;
	        }
	        input = input.substring(offset);
	        return input.split(reg, limit);
	    }
	    exports.split = split;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 29 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Util", "./Util", "./Array", "./ListClass"], function (require, exports) {
	    "use strict";
	
	    var Util_1 = require("./Util");
	    var Util_2 = require("./Util");
	    var Array_1 = require("./Array");
	    var ListClass_1 = require("./ListClass");
	    var Enumerator = function () {
	        function Enumerator(iter) {
	            this.iter = iter;
	        }
	        Enumerator.prototype.MoveNext = function () {
	            var cur = this.iter.next();
	            this.current = cur.value;
	            return !cur.done;
	        };
	        Object.defineProperty(Enumerator.prototype, "Current", {
	            get: function get() {
	                return this.current;
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Enumerator.prototype.Reset = function () {
	            throw new Error("JS iterators cannot be reset");
	        };
	        Enumerator.prototype.Dispose = function () {};
	        return Enumerator;
	    }();
	    exports.Enumerator = Enumerator;
	    function getEnumerator(o) {
	        return typeof o.GetEnumerator === "function" ? o.GetEnumerator() : new Enumerator(o[Symbol.iterator]());
	    }
	    exports.getEnumerator = getEnumerator;
	    function toIterator(en) {
	        return {
	            next: function next() {
	                return en.MoveNext() ? { done: false, value: en.Current } : { done: true, value: null };
	            }
	        };
	    }
	    exports.toIterator = toIterator;
	    function __failIfNone(res) {
	        if (res == null) throw new Error("Seq did not contain any matching element");
	        return res;
	    }
	    function toList(xs) {
	        return foldBack(function (x, acc) {
	            return new ListClass_1.default(x, acc);
	        }, xs, new ListClass_1.default());
	    }
	    exports.toList = toList;
	    function ofList(xs) {
	        return delay(function () {
	            return unfold(function (x) {
	                return x.tail != null ? [x.head, x.tail] : null;
	            }, xs);
	        });
	    }
	    exports.ofList = ofList;
	    function ofArray(xs) {
	        return delay(function () {
	            return unfold(function (i) {
	                return i < xs.length ? [xs[i], i + 1] : null;
	            }, 0);
	        });
	    }
	    exports.ofArray = ofArray;
	    function append(xs, ys) {
	        return delay(function () {
	            var firstDone = false;
	            var i = xs[Symbol.iterator]();
	            var iters = [i, null];
	            return unfold(function () {
	                var cur;
	                if (!firstDone) {
	                    cur = iters[0].next();
	                    if (!cur.done) {
	                        return [cur.value, iters];
	                    } else {
	                        firstDone = true;
	                        iters = [null, ys[Symbol.iterator]()];
	                    }
	                }
	                cur = iters[1].next();
	                return !cur.done ? [cur.value, iters] : null;
	            }, iters);
	        });
	    }
	    exports.append = append;
	    function average(xs) {
	        var count = 1;
	        var sum = reduce(function (acc, x) {
	            count++;
	            return acc + x;
	        }, xs);
	        return sum / count;
	    }
	    exports.average = average;
	    function averageBy(f, xs) {
	        var count = 1;
	        var sum = reduce(function (acc, x) {
	            count++;
	            return (count === 2 ? f(acc) : acc) + f(x);
	        }, xs);
	        return sum / count;
	    }
	    exports.averageBy = averageBy;
	    function concat(xs) {
	        return delay(function () {
	            var iter = xs[Symbol.iterator]();
	            var output = null;
	            return unfold(function (innerIter) {
	                var hasFinished = false;
	                while (!hasFinished) {
	                    if (innerIter == null) {
	                        var cur = iter.next();
	                        if (!cur.done) {
	                            innerIter = cur.value[Symbol.iterator]();
	                        } else {
	                            hasFinished = true;
	                        }
	                    } else {
	                        var cur = innerIter.next();
	                        if (!cur.done) {
	                            output = cur.value;
	                            hasFinished = true;
	                        } else {
	                            innerIter = null;
	                        }
	                    }
	                }
	                return innerIter != null && output != null ? [output, innerIter] : null;
	            }, null);
	        });
	    }
	    exports.concat = concat;
	    function collect(f, xs) {
	        return concat(map(f, xs));
	    }
	    exports.collect = collect;
	    function choose(f, xs) {
	        var trySkipToNext = function trySkipToNext(iter) {
	            var cur = iter.next();
	            if (!cur.done) {
	                var y = f(cur.value);
	                return y != null ? [y, iter] : trySkipToNext(iter);
	            }
	            return void 0;
	        };
	        return delay(function () {
	            return unfold(function (iter) {
	                return trySkipToNext(iter);
	            }, xs[Symbol.iterator]());
	        });
	    }
	    exports.choose = choose;
	    function compareWith(f, xs, ys) {
	        var nonZero = tryFind(function (i) {
	            return i != 0;
	        }, map2(function (x, y) {
	            return f(x, y);
	        }, xs, ys));
	        return nonZero != null ? nonZero : count(xs) - count(ys);
	    }
	    exports.compareWith = compareWith;
	    function delay(f) {
	        return _a = {}, _a[Symbol.iterator] = function () {
	            return f()[Symbol.iterator]();
	        }, _a;
	        var _a;
	    }
	    exports.delay = delay;
	    function empty() {
	        return unfold(function () {
	            return void 0;
	        });
	    }
	    exports.empty = empty;
	    function enumerateWhile(cond, xs) {
	        return concat(unfold(function () {
	            return cond() ? [xs, true] : null;
	        }));
	    }
	    exports.enumerateWhile = enumerateWhile;
	    function enumerateThenFinally(xs, finalFn) {
	        return delay(function () {
	            var iter;
	            try {
	                iter = xs[Symbol.iterator]();
	            } catch (err) {
	                return void 0;
	            } finally {
	                finalFn();
	            }
	            return unfold(function (iter) {
	                try {
	                    var cur = iter.next();
	                    return !cur.done ? [cur.value, iter] : null;
	                } catch (err) {
	                    return void 0;
	                } finally {
	                    finalFn();
	                }
	            }, iter);
	        });
	    }
	    exports.enumerateThenFinally = enumerateThenFinally;
	    function enumerateUsing(disp, work) {
	        var isDisposed = false;
	        var disposeOnce = function disposeOnce() {
	            if (!isDisposed) {
	                isDisposed = true;
	                disp.Dispose();
	            }
	        };
	        try {
	            return enumerateThenFinally(work(disp), disposeOnce);
	        } catch (err) {
	            return void 0;
	        } finally {
	            disposeOnce();
	        }
	    }
	    exports.enumerateUsing = enumerateUsing;
	    function exactlyOne(xs) {
	        var iter = xs[Symbol.iterator]();
	        var fst = iter.next();
	        if (fst.done) throw new Error("Seq was empty");
	        var snd = iter.next();
	        if (!snd.done) throw new Error("Seq had multiple items");
	        return fst.value;
	    }
	    exports.exactlyOne = exactlyOne;
	    function except(itemsToExclude, source) {
	        var exclusionItems = Array.from(itemsToExclude);
	        var testIsNotInExclusionItems = function testIsNotInExclusionItems(element) {
	            return !exclusionItems.some(function (excludedItem) {
	                return Util_1.equals(excludedItem, element);
	            });
	        };
	        return filter(testIsNotInExclusionItems, source);
	    }
	    exports.except = except;
	    function exists(f, xs) {
	        function aux(iter) {
	            var cur = iter.next();
	            return !cur.done && (f(cur.value) || aux(iter));
	        }
	        return aux(xs[Symbol.iterator]());
	    }
	    exports.exists = exists;
	    function exists2(f, xs, ys) {
	        function aux(iter1, iter2) {
	            var cur1 = iter1.next(),
	                cur2 = iter2.next();
	            return !cur1.done && !cur2.done && (f(cur1.value, cur2.value) || aux(iter1, iter2));
	        }
	        return aux(xs[Symbol.iterator](), ys[Symbol.iterator]());
	    }
	    exports.exists2 = exists2;
	    function filter(f, xs) {
	        function trySkipToNext(iter) {
	            var cur = iter.next();
	            while (!cur.done) {
	                if (f(cur.value)) {
	                    return [cur.value, iter];
	                }
	                cur = iter.next();
	            }
	            return void 0;
	        }
	        return delay(function () {
	            return unfold(trySkipToNext, xs[Symbol.iterator]());
	        });
	    }
	    exports.filter = filter;
	    function where(f, xs) {
	        return filter(f, xs);
	    }
	    exports.where = where;
	    function fold(f, acc, xs) {
	        if (Array.isArray(xs) || ArrayBuffer.isView(xs)) {
	            return xs.reduce(f, acc);
	        } else {
	            var cur = void 0;
	            for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
	                cur = iter.next();
	                if (cur.done) break;
	                acc = f(acc, cur.value, i);
	            }
	            return acc;
	        }
	    }
	    exports.fold = fold;
	    function foldBack(f, xs, acc) {
	        var arr = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs : Array.from(xs);
	        for (var i = arr.length - 1; i >= 0; i--) {
	            acc = f(arr[i], acc, i);
	        }
	        return acc;
	    }
	    exports.foldBack = foldBack;
	    function fold2(f, acc, xs, ys) {
	        var iter1 = xs[Symbol.iterator](),
	            iter2 = ys[Symbol.iterator]();
	        var cur1, cur2;
	        for (var i = 0;; i++) {
	            cur1 = iter1.next();
	            cur2 = iter2.next();
	            if (cur1.done || cur2.done) {
	                break;
	            }
	            acc = f(acc, cur1.value, cur2.value, i);
	        }
	        return acc;
	    }
	    exports.fold2 = fold2;
	    function foldBack2(f, xs, ys, acc) {
	        var ar1 = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs : Array.from(xs);
	        var ar2 = Array.isArray(ys) || ArrayBuffer.isView(ys) ? ys : Array.from(ys);
	        for (var i = ar1.length - 1; i >= 0; i--) {
	            acc = f(ar1[i], ar2[i], acc, i);
	        }
	        return acc;
	    }
	    exports.foldBack2 = foldBack2;
	    function forAll(f, xs) {
	        return fold(function (acc, x) {
	            return acc && f(x);
	        }, true, xs);
	    }
	    exports.forAll = forAll;
	    function forAll2(f, xs, ys) {
	        return fold2(function (acc, x, y) {
	            return acc && f(x, y);
	        }, true, xs, ys);
	    }
	    exports.forAll2 = forAll2;
	    function tryHead(xs) {
	        var iter = xs[Symbol.iterator]();
	        var cur = iter.next();
	        return cur.done ? null : cur.value;
	    }
	    exports.tryHead = tryHead;
	    function head(xs) {
	        return __failIfNone(tryHead(xs));
	    }
	    exports.head = head;
	    function initialize(n, f) {
	        return delay(function () {
	            return unfold(function (i) {
	                return i < n ? [f(i), i + 1] : null;
	            }, 0);
	        });
	    }
	    exports.initialize = initialize;
	    function initializeInfinite(f) {
	        return delay(function () {
	            return unfold(function (i) {
	                return [f(i), i + 1];
	            }, 0);
	        });
	    }
	    exports.initializeInfinite = initializeInfinite;
	    function tryItem(i, xs) {
	        if (i < 0) return null;
	        if (Array.isArray(xs) || ArrayBuffer.isView(xs)) return i < xs.length ? xs[i] : null;
	        for (var j = 0, iter = xs[Symbol.iterator]();; j++) {
	            var cur = iter.next();
	            if (cur.done) return null;
	            if (j === i) return cur.value;
	        }
	    }
	    exports.tryItem = tryItem;
	    function item(i, xs) {
	        return __failIfNone(tryItem(i, xs));
	    }
	    exports.item = item;
	    function iterate(f, xs) {
	        fold(function (_, x) {
	            return f(x);
	        }, null, xs);
	    }
	    exports.iterate = iterate;
	    function iterate2(f, xs, ys) {
	        fold2(function (_, x, y) {
	            return f(x, y);
	        }, null, xs, ys);
	    }
	    exports.iterate2 = iterate2;
	    function iterateIndexed(f, xs) {
	        fold(function (_, x, i) {
	            return f(i, x);
	        }, null, xs);
	    }
	    exports.iterateIndexed = iterateIndexed;
	    function iterateIndexed2(f, xs, ys) {
	        fold2(function (_, x, y, i) {
	            return f(i, x, y);
	        }, null, xs, ys);
	    }
	    exports.iterateIndexed2 = iterateIndexed2;
	    function isEmpty(xs) {
	        var i = xs[Symbol.iterator]();
	        return i.next().done;
	    }
	    exports.isEmpty = isEmpty;
	    function tryLast(xs) {
	        try {
	            return reduce(function (_, x) {
	                return x;
	            }, xs);
	        } catch (err) {
	            return null;
	        }
	    }
	    exports.tryLast = tryLast;
	    function last(xs) {
	        return __failIfNone(tryLast(xs));
	    }
	    exports.last = last;
	    function count(xs) {
	        return Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs.length : fold(function (acc, x) {
	            return acc + 1;
	        }, 0, xs);
	    }
	    exports.count = count;
	    function map(f, xs) {
	        return delay(function () {
	            return unfold(function (iter) {
	                var cur = iter.next();
	                return !cur.done ? [f(cur.value), iter] : null;
	            }, xs[Symbol.iterator]());
	        });
	    }
	    exports.map = map;
	    function mapIndexed(f, xs) {
	        return delay(function () {
	            var i = 0;
	            return unfold(function (iter) {
	                var cur = iter.next();
	                return !cur.done ? [f(i++, cur.value), iter] : null;
	            }, xs[Symbol.iterator]());
	        });
	    }
	    exports.mapIndexed = mapIndexed;
	    function map2(f, xs, ys) {
	        return delay(function () {
	            var iter1 = xs[Symbol.iterator]();
	            var iter2 = ys[Symbol.iterator]();
	            return unfold(function () {
	                var cur1 = iter1.next(),
	                    cur2 = iter2.next();
	                return !cur1.done && !cur2.done ? [f(cur1.value, cur2.value), null] : null;
	            });
	        });
	    }
	    exports.map2 = map2;
	    function mapIndexed2(f, xs, ys) {
	        return delay(function () {
	            var i = 0;
	            var iter1 = xs[Symbol.iterator]();
	            var iter2 = ys[Symbol.iterator]();
	            return unfold(function () {
	                var cur1 = iter1.next(),
	                    cur2 = iter2.next();
	                return !cur1.done && !cur2.done ? [f(i++, cur1.value, cur2.value), null] : null;
	            });
	        });
	    }
	    exports.mapIndexed2 = mapIndexed2;
	    function map3(f, xs, ys, zs) {
	        return delay(function () {
	            var iter1 = xs[Symbol.iterator]();
	            var iter2 = ys[Symbol.iterator]();
	            var iter3 = zs[Symbol.iterator]();
	            return unfold(function () {
	                var cur1 = iter1.next(),
	                    cur2 = iter2.next(),
	                    cur3 = iter3.next();
	                return !cur1.done && !cur2.done && !cur3.done ? [f(cur1.value, cur2.value, cur3.value), null] : null;
	            });
	        });
	    }
	    exports.map3 = map3;
	    function mapFold(f, acc, xs) {
	        var result = [];
	        var r;
	        var cur;
	        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
	            cur = iter.next();
	            if (cur.done) break;
	            _a = f(acc, cur.value), r = _a[0], acc = _a[1];
	            result.push(r);
	        }
	        return [result, acc];
	        var _a;
	    }
	    exports.mapFold = mapFold;
	    function mapFoldBack(f, xs, acc) {
	        var arr = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs : Array.from(xs);
	        var result = [];
	        var r;
	        for (var i = arr.length - 1; i >= 0; i--) {
	            _a = f(arr[i], acc), r = _a[0], acc = _a[1];
	            result.push(r);
	        }
	        return [result, acc];
	        var _a;
	    }
	    exports.mapFoldBack = mapFoldBack;
	    function max(xs) {
	        return reduce(function (acc, x) {
	            return Util_2.compare(acc, x) === 1 ? acc : x;
	        }, xs);
	    }
	    exports.max = max;
	    function maxBy(f, xs) {
	        return reduce(function (acc, x) {
	            return Util_2.compare(f(acc), f(x)) === 1 ? acc : x;
	        }, xs);
	    }
	    exports.maxBy = maxBy;
	    function min(xs) {
	        return reduce(function (acc, x) {
	            return Util_2.compare(acc, x) === -1 ? acc : x;
	        }, xs);
	    }
	    exports.min = min;
	    function minBy(f, xs) {
	        return reduce(function (acc, x) {
	            return Util_2.compare(f(acc), f(x)) === -1 ? acc : x;
	        }, xs);
	    }
	    exports.minBy = minBy;
	    function pairwise(xs) {
	        return skip(2, scan(function (last, next) {
	            return [last[1], next];
	        }, [0, 0], xs));
	    }
	    exports.pairwise = pairwise;
	    function permute(f, xs) {
	        return ofArray(Array_1.permute(f, Array.from(xs)));
	    }
	    exports.permute = permute;
	    function rangeStep(first, step, last) {
	        if (step === 0) throw new Error("Step cannot be 0");
	        return delay(function () {
	            return unfold(function (x) {
	                return step > 0 && x <= last || step < 0 && x >= last ? [x, x + step] : null;
	            }, first);
	        });
	    }
	    exports.rangeStep = rangeStep;
	    function rangeChar(first, last) {
	        return delay(function () {
	            return unfold(function (x) {
	                return x <= last ? [x, String.fromCharCode(x.charCodeAt(0) + 1)] : null;
	            }, first);
	        });
	    }
	    exports.rangeChar = rangeChar;
	    function range(first, last) {
	        return rangeStep(first, 1, last);
	    }
	    exports.range = range;
	    function readOnly(xs) {
	        return map(function (x) {
	            return x;
	        }, xs);
	    }
	    exports.readOnly = readOnly;
	    function reduce(f, xs) {
	        if (Array.isArray(xs) || ArrayBuffer.isView(xs)) return xs.reduce(f);
	        var iter = xs[Symbol.iterator]();
	        var cur = iter.next();
	        if (cur.done) throw new Error("Seq was empty");
	        var acc = cur.value;
	        for (;;) {
	            cur = iter.next();
	            if (cur.done) break;
	            acc = f(acc, cur.value);
	        }
	        return acc;
	    }
	    exports.reduce = reduce;
	    function reduceBack(f, xs) {
	        var ar = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs : Array.from(xs);
	        if (ar.length === 0) throw new Error("Seq was empty");
	        var acc = ar[ar.length - 1];
	        for (var i = ar.length - 2; i >= 0; i--) {
	            acc = f(ar[i], acc, i);
	        }return acc;
	    }
	    exports.reduceBack = reduceBack;
	    function replicate(n, x) {
	        return initialize(n, function () {
	            return x;
	        });
	    }
	    exports.replicate = replicate;
	    function reverse(xs) {
	        var ar = Array.isArray(xs) || ArrayBuffer.isView(xs) ? xs.slice(0) : Array.from(xs);
	        return ofArray(ar.reverse());
	    }
	    exports.reverse = reverse;
	    function scan(f, seed, xs) {
	        return delay(function () {
	            var iter = xs[Symbol.iterator]();
	            return unfold(function (acc) {
	                if (acc == null) return [seed, seed];
	                var cur = iter.next();
	                if (!cur.done) {
	                    acc = f(acc, cur.value);
	                    return [acc, acc];
	                }
	                return void 0;
	            }, null);
	        });
	    }
	    exports.scan = scan;
	    function scanBack(f, xs, seed) {
	        return reverse(scan(function (acc, x) {
	            return f(x, acc);
	        }, seed, reverse(xs)));
	    }
	    exports.scanBack = scanBack;
	    function singleton(x) {
	        return unfold(function (x) {
	            return x != null ? [x, null] : null;
	        }, x);
	    }
	    exports.singleton = singleton;
	    function skip(n, xs) {
	        return _a = {}, _a[Symbol.iterator] = function () {
	            var iter = xs[Symbol.iterator]();
	            for (var i = 1; i <= n; i++) {
	                if (iter.next().done) throw new Error("Seq has not enough elements");
	            }return iter;
	        }, _a;
	        var _a;
	    }
	    exports.skip = skip;
	    function skipWhile(f, xs) {
	        return delay(function () {
	            var hasPassed = false;
	            return filter(function (x) {
	                return hasPassed || (hasPassed = !f(x));
	            }, xs);
	        });
	    }
	    exports.skipWhile = skipWhile;
	    function sortWith(f, xs) {
	        var ys = Array.from(xs);
	        return ofArray(ys.sort(f));
	    }
	    exports.sortWith = sortWith;
	    function sum(xs) {
	        return fold(function (acc, x) {
	            return acc + x;
	        }, 0, xs);
	    }
	    exports.sum = sum;
	    function sumBy(f, xs) {
	        return fold(function (acc, x) {
	            return acc + f(x);
	        }, 0, xs);
	    }
	    exports.sumBy = sumBy;
	    function tail(xs) {
	        var iter = xs[Symbol.iterator]();
	        var cur = iter.next();
	        if (cur.done) throw new Error("Seq was empty");
	        return _a = {}, _a[Symbol.iterator] = function () {
	            return iter;
	        }, _a;
	        var _a;
	    }
	    exports.tail = tail;
	    function take(n, xs, truncate) {
	        if (truncate === void 0) {
	            truncate = false;
	        }
	        return delay(function () {
	            var iter = xs[Symbol.iterator]();
	            return unfold(function (i) {
	                if (i < n) {
	                    var cur = iter.next();
	                    if (!cur.done) return [cur.value, i + 1];
	                    if (!truncate) throw new Error("Seq has not enough elements");
	                }
	                return void 0;
	            }, 0);
	        });
	    }
	    exports.take = take;
	    function truncate(n, xs) {
	        return take(n, xs, true);
	    }
	    exports.truncate = truncate;
	    function takeWhile(f, xs) {
	        return delay(function () {
	            var iter = xs[Symbol.iterator]();
	            return unfold(function (i) {
	                var cur = iter.next();
	                if (!cur.done && f(cur.value)) return [cur.value, null];
	                return void 0;
	            }, 0);
	        });
	    }
	    exports.takeWhile = takeWhile;
	    function tryFind(f, xs, defaultValue) {
	        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
	            var cur = iter.next();
	            if (cur.done) return defaultValue === void 0 ? null : defaultValue;
	            if (f(cur.value, i)) return cur.value;
	        }
	    }
	    exports.tryFind = tryFind;
	    function find(f, xs) {
	        return __failIfNone(tryFind(f, xs));
	    }
	    exports.find = find;
	    function tryFindBack(f, xs, defaultValue) {
	        var match = null;
	        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
	            var cur = iter.next();
	            if (cur.done) return match === null ? defaultValue === void 0 ? null : defaultValue : match;
	            if (f(cur.value, i)) match = cur.value;
	        }
	    }
	    exports.tryFindBack = tryFindBack;
	    function findBack(f, xs) {
	        return __failIfNone(tryFindBack(f, xs));
	    }
	    exports.findBack = findBack;
	    function tryFindIndex(f, xs) {
	        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
	            var cur = iter.next();
	            if (cur.done) return null;
	            if (f(cur.value, i)) return i;
	        }
	    }
	    exports.tryFindIndex = tryFindIndex;
	    function findIndex(f, xs) {
	        return __failIfNone(tryFindIndex(f, xs));
	    }
	    exports.findIndex = findIndex;
	    function tryFindIndexBack(f, xs) {
	        var match = -1;
	        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
	            var cur = iter.next();
	            if (cur.done) return match === -1 ? null : match;
	            if (f(cur.value, i)) match = i;
	        }
	    }
	    exports.tryFindIndexBack = tryFindIndexBack;
	    function findIndexBack(f, xs) {
	        return __failIfNone(tryFindIndexBack(f, xs));
	    }
	    exports.findIndexBack = findIndexBack;
	    function tryPick(f, xs) {
	        for (var i = 0, iter = xs[Symbol.iterator]();; i++) {
	            var cur = iter.next();
	            if (cur.done) break;
	            var y = f(cur.value, i);
	            if (y != null) return y;
	        }
	        return void 0;
	    }
	    exports.tryPick = tryPick;
	    function pick(f, xs) {
	        return __failIfNone(tryPick(f, xs));
	    }
	    exports.pick = pick;
	    function unfold(f, acc) {
	        return _a = {}, _a[Symbol.iterator] = function () {
	            return {
	                next: function next() {
	                    var res = f(acc);
	                    if (res != null) {
	                        acc = res[1];
	                        return { done: false, value: res[0] };
	                    }
	                    return { done: true };
	                }
	            };
	        }, _a;
	        var _a;
	    }
	    exports.unfold = unfold;
	    function zip(xs, ys) {
	        return map2(function (x, y) {
	            return [x, y];
	        }, xs, ys);
	    }
	    exports.zip = zip;
	    function zip3(xs, ys, zs) {
	        return map3(function (x, y, z) {
	            return [x, y, z];
	        }, xs, ys, zs);
	    }
	    exports.zip3 = zip3;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 30 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Symbol", "./Symbol", "./List", "./List", "./Set", "./Map", "./Map", "./Set", "./Util", "./Util", "./Util", "./Seq", "./Reflection", "./Date", "./String"], function (require, exports) {
	    "use strict";
	
	    var Symbol_1 = require("./Symbol");
	    var Symbol_2 = require("./Symbol");
	    var List_1 = require("./List");
	    var List_2 = require("./List");
	    var Set_1 = require("./Set");
	    var Map_1 = require("./Map");
	    var Map_2 = require("./Map");
	    var Set_2 = require("./Set");
	    var Util_1 = require("./Util");
	    var Util_2 = require("./Util");
	    var Util_3 = require("./Util");
	    var Seq_1 = require("./Seq");
	    var Reflection_1 = require("./Reflection");
	    var Date_1 = require("./Date");
	    var String_1 = require("./String");
	    function toJson(o) {
	        return JSON.stringify(o, function (k, v) {
	            if (ArrayBuffer.isView(v)) {
	                return Array.from(v);
	            } else if (v != null && (typeof v === 'undefined' ? 'undefined' : _typeof(v)) === "object") {
	                var properties = typeof v[Symbol_1.default.reflection] === "function" ? v[Symbol_1.default.reflection]().properties : null;
	                if (v instanceof List_1.default || v instanceof Set_1.default || v instanceof Set) {
	                    return Array.from(v);
	                } else if (v instanceof Map_1.default || v instanceof Map) {
	                    return Seq_1.fold(function (o, kv) {
	                        return o[toJson(kv[0])] = kv[1], o;
	                    }, {}, v);
	                } else if (!Util_1.hasInterface(v, "FSharpRecord") && properties) {
	                    return Seq_1.fold(function (o, prop) {
	                        return o[prop] = v[prop], o;
	                    }, {}, Object.getOwnPropertyNames(properties));
	                } else if (Util_1.hasInterface(v, "FSharpUnion")) {
	                    if (!v.Fields || !v.Fields.length) {
	                        return v.Case;
	                    } else if (v.Fields.length === 1) {
	                        var fieldValue = typeof v.Fields[0] === 'undefined' ? null : v.Fields[0];
	                        return _a = {}, _a[v.Case] = fieldValue, _a;
	                    } else {
	                        return _b = {}, _b[v.Case] = v.Fields, _b;
	                    }
	                }
	            }
	            return v;
	            var _a, _b;
	        });
	    }
	    exports.toJson = toJson;
	    function combine(path1, path2) {
	        return typeof path2 === "number" ? path1 + "[" + path2 + "]" : (path1 ? path1 + "." : "") + path2;
	    }
	    function isNullable(typ) {
	        if (typeof typ === "string") {
	            return typ !== "boolean" && typ !== "number";
	        } else if (typ instanceof Util_3.NonDeclaredType) {
	            return typ.kind !== "Array" && typ.kind !== "Tuple";
	        } else {
	            var info = typeof typ.prototype[Symbol_1.default.reflection] === "function" ? typ.prototype[Symbol_1.default.reflection]() : null;
	            return info ? info.nullable : true;
	        }
	    }
	    function invalidate(val, typ, path) {
	        throw new Error(String_1.fsFormat("%A", val) + " " + (path ? "(" + path + ")" : "") + " is not of type " + Reflection_1.getTypeFullName(typ));
	    }
	    function needsInflate(enclosing) {
	        var typ = enclosing.head;
	        if (typeof typ === "string") {
	            return false;
	        }
	        if (typ instanceof Util_3.NonDeclaredType) {
	            switch (typ.kind) {
	                case "Option":
	                case "Array":
	                    return typ.definition != null || needsInflate(new List_1.default(typ.generics, enclosing));
	                case "Tuple":
	                    return typ.generics.some(function (x) {
	                        return needsInflate(new List_1.default(x, enclosing));
	                    });
	                case "GenericParam":
	                    return needsInflate(Reflection_1.resolveGeneric(typ.definition, enclosing.tail));
	                case "GenericType":
	                    return true;
	                default:
	                    return false;
	            }
	        }
	        return true;
	    }
	    function inflateArray(arr, enclosing, path) {
	        if (!Array.isArray) {
	            invalidate(arr, "array", path);
	        }
	        return needsInflate(enclosing) ? arr.map(function (x, i) {
	            return inflate(x, enclosing, combine(path, i));
	        }) : arr;
	    }
	    function inflateMap(obj, keyEnclosing, valEnclosing, path) {
	        var inflateKey = keyEnclosing.head !== "string";
	        var inflateVal = needsInflate(valEnclosing);
	        return Object.getOwnPropertyNames(obj).map(function (k) {
	            var key = inflateKey ? inflate(JSON.parse(k), keyEnclosing, combine(path, k)) : k;
	            var val = inflateVal ? inflate(obj[k], valEnclosing, combine(path, k)) : obj[k];
	            return [key, val];
	        });
	    }
	    function inflateList(val, enclosing, path) {
	        var ar = [],
	            li = new List_1.default(),
	            cur = val,
	            inf = needsInflate(enclosing);
	        while (cur.tail != null) {
	            ar.push(inf ? inflate(cur.head, enclosing, path) : cur.head);
	            cur = cur.tail;
	        }
	        ar.reverse();
	        for (var i = 0; i < ar.length; i++) {
	            li = new List_1.default(ar[i], li);
	        }
	        return li;
	    }
	    function inflate(val, typ, path) {
	        var enclosing = null;
	        if (typ instanceof List_1.default) {
	            enclosing = typ;
	            typ = typ.head;
	        } else {
	            enclosing = new List_1.default(typ, new List_1.default());
	        }
	        if (val == null) {
	            if (!isNullable(typ)) {
	                invalidate(val, typ, path);
	            }
	            return val;
	        } else if (typeof typ === "string") {
	            if ((typ === "boolean" || typ === "number" || typ === "string") && (typeof val === 'undefined' ? 'undefined' : _typeof(val)) !== typ) {
	                invalidate(val, typ, path);
	            }
	            return val;
	        } else if (typ instanceof Util_3.NonDeclaredType) {
	            switch (typ.kind) {
	                case "Unit":
	                    return null;
	                case "Option":
	                    return inflate(val, new List_1.default(typ.generics, enclosing), path);
	                case "Array":
	                    if (typ.definition != null) {
	                        return new typ.definition(val);
	                    } else {
	                        return inflateArray(val, new List_1.default(typ.generics, enclosing), path);
	                    }
	                case "Tuple":
	                    return typ.generics.map(function (x, i) {
	                        return inflate(val[i], new List_1.default(x, enclosing), combine(path, i));
	                    });
	                case "GenericParam":
	                    return inflate(val, Reflection_1.resolveGeneric(typ.definition, enclosing.tail), path);
	                case "GenericType":
	                    var def = typ.definition;
	                    if (def === List_1.default) {
	                        return Array.isArray(val) ? List_2.ofArray(inflateArray(val, Reflection_1.resolveGeneric(0, enclosing), path)) : inflateList(val, Reflection_1.resolveGeneric(0, enclosing), path);
	                    }
	                    if (def === Set_1.default) {
	                        return Set_2.create(inflateArray(val, Reflection_1.resolveGeneric(0, enclosing), path));
	                    }
	                    if (def === Set) {
	                        return new Set(inflateArray(val, Reflection_1.resolveGeneric(0, enclosing), path));
	                    }
	                    if (def === Map_1.default) {
	                        return Map_2.create(inflateMap(val, Reflection_1.resolveGeneric(0, enclosing), Reflection_1.resolveGeneric(1, enclosing), path));
	                    }
	                    if (def === Map) {
	                        return new Map(inflateMap(val, Reflection_1.resolveGeneric(0, enclosing), Reflection_1.resolveGeneric(1, enclosing), path));
	                    }
	                    return inflate(val, new List_1.default(typ.definition, enclosing), path);
	                default:
	                    return val;
	            }
	        } else if (typeof typ === "function") {
	            if (typ === Date) {
	                return Date_1.parse(val);
	            }
	            var info = typeof typ.prototype[Symbol_1.default.reflection] === "function" ? typ.prototype[Symbol_1.default.reflection]() : {};
	            if (info.cases) {
	                var uCase = void 0,
	                    uFields = [];
	                if (typeof val === "string") {
	                    uCase = val;
	                } else if (typeof val.Case === "string" && Array.isArray(val.Fields)) {
	                    uCase = val.Case;
	                    uFields = val.Fields;
	                } else {
	                    var caseName = Object.getOwnPropertyNames(val)[0];
	                    var fieldTypes = info.cases[caseName];
	                    if (Array.isArray(fieldTypes)) {
	                        var fields = fieldTypes.length > 1 ? val[caseName] : [val[caseName]];
	                        uCase = caseName;
	                        path = combine(path, caseName);
	                        for (var i = 0; i < fieldTypes.length; i++) {
	                            uFields.push(inflate(fields[i], new List_1.default(fieldTypes[i], enclosing), combine(path, i)));
	                        }
	                    }
	                }
	                if (uCase in info.cases === false) {
	                    invalidate(val, typ, path);
	                }
	                return new typ(uCase, uFields);
	            }
	            if (info.properties) {
	                var newObj = new typ();
	                var properties = info.properties;
	                var ks = Object.getOwnPropertyNames(properties);
	                for (var i = 0; i < ks.length; i++) {
	                    var k = ks[i];
	                    newObj[k] = inflate(val[k], new List_1.default(properties[k], enclosing), combine(path, k));
	                }
	                return newObj;
	            }
	            return val;
	        }
	        throw new Error("Unexpected type when deserializing JSON: " + typ);
	    }
	    function inflatePublic(val, genArgs) {
	        return inflate(val, genArgs ? genArgs.T : null, "");
	    }
	    exports.inflate = inflatePublic;
	    function ofJson(json, genArgs) {
	        return inflate(JSON.parse(json), genArgs ? genArgs.T : null, "");
	    }
	    exports.ofJson = ofJson;
	    function toJsonWithTypeInfo(o) {
	        return JSON.stringify(o, function (k, v) {
	            if (ArrayBuffer.isView(v)) {
	                return Array.from(v);
	            } else if (v != null && (typeof v === 'undefined' ? 'undefined' : _typeof(v)) === "object") {
	                var typeName = typeof v[Symbol_1.default.reflection] === "function" ? v[Symbol_1.default.reflection]().type : null;
	                if (v instanceof List_1.default || v instanceof Set_1.default || v instanceof Set) {
	                    return {
	                        $type: typeName || "System.Collections.Generic.HashSet",
	                        $values: Array.from(v)
	                    };
	                } else if (v instanceof Map_1.default || v instanceof Map) {
	                    return Seq_1.fold(function (o, kv) {
	                        o[kv[0]] = kv[1];return o;
	                    }, { $type: typeName || "System.Collections.Generic.Dictionary" }, v);
	                } else if (typeName) {
	                    if (Util_1.hasInterface(v, "FSharpUnion") || Util_1.hasInterface(v, "FSharpRecord")) {
	                        return Object.assign({ $type: typeName }, v);
	                    } else {
	                        var proto = Object.getPrototypeOf(v),
	                            props = Object.getOwnPropertyNames(proto),
	                            o_1 = { $type: typeName };
	                        for (var i = 0; i < props.length; i++) {
	                            var prop = Object.getOwnPropertyDescriptor(proto, props[i]);
	                            if (prop.get) o_1[props[i]] = prop.get.apply(v);
	                        }
	                        return o_1;
	                    }
	                }
	            }
	            return v;
	        });
	    }
	    exports.toJsonWithTypeInfo = toJsonWithTypeInfo;
	    function ofJsonWithTypeInfo(json, genArgs) {
	        var parsed = JSON.parse(json, function (k, v) {
	            if (v == null) return v;else if ((typeof v === 'undefined' ? 'undefined' : _typeof(v)) === "object" && typeof v.$type === "string") {
	                var type = v.$type.replace('+', '.'),
	                    i = type.indexOf('`');
	                if (i > -1) {
	                    type = type.substr(0, i);
	                } else {
	                    i = type.indexOf(',');
	                    type = i > -1 ? type.substr(0, i) : type;
	                }
	                if (type === "System.Collections.Generic.List" || type.indexOf("[]") === type.length - 2) {
	                    return v.$values;
	                }
	                if (type === "Microsoft.FSharp.Collections.FSharpList") {
	                    return List_2.ofArray(v.$values);
	                } else if (type == "Microsoft.FSharp.Collections.FSharpSet") {
	                    return Set_2.create(v.$values);
	                } else if (type == "System.Collections.Generic.HashSet") {
	                    return new Set(v.$values);
	                } else if (type == "Microsoft.FSharp.Collections.FSharpMap") {
	                    delete v.$type;
	                    return Map_2.create(Object.getOwnPropertyNames(v).map(function (k) {
	                        return [k, v[k]];
	                    }));
	                } else if (type == "System.Collections.Generic.Dictionary") {
	                    delete v.$type;
	                    return new Map(Object.getOwnPropertyNames(v).map(function (k) {
	                        return [k, v[k]];
	                    }));
	                } else {
	                    var T = Symbol_2.getType(type);
	                    if (T) {
	                        delete v.$type;
	                        return Object.assign(new T(), v);
	                    }
	                }
	            } else if (/^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:\.\d+)?(?:[+-]\d{2}:\d{2}|Z)$/.test(v)) return Date_1.parse(v);else return v;
	        });
	        var expected = genArgs ? genArgs.T : null;
	        if (parsed != null && typeof expected === "function" && !(parsed instanceof Util_2.getDefinition(expected))) {
	            throw new Error("JSON is not of type " + expected.name + ": " + json);
	        }
	        return parsed;
	    }
	    exports.ofJsonWithTypeInfo = ofJsonWithTypeInfo;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 31 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./List", "./List", "./Util", "./GenericComparer", "./Symbol", "./Seq", "./Seq", "./Seq", "./Seq", "./Seq", "./Seq", "./Seq"], function (require, exports) {
	    "use strict";
	
	    var List_1 = require("./List");
	    var List_2 = require("./List");
	    var Util_1 = require("./Util");
	    var GenericComparer_1 = require("./GenericComparer");
	    var Symbol_1 = require("./Symbol");
	    var Seq_1 = require("./Seq");
	    var Seq_2 = require("./Seq");
	    var Seq_3 = require("./Seq");
	    var Seq_4 = require("./Seq");
	    var Seq_5 = require("./Seq");
	    var Seq_6 = require("./Seq");
	    var Seq_7 = require("./Seq");
	    function distinctBy(f, xs) {
	        return Seq_6.choose(function (tup) {
	            return tup[0];
	        }, Seq_7.scan(function (tup, x) {
	            var acc = tup[1];
	            var k = f(x);
	            return acc.has(k) ? [null, acc] : [x, add(k, acc)];
	        }, [null, create()], xs));
	    }
	    exports.distinctBy = distinctBy;
	    function distinct(xs) {
	        return distinctBy(function (x) {
	            return x;
	        }, xs);
	    }
	    exports.distinct = distinct;
	    var SetTree = function () {
	        function SetTree(caseName, fields) {
	            this.Case = caseName;
	            this.Fields = fields;
	        }
	        return SetTree;
	    }();
	    exports.SetTree = SetTree;
	    var tree_tolerance = 2;
	    function tree_countAux(s, acc) {
	        return s.Case === "SetOne" ? acc + 1 : s.Case === "SetEmpty" ? acc : tree_countAux(s.Fields[1], tree_countAux(s.Fields[2], acc + 1));
	    }
	    function tree_count(s) {
	        return tree_countAux(s, 0);
	    }
	    function tree_SetOne(n) {
	        return new SetTree("SetOne", [n]);
	    }
	    function tree_SetNode(x, l, r, h) {
	        return new SetTree("SetNode", [x, l, r, h]);
	    }
	    function tree_height(t) {
	        return t.Case === "SetOne" ? 1 : t.Case === "SetNode" ? t.Fields[3] : 0;
	    }
	    function tree_mk(l, k, r) {
	        var matchValue = [l, r];
	        var $target1 = function $target1() {
	            var hl = tree_height(l);
	            var hr = tree_height(r);
	            var m = hl < hr ? hr : hl;
	            return tree_SetNode(k, l, r, m + 1);
	        };
	        if (matchValue[0].Case === "SetEmpty") {
	            if (matchValue[1].Case === "SetEmpty") {
	                return tree_SetOne(k);
	            } else {
	                return $target1();
	            }
	        } else {
	            return $target1();
	        }
	    }
	    function tree_rebalance(t1, k, t2) {
	        var t1h = tree_height(t1);
	        var t2h = tree_height(t2);
	        if (t2h > t1h + tree_tolerance) {
	            if (t2.Case === "SetNode") {
	                if (tree_height(t2.Fields[1]) > t1h + 1) {
	                    if (t2.Fields[1].Case === "SetNode") {
	                        return tree_mk(tree_mk(t1, k, t2.Fields[1].Fields[1]), t2.Fields[1].Fields[0], tree_mk(t2.Fields[1].Fields[2], t2.Fields[0], t2.Fields[2]));
	                    } else {
	                        throw new Error("rebalance");
	                    }
	                } else {
	                    return tree_mk(tree_mk(t1, k, t2.Fields[1]), t2.Fields[0], t2.Fields[2]);
	                }
	            } else {
	                throw new Error("rebalance");
	            }
	        } else {
	            if (t1h > t2h + tree_tolerance) {
	                if (t1.Case === "SetNode") {
	                    if (tree_height(t1.Fields[2]) > t2h + 1) {
	                        if (t1.Fields[2].Case === "SetNode") {
	                            return tree_mk(tree_mk(t1.Fields[1], t1.Fields[0], t1.Fields[2].Fields[1]), t1.Fields[2].Fields[0], tree_mk(t1.Fields[2].Fields[2], k, t2));
	                        } else {
	                            throw new Error("rebalance");
	                        }
	                    } else {
	                        return tree_mk(t1.Fields[1], t1.Fields[0], tree_mk(t1.Fields[2], k, t2));
	                    }
	                } else {
	                    throw new Error("rebalance");
	                }
	            } else {
	                return tree_mk(t1, k, t2);
	            }
	        }
	    }
	    function tree_add(comparer, k, t) {
	        if (t.Case === "SetOne") {
	            var c = comparer.Compare(k, t.Fields[0]);
	            if (c < 0) {
	                return tree_SetNode(k, new SetTree("SetEmpty", []), t, 2);
	            } else if (c === 0) {
	                return t;
	            } else {
	                return tree_SetNode(k, t, new SetTree("SetEmpty", []), 2);
	            }
	        } else if (t.Case === "SetEmpty") {
	            return tree_SetOne(k);
	        } else {
	            var c = comparer.Compare(k, t.Fields[0]);
	            if (c < 0) {
	                return tree_rebalance(tree_add(comparer, k, t.Fields[1]), t.Fields[0], t.Fields[2]);
	            } else if (c === 0) {
	                return t;
	            } else {
	                return tree_rebalance(t.Fields[1], t.Fields[0], tree_add(comparer, k, t.Fields[2]));
	            }
	        }
	    }
	    function tree_balance(comparer, t1, k, t2) {
	        var matchValue = [t1, t2];
	        var $target1 = function $target1(t1_1) {
	            return tree_add(comparer, k, t1_1);
	        };
	        var $target2 = function $target2(k1, t2_1) {
	            return tree_add(comparer, k, tree_add(comparer, k1, t2_1));
	        };
	        if (matchValue[0].Case === "SetOne") {
	            if (matchValue[1].Case === "SetEmpty") {
	                return $target1(matchValue[0]);
	            } else {
	                if (matchValue[1].Case === "SetOne") {
	                    return $target2(matchValue[0].Fields[0], matchValue[1]);
	                } else {
	                    return $target2(matchValue[0].Fields[0], matchValue[1]);
	                }
	            }
	        } else {
	            if (matchValue[0].Case === "SetNode") {
	                if (matchValue[1].Case === "SetOne") {
	                    var k2 = matchValue[1].Fields[0];
	                    var t1_1 = matchValue[0];
	                    return tree_add(comparer, k, tree_add(comparer, k2, t1_1));
	                } else {
	                    if (matchValue[1].Case === "SetNode") {
	                        var h1 = matchValue[0].Fields[3];
	                        var h2 = matchValue[1].Fields[3];
	                        var k1 = matchValue[0].Fields[0];
	                        var k2 = matchValue[1].Fields[0];
	                        var t11 = matchValue[0].Fields[1];
	                        var t12 = matchValue[0].Fields[2];
	                        var t21 = matchValue[1].Fields[1];
	                        var t22 = matchValue[1].Fields[2];
	                        if (h1 + tree_tolerance < h2) {
	                            return tree_rebalance(tree_balance(comparer, t1, k, t21), k2, t22);
	                        } else {
	                            if (h2 + tree_tolerance < h1) {
	                                return tree_rebalance(t11, k1, tree_balance(comparer, t12, k, t2));
	                            } else {
	                                return tree_mk(t1, k, t2);
	                            }
	                        }
	                    } else {
	                        return $target1(matchValue[0]);
	                    }
	                }
	            } else {
	                var t2_1 = matchValue[1];
	                return tree_add(comparer, k, t2_1);
	            }
	        }
	    }
	    function tree_split(comparer, pivot, t) {
	        if (t.Case === "SetOne") {
	            var c = comparer.Compare(t.Fields[0], pivot);
	            if (c < 0) {
	                return [t, false, new SetTree("SetEmpty", [])];
	            } else if (c === 0) {
	                return [new SetTree("SetEmpty", []), true, new SetTree("SetEmpty", [])];
	            } else {
	                return [new SetTree("SetEmpty", []), false, t];
	            }
	        } else if (t.Case === "SetEmpty") {
	            return [new SetTree("SetEmpty", []), false, new SetTree("SetEmpty", [])];
	        } else {
	            var c = comparer.Compare(pivot, t.Fields[0]);
	            if (c < 0) {
	                var patternInput = tree_split(comparer, pivot, t.Fields[1]);
	                return [patternInput[0], patternInput[1], tree_balance(comparer, patternInput[2], t.Fields[0], t.Fields[2])];
	            } else if (c === 0) {
	                return [t.Fields[1], true, t.Fields[2]];
	            } else {
	                var patternInput = tree_split(comparer, pivot, t.Fields[2]);
	                return [tree_balance(comparer, t.Fields[1], t.Fields[0], patternInput[0]), patternInput[1], patternInput[2]];
	            }
	        }
	    }
	    function tree_spliceOutSuccessor(t) {
	        if (t.Case === "SetOne") {
	            return [t.Fields[0], new SetTree("SetEmpty", [])];
	        } else if (t.Case === "SetNode") {
	            if (t.Fields[1].Case === "SetEmpty") {
	                return [t.Fields[0], t.Fields[2]];
	            } else {
	                var patternInput = tree_spliceOutSuccessor(t.Fields[1]);
	                return [patternInput[0], tree_mk(patternInput[1], t.Fields[0], t.Fields[2])];
	            }
	        } else {
	            throw new Error("internal error: Map.spliceOutSuccessor");
	        }
	    }
	    function tree_remove(comparer, k, t) {
	        if (t.Case === "SetOne") {
	            var c = comparer.Compare(k, t.Fields[0]);
	            if (c === 0) {
	                return new SetTree("SetEmpty", []);
	            } else {
	                return t;
	            }
	        } else if (t.Case === "SetNode") {
	            var c = comparer.Compare(k, t.Fields[0]);
	            if (c < 0) {
	                return tree_rebalance(tree_remove(comparer, k, t.Fields[1]), t.Fields[0], t.Fields[2]);
	            } else if (c === 0) {
	                var matchValue = [t.Fields[1], t.Fields[2]];
	                if (matchValue[0].Case === "SetEmpty") {
	                    return t.Fields[2];
	                } else if (matchValue[1].Case === "SetEmpty") {
	                    return t.Fields[1];
	                } else {
	                    var patternInput = tree_spliceOutSuccessor(t.Fields[2]);
	                    return tree_mk(t.Fields[1], patternInput[0], patternInput[1]);
	                }
	            } else {
	                return tree_rebalance(t.Fields[1], t.Fields[0], tree_remove(comparer, k, t.Fields[2]));
	            }
	        } else {
	            return t;
	        }
	    }
	    function tree_mem(comparer, k, t) {
	        if (t.Case === "SetOne") {
	            return comparer.Compare(k, t.Fields[0]) === 0;
	        } else if (t.Case === "SetEmpty") {
	            return false;
	        } else {
	            var c = comparer.Compare(k, t.Fields[0]);
	            if (c < 0) {
	                return tree_mem(comparer, k, t.Fields[1]);
	            } else if (c === 0) {
	                return true;
	            } else {
	                return tree_mem(comparer, k, t.Fields[2]);
	            }
	        }
	    }
	    function tree_iter(f, t) {
	        if (t.Case === "SetOne") {
	            f(t.Fields[0]);
	        } else {
	            if (t.Case === "SetEmpty") {} else {
	                tree_iter(f, t.Fields[1]);
	                f(t.Fields[0]);
	                tree_iter(f, t.Fields[2]);
	            }
	        }
	    }
	    function tree_foldBack(f, m, x) {
	        return m.Case === "SetOne" ? f(m.Fields[0], x) : m.Case === "SetEmpty" ? x : tree_foldBack(f, m.Fields[1], f(m.Fields[0], tree_foldBack(f, m.Fields[2], x)));
	    }
	    function tree_fold(f, x, m) {
	        if (m.Case === "SetOne") {
	            return f(x, m.Fields[0]);
	        } else if (m.Case === "SetEmpty") {
	            return x;
	        } else {
	            var x_1 = tree_fold(f, x, m.Fields[1]);
	            var x_2 = f(x_1, m.Fields[0]);
	            return tree_fold(f, x_2, m.Fields[2]);
	        }
	    }
	    function tree_forall(f, m) {
	        return m.Case === "SetOne" ? f(m.Fields[0]) : m.Case === "SetEmpty" ? true : (f(m.Fields[0]) ? tree_forall(f, m.Fields[1]) : false) ? tree_forall(f, m.Fields[2]) : false;
	    }
	    function tree_exists(f, m) {
	        return m.Case === "SetOne" ? f(m.Fields[0]) : m.Case === "SetEmpty" ? false : (f(m.Fields[0]) ? true : tree_exists(f, m.Fields[1])) ? true : tree_exists(f, m.Fields[2]);
	    }
	    function tree_isEmpty(m) {
	        return m.Case === "SetEmpty" ? true : false;
	    }
	    function tree_subset(comparer, a, b) {
	        return tree_forall(function (x) {
	            return tree_mem(comparer, x, b);
	        }, a);
	    }
	    function tree_psubset(comparer, a, b) {
	        return tree_forall(function (x) {
	            return tree_mem(comparer, x, b);
	        }, a) ? tree_exists(function (x) {
	            return !tree_mem(comparer, x, a);
	        }, b) : false;
	    }
	    function tree_filterAux(comparer, f, s, acc) {
	        if (s.Case === "SetOne") {
	            if (f(s.Fields[0])) {
	                return tree_add(comparer, s.Fields[0], acc);
	            } else {
	                return acc;
	            }
	        } else if (s.Case === "SetEmpty") {
	            return acc;
	        } else {
	            var acc_1 = f(s.Fields[0]) ? tree_add(comparer, s.Fields[0], acc) : acc;
	            return tree_filterAux(comparer, f, s.Fields[1], tree_filterAux(comparer, f, s.Fields[2], acc_1));
	        }
	    }
	    function tree_filter(comparer, f, s) {
	        return tree_filterAux(comparer, f, s, new SetTree("SetEmpty", []));
	    }
	    function tree_diffAux(comparer, m, acc) {
	        return m.Case === "SetOne" ? tree_remove(comparer, m.Fields[0], acc) : m.Case === "SetEmpty" ? acc : tree_diffAux(comparer, m.Fields[1], tree_diffAux(comparer, m.Fields[2], tree_remove(comparer, m.Fields[0], acc)));
	    }
	    function tree_diff(comparer, a, b) {
	        return tree_diffAux(comparer, b, a);
	    }
	    function tree_union(comparer, t1, t2) {
	        var matchValue = [t1, t2];
	        var $target2 = function $target2(t) {
	            return t;
	        };
	        var $target3 = function $target3(k1, t2_1) {
	            return tree_add(comparer, k1, t2_1);
	        };
	        if (matchValue[0].Case === "SetEmpty") {
	            var t = matchValue[1];
	            return t;
	        } else {
	            if (matchValue[0].Case === "SetOne") {
	                if (matchValue[1].Case === "SetEmpty") {
	                    return $target2(matchValue[0]);
	                } else {
	                    if (matchValue[1].Case === "SetOne") {
	                        return $target3(matchValue[0].Fields[0], matchValue[1]);
	                    } else {
	                        return $target3(matchValue[0].Fields[0], matchValue[1]);
	                    }
	                }
	            } else {
	                if (matchValue[1].Case === "SetEmpty") {
	                    return $target2(matchValue[0]);
	                } else {
	                    if (matchValue[1].Case === "SetOne") {
	                        var k2 = matchValue[1].Fields[0];
	                        var t1_1 = matchValue[0];
	                        return tree_add(comparer, k2, t1_1);
	                    } else {
	                        var h1 = matchValue[0].Fields[3];
	                        var h2 = matchValue[1].Fields[3];
	                        var k1 = matchValue[0].Fields[0];
	                        var k2 = matchValue[1].Fields[0];
	                        var t11 = matchValue[0].Fields[1];
	                        var t12 = matchValue[0].Fields[2];
	                        var t21 = matchValue[1].Fields[1];
	                        var t22 = matchValue[1].Fields[2];
	                        if (h1 > h2) {
	                            var patternInput = tree_split(comparer, k1, t2);
	                            var lo = patternInput[0];
	                            var hi = patternInput[2];
	                            return tree_balance(comparer, tree_union(comparer, t11, lo), k1, tree_union(comparer, t12, hi));
	                        } else {
	                            var patternInput = tree_split(comparer, k2, t1);
	                            var lo = patternInput[0];
	                            var hi = patternInput[2];
	                            return tree_balance(comparer, tree_union(comparer, t21, lo), k2, tree_union(comparer, t22, hi));
	                        }
	                    }
	                }
	            }
	        }
	    }
	    function tree_intersectionAux(comparer, b, m, acc) {
	        if (m.Case === "SetOne") {
	            if (tree_mem(comparer, m.Fields[0], b)) {
	                return tree_add(comparer, m.Fields[0], acc);
	            } else {
	                return acc;
	            }
	        } else if (m.Case === "SetEmpty") {
	            return acc;
	        } else {
	            var acc_1 = tree_intersectionAux(comparer, b, m.Fields[2], acc);
	            var acc_2 = tree_mem(comparer, m.Fields[0], b) ? tree_add(comparer, m.Fields[0], acc_1) : acc_1;
	            return tree_intersectionAux(comparer, b, m.Fields[1], acc_2);
	        }
	    }
	    function tree_intersection(comparer, a, b) {
	        return tree_intersectionAux(comparer, b, a, new SetTree("SetEmpty", []));
	    }
	    function tree_partition1(comparer, f, k, acc1, acc2) {
	        return f(k) ? [tree_add(comparer, k, acc1), acc2] : [acc1, tree_add(comparer, k, acc2)];
	    }
	    function tree_partitionAux(comparer, f, s, acc_0, acc_1) {
	        var acc = [acc_0, acc_1];
	        if (s.Case === "SetOne") {
	            var acc1 = acc[0];
	            var acc2 = acc[1];
	            return tree_partition1(comparer, f, s.Fields[0], acc1, acc2);
	        } else {
	            if (s.Case === "SetEmpty") {
	                return acc;
	            } else {
	                var acc_2 = tree_partitionAux(comparer, f, s.Fields[2], acc[0], acc[1]);
	                var acc_3 = tree_partition1(comparer, f, s.Fields[0], acc_2[0], acc_2[1]);
	                return tree_partitionAux(comparer, f, s.Fields[1], acc_3[0], acc_3[1]);
	            }
	        }
	    }
	    function tree_partition(comparer, f, s) {
	        var seed = [new SetTree("SetEmpty", []), new SetTree("SetEmpty", [])];
	        var arg30_ = seed[0];
	        var arg31_ = seed[1];
	        return tree_partitionAux(comparer, f, s, arg30_, arg31_);
	    }
	    function tree_minimumElementAux(s, n) {
	        return s.Case === "SetOne" ? s.Fields[0] : s.Case === "SetEmpty" ? n : tree_minimumElementAux(s.Fields[1], s.Fields[0]);
	    }
	    function tree_minimumElementOpt(s) {
	        return s.Case === "SetOne" ? s.Fields[0] : s.Case === "SetEmpty" ? null : tree_minimumElementAux(s.Fields[1], s.Fields[0]);
	    }
	    function tree_maximumElementAux(s, n) {
	        return s.Case === "SetOne" ? s.Fields[0] : s.Case === "SetEmpty" ? n : tree_maximumElementAux(s.Fields[2], s.Fields[0]);
	    }
	    function tree_maximumElementOpt(s) {
	        return s.Case === "SetOne" ? s.Fields[0] : s.Case === "SetEmpty" ? null : tree_maximumElementAux(s.Fields[2], s.Fields[0]);
	    }
	    function tree_minimumElement(s) {
	        var matchValue = tree_minimumElementOpt(s);
	        if (matchValue == null) {
	            throw new Error("Set contains no elements");
	        } else {
	            return matchValue;
	        }
	    }
	    function tree_maximumElement(s) {
	        var matchValue = tree_maximumElementOpt(s);
	        if (matchValue == null) {
	            throw new Error("Set contains no elements");
	        } else {
	            return matchValue;
	        }
	    }
	    function tree_collapseLHS(stack) {
	        return stack.tail != null ? stack.head.Case === "SetOne" ? stack : stack.head.Case === "SetNode" ? tree_collapseLHS(List_2.ofArray([stack.head.Fields[1], tree_SetOne(stack.head.Fields[0]), stack.head.Fields[2]], stack.tail)) : tree_collapseLHS(stack.tail) : new List_1.default();
	    }
	    function tree_mkIterator(s) {
	        return { stack: tree_collapseLHS(new List_1.default(s, new List_1.default())), started: false };
	    }
	    ;
	    function tree_moveNext(i) {
	        function current(i) {
	            if (i.stack.tail == null) {
	                return null;
	            } else if (i.stack.head.Case === "SetOne") {
	                return i.stack.head.Fields[0];
	            }
	            throw new Error("Please report error: Set iterator, unexpected stack for current");
	        }
	        if (i.started) {
	            if (i.stack.tail == null) {
	                return { done: true, value: null };
	            } else {
	                if (i.stack.head.Case === "SetOne") {
	                    i.stack = tree_collapseLHS(i.stack.tail);
	                    return {
	                        done: i.stack.tail == null,
	                        value: current(i)
	                    };
	                } else {
	                    throw new Error("Please report error: Set iterator, unexpected stack for moveNext");
	                }
	            }
	        } else {
	            i.started = true;
	            return {
	                done: i.stack.tail == null,
	                value: current(i)
	            };
	        }
	        ;
	    }
	    function tree_compareStacks(comparer, l1, l2) {
	        var $target8 = function $target8(n1k, t1) {
	            return tree_compareStacks(comparer, List_2.ofArray([new SetTree("SetEmpty", []), tree_SetOne(n1k)], t1), l2);
	        };
	        var $target9 = function $target9(n1k, n1l, n1r, t1) {
	            return tree_compareStacks(comparer, List_2.ofArray([n1l, tree_SetNode(n1k, new SetTree("SetEmpty", []), n1r, 0)], t1), l2);
	        };
	        var $target11 = function $target11(n2k, n2l, n2r, t2) {
	            return tree_compareStacks(comparer, l1, List_2.ofArray([n2l, tree_SetNode(n2k, new SetTree("SetEmpty", []), n2r, 0)], t2));
	        };
	        if (l1.tail != null) {
	            if (l2.tail != null) {
	                if (l2.head.Case === "SetOne") {
	                    if (l1.head.Case === "SetOne") {
	                        var n1k = l1.head.Fields[0],
	                            n2k = l2.head.Fields[0],
	                            t1 = l1.tail,
	                            t2 = l2.tail,
	                            c = comparer.Compare(n1k, n2k);
	                        if (c !== 0) {
	                            return c;
	                        } else {
	                            return tree_compareStacks(comparer, t1, t2);
	                        }
	                    } else {
	                        if (l1.head.Case === "SetNode") {
	                            if (l1.head.Fields[1].Case === "SetEmpty") {
	                                var emp = l1.head.Fields[1],
	                                    n1k = l1.head.Fields[0],
	                                    n1r = l1.head.Fields[2],
	                                    n2k = l2.head.Fields[0],
	                                    t1 = l1.tail,
	                                    t2 = l2.tail,
	                                    c = comparer.Compare(n1k, n2k);
	                                if (c !== 0) {
	                                    return c;
	                                } else {
	                                    return tree_compareStacks(comparer, List_2.ofArray([n1r], t1), List_2.ofArray([emp], t2));
	                                }
	                            } else {
	                                return $target9(l1.head.Fields[0], l1.head.Fields[1], l1.head.Fields[2], l1.tail);
	                            }
	                        } else {
	                            var n2k = l2.head.Fields[0],
	                                t2 = l2.tail;
	                            return tree_compareStacks(comparer, l1, List_2.ofArray([new SetTree("SetEmpty", []), tree_SetOne(n2k)], t2));
	                        }
	                    }
	                } else {
	                    if (l2.head.Case === "SetNode") {
	                        if (l2.head.Fields[1].Case === "SetEmpty") {
	                            if (l1.head.Case === "SetOne") {
	                                var n1k = l1.head.Fields[0],
	                                    n2k = l2.head.Fields[0],
	                                    n2r = l2.head.Fields[2],
	                                    t1 = l1.tail,
	                                    t2 = l2.tail,
	                                    c = comparer.Compare(n1k, n2k);
	                                if (c !== 0) {
	                                    return c;
	                                } else {
	                                    return tree_compareStacks(comparer, List_2.ofArray([new SetTree("SetEmpty", [])], t1), List_2.ofArray([n2r], t2));
	                                }
	                            } else {
	                                if (l1.head.Case === "SetNode") {
	                                    if (l1.head.Fields[1].Case === "SetEmpty") {
	                                        var n1k = l1.head.Fields[0],
	                                            n1r = l1.head.Fields[2],
	                                            n2k = l2.head.Fields[0],
	                                            n2r = l2.head.Fields[2],
	                                            t1 = l1.tail,
	                                            t2 = l2.tail,
	                                            c = comparer.Compare(n1k, n2k);
	                                        if (c !== 0) {
	                                            return c;
	                                        } else {
	                                            return tree_compareStacks(comparer, List_2.ofArray([n1r], t1), List_2.ofArray([n2r], t2));
	                                        }
	                                    } else {
	                                        return $target9(l1.head.Fields[0], l1.head.Fields[1], l1.head.Fields[2], l1.tail);
	                                    }
	                                } else {
	                                    return $target11(l2.head.Fields[0], l2.head.Fields[1], l2.head.Fields[2], l2.tail);
	                                }
	                            }
	                        } else {
	                            if (l1.head.Case === "SetOne") {
	                                return $target8(l1.head.Fields[0], l1.tail);
	                            } else {
	                                if (l1.head.Case === "SetNode") {
	                                    return $target9(l1.head.Fields[0], l1.head.Fields[1], l1.head.Fields[2], l1.tail);
	                                } else {
	                                    return $target11(l2.head.Fields[0], l2.head.Fields[1], l2.head.Fields[2], l2.tail);
	                                }
	                            }
	                        }
	                    } else {
	                        if (l1.head.Case === "SetOne") {
	                            return $target8(l1.head.Fields[0], l1.tail);
	                        } else {
	                            if (l1.head.Case === "SetNode") {
	                                return $target9(l1.head.Fields[0], l1.head.Fields[1], l1.head.Fields[2], l1.tail);
	                            } else {
	                                return tree_compareStacks(comparer, l1.tail, l2.tail);
	                            }
	                        }
	                    }
	                }
	            } else {
	                return 1;
	            }
	        } else {
	            if (l2.tail != null) {
	                return -1;
	            } else {
	                return 0;
	            }
	        }
	    }
	    function tree_compare(comparer, s1, s2) {
	        if (s1.Case === "SetEmpty") {
	            if (s2.Case === "SetEmpty") {
	                return 0;
	            } else {
	                return -1;
	            }
	        } else {
	            if (s2.Case === "SetEmpty") {
	                return 1;
	            } else {
	                return tree_compareStacks(comparer, List_2.ofArray([s1]), List_2.ofArray([s2]));
	            }
	        }
	    }
	    function tree_mkFromEnumerator(comparer, acc, e) {
	        var cur = e.next();
	        while (!cur.done) {
	            acc = tree_add(comparer, cur.value, acc);
	            cur = e.next();
	        }
	        return acc;
	    }
	    function tree_ofSeq(comparer, c) {
	        var ie = c[Symbol.iterator]();
	        return tree_mkFromEnumerator(comparer, new SetTree("SetEmpty", []), ie);
	    }
	    var FableSet = function () {
	        function FableSet() {}
	        FableSet.prototype.ToString = function () {
	            return "set [" + Array.from(this).map(Util_1.toString).join("; ") + "]";
	        };
	        FableSet.prototype.Equals = function (s2) {
	            return this.CompareTo(s2) === 0;
	        };
	        FableSet.prototype.CompareTo = function (s2) {
	            return this === s2 ? 0 : tree_compare(this.comparer, this.tree, s2.tree);
	        };
	        FableSet.prototype[Symbol.iterator] = function () {
	            var i = tree_mkIterator(this.tree);
	            return {
	                next: function next() {
	                    return tree_moveNext(i);
	                }
	            };
	        };
	        FableSet.prototype.values = function () {
	            return this[Symbol.iterator]();
	        };
	        FableSet.prototype.has = function (v) {
	            return tree_mem(this.comparer, v, this.tree);
	        };
	        FableSet.prototype.add = function (v) {
	            throw new Error("not supported");
	        };
	        FableSet.prototype.delete = function (v) {
	            throw new Error("not supported");
	        };
	        FableSet.prototype.clear = function () {
	            throw new Error("not supported");
	        };
	        Object.defineProperty(FableSet.prototype, "size", {
	            get: function get() {
	                return tree_count(this.tree);
	            },
	            enumerable: true,
	            configurable: true
	        });
	        FableSet.prototype[Symbol_1.default.reflection] = function () {
	            return {
	                type: "Microsoft.FSharp.Collections.FSharpSet",
	                interfaces: ["System.IEquatable", "System.IComparable"]
	            };
	        };
	        return FableSet;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = FableSet;
	    function from(comparer, tree) {
	        var s = new FableSet();
	        s.tree = tree;
	        s.comparer = comparer || new GenericComparer_1.default();
	        return s;
	    }
	    function create(ie, comparer) {
	        comparer = comparer || new GenericComparer_1.default();
	        return from(comparer, ie ? tree_ofSeq(comparer, ie) : new SetTree("SetEmpty", []));
	    }
	    exports.create = create;
	    function isEmpty(s) {
	        return tree_isEmpty(s.tree);
	    }
	    exports.isEmpty = isEmpty;
	    function add(item, s) {
	        return from(s.comparer, tree_add(s.comparer, item, s.tree));
	    }
	    exports.add = add;
	    function addInPlace(item, s) {
	        return s.has(item) ? false : (s.add(item), true);
	    }
	    exports.addInPlace = addInPlace;
	    function remove(item, s) {
	        return from(s.comparer, tree_remove(s.comparer, item, s.tree));
	    }
	    exports.remove = remove;
	    function union(set1, set2) {
	        return set2.tree.Case === "SetEmpty" ? set1 : set1.tree.Case === "SetEmpty" ? set2 : from(set1.comparer, tree_union(set1.comparer, set1.tree, set2.tree));
	    }
	    exports.union = union;
	    function op_Addition(set1, set2) {
	        return union(set1, set2);
	    }
	    exports.op_Addition = op_Addition;
	    function unionInPlace(set1, set2) {
	        Seq_1.iterate(function (x) {
	            set1.add(x);
	        }, set2);
	    }
	    exports.unionInPlace = unionInPlace;
	    function unionMany(sets) {
	        return Seq_2.fold(function (acc, s) {
	            return union(s, acc);
	        }, create(), sets);
	    }
	    exports.unionMany = unionMany;
	    function difference(set1, set2) {
	        return set1.tree.Case === "SetEmpty" ? set1 : set2.tree.Case === "SetEmpty" ? set1 : from(set1.comparer, tree_diff(set1.comparer, set1.tree, set2.tree));
	    }
	    exports.difference = difference;
	    function op_Subtraction(set1, set2) {
	        return difference(set1, set2);
	    }
	    exports.op_Subtraction = op_Subtraction;
	    function differenceInPlace(set1, set2) {
	        Seq_1.iterate(function (x) {
	            set1.delete(x);
	        }, set2);
	    }
	    exports.differenceInPlace = differenceInPlace;
	    function intersect(set1, set2) {
	        return set2.tree.Case === "SetEmpty" ? set2 : set1.tree.Case === "SetEmpty" ? set1 : from(set1.comparer, tree_intersection(set1.comparer, set1.tree, set2.tree));
	    }
	    exports.intersect = intersect;
	    function intersectInPlace(set1, set2) {
	        var set2_ = set2 instanceof Set ? set2 : new Set(set2);
	        Seq_1.iterate(function (x) {
	            if (!set2_.has(x)) {
	                set1.delete(x);
	            }
	        }, set1);
	    }
	    exports.intersectInPlace = intersectInPlace;
	    function intersectMany(sets) {
	        return Seq_3.reduce(function (s1, s2) {
	            return intersect(s1, s2);
	        }, sets);
	    }
	    exports.intersectMany = intersectMany;
	    function isProperSubsetOf(set1, set2) {
	        if (set1 instanceof FableSet && set2 instanceof FableSet) {
	            return tree_psubset(set1.comparer, set1.tree, set2.tree);
	        } else {
	            set2 = set2 instanceof Set ? set2 : new Set(set2);
	            return Seq_4.forAll(function (x) {
	                return set2.has(x);
	            }, set1) && Seq_5.exists(function (x) {
	                return !set1.has(x);
	            }, set2);
	        }
	    }
	    exports.isProperSubsetOf = isProperSubsetOf;
	    function isProperSubset(set1, set2) {
	        return isProperSubsetOf(set1, set2);
	    }
	    exports.isProperSubset = isProperSubset;
	    function isSubsetOf(set1, set2) {
	        if (set1 instanceof FableSet && set2 instanceof FableSet) {
	            return tree_subset(set1.comparer, set1.tree, set2.tree);
	        } else {
	            set2 = set2 instanceof Set ? set2 : new Set(set2);
	            return Seq_4.forAll(function (x) {
	                return set2.has(x);
	            }, set1);
	        }
	    }
	    exports.isSubsetOf = isSubsetOf;
	    function isSubset(set1, set2) {
	        return isSubsetOf(set1, set2);
	    }
	    exports.isSubset = isSubset;
	    function isProperSupersetOf(set1, set2) {
	        if (set1 instanceof FableSet && set2 instanceof FableSet) {
	            return tree_psubset(set1.comparer, set2.tree, set1.tree);
	        } else {
	            return isProperSubset(set2 instanceof Set ? set2 : new Set(set2), set1);
	        }
	    }
	    exports.isProperSupersetOf = isProperSupersetOf;
	    function isProperSuperset(set1, set2) {
	        return isProperSupersetOf(set1, set2);
	    }
	    exports.isProperSuperset = isProperSuperset;
	    function isSupersetOf(set1, set2) {
	        if (set1 instanceof FableSet && set2 instanceof FableSet) {
	            return tree_subset(set1.comparer, set2.tree, set1.tree);
	        } else {
	            return isSubset(set2 instanceof Set ? set2 : new Set(set2), set1);
	        }
	    }
	    exports.isSupersetOf = isSupersetOf;
	    function isSuperset(set1, set2) {
	        return isSupersetOf(set1, set2);
	    }
	    exports.isSuperset = isSuperset;
	    function copyTo(xs, arr, arrayIndex, count) {
	        if (!Array.isArray(arr) && !ArrayBuffer.isView(arr)) throw new Error("Array is invalid");
	        count = count || arr.length;
	        var i = arrayIndex || 0;
	        var iter = xs[Symbol.iterator]();
	        while (count--) {
	            var el = iter.next();
	            if (el.done) break;
	            arr[i++] = el.value;
	        }
	    }
	    exports.copyTo = copyTo;
	    function partition(f, s) {
	        if (s.tree.Case === "SetEmpty") {
	            return [s, s];
	        } else {
	            var tuple = tree_partition(s.comparer, f, s.tree);
	            return [from(s.comparer, tuple[0]), from(s.comparer, tuple[1])];
	        }
	    }
	    exports.partition = partition;
	    function filter(f, s) {
	        if (s.tree.Case === "SetEmpty") {
	            return s;
	        } else {
	            return from(s.comparer, tree_filter(s.comparer, f, s.tree));
	        }
	    }
	    exports.filter = filter;
	    function map(f, s) {
	        var comparer = new GenericComparer_1.default();
	        return from(comparer, tree_fold(function (acc, k) {
	            return tree_add(comparer, f(k), acc);
	        }, new SetTree("SetEmpty", []), s.tree));
	    }
	    exports.map = map;
	    function exists(f, s) {
	        return tree_exists(f, s.tree);
	    }
	    exports.exists = exists;
	    function forAll(f, s) {
	        return tree_forall(f, s.tree);
	    }
	    exports.forAll = forAll;
	    function fold(f, seed, s) {
	        return tree_fold(f, seed, s.tree);
	    }
	    exports.fold = fold;
	    function foldBack(f, s, seed) {
	        return tree_foldBack(f, s.tree, seed);
	    }
	    exports.foldBack = foldBack;
	    function iterate(f, s) {
	        tree_iter(f, s.tree);
	    }
	    exports.iterate = iterate;
	    function minimumElement(s) {
	        return tree_minimumElement(s.tree);
	    }
	    exports.minimumElement = minimumElement;
	    function minElement(s) {
	        return tree_minimumElement(s.tree);
	    }
	    exports.minElement = minElement;
	    function maximumElement(s) {
	        return tree_maximumElement(s.tree);
	    }
	    exports.maximumElement = maximumElement;
	    function maxElement(s) {
	        return tree_maximumElement(s.tree);
	    }
	    exports.maxElement = maxElement;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 32 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(global, module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports"], function (require, exports) {
	    "use strict";
	
	    var fableGlobal = function () {
	        var globalObj = typeof window !== "undefined" ? window : typeof global !== "undefined" ? global : typeof self !== "undefined" ? self : null;
	        if (typeof globalObj.__FABLE_CORE__ === "undefined") {
	            globalObj.__FABLE_CORE__ = {
	                types: new Map(),
	                symbols: {
	                    reflection: Symbol("reflection")
	                }
	            };
	        }
	        return globalObj.__FABLE_CORE__;
	    }();
	    function setType(fullName, cons) {
	        fableGlobal.types.set(fullName, cons);
	    }
	    exports.setType = setType;
	    function getType(fullName) {
	        return fableGlobal.types.get(fullName);
	    }
	    exports.getType = getType;
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = fableGlobal.symbols;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, (function() { return this; }()), __webpack_require__(3)(module)))

/***/ },
/* 33 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Util", "./Long"], function (require, exports) {
	    "use strict";
	
	    var Util_1 = require("./Util");
	    var Long = require("./Long");
	    function create(d, h, m, s, ms) {
	        if (d === void 0) {
	            d = 0;
	        }
	        if (h === void 0) {
	            h = 0;
	        }
	        if (m === void 0) {
	            m = 0;
	        }
	        if (s === void 0) {
	            s = 0;
	        }
	        if (ms === void 0) {
	            ms = 0;
	        }
	        switch (arguments.length) {
	            case 1:
	                return fromTicks(arguments[0]);
	            case 3:
	                d = 0, h = arguments[0], m = arguments[1], s = arguments[2], ms = 0;
	                break;
	            default:
	                d = arguments[0], h = arguments[1], m = arguments[2], s = arguments[3], ms = arguments[4] || 0;
	                break;
	        }
	        return d * 86400000 + h * 3600000 + m * 60000 + s * 1000 + ms;
	    }
	    exports.create = create;
	    function fromTicks(ticks) {
	        return ticks.div(10000).toNumber();
	    }
	    exports.fromTicks = fromTicks;
	    function fromDays(d) {
	        return create(d, 0, 0, 0);
	    }
	    exports.fromDays = fromDays;
	    function fromHours(h) {
	        return create(h, 0, 0);
	    }
	    exports.fromHours = fromHours;
	    function fromMinutes(m) {
	        return create(0, m, 0);
	    }
	    exports.fromMinutes = fromMinutes;
	    function fromSeconds(s) {
	        return create(0, 0, s);
	    }
	    exports.fromSeconds = fromSeconds;
	    function days(ts) {
	        return Math.floor(ts / 86400000);
	    }
	    exports.days = days;
	    function hours(ts) {
	        return Math.floor(ts % 86400000 / 3600000);
	    }
	    exports.hours = hours;
	    function minutes(ts) {
	        return Math.floor(ts % 3600000 / 60000);
	    }
	    exports.minutes = minutes;
	    function seconds(ts) {
	        return Math.floor(ts % 60000 / 1000);
	    }
	    exports.seconds = seconds;
	    function milliseconds(ts) {
	        return Math.floor(ts % 1000);
	    }
	    exports.milliseconds = milliseconds;
	    function ticks(ts) {
	        return Long.fromNumber(ts).mul(10000);
	    }
	    exports.ticks = ticks;
	    function totalDays(ts) {
	        return ts / 86400000;
	    }
	    exports.totalDays = totalDays;
	    function totalHours(ts) {
	        return ts / 3600000;
	    }
	    exports.totalHours = totalHours;
	    function totalMinutes(ts) {
	        return ts / 60000;
	    }
	    exports.totalMinutes = totalMinutes;
	    function totalSeconds(ts) {
	        return ts / 1000;
	    }
	    exports.totalSeconds = totalSeconds;
	    function negate(ts) {
	        return ts * -1;
	    }
	    exports.negate = negate;
	    function add(ts1, ts2) {
	        return ts1 + ts2;
	    }
	    exports.add = add;
	    function subtract(ts1, ts2) {
	        return ts1 - ts2;
	    }
	    exports.subtract = subtract;
	    function compare(x, y) {
	        return Util_1.compare(x, y);
	    }
	    exports.compare = compare;
	    function compareTo(x, y) {
	        return Util_1.compare(x, y);
	    }
	    exports.compareTo = compareTo;
	    function duration(x) {
	        return Math.abs(x);
	    }
	    exports.duration = duration;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 34 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Event", "./Symbol"], function (require, exports) {
	    "use strict";
	
	    var Event_1 = require("./Event");
	    var Symbol_1 = require("./Symbol");
	    var Timer = function () {
	        function Timer(interval) {
	            this.Interval = interval > 0 ? interval : 100;
	            this.AutoReset = true;
	            this._elapsed = new Event_1.default();
	        }
	        Object.defineProperty(Timer.prototype, "Elapsed", {
	            get: function get() {
	                return this._elapsed;
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Object.defineProperty(Timer.prototype, "Enabled", {
	            get: function get() {
	                return this._enabled;
	            },
	            set: function set(x) {
	                var _this = this;
	                if (!this._isDisposed && this._enabled != x) {
	                    if (this._enabled = x) {
	                        if (this.AutoReset) {
	                            this._intervalId = setInterval(function () {
	                                if (!_this.AutoReset) _this.Enabled = false;
	                                _this._elapsed.Trigger(new Date());
	                            }, this.Interval);
	                        } else {
	                            this._timeoutId = setTimeout(function () {
	                                _this.Enabled = false;
	                                _this._timeoutId = 0;
	                                if (_this.AutoReset) _this.Enabled = true;
	                                _this._elapsed.Trigger(new Date());
	                            }, this.Interval);
	                        }
	                    } else {
	                        if (this._timeoutId) {
	                            clearTimeout(this._timeoutId);
	                            this._timeoutId = 0;
	                        }
	                        if (this._intervalId) {
	                            clearInterval(this._intervalId);
	                            this._intervalId = 0;
	                        }
	                    }
	                }
	            },
	            enumerable: true,
	            configurable: true
	        });
	        Timer.prototype.Dispose = function () {
	            this.Enabled = false;
	            this._isDisposed = true;
	        };
	        Timer.prototype.Close = function () {
	            this.Dispose();
	        };
	        Timer.prototype.Start = function () {
	            this.Enabled = true;
	        };
	        Timer.prototype.Stop = function () {
	            this.Enabled = false;
	        };
	        Timer.prototype[Symbol_1.default.reflection] = function () {
	            return {
	                type: "System.Timers.Timer",
	                interfaces: ["System.IDisposable"]
	            };
	        };
	        return Timer;
	    }();
	    Object.defineProperty(exports, "__esModule", { value: true });
	    exports.default = Timer;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ },
/* 35 */
/***/ function(module, exports, __webpack_require__) {

	/* WEBPACK VAR INJECTION */(function(module) {'use strict';
	
	var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };
	
	(function (dependencies, factory) {
	    if (( false ? 'undefined' : _typeof(module)) === 'object' && _typeof(module.exports) === 'object') {
	        var v = factory(__webpack_require__(5), exports);if (v !== undefined) module.exports = v;
	    } else if (true) {
	        __webpack_require__(7)(dependencies, factory);
	    }
	})(["require", "exports", "./Symbol"], function (require, exports) {
	    "use strict";
	
	    var Symbol_1 = require("./Symbol");
	    var NonDeclaredType = function () {
	        function NonDeclaredType(kind, definition, generics) {
	            this.kind = kind;
	            this.definition = definition;
	            this.generics = generics;
	        }
	        NonDeclaredType.prototype.Equals = function (other) {
	            if (this.kind === other.kind && this.definition === other.definition) {
	                return _typeof(this.generics) === "object" ? equalsRecords(this.generics, other.generics) : this.generics === other.generics;
	            }
	            return false;
	        };
	        return NonDeclaredType;
	    }();
	    exports.NonDeclaredType = NonDeclaredType;
	    exports.Any = new NonDeclaredType("Any");
	    exports.Unit = new NonDeclaredType("Unit");
	    function Option(t) {
	        return new NonDeclaredType("Option", null, t);
	    }
	    exports.Option = Option;
	    function FableArray(t, isTypedArray) {
	        if (isTypedArray === void 0) {
	            isTypedArray = false;
	        }
	        var def = null,
	            genArg = null;
	        if (isTypedArray) {
	            def = t;
	        } else {
	            genArg = t;
	        }
	        return new NonDeclaredType("Array", def, genArg);
	    }
	    exports.Array = FableArray;
	    function Tuple(ts) {
	        return new NonDeclaredType("Tuple", null, ts);
	    }
	    exports.Tuple = Tuple;
	    function GenericParam(definition) {
	        return new NonDeclaredType("GenericParam", definition);
	    }
	    exports.GenericParam = GenericParam;
	    function Interface(definition) {
	        return new NonDeclaredType("Interface", definition);
	    }
	    exports.Interface = Interface;
	    function makeGeneric(typeDef, genArgs) {
	        return new NonDeclaredType("GenericType", typeDef, genArgs);
	    }
	    exports.makeGeneric = makeGeneric;
	    function isGeneric(typ) {
	        return typ instanceof NonDeclaredType && typ.generics != null;
	    }
	    exports.isGeneric = isGeneric;
	    function getDefinition(typ) {
	        return isGeneric(typ) ? typ.definition : typ;
	    }
	    exports.getDefinition = getDefinition;
	    function extendInfo(cons, info) {
	        var parent = Object.getPrototypeOf(cons.prototype);
	        if (typeof parent[Symbol_1.default.reflection] === "function") {
	            var newInfo_1 = {},
	                parentInfo_1 = parent[Symbol_1.default.reflection]();
	            Object.getOwnPropertyNames(info).forEach(function (k) {
	                var i = info[k];
	                if ((typeof i === 'undefined' ? 'undefined' : _typeof(i)) === "object") {
	                    newInfo_1[k] = Array.isArray(i) ? (parentInfo_1[k] || []).concat(i) : Object.assign(parentInfo_1[k] || {}, i);
	                } else {
	                    newInfo_1[k] = i;
	                }
	            });
	            return newInfo_1;
	        }
	        return info;
	    }
	    exports.extendInfo = extendInfo;
	    function hasInterface(obj, interfaceName) {
	        if (interfaceName === "System.Collections.Generic.IEnumerable") {
	            return typeof obj[Symbol.iterator] === "function";
	        } else if (typeof obj[Symbol_1.default.reflection] === "function") {
	            var interfaces = obj[Symbol_1.default.reflection]().interfaces;
	            return Array.isArray(interfaces) && interfaces.indexOf(interfaceName) > -1;
	        }
	        return false;
	    }
	    exports.hasInterface = hasInterface;
	    function isArray(obj) {
	        return Array.isArray(obj) || ArrayBuffer.isView(obj);
	    }
	    exports.isArray = isArray;
	    function getRestParams(args, idx) {
	        for (var _len = args.length, restArgs = Array(_len > idx ? _len - idx : 0), _key = idx; _key < _len; _key++) {
	            restArgs[_key - idx] = args[_key];
	        }return restArgs;
	    }
	    exports.getRestParams = getRestParams;
	    function toString(o) {
	        return o != null && typeof o.ToString == "function" ? o.ToString() : String(o);
	    }
	    exports.toString = toString;
	    function hash(x) {
	        var s = JSON.stringify(x);
	        var h = 5381,
	            i = 0,
	            len = s.length;
	        while (i < len) {
	            h = h * 33 ^ s.charCodeAt(i++);
	        }
	        return h;
	    }
	    exports.hash = hash;
	    function equals(x, y) {
	        if (x === y) return true;else if (x == null) return y == null;else if (y == null) return false;else if (Object.getPrototypeOf(x) !== Object.getPrototypeOf(y)) return false;else if (typeof x.Equals === "function") return x.Equals(y);else if (Array.isArray(x)) {
	            if (x.length != y.length) return false;
	            for (var i = 0; i < x.length; i++) {
	                if (!equals(x[i], y[i])) return false;
	            }return true;
	        } else if (ArrayBuffer.isView(x)) {
	            if (x.byteLength !== y.byteLength) return false;
	            var dv1 = new DataView(x.buffer),
	                dv2 = new DataView(y.buffer);
	            for (var i = 0; i < x.byteLength; i++) {
	                if (dv1.getUint8(i) !== dv2.getUint8(i)) return false;
	            }return true;
	        } else if (x instanceof Date) return x.getTime() == y.getTime();else return false;
	    }
	    exports.equals = equals;
	    function compare(x, y) {
	        if (x === y) return 0;
	        if (x == null) return y == null ? 0 : -1;else if (y == null) return 1;else if (Object.getPrototypeOf(x) !== Object.getPrototypeOf(y)) return -1;else if (typeof x.CompareTo === "function") return x.CompareTo(y);else if (Array.isArray(x)) {
	            if (x.length != y.length) return x.length < y.length ? -1 : 1;
	            for (var i = 0, j = 0; i < x.length; i++) {
	                if ((j = compare(x[i], y[i])) !== 0) return j;
	            }return 0;
	        } else if (ArrayBuffer.isView(x)) {
	            if (x.byteLength != y.byteLength) return x.byteLength < y.byteLength ? -1 : 1;
	            var dv1 = new DataView(x.buffer),
	                dv2 = new DataView(y.buffer);
	            for (var i = 0, b1 = 0, b2 = 0; i < x.byteLength; i++) {
	                b1 = dv1.getUint8(i), b2 = dv2.getUint8(i);
	                if (b1 < b2) return -1;
	                if (b1 > b2) return 1;
	            }
	            return 0;
	        } else if (x instanceof Date) return compare(x.getTime(), y.getTime());else return x < y ? -1 : 1;
	    }
	    exports.compare = compare;
	    function equalsRecords(x, y) {
	        if (x === y) {
	            return true;
	        } else {
	            var keys = Object.getOwnPropertyNames(x);
	            for (var i = 0; i < keys.length; i++) {
	                if (!equals(x[keys[i]], y[keys[i]])) return false;
	            }
	            return true;
	        }
	    }
	    exports.equalsRecords = equalsRecords;
	    function compareRecords(x, y) {
	        if (x === y) {
	            return 0;
	        } else {
	            var keys = Object.getOwnPropertyNames(x);
	            for (var i = 0; i < keys.length; i++) {
	                var res = compare(x[keys[i]], y[keys[i]]);
	                if (res !== 0) return res;
	            }
	            return 0;
	        }
	    }
	    exports.compareRecords = compareRecords;
	    function equalsUnions(x, y) {
	        if (x === y) {
	            return true;
	        } else if (x.Case !== y.Case) {
	            return false;
	        } else {
	            for (var i = 0; i < x.Fields.length; i++) {
	                if (!equals(x.Fields[i], y.Fields[i])) return false;
	            }
	            return true;
	        }
	    }
	    exports.equalsUnions = equalsUnions;
	    function compareUnions(x, y) {
	        if (x === y) {
	            return 0;
	        } else {
	            var res = compare(x.Case, y.Case);
	            if (res !== 0) return res;
	            for (var i = 0; i < x.Fields.length; i++) {
	                res = compare(x.Fields[i], y.Fields[i]);
	                if (res !== 0) return res;
	            }
	            return 0;
	        }
	    }
	    exports.compareUnions = compareUnions;
	    function createDisposable(f) {
	        return _a = {
	            Dispose: f
	        }, _a[Symbol_1.default.reflection] = function () {
	            return { interfaces: ["System.IDisposable"] };
	        }, _a;
	        var _a;
	    }
	    exports.createDisposable = createDisposable;
	    function createObj(fields) {
	        var iter = fields[Symbol.iterator]();
	        var cur = iter.next(),
	            o = {};
	        while (!cur.done) {
	            o[cur.value[0]] = cur.value[1];
	            cur = iter.next();
	        }
	        return o;
	    }
	    exports.createObj = createObj;
	    function toPlainJsObj(source) {
	        if (source != null && source.constructor != Object) {
	            var target = {};
	            var props = Object.getOwnPropertyNames(source);
	            for (var i = 0; i < props.length; i++) {
	                target[props[i]] = source[props[i]];
	            }
	            var proto = Object.getPrototypeOf(source);
	            if (proto != null) {
	                props = Object.getOwnPropertyNames(proto);
	                for (var i = 0; i < props.length; i++) {
	                    var prop = Object.getOwnPropertyDescriptor(proto, props[i]);
	                    if (prop.value) {
	                        target[props[i]] = prop.value;
	                    } else if (prop.get) {
	                        target[props[i]] = prop.get.apply(source);
	                    }
	                }
	            }
	            return target;
	        } else {
	            return source;
	        }
	    }
	    exports.toPlainJsObj = toPlainJsObj;
	    function round(value, digits) {
	        if (digits === void 0) {
	            digits = 0;
	        }
	        var m = Math.pow(10, digits);
	        var n = +(digits ? value * m : value).toFixed(8);
	        var i = Math.floor(n),
	            f = n - i;
	        var e = 1e-8;
	        var r = f > 0.5 - e && f < 0.5 + e ? i % 2 == 0 ? i : i + 1 : Math.round(n);
	        return digits ? r / m : r;
	    }
	    exports.round = round;
	    function randomNext(min, max) {
	        return Math.floor(Math.random() * (max - min)) + min;
	    }
	    exports.randomNext = randomNext;
	    function defaultArg(arg, defaultValue) {
	        return arg == null ? defaultValue : arg;
	    }
	    exports.defaultArg = defaultArg;
	});
	/* WEBPACK VAR INJECTION */}.call(exports, __webpack_require__(3)(module)))

/***/ }
/******/ ]);
//# sourceMappingURL=bundle.js.map