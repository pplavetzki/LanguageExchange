/**
 * Created by paul on 5/11/15.
 */
/**
 * Created by paul on 5/7/15.
 */
'use strict';

var authModule = require('./index');

function getStates() {
    
    return [{
            state: 'login',
            config: {
                url: '/login',
                views: {
                    root: {
                        templateUrl: 'views/modules/auth/login.html',
                        controller: 'Login',
                        controllerAs: 'vm',
                        title: 'Login'
                    }
                }
            }
            },{
        state: 'registration',
        config: {
                url: '/registration',
                views: {
                    root: {
                        templateUrl: 'views/modules/auth/registration.html',
                        controller: 'Registration',
                        controllerAs: 'vm',
                        title: "Registration"
                    }
                }
        }
    }];
}

onRun.$inject = ['routeHelper'];
function onRun(routeHelper){
    routeHelper.configureStates(getStates());
};

authModule
    .run(onRun);