"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const expect = require("expect");
const NeoJs_1 = require("../NeoJs");
let neo = new NeoJs_1.default({
    scriptHash: 'e68cb21b3c7d2453b06164e97d9935a6381ec100'
});
describe("Token Expiration", function () {
    before(async () => {
    });
    it('should return 01 balanceOf', async () => {
        let result = await neo.get('balanceOf', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address')]);
        expect(result[0].value).toEqual('01');
    });
    it('should return 01 totalSupply', async () => {
        let result = await neo.get('totalSupply', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address')]);
        expect(result[0].value).toEqual('01');
    });
    it('should not have active Lend on fresh token', async () => {
        let result = await neo.get('isLendActive', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address')]);
        console.log('lend', result);
    });
    it('should return tokens of owner', async () => {
        let result = await neo.get('tokensOfOwner', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address')]);
        console.log('tokensOfOwner', result);
    });
});
//# sourceMappingURL=token-expiration-basic.spec.js.map