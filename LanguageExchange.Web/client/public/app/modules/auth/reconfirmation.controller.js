'use strict';

var authModule = require('./index');
var Reconfirmation = Reconfirmation;

Reconfirmation.$inject = ['$stateParams', 'authServices'];

function Reconfirmation($stateParams, authServices) {
    var vm = this;
    var userName = $stateParams.userName;
    vm.showResend = true;
    vm.message = 'Your email address is not confirmed. Please confirm your email address or click the link below to regenerate a confirmation email.';  
    
    vm.sendReconfirm = sendReconfirm;
    
    function sendReconfirm() {
        authServices.reconfirmEmail(userName).then(function (data) {
            vm.message += 'Thank you. Check your spam if you do not see the email within an hour.';
            vm.showResend = false;
        });
    }
};

authModule.controller('Reconfirmation', Reconfirmation);