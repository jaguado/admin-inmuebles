import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';  // Import it up here
import { AuthService as SocialAuthService, SocialUser } from 'angularx-social-login';
import { User, Condo, Menu, Credentials } from './shared/models';
import { DefaultCondos, DefaultMenu, DefaultProperties } from './shared/mockdata';
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
  private adminRoles: string[] = ['Admin', 'God'];
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
    return this.getFilteredMenu(this.selectedCondo.menu, this.selectedCondo);
  }

  getFilteredMenu(menu: Menu[], condo: any): Menu[] {
    if (!menu) { return []; }
    return menu.filter(m => m.enabled)
               .filter(m => !m.requireAdminRole || this.isAdmin(condo));
  }

  isAdmin(condo: any): boolean {
    if (!condo) { return false; }
    return condo.Roles ? condo.Roles && condo.Roles.some((s: string) => this.adminRoles.includes(s))
                       : condo.roles && condo.roles.some((s: string) => this.adminRoles.includes(s));
  }

  async signIn(credentials: Credentials) {
    const result = await this.http.post(this.baseUrl + 'login', credentials)
      .toPromise<any>();
    console.log('signIn', 'result', result);
    this.cleanSession();
    this.user = new User();
    this.user = Object.assign(this.user, result);
    this.loadCondos(result.data);
    return this.user;
  }

  loadCondos(data: any) {
    // load condos information from response or from dummy data
    if (!data || data.length === 0) {
      // user dummy data for new users
      this.user.dummyData = true;
      this.condos = DefaultCondos;
    } else {
      this.condos = [];
      data.forEach((condo: any) => {
        if (condo) {
          this.condos.push(
            {
              'id': condo.Rut,
              'name': condo.RazonSocial,
              'menu': this.getFilteredMenu(DefaultMenu, condo),
              'enabled': condo.Vigencia === 1,
              'properties': DefaultProperties, // TODO replace with real data
              'roles': condo.Roles
            });
          }
      });
    }
    // if only exists one skip selection screen
    if (this.condos.filter(c => c.enabled).length < 2) {
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

  async saveCustomer(userInfo: User) {
    const payload = {
      'Rut': userInfo.rut,
      'Mail': userInfo.email,
      'Nombre': userInfo.name,
      'Icono': userInfo.photoUrl
    };
    const result = await this.http.post(this.baseUrl + 'v1/Customer', payload)
      .toPromise<any>();
    // update user state // FIXME update with db data
    this.user.state = 1;
    return result;
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
