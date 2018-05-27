import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { AuthServiceMock } from '../test/AuthServiceMock';
import { AuthInterceptor } from './auth.interceptor';

describe(`AuthHttpInterceptor`, () => {
    
    let httpMock: HttpTestingController;
    let service: AuthService;

    beforeEach(() => {
        TestBed.configureTestingModule({
        imports: [HttpClientTestingModule],
        providers: [
            { provide: AuthService, useValue: new AuthServiceMock(null)},
            { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
        ],
        });

        service = TestBed.get(AuthService);
        httpMock = TestBed.get(HttpTestingController);
    });

    it('should include Authorization header in request if user is logged in in AuthService', inject([HttpClient, HttpTestingController],
        (http: HttpClient, mock: HttpTestingController) => {

        spyOn(service, 'getAccessToken').and.returnValue('fakeauthheader');
        spyOn(service, 'isLoggedIn').and.returnValue(true);
        
        http.get('/api').subscribe(response => expect(response).toBeTruthy());
        const request = mock.expectOne(req => (req.url === '/api'));

        expect(request.request.headers.get('Authorization')).toBe('Bearer fakeauthheader');

        request.flush({data: 'test'});
        mock.verify();
    }));

    it('should ommit Authorization header from request if user NOT logged in in AuthService', inject([HttpClient, HttpTestingController],
        (http: HttpClient, mock: HttpTestingController) => {

        spyOn(service, 'isLoggedIn').and.returnValue(false);
        
        http.get('/api').subscribe(response => expect(response).toBeTruthy());
        const request = mock.expectOne(req => (req.url === '/api'));

        expect(request.request.headers.has('Authorization')).toBe(false);

        request.flush({data: 'test'});
        mock.verify();
    }));
    
    it('should ommit Authorization header from request if access token is null', inject([HttpClient, HttpTestingController],
        (http: HttpClient, mock: HttpTestingController) => {

        spyOn(service, 'getAccessToken').and.returnValue(null);
        
        http.get('/api').subscribe(response => expect(response).toBeTruthy());
        const request = mock.expectOne(req => (req.url === '/api'));

        expect(request.request.headers.has('Authorization')).toBe(false);

        request.flush({data: 'test'});
        mock.verify();
    }));

    afterEach(inject([HttpTestingController], (mock: HttpTestingController) => {
         mock.verify();
    }));
});