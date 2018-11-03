angular.module('marketplace').factory('auctionService', function ($http) {
    let baseUrl = 'http://localhost:5001/api';
    var auctionService = {
        get() {
            return $http.get(`${baseUrl}/auction`).then(response => response.data);
        },
        myTokens(address) {
            let otherAddress = window.neo.getByteArrayAddress(address);
            return $http.get(`${baseUrl}/ownedTokens?address=${otherAddress.value}`)
                .catch(err => {
                    console.error(err)
                    throw err;
                })
                .then(response => response.data);
        }
    };

    return auctionService;
});
