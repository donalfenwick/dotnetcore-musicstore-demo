import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AlbumsComponent } from './albums/albums.component';
import { AlbumdetailsComponent } from './albumdetails/albumdetails.component';
import { FormatdurationPipe } from './pipes/formatduration.pipe';
import { ArtistsComponent } from './artists/artists.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AuthService } from './services/auth.service';
import { UserDetailsComponent } from './user-details/user-details.component';
import { AuthGuardService } from './guards/auth-guard.service';
import { MusicstoreService } from './services/musicstore.service';
import { SearchResultsComponent } from './search-results/search-results.component';
import { TimesPipe } from './pipes/times.pipe';
import { UserManager, UserManagerSettings } from 'oidc-client';
import { ConfiguredUserManager } from './auth/ConfiguredUserManager';
import { PaginationComponent } from './pagination/pagination.component';
import { ApplicationRoutes } from './app.routes';
import { NavmenuComponent } from './navmenu/navmenu.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { AppInitService } from './app-init.service';
import { FilterAlbumsByGenrePipe } from './pipes/filter-albums-by-genre.pipe';
import { environment } from '../environments/environment';

export function app_Init(initsvc: AppInitService){
  return () => initsvc.initApp();
}
@NgModule({
  declarations: [
    AppComponent,
    AlbumdetailsComponent,
    AlbumsComponent,
    FormatdurationPipe,
    ArtistsComponent,
    AuthCallbackComponent,
    UserDetailsComponent,
    SearchResultsComponent,
    TimesPipe,
    PaginationComponent,
    NavmenuComponent,
    FilterAlbumsByGenrePipe
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(ApplicationRoutes.routes),
    NgbModule.forRoot(),
  ],
  providers: [
    AuthService, 
    AuthGuardService, 
    MusicstoreService,
    AppInitService,
    { provide: APP_INITIALIZER, useFactory: app_Init, deps: [ AppInitService ], multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    { 
      provide: UserManager, 
      useFactory: () => { 

        let siteBasePath = 'http://localhost:5600/';
        let identotyServerAuthority = 'http://localhost:5601/';

        if(environment.siteBasePath && environment.siteBasePath.length > 0){
          siteBasePath = environment.siteBasePath;
        }
        if(environment.identotyServerAuthority && environment.identotyServerAuthority.length > 0){
          identotyServerAuthority = environment.identotyServerAuthority;
        }

        let settings: UserManagerSettings = {
          authority: identotyServerAuthority,
          client_id: 'musicStoreAngularFrotend',
          redirect_uri: `${siteBasePath}auth-callback`,
          post_logout_redirect_uri: siteBasePath,
          response_type:"id_token token",
          scope:"openid profile apiAccess email roles",
          filterProtocolClaims: true,
          loadUserInfo: true,
          automaticSilentRenew: true,
          silent_redirect_uri: `${siteBasePath}assets/silent-refresh.html`
        };
        return new UserManager(settings);
      }  
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
