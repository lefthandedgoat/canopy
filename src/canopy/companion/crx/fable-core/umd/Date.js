import { create as timeSpanCreate } from "./TimeSpan";
import { compare as utilCompare } from "./Util";
import * as Long from "./Long";
export function minValue() {
    return parse(-8640000000000000, 1);
}
export function maxValue() {
    return parse(8640000000000000, 1);
}
export function parse(v, kind) {
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
export function tryParse(v) {
    try {
        return [true, parse(v)];
    }
    catch (_err) {
        return [false, minValue()];
    }
}
export function create(year, month, day, h, m, s, ms, kind) {
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
export function now() {
    return parse();
}
export function utcNow() {
    return parse(null, 1);
}
export function today() {
    return date(now());
}
export function isLeapYear(year) {
    return year % 4 == 0 && year % 100 != 0 || year % 400 == 0;
}
export function daysInMonth(year, month) {
    return month == 2
        ? isLeapYear(year) ? 29 : 28
        : month >= 8 ? month % 2 == 0 ? 31 : 30 : month % 2 == 0 ? 30 : 31;
}
export function toUniversalTime(d) {
    return d.kind === 2 ? new Date(d.getTime()) : d;
}
export function toLocalTime(d) {
    if (d.kind === 2) {
        return d;
    }
    else {
        var d2 = new Date(d.getTime());
        d2.kind = 2;
        return d2;
    }
}
export function timeOfDay(d) {
    return timeSpanCreate(0, hour(d), minute(d), second(d), millisecond(d));
}
export function date(d) {
    return create(year(d), month(d), day(d), 0, 0, 0, 0, d.kind || 1);
}
export function kind(d) {
    return d.kind || 1;
}
export function day(d) {
    return d.kind === 2 ? d.getDate() : d.getUTCDate();
}
export function hour(d) {
    return d.kind === 2 ? d.getHours() : d.getUTCHours();
}
export function millisecond(d) {
    return d.kind === 2 ? d.getMilliseconds() : d.getUTCMilliseconds();
}
export function minute(d) {
    return d.kind === 2 ? d.getMinutes() : d.getUTCMinutes();
}
export function month(d) {
    return (d.kind === 2 ? d.getMonth() : d.getUTCMonth()) + 1;
}
export function second(d) {
    return d.kind === 2 ? d.getSeconds() : d.getUTCSeconds();
}
export function year(d) {
    return d.kind === 2 ? d.getFullYear() : d.getUTCFullYear();
}
export function dayOfWeek(d) {
    return d.kind === 2 ? d.getDay() : d.getUTCDay();
}
export function ticks(d) {
    return Long.fromNumber(d.getTime())
        .add(62135596800000)
        .sub(d.kind == 2 ? d.getTimezoneOffset() * 60 * 1000 : 0)
        .mul(10000);
}
export function toBinary(d) {
    return ticks(d);
}
export function dayOfYear(d) {
    var _year = year(d);
    var _month = month(d);
    var _day = day(d);
    for (var i = 1; i < _month; i++)
        _day += daysInMonth(_year, i);
    return _day;
}
export function add(d, ts) {
    return parse(d.getTime() + ts, d.kind || 1);
}
export function addDays(d, v) {
    return parse(d.getTime() + v * 86400000, d.kind || 1);
}
export function addHours(d, v) {
    return parse(d.getTime() + v * 3600000, d.kind || 1);
}
export function addMinutes(d, v) {
    return parse(d.getTime() + v * 60000, d.kind || 1);
}
export function addSeconds(d, v) {
    return parse(d.getTime() + v * 1000, d.kind || 1);
}
export function addMilliseconds(d, v) {
    return parse(d.getTime() + v, d.kind || 1);
}
export function addTicks(d, t) {
    return parse(Long.fromNumber(d.getTime()).add(t.div(10000)).toNumber(), d.kind || 1);
}
export function addYears(d, v) {
    var newMonth = month(d);
    var newYear = year(d) + v;
    var _daysInMonth = daysInMonth(newYear, newMonth);
    var newDay = Math.min(_daysInMonth, day(d));
    return create(newYear, newMonth, newDay, hour(d), minute(d), second(d), millisecond(d), d.kind || 1);
}
export function addMonths(d, v) {
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
export function subtract(d, that) {
    return typeof that == "number"
        ? parse(d.getTime() - that, d.kind || 1)
        : d.getTime() - that.getTime();
}
export function toLongDateString(d) {
    return d.toDateString();
}
export function toShortDateString(d) {
    return d.toLocaleDateString();
}
export function toLongTimeString(d) {
    return d.toLocaleTimeString();
}
export function toShortTimeString(d) {
    return d.toLocaleTimeString().replace(/:\d\d(?!:)/, "");
}
export function equals(d1, d2) {
    return d1.getTime() == d2.getTime();
}
export function compare(x, y) {
    return utilCompare(x, y);
}
export function compareTo(x, y) {
    return utilCompare(x, y);
}
export function op_Addition(x, y) {
    return add(x, y);
}
export function op_Subtraction(x, y) {
    return subtract(x, y);
}
