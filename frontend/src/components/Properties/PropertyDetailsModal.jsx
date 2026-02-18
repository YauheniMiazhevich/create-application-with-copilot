import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import Modal from '../common/Modal.jsx';
import LoadingSpinner from '../common/LoadingSpinner.jsx';
import { generatePropertyImage } from '../../utils/imageGenerator';
import './PropertyDetailsModal.css';

const PropertyDetailsModal = React.memo(({ 
  isOpen, 
  onClose, 
  property, 
  owner, 
  onEdit, 
  showEditButton,
  loading 
}) => {
  const [imageUrl, setImageUrl] = useState('');

  useEffect(() => {
    if (property?.propertyType?.type) {
      generatePropertyImage(property.propertyType.type).then(setImageUrl);
    }
  }, [property?.propertyType?.type]);

  if (!property) return null;

  const formatCurrency = (amount) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(amount);
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} title="Property Details" size="large">
      {loading ? (
        <LoadingSpinner message="Loading property details..." />
      ) : (
        <div className="property-details-modal">
          <div className="property-details-grid">
            <div className="property-details-section">
              <h3>Property Information</h3>
              {imageUrl && (
                <div className="property-details-image">
                  <img src={imageUrl} alt={property.propertyType?.type || 'Property'} />
                </div>
              )}
              <dl className="property-details-list">
                <dt>Property ID</dt>
                <dd>#{property.id}</dd>

                <dt>Property Type</dt>
                <dd>{property.propertyType?.type || 'N/A'}</dd>

                <dt>Country</dt>
                <dd>{property.country}</dd>

                <dt>City</dt>
                <dd>{property.city}</dd>

                <dt>Street</dt>
                <dd>{property.street || 'N/A'}</dd>

                <dt>Zip Code</dt>
                <dd>{property.zipCode || 'N/A'}</dd>

                <dt>Property Length</dt>
                <dd>{property.propertyLength} sq ft</dd>

                <dt>Property Cost</dt>
                <dd className="property-cost">{formatCurrency(property.propertyCost)}</dd>

                <dt>Date of Building</dt>
                <dd>{formatDate(property.dateOfBuilding)}</dd>

                <dt>Description</dt>
                <dd className="property-description">{property.description || 'No description provided.'}</dd>
              </dl>
            </div>

            <div className="property-details-section">
              <h3>Owner Information</h3>
              {owner ? (
                <dl className="property-details-list">
                  <dt>Name</dt>
                  <dd>{owner.firstName} {owner.lastName}</dd>

                  <dt>Email</dt>
                  <dd>
                    <a href={`mailto:${owner.email}`}>{owner.email}</a>
                  </dd>

                  <dt>Phone</dt>
                  <dd>
                    <a href={`tel:${owner.phone}`}>{owner.phone}</a>
                  </dd>

                  <dt>Address</dt>
                  <dd>{owner.address || 'N/A'}</dd>

                  <dt>Company Contact</dt>
                  <dd>{owner.isCompanyContact ? 'Yes' : 'No'}</dd>

                  <dt>Description</dt>
                  <dd className="property-description">{owner.description || 'No description provided.'}</dd>
                </dl>
              ) : (
                <p>Owner information not available.</p>
              )}
            </div>
          </div>

          {showEditButton && (
            <div className="property-details-actions">
              <button 
                className="property-details-edit-button"
                onClick={onEdit}
                aria-label="Edit property details"
              >
                ✏️ Edit Property
              </button>
            </div>
          )}
        </div>
      )}
    </Modal>
  );
});

PropertyDetailsModal.displayName = 'PropertyDetailsModal';

PropertyDetailsModal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  property: PropTypes.shape({
    id: PropTypes.number,
    country: PropTypes.string,
    city: PropTypes.string,
    street: PropTypes.string,
    zipCode: PropTypes.string,
    propertyLength: PropTypes.number,
    propertyCost: PropTypes.number,
    description: PropTypes.string,
    dateOfBuilding: PropTypes.string,
    propertyType: PropTypes.shape({
      type: PropTypes.string
    })
  }),
  owner: PropTypes.shape({
    firstName: PropTypes.string,
    lastName: PropTypes.string,
    email: PropTypes.string,
    phone: PropTypes.string,
    address: PropTypes.string,
    description: PropTypes.string,
    isCompanyContact: PropTypes.bool
  }),
  onEdit: PropTypes.func,
  showEditButton: PropTypes.bool,
  loading: PropTypes.bool
};

PropertyDetailsModal.defaultProps = {
  showEditButton: false,
  loading: false
};

export default PropertyDetailsModal;
