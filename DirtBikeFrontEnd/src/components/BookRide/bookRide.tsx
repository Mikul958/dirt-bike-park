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
        cartService.addItemToCart({ Id: 0, CartId: "0", ParkId: park.Id, Park: park, NumAdults: numAdults, NumChildren: numChildren, Date: dateTime, TotalPrice: 0 })
        onBook();
    }

    // Removed unused totalPrice variable

    const getTotalPrice = () => {
        const totalPrice = ((numAdults * park.PricePerAdult) + (numChildren * park.PricePerChild));
        return `$${(isNaN(totalPrice) ? 0 : totalPrice).toFixed(2)}`;
    }
    return (park && 
        <div className="bookRide book-container">
            <h2>Book Your Ride</h2>
            <div className="price-container">
                <div className="price adult-price">
                    <label htmlFor="numAdults"><b>Adults (${park.PricePerAdult || 0}/day)</b></label>
                    <input className="input-field" type="number" min={0} value={numAdults} placeholder="Enter number of adults" onChange={e => setNumAdults(Number.parseInt(e.target.value))} />
                </div>
                <div className="price kid-price">
                    <label htmlFor="numKids"><b>Kids (${park.PricePerChild || 0}/day)</b></label>
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