export interface IBasketItem{
    productId: number;
    quantity: number;
    imageFile: string;
    productName: string;
    price: number;
}

export interface IBasket{
    userName: string;
    items: IBasketItem[];
    totalPrice: number;
}

export class Basket implements IBasket {
    userName: string = 'joel';
    totalPrice: number = 0;
    items: IBasketItem[] = [];
}

