'use strict';

var angular = require('angular');

module.exports = angular.module('app.information', ['app.core']);

require('./info.routes');
require('./info.controller');