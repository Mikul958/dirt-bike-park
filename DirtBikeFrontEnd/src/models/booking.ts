import Park from "./park";

export interface Booking
{
    id: number;
    cartId: string;
    parkId: number;
    park: Park;
    date: string;
    numAdults: number;
    numChildren: number;
    totalPrice: number;
}