import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext.jsx';
import './Header.css';

function Header() {
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
    <header className="app-header">
      <div className="header-content">
        <div className="header-left">
          <h1>Property Management</h1>
        </div>
        <div className="header-right">
          <span className="user-info">
            {user?.email}
            {user?.roles && user.roles.length > 0 && (
              <span className="user-role"> ({user.roles.join(', ')})</span>
            )}
          </span>
          <button onClick={handleLogout} className="btn-logout">
            Logout
          </button>
        </div>
      </div>
    </header>
  );
}

export default Header;
