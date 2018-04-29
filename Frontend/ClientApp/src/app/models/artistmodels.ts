export interface Artist{
    id: number;
    name: string;
    bioImageUrl: string;
    bioText: string;
    genres: string[];
    publishedStatus: string;
}

export interface ArtistList
{
    items: Artist[];
    
    totalItems: number;
    pageIndex: number;
    pageSize: number;
}