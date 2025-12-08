import { Booking } from "../models/booking";

export default class CartService {
    private items: Booking[];

    private CART_KEY = 'rideFinderExampleApp'


    //loadCart will be our public facing method, all invocations of getCart should be internal so we only have one source of truth

    loadCart = (): Booking[] => {
        return JSON.parse(localStorage.getItem(this.CART_KEY));
    }

    addItemToCart = (newItem: Booking) => {
        const cart = this.loadCart() || [];
        const itemInCart = cart.findIndex((item: Booking) => item.Park.Id === newItem.Park.Id);
        if(itemInCart > -1) {
            this.updateCart(cart[itemInCart], newItem);
        }
        cart.push(newItem);
        this.save(cart);
    }

    removeItemFromCart = (remItem: Booking) => {
        const cart = this.loadCart();
        
        const result = cart.filter((val: Booking) => val.Park.Id !== remItem.Park.Id)
        this.save(result);
    }

    updateCart(oldItem: Booking, newItem: Booking) {
        const cart = this.loadCart();
        if(oldItem.Park.Id !== newItem.Park.Id) {
            this.save(cart);
        }

        //Update with new item first and then old item if it doesn't exist
        const combinedItem = {
            Id: newItem.Id || oldItem.Id,
            CartId: newItem.CartId || oldItem.CartId,
            ParkId: newItem.ParkId || oldItem.ParkId,
            Park: newItem.Park || oldItem.Park,
            Date: newItem.Date || oldItem.Date,
            NumAdults: newItem.NumAdults || oldItem.NumAdults,
            NumChildren: newItem.NumChildren || oldItem.NumChildren,
            TotalPrice: newItem.TotalPrice || oldItem.TotalPrice
        };
        const index = cart.findIndex((val: Booking) => val.Park.Id === combinedItem.Park.Id);
        if(index > -1) {
            cart[index] = combinedItem;
        }
        this.save(cart);
    }

    private save(cart: Booking[]) {
        localStorage.setItem(this.CART_KEY, JSON.stringify(cart));
    }
}