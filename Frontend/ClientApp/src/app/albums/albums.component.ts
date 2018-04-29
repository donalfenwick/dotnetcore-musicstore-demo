import { Component } from '@angular/core';
import { MusicstoreService } from '../services/musicstore.service';
import { Artist } from '../models/artistmodels';
import { AlbumDetail } from '../models/albummodels';
import { GenreDetail } from '../models/genremodels';
import { ActivatedRoute, Router } from '@angular/router';
import { AlbumGroupDetail } from '../models/albumgroupmodels';

@Component({
  selector: 'app-albums',
  templateUrl: './albums.component.html',
  styleUrls: ['./albums.component.less'],
  providers: [MusicstoreService]
})
export class AlbumsComponent {

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

  public constructor(private service: MusicstoreService, private route: ActivatedRoute, private router: Router){

  }

  ngOnInit(){
    //let filterGenre: string = ;
    console.log('calling init method of home component')

    this.route.params.subscribe(params => {
      console.log(params);
      if (params['groupkey']) { 
        this.group = params['groupkey'];
        this.initFromGroup(this.group, 0)
      }else if(params['artistid']){
          this.artistId = parseInt(params['artistid']);
          this.initWithArtist(this.artistId, 0);
      }
    });
  }
  onClickUpdatePage(page: number): void{
    if(page > -1 && page < this.numPages){
      if(this.artistId != null){
        this.initWithArtist(this.artistId, page)
      }else{
        this.initFromGroup(this.group, page);
      }
    }
  }

  // sets up the page with the supplied album group key
  initFromGroup(key: string, page: number){
    
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
