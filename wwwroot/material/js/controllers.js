function mainCtrl() {
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
}