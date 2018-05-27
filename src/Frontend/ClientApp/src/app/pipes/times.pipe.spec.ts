import { TimesPipe } from './times.pipe';

describe('TimesPipe', () => {

  let pipe: TimesPipe;

  beforeEach( () => {
    pipe = new TimesPipe();
  })

  it('create an instance', () => {
    
    expect(pipe).toBeTruthy();
  });

  it('should contain an array of values matching the input number', ()=>{
    const formattedResult:number[] = pipe.transform(2);

    expect(formattedResult).toEqual([1,2]);
  });

  it('length of resulting array should match the input number', ()=>{
    const formattedResult:number[] = pipe.transform(2);

    expect(formattedResult.length).toBe(2);
  });

  it('length of resulting array should be zero if null is provided as the input', ()=>{
    const formattedResult:number[] = pipe.transform(null);

    expect(formattedResult.length).toBe(0);
  });
});
