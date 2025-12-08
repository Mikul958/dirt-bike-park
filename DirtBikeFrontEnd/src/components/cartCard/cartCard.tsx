import { Booking } from "../../models/booking"
import './cartCard.css';

type cartCardProps = {
	booking: Booking
	updateFn: (booking: Booking) => void
	deleteFn: (bookingToDelete: Booking) => void
}

export default function CartCard(props: cartCardProps) {
	const { booking, updateFn, deleteFn } = props;

	const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const newItem: Booking = {
			...booking,
			[e.target.name]: Number.parseInt(e.target.value)
		}
		updateFn(newItem);
	}

	return (
		<div className="card-body">
			<div className="card left-column">
				<img src={booking.Park.ImageUrl} alt={booking.Park.Name} className="card-image" />
			</div>
			<div className="card right-column">
				<h2 className="card-title">
					{booking.Park.Name}
				</h2>
				<h3 className="card-subtitle">
					{booking.Date}
				</h3>
				<div>
					Location: {booking.Park.Location}
				</div>
				<div>
					Adults: <input name="numAdults" value={booking.NumAdults} onChange={handleChange}/> x ${booking.Park.PricePerAdult} = ${(booking.NumAdults * booking.Park.PricePerAdult).toFixed(2)}
				</div>
				<div>
					Children: <input name="numKids" value={booking.NumChildren} onChange={handleChange}/> x ${booking.Park.PricePerChild} = ${(booking.NumChildren * booking.Park.PricePerChild).toFixed(2)}
				</div>
			</div>
			<button className="delete-button" onClick={() => deleteFn(booking)}>
				<svg className="icon" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M3 6h18"/><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/><line x1="10" y1="11" x2="10" y2="17"/><line x1="14" y1="11" x2="14" y2="17"/></svg>
			</button>
		</div>
	)
}