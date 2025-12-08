import './App.css';
import { Route, Routes } from 'react-router-dom';
import Home from './organisms/Home/home';
import ParkDetails from './organisms/ParkDetails/parkDetails';
import Cart from './organisms/Cart/cart'
import ParkService from './services/parkService';
import CartService from './services/cartService';
import { useEffect, useState } from 'react';
import Homebar from './components/Homebar/homebar';
import Footer from './components/Footer/footer';

function App() {

  const parkService = new ParkService();
  const cartService = new CartService();
  const [cart, setCart] = useState(cartService.getCart());

  // Load cart asynchronously when component mounts
  useEffect(() => {
      cartService.loadCart().then(loadedCart => {
          setCart(loadedCart);
      });
  }, []);

  const handleChange = () => {
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
