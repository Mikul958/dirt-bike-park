import ParkResponse from "./park";

export interface BookingInput
{
    date: string;
    numAdults: number;
    numChildren: number;
}

export interface BookingResponse
{
    id: number;
    park: ParkResponse;
    date: string;
    numAdults: number;
    numChildren: number;
    totalPrice: number;
}