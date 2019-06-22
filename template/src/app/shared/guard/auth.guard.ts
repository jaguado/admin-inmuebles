import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '../../auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router, private authService: AuthService) {}

    canActivate() {
        console.log('auth.guard.ts', 'canActivate');
        if (this.authService.user !== null) {
            return true;
        }
        console.log('authService.user null', this.authService);
        this.router.navigate(['/login']);
        return false;
    }
}
