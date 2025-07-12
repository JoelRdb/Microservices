import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'
})
export class StoreService {

  constructor(private httpClient : HttpClient) { }

    baseUrl = 'https://localhost:8010/';

    getProducts() {
      return this.httpClient.get<IPagination<IProduct[]>>(this.baseUrl + 'Catalog/GetAllProducts');
    }
}
