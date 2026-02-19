import { useState, useEffect, useCallback } from 'react';
import * as propertyTypeService from '../services/propertyTypeService';

const usePropertyTypes = () => {
  const [propertyTypes, setPropertyTypes] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(undefined);

  const fetchPropertyTypes = useCallback(async () => {
    setLoading(true);
    setError(undefined);
    try {
      const data = await propertyTypeService.getAllPropertyTypes();
      setPropertyTypes(data);
      return data;
    } catch (err) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to fetch property types';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchPropertyTypes().catch(() => {
      // Error is already handled in fetchPropertyTypes state
    });
  }, [fetchPropertyTypes]);

  return {
    propertyTypes,
    loading,
    error,
    fetchPropertyTypes
  };
};

export default usePropertyTypes;
