import {  User } from 'oidc-client';
export class OidcUserStub implements User{
  id_token: string = 'testtoken';
  session_state: any = {};
  access_token: string = 'testaccesstoken';
  token_type: string = 'Bearer'
  scope: string = 'testscope';
  profile: any = {};
  expires_at: number = -1;
  state: any = {};
  toStorageString(){  return ''; }

  get expires_in() { return -1; }
  get expired() { return false; }
  get scopes() { return ['testscope'] }
}