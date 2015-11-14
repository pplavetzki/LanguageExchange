'use strict';

var servicesModule = require('./index');

var authServices = authServices;

authServices.$inject = ['$http']

function authServices($http) {

    var service = {
    	confirmEmail: confirmEmail
    };

    return service;

    function confirmEmail(userId, code) {
    	return $http.get('http://localhost:49425/api/account/confirm?userId=' + userId + '&code=' + encodeURIComponent(code))
            .then(complete)
            .catch(failed);

        function complete(response) {
            return response.data;
        }
        
        function failed(error) {
            return console.log(error);
        }
    }
}

servicesModule.factory('authServices', authServices);