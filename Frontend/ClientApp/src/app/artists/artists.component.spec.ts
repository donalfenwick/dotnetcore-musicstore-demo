import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ArtistsComponent } from './artists.component';
import { RouterTestingModule } from '@angular/router/testing';
import { TimesPipe } from '../pipes/times.pipe';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { UserManager } from 'oidc-client';

describe('ArtistsComponent', () => {
  let component: ArtistsComponent;
  let fixture: ComponentFixture<ArtistsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      providers: [AuthService, UserManager],
      imports:[RouterTestingModule.withRoutes([]), HttpClientModule],
      declarations: [ ArtistsComponent, TimesPipe ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ArtistsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
