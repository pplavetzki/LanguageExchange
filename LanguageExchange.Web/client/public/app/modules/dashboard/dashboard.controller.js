/**
 * Created by paul on 5/7/15.
 */
'use strict';

var dashboardModule = require('./index');
var Dashboard = Dashboard;

Dashboard.$inject = ['$state'];

function Dashboard ($state) {
    var vm = this;

    vm.gotoAnalysis = function() {
        $state.go('app.workflow');
    };
};

dashboardModule.controller('Dashboard', Dashboard);