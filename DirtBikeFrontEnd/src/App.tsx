import './App.css';
import { Route, Routes } from 'react-router-dom';
import Home from './organisms/Home/home';
import ParkDetails from './organisms/ParkDetails/parkDetails';
import Cart from './organisms/Cart/cart'
import ParkService from './services/parkService';
import CartService from './services/cartService';
import { useEffect, useRef, useState } from 'react';
import Homebar from './components/Homebar/homebar';
import Footer from './components/Footer/footer';

function App()
{
    const parkServiceRef = useRef(new ParkService());
    const cartServiceRef = useRef(new CartService());

    const parkService = parkServiceRef.current;
    const cartService = cartServiceRef.current;

    const [parks, setParks] = useState(parkService.getAllParks())
    const [cart, setCart] = useState(cartService.getCart());

  // Load cart asynchronously when component mounts
  useEffect(() => {
      parkService.loadParks()
      .then(loadedParks => {
        setParks(loadedParks);
      })
      cartService.loadCart()
      .then(loadedCart => {
        setCart(loadedCart);
      });
  }, [parkService, cartService]);

  const handleChange = () => {
      setParks(parkService.getAllParks())
      setCart(cartService.getCart());
  }

  return (
      <div className="App">
          <div className="header content">
              <Homebar numItems={cart ? cart.bookings.length : 0} />
          </div>
          <Routes>
              <Route path="/*" element={<Home parkService={parkService} cartService={cartService} />} />
              <Route path="details/:parkId" element={<ParkDetails parkService={parkService} cartService={cartService} onBook={handleChange} />} />
		        <Route path="/cart" element={<Cart cartService={cartService} handleChange={handleChange} /> } />
          </Routes>
          <div className="footer content">
            <Footer />
          </div>
      </div>
   );
}

export default App;
