import { Injectable } from '@angular/core';
import { HttpClient  } from '@angular/common/http';  // Import it up here
import { AuthService as SocialAuthService, SocialUser } from 'angularx-social-login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public user: SocialUser = null;
  baseUrl: String = 'https://reqres.in/api/';
  constructor(private http: HttpClient, public socialAuthService: SocialAuthService) { }

  checkCredentials(credentials: object) {
    return this.http.post(this.baseUrl + 'login', credentials).toPromise();
  }
}
