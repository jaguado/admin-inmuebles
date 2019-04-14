var authInterval = 30000;
var authTimer = null;
var loginPath = "/login/index.html";
var homePath = "/material/index.html";
var user = localStorage.getItem('user') != null ? JSON.parse(localStorage.getItem('user')) : null;

function mainCtrl($http) {
    this.text = "Angular funcionando!!";

    /*
        Este es un ejemplo de un controller, esto es MVC, así que aqui debes hacer
        las llamadas a apis y manipular models, al hacerlo, angularjs automaticamente deberia
        refrescar los controles en el DOM. 

        Un ejemplo:

        Voy a crear una lista y la voy a usar para poblar un combobox.    

        Acá se puede ver como iterar por sobre la lista y asignar el valor seleccionado a un "model" de nombre "ctrl.mes".
        <select class="form-control" ng-model="ctrl.mes">
            <option ng-repeat="mes in ctrl.listaConMeses">{{mes}}</option>
        </select>

        Acá se muestra el model "ctrl.mes", como puedes ver al actualizar el selector, se actualiza
        el model y automaticamente se actualiza el label

        <label>{{ctrl.mes}}</label>
    */

    this.listaConMeses = ["Enero", "Febrero", "Marzo", "Abril"];

    this.testApi = function () {
        console.log('testing API');
        return $http.get("../js/app.js").then(function (response) {
            console.log('status code', response.status);
            return response.status == 200;
        }, function (response) {
            console.log('err', response);
            return false;
        });
    }
}

/// Este controlador se va a encargar de todo lo que tiene que ver con autenticación
/// Además de la información personal del usuario se encargará de administrar menus, permisos y roles
function authCtrl($scope, $rootScope, $http, $interval, $location, $window, Analytics) {
    $scope.showMessages = false;
    $scope.showNotifications = false;
    $scope.showLanguage = false;
    $scope.showSearch=false;

    $scope.currentPage = 'index2.html';

    var checkAuth = function () {
        console.log('checking auth....');
        if (user === null && $window.location.pathname !== loginPath) {
            console.log($window);
            //TODO check session state
            $window.location.href = loginPath;
        } else
            console.log('login', user);
    };

    $scope.login = function () {
        //TODO check credentials
        console.log('login', user);
        $window.location.href = "../index.html";
    };

    $scope.logoff = function () {
        user = null;
        localStorage.setItem('user', null);
        $window.location.href = loginPath;
    }
    $rootScope.$on("logoff", function () {
        $scope.logoff();
    });
    $rootScope.$on('event:social-sign-in-success', function (event, userDetails) {
        /*  Login ok */
        user = userDetails;
        this.user = user;
        localStorage.setItem('user', JSON.stringify(user));
        Analytics.set('&uid', user.uid);
        Analytics.trackEvent('auth', user.provider);
        console.log('social-sign-in-success');
        if (user.provider == "google") {
            $scope.$apply(function () {
                $scope.user = user;
            });
        }
        $window.location.href = homePath;
    });
    $rootScope.$on('event:social-sign-out-success', function (event, logoutStatus) {
        //logout ok
        console.log('social-sign-out-success');
        $scope.logoff();
    });


    this.user = user;
    checkAuth();
    authTimer = $interval(checkAuth, authInterval);


    console.log('loading menu');
    $scope.menu = [{
            "Section": "Personal",
            "Menus": [{
                    "Name": "Dashboard",
                    "IconClass": "mdi mdi-gauge",
                    "Options": [{
                            "Title": "Dashboard 1",
                            "Href": "index2.html",
                            "BreadcrumbLabel" : "Personal / Dashboard",
                            "Options": null
                        },
                        {
                            "Title": "Test Page",
                            "Href": "/Views/testPage.html",
                            "Options": null
                        }
                    ]
                },
                {
                    "Name": "Templates",
                    "IconClass": "mdi mdi-laptop-windows",
                    "Options": [{
                            "Title": "Template 1",
                            "Href": "index2.html",
                            "Options": null
                        },
                        {
                            "Title": "Template 2",
                            "Href": "index3.html",
                            "Options": null
                        }
                    ]
                }
            ]
        },
        {
            "Section": "Options",
            "Menus": [{
                "Name": "Configuration",
                "IconClass": "ti-settings",
                "Options": [{
                    "Title": "Account",
                    "Href": "index3.html",
                    "Options": null
                }]
            }]
        }
    ];
    $scope.openMenu = function(option){
        console.log('openMenu', option);
        $scope.currentPage = option.Href;
        //TODO refresh breadcrumb
    };
}