import { Component, Input, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { Stock } from '../models/stock.model';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { CommonModule, DatePipe } from '@angular/common';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-stock-chart',
  imports: [CommonModule, MatCardModule, NgxChartsModule],
  templateUrl: './stock-chart.component.html',
  styleUrl: './stock-chart.component.scss',
})
export class StockChartComponent {
  @Input() stock!: Stock;

  chartData: any[] = [];
  filteredChartData: any[] = [];
  view: [number, number] = [750, 400];
  showXAxis = true;
  showYAxis = true;
  gradient = true;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'Date';
  showYAxisLabel = true;
  yAxisLabel = 'Price';
  autoScale = true;

  ngOnChanges(): void {
    const datePipe = new DatePipe('en-US');

    this.chartData = [
      {
        name: 'Open',
        series: this.stock.prices.map((p) => ({
          name: datePipe.transform(p.date, 'yyyy-MM-dd'),
          value: p.open,
        })),
      },
      {
        name: 'High',
        series: this.stock.prices.map((p) => ({
          name: datePipe.transform(p.date, 'yyyy-MM-dd'),
          value: p.high,
        })),
      },
      {
        name: 'Low',
        series: this.stock.prices.map((p) => ({
          name: datePipe.transform(p.date, 'yyyy-MM-dd'),
          value: p.low,
        })),
      },
      {
        name: 'Close',
        series: this.stock.prices.map((p) => ({
          name: datePipe.transform(p.date, 'yyyy-MM-dd'),
          value: p.close,
        })),
      },
      // {
      //   name: 'Volume',
      //   series: this.stock.prices.map((p) => ({
      //     name: datePipe.transform(p.date, 'yyyy-MM-dd'),
      //     value: p.volume,
      //   })),
      // },
    ];

    this.filteredChartData = [...this.chartData];
  }

  onLegendLabelClick(legendItem: any): void {
    this.filteredChartData = this.chartData.map((series) => ({
      ...series,
      visible: series.name === legendItem ? !series.visible : series.visible,
    }));

    if (this.filteredChartData.every((series) => !series.visible)) {
      this.filteredChartData = this.chartData.map((series) => ({
        ...series,
        visible: true,
      }));
    }
  }
}
