import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    constructor(private translate: TranslateService) {
        translate.setDefaultLang('en');
    }

    ngOnInit() {
        console.log('appcomponent.ts', 'ngOnInit');
    }
}
