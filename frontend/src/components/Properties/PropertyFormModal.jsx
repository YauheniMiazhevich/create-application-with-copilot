import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import Modal from '../common/Modal.jsx';
import './PropertyFormModal.css';

const PropertyFormModal = ({ 
  isOpen, 
  onClose, 
  onSubmit, 
  initialData = null, 
  propertyTypes = [], 
  mode = 'create',
  loading = false 
}) => {
  const [formData, setFormData] = useState({
    ownerId: '',
    propertyTypeId: '',
    propertyLength: '',
    propertyCost: '',
    dateOfBuilding: '',
    description: '',
    country: '',
    city: '',
    street: '',
    zipCode: ''
  });

  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (mode === 'edit' && initialData) {
      setFormData({
        ownerId: initialData.ownerId || '',
        propertyTypeId: initialData.propertyTypeId || '',
        propertyLength: initialData.propertyLength || '',
        propertyCost: initialData.propertyCost || '',
        dateOfBuilding: initialData.dateOfBuilding?.split('T')[0] || '',
        description: initialData.description || '',
        country: initialData.country || '',
        city: initialData.city || '',
        street: initialData.street || '',
        zipCode: initialData.zipCode || ''
      });
    } else {
      // Reset form for create mode
      setFormData({
        ownerId: '',
        propertyTypeId: '',
        propertyLength: '',
        propertyCost: '',
        dateOfBuilding: '',
        description: '',
        country: '',
        city: '',
        street: '',
        zipCode: ''
      });
    }
    setErrors({});
  }, [mode, initialData, isOpen]);

  const validateForm = () => {
    const newErrors = {};

    // OwnerId validation
    if (!formData.ownerId && formData.ownerId !== 0) {
      newErrors.ownerId = 'Owner ID is required';
    } else if (formData.ownerId <= 0) {
      newErrors.ownerId = 'Owner ID must be greater than 0';
    }

    // PropertyTypeId validation
    if (!formData.propertyTypeId || formData.propertyTypeId <= 0) {
      newErrors.propertyTypeId = 'Property type is required';
    }

    // PropertyLength validation
    if (!formData.propertyLength || formData.propertyLength <= 0) {
      newErrors.propertyLength = 'Property length must be greater than 0';
    }

    // PropertyCost validation
    if (!formData.propertyCost && formData.propertyCost !== 0) {
      newErrors.propertyCost = 'Property cost is required';
    } else if (parseFloat(formData.propertyCost) < 0) {
      newErrors.propertyCost = 'Property cost must be at least 0';
    }

    // DateOfBuilding validation
    if (formData.dateOfBuilding) {
      const buildingDate = new Date(formData.dateOfBuilding);
      const today = new Date();
      today.setHours(0, 0, 0, 0);
      if (buildingDate > today) {
        newErrors.dateOfBuilding = 'Date of building cannot be in the future';
      }
    }

    // Country validation (required)
    if (!formData.country || !formData.country.trim()) {
      newErrors.country = 'Country is required';
    } else if (formData.country.length > 100) {
      newErrors.country = 'Country must be 100 characters or less';
    }

    // City validation (required)
    if (!formData.city || !formData.city.trim()) {
      newErrors.city = 'City is required';
    } else if (formData.city.length > 100) {
      newErrors.city = 'City must be 100 characters or less';
    }

    // Description validation
    if (formData.description && formData.description.length > 1000) {
      newErrors.description = 'Description must be 1000 characters or less';
    }

    // Street validation
    if (formData.street && formData.street.length > 200) {
      newErrors.street = 'Street must be 200 characters or less';
    }

    // ZipCode validation
    if (formData.zipCode && formData.zipCode.length > 20) {
      newErrors.zipCode = 'Zip code must be 20 characters or less';
    }

    return newErrors;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
    
    // Clear error for this field when user starts typing
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: '' }));
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    
    const validationErrors = validateForm();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }

    // Convert string values to appropriate types
    const submitData = {
      ...formData,
      ownerId: parseInt(formData.ownerId, 10),
      propertyTypeId: parseInt(formData.propertyTypeId, 10),
      propertyLength: parseFloat(formData.propertyLength),
      propertyCost: parseFloat(formData.propertyCost),
      dateOfBuilding: formData.dateOfBuilding || undefined
    };

    onSubmit(submitData);
  };

  return (
    <Modal 
      isOpen={isOpen} 
      onClose={onClose} 
      title={mode === 'create' ? 'Add New Property' : 'Edit Property'}
      size="large"
    >
      <form onSubmit={handleSubmit} className="property-form" noValidate>
        <div className="form-grid">
          <div className="form-group">
            <label htmlFor="ownerId">
              Owner ID <span className="required">*</span>
            </label>
            <input
              type="number"
              id="ownerId"
              name="ownerId"
              value={formData.ownerId}
              onChange={handleChange}
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.ownerId}
              aria-describedby={errors.ownerId ? 'ownerId-error' : undefined}
            />
            {errors.ownerId && (
              <span className="error-text" id="ownerId-error" role="alert">
                {errors.ownerId}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="propertyTypeId">
              Property Type <span className="required">*</span>
            </label>
            <select
              id="propertyTypeId"
              name="propertyTypeId"
              value={formData.propertyTypeId}
              onChange={handleChange}
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.propertyTypeId}
              aria-describedby={errors.propertyTypeId ? 'propertyTypeId-error' : undefined}
            >
              <option value="">Select a property type</option>
              {propertyTypes.map(type => (
                <option key={type.id} value={type.id}>
                  {type.type}
                </option>
              ))}
            </select>
            {errors.propertyTypeId && (
              <span className="error-text" id="propertyTypeId-error" role="alert">
                {errors.propertyTypeId}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="propertyLength">
              Property Length (sq ft) <span className="required">*</span>
            </label>
            <input
              type="number"
              id="propertyLength"
              name="propertyLength"
              value={formData.propertyLength}
              onChange={handleChange}
              step="0.01"
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.propertyLength}
              aria-describedby={errors.propertyLength ? 'propertyLength-error' : undefined}
            />
            {errors.propertyLength && (
              <span className="error-text" id="propertyLength-error" role="alert">
                {errors.propertyLength}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="propertyCost">
              Property Cost ($) <span className="required">*</span>
            </label>
            <input
              type="number"
              id="propertyCost"
              name="propertyCost"
              value={formData.propertyCost}
              onChange={handleChange}
              step="0.01"
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.propertyCost}
              aria-describedby={errors.propertyCost ? 'propertyCost-error' : undefined}
            />
            {errors.propertyCost && (
              <span className="error-text" id="propertyCost-error" role="alert">
                {errors.propertyCost}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="dateOfBuilding">Date of Building</label>
            <input
              type="date"
              id="dateOfBuilding"
              name="dateOfBuilding"
              value={formData.dateOfBuilding}
              onChange={handleChange}
              max={new Date().toISOString().split('T')[0]}
              disabled={loading}
              aria-invalid={!!errors.dateOfBuilding}
              aria-describedby={errors.dateOfBuilding ? 'dateOfBuilding-error' : undefined}
            />
            {errors.dateOfBuilding && (
              <span className="error-text" id="dateOfBuilding-error" role="alert">
                {errors.dateOfBuilding}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="country">
              Country <span className="required">*</span>
            </label>
            <input
              type="text"
              id="country"
              name="country"
              value={formData.country}
              onChange={handleChange}
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.country}
              aria-describedby={errors.country ? 'country-error' : undefined}
            />
            {errors.country && (
              <span className="error-text" id="country-error" role="alert">
                {errors.country}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="city">
              City <span className="required">*</span>
            </label>
            <input
              type="text"
              id="city"
              name="city"
              value={formData.city}
              onChange={handleChange}
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.city}
              aria-describedby={errors.city ? 'city-error' : undefined}
            />
            {errors.city && (
              <span className="error-text" id="city-error" role="alert">
                {errors.city}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="street">Street</label>
            <input
              type="text"
              id="street"
              name="street"
              value={formData.street}
              onChange={handleChange}
              maxLength="200"
              disabled={loading}
              aria-invalid={!!errors.street}
              aria-describedby={errors.street ? 'street-error' : undefined}
            />
            {errors.street && (
              <span className="error-text" id="street-error" role="alert">
                {errors.street}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="zipCode">Zip Code</label>
            <input
              type="text"
              id="zipCode"
              name="zipCode"
              value={formData.zipCode}
              onChange={handleChange}
              maxLength="20"
              disabled={loading}
              aria-invalid={!!errors.zipCode}
              aria-describedby={errors.zipCode ? 'zipCode-error' : undefined}
            />
            {errors.zipCode && (
              <span className="error-text" id="zipCode-error" role="alert">
                {errors.zipCode}
              </span>
            )}
          </div>
        </div>

        <div className="form-group form-group-full">
          <label htmlFor="description">Description</label>
          <textarea
            id="description"
            name="description"
            value={formData.description}
            onChange={handleChange}
            maxLength="1000"
            rows="4"
            disabled={loading}
            aria-invalid={!!errors.description}
            aria-describedby={errors.description ? 'description-error' : undefined}
          />
          {errors.description && (
            <span className="error-text" id="description-error" role="alert">
              {errors.description}
            </span>
          )}
        </div>

        <div className="form-actions">
          <button 
            type="button" 
            onClick={onClose} 
            className="form-button form-button-cancel"
            disabled={loading}
          >
            Cancel
          </button>
          <button 
            type="submit" 
            className="form-button form-button-submit"
            disabled={loading}
          >
            {loading ? 'Saving...' : mode === 'create' ? 'Create Property' : 'Update Property'}
          </button>
        </div>
      </form>
    </Modal>
  );
};

PropertyFormModal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
  initialData: PropTypes.object,
  propertyTypes: PropTypes.arrayOf(PropTypes.shape({
    id: PropTypes.number.isRequired,
    type: PropTypes.string.isRequired
  })),
  mode: PropTypes.oneOf(['create', 'edit']),
  loading: PropTypes.bool
};

export default PropertyFormModal;
