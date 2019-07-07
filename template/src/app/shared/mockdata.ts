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

export const DefaultProperties: any[] = [
  {
    'id': 1,
    'alias': 'Departamento',
    'icon': '/assets/images/icons/apartment.svg',
    'chartLabels': ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio'],
    'chartData': [
      { data: [50000, 79000, 80000, 81000, 56000, 55000, 40000], label: 'Deuda' },
      { data: [28000, 79000, 80000, 50000, 56000, 55000, 0], label: 'Pago' }
    ]
  },
  {
    'id': 2,
    'alias': 'Casa',
    'icon': '/assets/images/icons/small-house.svg',
    'chartLabels': ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio'],
    'chartData': [
      { data: [50000, 79000, 80000, 81000, 56000, 55000, 40000], label: 'Deuda' },
      { data: [58000, 59000, 40000, 100000, 56000, 55000, 0], label: 'Pago' }
    ]
  },
  {
    'id': 3,
    'alias': 'Minimarket',
    'icon': '/assets/images/icons/shop-house.svg',
    'chartLabels': ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio'],
    'chartData': [
      { data: [200000, 79000, 80000, 81000, 56000, 55000, 400000], label: 'Deuda' },
      { data: [280000, 79000, 800000, 50000, 56000, 55000, 500000], label: 'Pago' }
    ]
  }
];

export const DefaultCondos: Condo[] = [
  {
    'id': 1,
    'name': 'Condominio Principal',
    'menu': DefaultMenu,
    'enabled': true,
    'properties': DefaultProperties
  },
  {
    'id': 2,
    'name': 'Edificio Vacaciones',
    'menu': DefaultMenu,
    'enabled': false,
    'properties': DefaultProperties
  }
];
