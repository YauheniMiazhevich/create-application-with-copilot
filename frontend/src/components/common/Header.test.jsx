import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { MemoryRouter } from 'react-router-dom';
import Header from './Header.jsx';

const mockLogout = vi.fn();
const mockNavigate = vi.fn();

vi.mock('../../contexts/AuthContext.jsx', () => ({
  useAuth: vi.fn()
}));

vi.mock('react-router-dom', async (importOriginal) => {
  const actual = await importOriginal();
  return {
    ...actual,
    useNavigate: () => mockNavigate
  };
});

import { useAuth } from '../../contexts/AuthContext.jsx';

describe('Header', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should render nothing when user is not authenticated', () => {
    useAuth.mockReturnValue({ user: null, isAuthenticated: false, logout: mockLogout });

    const { container } = render(
      <MemoryRouter>
        <Header />
      </MemoryRouter>
    );

    expect(container.firstChild).toBeNull();
  });

  it('should render the header when user is authenticated', () => {
    useAuth.mockReturnValue({
      user: { email: 'test@example.com', roles: ['User'] },
      isAuthenticated: true,
      logout: mockLogout
    });

    render(
      <MemoryRouter>
        <Header />
      </MemoryRouter>
    );

    expect(screen.getByText('Property Management')).toBeInTheDocument();
  });

  it('should render the username when authenticated', () => {
    useAuth.mockReturnValue({
      user: { email: 'test@example.com', userName: 'testuser', roles: ['User'] },
      isAuthenticated: true,
      logout: mockLogout
    });

    render(
      <MemoryRouter>
        <Header />
      </MemoryRouter>
    );

    expect(screen.getByText('testuser')).toBeInTheDocument();
  });

  it('should render the Logout button when authenticated', () => {
    useAuth.mockReturnValue({
      user: { email: 'test@example.com', roles: ['User'] },
      isAuthenticated: true,
      logout: mockLogout
    });

    render(
      <MemoryRouter>
        <Header />
      </MemoryRouter>
    );

    expect(screen.getByRole('button', { name: /logout/i })).toBeInTheDocument();
  });

  it('should call logout and navigate to /login when Logout is clicked', async () => {
    const user = userEvent.setup();
    useAuth.mockReturnValue({
      user: { email: 'test@example.com', roles: ['User'] },
      isAuthenticated: true,
      logout: mockLogout
    });

    render(
      <MemoryRouter>
        <Header />
      </MemoryRouter>
    );

    await user.click(screen.getByRole('button', { name: /logout/i }));

    expect(mockLogout).toHaveBeenCalledTimes(1);
    expect(mockNavigate).toHaveBeenCalledWith('/login');
  });


  it('should render with proper semantic HTML', () => {
    useAuth.mockReturnValue({
      user: { email: 'test@example.com', roles: ['User'] },
      isAuthenticated: true,
      logout: mockLogout
    });

    const { container } = render(
      <MemoryRouter>
        <Header />
      </MemoryRouter>
    );

    const header = container.querySelector('header');
    expect(header).toBeInTheDocument();
    expect(header).toHaveClass('page-header');
  });
});
