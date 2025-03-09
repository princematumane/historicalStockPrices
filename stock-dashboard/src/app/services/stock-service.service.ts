// src/app/services/stock.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../environments/environment';
import { Stock } from '../models/stock.model';

@Injectable()
export class StockService {
  private apiUrl = `${environment.apiUrl}/Stock`;

  constructor(private http: HttpClient) {}

  getStockData(symbol: string): Observable<Stock> {
    return this.http.get<Stock>(`${this.apiUrl}/${symbol}`);
  }
}
