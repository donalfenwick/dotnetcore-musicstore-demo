import { Injectable } from '@angular/core';
import { } from 'rxjs/add/operator/map';
import { Artist, ArtistList } from './../models/artistmodels';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { ALbumList, AlbumDetail, UserAlbumOwnershipStatus } from '../models/albummodels';
import { GenreList } from '../models/genremodels';
import { AlbumGroupDetail } from '../models/albumgroupmodels';
import { AuthService } from './auth.service';
import { UserProfile } from '../models/usermodels';
@Injectable()
export class MusicstoreService {

  public apiHost:string = 'http://localhost:5600';
  private _baseUrl: string;
  get baseUrl():string { return this._baseUrl; }

  private listPageSize: number = 16;
  get pageSize():number { return this.listPageSize; }

  constructor(private httpClient: HttpClient, private authService: AuthService) { 
    this._baseUrl = `${this.apiHost}/api`;

  }

  getGenres(): Observable<GenreList>{
    return this.httpClient.get<GenreList>(`${this.baseUrl}/genre`);
  }

  getArtists(page: number): Observable<ArtistList>{
    return this.httpClient.get<ArtistList>(`${this.baseUrl}/artist`,{
      params: {
        pageSize: this.listPageSize.toString(), 
        page: (page ? page.toString() : '0')
      } 
    });
  }

  getArtistById(id: number): Observable<Artist>{
    return this.httpClient.get<Artist>(`${this.baseUrl}/artist/${id}`);
  }

  getAlbumsByGroup(group: string, page: number): Observable<ALbumList>{
    let requestParams = new HttpParams()
        .set('page', (page ? page.toString() : '0'))
        .set('pageSize',this.listPageSize.toString());
    return this.httpClient.get<ALbumList>(`${this.baseUrl}/album/group/${group}`, { params: requestParams });
  }

  searchAlbums(queryExpression: string, page: number): Observable<ALbumList>{
    let requestParams = new HttpParams()
        .set('query', queryExpression)
        .set('page', (page ? page.toString() : '0'))
        .set('pageSize',this.listPageSize.toString());
    return this.httpClient.get<ALbumList>(`${this.baseUrl}/album/search`, { params: requestParams });
  }

  getAlbumsByArtist(artistId: number, page: number): Observable<ALbumList>{
    return this.httpClient.get<ALbumList>(`${this.baseUrl}/artist/${artistId}/albums`,{
      params: {
        pageSize: this.listPageSize.toString(), 
        page: (page ? page.toString() : '0')
      } 
    });
  }

  getPurchasedAlbumsForUser(page: number): Observable<ALbumList>{
    return this.httpClient.get<ALbumList>(`${this.baseUrl}/album/purchased`,{
      params: {
        pageSize: this.listPageSize.toString(), 
        page: (page ? page.toString() : '0')
      } 
    });
  }

  getAlbumGroupByKey(key: string): Observable<AlbumGroupDetail>{
    return this.httpClient.get<AlbumGroupDetail>(`${this.baseUrl}/albumgroup/${key}`);
  }

  getAlbumById(id: number): Observable<AlbumDetail>{
    return this.httpClient.get<AlbumDetail>(`${this.baseUrl}/album/${id}`);
  }

  purchaseAlbumForUser(albumId: number): Observable<void>{
    return this.httpClient.post<void>(`${this.baseUrl}/album/${albumId}/purchase`,{});
  }

  getAlbumOwnershipStatus(albumId: number): Observable<UserAlbumOwnershipStatus>{
    return this.httpClient.get<UserAlbumOwnershipStatus>(`${this.baseUrl}/album/${albumId}/owenershipstatus`);
  }
  
  getUserProfile(): Observable<UserProfile>{
    return this.httpClient.get<UserProfile>(`${this.baseUrl}/user/me/profile`);
  }

  updateUserProfile(profile: UserProfile): Observable<UserProfile>{
    return this.httpClient.post<UserProfile>(`${this.baseUrl}/user/me/profile`, profile);
  }
}
