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
  public barChartLabels: string[] = ['2006', '2007', '2008', '2009', '2010', '2011', '2012'];
  public barChartType: string;
  public barChartLegend: boolean;

  public barChartData: any[] = [
      { data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A' },
      { data: [28, 48, 40, 19, 86, 27, 90], label: 'Series B' }
  ];

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
}
