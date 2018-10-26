/* Intercept all http calls to add authorization headers with user token*/
var httpInterceptor = function ($q, $location, $rootScope) {
    return {
        request: function (config) { 
            console.log('config', config);
            //add access token if url is from the base api domain
            if (user != null && !config.url.includes("access_token")) {
                console.log('httpInterceptor', 'request', 'adding access token for user', user);
                config.url += config.url.includes("?") ? "&" : "?";
                config.url += "access_token=" + user.access_token;
                config.url += "&provider=" + user.provider;
            }
            return config;
        },

        response: function (result) { //res
            console.log('httpInterceptor', 'response', result.status);
            return result;
        },

        responseError: function (rejection) { //error
            console.log('Failed with', rejection.status);
            if (rejection.status == 401 || rejection.status == 403) {
                //force logoff
                $rootScope.$broadcast("logoff");             
            }
            return $q.reject(rejection);
        }
    }
};

angular.module("app", [])
       .controller("mainCtrl", mainCtrl)
       .controller("authCtrl", authCtrl)
       .config(function ($httpProvider) {
            $httpProvider.interceptors.push(httpInterceptor);
        });