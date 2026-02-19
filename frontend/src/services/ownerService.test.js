import { describe, it, expect, vi, beforeEach } from 'vitest';
import * as ownerService from './ownerService';
import api from './api';

vi.mock('./api');

describe('ownerService', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  describe('getAllOwners', () => {
    it('should fetch all owners', async () => {
      const mockOwners = [
        { id: 1, firstName: 'John', lastName: 'Doe' },
        { id: 2, firstName: 'Jane', lastName: 'Smith' }
      ];
      api.get.mockResolvedValue({ data: mockOwners });

      const result = await ownerService.getAllOwners();

      expect(api.get).toHaveBeenCalledWith('/owners');
      expect(result).toEqual(mockOwners);
    });
  });

  describe('getOwnerById', () => {
    it('should fetch owner by id', async () => {
      const mockOwner = { id: 1, firstName: 'John', lastName: 'Doe' };
      api.get.mockResolvedValue({ data: mockOwner });

      const result = await ownerService.getOwnerById(1);

      expect(api.get).toHaveBeenCalledWith('/owners/1');
      expect(result).toEqual(mockOwner);
    });
  });

  describe('createOwner', () => {
    it('should create a new owner', async () => {
      const newOwner = { firstName: 'Alice', lastName: 'Johnson' };
      const createdOwner = { id: 3, ...newOwner };
      api.post.mockResolvedValue({ data: createdOwner });

      const result = await ownerService.createOwner(newOwner);

      expect(api.post).toHaveBeenCalledWith('/owners', newOwner);
      expect(result).toEqual(createdOwner);
    });
  });

  describe('updateOwner', () => {
    it('should update existing owner', async () => {
      const updateData = { firstName: 'Updated Name' };
      const updatedOwner = { id: 1, firstName: 'Updated Name', lastName: 'Doe' };
      api.patch.mockResolvedValue({ data: updatedOwner });

      const result = await ownerService.updateOwner(1, updateData);

      expect(api.patch).toHaveBeenCalledWith('/owners/1', updateData);
      expect(result).toEqual(updatedOwner);
    });
  });

  describe('deleteOwner', () => {
    it('should delete owner', async () => {
      api.delete.mockResolvedValue({ data: {} });

      const result = await ownerService.deleteOwner(1);

      expect(api.delete).toHaveBeenCalledWith('/owners/1');
      expect(result).toEqual({});
    });
  });
});
