import React from 'react';
import PropTypes from 'prop-types';
import './Sidebar.css';

const Sidebar = ({ onAddProperty, onAddOwner, isAdmin }) => {
  if (!isAdmin) {
    return null; // Don't render sidebar for non-admin users
  }

  return (
    <nav className="sidebar" aria-label="Main navigation">
      <div className="sidebar-content">
        <h2 className="sidebar-title">Admin Actions</h2>
        <ul className="sidebar-menu">
          <li>
            <button 
              className="sidebar-button"
              onClick={onAddProperty}
              aria-label="Add new property"
            >
              <span className="sidebar-button-icon">🏠</span>
              <span className="sidebar-button-text">Add Property</span>
            </button>
          </li>
          <li>
            <button 
              className="sidebar-button"
              onClick={onAddOwner}
              aria-label="Add new owner"
            >
              <span className="sidebar-button-icon">👤</span>
              <span className="sidebar-button-text">Add Owner</span>
            </button>
          </li>
        </ul>
      </div>
    </nav>
  );
};

Sidebar.propTypes = {
  onAddProperty: PropTypes.func.isRequired,
  onAddOwner: PropTypes.func.isRequired,
  isAdmin: PropTypes.bool.isRequired
};

export default Sidebar;
