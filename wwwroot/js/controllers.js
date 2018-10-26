var authInterval = 30000;
var authTimer = null;

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

function authCtrl($scope, $rootScope, $http, $interval, $location){
    var checkAuth = function(){
        console.log('checking auth....');
        //TODO validar si el usuario esta logueado y si la sesion aún es valida
    };
    checkAuth();    
    authTimer = $interval(checkAuth, authInterval);
}