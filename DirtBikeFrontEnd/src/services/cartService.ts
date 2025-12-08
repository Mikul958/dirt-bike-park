import { Booking } from "../models/booking";
import { Cart } from "../models/cart";

export default class CartService
{
    private readonly CART_URL_BASE: string = "https://localhost:7226/api/cart/"
    private readonly BOOKING_URL_BASE: string = "https://localhost:7226/api/booking/"

    private readonly CART_KEY: string = "CART_KEY";
    private cartId: string | null = null;
    private cart: Cart | null = null;

    constructor() {
        const storedCartId = localStorage.getItem(this.CART_KEY);
        if (storedCartId)
            this.cartId = storedCartId;
    }

    getCart = (): Cart | null => {
        return this.cart;
    }

    loadCart = async (): Promise<Cart> => {
        const res = await fetch(this.CART_URL_BASE + this.cartId)
        if (!res.ok)
            throw new Error("Failed to fetch cart: " + res.text());

        this.cart = await res.json() as Cart;
        if (this.cartId == "") {
            this.cartId = this.cart.Id;
            localStorage.setItem(this.CART_KEY, this.cart.Id);
        }
        return this.cart;
    }

    addBookingToCart = async (booking: Booking) => {
        // Attempt to create booking and retrieve created booking from response
        const bookingUrl = new URL(this.BOOKING_URL_BASE + "park/" + booking.ParkId)
        const bookingRes = await fetch(bookingUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(booking)
        });
        if (!bookingRes.ok)
            throw new Error("Failed to create booking: " + (await bookingRes.text()))
        const createdBooking: Booking = (await bookingRes.json()) as unknown as Booking;
        
        // Attempt to add created booking to the cart
        const cartUrl = new URL(this.CART_URL_BASE + this.cartId + "/add")
        cartUrl.searchParams.append("parkId", createdBooking.ParkId.toString())
        cartUrl.searchParams.append("bookingId", createdBooking.Id.toString())
        const cartRes = await fetch(cartUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            }
        });
        if (!cartRes.ok)
            throw new Error("Failed to add booking to cart: " + (await cartRes.text()));
    }

    removeItemFromCart = async (bookingId: number) => {
        const url = new URL(this.CART_URL_BASE + this.cartId + "/remove")
        url.searchParams.append("bookingId", bookingId.toString())
        
        const res = await fetch(url);
        if (!res.ok)
            throw new Error("Failed to remove booking from cart");
    }

    updateCart(oldBooking: Booking, newBooking: Booking) {
        const combinedBooking = {
            Id: newBooking.Id || oldBooking.Id,
            CartId: newBooking.CartId || oldBooking.CartId,
            ParkId: newBooking.ParkId || oldBooking.ParkId,
            Park: newBooking.Park || oldBooking.Park,
            Date: newBooking.Date || oldBooking.Date,
            NumAdults: newBooking.NumAdults || oldBooking.NumAdults,
            NumChildren: newBooking.NumChildren || oldBooking.NumChildren,
            TotalPrice: newBooking.TotalPrice || oldBooking.TotalPrice
        };
        const index = this.cart.Bookings.findIndex((val: Booking) => val.Park.Id === combinedBooking.Park.Id);
        if(index > -1) {
            this.cart.Bookings[index] = combinedBooking;
        }
    }
}