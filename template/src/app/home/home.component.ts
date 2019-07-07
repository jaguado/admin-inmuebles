import { Component, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { TranslateService, LangChangeEvent } from '@ngx-translate/core';
import { AuthService, SocialUser } from '../auth.service';
import { environment } from './../../environments/environment';
import { Property } from '../shared/models';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnChanges {
    constructor(private authService: AuthService, private translate: TranslateService) {
      translate.setDefaultLang(environment.defaultLanguage);
    }
    public welcomeMessage: String = '';
    public warningMessage: String = '';
    public showWarningMessage: boolean;
    public user: SocialUser = this.authService.user;

    // bar chart
    public barChartOptions: any = {
      scaleShowVerticalLines: false,
      responsive: true
  };

  public barChartType: string;
  public barChartLegend: boolean;


    ngOnInit() {
        this.LoadWelcomeMessages();
        this.CheckUserStateAndShowWarning();
        this.translate.onLangChange.subscribe((params: LangChangeEvent) => {
            this.LoadWelcomeMessages();
        });

        this.barChartType = 'bar';
        this.barChartLegend = true;
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

    CheckUserStateAndShowWarning() {
      this.showWarningMessage = !this.authService.isUserActive();
    }

    availableEstate() {
      return !this.authService.selectedCondo.properties ? [] :  this.authService.selectedCondo.properties;
    }

    pay(property: Property) {
      console.log('TODO implement payment', property);
    }

    public chartClicked(e: any): void {
      console.log(e);
    }

    public chartHovered(e: any): void {
        console.log(e);
    }
}
