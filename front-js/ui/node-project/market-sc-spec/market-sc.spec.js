import * as expect from "expect";
import NeoJs from "../NeoJs";

let neo = new NeoJs({
    scriptHash: 'accdbdb7d084fef793c3e1d460dc1a729528bd79' //market #03.11.18 15:38
});

describe("Market Tokens", function () {
    before(async () => {
        // let result = await neo.call('mintToken', [neo.sc.ContractParam.byteArray(neo.config.myAddress, 'address')]);
    });
});
