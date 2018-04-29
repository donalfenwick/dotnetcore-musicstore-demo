import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatduration'
})
export class FormatdurationPipe implements PipeTransform {

  transform(value: number, args?: any): any {
    let hours = Math.floor(value / 3600);
    value %= 3600;
    let minutes = Math.floor(value / 60);
    let seconds = value % 60;
    if( hours > 0 ){
      return hours+ ':'+minutes+':'+seconds;
    } else {
      return minutes + ':' + this.zeroPad(seconds, 2);
    }
  }

  zeroPad(num:number, places: number) {
    var zero = places - num.toString().length + 1;
    return Array(+(zero > 0 && zero)).join("0") + num;
  }

}
