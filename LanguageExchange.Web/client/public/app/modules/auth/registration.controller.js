'use strict';

var authModule = require('./index');
var Registration = Registration;

Registration.$inject = [];

function Registration() {
    var vm = this;

};

authModule.controller('Registration', Registration);