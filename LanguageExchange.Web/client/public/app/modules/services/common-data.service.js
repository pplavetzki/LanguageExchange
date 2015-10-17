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
        $http.defaults.headers.common.Authorization = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ3ZWJzaXRlIjoiaHR0cDovL2xvY2FsaG9zdDoxMDEwMCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3VyaSI6Imh0dHA6Ly9sb2NhbGhvc3QiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiI2MzU4MDczMTUyMjA1NjI1MjIiLCJzY29wZSI6IldlYkJyaWRnZSIsImlzcyI6IlBhcmVpZG9saWFTVyIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6MTAxMDAiLCJleHAiOjE0NDUxMzQ3MjIsIm5iZiI6MTQ0NTEwNTkyMn0.q404E23ESSwb5fRFQohJI6kSYvGG1b5x-AB3wsD2uqA";

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