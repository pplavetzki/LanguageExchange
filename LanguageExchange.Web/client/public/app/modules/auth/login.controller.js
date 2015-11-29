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

    function submit() {
        if ($scope.loginform.$valid) {
            var data = {
                password: vm.password,
                userName: vm.username
            };
            authServices.authorize(data).then(function (data) {
                if (data && data.access_token) {
                    $window.sessionStorage.token = data.access_token;
                    $state.go('app.dashboard.greetings');
                }
                else {
                    if (data.error_description == 'Email is invalid.') {
                        $state.go('reconfirmation', { userName: vm.username });
                    }
                }
            });
        }
    }

};

authModule.controller('Login', Login);