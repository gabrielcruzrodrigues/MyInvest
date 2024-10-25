import { TestBed } from '@angular/core/testing';

import { PursesService } from './purses.service';

describe('PursesService', () => {
  let service: PursesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PursesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
