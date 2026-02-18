import { describe, it, expect, vi, beforeEach } from 'vitest';
import { renderHook, waitFor, act } from '@testing-library/react';
import useOwners from './useOwners';
import * as ownerService from '../services/ownerService';

vi.mock('../services/ownerService');

describe('useOwners', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('fetchOwners', () => {
    it('should fetch all owners successfully', async () => {
      const mockOwners = [
        { id: 1, firstName: 'John', lastName: 'Doe' },
        { id: 2, firstName: 'Jane', lastName: 'Smith' }
      ];
      ownerService.getAllOwners.mockResolvedValue(mockOwners);

      const { result } = renderHook(() => useOwners());

      await act(async () => {
        const owners = await result.current.fetchOwners();
        expect(owners).toEqual(mockOwners);
      });

      await waitFor(() => {
        expect(result.current.owners).toEqual(mockOwners);
        expect(result.current.loading).toBe(false);
      });
    });

    it('should handle fetch error', async () => {
      ownerService.getAllOwners.mockRejectedValue(new Error('Fetch failed'));

      const { result } = renderHook(() => useOwners());

      try {
        await act(async () => {
          await result.current.fetchOwners();
        });
      } catch (error) {
        // Expected to throw
      }

      await waitFor(() => {
        expect(result.current.error).toBe('Fetch failed');
        expect(result.current.loading).toBe(false);
      });
    });
  });

  describe('fetchOwnerById', () => {
    it('should fetch owner by id', async () => {
      const mockOwner = { id: 1, firstName: 'John', lastName: 'Doe' };
      ownerService.getOwnerById.mockResolvedValue(mockOwner);

      const { result } = renderHook(() => useOwners());

      let owner;
      await act(async () => {
        owner = await result.current.fetchOwnerById(1);
      });

      expect(owner).toEqual(mockOwner);
      expect(ownerService.getOwnerById).toHaveBeenCalledWith(1);
    });

    it('should handle fetch by id error', async () => {
      ownerService.getOwnerById.mockRejectedValue(new Error('Not found'));

      const { result } = renderHook(() => useOwners());

      await expect(act(async () => {
        await result.current.fetchOwnerById(999);
      })).rejects.toThrow();
    });

    it('should set loading state during fetch', async () => {
      ownerService.getOwnerById.mockImplementation(() => 
        new Promise(resolve => setTimeout(() => resolve({}), 100))
      );

      const { result } = renderHook(() => useOwners());

      act(() => {
        result.current.fetchOwnerById(1);
      });

      expect(result.current.loading).toBe(true);

      await waitFor(() => {
        expect(result.current.loading).toBe(false);
      });
    });
  });

  describe('createOwner', () => {
    it('should create owner and add to list', async () => {
      const newOwner = { id: 3, firstName: 'Alice', lastName: 'Johnson' };
      ownerService.createOwner.mockResolvedValue(newOwner);

      const { result } = renderHook(() => useOwners());

      let created;
      await act(async () => {
        created = await result.current.createOwner(newOwner);
      });

      expect(created).toEqual(newOwner);
      await waitFor(() => {
        expect(result.current.owners).toContainEqual(newOwner);
      });
    });

    it('should handle create error', async () => {
      ownerService.createOwner.mockRejectedValue(new Error('Create failed'));

      const { result } = renderHook(() => useOwners());

      await expect(act(async () => {
        await result.current.createOwner({});
      })).rejects.toThrow();
    });
  });

  describe('updateOwner', () => {
    it('should update owner in list', async () => {
      const initialOwners = [
        { id: 1, firstName: 'John', lastName: 'Doe' },
        { id: 2, firstName: 'Jane', lastName: 'Smith' }
      ];
      const updatedOwner = { id: 1, firstName: 'Johnny', lastName: 'Doe' };

      ownerService.getAllOwners.mockResolvedValue(initialOwners);
      ownerService.updateOwner.mockResolvedValue(updatedOwner);

      const { result } = renderHook(() => useOwners());

      await act(async () => {
        await result.current.fetchOwners();
      });

      await act(async () => {
        await result.current.updateOwner(1, { firstName: 'Johnny' });
      });

      await waitFor(() => {
        const updated = result.current.owners.find(o => o.id === 1);
        expect(updated.firstName).toBe('Johnny');
      });
    });

    it('should handle update error', async () => {
      ownerService.updateOwner.mockRejectedValue(new Error('Update failed'));

      const { result } = renderHook(() => useOwners());

      await expect(act(async () => {
        await result.current.updateOwner(1, {});
      })).rejects.toThrow();
    });
  });
});
