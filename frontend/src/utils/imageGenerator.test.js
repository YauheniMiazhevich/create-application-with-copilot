import { describe, it, expect, vi, beforeEach } from 'vitest';
import { generatePropertyImage, preloadPropertyImages, clearImageCache } from './imageGenerator';

describe('imageGenerator', () => {
  beforeEach(() => {
    clearImageCache();
    vi.clearAllMocks();
    // Clear window.puter before each test
    delete window.puter;
  });

  describe('generatePropertyImage', () => {
    it('should return placeholder image when propertyTypeName is empty', async () => {
      const result = await generatePropertyImage('');
      expect(result).toContain('data:image/svg+xml');
      expect(result).toContain('Property');
    });

    it('should return placeholder image when propertyTypeName is undefined', async () => {
      const result = await generatePropertyImage(undefined);
      expect(result).toContain('data:image/svg+xml');
    });

    it('should return placeholder when Puter AI is not available', async () => {
      window.puter = undefined;
      const result = await generatePropertyImage('House');
      expect(result).toContain('data:image/svg+xml');
    });

    it('should generate image using Puter AI when available', async () => {
      const mockImageData = 'http://example.com/image.jpg';
      window.puter = {
        ai: {
          image: vi.fn().mockResolvedValue(mockImageData)
        }
      };

      const result = await generatePropertyImage('Apartment');
      
      expect(window.puter.ai.image).toHaveBeenCalledWith({
        prompt: 'Property Apartment',
        size: '400x300'
      });
      expect(result).toBe(mockImageData);
    });

    it('should cache generated images', async () => {
      const mockImageData = 'http://example.com/image.jpg';
      window.puter = {
        ai: {
          image: vi.fn().mockResolvedValue(mockImageData)
        }
      };

      const result1 = await generatePropertyImage('Villa');
      const result2 = await generatePropertyImage('Villa');
      
      expect(window.puter.ai.image).toHaveBeenCalledTimes(1);
      expect(result1).toBe(result2);
    });

    it('should return placeholder on Puter AI error', async () => {
      window.puter = {
        ai: {
          image: vi.fn().mockRejectedValue(new Error('API Error'))
        }
      };

      const result = await generatePropertyImage('Condo');
      expect(result).toContain('data:image/svg+xml');
    });

    it('should generate different images for different property types', async () => {
      const mockImageData1 = 'http://example.com/house.jpg';
      const mockImageData2 = 'http://example.com/apartment.jpg';
      
      window.puter = {
        ai: {
          image: vi.fn()
            .mockResolvedValueOnce(mockImageData1)
            .mockResolvedValueOnce(mockImageData2)
        }
      };

      const result1 = await generatePropertyImage('House');
      const result2 = await generatePropertyImage('Apartment');
      
      expect(result1).toBe(mockImageData1);
      expect(result2).toBe(mockImageData2);
      expect(window.puter.ai.image).toHaveBeenCalledTimes(2);
    });
  });

  describe('preloadPropertyImages', () => {
    it('should preload images for multiple property types', async () => {
      const mockImageData = 'http://example.com/image.jpg';
      window.puter = {
        ai: {
          image: vi.fn().mockResolvedValue(mockImageData)
        }
      };

      const propertyTypes = ['House', 'Apartment', 'Villa'];
      await preloadPropertyImages(propertyTypes);

      expect(window.puter.ai.image).toHaveBeenCalledTimes(3);
    });

    it('should handle errors gracefully during preload', async () => {
      window.puter = {
        ai: {
          image: vi.fn()
            .mockResolvedValueOnce('http://example.com/image1.jpg')
            .mockRejectedValueOnce(new Error('Error'))
            .mockResolvedValueOnce('http://example.com/image2.jpg')
        }
      };

      const propertyTypes = ['House', 'Apartment', 'Villa'];
      await expect(preloadPropertyImages(propertyTypes)).resolves.not.toThrow();
    });

    it('should handle empty array', async () => {
      await expect(preloadPropertyImages([])).resolves.not.toThrow();
    });
  });

  describe('clearImageCache', () => {
    it('should clear the image cache', async () => {
      const mockImageData = 'http://example.com/image.jpg';
      window.puter = {
        ai: {
          image: vi.fn().mockResolvedValue(mockImageData)
        }
      };

      await generatePropertyImage('House');
      expect(window.puter.ai.image).toHaveBeenCalledTimes(1);

      clearImageCache();

      await generatePropertyImage('House');
      expect(window.puter.ai.image).toHaveBeenCalledTimes(2);
    });
  });
});
