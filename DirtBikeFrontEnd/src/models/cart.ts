import { Booking } from "./booking";

export interface Cart
{
    id: string;
    taxRate: number;
    bookings: Booking[];
    totalPrice: number;
}