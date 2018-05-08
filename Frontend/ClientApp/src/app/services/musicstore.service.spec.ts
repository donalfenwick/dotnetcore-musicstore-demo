import { TestBed, getTestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { MusicstoreService } from './musicstore.service';
import { HttpClientModule, HttpClient, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthService } from './auth.service';
import { AuthServiceMock } from '../test/AuthServiceMock';
import { Observable } from "rxjs/Observable";
import { ArtistList, Artist } from '../models/artistmodels';
import { AuthInterceptor } from '../interceptors/auth.interceptor';
import { GenreList } from '../models/genremodels';
import { ALbumList, AlbumDetail, UserAlbumOwnershipStatus } from '../models/albummodels';
import { AlbumGroupDetail } from '../models/albumgroupmodels';
import { UserProfile } from '../models/usermodels';
describe('MusicstoreService', () => {
  let httpMock: HttpTestingController;
  let service: MusicstoreService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers: [
        MusicstoreService,
        { provide: AuthService, useValue: new AuthServiceMock(null) },
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
      ]
    });
    service = TestBed.get(MusicstoreService);
    httpMock = TestBed.get(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', inject([MusicstoreService], (service: MusicstoreService) => {
    expect(service).toBeTruthy();
  }));

  

  it('getGenres should call the correct backend http endpoint',  () => {
    const resultData = <GenreList>{ genres:[] };

    service.getGenres().subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/genre`);
    expect(req.request.method).toBe('GET');

    req.flush(resultData);
  });


  it('getArtists should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData: ArtistList = {  items: [], pageIndex: 1, pageSize: service.pageSize, totalItems: 40 };

    service.getArtists(1).subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/artist`);
    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('page')).toBe('1');
    expect(req.request.params.get('pageSize')).toBe(service.pageSize.toString());

    req.flush(resultData);
  });

  it('getAlbumsByGroup should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData: ALbumList = {  items: [], pageIndex: 1, pageSize: service.pageSize, totalItems: 40 };

    service.getAlbumsByGroup('groupKeyValue', 1).subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/album/group/groupKeyValue`);
    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('page')).toBe('1');
    expect(req.request.params.get('pageSize')).toBe(service.pageSize.toString());

    req.flush(resultData);
  });

  it('searchAlbums should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData: ALbumList = {  items: [], pageIndex: 1, pageSize: service.pageSize, totalItems: 40 };

    service.searchAlbums('searchQuery', 1).subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/album/search`);
    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('page')).toBe('1');
    expect(req.request.params.get('query')).toBe('searchQuery');
    expect(req.request.params.get('pageSize')).toBe(service.pageSize.toString());

    req.flush(resultData);
  });

  it('getAlbumsByArtist should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData: ALbumList = {  items: [], pageIndex: 1, pageSize: service.pageSize, totalItems: 40 };
    const artistid = 1234;

    service.getAlbumsByArtist(artistid, 1).subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/artist/${artistid}/albums`);
    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('page')).toBe('1');
    expect(req.request.params.get('pageSize')).toBe(service.pageSize.toString());

    req.flush(resultData);
  });

  it('getArtistById should call the correct backend http endpoint',  () => {
    const artistid = 1234;
    const resultData = <Artist>{ id: artistid };

    service.getArtistById(artistid).subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/artist/${artistid}`);
    expect(req.request.method).toBe('GET');

    req.flush(resultData);
  });

  it('getPurchasedAlbumsForUser should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData: ALbumList = {  items: [], pageIndex: 1, pageSize: service.pageSize, totalItems: 40 };

    service.getPurchasedAlbumsForUser(1).subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/album/purchased`);
    expect(req.request.method).toBe('GET');
    expect(req.request.params.get('page')).toBe('1');
    expect(req.request.params.get('pageSize')).toBe(service.pageSize.toString());

    req.flush(resultData);
  });

  it('getPurchasedAlbumsForUser should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData: AlbumGroupDetail = {  key: '', name: '', totalAlbums: 1 };

    service.getAlbumGroupByKey('testGroupName').subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/albumgroup/testGroupName`);
    expect(req.request.method).toBe('GET');

    req.flush(resultData);
  });
 
  it('getAlbumById should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData = <AlbumDetail>{ id: 1234 };

    service.getAlbumById(1234).subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/album/1234`);
    expect(req.request.method).toBe('GET');

    req.flush(resultData);
  }); 

  it('purchaseAlbumForUser should call the correct backend http endpoint',  () => {

    service.purchaseAlbumForUser(1234).subscribe();

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/album/1234/purchase`);
    expect(req.request.method).toBe('POST');
  }); 
  
  it('getAlbumOwnershipStatus should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData: UserAlbumOwnershipStatus = { isOwned: true, purchaseDate: '2018-01-01' };

    service.getAlbumOwnershipStatus(1234).subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/album/1234/owenershipstatus`);
    expect(req.request.method).toBe('GET');

    req.flush(resultData);
  });

  it('getUserProfile should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData: UserProfile = { username: 'user1', firstname: 'name', surname: 'name', emailAddress: 'user@address.tld', phoneNumber: '12345', dateOfBirth: '2018-01-01' };

    service.getUserProfile().subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/user/me/profile`);
    expect(req.request.method).toBe('GET');

    req.flush(resultData);
  });

  it('getAlbumOwnershipStatus should call the correct backend http endpoint and suply correct parametrs',  () => {
    const resultData: UserProfile = { username: 'user1', firstname: 'name', surname: 'name', emailAddress: 'user@address.tld', phoneNumber: '12345', dateOfBirth: '2018-01-01' };

    service.updateUserProfile(resultData).subscribe( r => {
      expect(r).toBe(resultData);
    });

    var req = httpMock.expectOne( r => r.url === `${service.baseUrl}/user/me/profile`);
    expect(req.request.method).toBe('POST');

    req.flush(resultData);
  });
  
  
});
