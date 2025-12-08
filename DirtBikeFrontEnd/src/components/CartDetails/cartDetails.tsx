import CartService from "../../services/cartService";
import CartCard from "../cartCard/cartCard";
import { Booking } from '../../models/booking';
import { useState } from "react";
import "./cartDetails.css"
import PaymentDetails from "../PaymentDetails/PaymentDetails";

type CartDetailsProps = {
	cartService: CartService
    handleDelete: () => void
}

export default function CartDetails(props: CartDetailsProps) {
	const { cartService, handleDelete } = props;

    //Pulling from local storage as source of truth
    const [cart, setCart] = useState(cartService.loadCart());
    const [paymentOption, setPaymentOption] = useState("PAY_AT_PARK");
	
    const updateCartItem = (newBooking: Booking) => {
        const item = cart.find((item: Booking) => item.Park.Id === newBooking.Park.Id);
        cartService.updateCart(item, newBooking);
        setCart(cartService.loadCart());
    }

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setPaymentOption(e.target.value);
    }

    const deleteCartItem = (booking: Booking) => {
        cartService.removeItemFromCart(booking);
        handleDelete();
        setCart(cartService.loadCart());
    }

    const getTaxPrice = () => {
        return cart.reduce((acc, curr) => {
            const {NumAdults, NumChildren, Park} = curr;
            return (
                acc + 
                    ((NumAdults * 1 * Park.PricePerAdult) + 
                    (NumChildren * 1 * Park.PricePerChild)) * 0.08
        )
        }, 0)
    }

    const getTotalPrice = () => {
        return cart.reduce((acc, curr) => {
            const {NumAdults, NumChildren, Park} = curr;
            return (
                acc + 
                    ((NumAdults * 1 * Park.PricePerAdult) + 
                    (NumChildren * 1 * Park.PricePerChild)) * 1.08 
        )
        }, 0)
    }
	
    return(
        <div>
            <div className="cartItems column">
                {cart.map(((booking: Booking) => <CartCard booking={booking} updateFn={(e) => updateCartItem(e)} deleteFn={deleteCartItem} />))}      
            </div>
            <div>
                Tax: ${getTaxPrice().toFixed(2)}
            </div>
            <div>
                Total Price: ${getTotalPrice().toFixed(2)}
            </div>
            <label>
                How would you like to pay?
                <input type="radio" name="selectedPayment" value={"PAY_AT_PARK"} checked={paymentOption === "PAY_AT_PARK"} onChange={handleChange} /> Pay Later at the Park
                <input type="radio" name="selectedPayment" value={"PAY_NOW"} checked={paymentOption === "PAY_NOW"} onChange={handleChange} /> Pay Now
            </label>
            {
                paymentOption === "PAY_NOW" && <PaymentDetails />
            }
        </div>
    )
}