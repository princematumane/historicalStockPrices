// src/app/models/stock.model.ts
export interface Stock {
  symbol: string;
  prices: StockPrice[];
}

export interface StockPrice {
  date: Date;
  open: number;
  high: number;
  low: number;
  close: number;
  volume: number;
}
