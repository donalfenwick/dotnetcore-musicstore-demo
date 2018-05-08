import { TestBed, ComponentFixture, fakeAsync, tick } from "@angular/core/testing";
import { UserManager } from "oidc-client";
import { AuthService } from "./auth.service";
import { OidcUserStub } from "../test/OidcUserStub";

describe('AuthService', () => {

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[],
      providers: [
        AuthService,
        UserManager
      ]
    });
    spyOnProperty(TestBed.get(UserManager),'events').and.returnValue({
      // mock the callback event called from the AuthService constructor
      addSilentRenewError: (fn:Function)=> { fn(); }
    })
  });

  afterEach(() => {
    
  });

  it('should be created', () => {
    const service = TestBed.get(AuthService);
    expect(service).toBeTruthy();
  });
  
  it('initAuth sets the returned user on the service before resolving', fakeAsync(() => {
    var user = new OidcUserStub();
    spyOn(TestBed.get(UserManager), 'getUser').and.returnValue(Promise.resolve(user));
    const service:AuthService = TestBed.get(AuthService);

    const initPromise = service.initAuth();
    
    tick();
    expect(service.user).toEqual(user);
  }));

  it('initAuth resolves the correct error when the wrapped call to getUser() fails', async (done) => {
    let producedError = new Error('error getting user');    
    spyOn(TestBed.get(UserManager), 'getUser').and.returnValue(Promise.reject(producedError));
    const service:AuthService = TestBed.get(AuthService);

    try {
      var result = await service.initAuth();  
      done.fail();
    } catch (e) {
      expect(e).toBe(producedError);
      done();
    }
  });

  it('isLoggedIn() returns true if the user.expired property is false', () => {
    const service:AuthService = TestBed.get(AuthService);
    service.user = new OidcUserStub();
    spyOnProperty(service.user,'expired').and.returnValue(false);

    expect(service.isLoggedIn()).toBeTruthy();
  });

  it('isLoggedIn() returns true if the user object is null', () => {
    const service:AuthService = TestBed.get(AuthService);
    service.user = null;

    expect(service.isLoggedIn()).toBeFalsy();
  });

  it('isLoggedIn() returns true if the user.expired property is true', () => {
    const service:AuthService = TestBed.get(AuthService);
    service.user = new OidcUserStub();
    spyOnProperty(service.user,'expired').and.returnValue(true);

    expect(service.isLoggedIn()).toBeFalsy();
  });

  it('getAuthorizationHeaderValue() formats the header with the correct scheme and token', () => {
    const service:AuthService = TestBed.get(AuthService);
    service.user = new OidcUserStub();
    service.user.access_token = 'abcdef123456';
    service.user.token_type = 'Bearer';

    expect(service.getAuthorizationHeaderValue()).toBe('Bearer abcdef123456');
  });

  it('getAuthorizationHeaderValue() returns empty if access token is null', () => {
    const service:AuthService = TestBed.get(AuthService);
    service.user = new OidcUserStub();
    service.user.access_token = null;

    expect(service.getAuthorizationHeaderValue()).toBe('');
  });

  it('getAccessToken() returns empty if user is null', () => {
    const service:AuthService = TestBed.get(AuthService);
    service.user = null;

    expect(service.getAccessToken()).toBe('');
  });

  it('getAccessToken() returns empty if user is null', () => {
    const service:AuthService = TestBed.get(AuthService);
    service.user = new OidcUserStub();
    service.user.access_token = null;
    expect(service.getAccessToken()).toBe('');
  });

  it('getAccessToken() returns access token when set on the user', () => {
    const service:AuthService = TestBed.get(AuthService);
    service.user = new OidcUserStub();
    service.user.access_token = 'sampleAccessToken';
    expect(service.getAccessToken()).toBe('sampleAccessToken');
  });

  it('startAuthentication() triggers oidc call usermanager.signinRedirect', () => {
    const service:AuthService = TestBed.get(AuthService);
    const spy = spyOn(TestBed.get(UserManager), 'signinRedirect').and.returnValue(Promise.resolve());

    service.startAuthentication();

    expect(spy).toHaveBeenCalled();
  });

  it('startSignout() triggers oidc call usermanager.signoutRedirect', () => {
    const service:AuthService = TestBed.get(AuthService);
    const spy = spyOn(TestBed.get(UserManager), 'signoutRedirect').and.returnValue(Promise.resolve());

    service.startSignout();

    expect(spy).toHaveBeenCalled();
  });

  it('completeAuthentication() triggers oidc call usermanager.signinRedirectCallback', () => {
    const service:AuthService = TestBed.get(AuthService);
    const usr = new OidcUserStub();
    const spy = spyOn(TestBed.get(UserManager), 'signinRedirectCallback').and.returnValue(Promise.resolve(usr));

    service.completeAuthentication();

    expect(spy).toHaveBeenCalled();
  });

  it('completeAuthentication() should populate the user property on the service once its promise resolves', async () => {
    const service:AuthService = TestBed.get(AuthService);
    const usr = new OidcUserStub();
    const spy = spyOn(TestBed.get(UserManager), 'signinRedirectCallback').and.returnValue(Promise.resolve(usr));

    await service.completeAuthentication();

    expect(service.user).toEqual(usr);
  });

});
