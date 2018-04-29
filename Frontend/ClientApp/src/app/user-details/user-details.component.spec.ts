import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserDetailsComponent } from './user-details.component';
import { FormsModule } from '@angular/forms';
import { NgDatepickerModule } from 'ng2-datepicker';
import { AuthService } from '../services/auth.service';
import { AuthServiceMock } from '../test/AuthServiceMock';
import { MusicstoreService } from '../services/musicstore.service';
import { MockMusicStoreService } from '../test/MockMusicStoreService';
import { UserManager } from 'oidc-client';
import { MockUserManager } from '../test/MockUserManager';

describe('UserDetailsComponent', () => {
  let component: UserDetailsComponent;
  let fixture: ComponentFixture<UserDetailsComponent>;

  beforeEach(async(() => {
    TestBed.overrideProvider(AuthService, { useValue: new AuthServiceMock(<UserManager>new MockUserManager()) });
    TestBed.overrideProvider(MusicstoreService, { useValue: new MockMusicStoreService() });
    TestBed.configureTestingModule({
      imports:[
        FormsModule,
        NgDatepickerModule,
        ],
      declarations: [ UserDetailsComponent ],
      providers:[AuthService,MusicstoreService]
    })
    
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
