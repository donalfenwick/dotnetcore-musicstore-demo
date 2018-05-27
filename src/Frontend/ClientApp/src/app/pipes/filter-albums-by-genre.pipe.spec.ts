import { FilterAlbumsByGenrePipe } from './filter-albums-by-genre.pipe';
import { AlbumDetail } from '../models/albummodels';

describe('FilterAlbumsByGenrePipePipe', () => {
  it('create an instance', () => {
    const pipe = new FilterAlbumsByGenrePipe();
    expect(pipe).toBeTruthy();
  });

  it('should return null when albums input is provided', () => {
    const pipe = new FilterAlbumsByGenrePipe();

    let result = pipe.transform(null, 'samplefilter');

    expect(result).toBeNull();
  });


  it('should NOT return null when non null albums input is provided', () => {
    const pipe = new FilterAlbumsByGenrePipe();

    let result = pipe.transform([], 'samplefilter');

    expect(result).not.toBeNull();
  });

  it('should return full list when null filter is provided', () => {
    const pipe = new FilterAlbumsByGenrePipe();
    const albums: AlbumDetail[] = [
      <AlbumDetail>{ id: 1, title: 'album1', genres: [ 'rock', 'country' ] },
      <AlbumDetail>{ id: 2, title: 'album2', genres: [ 'rock', 'alternative' ] }, 
      <AlbumDetail>{ id: 3, title: 'album3', genres: [ 'dance' ] }, 
      <AlbumDetail>{ id: 4, title: 'album4', genres: [ 'rock', 'punk'] } 
    ];

    let results = pipe.transform(albums, null);

    expect(results.length).toBe(4);
  });

  it('should return full list when empty filter is provided', () => {
    const pipe = new FilterAlbumsByGenrePipe();
    const albums: AlbumDetail[] = [
      <AlbumDetail>{ id: 1, title: 'album1', genres: [ 'rock', 'country' ] },
      <AlbumDetail>{ id: 2, title: 'album2', genres: [ 'rock', 'alternative' ] }, 
      <AlbumDetail>{ id: 3, title: 'album3', genres: [ 'dance' ] }, 
      <AlbumDetail>{ id: 4, title: 'album4', genres: [ 'rock', 'punk'] } 
    ];

    let results = pipe.transform(albums, '');

    expect(results.length).toBe(4);
  });

  it('should return only albums that have a matching genre', () => {
    const pipe = new FilterAlbumsByGenrePipe();
    const albums: AlbumDetail[] = [
      <AlbumDetail>{ id: 1, title: 'album1', genres: [ 'rock', 'country' ] },
      <AlbumDetail>{ id: 2, title: 'album2', genres: [ 'rock', 'alternative' ] }, 
      <AlbumDetail>{ id: 3, title: 'album3', genres: [ 'dance' ] }, 
      <AlbumDetail>{ id: 4, title: 'album4', genres: [ 'rock', 'punk'] } 
    ];

    let results = pipe.transform(albums, 'rock');

    expect(results.length).toBe(3);
    expect(results.every(x => x.genres.indexOf('rock') != -1));
  });

  it('should not fail when an albums genres list is set to null', () => {
    const pipe = new FilterAlbumsByGenrePipe();
    const albums: AlbumDetail[] = [
      <AlbumDetail>{ id: 1, title: 'album1', genres: [ 'rock', 'country' ] },
      <AlbumDetail>{ id: 2, title: 'album2', genres: [ 'rock', 'alternative' ] }, 
      <AlbumDetail>{ id: 3, title: 'album3', genres: null }, 
      <AlbumDetail>{ id: 4, title: 'album4', genres: [ 'rock', 'punk'] } 
    ];

    let results = pipe.transform(albums, 'alternative');

    expect(results.length).toBe(1);
  });

  it('should not fail with a null element in the albums array', () => {
    const pipe = new FilterAlbumsByGenrePipe();
    const albums: AlbumDetail[] = [
      <AlbumDetail>{ id: 1, title: 'album1', genres: [ 'rock', 'country' ] },
      null
    ];

    let results = pipe.transform(albums, 'rock');

    expect(results.length).toBe(1);
  });
});
