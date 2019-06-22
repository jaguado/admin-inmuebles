import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { FacebookLoginProvider, GoogleLoginProvider } from 'angularx-social-login';
import { TranslateService } from '@ngx-translate/core';
import { environment } from '../../environments/environment';


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
  condoSelection: Boolean = false;
  NonProduction: Boolean = !environment.production;
  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private translate: TranslateService
  ) { }

  suscribe() {
    this.authService.authService.authState.subscribe(user => {
      console.log('authState', 'subscribe', user);
      this.authService.user = user;
      if (user) {
        this.router.navigate(['/dashboard']);
      }
    });
  }
  signInWithGoogle(): void {
    this.suscribe();
    this.authService.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }

  signInWithFB(): void {
    this.suscribe();
    this.authService.authService.signIn(FacebookLoginProvider.PROVIDER_ID);
  }

  signOut(): void {}

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
    if (this.loginForm.invalid) {
      this.translate.get('CompleteAllHighlightFields').subscribe((res: string) => {
        this.errorMessage = res;
      });
      return;
    } else {
      let creds = this.loginForm.value;
      // TODO remove this for other environments
      // Using mock data
      creds = {
        email: 'eve.holt@reqres.in',
        password: 'cityslicka'
      };
      this.authService
        .signIn(creds)
        .then(res => {
          console.log('userService.checkUser', res);
          this.router.navigate(['/dashboard']);
        })
        .catch(err => {
          console.log('checkCredentials error', err.error);
          this.errorMessage = 'Error: ' + err.error.error;
        });
    }
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
}
