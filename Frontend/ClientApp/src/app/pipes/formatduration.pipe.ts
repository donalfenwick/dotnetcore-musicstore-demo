import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatduration'
})
export class FormatdurationPipe implements PipeTransform {

  transform(value: number, args?: any): any {
    if(value){
      let hours = Math.floor(value / 3600);
      value %= 3600;
      let minutes = Math.floor(value / 60);
      let seconds = value % 60;
      if( hours > 0 ){
        return `${hours}:${this.formatTimeComponent(minutes)}:${this.formatTimeComponent(seconds)}`;
      } else {
        return `${minutes}:${this.formatTimeComponent(seconds)}`;
      }
    }else{
      return '';
    }
  }

  private formatTimeComponent(num:number) {
    if(num <=0){
      return '00';
    }
    if(num < 10){
      return '0'+num;
    }
    return num.toString();
  }

}
