import { Component, OnInit, OnDestroy } from '@angular/core';
import { AlbumDetail } from '../models/albummodels';
import { MusicstoreService } from '../services/musicstore.service';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css','./../albums/albums.component.less']
})
export class SearchResultsComponent implements OnInit, OnDestroy {

  constructor(private service: MusicstoreService, private route: ActivatedRoute) { }


  results: AlbumDetail[];
  isSearching: boolean = false;
  searchQuery: string;
  pageIndex = 0;
  numPages = 0;

  private routeSub:Subscription;

  ngOnInit() {

    this.routeSub = this.route.params.subscribe(params => {
      if (params['query']) { 
        this.searchQuery = params['query'];
        this.beginSearch(this.searchQuery, 0);
      }
    });
  }

  ngOnDestroy(){
    if(this.routeSub){
      this.routeSub.unsubscribe();
    }
  }

  public beginSearch(expression:string, page: number):void{
    this.isSearching = true;
    this.service.searchAlbums(expression, page)
      .subscribe( result => {
        this.results = result.items;
        this.pageIndex = result.pageIndex;
        this.numPages = Math.floor(result.totalItems / result.pageSize) + ((result.totalItems % result.pageSize == 0) ? 0 : 1);
        this.isSearching = false;
      });
  }

  onClickUpdatePage(page: number):void{
    this.beginSearch(this.searchQuery, page);
  }
}
