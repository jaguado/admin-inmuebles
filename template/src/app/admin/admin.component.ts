import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { HttpClient } from '@angular/common/http';  // Import it up here
import { AuthService } from '../auth.service';
import { environment } from '../../environments/environment';

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
}

export interface DataColumn {
  Name: string;
}

export enum View {
  Tables = 1, Columns = 2, Data = 3
}

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  dataSourceTables = null;
  dataSourceColumns = null;
  dataSourceData = null;
  places: Array<any> = [];
  currentView: View = null;
  selectedTable: Table = null;
  dataColumns: DataColumn[] = [];
  dataDisplayedColumns: string[] = [];

  constructor(private http: HttpClient) { }

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

  goBack(){
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
      console.log('loadTables', res);
      this.dataSourceTables = new MatTableDataSource(res);
    })
    .catch(err => {
      console.log('loadTables error', err);
    })
    .finally(() => {
    });
  }

  loadColumns() {
    this.dataSourceColumns = null;
    this.http.get(environment.baseUrl + 'v1/GenericForms/' + this.selectedTable.Nombre)
    .toPromise()
    .then((res: Columns[]) => {
      if (res) {
        let int = 0;
        res.forEach(row => {
          row.Position = ++int;
        });
      }
      console.log('loadColumns', res);
      this.dataSourceColumns = new MatTableDataSource(res);
    })
    .catch(err => {
      console.log('loadColumns error', err);
    })
    .finally(() => {
    });
  }

  loadData() {
    this.dataColumns = [];
    this.dataDisplayedColumns = [];
    this.dataSourceData = null;
    this.http.get(environment.baseUrl + 'v1/GenericForms/' + this.selectedTable.Nombre + '/data')
    .toPromise()
    .then((res: any[]) => {
      if (res) {
        if (res.length > 0) {
          const firstRow = res[0];
          console.log('loadData', 'firstRow', firstRow);
          Object.keys(firstRow).forEach(p => {
            this.dataColumns.push({ Name: p});
          });
          this.dataDisplayedColumns = this.dataColumns.map(column => column.Name);
        }
        console.log('loadData', res);
        this.dataSourceData = new MatTableDataSource(res);
      }
    })
    .catch(err => {
      if(err.status !== 404) {
        console.log('loadData error', err);
      } else {
        console.log('no data found on table', this.selectedTable.Nombre);
      }
    })
    .finally(() => {
    });
  }
}
