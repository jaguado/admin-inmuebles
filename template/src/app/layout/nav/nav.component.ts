import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { BreakpointObserver, Breakpoints, BreakpointState } from '@angular/cdk/layout';
import { Component } from '@angular/core';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {
  isHandset: Observable<BreakpointState> = this.breakpointObserver.observe(Breakpoints.Handset);
  constructor(private breakpointObserver: BreakpointObserver, private router: Router, private authService: AuthService) {}

  signOut(): void {
    this.authService
      .authService
      .signOut(true)
      .catch(e => console.log('error on signOut', e))
      .finally(() => {
        this.router.navigate(['/login']);
      });
  }

  getMenu() {
    return this.authService.getMenu();
  }
}
