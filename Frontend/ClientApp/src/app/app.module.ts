import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgDatepickerModule } from 'ng2-datepicker';

import { AppComponent } from './app.component';
import { AlbumsComponent } from './albums/albums.component';
import { AlbumdetailsComponent } from './albumdetails/albumdetails.component';
import { FormatdurationPipe } from './pipes/formatduration.pipe';
import { ArtistsComponent } from './artists/artists.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AuthService } from './services/auth.service';
import { AuthSilentrefreshCallbackComponent } from './auth-silentrefresh-callback/auth-silentrefresh-callback.component';
import { UserDetailsComponent } from './user-details/user-details.component';
import { AuthGuardService } from './guards/auth-guard.service';
import { MusicstoreService } from './services/musicstore.service';
import { SearchResultsComponent } from './search-results/search-results.component';
import { TimesPipe } from './pipes/times.pipe';
import { UserManager } from 'oidc-client';
import { ConfiguredUserManager } from './auth/ConfiguredUserManager';

@NgModule({
  declarations: [
    AppComponent,
    AlbumdetailsComponent,
    AlbumsComponent,
    FormatdurationPipe,
    ArtistsComponent,
    AuthCallbackComponent,
    AuthSilentrefreshCallbackComponent,
    UserDetailsComponent,
    SearchResultsComponent,
    TimesPipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgDatepickerModule,
    RouterModule.forRoot([
      { path: 'auth-callback', component: AuthCallbackComponent },
      {path: 'auth-silentrefresh-callback', component: AuthSilentrefreshCallbackComponent },
      { path: '', redirectTo: 'albums/FEATURED_ALBUMS', pathMatch: 'full' },
      { path: 'albums/:groupkey', component: AlbumsComponent},
      { path: 'artists', component: ArtistsComponent },
      { path: 'artists/:artistid/albums', component: AlbumsComponent},
      { path: 'albumdetails/:id', component: AlbumdetailsComponent },
      { path: 'user/profile', component: UserDetailsComponent, canActivate: [AuthGuardService] },
      { path: 'albums/search/:query', component: SearchResultsComponent }
    ])
  ],
  providers: [AuthService, AuthGuardService, MusicstoreService,{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  },
  {
    provide: UserManager,
    useClass: ConfiguredUserManager
  }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
