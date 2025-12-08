import IPark from "./park";

export interface CartItem {
    park: IPark;
    numDays: string;
    numAdults: number;
    numKids: number;
}