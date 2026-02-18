import { describe, it, expect, vi, beforeEach } from 'vitest';

// Mock the api module before importing authService
vi.mock('./api', () => ({
  default: {
    post: vi.fn(),
    get: vi.fn()
  }
}));

import authService from './authService';
import api from './api';

const TOKEN_KEY = 'auth_token';
const USER_KEY = 'auth_user';

describe('authService', () => {
  beforeEach(() => {
    localStorage.clear();
    vi.clearAllMocks();
  });

  describe('logout', () => {
    it('should remove auth_token from localStorage', async () => {
      localStorage.setItem(TOKEN_KEY, 'test-token');
      api.post.mockResolvedValueOnce({});

      await authService.logout();

      expect(localStorage.getItem(TOKEN_KEY)).toBeNull();
    });

    it('should remove auth_user from localStorage', async () => {
      localStorage.setItem(USER_KEY, JSON.stringify({ email: 'test@example.com' }));
      api.post.mockResolvedValueOnce({});

      await authService.logout();

      expect(localStorage.getItem(USER_KEY)).toBeNull();
    });

    it('should call POST /auth/logout', async () => {
      api.post.mockResolvedValueOnce({});

      await authService.logout();

      expect(api.post).toHaveBeenCalledWith('/auth/logout');
    });

    it('should still clear localStorage even if backend logout request fails', async () => {
      localStorage.setItem(TOKEN_KEY, 'test-token');
      localStorage.setItem(USER_KEY, JSON.stringify({ email: 'test@example.com' }));
      api.post.mockRejectedValueOnce(new Error('Network error'));

      await authService.logout();

      expect(localStorage.getItem(TOKEN_KEY)).toBeNull();
      expect(localStorage.getItem(USER_KEY)).toBeNull();
    });
  });

  describe('getToken', () => {
    it('should return the stored token', () => {
      localStorage.setItem(TOKEN_KEY, 'my-token');
      expect(authService.getToken()).toBe('my-token');
    });

    it('should return null when no token is stored', () => {
      expect(authService.getToken()).toBeNull();
    });
  });

  describe('getStoredUser', () => {
    it('should return the parsed stored user', () => {
      const user = { email: 'test@example.com', roles: ['User'] };
      localStorage.setItem(USER_KEY, JSON.stringify(user));
      expect(authService.getStoredUser()).toEqual(user);
    });

    it('should return null when no user is stored', () => {
      expect(authService.getStoredUser()).toBeNull();
    });
  });

  describe('isAuthenticated', () => {
    it('should return true when a token exists', () => {
      localStorage.setItem(TOKEN_KEY, 'some-token');
      expect(authService.isAuthenticated()).toBe(true);
    });

    it('should return false when no token exists', () => {
      expect(authService.isAuthenticated()).toBe(false);
    });
  });
});
