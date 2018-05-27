import { Pipe, PipeTransform } from '@angular/core';

// returns an array of ints so that *ngFor="let i of 10|times" can be used to iterate without an array
@Pipe({name: 'times'})
export class TimesPipe implements PipeTransform {
  transform(value: number): any {
    let result: number[] = [];
    if(value){
      for(let i = 1; i <= value; i++){
        result.push(i);
      }
    }
    return result;
  }
}