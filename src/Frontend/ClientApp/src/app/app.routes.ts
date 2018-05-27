import { AuthCallbackComponent } from "./auth-callback/auth-callback.component";
import { AlbumsComponent } from "./albums/albums.component";
import { ArtistsComponent } from "./artists/artists.component";
import { AlbumdetailsComponent } from "./albumdetails/albumdetails.component";
import { UserDetailsComponent } from "./user-details/user-details.component";
import { AuthGuardService } from "./guards/auth-guard.service";
import { SearchResultsComponent } from "./search-results/search-results.component";
import { Route } from "@angular/router";


export class ApplicationRoutes{
    static routes: Route[] = [
        { path: 'auth-callback', component: AuthCallbackComponent },
        { path: '', redirectTo: 'albums/FEATURED_ALBUMS', pathMatch: 'full' },
        { path: 'albums/:groupkey', component: AlbumsComponent},
        { path: 'artists', component: ArtistsComponent },
        { path: 'artists/:artistid/albums', component: AlbumsComponent},
        { path: 'albumdetails/:id', component: AlbumdetailsComponent },
        { path: 'user/profile', component: UserDetailsComponent, canActivate: [AuthGuardService] },
        { path: 'albums/search/:query', component: SearchResultsComponent }
      ]
}