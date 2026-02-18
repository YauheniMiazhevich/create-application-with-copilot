import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext.jsx';
import './Header.css';

const Header = () => {
  const { user, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  if (!isAuthenticated) {
    return null;
  }

  return (
    <header className="page-header">
      <div className="page-header-content">
        <h1 className="page-header-title">Property Management</h1>
        <div className="page-header-user">
          <span className="page-header-email">{user?.userName}</span>
          <button onClick={handleLogout} className="page-header-logout">
            Logout
          </button>
        </div>
      </div>
    </header>
  );
};

export default Header;
