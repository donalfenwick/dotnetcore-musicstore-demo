export interface GenreList{
    genres: GenreDetail[];
}

export interface GenreDetail{
    name: string;
    totalArtists: number;
    totalAlbums: number;
}