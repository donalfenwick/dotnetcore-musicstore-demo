import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchResultsComponent } from './search-results.component';
import { RouterTestingModule } from '@angular/router/testing';
import { TimesPipe } from '../pipes/times.pipe';
import { MusicstoreService } from '../services/musicstore.service';
import { MockMusicStoreService } from '../test/MockMusicStoreService';
import { HttpClientModule } from '@angular/common/http';
import { AuthServiceMock } from '../test/AuthServiceMock';
import { AuthService } from '../services/auth.service';
import { UserManager } from 'oidc-client';
import { MockUserManager } from '../test/MockUserManager';

describe('SearchResultsComponent', () => {
  let component: SearchResultsComponent;
  let fixture: ComponentFixture<SearchResultsComponent>;

  TestBed.overrideProvider(MusicstoreService, { useValue: new MockMusicStoreService() });
  TestBed.overrideProvider(AuthService, { useValue: new AuthServiceMock(<UserManager>new MockUserManager()) });
  TestBed.overrideProvider(UserManager, { useValue: new MockUserManager() });
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports:[RouterTestingModule.withRoutes([]), HttpClientModule ],
      declarations: [ SearchResultsComponent, TimesPipe ],
      providers:[MusicstoreService,AuthService,UserManager]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchResultsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
