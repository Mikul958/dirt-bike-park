import { Booking } from "../models/booking";

export default class CartService {
    private items: Booking[];

    private CART_KEY = 'rideFinderExampleApp'


    //loadCart will be our public facing method, all invocations of getCart should be internal so we only have one source of truth

    loadCart = (): Booking[] => {
        return JSON.parse(localStorage.getItem(this.CART_KEY));
    }

    addItemToCart = (newBooking: Booking) => {
        const cart = this.loadCart() || [];
        const bookingInCart = cart.findIndex((booking: Booking) => booking.Park.Id === newBooking.Park.Id);
        if(bookingInCart > -1) {
            this.updateCart(cart[bookingInCart], newBooking);
        }
        cart.push(newBooking);
        this.save(cart);
    }

    removeItemFromCart = (booking: Booking) => {
        const cart = this.loadCart();
        
        const result = cart.filter((val: Booking) => val.Park.Id !== booking.Park.Id)
        this.save(result);
    }

    updateCart(oldBooking: Booking, newBooking: Booking) {
        const cart = this.loadCart();
        if(oldBooking.Park.Id !== newBooking.Park.Id) {
            this.save(cart);
        }

        //Update with new item first and then old item if it doesn't exist
        const combinedItem = {
            Id: newBooking.Id || oldBooking.Id,
            CartId: newBooking.CartId || oldBooking.CartId,
            ParkId: newBooking.ParkId || oldBooking.ParkId,
            Park: newBooking.Park || oldBooking.Park,
            Date: newBooking.Date || oldBooking.Date,
            NumAdults: newBooking.NumAdults || oldBooking.NumAdults,
            NumChildren: newBooking.NumChildren || oldBooking.NumChildren,
            TotalPrice: newBooking.TotalPrice || oldBooking.TotalPrice
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