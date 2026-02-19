import { describe, it, expect, vi, beforeEach, afterEach } from 'vitest';
import { render, screen, fireEvent } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import Modal from './Modal.jsx';

describe('Modal', () => {
  beforeEach(() => {
    document.body.style.overflow = '';
  });

  afterEach(() => {
    document.body.style.overflow = '';
  });

  it('should not render when isOpen is false', () => {
    render(
      <Modal isOpen={false} onClose={vi.fn()} title="Test Modal">
        <div>Content</div>
      </Modal>
    );
    
    expect(screen.queryByText('Test Modal')).not.toBeInTheDocument();
  });

  it('should render when isOpen is true', () => {
    render(
      <Modal isOpen={true} onClose={vi.fn()} title="Test Modal">
        <div>Modal Content</div>
      </Modal>
    );
    
    expect(screen.getByText('Test Modal')).toBeInTheDocument();
    expect(screen.getByText('Modal Content')).toBeInTheDocument();
  });

  it('should call onClose when close button is clicked', async () => {
    const user = userEvent.setup();
    const onClose = vi.fn();
    
    render(
      <Modal isOpen={true} onClose={onClose} title="Test Modal">
        <div>Content</div>
      </Modal>
    );
    
    const closeButton = screen.getByLabelText('Close modal');
    await user.click(closeButton);
    
    expect(onClose).toHaveBeenCalledTimes(1);
  });

  it('should call onClose when overlay is clicked', () => {
    const onClose = vi.fn();
    
    render(
      <Modal isOpen={true} onClose={onClose} title="Test Modal">
        <div>Content</div>
      </Modal>
    );
    
    const overlay = screen.getByTestId('modal-overlay');
    // Use fireEvent to click directly on the overlay element
    fireEvent.click(overlay);
    
    expect(onClose).toHaveBeenCalled();
  });

  it('should not close when clicking inside modal content', async () => {
    const user = userEvent.setup();
    const onClose = vi.fn();
    
    render(
      <Modal isOpen={true} onClose={onClose} title="Test Modal">
        <div>Modal Content</div>
      </Modal>
    );
    
    const content = screen.getByText('Modal Content');
    await user.click(content);
    
    expect(onClose).not.toHaveBeenCalled();
  });

  it('should have proper ARIA attributes', () => {
    render(
      <Modal isOpen={true} onClose={vi.fn()} title="Test Modal">
        <div>Content</div>
      </Modal>
    );
    
    const dialog = screen.getByRole('dialog');
    expect(dialog).toHaveAttribute('aria-modal', 'true');
    expect(dialog).toHaveAttribute('aria-labelledby', 'modal-title');
  });

  it('should render with different sizes', () => {
    const { container, rerender } = render(
      <Modal isOpen={true} onClose={vi.fn()} title="Small Modal" size="small">
        <div>Content</div>
      </Modal>
    );
    
    let modalContent = container.querySelector('.modal-content');
    expect(modalContent).toHaveClass('modal-small');
    
    rerender(
      <Modal isOpen={true} onClose={vi.fn()} title="Large Modal" size="large">
        <div>Content</div>
      </Modal>
    );
    
    modalContent = container.querySelector('.modal-content');
    expect(modalContent).toHaveClass('modal-large');
  });

  it('should default to medium size', () => {
    const { container } = render(
      <Modal isOpen={true} onClose={vi.fn()} title="Test Modal">
        <div>Content</div>
      </Modal>
    );
    
    const modalContent = container.querySelector('.modal-content');
    expect(modalContent).toHaveClass('modal-medium');
  });

  it('should prevent body scroll when open', () => {
    const { unmount } = render(
      <Modal isOpen={true} onClose={vi.fn()} title="Test Modal">
        <div>Content</div>
      </Modal>
    );
    
    expect(document.body.style.overflow).toBe('hidden');
    
    unmount();
    
    expect(document.body.style.overflow).toBe('unset');
  });
});
