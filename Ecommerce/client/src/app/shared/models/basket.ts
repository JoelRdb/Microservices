export interface IBasketItem{
    productId: string;
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
    items: IBasketItem[] = [];
    totalPrice: number = 0;
}

export interface IBasketTotal{
    total: number;
}
