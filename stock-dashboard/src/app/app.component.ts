import { Component } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StockDashboardComponent } from './stock-dashboard/stock-dashboard.component';

@Component({
  selector: 'app-root',
  imports: [StockDashboardComponent],
  providers: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'stock-dashboard';
}
