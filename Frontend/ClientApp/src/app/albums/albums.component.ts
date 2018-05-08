import { Component, OnInit, OnDestroy } from '@angular/core';
import { MusicstoreService } from '../services/musicstore.service';
import { Artist } from '../models/artistmodels';
import { AlbumDetail } from '../models/albummodels';
import { GenreDetail } from '../models/genremodels';
import { ActivatedRoute, Router } from '@angular/router';
import { AlbumGroupDetail } from '../models/albumgroupmodels';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-albums',
  templateUrl: './albums.component.html',
  styleUrls: ['./albums.component.less'],
  providers: []
})
export class AlbumsComponent implements OnInit, OnDestroy {

  private title: string;
  private group: string = null;
  private artistId: number = null;

  allFeaturedAlbums: AlbumDetail[];
  featuredAlbums: AlbumDetail[];
  genres: GenreDetail[];
  pageIndex: number = 0;
  numPages: number = 1;
  

  // filter for list
  selectedGenre: string ='';

  private routeSub:Subscription;
  
  public constructor(private service: MusicstoreService, private route: ActivatedRoute, private router: Router){

  }

  ngOnInit(){
    
    this.routeSub = this.route.params.subscribe(params => {
      
      if (params['groupkey']) { 
        this.group = params['groupkey'];
        this.initWithGroup(this.group, 0)
      }else if(params['artistid']){
          this.artistId = parseInt(params['artistid']);
          this.initWithArtist(this.artistId, 0);
      }
    });
  }

  ngOnDestroy(){
    if(this.routeSub){
      this.routeSub.unsubscribe();
    }
  }

  onClickUpdatePage(page: number): void{
    if(page > -1 && page < this.numPages){
      if(this.artistId != null){
        this.initWithArtist(this.artistId, page)
      }else{
        this.initWithGroup(this.group, page);
      }
    }
  }

  // sets up the page with the supplied album group key
  initWithGroup(key: string, page: number){
    
    this.service.getAlbumGroupByKey(key).subscribe(g => this.title = g .name);
    this.service.getAlbumsByGroup(key, page)
      .subscribe(result =>{
        this.allFeaturedAlbums = result.items;
        this.featuredAlbums = this.allFeaturedAlbums;
        this.numPages = Math.floor(result.totalItems / result.pageSize) + ((result.totalItems % result.pageSize == 0) ? 0 : 1);
        this.pageIndex = result.pageIndex;
      });

    this.service.getGenres().subscribe(result =>{
      // get all genres that have albums
      this.genres = result.genres.filter( g => g.totalAlbums > 0);
    });
    
  }

  initWithArtist(artistId: number, page: number){
    this.service.getArtistById(artistId).subscribe(g => this.title = 'Albums by ' + g.name);
    this.service.getAlbumsByArtist(artistId, page)
      .subscribe(result =>{
        this.allFeaturedAlbums = result.items;
        this.featuredAlbums = this.allFeaturedAlbums;
        this.numPages = (result.totalItems / result.pageSize) + ((result.totalItems % result.pageSize == 0) ? 0 : 1);
        this.pageIndex = result.pageIndex;
      });

    this.service.getGenres().subscribe(result =>{
      // get all genres that have albums
      this.genres = result.genres.filter( g => g.totalAlbums > 0);
    });
    
  }

  onChangeGenreFilter(genre: string): void{
    if(genre){
      this.featuredAlbums = this.allFeaturedAlbums.filter(x => x.genres.indexOf(genre) != -1 );
    }else{
      this.featuredAlbums = this.allFeaturedAlbums;
    }
  }

  
}
