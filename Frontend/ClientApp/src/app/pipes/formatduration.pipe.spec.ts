import { FormatdurationPipe } from './formatduration.pipe';

describe('FormatdurationPipe', () => {
  it('create an instance', () => {
    const pipe = new FormatdurationPipe();
    expect(pipe).toBeTruthy();
  });

  it('should return an empty string if null input is provided', ()=>{
    const pipe = new FormatdurationPipe();
    expect(pipe.transform(null)).toBe('');
  });

  it('should return minuites component formatted to zero when input is less than 60 seconds', ()=>{
    const pipe = new FormatdurationPipe();
    expect(pipe.transform(5)).toBe('0:05');
  });

  it('should pad seconds when the seconds remainder is less than ten', ()=>{
    const pipe = new FormatdurationPipe();
    expect(pipe.transform(65)).toBe('1:05');
  });

  it('should contain hour in output when total seconds is greater than 1 hour', ()=>{
    const pipe = new FormatdurationPipe();
    expect(pipe.transform(3605)).toBe('1:00:05');
  });

  
});
