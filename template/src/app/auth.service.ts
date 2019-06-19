import { Injectable } from '@angular/core';
import { HttpClient  } from '@angular/common/http';  // Import it up here


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl: String = 'https://reqres.in/api/';
  constructor(private http: HttpClient) { }

  checkCredentials(credentials: object) {
    return this.http.post(this.baseUrl + 'login', credentials).toPromise();
  }
}
