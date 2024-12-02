import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestRecoverPasswordComponent } from './request-recover-password.component';

describe('RequestRecoverPasswordComponent', () => {
  let component: RequestRecoverPasswordComponent;
  let fixture: ComponentFixture<RequestRecoverPasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RequestRecoverPasswordComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RequestRecoverPasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
