import { BookingResponse } from "../../models/booking"
import './cartCard.css';

type cartCardProps = {
	booking: BookingResponse
	updateFn: (booking: BookingResponse) => void
	deleteFn: (bookingToDelete: BookingResponse) => void
}

export default function CartCard(props: cartCardProps) {
	const { booking, updateFn, deleteFn } = props;

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const newItem: BookingResponse = {
			...booking,
			[e.target.name]: Number.parseInt(e.target.value)
		}
		updateFn(newItem);
	}

	return (
		<div className="card-body">
			<div className="card left-column">
				<img src={booking.park.imageUrl} alt={booking.park.name} className="card-image" />
			</div>
			<div className="card right-column">
				<h2 className="card-title">
					{booking.park.name}
				</h2>
				<h3 className="card-subtitle">
					{booking.date}
				</h3>
				<div>
					Location: {booking.park.location}
				</div>
				<div>
					Adults: <input name="numAdults" value={booking.numAdults} onChange={handleChange}/> x ${booking.park.pricePerAdult} = ${(booking.numAdults * booking.park.pricePerAdult).toFixed(2)}
				</div>
				<div>
					Children: <input name="numKids" value={booking.numChildren} onChange={handleChange}/> x ${booking.park.pricePerChild} = ${(booking.numChildren * booking.park.pricePerChild).toFixed(2)}
				</div>
			</div>
			<button className="delete-button" onClick={() => deleteFn(booking)}>
				<svg className="icon" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M3 6h18"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/><line x1="10" y1="11" x2="10" y2="17"/><line x1="14" y1="11" x2="14" y2="17"/></svg>
			</button>
		</div>
	)
}