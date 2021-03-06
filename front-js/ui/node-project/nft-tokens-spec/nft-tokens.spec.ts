import * as expect from "expect";
import NeoJs from "../NeoJs";
import * as _ from 'lodash';
import Aigle from "aigle";

let neo = new NeoJs({
    scriptHash: '577c60593353d5f91a2190f1db5d6fc7f3734164' //nft #03.11.18 16:22
});

describe("NFT Tokens", function () {
    before(async () => {
        let result = await neo.get('totalSupply', []);
        if (result[0].value !== '02') {
            console.log('MINTING 2 TOKENS!!!');
            await neo.call('mintToken', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address'), 50, 50, 50, 50, 0]);
            await neo.call('mintToken', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address'), 70, 50, 40, 20, 0]);
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
            await Aigle.forEach(tokenIds, async (tokenId) => {
                let tokensOfOwnerResult = await neo.get('ownerOf', [tokenId]);
                console.log(tokenId, tokensOfOwnerResult);
            });
        });

    });
});
