import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { AuthService as SocialAuthService, SocialUser } from 'angularx-social-login';
import { FacebookLoginProvider, GoogleLoginProvider } from 'angularx-social-login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  private user: SocialUser = null;
  email = new FormControl('');
  loginForm: FormGroup;
  errorMessage: String;
  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private socialAuthService: SocialAuthService
  ) {}

  signInWithGoogle(): void {
    this.socialAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }

  signInWithFB(): void {
    this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID);
  }

  signOut(): void {}

  ngOnInit() {
    console.log('login ngOnInit, logged user:', this.user);
    // window.localStorage.removeItem('token');
    this.loginForm = this.formBuilder.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
    this.socialAuthService.authState.subscribe(user => {
      this.user = user;
      if (user) {
        localStorage.setItem('authorization', JSON.stringify(user));
        this.router.navigate(['/dashboard']);
      } else {
        localStorage.removeItem('authorization');
      }
    });
  }

  onLogin() {
    if (this.loginForm.invalid) {
      this.errorMessage = 'Favor completar todos los campos';
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
        .checkCredentials(creds)
        .then(res => {
          console.log('userService.checkUser', res);
          localStorage.setItem('authorization', JSON.stringify(res));
          this.router.navigate(['/dashboard']);
        })
        .catch(err => {
          console.log('checkCredentials error', err.error);
          this.errorMessage = 'Error: ' + err.error.error;
          localStorage.removeItem('authorization');
        });
    }
  }
}
