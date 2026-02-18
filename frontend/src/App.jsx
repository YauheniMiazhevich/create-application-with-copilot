import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import './App.css';
import { AuthProvider } from './contexts/AuthContext.jsx';
import PrivateRoute from './components/PrivateRoute.jsx';
import Header from './components/Header.jsx';
import LoginRegister from './components/Auth/LoginRegister.jsx';
import RealEstateMain from './components/RealEstateMain.jsx';

function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <div className="App">
          <Routes>
            <Route path="/login" element={
              <>
                <Header />
                <LoginRegister />
              </>
            } />
            <Route 
              path="/dashboard" 
              element={
                <PrivateRoute>
                  <RealEstateMain />
                </PrivateRoute>
              } 
            />
            <Route path="/" element={<Navigate to="/dashboard" replace />} />
          </Routes>
        </div>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
