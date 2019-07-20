import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import {
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatSidenavModule,
    MatToolbarModule,
    MatCardModule,
    MatTableModule,
    MatFormFieldModule
} from '@angular/material';
import { TranslateModule } from '@ngx-translate/core';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { TopnavComponent } from './components/topnav/topnav.component';
import { LayoutRoutingModule } from './layout-routing.module';
import { LayoutComponent } from './layout.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from '../home/home.component';
import { AdminComponent } from '../admin/admin.component';
import { AdminUsersComponent  } from '../admin-users/admin-users.component';
import { AdminCondosComponent } from '../admin-condos/admin-condos.component';
import { FlexLayoutModule } from '@angular/flex-layout';
import { UserProfileComponent } from '../user-profile/user-profile.component';
import { ChartsModule as Ng2Charts } from 'ng2-charts';
import { ReactiveFormsModule } from '@angular/forms';
import { SettingsComponent } from '../settings/settings.component';
import { InboxComponent } from '../inbox/inbox.component';

@NgModule({
    imports: [
        CommonModule,
        LayoutRoutingModule,
        MatToolbarModule,
        MatButtonModule,
        MatSidenavModule,
        MatIconModule,
        MatInputModule,
        MatMenuModule,
        MatListModule,
        MatCardModule,
        MatTableModule,
        TranslateModule,
        MatFormFieldModule,
        Ng2Charts,
        ReactiveFormsModule,
        FlexLayoutModule.withConfig({addFlexToParent: false})
    ],
    declarations: [
      LayoutComponent,
      NavComponent,
      TopnavComponent,
      SidebarComponent,
      HomeComponent,
      AdminComponent,
      AdminUsersComponent,
      AdminCondosComponent,
      UserProfileComponent,
      SettingsComponent,
      InboxComponent
    ]
})
export class LayoutModule {}
