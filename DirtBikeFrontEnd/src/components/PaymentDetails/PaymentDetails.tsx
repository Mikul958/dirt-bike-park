import CartService from "../../services/cartService";
import { useState } from "react";

type PaymentDetailsProps = {
    cartService: CartService;
}

export default function PaymentDetails(props: PaymentDetailsProps) {
    const { cartService } = props;

    const [cardNumber, setCardNumber] = useState("");
    const [ccv, setCcv] = useState("");
    const [expDate, setExpDate] = useState("");
    const [name, setName] = useState("");

    const handleCcvChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setCcv(e.target.value);
    }
    
    const handleExpDate = (e: React.ChangeEvent<HTMLInputElement>) => {
        setExpDate(e.target.value)
    }

    const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setName(e.target.value);
    }

    const sendCardDetails = () => {
        cartService.submitPayment(cardNumber, ccv, expDate, name)
        setCardNumber("");
        setCcv("");
        setExpDate("");
        setName("");
    }

    return (
        <div>
            <div>
                <label>Card Number</label>
                <input type="text" onChange={e => setCardNumber(e.target.value.replace(/\D/, ''))} value={cardNumber} />
            </div>
            <div>
                <label>CCV</label>
                <input type="text" onChange={handleCcvChange} value={ccv} />
            </div>
            <div>
                <label>Expiration Date</label>
                <input type="text" onChange={handleExpDate} value={expDate} />
            </div>
            <div>
                <label>Name on Card</label>
                <input type="text" onChange={handleNameChange} value={name}/>
            </div>
            <button onClick={() => sendCardDetails()}>Submit Payment</button>
        </div>
    )
}