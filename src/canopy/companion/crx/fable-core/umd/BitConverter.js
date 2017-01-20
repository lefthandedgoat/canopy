import * as Long from "./Long";
var littleEndian = true;
export function isLittleEndian() {
    return littleEndian;
}
export function getBytesBoolean(value) {
    var bytes = new Uint8Array(1);
    new DataView(bytes.buffer).setUint8(0, value ? 1 : 0);
    return bytes;
}
export function getBytesChar(value) {
    var bytes = new Uint8Array(2);
    new DataView(bytes.buffer).setUint16(0, value.charCodeAt(0), littleEndian);
    return bytes;
}
export function getBytesInt16(value) {
    var bytes = new Uint8Array(2);
    new DataView(bytes.buffer).setInt16(0, value, littleEndian);
    return bytes;
}
export function getBytesInt32(value) {
    var bytes = new Uint8Array(4);
    new DataView(bytes.buffer).setInt32(0, value, littleEndian);
    return bytes;
}
export function getBytesInt64(value) {
    var bytes = new Uint8Array(8);
    new DataView(bytes.buffer).setInt32(littleEndian ? 0 : 4, value.getLowBits(), littleEndian);
    new DataView(bytes.buffer).setInt32(littleEndian ? 4 : 0, value.getHighBits(), littleEndian);
    return bytes;
}
export function getBytesUInt16(value) {
    var bytes = new Uint8Array(2);
    new DataView(bytes.buffer).setUint16(0, value, littleEndian);
    return bytes;
}
export function getBytesUInt32(value) {
    var bytes = new Uint8Array(4);
    new DataView(bytes.buffer).setUint32(0, value, littleEndian);
    return bytes;
}
export function getBytesUInt64(value) {
    var bytes = new Uint8Array(8);
    new DataView(bytes.buffer).setUint32(littleEndian ? 0 : 4, value.getLowBitsUnsigned(), littleEndian);
    new DataView(bytes.buffer).setUint32(littleEndian ? 4 : 0, value.getHighBitsUnsigned(), littleEndian);
    return bytes;
}
export function getBytesSingle(value) {
    var bytes = new Uint8Array(4);
    new DataView(bytes.buffer).setFloat32(0, value, littleEndian);
    return bytes;
}
export function getBytesDouble(value) {
    var bytes = new Uint8Array(8);
    new DataView(bytes.buffer).setFloat64(0, value, littleEndian);
    return bytes;
}
export function int64BitsToDouble(value) {
    var buffer = new ArrayBuffer(8);
    new DataView(buffer).setInt32(littleEndian ? 0 : 4, value.getLowBits(), littleEndian);
    new DataView(buffer).setInt32(littleEndian ? 4 : 0, value.getHighBits(), littleEndian);
    return new DataView(buffer).getFloat64(0, littleEndian);
}
export function doubleToInt64Bits(value) {
    var buffer = new ArrayBuffer(8);
    new DataView(buffer).setFloat64(0, value, littleEndian);
    var lowBits = new DataView(buffer).getInt32(littleEndian ? 0 : 4, littleEndian);
    var highBits = new DataView(buffer).getInt32(littleEndian ? 4 : 0, littleEndian);
    return Long.fromBits(lowBits, highBits, false);
}
export function toBoolean(bytes, offset) {
    return new DataView(bytes.buffer).getUint8(offset) === 1 ? true : false;
}
export function toChar(bytes, offset) {
    var code = new DataView(bytes.buffer).getUint16(offset, littleEndian);
    return String.fromCharCode(code);
}
export function toInt16(bytes, offset) {
    return new DataView(bytes.buffer).getInt16(offset, littleEndian);
}
export function toInt32(bytes, offset) {
    return new DataView(bytes.buffer).getInt32(offset, littleEndian);
}
export function toInt64(bytes, offset) {
    var lowBits = new DataView(bytes.buffer).getInt32(offset + (littleEndian ? 0 : 4), littleEndian);
    var highBits = new DataView(bytes.buffer).getInt32(offset + (littleEndian ? 4 : 0), littleEndian);
    return Long.fromBits(lowBits, highBits, false);
}
export function toUInt16(bytes, offset) {
    return new DataView(bytes.buffer).getUint16(offset, littleEndian);
}
export function toUInt32(bytes, offset) {
    return new DataView(bytes.buffer).getUint32(offset, littleEndian);
}
export function toUInt64(bytes, offset) {
    var lowBits = new DataView(bytes.buffer).getUint32(offset + (littleEndian ? 0 : 4), littleEndian);
    var highBits = new DataView(bytes.buffer).getUint32(offset + (littleEndian ? 4 : 0), littleEndian);
    return Long.fromBits(lowBits, highBits, true);
}
export function toSingle(bytes, offset) {
    return new DataView(bytes.buffer).getFloat32(offset, littleEndian);
}
export function toDouble(bytes, offset) {
    return new DataView(bytes.buffer).getFloat64(offset, littleEndian);
}
export function toString(bytes, offset, count) {
    var ar = bytes;
    if (typeof offset !== "undefined" && typeof count !== "undefined")
        ar = bytes.subarray(offset, offset + count);
    else if (typeof offset !== "undefined")
        ar = bytes.subarray(offset);
    return Array.from(ar).map(function (b) { return ("0" + b.toString(16)).slice(-2); }).join("-");
}
