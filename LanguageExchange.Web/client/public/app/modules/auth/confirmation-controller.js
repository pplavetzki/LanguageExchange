'use strict';

var authModule = require('./index');
var Confirmation = Confirmation;

Confirmation.$inject = ['$state', '$stateParams', 'authServices'];

function Confirmation($state, $stateParams, authServices) {
    var vm = this;
    vm.message = 'There was a problem confirming your email address. Please try again or click here to receive another confirmation email.';

    authServices.confirmEmail($stateParams.userId, $stateParams.code).then(function (data) {
        if (data && data.success) {
            vm.message = 'You Sir or Madame are confirmed! Please click here to login.';
        }
    });
};

authModule.controller('Confirmation', Confirmation);