import Park from "./park";

export interface Booking
{
    Id: number;
    CartId: string;
    ParkId: number;
    Park: Park;
    Date: string;
    NumAdults: number;
    NumChildren: number;
    TotalPrice: number;
}