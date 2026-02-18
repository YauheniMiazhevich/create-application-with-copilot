import React, { useState, useEffect } from 'react';
import { useAuth } from '../contexts/AuthContext.jsx';
import useProperties from '../hooks/useProperties';
import usePropertyTypes from '../hooks/usePropertyTypes';
import useOwners from '../hooks/useOwners';
import ErrorBoundary from './common/ErrorBoundary.jsx';
import Header from './common/Header.jsx';
import LoadingSpinner from './common/LoadingSpinner.jsx';
import ErrorMessage from './common/ErrorMessage.jsx';
import Sidebar from './Sidebar/Sidebar.jsx';
import PropertyCard from './Properties/PropertyCard.jsx';
import PropertyDetailsModal from './Properties/PropertyDetailsModal.jsx';
import PropertyFormModal from './Properties/PropertyFormModal.jsx';
import OwnerFormModal from './Owners/OwnerFormModal.jsx';
import './RealEstateMain.css';

const RealEstateMain = () => {
  const { user } = useAuth();
  const { 
    properties, 
    loading: propertiesLoading, 
    error: propertiesError, 
    fetchProperties, 
    createProperty, 
    updateProperty, 
    deleteProperty 
  } = useProperties();
  
  const { 
    propertyTypes, 
    loading: typesLoading 
  } = usePropertyTypes();
  
  const { 
    fetchOwnerById, 
    createOwner, 
    loading: ownersLoading 
  } = useOwners();

  // Modal states
  const [selectedProperty, setSelectedProperty] = useState(null);
  const [selectedOwner, setSelectedOwner] = useState(null);
  const [showPropertyModal, setShowPropertyModal] = useState(false);
  const [showPropertyFormModal, setShowPropertyFormModal] = useState(false);
  const [showOwnerFormModal, setShowOwnerFormModal] = useState(false);
  const [propertyFormMode, setPropertyFormMode] = useState('create');
  const [loadingOwner, setLoadingOwner] = useState(false);

  // Check if user is admin
  const isAdmin = user?.roles?.includes('Admin');

  // Fetch properties on mount
  useEffect(() => {
    fetchProperties();
  }, [fetchProperties]);

  // Handle property card click
  const handlePropertyClick = async (property) => {
    setSelectedProperty(property);
    setLoadingOwner(true);
    setShowPropertyModal(true);
    
    try {
      const owner = await fetchOwnerById(property.ownerId);
      setSelectedOwner(owner);
    } catch (error) {
      console.error('Failed to fetch owner:', error);
      setSelectedOwner(null);
    } finally {
      setLoadingOwner(false);
    }
  };

  // Handle property delete
  const handleDeleteProperty = async (propertyId) => {
    try {
      await deleteProperty(propertyId);
      // Show success notification (could add a toast notification here)
      console.log('Property deleted successfully');
    } catch (error) {
      alert(`Failed to delete property: ${error.message}`);
    }
  };

  // Handle property edit
  const handleEditProperty = () => {
    setPropertyFormMode('edit');
    setShowPropertyModal(false);
    setShowPropertyFormModal(true);
  };

  // Handle create property
  const handleCreateProperty = async (propertyData) => {
    try {
      await createProperty(propertyData);
      setShowPropertyFormModal(false);
      // Show success notification
      alert('Property created successfully!');
    } catch (error) {
      alert(`Failed to create property: ${error.message}`);
    }
  };

  // Handle update property
  const handleUpdateProperty = async (propertyData) => {
    try {
      await updateProperty(selectedProperty.id, propertyData);
      setShowPropertyFormModal(false);
      setSelectedProperty(null);
      // Show success notification
      alert('Property updated successfully!');
    } catch (error) {
      alert(`Failed to update property: ${error.message}`);
    }
  };

  // Handle create owner
  const handleCreateOwner = async (ownerData) => {
    try {
      await createOwner(ownerData);
      setShowOwnerFormModal(false);
      // Show success notification
      alert('Owner created successfully!');
    } catch (error) {
      alert(`Failed to create owner: ${error.message}`);
    }
  };

  // Handle close property details modal
  const handleClosePropertyModal = () => {
    setShowPropertyModal(false);
    setSelectedProperty(null);
    setSelectedOwner(null);
  };

  // Handle close property form modal
  const handleClosePropertyFormModal = () => {
    setShowPropertyFormModal(false);
    if (propertyFormMode === 'edit') {
      setSelectedProperty(null);
    }
  };

  // Handle open add property modal
  const handleOpenAddProperty = () => {
    setPropertyFormMode('create');
    setSelectedProperty(null);
    setShowPropertyFormModal(true);
  };

  // Handle open add owner modal
  const handleOpenAddOwner = () => {
    setShowOwnerFormModal(true);
  };

  const loading = propertiesLoading || typesLoading;

  return (
    <ErrorBoundary>
      <div className="real-estate-main">
        <Header user={user} />
        
        <div className="real-estate-layout">
          <Sidebar 
            isAdmin={isAdmin}
            onAddProperty={handleOpenAddProperty}
            onAddOwner={handleOpenAddOwner}
          />
          
          <main className="real-estate-content" aria-label="Properties list">
            {loading && properties.length === 0 ? (
              <LoadingSpinner message="Loading properties..." />
            ) : propertiesError ? (
              <ErrorMessage 
                message={propertiesError} 
                onRetry={fetchProperties}
              />
            ) : properties.length === 0 ? (
              <div className="real-estate-empty">
                <h2>No Properties Found</h2>
                <p>There are no properties to display.</p>
                {isAdmin && (
                  <button 
                    className="real-estate-empty-button"
                    onClick={handleOpenAddProperty}
                  >
                    Add Your First Property
                  </button>
                )}
              </div>
            ) : (
              <div className="properties-grid">
                {properties.map(property => (
                  <PropertyCard
                    key={property.id}
                    property={property}
                    onClick={() => handlePropertyClick(property)}
                    onDelete={handleDeleteProperty}
                    showDeleteButton={isAdmin}
                  />
                ))}
              </div>
            )}
          </main>
        </div>

        {/* Property Details Modal */}
        <PropertyDetailsModal
          isOpen={showPropertyModal}
          onClose={handleClosePropertyModal}
          property={selectedProperty}
          owner={selectedOwner}
          onEdit={handleEditProperty}
          showEditButton={isAdmin}
          loading={loadingOwner}
        />

        {/* Property Form Modal */}
        <PropertyFormModal
          isOpen={showPropertyFormModal}
          onClose={handleClosePropertyFormModal}
          onSubmit={propertyFormMode === 'create' ? handleCreateProperty : handleUpdateProperty}
          initialData={propertyFormMode === 'edit' ? selectedProperty : null}
          propertyTypes={propertyTypes}
          mode={propertyFormMode}
          loading={propertiesLoading}
        />

        {/* Owner Form Modal */}
        <OwnerFormModal
          isOpen={showOwnerFormModal}
          onClose={() => setShowOwnerFormModal(false)}
          onSubmit={handleCreateOwner}
          mode="create"
          loading={ownersLoading}
        />
      </div>
    </ErrorBoundary>
  );
};

export default RealEstateMain;
