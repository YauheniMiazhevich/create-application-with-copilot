import React from 'react';
import PropTypes from 'prop-types';
import './Header.css';

const Header = ({ user }) => {
  return (
    <header className="page-header">
      <div className="page-header-content">
        <h1 className="page-header-title">Real Estate Main</h1>
        {user && (
          <div className="page-header-user">
            <span className="page-header-email">{user.email}</span>
            <div className="page-header-roles">
              {user.roles && user.roles.map((role, index) => (
                <span key={index} className="page-header-role-badge">
                  {role}
                </span>
              ))}
            </div>
          </div>
        )}
      </div>
    </header>
  );
};

Header.propTypes = {
  user: PropTypes.shape({
    email: PropTypes.string,
    roles: PropTypes.arrayOf(PropTypes.string)
  })
};

export default Header;
