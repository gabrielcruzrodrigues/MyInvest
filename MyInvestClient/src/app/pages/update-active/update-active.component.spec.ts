import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateActiveComponent } from './update-active.component';

describe('UpdateActiveComponent', () => {
  let component: UpdateActiveComponent;
  let fixture: ComponentFixture<UpdateActiveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [UpdateActiveComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(UpdateActiveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
