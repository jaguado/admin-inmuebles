import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient  } from '@angular/common/http';  // Import it up here
import { AuthService as SocialAuthService, SocialUser } from 'angularx-social-login';
import { DefaultCondos } from './shared/mockdata'
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  public user: SocialUser = null;
  public condos: any = DefaultCondos;
  public selectedCondo: any = null;

  public showCondoSelection(): Boolean {
    return this.user && this.condos && !this.selectedCondo;
  };

  baseUrl: String = 'https://reqres.in/api/';
  constructor(private http: HttpClient, public authService: SocialAuthService, private router: Router) { }

  signIn(credentials: any) {
    return this.http.post(this.baseUrl + 'login', credentials)
    .toPromise<any>()
    .then(token => {
      this.user = new SocialUser();
      this.user.email = credentials.email;
      this.user.firstName = credentials.firstName;
      this.user.lastName = credentials.lastName;
      this.user.photoUrl = credentials.photoUrl;
      this.user.id = credentials.id;
      this.user.authToken = token.token;
      this.user.provider = 'internal';
      return this.user;
    });
  }

  signOut() {
    if (this.user && this.user.provider !== 'internal') {
      this.authService.signOut();
    }
    this.user = null;
    this.selectedCondo = null;
    this.router.navigate(['/login']);
  }  
}

export type SocialUser = SocialUser;
