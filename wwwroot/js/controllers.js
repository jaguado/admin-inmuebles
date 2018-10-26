var authInterval = 30000;
var authTimer = null;
var loginPath = "/material/login.html";
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

    this.testApi = function(){
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

function authCtrl($scope, $rootScope, $http, $interval, $location, $window){
    var checkAuth = function(){
        console.log('checking auth....');
        if(user===null && $window.location.pathname !== loginPath) {
            console.log($window);
            //TODO check session state
            $window.location.href = loginPath;
        }
    };

    $scope.login = function(){
        user = {
            name: "Jorge",
            admin: true,
            access_token: "ya29.Gl1CBsmK6mb2C6qiUyfF5mV2y5_gJAkWSxrSgNLxY4YJ0tWUH1uINqPKvJnP1Rg_oqQcSP-7gADzltBdjhLhX04xm0IAYSpq5cy6YTzs8d86okAPy7wn3KKrkmfFIgc",
            provider: "google"
        };
        localStorage.setItem('user', JSON.stringify(user));
        console.log('login', user);
        $window.location.href = "../Index.html";
    };

    this.user =  user;

    checkAuth();    
    authTimer = $interval(checkAuth, authInterval);
}