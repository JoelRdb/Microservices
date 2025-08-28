import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Basket, IBasket, IBasketItem, IBasketTotal } from '../shared/models/basket';
import { BehaviorSubject } from 'rxjs';
import { IProduct } from '../shared/models/product';
import { AccountService } from '../account/account.service';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  // baseUrl = 'https://localhost:8010/';
  baseUrl = 'https://id-local.eshopping.com:44344/';

  constructor(private http: HttpClient, private acntService: AccountService, private router: Router) { }

  private basketSource = new BehaviorSubject<Basket | null>(null); //stockage interne de la dernière valeur du panier(IBasket), garde en mémoire la dernière valeur émise.
  basketSource$ = this.basketSource.asObservable(); // Version observable de basketSource, que le composants peuvent écouter pour etre notifiés quand le panier change :  Utiliser dans navbar.component pour l'affichage du nombre d'éléments d'un panier

  private basketTotal = new BehaviorSubject<IBasketTotal |null>(null);
  basketTotal$ = this.basketTotal.asObservable();

  getBasket(username: string){
    return this.http.get<IBasket>(this.baseUrl + 'Basket/GetBasket/joel').subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateBasketTotal();
      }
      });
  }

  
  setBasket(basket: IBasket) {
    return this.http.post<IBasket>(this.baseUrl + 'Basket/CreateBasket', basket).subscribe({
      next: basket => {
        this.basketSource.next(basket);
        this.calculateBasketTotal();
      }      
    });
  }


  checkoutBasket(basket: IBasket){
    const httpOptions = {
      headers: new HttpHeaders ({
        'Content-Type': 'application/json',
        'Authorization': this.acntService.authorizationHeaderValue
      })
    };
    return this.http.post<IBasket>(this.baseUrl + 'Basket/CheckoutV2', basket, httpOptions).subscribe({
      next: basket => {
        this.basketSource.next(null);
        this.router.navigateByUrl('/');
      }
    })
  }

  // Basket Operation start here
  addItemToBasket(item: IProduct, quantity = 1){
    const itemToAdd :IBasketItem = this.mapProductItemToBasketItem(item);
    const basket = this.getCurrentBasket() ?? this.createBasket();
    // now item can be added in the basket
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.setBasket(basket);
  }
  mapProductItemToBasketItem(item: IProduct) : IBasketItem {
    return {
      productId: item.id,
      quantity: 0,
      imageFile: item.imageFile,
      productName: item.name,
      price: item.price
    }
  }
  getCurrentBasket() {
    return this.basketSource.value;
  }
  createBasket(): Basket {
    const basket = new Basket();
    localStorage.setItem('basket_username', 'joel'); //TODO: joel can be replaced with LoggedIn User, we can see this in Inspection>Aoolicatin>Local storage
    return basket;
  } 
  addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const item = items.find(x => x.productId == itemToAdd.productId);
    if(item){
      item.quantity += quantity;
    }else{
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    }
    return items;
  }

  private calculateBasketTotal()
  {
    const basket = this.getCurrentBasket();
    if(!basket) return;
    //We are going to loop over in array and calculate total
    const total = basket.items.reduce((x, y)=> (y.price * y.quantity) + x, 0);
    this.basketTotal.next({total});
  }

  incrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    const foundItemIndex = basket.items.findIndex((x) => x.productId === item.productId);
    basket.items[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementItemQuantity(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    const foundItemIndex = basket.items.findIndex((x) => x.productId === item.productId);
    if(basket.items[foundItemIndex].quantity > 1){
      basket.items[foundItemIndex].quantity--;
      this.setBasket(basket);
    }else{
      this.removeItemFromBasket(item);
    }
  }
  removeItemFromBasket(item: IBasketItem){
    const basket = this.getCurrentBasket();
    if(!basket) return;
    if(basket.items.some((x) => x.productId === item.productId)){ // Vérifie si au moins un des éléments du panier possède l'Id 
      basket.items = basket.items.filter((x) => x.productId!== item.productId) // Exclure l'élément de notre panier
      if(basket.items.length > 0){ // S'il y a encore d'élement(s) dans le panier, alors mettre  à jour  le panier 
        this.setBasket(basket);
      }else{ // Si aucun élément dans le panier, alors supprimer définitivement le panier
        this.deleteBasket(basket.userName);
      }
    }
  }
  deleteBasket(userName: string) {
    return this.http.delete(this.baseUrl + 'Basket/DeleteBasket/' + userName).subscribe({
      next: (response) => {
        this.basketSource.next(null);
        this.basketTotal.next(null);
        localStorage.removeItem('basket_username');
      }, error: (err) => {
        console.log('Error Occured while deletin basket');
        console.log(err);
      }
    })
  }
  // Basket Operation end here
}

