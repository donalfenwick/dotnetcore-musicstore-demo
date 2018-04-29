import { User, SigninRequest, StateStore, SignoutRequest, UserManagerEvents } from "oidc-client";

export class MockUserManager {

    private user: User = <User>{
        expired:false, 
        access_token: 'sampleAccessTokenValue',
        id_token: 'sampleIdTokenValue',
        token_type: '',
        state: '',
        expires_at: 0,
        expires_in: 0,
        profile: {},
        scope: '',
        scopes: [],
        session_state: {}
    };

    getUser(): Promise<User>{
        return new Promise<User>((res,reject) => { res(this.user) });
    }

    clearStaleState(): Promise<void> { return new Promise<void>((r) => { r(); });}

  storeUser(user:User): Promise<void> { return new Promise<void>((r) => { r(); });}
  removeUser(): Promise<void> { return new Promise<void>((r) => { r(); });}

  signinPopup(args?: any): Promise<User>{
    return new Promise<User>((resolve,reject) => { resolve(this.user) });
}
  signinPopupCallback(url?: string): Promise<any>{ return new Promise<any>((r) => { r(); });}

  signinSilent(args?: any): Promise<User>{
    return new Promise<User>((resolve,reject) => { resolve(this.user) });
}
  signinSilentCallback(url?: string): Promise<any>{ return new Promise<any>((r) => { r(); });}

  signinRedirect(args?: any): Promise<any>{ return new Promise<any>((r) => { r(); });}
  signinRedirectCallback(url?: string): Promise<User>{
    return new Promise<User>((res,reject) => { res(this.user) });
}

  signoutRedirect(args?: any): Promise<any>{ return new Promise<any>((r) => { r(); });}
  signoutRedirectCallback(url?: string): Promise<any>{ return new Promise<any>((r) => { r(); });}

  signoutPopup(args?: any): Promise<any>{ return new Promise<any>((r) => { r({}); });}
  signoutPopupCallback(url?: any, keepOpen?: any): Promise<void>{ return new Promise<void>((r) => { r(); });}
  //signoutPopupCallback(keepOpen?: boolean): Promise<void> { return new Promise<void>((r) => { r(); });}

  querySessionStatus(args?: any): Promise<any>{ return new Promise<any>((r) => { r(); });}

  revokeAccessToken(): Promise<void> { return new Promise<void>((r) => { r(); });}

  startSilentRenew(): void{}
  stopSilentRenew(): void{}
    events:UserManagerEvents = <UserManagerEvents>{
        addSilentRenewError(callback:Function): void{ }
    }
    
    
  createSigninRequest(args?: any): Promise<SigninRequest> { return new Promise<SigninRequest>((r) => { r(<SigninRequest>{}); });}
  processSigninResponse(): Promise<any>{ return new Promise<any>((r) => { r({}); });}

  createSignoutRequest(args?: any): Promise<SignoutRequest>{ return new Promise<SignoutRequest>((r) => { r(<SignoutRequest>{}); });}
  processSignoutResponse():  Promise<any>{ return new Promise<any>((r) => { r({}); });}

  //clearStaleState(stateStore: StateStore): Promise<any>{ return new Promise<any>((r) => { r({}); });}
}