import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';


@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {

  constructor() { }

  @Input('page')
  page: number = 0;
  @Input('numpages')
  numPages: number = 0;

  @Output('pagechange')
  pageChangeEvent = new EventEmitter();

  ngOnInit() {
  }

  onClickUpdatePage(index: number):void{
    if(index != this.page){
      this.pageChangeEvent.emit(index);
    }
  }

}
