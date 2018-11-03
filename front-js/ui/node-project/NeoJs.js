"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const get = require("./get");
const call = require("./call");
const neoConfig = require("./neoConfig");
const neon_js_1 = require("@cityofzion/neon-js");
const _ = require("lodash");
class NeoJs {
    constructor(config) {
        this.config = _.merge(neoConfig, config);
        this.Neon = neon_js_1.default;
        this.sc = neon_js_1.sc;
    }
    call(...args) {
        return call(...args, this.config);
    }
    get(...args) {
        return get.apply(null, args.concat([this.config]));
    }
}
exports.default = NeoJs;
//# sourceMappingURL=NeoJs.js.map