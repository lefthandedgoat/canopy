(function (dependencies, factory) {
    if (typeof module === 'object' && typeof module.exports === 'object') {
        var v = factory(require, exports); if (v !== undefined) module.exports = v;
    }
    else if (typeof define === 'function' && define.amd) {
        define(dependencies, factory);
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
        var date = (v == null) ? new Date() : new Date(v);
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
        }
        catch (_err) {
            return [false, minValue()];
        }
    }
    exports.tryParse = tryParse;
    function create(year, month, day, h, m, s, ms, kind) {
        if (h === void 0) { h = 0; }
        if (m === void 0) { m = 0; }
        if (s === void 0) { s = 0; }
        if (ms === void 0) { ms = 0; }
        if (kind === void 0) { kind = 2; }
        var date;
        if (kind === 2) {
            date = new Date(year, month - 1, day, h, m, s, ms);
            date.kind = kind;
        }
        else {
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
        return month == 2
            ? isLeapYear(year) ? 29 : 28
            : month >= 8 ? month % 2 == 0 ? 31 : 30 : month % 2 == 0 ? 30 : 31;
    }
    exports.daysInMonth = daysInMonth;
    function toUniversalTime(d) {
        return d.kind === 2 ? new Date(d.getTime()) : d;
    }
    exports.toUniversalTime = toUniversalTime;
    function toLocalTime(d) {
        if (d.kind === 2) {
            return d;
        }
        else {
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
        return Long.fromNumber(d.getTime())
            .add(62135596800000)
            .sub(d.kind == 2 ? d.getTimezoneOffset() * 60 * 1000 : 0)
            .mul(10000);
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
        for (var i = 1; i < _month; i++)
            _day += daysInMonth(_year, i);
        return _day;
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
        }
        else if (newMonth < 1) {
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
        return typeof that == "number"
            ? parse(d.getTime() - that, d.kind || 1)
            : d.getTime() - that.getTime();
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
