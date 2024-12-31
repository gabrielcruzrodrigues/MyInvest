import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateSmtpProfilesComponent } from './create-smtp-profiles.component';

describe('CreateSmtpProfilesComponent', () => {
  let component: CreateSmtpProfilesComponent;
  let fixture: ComponentFixture<CreateSmtpProfilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateSmtpProfilesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateSmtpProfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
