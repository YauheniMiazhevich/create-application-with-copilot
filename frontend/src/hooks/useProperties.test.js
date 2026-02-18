import { describe, it, expect, vi, beforeEach } from 'vitest';
import { renderHook, waitFor, act } from '@testing-library/react';
import useProperties from './useProperties';
import * as propertyService from '../services/propertyService';

vi.mock('../services/propertyService');

describe('useProperties', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('fetchProperties', () => {
    it('should fetch properties successfully', async () => {
      const mockProperties = [
        { id: 1, city: 'New York', country: 'USA' },
        { id: 2, city: 'London', country: 'UK' }
      ];
      propertyService.getAllProperties.mockResolvedValue(mockProperties);

      const { result } = renderHook(() => useProperties());

      expect(result.current.loading).toBe(false);
      expect(result.current.properties).toEqual([]);

      await act(async () => {
        await result.current.fetchProperties();
      });

      await waitFor(() => {
        expect(result.current.properties).toEqual(mockProperties);
        expect(result.current.loading).toBe(false);
        expect(result.current.error).toBe(undefined);
      });
    });

    it('should handle fetch error', async () => {
      const errorMessage = 'Failed to fetch properties';
      propertyService.getAllProperties.mockRejectedValue(new Error(errorMessage));

      const { result } = renderHook(() => useProperties());

      await act(async () => {
        await result.current.fetchProperties();
      });

      await waitFor(() => {
        expect(result.current.error).toBe(errorMessage);
        expect(result.current.loading).toBe(false);
      });
    });

    it('should set loading state during fetch', async () => {
      propertyService.getAllProperties.mockImplementation(() => 
        new Promise(resolve => setTimeout(() => resolve([]), 100))
      );

      const { result } = renderHook(() => useProperties());

      act(() => {
        result.current.fetchProperties();
      });

      expect(result.current.loading).toBe(true);

      await waitFor(() => {
        expect(result.current.loading).toBe(false);
      });
    });
  });

  describe('createProperty', () => {
    it('should create property and add to list', async () => {
      const newProperty = { id: 3, city: 'Paris', country: 'France' };
      propertyService.createProperty.mockResolvedValue(newProperty);

      const { result } = renderHook(() => useProperties());

      await act(async () => {
        const created = await result.current.createProperty(newProperty);
        expect(created).toEqual(newProperty);
      });

      await waitFor(() => {
        expect(result.current.properties).toContainEqual(newProperty);
      });
    });

    it('should handle create error', async () => {
      const errorMessage = 'Validation failed';
      propertyService.createProperty.mockRejectedValue({ message: errorMessage });

      const { result } = renderHook(() => useProperties());

      try {
        await act(async () => {
          await result.current.createProperty({});
        });
      } catch (error) {
        // Expected to throw
      }

      await waitFor(() => {
        expect(result.current.error).toBe(errorMessage);
        expect(result.current.loading).toBe(false);
      });
    });
  });

  describe('updateProperty', () => {
    it('should update property in list', async () => {
      const initialProperties = [
        { id: 1, city: 'New York', country: 'USA' },
        { id: 2, city: 'London', country: 'UK' }
      ];
      const updatedProperty = { id: 1, city: 'Updated York', country: 'USA' };

      propertyService.getAllProperties.mockResolvedValue(initialProperties);
      propertyService.updateProperty.mockResolvedValue(updatedProperty);

      const { result } = renderHook(() => useProperties());

      await act(async () => {
        await result.current.fetchProperties();
      });

      await act(async () => {
        await result.current.updateProperty(1, { city: 'Updated York' });
      });

      await waitFor(() => {
        const updated = result.current.properties.find(p => p.id === 1);
        expect(updated.city).toBe('Updated York');
      });
    });

    it('should handle update error', async () => {
      propertyService.updateProperty.mockRejectedValue(new Error('Update failed'));

      const { result } = renderHook(() => useProperties());

      await expect(act(async () => {
        await result.current.updateProperty(1, {});
      })).rejects.toThrow();
    });
  });

  describe('deleteProperty', () => {
    it('should remove property from list', async () => {
      const initialProperties = [
        { id: 1, city: 'New York', country: 'USA' },
        { id: 2, city: 'London', country: 'UK' }
      ];

      propertyService.getAllProperties.mockResolvedValue(initialProperties);
      propertyService.deleteProperty.mockResolvedValue({});

      const { result } = renderHook(() => useProperties());

      await act(async () => {
        await result.current.fetchProperties();
      });

      await act(async () => {
        await result.current.deleteProperty(1);
      });

      await waitFor(() => {
        expect(result.current.properties).toHaveLength(1);
        expect(result.current.properties.find(p => p.id === 1)).toBeUndefined();
      });
    });

    it('should handle delete error', async () => {
      propertyService.deleteProperty.mockRejectedValue(new Error('Cannot delete'));

      const { result } = renderHook(() => useProperties());

      await expect(act(async () => {
        await result.current.deleteProperty(1);
      })).rejects.toThrow();
    });
  });
});
