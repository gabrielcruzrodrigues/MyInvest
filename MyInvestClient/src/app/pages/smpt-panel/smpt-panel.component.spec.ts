import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmptPanelComponent } from './smpt-panel.component';

describe('SmptPanelComponent', () => {
  let component: SmptPanelComponent;
  let fixture: ComponentFixture<SmptPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SmptPanelComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SmptPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
