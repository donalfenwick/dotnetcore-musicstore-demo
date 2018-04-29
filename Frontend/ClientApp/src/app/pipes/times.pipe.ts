import { Pipe, PipeTransform } from '@angular/core';

// returns an iterator so that *ngFor="let i of 10|times" can be used to iterate without an array
@Pipe({name: 'times'})
export class TimesPipe implements PipeTransform {
  transform(value: number): any {
    const iterable = {};
    iterable[Symbol.iterator] = function* () {
      let n = 0;
      while (n < value) {
        yield ++n;
      }
    };
    return iterable;
  }
}