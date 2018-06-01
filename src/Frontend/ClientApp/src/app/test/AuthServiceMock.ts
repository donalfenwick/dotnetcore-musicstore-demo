import { UserManager } from "oidc-client";

export class AuthServiceMock{ 
    constructor(public manager: UserManager){
        
    }
      initAuth(): Promise<any>{return new Promise((r)=>{ r(); })}

      getOidcConfig(): Promise<any>{return Promise.resolve({}); }
      
      isLoggedIn(): boolean {
        return false;
      }

      getClaims(): any {
        return {};
      }

      getAccessToken(): string {
        return 'tokenvalue';
      }

      getAuthorizationHeaderValue(): string {
        return 'Bearer tokenvalue';
      }

      startAuthentication(): Promise<void> {
        return new Promise( (r, e) => { r(); });
      }
    
      startSignout(): Promise<void>{
        return new Promise( (r, e) => { r(); });
      }
    
      completeAuthentication(): Promise<void> {
        return new Promise( (r, e) => { r(); });
      }
 }