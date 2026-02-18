import { useState, useCallback } from 'react';
import * as ownerService from '../services/ownerService';

const useOwners = () => {
  const [owners, setOwners] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(undefined);

  const fetchOwners = useCallback(async () => {
    setLoading(true);
    setError(undefined);
    try {
      const data = await ownerService.getAllOwners();
      setOwners(data);
      return data;
    } catch (err) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to fetch owners';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, []);

  const fetchOwnerById = useCallback(async (id) => {
    setLoading(true);
    setError(undefined);
    try {
      const owner = await ownerService.getOwnerById(id);
      return owner;
    } catch (err) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to fetch owner';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, []);

  const createOwner = useCallback(async (ownerData) => {
    setLoading(true);
    setError(undefined);
    try {
      const newOwner = await ownerService.createOwner(ownerData);
      setOwners(prev => [...prev, newOwner]);
      return newOwner;
    } catch (err) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to create owner';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, []);

  const updateOwner = useCallback(async (id, ownerData) => {
    setLoading(true);
    setError(undefined);
    try {
      const updatedOwner = await ownerService.updateOwner(id, ownerData);
      setOwners(prev => prev.map(o => o.id === id ? updatedOwner : o));
      return updatedOwner;
    } catch (err) {
      const errorMessage = err.response?.data?.message || err.message || 'Failed to update owner';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  }, []);

  return {
    owners,
    loading,
    error,
    fetchOwners,
    fetchOwnerById,
    createOwner,
    updateOwner
  };
};

export default useOwners;
