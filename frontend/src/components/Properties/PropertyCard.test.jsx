import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import PropertyCard from './PropertyCard.jsx';
import * as imageGenerator from '../../utils/imageGenerator';

vi.mock('../../utils/imageGenerator');

describe('PropertyCard', () => {
  const mockProperty = {
    id: 1,
    city: 'New York',
    country: 'USA',
    propertyLength: 2500,
    propertyCost: 500000,
    dateOfBuilding: '2020-01-15T00:00:00Z',
    propertyType: {
      type: 'House'
    }
  };

  beforeEach(() => {
    vi.clearAllMocks();
    imageGenerator.generatePropertyImage.mockResolvedValue('http://example.com/image.jpg');
  });

  it('should render property information', async () => {
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={vi.fn()}
        showDeleteButton={false}
      />
    );

    await waitFor(() => {
      expect(screen.getByText('House')).toBeInTheDocument();
      expect(screen.getByText(/New York, USA/)).toBeInTheDocument();
      expect(screen.getByText(/2500 sq ft/)).toBeInTheDocument();
    });
  });

  it('should format currency correctly', async () => {
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={vi.fn()}
        showDeleteButton={false}
      />
    );

    await waitFor(() => {
      expect(screen.getByText(/\$500,000/)).toBeInTheDocument();
    });
  });

  it('should call onClick when card is clicked', async () => {
    const user = userEvent.setup();
    const onClick = vi.fn();
    
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={onClick}
        onDelete={vi.fn()}
        showDeleteButton={false}
      />
    );

    const card = screen.getByRole('button');
    await user.click(card);

    expect(onClick).toHaveBeenCalledTimes(1);
  });

  it('should call onClick when Enter key is pressed', async () => {
    const user = userEvent.setup();
    const onClick = vi.fn();
    
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={onClick}
        onDelete={vi.fn()}
        showDeleteButton={false}
      />
    );

    const card = screen.getByRole('button');
    card.focus();
    await user.keyboard('{Enter}');

    expect(onClick).toHaveBeenCalled();
  });

  it('should show delete button when showDeleteButton is true', () => {
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={vi.fn()}
        showDeleteButton={true}
      />
    );

    expect(screen.getByLabelText('Delete property')).toBeInTheDocument();
  });

  it('should not show delete button when showDeleteButton is false', () => {
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={vi.fn()}
        showDeleteButton={false}
      />
    );

    expect(screen.queryByLabelText('Delete property')).not.toBeInTheDocument();
  });

  it('should show confirmation dialog before deleting', async () => {
    const user = userEvent.setup();
    const onDelete = vi.fn();
    window.confirm = vi.fn(() => false);
    
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={onDelete}
        showDeleteButton={true}
      />
    );

    const deleteButton = screen.getByLabelText('Delete property');
    await user.click(deleteButton);

    expect(window.confirm).toHaveBeenCalled();
    expect(onDelete).not.toHaveBeenCalled();
  });

  it('should call onDelete when confirmed', async () => {
    const user = userEvent.setup();
    const onDelete = vi.fn();
    window.confirm = vi.fn(() => true);
    
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={onDelete}
        showDeleteButton={true}
      />
    );

    const deleteButton = screen.getByLabelText('Delete property');
    await user.click(deleteButton);

    expect(onDelete).toHaveBeenCalledWith(1);
  });

  it('should not trigger onClick when delete button is clicked', async () => {
    const user = userEvent.setup();
    const onClick = vi.fn();
    const onDelete = vi.fn();
    window.confirm = vi.fn(() => true);
    
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={onClick}
        onDelete={onDelete}
        showDeleteButton={true}
      />
    );

    const deleteButton = screen.getByLabelText('Delete property');
    await user.click(deleteButton);

    expect(onDelete).toHaveBeenCalled();
    expect(onClick).not.toHaveBeenCalled();
  });

  it('should generate image for property type', async () => {
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={vi.fn()}
        showDeleteButton={false}
      />
    );

    await waitFor(() => {
      expect(imageGenerator.generatePropertyImage).toHaveBeenCalledWith('House');
    });
  });

  it('should show loading state while image is generating', () => {
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={vi.fn()}
        showDeleteButton={false}
      />
    );

    expect(screen.getByText('Loading...')).toBeInTheDocument();
  });

  it('should have proper ARIA label', () => {
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={vi.fn()}
        showDeleteButton={false}
      />
    );

    const card = screen.getByRole('button');
    expect(card).toHaveAttribute('aria-label', 'Property: House in New York, USA');
  });

  it('should be keyboard accessible', () => {
    render(
      <PropertyCard 
        property={mockProperty}
        onClick={vi.fn()}
        onDelete={vi.fn()}
        showDeleteButton={false}
      />
    );

    const card = screen.getByRole('button');
    expect(card).toHaveAttribute('tabIndex', '0');
  });
});
