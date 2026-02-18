import { describe, it, expect, vi, beforeEach } from 'vitest';
import { renderHook, waitFor } from '@testing-library/react';
import usePropertyTypes from './usePropertyTypes';
import * as propertyTypeService from '../services/propertyTypeService';

vi.mock('../services/propertyTypeService');

describe('usePropertyTypes', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should fetch property types on mount', async () => {
    const mockPropertyTypes = [
      { id: 1, type: 'House' },
      { id: 2, type: 'Apartment' },
      { id: 3, type: 'Villa' }
    ];
    propertyTypeService.getAllPropertyTypes.mockResolvedValue(mockPropertyTypes);

    const { result } = renderHook(() => usePropertyTypes());

    expect(result.current.loading).toBe(true);

    await waitFor(() => {
      expect(result.current.propertyTypes).toEqual(mockPropertyTypes);
      expect(result.current.loading).toBe(false);
      expect(result.current.error).toBe(undefined);
    });

    expect(propertyTypeService.getAllPropertyTypes).toHaveBeenCalledTimes(1);
  });

  it('should handle fetch error', async () => {
    const errorMessage = 'Failed to fetch property types';
    propertyTypeService.getAllPropertyTypes.mockRejectedValue(new Error(errorMessage));

    const { result } = renderHook(() => usePropertyTypes());

    await waitFor(() => {
      expect(result.current.error).toBe(errorMessage);
      expect(result.current.loading).toBe(false);
      expect(result.current.propertyTypes).toEqual([]);
    });
  });

  it('should not fetch multiple times on re-render', async () => {
    const mockPropertyTypes = [{ id: 1, type: 'House' }];
    propertyTypeService.getAllPropertyTypes.mockResolvedValue(mockPropertyTypes);

    const { result, rerender } = renderHook(() => usePropertyTypes());

    await waitFor(() => {
      expect(result.current.propertyTypes).toEqual(mockPropertyTypes);
    });

    rerender();

    expect(propertyTypeService.getAllPropertyTypes).toHaveBeenCalledTimes(1);
  });

  it('should handle empty property types array', async () => {
    propertyTypeService.getAllPropertyTypes.mockResolvedValue([]);

    const { result } = renderHook(() => usePropertyTypes());

    await waitFor(() => {
      expect(result.current.propertyTypes).toEqual([]);
      expect(result.current.loading).toBe(false);
    });
  });

  it('should provide fetchPropertyTypes function', async () => {
    const mockPropertyTypes = [{ id: 1, type: 'House' }];
    propertyTypeService.getAllPropertyTypes.mockResolvedValue(mockPropertyTypes);

    const { result } = renderHook(() => usePropertyTypes());

    expect(typeof result.current.fetchPropertyTypes).toBe('function');

    await waitFor(() => {
      expect(result.current.loading).toBe(false);
    });
  });
});
