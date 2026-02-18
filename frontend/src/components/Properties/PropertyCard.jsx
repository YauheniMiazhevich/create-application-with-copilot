import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import { generatePropertyImage } from '../../utils/imageGenerator';
import './PropertyCard.css';

const PropertyCard = React.memo(({ property, onClick, onDelete, showDeleteButton }) => {
  const [imageUrl, setImageUrl] = useState('');
  const [imageLoading, setImageLoading] = useState(true);

  useEffect(() => {
    const loadImage = async () => {
      setImageLoading(true);
      try {
        const url = await generatePropertyImage(property.propertyType?.type || 'Property');
        setImageUrl(url);
      } catch (error) {
        console.error('Failed to load property image:', error);
      } finally {
        setImageLoading(false);
      }
    };

    loadImage();
  }, [property.propertyType?.type]);

  const handleKeyPress = (e) => {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      onClick();
    }
  };

  const handleDelete = (e) => {
    e.stopPropagation();
    if (window.confirm(`Are you sure you want to delete this property in ${property.city}, ${property.country}?`)) {
      onDelete(property.id);
    }
  };

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  };

  return (
    <article
      className="property-card"
      onClick={onClick}
      onKeyPress={handleKeyPress}
      tabIndex="0"
      role="button"
      aria-label={`Property: ${property.propertyType?.type || 'Unknown'} in ${property.city}, ${property.country}`}
    >
      <div className="property-card-image">
        {imageLoading ? (
          <div className="property-card-image-loading">Loading...</div>
        ) : (
          <img src={imageUrl} alt={`${property.propertyType?.type || 'Property'}`} />
        )}
      </div>
      <div className="property-card-content">
        <h3 className="property-card-type">{property.propertyType?.type || 'Unknown Type'}</h3>
        <div className="property-card-details">
          <p className="property-card-location">
            📍 {property.city}, {property.country}
          </p>
          <p className="property-card-info">
            📏 {property.propertyLength} sq ft
          </p>
          <p className="property-card-price">
            💰 {formatCurrency(property.propertyCost)}
          </p>
          <p className="property-card-date">
            📅 Built: {formatDate(property.dateOfBuilding)}
          </p>
        </div>
      </div>
      {showDeleteButton && (
        <button
          className="property-card-delete"
          onClick={handleDelete}
          aria-label="Delete property"
        >
          🗑️
        </button>
      )}
    </article>
  );
});

PropertyCard.displayName = 'PropertyCard';

PropertyCard.propTypes = {
  property: PropTypes.shape({
    id: PropTypes.number.isRequired,
    city: PropTypes.string.isRequired,
    country: PropTypes.string.isRequired,
    propertyLength: PropTypes.number.isRequired,
    propertyCost: PropTypes.number.isRequired,
    dateOfBuilding: PropTypes.string.isRequired,
    propertyType: PropTypes.shape({
      type: PropTypes.string
    })
  }).isRequired,
  onClick: PropTypes.func.isRequired,
  onDelete: PropTypes.func,
  showDeleteButton: PropTypes.bool
};

PropertyCard.defaultProps = {
  showDeleteButton: false
};

export default PropertyCard;
