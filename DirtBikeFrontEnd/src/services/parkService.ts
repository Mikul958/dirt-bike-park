import Park from "../models/park";
import Review from "../models/review"

export default class ParkService
{
    private readonly PARK_URL_BASE: string = "https://localhost:7226/api/park/"
    private parks: Park[] = [];

    public signal: number = 0;
    public parksLength: number = this.parks.length;

    loadParks = async (): Promise<Park[]> => {
        const res = await fetch(this.PARK_URL_BASE, {
            method: "GET",
            headers: {
                "Content-Type": "application/json"
            }
        })
        if (!res.ok)
            throw new Error("Failed to fetch parks: " + await res.text())

        this.parks = await res.json() as Park[]
        for (let park of this.parks) {
            park.reviews = mockReviews;
        }
        console.log("parkservice add:");
        console.log(this.parks);

        this.parksLength = this.parks.length;
        this.signal++;
        return this.parks;
    }
    
    getAllParks = (): Park[] => {
        return this.parks;
    };

    getParkById = (id: string): Park => {
        return this.parks.find((park: Park) => park.id.toString() === id);
    }
}

const mockReviews: Review[] = [
    {
        "author": {
            "id": "17fa0861-d120-4cd8-a24b-f9a579ecbf17",
            "displayName": "Moto R. Bike",
            "fullName": "James Sheldon",
            "dateOfBirth": new Date("1987-04-23T18:25:43.511Z")
        },
        "rating": 5,
        "dateWritten": new Date("2025-09-15T12:30:32.594Z"),
        "dateVisited": new Date("2025-09-13T00:00:00.000Z"),
        "review": "Phenomenal park!"
    },
    {
        "author": {
            "id": "bb7e4654-505c-483a-ae1d-ffcbd1a44d8b",
            "displayName": "Riciela Rider",
            "fullName": "Tina Stars",
            "dateOfBirth": new Date("1956-02-23T07:16:43.511Z")
        },
        "rating": 4,
        "dateWritten": new Date("2025-09-12T12:30:32.594Z"),
        "dateVisited": new Date("2025-09-11T00:00:00.000Z"),
        "review": "Great time but missed the mark on the rides for me"
    },
    {
        "author": {
            "id": "704cbc8b-5758-4564-926f-157177428d43",
            "displayName": "Wrangled Rider",
            "fullName": "Bill Stars",
            "dateOfBirth": new Date("1964-12-23T07:16:43.511Z")
        },
        "rating": 5,
        "dateWritten": new Date("2025-09-12T12:30:32.594Z"),
        "dateVisited": new Date("2025-09-11T00:00:00.000Z"),
        "review": "I had a blast but I think my wife was a little sick from some of the hills :("
    },
    {
        "author": {
            "id": "bd51ec1d-a13c-4f54-bee8-167181291df5",
            "displayName": "Removed by Content Moderation Team",
            "fullName": "Philip Shead",
            "dateOfBirth": new Date("1998-07-13T12:38:31.029Z")
        },
        "rating": 1,
        "dateWritten": new Date("2025-07-09T12:30:32.594Z"),
        "dateVisited": new Date("2025-06-12T00:00:00.000Z"),
        "review": "this plaec SUks, Crossbar Parkway iz wy btr"
    }
]