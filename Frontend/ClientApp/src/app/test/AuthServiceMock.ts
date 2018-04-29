import { UserManager } from "oidc-client";
import { MockUserManager } from "./MockUserManager";

export class AuthServiceMock{ 
    constructor(public manager: UserManager){
        
    }

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