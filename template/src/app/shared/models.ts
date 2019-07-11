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
  isAdmin: boolean;
}

export class Property {
  id: number;
  icon?: string;
  alias?: string;
  location?: string;
  zone?: string;
  chartLabels?: any[];
  chartData?: any[];
}

export class Condo {
  properties: Property[];
  id: number;
  name: string;
  menu: Menu[];
  enabled: boolean;
  roles: string[];
}

export class Menu {
  icon: string;
  label: string;
  link: string;
  enabled: boolean;
  requireAdminRole ? = false;
  childMenus ?: Menu[] = [];
}

export class Credentials {
  email: string;
  password: string;
}
