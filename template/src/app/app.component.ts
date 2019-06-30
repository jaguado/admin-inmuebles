import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { environment } from '../environments/environment';
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
    constructor(translate: TranslateService) {
        translate.setDefaultLang(environment.defaultLanguage);
    }

    ngOnInit() {
        console.log('appcomponent.ts', 'ngOnInit');
        // enforce https when host is different to localhost
        if (location.hostname !== 'localhost' && location.protocol === 'http:') {
          console.log('Https enforcement', location);
          window.location.href = location.href.replace('http', 'https');
        }
    }
}
