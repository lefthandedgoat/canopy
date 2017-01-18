(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
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
        if (options === void 0) { options = 0; }
        var reg = str instanceof RegExp
            ? (reg = str, str = pattern, reg.lastIndex = options, reg)
            : reg = create(pattern, options);
        return reg.test(str);
    }
    exports.isMatch = isMatch;
    function match(str, pattern, options) {
        if (options === void 0) { options = 0; }
        var reg = str instanceof RegExp
            ? (reg = str, str = pattern, reg.lastIndex = options, reg)
            : reg = create(pattern, options);
        return reg.exec(str);
    }
    exports.match = match;
    function matches(str, pattern, options) {
        if (options === void 0) { options = 0; }
        var reg = str instanceof RegExp
            ? (reg = str, str = pattern, reg.lastIndex = options, reg)
            : reg = create(pattern, options);
        if (!reg.global)
            throw new Error("Non-global RegExp");
        var m;
        var matches = [];
        while ((m = reg.exec(str)) !== null)
            matches.push(m);
        return matches;
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
        if (offset === void 0) { offset = 0; }
        function replacer() {
            var res = arguments[0];
            if (limit !== 0) {
                limit--;
                var match_1 = [];
                var len = arguments.length;
                for (var i = 0; i < len - 2; i++)
                    match_1.push(arguments[i]);
                match_1.index = arguments[len - 2];
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
        }
        else {
            if (limit != null) {
                var m = void 0;
                var sub1 = input.substring(offset);
                var _matches = matches(reg, sub1);
                var sub2 = matches.length > limit ? (m = _matches[limit - 1], sub1.substring(0, m.index + m[0].length)) : sub1;
                return input.substring(0, offset) + sub2.replace(reg, replacement) + input.substring(offset + sub2.length);
            }
            else {
                return input.replace(reg, replacement);
            }
        }
    }
    exports.replace = replace;
    function split(reg, input, limit, offset) {
        if (offset === void 0) { offset = 0; }
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
