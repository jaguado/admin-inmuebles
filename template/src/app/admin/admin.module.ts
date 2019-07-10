import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { MatButtonModule, MatCardModule, MatIconModule, MatTableModule } from '@angular/material';

@NgModule({
    imports: [CommonModule, AdminRoutingModule, MatButtonModule, MatCardModule, MatIconModule, MatTableModule],
    declarations: [AdminComponent]
})
export class AdminModule {}
