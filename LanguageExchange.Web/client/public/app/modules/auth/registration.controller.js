'use strict';

var authModule = require('./index');
var Registration = Registration;

Registration.$inject = ['$scope', 'authServices'];

function Registration($scope, authServices) {
    var vm = this;
    
    function submit() {
        console.log('register');
        if ($scope.registrationForm.$valid) {
            var data = {
                password: vm.password,
                userName: vm.userName,
                email: vm.email
            };
            authServices.quickRegister(data).then(function (data) {
                console.log(data);
            });
        }
    }

    vm.submit = submit;

};

authModule.controller('Registration', Registration);