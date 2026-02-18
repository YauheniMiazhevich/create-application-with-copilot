import React from 'react';
import PropTypes from 'prop-types';
import './ErrorMessage.css';

const ErrorMessage = ({ message, onRetry }) => {
  return (
    <div className="error-message-container" role="alert">
      <div className="error-icon">⚠️</div>
      <p className="error-text">{message}</p>
      {onRetry && (
        <button 
          className="error-retry-button"
          onClick={onRetry}
        >
          Try Again
        </button>
      )}
    </div>
  );
};

ErrorMessage.propTypes = {
  message: PropTypes.string.isRequired,
  onRetry: PropTypes.func
};

export default ErrorMessage;
