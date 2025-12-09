import { useParams } from "react-router";
import DetailsPage from "../../components/DetailsPage/detailsPage";
import { useEffect, useState } from "react";
import Park from "../../models/park";
import './parkDetails.css';
import ParkService from "../../services/parkService";
import CartService from "../../services/cartService";

interface ParkDetailsProps {
    parkService: ParkService
    cartService: CartService
    onBook: () => void
}

export default function ParkDetails(props: ParkDetailsProps) {
    const { parkId } = useParams();
    const { parkService, cartService, onBook } = props;
    const [selectedPark, setSelectedPark] = useState({} as Park)

    useEffect(() => {
        if (parkService.parksLength > 0) {
            const park = parkService.getParkById(parkId)
            setSelectedPark(park);
            console.log("Selected Park: ");
            console.log(park);
        }
    }, [parkId, parkService, parkService.signal])
    

    return (
        <div>
            <DetailsPage park={selectedPark} parkService={parkService} cartService={cartService} onBook={onBook}/>
        </div>
    )
}