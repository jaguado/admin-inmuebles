import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCondosComponent } from './admin-condos.component';

describe('AdminCondosComponent', () => {
  let component: AdminCondosComponent;
  let fixture: ComponentFixture<AdminCondosComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminCondosComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCondosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
