import { TestBed, inject } from '@angular/core/testing';

import { AppInitService } from './app-init.service';
import { AuthService } from './services/auth.service';
import { AuthServiceMock } from './test/AuthServiceMock';

describe('AppInitService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AppInitService,
        { provide: AuthService, useValue: new AuthServiceMock(null) }
      ]
    });
  });

  it('should be created', inject([AppInitService], (service: AppInitService) => {
    expect(service).toBeTruthy();
  }));

  it('should be created', async () => {
    
    const spy = spyOn(TestBed.get(AuthService), 'initAuth').and.returnValue(Promise.resolve());
    const service: AppInitService = TestBed.get(AppInitService);
    await service.initApp();

    expect(spy).toHaveBeenCalled();
  });
});
