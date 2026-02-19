import { describe, it, expect, vi, beforeEach } from 'vitest';
import * as propertyService from './propertyService';
import api from './api';

vi.mock('./api');

describe('propertyService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('getAllProperties', () => {
    it('should fetch all properties', async () => {
      const mockProperties = [
        { id: 1, city: 'New York', country: 'USA' },
        { id: 2, city: 'London', country: 'UK' }
      ];
      api.get.mockResolvedValue({ data: mockProperties });

      const result = await propertyService.getAllProperties();

      expect(api.get).toHaveBeenCalledWith('/properties');
      expect(result).toEqual(mockProperties);
    });

    it('should throw error when API call fails', async () => {
      api.get.mockRejectedValue(new Error('Network error'));

      await expect(propertyService.getAllProperties()).rejects.toThrow('Network error');
    });
  });

  describe('getPropertyById', () => {
    it('should fetch property by id', async () => {
      const mockProperty = { id: 1, city: 'New York', country: 'USA' };
      api.get.mockResolvedValue({ data: mockProperty });

      const result = await propertyService.getPropertyById(1);

      expect(api.get).toHaveBeenCalledWith('/properties/1');
      expect(result).toEqual(mockProperty);
    });

    it('should handle non-existent property', async () => {
      api.get.mockRejectedValue(new Error('Not found'));

      await expect(propertyService.getPropertyById(999)).rejects.toThrow('Not found');
    });
  });

  describe('createProperty', () => {
    it('should create a new property', async () => {
      const newProperty = { city: 'Paris', country: 'France' };
      const createdProperty = { id: 3, ...newProperty };
      api.post.mockResolvedValue({ data: createdProperty });

      const result = await propertyService.createProperty(newProperty);

      expect(api.post).toHaveBeenCalledWith('/properties', newProperty);
      expect(result).toEqual(createdProperty);
    });

    it('should handle validation errors', async () => {
      api.post.mockRejectedValue(new Error('Validation failed'));

      await expect(propertyService.createProperty({})).rejects.toThrow('Validation failed');
    });
  });

  describe('updateProperty', () => {
    it('should update existing property', async () => {
      const updateData = { city: 'Updated City' };
      const updatedProperty = { id: 1, city: 'Updated City', country: 'USA' };
      api.patch.mockResolvedValue({ data: updatedProperty });

      const result = await propertyService.updateProperty(1, updateData);

      expect(api.patch).toHaveBeenCalledWith('/properties/1', updateData);
      expect(result).toEqual(updatedProperty);
    });

    it('should handle update errors', async () => {
      api.patch.mockRejectedValue(new Error('Update failed'));

      await expect(propertyService.updateProperty(1, {})).rejects.toThrow('Update failed');
    });
  });

  describe('deleteProperty', () => {
    it('should delete property', async () => {
      api.delete.mockResolvedValue({ data: {} });

      const result = await propertyService.deleteProperty(1);

      expect(api.delete).toHaveBeenCalledWith('/properties/1');
      expect(result).toEqual({});
    });

    it('should handle delete errors', async () => {
      api.delete.mockRejectedValue(new Error('Cannot delete'));

      await expect(propertyService.deleteProperty(1)).rejects.toThrow('Cannot delete');
    });
  });
});
