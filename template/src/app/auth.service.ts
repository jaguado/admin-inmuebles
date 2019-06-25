import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient  } from '@angular/common/http';  // Import it up here
import { AuthService as SocialAuthService, SocialUser } from 'angularx-social-login';
import { User } from './shared/user';
import { DefaultCondos } from './shared/mockdata'
import { environment } from 'src/environments/environment';
import { invoke } from 'q';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  public user: User = null;
  public condos: any = DefaultCondos;
  public selectedCondo: any = null;

  public showCondoSelection(): Boolean {
    return this.user && this.condos && !this.selectedCondo;
  };

  public isUserActive(): boolean {
    return this.user.state == 1;
  }

  public getMenu(): any {
    if(!this.selectedCondo){
      return [];
    }
    return this.selectedCondo.menu.filter(m => m.enabled);
  }

  baseUrl: String = '/';
  constructor(private http: HttpClient, public authService: SocialAuthService, private router: Router) { }

  signIn(credentials: any) {
    return this.http.post(this.baseUrl + 'login', credentials)
    .toPromise<any>()
    .then(result => {
      this.user = new User();
      this.user.email = result.email;
      this.user.firstName = result.firstName;
      this.user.lastName = result.lastName;
      this.user.photoUrl = result.photoUrl;
      this.user.id = result.id;
      this.user.authToken = result.authToken;
      this.user.provider = result.provider;
      this.user.state = result.state;
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

export type SocialUser = User;