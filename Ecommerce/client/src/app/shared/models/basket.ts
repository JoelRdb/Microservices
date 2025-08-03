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

