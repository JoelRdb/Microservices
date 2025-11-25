export interface IOrder {
  id: number;
  userName: string;
  totalPrice: number;
  firstName: string | null;
  lastName: string | null;
  emailAddress: string | null;
  addressLine: string | null;
  country: string | null;
  state: string | null;
  zipCode: string | null;
  cardName: string | null;
  cardNumber: string | null;
  expiration: string | null;
  cvv: string | null;
  paymentMethod: number | null;

}