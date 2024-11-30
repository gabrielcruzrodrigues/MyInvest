import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RequestLoginByCodeComponent } from './request-login-by-code.component';

describe('RequestLoginByCodeComponent', () => {
  let component: RequestLoginByCodeComponent;
  let fixture: ComponentFixture<RequestLoginByCodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [RequestLoginByCodeComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RequestLoginByCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
