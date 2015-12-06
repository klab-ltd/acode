"use strict";

class Code {
    static getStatical(seed, enableSpecialASCIISet) {
        return Code._get(seed, enableSpecialASCIISet);
    }

    static getRandomly(seed, enableSpecialASCIISet) {
        var seedNum = Math.random().toString();
        var seedTime = Date.now().toString();
        return Code._get(seedNum + seedTime + seed, enableSpecialASCIISet);
    }

    static _get(seed, enableSpecialASCIISet) {
        var enableASCIISet = Code._enableDefaultASCIISet;
        if ((typeof enableSpecialASCIISet) === 'string') {
            for (var i = 0; i < enableSpecialASCIISet.length; i++) {
                var num = enableSpecialASCIISet[i].charCodeAt(0);
                if (num >= 0 && num <= 126)
                    enableASCIISet += enableSpecialASCIISet[i];
            }
        }

        var sha = require('crypto');
        var calc = sha.createHash('sha512');
        calc.update(seed, 'utf8');
        var raw = calc.digest();
        for (var i = 0; i < raw.length; i++)
            raw[i] = 255 - raw[i];

        calc = sha.createHash('sha512');
        calc.update(raw);
        raw = calc.digest();
        var code = '';
        for (var i = 0; i < raw.length; i++) {
            var c = String.fromCharCode(raw[i]);
            if (enableASCIISet.indexOf(c) < 0)
                code += raw[i].toString('16').toLowerCase().slice(-1);
            else
                code += c;
        }

        return code;
    }

    static get _enableDefaultASCIISet() {
        return '0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ';
    }

    static get printableSpecialASCIISet() {
        return '!"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~';
    }
}

exports.code = Code;
