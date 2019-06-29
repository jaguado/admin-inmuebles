import { AuthService } from './../auth.service';
import { environment } from './../../environments/environment';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  userForm: FormGroup;
  constructor(translate: TranslateService, private authService: AuthService, private formBuilder: FormBuilder) {
    translate.setDefaultLang(environment.defaultLanguage);
}

  ngOnInit() {
    this.userForm = this.formBuilder.group({
      rut: [this.authService.user.rut, Validators.required],
      name: [this.authService.user.name, Validators.required],
      email: [this.authService.user.email, Validators.required],
      icon: [this.authService.user.photoUrl]
    });
  }
}
