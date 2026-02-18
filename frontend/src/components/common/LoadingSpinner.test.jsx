import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import LoadingSpinner from './LoadingSpinner.jsx';

describe('LoadingSpinner', () => {
  it('should render with default message', () => {
    render(<LoadingSpinner />);
    
    expect(screen.getByText('Loading...')).toBeInTheDocument();
    expect(screen.getByRole('status')).toBeInTheDocument();
  });

  it('should render with custom message', () => {
    render(<LoadingSpinner message="Fetching data..." />);
    
    expect(screen.getByText('Fetching data...')).toBeInTheDocument();
  });

  it('should not render message when message is empty', () => {
    render(<LoadingSpinner message="" />);
    
    expect(screen.queryByText('Loading...')).not.toBeInTheDocument();
  });

  it('should have proper ARIA attributes', () => {
    render(<LoadingSpinner message="Please wait" />);
    
    const container = screen.getByRole('status');
    expect(container).toHaveAttribute('aria-live', 'polite');
  });

  it('should render spinner element', () => {
    const { container } = render(<LoadingSpinner />);
    
    const spinner = container.querySelector('.loading-spinner');
    expect(spinner).toBeInTheDocument();
  });
});
