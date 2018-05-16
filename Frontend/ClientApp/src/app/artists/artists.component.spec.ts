import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ArtistsComponent } from './artists.component';
import { RouterTestingModule } from '@angular/router/testing';
import { TimesPipe } from '../pipes/times.pipe';
import { MusicstoreService } from '../services/musicstore.service';
import { Observable, of } from 'rxjs';
import { ArtistList } from '../models/artistmodels';
import { By } from '@angular/platform-browser';
import { PaginationComponent } from '../pagination/pagination.component';

describe('ArtistsComponent', () => {
  let component: ArtistsComponent;
  let fixture: ComponentFixture<ArtistsComponent>;
  let service = new MusicstoreService(null, null);

  beforeEach(() => {
    
    TestBed.configureTestingModule({
      imports: [ RouterTestingModule.withRoutes([]) ],
      providers: [ 
        { provide: MusicstoreService, useValue: service }, 
      ],
      declarations: [ ArtistsComponent, TimesPipe, PaginationComponent ]
    });
    fixture = TestBed.createComponent(ArtistsComponent);
    component = fixture.componentInstance;
    
  });

  it('should create', () => {
    spyOn(service, 'getArtists').and.returnValue(of({}));
    fixture.detectChanges();

    expect(component).toBeTruthy();
  });

  // it('should create', () => {
  //   const spy = spyOn(service, 'getArtists').and.returnValue(Observable.of<ArtistList>({ items: [],totalItems: 4, pageIndex: 0 , pageSize: 2 }));

  //   fixture.detectChanges();
  //   let btn = fixture.debugElement.query(By.css('.pagination-link'));
  //   btn.triggerEventHandler('click', null);
  //   expect(spy).toHaveBeenCalled();
  // });

  it('loadArtists function should call MusicService.getArtists function with the supplied page', () => {
    const spy = spyOn(service, 'getArtists').and.returnValue(of({}));
    fixture.detectChanges();

    component.loadArtists(3);

    expect(spy).toHaveBeenCalledWith(3);
  })
});
