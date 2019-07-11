import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from '../../auth.service';
import { environment } from '../../../environments/environment';
import { User, Credentials } from '../models';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private authService: AuthService) {}

    canActivate() {
        console.log('auth.guard.ts', 'canActivate');
        if (this.authService.user && !this.authService.showCondoSelection()) {
            // TODO check if user have permissions to access current page
            return true;
        }
        this.authService.signOut();
        return false;
    }
}
