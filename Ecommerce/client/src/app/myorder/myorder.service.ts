import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { IOrder } from "../shared/models/order";
import { map, Observable, tap } from "rxjs";
import { response } from "express";

@Injectable({
    providedIn: 'root'
})
export class MyOrderService{

    constructor(private httpClient : HttpClient) {
        
    }

    getOrders() : Observable<IOrder[]>{
        return this.httpClient.get<IOrder[]>('https://id-local.eshopping.com:44344/Order/Rocky');
    }
}