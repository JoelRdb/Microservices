import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/product';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/type';

@Injectable({
  providedIn: 'root'
})
export class StoreService {

  constructor(private httpClient : HttpClient) { }

    baseUrl = 'https://localhost:8010/';

    getProducts() {
      return this.httpClient.get<IPagination<IProduct[]>>(this.baseUrl + 'Catalog/GetAllProducts');
    }

    getBrands() {
      return this.httpClient.get<IBrand[]>(this.baseUrl + 'Catalog/GetAllBrands');
    }

    getTypes() {
      return this.httpClient.get<IType[]>(this.baseUrl + 'Catalog/GetAllTypes');
    }
}
