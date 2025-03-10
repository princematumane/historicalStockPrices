# Stock Price Dashboard

## Overview

The Stock Price Dashboard is a web application that displays historical stock prices for selected companies. It provides a user-friendly interface to visualize daily stock data (Open, High, Low, Close, Volume) using interactive charts and tables. The application is built using Angular for the frontend and ASP.NET Core for the backend.

## Features

### Stock Data Visualization:

- Display daily stock prices for companies like AAPL, MSFT, GOOGL, AMZN, and FB.
- Interactive line chart to visualize Open, High, Low, Close, and Volume data.
- Toggle visibility of specific data series by clicking on the legend.

### Data Table:

- Display stock prices in a tabular format with sorting and pagination.

### Responsive Design:

- The application is fully responsive and works seamlessly on different screen sizes.

### Caching:

- Uses Redis to cache stock data and minimize API calls.

### Security:

- Implements CORS to allow cross-origin requests from the Angular frontend.
- Uses secure API endpoints.

### User-Friendly UI:

- Clean and intuitive interface built with Angular Material.
- Customizable chart colors and styles.

## Technologies Used

### Frontend

- **Angular**: Frontend framework for building the user interface.
- **Angular Material**: UI component library for a modern and responsive design.
- **NgxCharts**: Charting library for interactive and customizable charts.
- **RxJS**: For handling asynchronous data streams.

### Backend

- **ASP.NET Core**: Backend framework for building RESTful APIs.
- **Redis**: In-memory data store for caching stock data.
- **HttpClient**: For making API calls to external stock data providers (e.g., Alpha Vantage).

### Tools

- **Visual Studio Code**: For frontend development.
- **Visual Studio**: For backend development.
- **Postman**: For testing API endpoints.
- **Git**: For version control.

## Getting Started

### Prerequisites

- **Node.js**: Install Node.js from [nodejs.org](https://nodejs.org/).
- **Angular CLI**: Install Angular CLI globally using:
  ```bash
  npm install -g @angular/cli
  ```
- **.NET SDK**: Install the .NET SDK from [dotnet.microsoft.com](https://dotnet.microsoft.com/).
- **Redis**: Install Redis from [redis.io](https://redis.io/).

## Installation

- . Clone the repository:
  ```bash
  git clone https://github.com/princematumane/historicalStockPrices.git
  ```

### Frontend (Angular)

1. Goto frontend folder
   ```
       cd historicalStockPrices/stock-dashboard
   ```
2. Install dependencies:
   ```bash
   npm install
   ```
3. Start the Angular development server:
   ```bash
   ng serve
   ```
4. Open the application in your browser:
   ```
   http://localhost:4200
   ```

### Backend (ASP.NET Core)

1. Navigate to the backend directory:
   ```bash
   cd historicalStockPrices/StockPricesDashboardAPI
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Update appsettings based on your config
   ```
   "AlphaVantage": {
    "ApiKey": "{{yourApiKey}}",
    "BaseUrl": "{{baseUrl}}",
    "Host": "{{hostName}}"
   },
   "RedisSettings": {
    "ConnectionString": "{{rediUrl}}"
   }
   ```
4. Start the backend server:
   ```bash
   dotnet run
   ```
5. The backend API will be available at:
   ```
   https://localhost:7298
   ```

### Docker (Redis)

1. Download docker, Then run

```bash
   docker run --name my-redis -p 6379:6379 -d redis
```
