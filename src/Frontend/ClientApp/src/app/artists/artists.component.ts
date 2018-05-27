import { Component, OnInit } from '@angular/core';
import { MusicstoreService } from '../services/musicstore.service';
import { Artist } from '../models/artistmodels';

@Component({
  selector: 'app-artists',
  templateUrl: './artists.component.html',
  styleUrls: ['./artists.component.less', './../albums/albums.component.less']
})
export class ArtistsComponent implements OnInit {

  title: string = 'Artists';
  artists: Artist[];
  pageIndex: number = 0;
  numPages: number;
  constructor(private service: MusicstoreService) { }

  ngOnInit() {
    this.loadArtists(0);
  }

  loadArtists(page: number){

    this.service.getArtists(page)
      .subscribe(result => {
        this.artists = result.items;
        this.pageIndex = result.pageIndex;
        this.numPages = Math.floor(result.totalItems / result.pageSize) + ((result.totalItems % result.pageSize == 0) ? 0 : 1);
      });
  }
  onClickUpdatePage(page: number):void{
    this.loadArtists(page);
  }

}
