import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { StockService } from '../services/stock-service.service';
import { Stock } from '../models/stock.model';
import { StockChartComponent } from '../stock-chart/stock-chart.component';
import { StockTableComponent } from '../stock-table/stock-table.component';
import { CommonModule } from '@angular/common';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { Observable, ReplaySubject, startWith, switchMap } from 'rxjs';

@Component({
  selector: 'app-stock-dashboard',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatIconModule,
    MatTableModule,
    MatToolbarModule,
    MatSelectModule,
    MatButtonModule,
    StockChartComponent,
    StockTableComponent,
  ],
  templateUrl: './stock-dashboard.component.html',
  styleUrl: './stock-dashboard.component.scss',
})
export class StockDashboardComponent implements OnInit {
  stock$!: Observable<Stock>;

  stockControl = new FormControl('AAPL');

  constructor(private stockService: StockService) {}

  ngOnInit(): void {
    this.stock$ = this.stockControl.valueChanges.pipe(
      startWith('AAPL'),
      switchMap((x) => this.fetchStockData(x ?? ''))
    );
  }

  fetchStockData(symbol: string): Observable<Stock> {
    return this.stockService.getStockData(symbol);
  }
}
