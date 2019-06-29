export const DefaultMenu: any = [
  {
    'icon': 'home',
    'label': 'Home',
    'link': '/home',
    'enabled': true
  },
  {
    'icon': 'dashboard',
    'label': 'Dashboard',
    'link': '/dashboard',
    'enabled': false
  },
  {
    'icon': 'bar_chart',
    'label': 'Charts',
    'link': '/charts',
    'enabled': false
  },
  {
    'icon': 'table_chart',
    'label': 'Tables',
    'link': '/tables',
    'enabled': false
  },
  {
    'icon': 'input',
    'label': 'Forms',
    'link': '/forms',
    'enabled': false
  },
  {
    'icon': 'grid_on',
    'label': 'Grid',
    'link': '/grid',
    'enabled': false
  },
  {
    'icon': 'code',
    'label': 'Components',
    'link': '/components',
    'enabled': false
  },
  {
    'icon': 'insert_drive_file',
    'label': 'Blank page',
    'link': '/blank-page',
    'enabled': false
  }
];

export const DefaultCondos: any = [
  {
    'id': 1,
    'name': 'Condominio Principal',
    'menu': DefaultMenu,
    'enabled': true
  },
  {
    'id': 2,
    'name': 'Edificio Vacaciones',
    'menu': DefaultMenu,
    'enabled': false
  }
];
