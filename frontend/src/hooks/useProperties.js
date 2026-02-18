import { useState, useCallback } from 'react';
import * as propertyService from '../services/propertyService';

const useProperties = () => {
  const [properties, setProperties] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(undefined);

  const fetchProperties = useCallback(async () => {
    setLoading(true);
    setError(undefined);
    try {
      const data = await propertyService.getAllProperties();
      setProperties(data);
    } catch (err) {
      setError(err.response?.data?.message || err.message || 'Failed to fetch properties');
    } finally {
      setLoading(false);
    }
  }, []);

  const createProperty = useCallback(async (propertyData) => {
    setLoading(true);
    setError(undefined);
    try {
      const newProperty = await propertyService.createProperty(propertyData);
      setProperties(prev => [...prev, newProperty]);
      return newProperty;
    } catch (err) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to create property';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, []);

  const updateProperty = useCallback(async (id, propertyData) => {
    setLoading(true);
    setError(undefined);
    try {
      const updatedProperty = await propertyService.updateProperty(id, propertyData);
      setProperties(prev => prev.map(p => p.id === id ? updatedProperty : p));
      return updatedProperty;
    } catch (err) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to update property';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, []);

  const deleteProperty = useCallback(async (id) => {
    setLoading(true);
    setError(undefined);
    try {
      await propertyService.deleteProperty(id);
      setProperties(prev => prev.filter(p => p.id !== id));
      return true;
    } catch (err) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to delete property';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, []);

  return {
    properties,
    loading,
    error,
    fetchProperties,
    createProperty,
    updateProperty,
    deleteProperty
  };
};

export default useProperties;
