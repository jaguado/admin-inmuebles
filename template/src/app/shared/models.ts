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
  rut: any;
  dummyData: boolean;
}

export class Property {
  id: number;
  icon?: string;
  alias?: string;
  location?: string;
  zone?: string;
}

export class Condo {
  properties: Property[];
  id: number;
  name: string;
  menu: Menu[];
  enabled: boolean;
}

export class Menu {
  icon: string;
  label: string;
  link: string;
  enabled: boolean;
}
