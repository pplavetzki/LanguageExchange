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

    function authorize(data) {
        return $http.post(constants.appBaseUrl + 'auth/login', data)
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