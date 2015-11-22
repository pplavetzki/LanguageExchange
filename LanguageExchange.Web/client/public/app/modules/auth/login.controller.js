/**
 * Created by paul on 5/11/15.
 */
/**
 * Created by paul on 5/7/15.
 */
'use strict';

var authModule = require('./index');
var Login = Login;

Login.$inject = ['$timeout', '$window', '$state', '$scope', 'authServices'];

function Login ($timeout, $window, $state, $scope, authServices) {
    var vm = this;

    vm.submit = submit;
    
    authServices.authorize().then(function (data) {
        if (data && data.token) {
            vm.token = data.token;
            console.log(vm.token);
        }
    });

    function submit() {
        if($scope.loginform.$valid) {
            if (vm.username === vm.password) {
                $window.sessionStorage.token = 'let me in!';
                $state.go('app.dashboard');
            }
        }
    }

};

authModule.controller('Login', Login);