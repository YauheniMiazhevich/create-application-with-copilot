import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import PropertyDetailsModal from './PropertyDetailsModal.jsx';
import * as imageGenerator from '../../utils/imageGenerator';

vi.mock('../../utils/imageGenerator');

describe('PropertyDetailsModal', () => {
  const mockProperty = {
    id: 1,
    city: 'New York',
    country: 'USA',
    street: '123 Main St',
    zipCode: '10001',
    propertyLength: 2500,
    propertyCost: 500000,
    description: 'Beautiful property',
    dateOfBuilding: '2020-01-15T00:00:00Z',
    propertyType: {
      type: 'House'
    }
  };

  const mockOwner = {
    firstName: 'John',
    lastName: 'Doe',
    email: 'john@example.com',
    phone: '+1 234 567 8900',
    address: '456 Oak Ave',
    description: 'Property owner',
    isCompanyContact: false
  };

  beforeEach(() => {
    vi.clearAllMocks();
    imageGenerator.generatePropertyImage.mockResolvedValue('http://example.com/image.jpg');
  });

  it('should not render when isOpen is false', () => {
    render(
      <PropertyDetailsModal
        isOpen={false}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
      />
    );

    expect(screen.queryByText('Property Details')).not.toBeInTheDocument();
  });

  it('should render when isOpen is true', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
      />
    );

    expect(screen.getByText('Property Details')).toBeInTheDocument();
  });

  it('should display property information', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
      />
    );

    expect(screen.getByText('House')).toBeInTheDocument();
    expect(screen.getByText('New York')).toBeInTheDocument();
    expect(screen.getByText('USA')).toBeInTheDocument();
    expect(screen.getByText('123 Main St')).toBeInTheDocument();
    expect(screen.getByText('10001')).toBeInTheDocument();
  });

  it('should display owner information', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
      />
    );

    expect(screen.getByText('John Doe')).toBeInTheDocument();
    expect(screen.getByText('john@example.com')).toBeInTheDocument();
    expect(screen.getByText('+1 234 567 8900')).toBeInTheDocument();
  });

  it('should show loading spinner when loading', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={undefined}
        loading={true}
      />
    );

    expect(screen.getByText('Loading property details...')).toBeInTheDocument();
  });

  it('should show edit button when showEditButton is true', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
        showEditButton={true}
      />
    );

    expect(screen.getByLabelText('Edit property details')).toBeInTheDocument();
  });

  it('should not show edit button when showEditButton is false', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
        showEditButton={false}
      />
    );

    expect(screen.queryByLabelText('Edit property details')).not.toBeInTheDocument();
  });

  it('should call onEdit when edit button is clicked', async () => {
    const user = userEvent.setup();
    const onEdit = vi.fn();

    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
        onEdit={onEdit}
        showEditButton={true}
      />
    );

    const editButton = screen.getByLabelText('Edit property details');
    await user.click(editButton);

    expect(onEdit).toHaveBeenCalledTimes(1);
  });

  it('should display formatted currency', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
      />
    );

    expect(screen.getByText(/\$500,000/)).toBeInTheDocument();
  });

  it('should display formatted date', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
      />
    );

    expect(screen.getByText(/January/)).toBeInTheDocument();
    expect(screen.getByText(/2020/)).toBeInTheDocument();
  });

  it('should show owner not available when owner is null', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={undefined}
        loading={false}
      />
    );

    expect(screen.getByText('Owner information not available.')).toBeInTheDocument();
  });

  it('should display property with two column layout', () => {
    const { container } = render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
      />
    );

    const grid = container.querySelector('.property-details-grid');
    expect(grid).toBeInTheDocument();
  });

  it('should display company contact status', () => {
    const ownerWithCompany = { ...mockOwner, isCompanyContact: true };

    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={ownerWithCompany}
      />
    );

    expect(screen.getByText('Yes')).toBeInTheDocument();
  });

  it('should render email and phone as links', () => {
    render(
      <PropertyDetailsModal
        isOpen={true}
        onClose={vi.fn()}
        property={mockProperty}
        owner={mockOwner}
      />
    );

    const emailLink = screen.getByText('john@example.com');
    expect(emailLink).toHaveAttribute('href', 'mailto:john@example.com');

    const phoneLink = screen.getByText('+1 234 567 8900');
    expect(phoneLink).toHaveAttribute('href', 'tel:+1 234 567 8900');
  });
});
