import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import Modal from '../common/Modal.jsx';
import '../Properties/PropertyFormModal.css'; // Reuse same form styling

const OwnerFormModal = ({ 
  isOpen, 
  onClose, 
  onSubmit, 
  initialData = null, 
  mode = 'create',
  loading = false 
}) => {
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    address: '',
    description: ''
  });

  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (mode === 'edit' && initialData) {
      setFormData({
        firstName: initialData.firstName || '',
        lastName: initialData.lastName || '',
        email: initialData.email || '',
        phone: initialData.phone || '',
        address: initialData.address || '',
        description: initialData.description || ''
      });
    } else {
      // Reset form for create mode
      setFormData({
        firstName: '',
        lastName: '',
        email: '',
        phone: '',
        address: '',
        description: ''
      });
    }
    setErrors({});
  }, [mode, initialData, isOpen]);

  const validateForm = () => {
    const newErrors = {};

    // FirstName validation (required, max 100)
    if (!formData.firstName || !formData.firstName.trim()) {
      newErrors.firstName = 'First name is required';
    } else if (formData.firstName.length > 100) {
      newErrors.firstName = 'First name must be 100 characters or less';
    }

    // LastName validation (required, max 100)
    if (!formData.lastName || !formData.lastName.trim()) {
      newErrors.lastName = 'Last name is required';
    } else if (formData.lastName.length > 100) {
      newErrors.lastName = 'Last name must be 100 characters or less';
    }

    // Email validation (required, valid format, max 200)
    if (!formData.email || !formData.email.trim()) {
      newErrors.email = 'Email is required';
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(formData.email)) {
      newErrors.email = 'Email format is invalid';
    } else if (formData.email.length > 200) {
      newErrors.email = 'Email must be 200 characters or less';
    }

    // Phone validation (required, regex pattern, max 20)
    if (!formData.phone || !formData.phone.trim()) {
      newErrors.phone = 'Phone is required';
    } else if (!/^[\d\s\+\-\(\)]+$/.test(formData.phone)) {
      newErrors.phone = 'Phone can only contain digits, spaces, +, -, (, )';
    } else if (formData.phone.length > 20) {
      newErrors.phone = 'Phone must be 20 characters or less';
    }

    // Address validation (optional, max 500)
    if (formData.address && formData.address.length > 500) {
      newErrors.address = 'Address must be 500 characters or less';
    }

    // Description validation (optional, max 1000)
    if (formData.description && formData.description.length > 1000) {
      newErrors.description = 'Description must be 1000 characters or less';
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

    onSubmit(formData);
  };

  return (
    <Modal 
      isOpen={isOpen} 
      onClose={onClose} 
      title={mode === 'create' ? 'Add New Owner' : 'Edit Owner'}
      size="medium"
    >
      <form onSubmit={handleSubmit} className="property-form" noValidate>
        <div className="form-grid">
          <div className="form-group">
            <label htmlFor="firstName">
              First Name <span className="required">*</span>
            </label>
            <input
              type="text"
              id="firstName"
              name="firstName"
              value={formData.firstName}
              onChange={handleChange}
              maxLength="100"
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.firstName}
              aria-describedby={errors.firstName ? 'firstName-error' : undefined}
            />
            {errors.firstName && (
              <span className="error-text" id="firstName-error" role="alert">
                {errors.firstName}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="lastName">
              Last Name <span className="required">*</span>
            </label>
            <input
              type="text"
              id="lastName"
              name="lastName"
              value={formData.lastName}
              onChange={handleChange}
              maxLength="100"
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.lastName}
              aria-describedby={errors.lastName ? 'lastName-error' : undefined}
            />
            {errors.lastName && (
              <span className="error-text" id="lastName-error" role="alert">
                {errors.lastName}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="email">
              Email <span className="required">*</span>
            </label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              maxLength="200"
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.email}
              aria-describedby={errors.email ? 'email-error' : undefined}
            />
            {errors.email && (
              <span className="error-text" id="email-error" role="alert">
                {errors.email}
              </span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="phone">
              Phone <span className="required">*</span>
            </label>
            <input
              type="tel"
              id="phone"
              name="phone"
              value={formData.phone}
              onChange={handleChange}
              maxLength="20"
              placeholder="+1 234 567-8900"
              disabled={loading}
              aria-required="true"
              aria-invalid={!!errors.phone}
              aria-describedby={errors.phone ? 'phone-error' : undefined}
            />
            {errors.phone && (
              <span className="error-text" id="phone-error" role="alert">
                {errors.phone}
              </span>
            )}
          </div>
        </div>

        <div className="form-group form-group-full">
          <label htmlFor="address">Address</label>
          <textarea
            id="address"
            name="address"
            value={formData.address}
            onChange={handleChange}
            maxLength="500"
            rows="3"
            disabled={loading}
            aria-invalid={!!errors.address}
            aria-describedby={errors.address ? 'address-error' : undefined}
          />
          {errors.address && (
            <span className="error-text" id="address-error" role="alert">
              {errors.address}
            </span>
          )}
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
            {loading ? 'Saving...' : mode === 'create' ? 'Create Owner' : 'Update Owner'}
          </button>
        </div>
      </form>
    </Modal>
  );
};

OwnerFormModal.propTypes = {
  isOpen: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSubmit: PropTypes.func.isRequired,
  initialData: PropTypes.object,
  mode: PropTypes.oneOf(['create', 'edit']),
  loading: PropTypes.bool
};

export default OwnerFormModal;
