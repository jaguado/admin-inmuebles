import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';  // Import it up here
import { AuthService as SocialAuthService, SocialUser } from 'angularx-social-login';
import { User, Condo, Menu, Credentials } from './shared/models';
import { DefaultCondos, DefaultMenu } from './shared/mockdata';
import { environment } from '../environments/environment';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})

export class AuthService {
  constructor(private http: HttpClient, public authService: SocialAuthService, private router: Router) { }

  public baseUrl: String = environment.baseUrl;
  public user: User = null;
  public condos: Condo[] = null;
  public selectedCondo: Condo = null;

  checkService() {
    return this.http.get(this.baseUrl + 'health');
  }

  public showCondoSelection(): Boolean {
    return this.user && this.condos && !this.selectedCondo;
  }

  public isUserActive(): boolean {
    return this.user.state === 1;
  }

  public getMenu(): Menu[] {
    if (!this.selectedCondo) {
      return [];
    }
    return this.selectedCondo.menu.filter(m => m.enabled);
  }

  signIn(credentials: Credentials) {
    return this.http.post(this.baseUrl + 'login', credentials)
    .toPromise<any>()
    .then(result => {
      console.log('signIn', 'result', result);
      this.cleanSession();
      this.user = new User();
      this.user = Object.assign(this.user, result);
      this.loadCondos(result.data);
      return this.user;
    });
  }

  loadCondos(data) {
    // load condos information from response or from dummy data
    if (!data || data.length === 0) {
      // user dummy data for new users
      this.user.dummyData = true;
      this.condos = DefaultCondos;
    } else {
      this.condos = [];
      data.forEach(condo => {
        if (condo) {
          this.condos.push(
            {
              'id': condo.Rut,
              'name': condo.RazonSocial,
              'menu': DefaultMenu,
              'enabled': condo.Vigencia === 1,
              'properties': []
            });
          }
      });
    }
    // if only exists one skip selection screen
    if (this.condos.length < 2) {
      this.selectedCondo = this.condos[0];
    }
    // console.log('loadCondos', this.condos, this.selectedCondo);
  }

  signOut() {
    if (this.user && this.user.provider !== 'internal') {
      this.authService.signOut();
    }
    this.cleanSession();
    this.router.navigate(['/login']);
  }

  cleanSession() {
    this.user = null;
    this.condos = null;
    this.selectedCondo = null;
  }
  saveCustomer(userInfo: User) {
    const payload = {
      'Rut': userInfo.rut,
      'Mail': userInfo.email,
      'Nombre': userInfo.name,
      'Icono': userInfo.photoUrl
    };
    return this.http.post(this.baseUrl + 'v1/Customer', payload)
    .toPromise<any>()
    .then(result => {
      // update user state // FIXME update with db data
      this.user.state = 1;
      return result;
    });
  }
}


@Injectable({
  providedIn: 'root'
})
export class PublicServices {
  constructor(private http: HttpClient, public authService: AuthService, private router: Router) { }

  resetPassword(creds: Credentials): Observable<object> {
    return this.http.post(this.authService.baseUrl + 'v1/resetPassword', creds);
  }

  createCustomer(creds: Credentials): Observable<object> {
    return this.http.post(this.authService.baseUrl + 'v1/newCustomer', creds);
  }
}

export type SocialUser = User;
