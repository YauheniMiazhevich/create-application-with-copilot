import { describe, it, expect, vi } from 'vitest';
import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Sidebar from './Sidebar.jsx';

describe('Sidebar', () => {
  it('should not render when user is not admin', () => {
    const { container } = render(
      <Sidebar 
        isAdmin={false}
        onAddProperty={vi.fn()}
        onAddOwner={vi.fn()}
      />
    );
    
    expect(container.firstChild).toBeNull();
  });

  it('should render when user is admin', () => {
    render(
      <Sidebar 
        isAdmin={true}
        onAddProperty={vi.fn()}
        onAddOwner={vi.fn()}
      />
    );
    
    expect(screen.getByText('Admin Actions')).toBeInTheDocument();
  });

  it('should render Add Property button for admin', () => {
    render(
      <Sidebar 
        isAdmin={true}
        onAddProperty={vi.fn()}
        onAddOwner={vi.fn()}
      />
    );
    
    expect(screen.getByText('Add Property')).toBeInTheDocument();
  });

  it('should render Add Owner button for admin', () => {
    render(
      <Sidebar 
        isAdmin={true}
        onAddProperty={vi.fn()}
        onAddOwner={vi.fn()}
      />
    );
    
    expect(screen.getByText('Add Owner')).toBeInTheDocument();
  });

  it('should call onAddProperty when Add Property button is clicked', async () => {
    const user = userEvent.setup();
    const onAddProperty = vi.fn();
    
    render(
      <Sidebar 
        isAdmin={true}
        onAddProperty={onAddProperty}
        onAddOwner={vi.fn()}
      />
    );
    
    const addPropertyButton = screen.getByText('Add Property');
    await user.click(addPropertyButton);
    
    expect(onAddProperty).toHaveBeenCalledTimes(1);
  });

  it('should call onAddOwner when Add Owner button is clicked', async () => {
    const user = userEvent.setup();
    const onAddOwner = vi.fn();
    
    render(
      <Sidebar 
        isAdmin={true}
        onAddProperty={vi.fn()}
        onAddOwner={onAddOwner}
      />
    );
    
    const addOwnerButton = screen.getByText('Add Owner');
    await user.click(addOwnerButton);
    
    expect(onAddOwner).toHaveBeenCalledTimes(1);
  });

  it('should have proper ARIA labels', () => {
    render(
      <Sidebar 
        isAdmin={true}
        onAddProperty={vi.fn()}
        onAddOwner={vi.fn()}
      />
    );
    
    const nav = screen.getByRole('navigation');
    expect(nav).toHaveAttribute('aria-label', 'Main navigation');
    
    const addPropertyButton = screen.getByLabelText('Add new property');
    expect(addPropertyButton).toBeInTheDocument();
    
    const addOwnerButton = screen.getByLabelText('Add new owner');
    expect(addOwnerButton).toBeInTheDocument();
  });

  it('should render with proper semantic HTML', () => {
    const { container } = render(
      <Sidebar 
        isAdmin={true}
        onAddProperty={vi.fn()}
        onAddOwner={vi.fn()}
      />
    );
    
    const nav = container.querySelector('nav');
    expect(nav).toBeInTheDocument();
    expect(nav).toHaveClass('sidebar');
    
    const list = container.querySelector('ul');
    expect(list).toBeInTheDocument();
    expect(list).toHaveClass('sidebar-menu');
  });

  it('should render button icons', () => {
    const { container } = render(
      <Sidebar 
        isAdmin={true}
        onAddProperty={vi.fn()}
        onAddOwner={vi.fn()}
      />
    );
    
    const icons = container.querySelectorAll('.sidebar-button-icon');
    expect(icons).toHaveLength(2);
    expect(icons[0]).toHaveTextContent('🏠');
    expect(icons[1]).toHaveTextContent('👤');
  });
});
