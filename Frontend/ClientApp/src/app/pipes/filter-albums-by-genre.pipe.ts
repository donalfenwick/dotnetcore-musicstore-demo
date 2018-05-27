import { Pipe, PipeTransform } from '@angular/core';
import { AlbumDetail } from '../models/albummodels';

@Pipe({
  name: 'filterAlbumsByGenre'
})
export class FilterAlbumsByGenrePipe implements PipeTransform {

  transform(value: AlbumDetail[], genre: string): AlbumDetail[] {
    if(value != null){
      if(!genre || genre.trim().length === 0){
        return value;
      }else{
        return value.filter( x => x != null && x.genres && x.genres.indexOf(genre) != -1);
      }
    }else{
      return null;
    }
  }

}
