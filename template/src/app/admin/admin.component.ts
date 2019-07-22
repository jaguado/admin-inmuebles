import { element } from 'protractor';
import { Observable } from 'rxjs';
import { Component, OnInit, ViewChild } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';  // Import it up here
import { environment } from '../../environments/environment';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

export interface Table {
  Position ?: number;
  BD: string;
  Nombre: string;
  Esquema: string;
}

export interface Columns {
  Position ?: number;
  Nombre: string;
  Tipo: string;
  Largo: number;
  Precision: number;
  IsIdentity ?: boolean;
}

export enum View {
  Tables = 1, Columns = 2, Data = 3, Edit = 4
}

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  dataSourceTables: MatTableDataSource<any> = new MatTableDataSource();
  dataSourceColumns: MatTableDataSource<any> = new MatTableDataSource();
  dataSourceData: MatTableDataSource<any> = new MatTableDataSource();
  places: Array<any> = [];
  currentView: View = null;
  selectedTable: Table = null;
  dataColumns: Columns[] = [];
  dataDisplayedColumns: string[] = [];
  filterValue = '';

  constructor(private http: HttpClient) { }

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ngOnInit() {
    this.currentView = View.Tables;
    this.loadTables();
  }

  view(table: Table) {
    console.log('view structure', table);
    this.selectedTable = table;
    this.currentView = View.Columns;
    this.loadColumns();
  }


  data(table: Table) {
    console.log('view data', table);
    this.selectedTable = table;
    this.currentView = View.Data;
    this.loadData();
  }

  edit(table: Table) {
    console.log('edit data', table);
    this.selectedTable = table;
    this.currentView = View.Edit;
    this.loadData();
  }

  goBack() {
    this.selectedTable = null;
    this.currentView = View.Tables;
  }

  loadTables() {
    this.dataSourceTables = null;
    this.http.get(environment.baseUrl + 'v1/GenericForms')
    .toPromise()
    .then((res: Table[]) => {
      if (res) {
        let int = 0;
        res.forEach(row => {
          row.Position = ++int;
        });
      }
      this.dataSourceTables = new MatTableDataSource(res);
      this.dataSourceTables.paginator = this.paginator;
      this.dataSourceTables.sort = this.sort;
    })
    .catch(err => {
      console.log('loadTables error', err);
    })
    .finally(() => {
    });
  }

  loadColumns() {
    this.dataSourceColumns = null;
    this.getColumns().then((res: Columns[]) => {
      if (res) {
        let int = 0;
        res.forEach(row => {
          row.Position = ++int;
          row.IsIdentity = row.Tipo.includes('identity');
        });
      }
      this.dataSourceColumns = new MatTableDataSource(res);
      this.dataSourceColumns.paginator = this.paginator;
      this.dataSourceColumns.sort = this.sort;
    })
    .catch(err => {
      console.log('loadColumns error', err);
    })
    .finally(() => {
    });
  }

  getColumns(): any {
    return this.http.get(environment.baseUrl + 'v1/GenericForms/' + this.selectedTable.Nombre)
               .toPromise();
  }

  loadData() {
    this.dataDisplayedColumns = [];
    this.dataSourceData = new MatTableDataSource();
    // load columns to identify fields types
    this.getColumns().then((res: Columns[]) => {
      console.log('loadData', 'getColumns', res);
      if (res) {
        let int = 0;
        res.forEach(row => {
          row.Position = ++int;
          row.IsIdentity = row.Tipo.includes('identity');
        });
        this.dataColumns = res;
        this.dataDisplayedColumns = !this.dataColumns ? [] : this.dataColumns.map(col => col.Nombre);
        if (this.currentView === View.Edit) {
          this.dataDisplayedColumns.push('update');
        }
      }
      this.dataSourceColumns = new MatTableDataSource(res);
      this.dataSourceColumns.paginator = this.paginator;
      this.dataSourceColumns.sort = this.sort;
    })
    .catch(err => {
      console.log('loadColumns error', err);
    })
    .finally(() => {
      // load data
      let result = [];
      this.http.get(environment.baseUrl + 'v1/GenericForms/' + this.selectedTable.Nombre + '/data')
          .toPromise()
          .then((res: any[]) => {
            if (res) {
              console.log('loadData', res);
              result = res;
            }
          })
          .catch(err => {
            if (err.status !== 404) {
              console.log('loadData error', err);
            } else {
              console.log('no data found on table', this.selectedTable.Nombre);
            }
          })
          .finally(() => {
              // add empty row to new fields creation
              if  (this.currentView === View.Edit) {
                const newEmptyObj = {};
                this.dataColumns.forEach((col: Columns) => {
                  newEmptyObj[col.Nombre] = null;
                });
                newEmptyObj['_IsNew'] = true;
                result.push(newEmptyObj);
              }
              this.dataSourceData = new MatTableDataSource(result);
              this.dataSourceData.paginator = this.paginator;
              this.dataSourceData.sort = this.sort;
          });
    });
  }

  applyFilter(filterValue: string) {
    try {
        if (!filterValue) {
          this.filterValue = '';
        }
        filterValue = filterValue.trim(); // Remove whitespace
        filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
        this.dataSourceData.filter = filterValue;
        if (this.dataSourceData.paginator) {
            this.dataSourceData.paginator.firstPage();
        }
        this.dataSourceColumns.filter = filterValue;
        if (this.dataSourceColumns.paginator) {
            this.dataSourceColumns.paginator.firstPage();
        }
        this.dataSourceTables.filter = filterValue;
        if (this.dataSourceTables.paginator) {
            this.dataSourceTables.paginator.firstPage();
        }
        this.filterValue = filterValue;
    } catch (ex) {
      console.log('applyFilterException', ex);
    }
  }

  updateOrCreate(row: any) {
    row._columns = this.dataColumns;
    console.log('updateOrCreate', row, row._IsNew);
    if (row._IsNew) {
      this.http.post(environment.baseUrl + 'v1/GenericForms/' + this.selectedTable.Nombre, row)
        .toPromise()
        .then((res: any[]) => {
          this.loadData();
        })
        .catch(err => {
          console.log('create error', err);
        })
        .finally(() => {
        });
    } else {
      this.http.put(environment.baseUrl + 'v1/GenericForms/' + this.selectedTable.Nombre, row)
          .toPromise()
          .then((res: any[]) => {
            this.loadData();
          })
          .catch(err => {
            console.log('update error', err);
          })
          .finally(() => {
          });
    }
  }

  delete(row: any) {
    row._columns = this.dataColumns;
    console.log('delete', row);
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
      body: row
    };
    this.http.delete(environment.baseUrl + 'v1/GenericForms/' + this.selectedTable.Nombre, options)
        .toPromise()
        .then(res => {
          this.loadData();
        })
        .catch(err => {
          console.log('delete error', err);
        })
        .finally(() => {
        });
  }
}
