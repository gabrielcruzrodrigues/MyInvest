import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListSmtpProfilesComponent } from './list-smtp-profiles.component';

describe('ListSmtpProfilesComponent', () => {
  let component: ListSmtpProfilesComponent;
  let fixture: ComponentFixture<ListSmtpProfilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListSmtpProfilesComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListSmtpProfilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
