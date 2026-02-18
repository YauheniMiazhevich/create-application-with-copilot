import { describe, it, expect, vi, beforeEach } from 'vitest';
import * as propertyTypeService from './propertyTypeService';
import api from './api';

vi.mock('./api');

describe('propertyTypeService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('getAllPropertyTypes', () => {
    it('should fetch all property types', async () => {
      const mockPropertyTypes = [
        { id: 1, type: 'House' },
        { id: 2, type: 'Apartment' },
        { id: 3, type: 'Villa' }
      ];
      api.get.mockResolvedValue({ data: mockPropertyTypes });

      const result = await propertyTypeService.getAllPropertyTypes();

      expect(api.get).toHaveBeenCalledWith('/propertytypes');
      expect(result).toEqual(mockPropertyTypes);
    });

    it('should handle API errors', async () => {
      api.get.mockRejectedValue(new Error('API Error'));

      await expect(propertyTypeService.getAllPropertyTypes()).rejects.toThrow('API Error');
    });

    it('should return empty array when no property types exist', async () => {
      api.get.mockResolvedValue({ data: [] });

      const result = await propertyTypeService.getAllPropertyTypes();

      expect(result).toEqual([]);
    });
  });
});
