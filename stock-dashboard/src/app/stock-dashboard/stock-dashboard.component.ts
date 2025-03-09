import {
  ChangeDetectionStrategy,
  Component,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { NgxSkeletonLoaderModule } from 'ngx-skeleton-loader';
import { StockService } from '../services/stock-service.service';
import { Stock } from '../models/stock.model';
import { StockChartComponent } from '../stock-chart/stock-chart.component';
import { StockTableComponent } from '../stock-table/stock-table.component';
import { CommonModule } from '@angular/common';
import { FormControl, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import {
  BehaviorSubject,
  Observable,
  startWith,
  Subject,
  switchMap,
  takeUntil,
} from 'rxjs';

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
    MatProgressSpinnerModule,
    StockChartComponent,
    StockTableComponent,
    NgxSkeletonLoaderModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [StockService],
  templateUrl: './stock-dashboard.component.html',
  styleUrl: './stock-dashboard.component.scss',
})
export class StockDashboardComponent implements OnInit, OnDestroy {
  stock$!: Observable<Stock>;

  stockControl = new FormControl('AAPL');
  private destroy$ = new Subject<void>();

  loading$ = new BehaviorSubject<boolean>(false);

  constructor(private stockService: StockService) {}

  ngOnInit(): void {
    this.stock$ = this.stockControl.valueChanges.pipe(
      startWith('AAPL'),
      switchMap((symbol) => {
        this.loading$.next(true);
        return this.fetchStockData(symbol ?? 'AAPL');
      }),
      switchMap((stock) => {
        this.loading$.next(false);
        return [stock];
      }),
      takeUntil(this.destroy$)
    );
  }

  fetchStockData(symbol: string): Observable<Stock> {
    return this.stockService.getStockData(symbol);
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
