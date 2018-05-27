import { FormatdurationPipe } from './formatduration.pipe';

describe('FormatdurationPipe', () => {


  let pipe: FormatdurationPipe;

  beforeEach(() => { pipe = new FormatdurationPipe();} )

  it('create an instance', () => {
    expect(pipe).toBeTruthy();
  });

  it('should return an empty string if null input is provided', ()=>{
    const result = pipe.transform(null);

    expect(result).toBe('');
  });

  it('should return minuites component formatted to zero when input is less than 60 seconds', ()=>{
    const result = pipe.transform(5);

    expect(result).toBe('0:05');
  });

  it('should pad seconds when the seconds remainder is less than ten', ()=>{
    const result = pipe.transform(65);

    expect(result).toBe('1:05');
  });

  it('should NOT pad seconds when the seconds remainder is greater than or euqal to ten', ()=>{
    const result = pipe.transform(75);

    expect(result).toBe('1:15');
  });

  it('should contain hour in output when total seconds is greater than 1 hour', ()=>{
    const result = pipe.transform(3605);

    expect(result).toBe('1:00:05');
  });

});
