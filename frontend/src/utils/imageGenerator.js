// Placeholder image data URL (simple gradient)
const PLACEHOLDER_IMAGE = 'data:image/svg+xml,%3Csvg width="400" height="300" xmlns="http://www.w3.org/2000/svg"%3E%3Cdefs%3E%3ClinearGradient id="grad" x1="0%25" y1="0%25" x2="100%25" y2="100%25"%3E%3Cstop offset="0%25" style="stop-color:%23667eea;stop-opacity:1" /%3E%3Cstop offset="100%25" style="stop-color:%23764ba2;stop-opacity:1" /%3E%3C/linearGradient%3E%3C/defs%3E%3Crect width="400" height="300" fill="url(%23grad)" /%3E%3Ctext x="50%25" y="50%25" font-family="Arial" font-size="24" fill="white" text-anchor="middle" dominant-baseline="middle"%3EProperty%3C/text%3E%3C/svg%3E';

// Cache for generated images to avoid regenerating the same property type
const imageCache = new Map();

/**
 * Generates a property image using Puter.js AI image generation
 * @param {string} propertyTypeName - The name of the property type (e.g., "House", "Apartment")
 * @returns {Promise<string>} - Returns the image URL or data URL
 */
export const generatePropertyImage = async (propertyTypeName) => {
  if (!propertyTypeName) {
    return PLACEHOLDER_IMAGE;
  }

  // Check cache first
  if (imageCache.has(propertyTypeName)) {
    return imageCache.get(propertyTypeName);
  }

  try {
    // Access Puter from window object (loaded via CDN script tag)
    const puter = window.puter;
    
    // Initialize Puter if needed
    if (!puter || !puter.ai) {
      console.warn('Puter AI not available, using placeholder image');
      return PLACEHOLDER_IMAGE;
    }

    const prompt = `Property ${propertyTypeName}`;
    
    // Generate image using Puter AI
    const imageData = await puter.ai.image({
      prompt: prompt,
      size: '400x300'
    });

    // Cache the result
    imageCache.set(propertyTypeName, imageData);
    
    return imageData;
  } catch (error) {
    console.error('Error generating image with Puter.js:', error);
    // Return placeholder on error
    return PLACEHOLDER_IMAGE;
  }
};

/**
 * Preload images for common property types
 * @param {Array<string>} propertyTypeNames - Array of property type names to preload
 */
export const preloadPropertyImages = async (propertyTypeNames) => {
  const promises = propertyTypeNames.map(name => generatePropertyImage(name));
  await Promise.allSettled(promises);
};

/**
 * Clear the image cache
 */
export const clearImageCache = () => {
  imageCache.clear();
};
