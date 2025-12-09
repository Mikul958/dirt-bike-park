import { ParkCard } from '../parkCard/parkCard';
import Park from '../../models/park';
import './featuredParks.css';

interface FeaturedParksProps {
    allParks: Park[]
}

export default function FeaturedParks(props: FeaturedParksProps) {
    const { allParks } = props;

    console.log("FeaturedParks");
    console.log(allParks);

    return(
        <>
            <h2>Featured Parks</h2>
            <div className="featured-park-grid">
                {allParks.map((park: Park) => {
                    return <ParkCard key={park.id} park={park} />
                })}
            </div>
        </>
    )
}