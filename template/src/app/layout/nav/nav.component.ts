import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { AuthService as SocialAuthService } from 'angularx-social-login';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {
  isHandset: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset);
  constructor(private breakpointObserver: BreakpointObserver, private router: Router, private socialAuthService: SocialAuthService) {}

  signOut(): void {
    this.socialAuthService
      .signOut(true)
      .catch(e => console.log('error on signOut', e))
      .finally(() => {
        localStorage.removeItem('authorization');
        this.router.navigate(['/login']);
      });
  }
}
