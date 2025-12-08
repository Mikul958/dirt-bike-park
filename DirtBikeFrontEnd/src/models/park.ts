import Review from "./review";

// You can use whatever data type for ID as long as it's guaranteed unique
// Here we use GUIDs because they're almost guaranteed unique
// and any other id system is abnormal and honestly worse in my opinion
export default interface Park
{
    Id: number;
    Name: string;
    Location: string;
    Description: string;
    ImageUrl?: string;
    PricePerAdult: number;
    PricePerChild: number;
    GuestLimit: number;
    Reviews: Review[];
}