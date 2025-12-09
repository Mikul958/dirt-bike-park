import Review from "./review";

export default interface ParkInput
{
    name: string;
    location: string;
    description: string;
    imageUrl?: string;
    pricePerAdult: number;
    pricePerChild: number;
    guestLimit: number;
}

export default interface ParkResponse
{
    id: number;
    name: string;
    location: string;
    description: string;
    imageUrl?: string;
    pricePerAdult: number;
    pricePerChild: number;
    guestLimit: number;
    reviews: Review[];  // Mocked by service, do not exist in API
}