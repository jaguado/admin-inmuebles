import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { FacebookLoginProvider, GoogleLoginProvider } from 'angularx-social-login';
import { TranslateService } from '@ngx-translate/core';
import { environment } from '../../environments/environment';
import { User } from '../shared/models';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  email = new FormControl('');
  loginForm: FormGroup;
  errorMessage: String;
  successMessage: String;
  NonProduction: Boolean = !environment.production;
  lockButton: Boolean = false;
  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private translate: TranslateService
  ) {
    translate.setDefaultLang(environment.defaultLanguage);
  }

  selectCondo(condo: any) {
    console.log('selectCondo', condo);
    this.authService.selectedCondo = condo;
    this.userRedir();
  }

  showUsingDummyData(): Boolean {
    return this.authService.user && this.authService.user.dummyData;
  }

  showCondoSelection(): Boolean {
    // console.log('login','showCondoSelection', this.authService.user, this.authService.showCondoSelection());
    return this.authService.showCondoSelection();
  }

  availableCondos(): any {
    return this.authService.condos;
  }

  userRedir() {
    if (this.authService.user && !this.showCondoSelection()) {
      console.log('userRedir', this.authService);
      this.router.navigate(['/']);
    }
  }
  suscribe() {
    this.authService.authService.authState.subscribe(user => {
      console.log('authState', 'subscribe', user);
      if (user) {
        this.authService.user = new User();
        this.authService.user = Object.assign(this.authService.user, user);
        this.authService.user.state = 2; // by default initial state user
        // social login creating customer on db and changing jwt for internal
        this.login({'email': '', 'password': ''});
      }
    });
  }
  signInWithGoogle(): void {
    if (!this.authService.user) {
      this.suscribe();
      this.authService.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
    }
  }

  signInWithFB(): void {
    if (!this.authService.user) {
      this.suscribe();
      this.authService.authService.signIn(FacebookLoginProvider.PROVIDER_ID);
    }
  }

  ngOnInit() {
    console.log('login ngOnInit, logged user:', this.authService.user);
    // window.localStorage.removeItem('token');
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: []
    });
  }

  onLogin() {
    this.clearFields();
    const creds = this.loginForm.value;
    if (this.loginForm.invalid || !creds.password) {
      this.translate.get('CompleteAllHighlightFields').subscribe((res: string) => {
        this.errorMessage = res;
      });
      return;
    } else {
      this.login(creds);
    }
  }

  login(creds: any) {
    this.lockButton = true;
    this.authService
    .signIn(creds)
    .then(res => {
      console.log('userService.checkUser', res);
      this.userRedir();
    })
    .catch(err => {
      console.log('checkCredentials error', err);
      this.errorMessage = err.statusText;
    })
    .finally(() => {
      this.lockButton = false;
    });
  }

  onPasswordRecovery() {
    this.clearFields();
    if (this.loginForm.valid) {
      this.errorMessage = '';
      // TODO check if customer exists and start on boarding process
      console.log('onNewCustomer', 'not implemented yet');
      this.translate.get('ThanksWillContactYou').subscribe((res: string) => {
        this.successMessage = res;
      });
    } else {
      this.translate.get('EnterYourEmailToContactYou').subscribe((res: string) => {
        this.errorMessage = res;
      });
    }
  }

  clearFields() {
    this.successMessage = null;
    this.errorMessage = null;
  }

  changeLang(language: string) {
    this.translate.use(language);
  }
}
