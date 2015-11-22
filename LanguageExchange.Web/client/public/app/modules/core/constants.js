'use strict';

var coreModule = require('./index');
var Constants = constants;

function constants() {

    return {
        appBaseUrl: 'http://localhost:9095/',
        apiBaseUrl: 'http://localhost:49425/api/'
    };

}

coreModule.constant('constants', Constants());