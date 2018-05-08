import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchResultsComponent } from './search-results.component';
import { RouterTestingModule } from '@angular/router/testing';
import { TimesPipe } from '../pipes/times.pipe';
import { MusicstoreService } from '../services/musicstore.service';
import { HttpClientModule } from '@angular/common/http';
import { AuthServiceMock } from '../test/AuthServiceMock';
import { AuthService } from '../services/auth.service';
import { UserManager } from 'oidc-client';
import { PaginationComponent } from '../pagination/pagination.component';
import { Observable, Subject } from 'rxjs';
import { Params, ActivatedRoute } from '@angular/router';
import { AlbumDetail } from '../models/albummodels';

describe('SearchResultsComponent', () => {
  let component: SearchResultsComponent;
  let fixture: ComponentFixture<SearchResultsComponent>;
  let paramValues = new Subject<Params>();
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[
        RouterTestingModule.withRoutes([]), 
        HttpClientModule
      ],
      declarations: [ 
        SearchResultsComponent,
        TimesPipe, 
        PaginationComponent 
      ],
      providers:[
        MusicstoreService,
        { provide: AuthService, useValue: new AuthServiceMock(null) },
        { provide: ActivatedRoute, useValue: { params: paramValues }  },
        UserManager
      ]
    });

    fixture = TestBed.createComponent(SearchResultsComponent);
    component = fixture.componentInstance;
    
  });

  afterEach(() => {
    fixture.destroy();
  });


  it('should create', () => {
    fixture.autoDetectChanges();
    expect(component).toBeTruthy();
  });

  it('onClickUpdatePage() should trigger a call to beginSearch() with the required page and current search query', () => {
    
    const spy = spyOn(component,'beginSearch');
    component.searchQuery = 'searchvalue';
    fixture.detectChanges();
    component.onClickUpdatePage(2);

    expect(spy).toHaveBeenCalledWith('searchvalue', 2);
  });

  it('passing a route param "query" should trigger a call to beginSearch() with supplied query', () => {
    
    fixture.detectChanges();
    
    const spy = spyOn(component,'beginSearch');
    paramValues.next({ query: 'testsearchterm' });
    fixture.detectChanges();
    
    expect(spy).toHaveBeenCalledWith('testsearchterm', 0);
  });

  it('beginSearch should trigger a call to searchAlbums() with supplied query and page', () => {
    
    fixture.detectChanges();
    
    const spy = spyOn(TestBed.get(MusicstoreService),'searchAlbums').and.returnValue(Observable.empty());
    component.beginSearch('testsearchterm',1)
    fixture.detectChanges();
    
    expect(spy).toHaveBeenCalledWith('testsearchterm', 1);
  });

  it('beginSearch should update properties on component', () => {
    
    fixture.detectChanges();
    const sampleResults = <AlbumDetail[]> [{id:1},{id:1},{id:1},{id:1}];
    const spy = spyOn(TestBed.get(MusicstoreService),'searchAlbums').and.returnValue(Observable.of({
      items: sampleResults,
      pageIndex: 1,
      pageSize: 5,
      totalItems: 17
    }));
    component.beginSearch('testsearchterm',1);
    fixture.detectChanges();
    
    expect(component.numPages).toBe(4);
    expect(component.pageIndex).toBe(1);
    expect(component.results).toBe(sampleResults);
  });

});
