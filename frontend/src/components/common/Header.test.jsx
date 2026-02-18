import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import Header from './Header.jsx';

describe('Header', () => {
  it('should render page title', () => {
    render(<Header user={undefined} />);
    
    expect(screen.getByText('Real Estate Main')).toBeInTheDocument();
  });

  it('should render user email when user is provided', () => {
    const user = {
      email: 'test@example.com',
      roles: ['User']
    };
    
    render(<Header user={user} />);
    
    expect(screen.getByText('test@example.com')).toBeInTheDocument();
  });

  it('should render user roles as badges', () => {
    const user = {
      email: 'admin@example.com',
      roles: ['Admin', 'User']
    };
    
    render(<Header user={user} />);
    
    expect(screen.getByText('Admin')).toBeInTheDocument();
    expect(screen.getByText('User')).toBeInTheDocument();
  });

  it('should not crash when user has no roles', () => {
    const user = {
      email: 'test@example.com',
      roles: undefined
    };
    
    render(<Header user={user} />);
    
    expect(screen.getByText('test@example.com')).toBeInTheDocument();
  });

  it('should not render user info when user is not provided', () => {
    render(<Header user={undefined} />);
    
    expect(screen.queryByText('@')).not.toBeInTheDocument();
  });

  it('should render with proper semantic HTML', () => {
    const user = { email: 'test@example.com', roles: ['User'] };
    const { container } = render(<Header user={user} />);
    
    const header = container.querySelector('header');
    expect(header).toBeInTheDocument();
    expect(header).toHaveClass('page-header');
  });

  it('should render multiple roles correctly', () => {
    const user = {
      email: 'superadmin@example.com',
      roles: ['Admin', 'User', 'Manager']
    };
    
    render(<Header user={user} />);
    
    const badges = screen.getAllByText((content, element) => {
      return element.className === 'page-header-role-badge';
    });
    
    expect(badges).toHaveLength(3);
  });
});
