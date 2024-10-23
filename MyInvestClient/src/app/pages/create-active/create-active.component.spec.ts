import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateActiveComponent } from './create-active.component';

describe('CreateActiveComponent', () => {
  let component: CreateActiveComponent;
  let fixture: ComponentFixture<CreateActiveComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateActiveComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateActiveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
