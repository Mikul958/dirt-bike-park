import { useState } from "react";
import Park from "../../models/park";
import './bookRide.css';
import cartService from "../../services/cartService";

interface bookRideProps {
    park: Park
    cartService: cartService
    onBook: () => void
}

export default function BookRide(props: bookRideProps) {
    const { park, cartService, onBook } = props;

    const [numAdults, setNumAdults] = useState(0);
    const [numChildren, setNumKids] = useState(0);
    const [dateTime, setDateTime] = useState("");
	
	const submitForm = () => {
        cartService.addBookingToCart({ id: 0, cartId: "0", parkId: park.id, park: park, numAdults: numAdults, numChildren: numChildren, date: dateTime, totalPrice: 0 })
        onBook();
    }

    const getTotalPrice = () => {
        const totalPrice = ((numAdults * park.pricePerAdult) + (numChildren * park.pricePerChild));
        return (isNaN(totalPrice) ? 0 : totalPrice)
    }

    return (park && 
        <div className="bookRide book-container">
            <h2>Book Your Ride</h2>
            <div className="price-container">
                <div className="price adult-price">
                    <label htmlFor="numAdults"><b>Adults (${park.pricePerAdult || 0}/day)</b></label>
                    <input className="input-field" type="number" min={0} value={numAdults} placeholder="Enter number of adults" onChange={e => setNumAdults(Number.parseInt(e.target.value))} />
                </div>
                <div className="price kid-price">
                    <label htmlFor="numKids"><b>Kids (${park.pricePerChild || 0}/day)</b></label>
                    <input className="input-field" type="number" min={0} value={numChildren} placeholder="Enter number of kids..." onChange={e => setNumKids(Number.parseInt(e.target.value))} />
                </div>
                <div className="days days-datetime">
                    <label htmlFor="dateTime"><b>Date</b></label>
                    <input className="input-field" type="date" id="dateTime" value={dateTime} onChange={e => setDateTime(e.target.value)} />
                </div>
            </div>
            <hr />
            <div className="total-price"><b>Total: {getTotalPrice()}</b></div>
            <button onClick={e => submitForm()} className="button-add-to-cart" disabled={numAdults === 0}>Add to Cart</button>
        </div>)
}