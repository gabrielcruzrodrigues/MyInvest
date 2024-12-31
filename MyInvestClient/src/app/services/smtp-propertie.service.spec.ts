import { TestBed } from '@angular/core/testing';

import { SmtpPropertieService } from './smtp-propertie.service';

describe('SmtpPropertieService', () => {
  let service: SmtpPropertieService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SmtpPropertieService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
