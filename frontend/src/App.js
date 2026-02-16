import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import './App.css';
import { AuthProvider } from './contexts/AuthContext';
import PrivateRoute from './components/PrivateRoute';
import Header from './components/Header';
import LoginRegister from './components/Auth/LoginRegister';
import ProductList from './components/ProductList';

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <div className="App">
          <Header />
          <Routes>
            <Route path="/login" element={<LoginRegister />} />
            <Route 
              path="/products" 
              element={
                <PrivateRoute>
                  <main className="main-content">
                    <ProductList />
                  </main>
                </PrivateRoute>
              } 
            />
            <Route path="/" element={<Navigate to="/products" replace />} />
          </Routes>
        </div>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
