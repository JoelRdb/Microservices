import { AsyncPipe, CurrencyPipe, NgForOf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MyOrderService } from './myorder.service';
import { IOrder } from '../shared/models/order';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-myorder',
  imports: [
    CurrencyPipe,
    NgForOf,
    AsyncPipe
],
  templateUrl: './myorder.component.html',
  styleUrl: './myorder.component.scss'
})
export class MyorderComponent implements OnInit {

  listOrder$!: Observable<IOrder[]>;

  constructor(private orderService : MyOrderService) {}

  ngOnInit(): void {
    this.listOrder$ = this.getOrders();
  }


  private getOrders() : Observable<IOrder[]>{
    return this.orderService.getOrders();
  }

}
