import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import PropertyFormModal from './PropertyFormModal.jsx';

describe('PropertyFormModal', () => {
  const mockPropertyTypes = [
    { id: 1, type: 'House' },
    { id: 2, type: 'Apartment' },
    { id: 3, type: 'Commercial' }
  ];

  const mockProperty = {
    id: 1,
    country: 'USA',
    city: 'New York',
    street: '5th Avenue',
    zipCode: '10001',
    propertyTypeId: 1,
    propertyLength: 2500,
    propertyCost: 500000,
    description: 'Beautiful property',
    dateOfBuilding: '2020-01-15T00:00:00Z',
    ownerId: 1
  };

  const mockOnClose = vi.fn();
  const mockOnSubmit = vi.fn();

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should not render when not open', () => {
    render(
      <PropertyFormModal
        isOpen={false}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    expect(screen.queryByText('Add New Property')).not.toBeInTheDocument();
  });

  it('should render add property form when no property provided', () => {
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    expect(screen.getByText('Add New Property')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Create Property' })).toBeInTheDocument();
  });

  it('should render edit property form when property provided', () => {
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        initialData={mockProperty}
        mode="edit"
        propertyTypes={mockPropertyTypes}
      />
    );

    expect(screen.getByText('Edit Property')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: 'Update Property' })).toBeInTheDocument();
  });

  it('should populate form with property data in edit mode', () => {
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        initialData={mockProperty}
        mode="edit"
        propertyTypes={mockPropertyTypes}
      />
    );

    expect(screen.getByLabelText(/Country/i).value).toBe('USA');
    expect(screen.getByLabelText(/City/i).value).toBe('New York');
    expect(screen.getByLabelText(/Street/i).value).toBe('5th Avenue');
    expect(screen.getByLabelText(/Zip Code/i).value).toBe('10001');
    expect(screen.getByLabelText(/Property Type/i).value).toBe('1');
    expect(screen.getByLabelText(/Property Length.*sq ft/i).value).toBe('2500');
    expect(screen.getByLabelText(/Property Cost/i).value).toBe('500000');
    expect(screen.getByLabelText(/Description/i).value).toBe('Beautiful property');
    expect(screen.getByLabelText(/Date of Building/i).value).toBe('2020-01-15');
    expect(screen.getByLabelText(/Owner ID/i).value).toBe('1');
  });

  it('should show validation error for empty country', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('Country is required')).toBeInTheDocument();
    expect(mockOnSubmit).not.toHaveBeenCalled();
  });

  it('should show validation error for country exceeding max length', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    const countryInput = screen.getByLabelText(/Country/i);
    await user.type(countryInput, 'A'.repeat(101));

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('Country must be 100 characters or less')).toBeInTheDocument();
  });

  it('should show validation error for empty city', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    await user.type(screen.getByLabelText(/Country/i), 'USA');

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('City is required')).toBeInTheDocument();
  });

  it('should show validation error for city exceeding max length', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    const cityInput = screen.getByLabelText(/City/i);
    await user.type(cityInput, 'B'.repeat(101));

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('City must be 100 characters or less')).toBeInTheDocument();
  });

  it('should show validation error for invalid property length', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    await user.type(screen.getByLabelText(/Country/i), 'USA');
    await user.type(screen.getByLabelText(/City/i), 'New York');
    await user.type(screen.getByLabelText(/Property Length.*sq ft/i), '0');

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('Property length must be greater than 0')).toBeInTheDocument();
  });

  it('should show validation error for invalid property cost', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    await user.type(screen.getByLabelText(/Country/i), 'USA');
    await user.type(screen.getByLabelText(/City/i), 'New York');
    await user.type(screen.getByLabelText(/Property Cost/i), '-100');

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('Property cost must be at least 0')).toBeInTheDocument();
  });

  it('should show validation error for future date of building', async () => {
    const user = userEvent.setup();
    const futureDate = new Date();
    futureDate.setFullYear(futureDate.getFullYear() + 1);
    const futureDateStr = futureDate.toISOString().split('T')[0];
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    await user.type(screen.getByLabelText(/Country/i), 'USA');
    await user.type(screen.getByLabelText(/City/i), 'New York');
    
    const dateInput = screen.getByLabelText(/Date of Building/i);
    await user.clear(dateInput);
    await user.type(dateInput, futureDateStr);

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('Date of building cannot be in the future')).toBeInTheDocument();
  });

  it('should show validation error for missing property type', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    await user.type(screen.getByLabelText(/Country/i), 'USA');
    await user.type(screen.getByLabelText(/City/i), 'New York');

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('Property type is required')).toBeInTheDocument();
  });

  it('should show validation error for missing owner ID', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    await user.type(screen.getByLabelText(/Country/i), 'USA');
    await user.type(screen.getByLabelText(/City/i), 'New York');
    
    const typeSelect = screen.getByLabelText(/Property Type/i);
    await user.selectOptions(typeSelect, '1');

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('Owner ID is required')).toBeInTheDocument();
  });

  it('should submit valid form data', async () => {
    const user = userEvent.setup();
    mockOnSubmit.mockResolvedValue(undefined);
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    await user.type(screen.getByLabelText(/Country/i), 'USA');
    await user.type(screen.getByLabelText(/City/i), 'New York');
    await user.type(screen.getByLabelText(/Street/i), '5th Avenue');
    await user.type(screen.getByLabelText(/Zip Code/i), '10001');
    
    const typeSelect = screen.getByLabelText(/Property Type/i);
    await user.selectOptions(typeSelect, '1');
    
    await user.type(screen.getByLabelText(/Property Length.*sq ft/i), '2500');
    await user.type(screen.getByLabelText(/Property Cost/i), '500000');
    await user.type(screen.getByLabelText(/Description/i), 'Beautiful property');
    await user.type(screen.getByLabelText(/Date of Building/i), '2020-01-15');
    await user.type(screen.getByLabelText(/Owner ID/i), '1');

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    await waitFor(() => {
      expect(mockOnSubmit).toHaveBeenCalledWith({
        country: 'USA',
        city: 'New York',
        street: '5th Avenue',
        zipCode: '10001',
        propertyTypeId: 1,
        propertyLength: 2500,
        propertyCost: 500000,
        description: 'Beautiful property',
        dateOfBuilding: '2020-01-15',
        ownerId: 1
      });
    });
  });

  it('should clear errors when user starts typing', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    expect(await screen.findByText('Country is required')).toBeInTheDocument();

    const countryInput = screen.getByLabelText(/Country/i);
    await user.type(countryInput, 'USA');

    expect(screen.queryByText('Country is required')).not.toBeInTheDocument();
  });

  it('should disable submit button when loading', () => {
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
        loading={true}
      />
    );

    const submitButton = screen.getByRole('button', { name: /Saving/ });
    expect(submitButton).toBeDisabled();
  });

  it('should call onClose when cancel button is clicked', async () => {
    const user = userEvent.setup();
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    const cancelButton = screen.getByRole('button', { name: 'Cancel' });
    await user.click(cancelButton);

    expect(mockOnClose).toHaveBeenCalled();
  });

  it('should populate property type dropdown', () => {
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    const typeSelect = screen.getByLabelText(/Property Type/i);
    const options = Array.from(typeSelect.options).map(opt => opt.text);

    expect(options).toContain('House');
    expect(options).toContain('Apartment');
    expect(options).toContain('Commercial');
  });

  it('should handle submission error gracefully', async () => {
    const user = userEvent.setup();
    mockOnSubmit.mockRejectedValue(new Error('Submission failed'));
    
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    await user.type(screen.getByLabelText(/Country/i), 'USA');
    await user.type(screen.getByLabelText(/City/i), 'New York');
    await user.type(screen.getByLabelText(/Property Length.*sq ft/i), '100');
    await user.type(screen.getByLabelText(/Property Cost/i), '1000');
    await user.selectOptions(screen.getByLabelText(/Property Type/i), '1');
    await user.type(screen.getByLabelText(/Owner ID/i), '1');

    const submitButton = screen.getByRole('button', { name: 'Create Property' });
    await user.click(submitButton);

    await waitFor(() => {
      expect(mockOnSubmit).toHaveBeenCalled();
    });
  });

  it('should render all required form fields', () => {
    render(
      <PropertyFormModal
        isOpen={true}
        onClose={mockOnClose}
        onSubmit={mockOnSubmit}
        propertyTypes={mockPropertyTypes}
      />
    );

    expect(screen.getByLabelText(/Country/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/City/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Street/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Zip Code/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Property Type/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Property Length.*sq ft/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Property Cost/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Description/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Date of Building/i)).toBeInTheDocument();
    expect(screen.getByLabelText(/Owner ID/i)).toBeInTheDocument();
  });
});
