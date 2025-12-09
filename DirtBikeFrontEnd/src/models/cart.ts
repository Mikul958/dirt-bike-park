import { BookingResponse } from "./booking";

export interface Cart
{
    id: string;
    taxRate: number;
    bookings: BookingResponse[];
    totalPrice: number;
}