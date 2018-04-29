import { TestBed, inject } from '@angular/core/testing';

import { MusicstoreService } from './musicstore.service';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from './auth.service';
import { AuthServiceMock } from '../test/AuthServiceMock';

describe('MusicstoreService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientModule],
      providers: [MusicstoreService, 
      { provide: AuthService, fromClass: AuthServiceMock }]
    });
  });

  it('should be created', inject([MusicstoreService], (service: MusicstoreService) => {
    expect(service).toBeTruthy();
  }));
});
