import { FormatdurationPipe } from '../pipes/formatduration.pipe';
import { MusicstoreService } from '../services/musicstore.service';
import { Observable, Subject } from 'rxjs';
import { AlbumsComponent } from './albums.component';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute, Params } from '@angular/router';
import { PaginationComponent } from '../pagination/pagination.component';
import { FormsModule } from '@angular/forms';
import { TimesPipe } from '../pipes/times.pipe';


describe('AlbumsComponent', () => {
  let component: AlbumsComponent;
  let fixture: ComponentFixture<AlbumsComponent>;
  let routeParams: Subject<Params>;
  
  beforeEach(() => {

    routeParams = new Subject<Params>();

    TestBed.configureTestingModule({
      imports: [ 
        RouterTestingModule.withRoutes([]),
        FormsModule
      ],
      providers: [ 
        { provide: MusicstoreService, useValue: new MusicstoreService(null, null) }, 
        { provide: ActivatedRoute, useValue: { params: routeParams }  }
      ],
      declarations: [ AlbumsComponent, PaginationComponent, TimesPipe ]
    });

    fixture = TestBed.createComponent(AlbumsComponent);
    component = fixture.componentInstance;
  });

  afterEach(() => {
    fixture.destroy();
  });

  it('should create', () => {
    fixture.autoDetectChanges();  
    expect(component).toBeTruthy();
  });
  
  it('when a route param value of "groupkey" is supplied the component should call the initWithGroup method with the supplied key and the first page index', () => {
    fixture.autoDetectChanges();
    const spy = spyOn(component, 'initWithGroup');
    routeParams.next({ groupkey: 'itemkey' });

    expect(spy).toHaveBeenCalledWith('itemkey', 0);
  });

  it('when a route param value of "artistid" is supplied the component should call the initWithArtist method with the supplied id and the first page index', () => {
    fixture.autoDetectChanges();
    const spy = spyOn(component, 'initWithArtist');
    routeParams.next({ artistid: '4' });

    expect(spy).toHaveBeenCalledWith(4, 0);
  });

  it('initWithArtist to retrieve data from service with the supplied artistID', () => {
    fixture.autoDetectChanges();
    const getArtistByIdSpy = spyOn(TestBed.get(MusicstoreService), 'getArtistById').and.returnValue(Observable.empty());
    const getAlbumsByArtistSpy = spyOn(TestBed.get(MusicstoreService), 'getAlbumsByArtist').and.returnValue(Observable.empty());
    const getGenresIdSpy = spyOn(TestBed.get(MusicstoreService), 'getGenres').and.returnValue(Observable.empty());
    
    component.initWithArtist(4, 0);

    expect(getArtistByIdSpy).toHaveBeenCalledWith(4);
    expect(getAlbumsByArtistSpy).toHaveBeenCalledWith(4, 0);
    expect(getGenresIdSpy).toHaveBeenCalled();
  });

  it('initWithGroup to retrieve data from service with the supplied group key', () => {
    fixture.autoDetectChanges();
    const getAlbumGroupByKeySpy = spyOn(TestBed.get(MusicstoreService), 'getAlbumGroupByKey').and.returnValue(Observable.empty());
    const getAlbumsByGroupSpy = spyOn(TestBed.get(MusicstoreService), 'getAlbumsByGroup').and.returnValue(Observable.empty());
    const getGenresIdSpy = spyOn(TestBed.get(MusicstoreService), 'getGenres').and.returnValue(Observable.empty());
    
    component.initWithGroup('testGroupKey', 0);

    expect(getAlbumGroupByKeySpy).toHaveBeenCalledWith('testGroupKey');
    expect(getAlbumsByGroupSpy).toHaveBeenCalledWith('testGroupKey', 0);
    expect(getGenresIdSpy).toHaveBeenCalled();
  });
});
