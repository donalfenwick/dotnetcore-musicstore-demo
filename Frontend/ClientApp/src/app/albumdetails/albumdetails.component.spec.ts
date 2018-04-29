import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from "@angular/router/testing";
import { Router } from "@angular/router";
import { AlbumdetailsComponent } from './albumdetails.component';
import { FormatdurationPipe } from '../pipes/formatduration.pipe';
import { MusicstoreService } from '../services/musicstore.service';
import { TimesPipe } from '../pipes/times.pipe';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { UserManager, User } from 'oidc-client';
import { GenreList } from '../models/genremodels';
import { MockMusicStoreService } from '../test/MockMusicStoreService';

describe('AlbumdetailsComponent', () => {
  let component: AlbumdetailsComponent;
  let fixture: ComponentFixture<AlbumdetailsComponent>;
  
  beforeEach(async(() => {
    TestBed.overrideProvider(MusicstoreService, { useValue: new MockMusicStoreService() });
    TestBed.configureTestingModule({
      imports: [ RouterTestingModule.withRoutes([]) ],
      providers: [ MusicstoreService ],
      declarations: [ AlbumdetailsComponent, FormatdurationPipe, TimesPipe ]
    })
    .compileComponents();
    
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AlbumdetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
