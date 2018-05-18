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
import { UserManager } from 'oidc-client';
import { ConfiguredUserManager } from './auth/ConfiguredUserManager';
import { PaginationComponent } from './pagination/pagination.component';
import { ApplicationRoutes } from './app.routes';
import { NavmenuComponent } from './navmenu/navmenu.component';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { AppInitService } from './app-init.service';
import { FilterAlbumsByGenrePipe } from './pipes/filter-albums-by-genre.pipe';

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
    { provide: UserManager, useClass: ConfiguredUserManager }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
