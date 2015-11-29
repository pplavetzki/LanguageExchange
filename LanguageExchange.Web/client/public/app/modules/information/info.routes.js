/**
 * Created by paul on 5/7/15.
 */
'use strict';

var informationModule = require('./index');

function getStates() {
    
    return [{
            state: 'app.information',
            config: {
                url: '/',
                title: 'Information',
                views: {
                    "content@app": {
                        templateUrl: 'views/modules/information/info.html',
                        controller: 'Info',
                        controllerAs: 'vm'
                    }
                }
            }
        }];
}

onRun.$inject = ['routeHelper'];
function onRun(routeHelper) {
    routeHelper.configureStates(getStates());
};

informationModule
    .run(onRun);