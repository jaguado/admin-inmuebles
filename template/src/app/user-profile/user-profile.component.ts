import { AuthService } from './../auth.service';
import { environment } from './../../environments/environment';
import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from '../shared/models';

@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  userForm: FormGroup;
  lockButton: Boolean;
  errorMessage: String;
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

  onSave() {
    this.errorMessage = null;
    if (this.userForm.valid) {
      this.lockButton = true;
      const payload = new User();
      payload.rut = this.userForm.value.rut;
      payload.name = this.userForm.value.name;
      payload.email = this.userForm.value.email;
      payload.photoUrl = this.userForm.value.icon;
      this.authService.saveCustomer(payload)
      .then(r => {
        console.log('valid form', 'saved', r);
      })
      .catch(c => {
        console.log('valid form', 'error', c);
      }).finally(() => {
        this.lockButton = false;
      });
    } else {
      this.errorMessage = 'CompleteAllFields';
    }
  }
}
