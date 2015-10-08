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

    vm.sso = sso;
    vm.submit = submit;

    function activate(){
        $timeout(function(){
            // Inline Admin-Form example
            $('#ssoLink').magnificPopup({
                removalDelay: 500, //delay removal by X to allow out-animation,
                mainClass: 'mfp-with-fade',
                type: 'inline',
                midClick: true // allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source.
            });
            $('#register').magnificPopup({
                removalDelay: 500, //delay removal by X to allow out-animation,
                mainClass: 'mfp-with-fade',
                type: 'inline',
                midClick: true // allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source.
            });
            $('#newAccount').magnificPopup({
                removalDelay: 500, //delay removal by X to allow out-animation,
                mainClass: 'mfp-with-fade',
                type: 'inline',
                midClick: true // allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source.
            });
            $('#testDrive').magnificPopup({
                removalDelay: 500, //delay removal by X to allow out-animation,
                mainClass: 'mfp-with-fade',
                type: 'inline',
                midClick: true // allow opening popup on middle mouse click. Always set it to true if you don't provide alternative source.
            });
        }, 400);
    }


    activate();

    function sso() {

    }

    function submit(){
        if($scope.loginform.$valid) {
            if (vm.username === vm.password) {
                $window.sessionStorage.token = 'let me in!';
                $state.go('app.dashboard');
            }
        }
    }

};

authModule.controller('Login', Login);