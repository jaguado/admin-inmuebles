import { Menu, Condo } from '../shared/models';
export const DefaultMenu: Menu[] = [
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
    'enabled': true
  },
  {
    'icon': 'table_chart',
    'label': 'Tables',
    'link': '/tables',
    'enabled': true
  },
  {
    'icon': 'input',
    'label': 'Forms',
    'link': '/forms',
    'enabled': true
  },
  {
    'icon': 'grid_on',
    'label': 'Grid',
    'link': '/grid',
    'enabled': true
  },
  {
    'icon': 'code',
    'label': 'Components',
    'link': '/components',
    'enabled': true
  },
  {
    'icon': 'insert_drive_file',
    'label': 'Blank page',
    'link': '/blank-page',
    'enabled': true
  }
];

export const DefaultCondos: Condo[] = [
  {
    'id': 1,
    'name': 'Condominio Principal',
    'menu': DefaultMenu,
    'enabled': true,
    'properties': [
      {
        'id': 1,
        'alias': 'Departamento',
        'icon': '/assets/images/icons/apartment.svg',
        'chartLabels': ['2006', '2007', '2008', '2009', '2010', '2011', '2012'],
        'chartData': [
          { data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A' },
          { data: [28, 48, 40, 19, 86, 27, 90], label: 'Series B' }
        ]
      },
      {
        'id': 2,
        'alias': 'Casa',
        'icon': '/assets/images/icons/small-house.svg',
        'chartLabels': ['2006', '2007', '2008', '2009', '2010', '2011', '2012'],
        'chartData': [
          { data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A' },
          { data: [28, 48, 40, 19, 86, 27, 90], label: 'Series B' }
        ]
      },
      {
        'id': 3,
        'alias': 'Minimarket',
        'icon': '/assets/images/icons/shop-house.svg',
        'chartLabels': ['2006', '2007', '2008', '2009', '2010', '2011', '2012'],
        'chartData': [
          { data: [65, 59, 80, 81, 56, 55, 40], label: 'Series A' },
          { data: [28, 48, 40, 19, 86, 27, 90], label: 'Series B' }
        ]
      }
    ]
  },
  {
    'id': 2,
    'name': 'Edificio Vacaciones',
    'menu': DefaultMenu,
    'enabled': false,
    'properties': []
  }
];
