import { SocialUser } from '../auth.service';
export class User implements SocialUser {
    // SocialUser fields
    id: string;
    provider: string;
    email: string;
    name: string;
    photoUrl: string;
    firstName: string;
    lastName: string;
    authToken: string;
    idToken: string;
    authorizationCode: string;
    facebook?: any;
    linkedIn?: any;
    // Other fields
    state: any;
    data: any;
  }
