"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const expect = require("expect");
const NeoJs_1 = require("../NeoJs");
let neo = new NeoJs_1.default({
    scriptHash: '5b8de0bb6790c711080a43c5ee9f1945f61210ca'
});
describe("NFT Tokens", function () {
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
    it('should return tokens of owner', async () => {
        let result = await neo.get('tokensOfOwner', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address')]);
        console.log('tokensOfOwner', result);
    });
});
//# sourceMappingURL=market-sc.spec.js.map