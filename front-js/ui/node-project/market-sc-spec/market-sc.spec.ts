import * as expect from "expect";
import NeoJs from "../NeoJs";

let neo = new NeoJs({
    scriptHash: '5b8de0bb6790c711080a43c5ee9f1945f61210ca' //nft #03.11.18 15:36
});

describe("NFT Tokens", function () {
    before(async () => {
        // let result = await neo.call('mintToken', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address')]);
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
