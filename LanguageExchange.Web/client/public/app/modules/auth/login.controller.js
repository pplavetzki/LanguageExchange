/**
 * Created by paul on 5/11/15.
 */
/**
 * Created by paul on 5/7/15.
 */
'use strict';

var authModule = require('./index');
var Login = Login;

Login.$inject = ['$timeout', '$window', '$state', '$scope'];

function Login ($timeout, $window, $state, $scope) {
    var vm = this;

    vm.submit = submit;

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