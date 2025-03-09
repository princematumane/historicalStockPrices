import { AfterViewInit, Component, Input, ViewChild } from '@angular/core';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { Stock, StockPrice } from '../models/stock.model';
import { CommonModule } from '@angular/common';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-stock-table',
  imports: [CommonModule, MatTableModule, MatPaginatorModule],
  templateUrl: './stock-table.component.html',
  styleUrl: './stock-table.component.scss',
})
export class StockTableComponent implements AfterViewInit {
  @Input() set stock(value: Stock) {
    if (value) {
      this.dataSource = new MatTableDataSource(value.prices);
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    }
  }

  displayedColumns: string[] = [
    'date',
    'open',
    'high',
    'low',
    'close',
    'volume',
  ];
  dataSource!: MatTableDataSource<StockPrice>;

  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  ngAfterViewInit(): void {
    if (this.dataSource) {
      this.dataSource.sort = this.sort;
      this.dataSource.paginator = this.paginator;
    }
  }
}
