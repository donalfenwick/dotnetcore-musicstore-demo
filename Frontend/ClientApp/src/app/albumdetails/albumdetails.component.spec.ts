import { async, ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { RouterTestingModule } from "@angular/router/testing";
import { Router, Params, ActivatedRoute } from "@angular/router";
import { AlbumdetailsComponent } from './albumdetails.component';
import { FormatdurationPipe } from '../pipes/formatduration.pipe';
import { MusicstoreService } from '../services/musicstore.service';
import { TimesPipe } from '../pipes/times.pipe';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { UserManager, User } from 'oidc-client';
import { GenreList } from '../models/genremodels';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/from'
import { AlbumDetail } from '../models/albummodels';
import { Location } from '@angular/common';
import { Subject } from 'rxjs';

describe('AlbumdetailsComponent', () => {
  let component: AlbumdetailsComponent;
  let fixture: ComponentFixture<AlbumdetailsComponent>;
  let routeParams: Subject<Params>;
  const album: AlbumDetail = <AlbumDetail>{ id:44, artistId: 22, artistName: '', title: '' };

  beforeEach(() => {

    routeParams = new Subject<Params>(); // use subject to trigger route changes
    TestBed.configureTestingModule({
      imports: [ RouterTestingModule.withRoutes([]) ],
      providers: [ 
        { provide: MusicstoreService, useValue: new MusicstoreService(null, null) }, 
        { provide: ActivatedRoute, useValue: { params: routeParams }  }
      ],
      declarations: [ AlbumdetailsComponent, FormatdurationPipe, TimesPipe ]
    });

    fixture = TestBed.createComponent(AlbumdetailsComponent);
    component = fixture.componentInstance;
  });
  
  afterEach(() => {
    fixture.destroy();
  });

  it('should create', () => {
    
    expect(component).toBeTruthy();
  });
  
  it('MusicService.getAlbumById function should be called when an ID parameter is supplied to the components route',()=>{

    const spy = spyOn(TestBed.get(MusicstoreService), 'getAlbumById').and.returnValue(Observable.of(album));
    spyOn(TestBed.get(MusicstoreService), 'getAlbumOwnershipStatus').and.returnValue(Observable.of({}));

    fixture.detectChanges();
    routeParams.next({ id: 3 });    
    //fixture.detectChanges();

    expect(spy).toHaveBeenCalled();
  });


  it('buyAlbumButtonOnClick() should call MusicStoreService.purchaseAlbumForUser() when album is NOT already owned', () => {

    component.album = album;
    component.ownedStatus = { purchaseDate: "2018-01-01", isOwned: false };
    const spy = spyOn(TestBed.get(MusicstoreService), 'purchaseAlbumForUser').and.returnValue(Observable.of({}));
    
    component.buyAlbumButtonOnClick();

    expect(spy).toHaveBeenCalledWith(album.id);
  });


  it('backButtonOnClick() should call location.back()', () => {

    const spy = spyOn(TestBed.get(Location), 'back');
    
    component.backButtonOnClick();

    expect(spy).toHaveBeenCalled();
  });

});
