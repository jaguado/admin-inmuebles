import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from '../../../auth.service';

@Component({
  selector: 'app-topnav',
  templateUrl: './topnav.component.html',
  styleUrls: ['./topnav.component.scss']
})
export class TopnavComponent implements OnInit {
  public pushRightClass: string;
  public condoInformationLabel: string;
  constructor(public router: Router, private translate: TranslateService, private authService: AuthService) {
    this.router.events.subscribe(val => {
      if (val instanceof NavigationEnd && window.innerWidth <= 992 && this.isToggled()) {
        this.toggleSidebar();
      }
    });
  }

  ngOnInit() {
    this.pushRightClass = 'push-right';
    this.condoInformation();
  }

  isToggled(): boolean {
    const dom: Element = document.querySelector('body');
    return dom.classList.contains(this.pushRightClass);
  }

  toggleSidebar() {
    const dom: any = document.querySelector('body');
    dom.classList.toggle(this.pushRightClass);
  }

  changeLang(language: string) {
    this.translate.use(language);
  }

  signOut(): void {
    this.authService.signOut();
  }

  condoInformation() {
    if (this.authService.user.dummyData) {
      this.translate.get('DummyDataLabel').subscribe((res: string) => {
        this.condoInformationLabel = this.authService.selectedCondo.name + ' | ' + res;
      });
    } else {
      this.condoInformationLabel = this.authService.selectedCondo.name;
    }
  }
}
