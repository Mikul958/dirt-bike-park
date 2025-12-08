import { Booking } from "./booking";

export interface Cart
{
    Id: string;
    TaxRate: number;
    Bookings: Booking[];
    TotalPrice: number;
}