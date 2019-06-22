import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient  } from '@angular/common/http';  // Import it up here
import { AuthService as SocialAuthService, SocialUser } from 'angularx-social-login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public user: SocialUser = null;
  baseUrl: String = 'https://reqres.in/api/';
  constructor(private http: HttpClient, public authService: SocialAuthService, private router: Router) { }

  signIn(credentials: any) {
    this.authService.authState.subscribe(user => {
      this.user = user;
      if (user) {
        this.router.navigate(['/dashboard']);
      }
    });

    return this.http.post(this.baseUrl + 'login', credentials)
    .toPromise<any>()
    .then(token => {
      this.user = new SocialUser();
      this.user.email = credentials.email;
      this.user.authToken = token.token;
      return this.user;
    });
  }

  signOut() {
    // TODO identify social or internal and logout
    if (!this.authService) {
      this.authService.signOut();
      this.user = null;
    }
  }
}
