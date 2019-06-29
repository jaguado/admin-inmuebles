import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient  } from '@angular/common/http';  // Import it up here
import { AuthService as SocialAuthService, SocialUser } from 'angularx-social-login';
import { User } from './shared/user';
import { DefaultCondos, DefaultMenu } from './shared/mockdata';
import { environment } from '../environments/environment';


@Injectable({
  providedIn: 'root'
})

export class AuthService {
  constructor(private http: HttpClient, public authService: SocialAuthService, private router: Router) { }

  public baseUrl: String = environment.baseUrl;
  public user: User = null;
  public condos: any = [];
  public selectedCondo: any = null;

  public showCondoSelection(): Boolean {
    return this.user && this.condos && !this.selectedCondo;
  }

  public isUserActive(): boolean {
    return this.user.state === 1;
  }

  public getMenu(): any {
    if (!this.selectedCondo) {
      return [];
    }
    return this.selectedCondo.menu.filter(m => m.enabled);
  }



  signIn(credentials: any) {
    return this.http.post(this.baseUrl + 'login', credentials)
    .toPromise<any>()
    .then(result => {
      this.user = new User();
      this.user.email = result.email;
      this.user.name = result.name;
      this.user.firstName = result.firstName;
      this.user.lastName = result.lastName;
      this.user.photoUrl = result.photoUrl;
      this.user.id = result.id;
      this.user.authToken = result.authToken;
      this.user.provider = result.provider;
      this.user.state = result.state;
      this.loadCondos(result.data);
      return this.user;
    });
  }

  loadCondos(data) {
    // load condos information from response
    if (!data) {
      // user dummy data for new users
      this.condos = DefaultCondos;
    } else {
      this.condos = [];
      data.forEach(condo => {
        this.condos.push(
          {
            'id': condo.Rut,
            'name': condo.RazonSocial,
            'menu': DefaultMenu,
            'enabled': condo.Vigencia === 1
          });
      });
    }
    // if only exists one skip selection screen
    if (this.condos.length < 2) {
      this.selectedCondo = this.condos[0];
    }
    console.log('loadCondos', this.condos, this.selectedCondo);
  }

  signOut() {
    if (this.user && this.user.provider !== 'internal') {
      this.authService.signOut();
    }
    this.user = null;
    this.selectedCondo = null;
    this.router.navigate(['/login']);
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
      console.log('save', result);
      return;
    });
  }
}

export type SocialUser = User;
