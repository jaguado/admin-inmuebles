import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule, MatCardModule, MatIconModule, MatTableModule, MatFormFieldModule,  MatInputModule } from '@angular/material';
import { MatGridListModule } from '@angular/material/grid-list';

import { StatModule } from '../shared/modules/stat/stat.module';
import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { UserProfileComponent } from '../user-profile/user-profile.component';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ReactiveFormsModule } from '@angular/forms';
import { ChartsModule as Ng2Charts } from 'ng2-charts';
import { HttpClient } from '@angular/common/http';

export const createTranslateLoader = (http: HttpClient) => {
  /* for development
    return new TranslateHttpLoader(
        http,
        '/start-javascript/sb-admin-material/master/dist/assets/i18n/',
        '.json'
    );*/
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
};

@NgModule({
    imports: [
        CommonModule,
        Ng2Charts,
        HomeRoutingModule,
        MatGridListModule,
        StatModule,
        MatCardModule,
        MatCardModule,
        MatTableModule,
        MatButtonModule,
        MatIconModule,
        MatFormFieldModule,
        MatInputModule,
        FlexLayoutModule.withConfig({addFlexToParent: false}),
        TranslateModule.forRoot({
          loader: {
            provide: TranslateLoader,
            useFactory: createTranslateLoader,
            deps: [HttpClient]
          }
        }),
        ReactiveFormsModule
    ],
    declarations: [HomeComponent, UserProfileComponent]
})
export class HomeModule {}
