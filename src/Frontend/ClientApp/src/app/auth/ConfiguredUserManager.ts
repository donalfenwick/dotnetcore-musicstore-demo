import { UserManager } from "oidc-client";
import { Injectable } from "@angular/core";

export class ConfiguredUserManager extends UserManager{
    constructor(){
        super({
            authority: 'http://localhost:5601/',
            client_id: 'musicStoreAngularFrotend',
            redirect_uri: 'http://localhost:5600/auth-callback',
            post_logout_redirect_uri: 'http://localhost:5600/',
            response_type:"id_token token",
            scope:"openid profile apiAccess email roles",
            filterProtocolClaims: true,
            loadUserInfo: true,
            automaticSilentRenew: true,
            silent_redirect_uri: 'http://localhost:5600/assets/silent-refresh.html'
          });
    }
}