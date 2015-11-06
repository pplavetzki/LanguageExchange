/**
 * Created by paul on 5/22/15.
 */
'use strict';

var servicesModule = require('./index');

var commonDataService = commonDataService;

commonDataService.$inject = ['$http']

function commonDataService($http){

    var service = {
        getCountries:getCountries,
        getStates: getStates,
        getValues: getValues,
        getValue: getValue
    };

    return service;

    function getCountries(){
        return $http.get('/data/countries.json')
            .then(complete)
            .catch(failed);

        function complete(response){
            return response.data;
        }

        function failed(error){
            return console.log(error);
        }
    }

    function getStates(){
        return $http.get('/data/states.json')
            .then(complete)
            .catch(failed);

        function complete(response){
            return response.data;
        }

        function failed(error){
            return console.log(error);
        }
    }

    function getValues() {
        $http.defaults.headers.common.Authorization = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJhY2Nlc3MiOiJmdWxsIiwic2NvcGUiOiJXZWJCcmlkZ2UiLCJjbGllbnQiOiJQYXJlaWRvbGlhU1ciLCJpc3MiOiJBdXRob3JpemF0aW9uU2VydmVyIiwiYXVkIjoiaHR0cDovL3BhcmVpZG9saWFzdyIsImV4cCI6MTQ0NTIxMTQ0NywibmJmIjoxNDQ1MjA3ODQ3fQ.uEY8aBffpngtIUG4LTDMBrG1Ps65Jlh3kCguJkzSe74"
        
        return $http.get('http://localhost:49425/api/values')
            .then(complete)
            .catch(failed);
        
        function complete(response) {
            return response.data;
        }
        
        function failed(error) {
            return console.log(error);
        }
    }

    function getValue(id) {
        return $http.get('http://localhost:49425/api/values/' + id)
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

servicesModule.factory('commonDataService', commonDataService);