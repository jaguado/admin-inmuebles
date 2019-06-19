import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router } from '@angular/router';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router) {}

    canActivate() {
        console.log('auth.guard.ts', 'canActivate');
        if (localStorage.getItem('authorization')) {
            return true;
        }
        // this.socialAuthService.signOut();
        this.router.navigate(['/login']);
        return false;
    }
}
