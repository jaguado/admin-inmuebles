import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { AuthService } from '../../auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private authService: AuthService) {}

    canActivate() {
        console.log('auth.guard.ts', 'canActivate');
        if (this.authService.user && !this.authService.showCondoSelection()) {
            return true;
        }
        this.authService.signOut();
        return false;
    }
}
