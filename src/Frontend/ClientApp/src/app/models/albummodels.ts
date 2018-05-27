export interface ALbumList{
    items : AlbumDetail[];
    pageIndex: number;
    totalItems: number;
    pageSize: number;
}

export class AlbumDetail{
    id: number;
    title: string;
    artistName: string;
    artistId: number;
    descriptionText: string;
    producer: string;
    label: string;
    totalDurationInSeconds: number; 
    coverImageUrl: string;
    releaseDate: string;
    
    tracks: AlbumTrack[];

    genres: string[];
}

export interface AlbumTrack{
    title: string;
    trackNumber: number;
    durationInSeconds: number;
}

export interface UserAlbumOwnershipStatus{
    purchaseDate: string;
    isOwned: boolean;
}