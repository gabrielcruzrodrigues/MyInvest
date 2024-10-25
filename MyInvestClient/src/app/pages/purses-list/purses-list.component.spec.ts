import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PursesListComponent } from './purses-list.component';

describe('PursesListComponent', () => {
  let component: PursesListComponent;
  let fixture: ComponentFixture<PursesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PursesListComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(PursesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
