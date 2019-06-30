import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
import { AuthService, SocialUser } from '../auth.service';
import { environment } from './../../environments/environment';
@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnChanges {
    public welcomeMessage: String = '';
    public warningMessage: String = '';
    public showWarningMessage: boolean = !this.authService.isUserActive();
    public user: SocialUser = this.authService.user;
    constructor(private authService: AuthService, private translate: TranslateService) {
      translate.setDefaultLang(environment.defaultLanguage);
    }

    ngOnInit() {
        this.LoadWelcomeMessages();
        this.translate.onLangChange.subscribe((params: LangChangeEvent) => {
            this.LoadWelcomeMessages();
        });
    }

    ngOnChanges(changes: SimpleChanges) {
        console.log('Home', 'ngOnChanges', changes);
    }

    LoadWelcomeMessages() {
        this.translate.get('WelcomeMessage').toPromise<string>().then(result => {
            this.welcomeMessage = result.replace('{{ name }}', this.user.name);
          }
        );
        this.translate.get('WarningMessage').toPromise<string>().then(result =>
            this.warningMessage = result
        );
    }

    availableEstate() {
      return this.authService.selectedCondo.properties;
    }
}
