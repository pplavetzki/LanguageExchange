'use strict';

var servicesModule = require('./index');

var authServices = authServices;

authServices.$inject = ['$http', 'constants']

function authServices($http, constants) {

    var service = {
        confirmEmail: confirmEmail,
        authorize: authorize
    };

    return service;

    function confirmEmail(userId, code) {
    	return $http.get(constants.apiBaseUrl + 'account/confirm?userId=' + userId + '&code=' + encodeURIComponent(code))
            .then(complete)
            .catch(failed);

        function complete(response) {
            return response.data;
        }
        
        function failed(error) {
            return console.log(error);
        }
    }

    function authorize() {
        return $http.get(constants.appBaseUrl + 'auth/scope')
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