import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { FacebookLoginProvider, GoogleLoginProvider } from 'angularx-social-login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  email = new FormControl('');
  loginForm: FormGroup;
  errorMessage: String;
  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private authService: AuthService
  ) {}

  suscribe(){
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
      password: ['', Validators.required]
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
}
