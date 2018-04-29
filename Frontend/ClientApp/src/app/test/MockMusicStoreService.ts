import { Observable } from 'rxjs';

import { UserProfile } from '../models/usermodels';

export class MockMusicStoreService{
    constructor(){}
    
    getUserProfile(): Observable<UserProfile>{
        return Observable.of({ 
            username: '',
            firstname: '',
            surname: '',
            emailAddress: '',
            phoneNumber: '',
            dateOfBirth: ''
        })
    }

}