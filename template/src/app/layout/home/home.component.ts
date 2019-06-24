import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
import { AuthService, SocialUser } from '../../auth.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnChanges {
    public welcomeMessage: string = '';
    public warningMessage: string = '';
    public showWarningMessage: boolean = false;
    public user: SocialUser = this.authService.user;
    constructor(private authService: AuthService, private translate: TranslateService) { }

    ngOnInit() {
        this.LoadWelcomeMessages();
        this.translate.onLangChange.subscribe((params: LangChangeEvent) => {
            this.LoadWelcomeMessages();
        });
    }

    ngOnChanges(changes: SimpleChanges) {
        console.log('Home', 'ngOnChanges', changes);
    }

    LoadWelcomeMessages(){
        this.translate.get('WelcomeMessage').toPromise<string>().then(result =>
            this.welcomeMessage = result.replace('{{ name }}', this.user.name)   
        );
        this.translate.get('warningMessage').toPromise<string>().then(result =>
            this.warningMessage = result 
        );
    }
}
