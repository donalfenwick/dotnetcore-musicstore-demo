import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PaginationComponent } from './pagination.component';
import { TimesPipe } from '../pipes/times.pipe';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

describe('PaginationComponent', () => {
  let component: PaginationComponent;
  let fixture: ComponentFixture<PaginationComponent>;

  beforeEach(() => {
    
    TestBed.configureTestingModule({
      declarations: [ PaginationComponent, TimesPipe ]
    });

    fixture = TestBed.createComponent(PaginationComponent);
    component = fixture.componentInstance;
    component.numPages = 10;
    component.page = 0;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render the correct number of pagination items', () => {
    
    const numLinks = fixture.debugElement.queryAll(By.css('.pagination-link.item')).length;

    expect(numLinks).toBe(10);
  });

  it('Pagination link within active <li> should have a label value of (pageIndex+1)', () => {
    component.page = 3;
    fixture.detectChanges();

    const el:HTMLElement = fixture.debugElement.query(By.css('li.active > a')).nativeElement;
    
    expect(el.innerText).toBe('4');
  });

  it('onClickUpdatePage should be called when a pagination link was clicked', () => {
    const spy = spyOn(component, 'onClickUpdatePage');
    fixture.detectChanges();

    const el:HTMLAnchorElement = fixture.debugElement.query(By.css('li > .pagination-link.item')).nativeElement;
    el.click();

    expect(spy).toHaveBeenCalled;
  });


  it('pageChangeEvent should emit an event with the page index when onClickUpdatePage is called', () => {
    const spy = spyOn(component.pageChangeEvent, 'emit');
    fixture.detectChanges();

    component.onClickUpdatePage(2);

    expect(spy).toHaveBeenCalledWith(2);
  });

  it('pageChangeEvent should NOT emit an event when onClickUpdatePage is called with the current page index', () => {
    const spy = spyOn(component.pageChangeEvent, 'emit');
    component.page = 1;
    fixture.detectChanges();

    component.onClickUpdatePage(1);

    expect(spy).not.toHaveBeenCalled();
  });

});
