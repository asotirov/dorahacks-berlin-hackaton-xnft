"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const expect = require("expect");
const NeoJs_1 = require("../NeoJs");
const _ = require("lodash");
const aigle_1 = require("aigle");
let neo = new NeoJs_1.default({
    scriptHash: 'ae0e436c61d4568a0cb8c37997bcdc3cab99f6f2'
});
describe("NFT Tokens", function () {
    before(async () => {
        let result = await neo.get('totalSupply', []);
        if (result[0].value !== '02') {
            console.log('MINTING 2 TOKENS!!!');
            await neo.call('mintToken', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address'), 50, 50, 50, 50, 30, 30, 33, 33, 0]);
            await neo.call('mintToken', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address'), 70, 50, 40, 20, 51, 34, 56, 11, 0]);
        }
    });
    it('should return 02 balanceOf', async () => {
        let result = await neo.get('balanceOf', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address')]);
        expect(result[0].value).toEqual('02');
    });
    it('should return 02 totalSupply', async () => {
        let result = await neo.get('totalSupply', []);
        expect(result[0].value).toEqual('02');
    });
    describe("tokens of owner", function () {
        let tokenIds;
        before(async () => {
            let tokensOfOwnerResult = await neo.get('tokensOfOwner', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address')]);
            tokenIds = _.map(tokensOfOwnerResult[0].value, 'value');
        });
        it('2 tokens minted', async () => {
            expect(tokenIds.length).toEqual(2);
        });
        it('owner of tokens is me', async () => {
            await aigle_1.default.forEach(tokenIds, async (tokenId) => {
                let tokensOfOwnerResult = await neo.get('ownerOf', [tokenId]);
                console.log(tokenId, tokensOfOwnerResult);
            });
        });
    });
});
//# sourceMappingURL=nft-tokens.spec.js.map