import { BookingInput, BookingResponse } from "../models/booking";
import { Cart } from "../models/cart";

export default class CartService
{
    private readonly CART_URL_BASE: string = "https://localhost:7226/api/cart/"
    private readonly BOOKING_URL_BASE: string = "https://localhost:7226/api/booking/"

    private readonly CART_KEY: string = "CART_KEY";
    private cartId: string = "";
    private cart: Cart = {
        id: "",
        taxRate: 0,
        bookings: [],
        totalPrice: 0
    }

    public signal: number = 0;

    loadCart = async (): Promise<Cart> => {
        const storedCartId = localStorage.getItem(this.CART_KEY);
        if (!(storedCartId === null || storedCartId === "null" || storedCartId === undefined || storedCartId === "undefined"))  // Javascript is a terrible language dude
            this.cartId = storedCartId;
        
        console.log(storedCartId);
        console.log(this.cartId);
        console.log(this.CART_URL_BASE + this.cartId);
        const res = await fetch(this.CART_URL_BASE + this.cartId, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        })
        if (!res.ok)
            throw new Error("Failed to fetch cart: " + await res.text());

        this.cart = await res.json() as Cart;
        if (this.cartId === "") {
            this.cartId = this.cart.id;
            localStorage.setItem(this.CART_KEY, this.cart.id);
        }

        console.log(this.cart);

        this.signal++;
        return this.cart;
    }

    getCart = (): Cart | null => {
        return this.cart;
    }

    addBookingToCart = async (parkId: number, booking: BookingInput) => {
        // Attempt to create booking and retrieve created booking from response
        const bookingUrl = new URL(this.BOOKING_URL_BASE + "park/" + parkId)
        const bookingRes = await fetch(bookingUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(booking)
        });
        if (!bookingRes.ok)
            throw new Error("Failed to create booking: " + (await bookingRes.text()))
        const createdBooking: BookingResponse = (await bookingRes.json()) as BookingResponse;
        
        // Attempt to add created booking to the cart
        const cartUrl = new URL(this.CART_URL_BASE + this.cartId + "/add")
        cartUrl.searchParams.append("parkId", createdBooking.park.id.toString())
        cartUrl.searchParams.append("bookingId", createdBooking.id.toString())
        const cartRes = await fetch(cartUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            }
        });
        if (!cartRes.ok)
            throw new Error("Failed to add booking to cart: " + await cartRes.text());
        this.signal++;
    }

    removeItemFromCart = async (bookingId: number) => {
        const url = new URL(this.CART_URL_BASE + this.cartId + "/remove")
        url.searchParams.append("bookingId", bookingId.toString())
        
        const res = await fetch(url, {
            method: "PUT"
        });
        if (!res.ok)
            throw new Error("Failed to remove booking from cart: " + await res.text());
        this.signal++;
    }

    updateCart(oldBooking: BookingResponse, newBooking: BookingResponse) {
        const combinedBooking = {
            id: newBooking.id || oldBooking.id,
            park: newBooking.park || oldBooking.park,
            date: newBooking.date || oldBooking.date,
            numAdults: newBooking.numAdults || oldBooking.numAdults,
            numChildren: newBooking.numChildren || oldBooking.numChildren,
            totalPrice: newBooking.totalPrice || oldBooking.totalPrice
        };
        const index = this.cart.bookings.findIndex((val: BookingResponse) => val.park.id === combinedBooking.park.id);
        if(index > -1) {
            this.cart.bookings[index] = combinedBooking;
        }
        this.signal++;
    }

    submitPayment = async (cardNumber: string, ccv: string, expDate: string, name: string) => {
        const url = new URL(this.CART_URL_BASE + this.cartId + "/payment");
        url.searchParams.append("cardNumber", cardNumber);
        url.searchParams.append("exp", expDate);
        url.searchParams.append("cardHolderName", name);
        url.searchParams.append("ccv", ccv);

        console.log("Fetching with: " + url);
        const res = await fetch(url, {
            method: "POST"
        });
        if (!res.ok)
            throw new Error("Failed to process payment: " + await res.text())
        this.signal++;
    }
}