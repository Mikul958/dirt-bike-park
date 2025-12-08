import Review from "./review";

// You can use whatever data type for ID as long as it's guaranteed unique
// Here we use GUIDs because they're almost guaranteed unique
// and any other id system is abnormal and honestly worse in my opinion
export default interface Park
{
    id: number;
    name: string;
    location: string;
    description: string;
    imageUrl?: string;
    pricePerAdult: number;
    pricePerChild: number;
    guestLimit: number;
    reviews: Review[];
}