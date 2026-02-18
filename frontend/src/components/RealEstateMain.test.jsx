import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import RealEstateMain from './RealEstateMain.jsx';
import { useAuth } from '../contexts/AuthContext.jsx';
import useProperties from '../hooks/useProperties';
import usePropertyTypes from '../hooks/usePropertyTypes';
import useOwners from '../hooks/useOwners';

vi.mock('../contexts/AuthContext');
vi.mock('../hooks/useProperties');
vi.mock('../hooks/usePropertyTypes');
vi.mock('../hooks/useOwners');
vi.mock('../utils/imageGenerator', () => ({
  generatePropertyImage: vi.fn().mockResolvedValue('http://example.com/image.jpg')
}));

describe('RealEstateMain', () => {
  const mockUser = {
    email: 'user@example.com',
    roles: ['User']
  };

  const mockAdminUser = {
    email: 'admin@example.com',
    roles: ['Admin']
  };

  const mockProperties = [
    {
      id: 1,
      city: 'New York',
      country: 'USA',
      ownerId: 1,
      propertyLength: 2500,
      propertyCost: 500000,
      dateOfBuilding: '2020-01-15T00:00:00Z',
      propertyType: { type: 'House' }
    },
    {
      id: 2,
      city: 'London',
      country: 'UK',
      ownerId: 2,
      propertyLength: 1800,
      propertyCost: 400000,
      dateOfBuilding: '2019-06-10T00:00:00Z',
      propertyType: { type: 'Apartment' }
    }
  ];

  const mockPropertyTypes = [
    { id: 1, type: 'House' },
    { id: 2, type: 'Apartment' }
  ];

  const mockOwner = {
    id: 1,
    firstName: 'John',
    lastName: 'Doe',
    email: 'john@example.com',
    phone: '+1 234 567 8900'
  };

  beforeEach(() => {
    vi.clearAllMocks();
    
    useAuth.mockReturnValue({
      user: mockUser
    });

    useProperties.mockReturnValue({
      properties: mockProperties,
      loading: false,
      error: undefined,
      fetchProperties: vi.fn(),
      createProperty: vi.fn(),
      updateProperty: vi.fn(),
      deleteProperty: vi.fn()
    });

    usePropertyTypes.mockReturnValue({
      propertyTypes: mockPropertyTypes,
      loading: false,
      error: undefined
    });

    useOwners.mockReturnValue({
      owners: [],
      loading: false,
      error: undefined,
      fetchOwnerById: vi.fn().mockResolvedValue(mockOwner),
      createOwner: vi.fn()
    });

    window.alert = vi.fn();
    window.confirm = vi.fn(() => true);
  });

  it('should render header with user information', () => {
    render(<RealEstateMain />);

    expect(screen.getByText('Real Estate Main')).toBeInTheDocument();
    expect(screen.getByText('user@example.com')).toBeInTheDocument();
  });

  it('should fetch properties on mount', () => {
    const fetchProperties = vi.fn();
    useProperties.mockReturnValue({
      properties: [],
      loading: false,
      error: undefined,
      fetchProperties,
      createProperty: vi.fn(),
      updateProperty: vi.fn(),
      deleteProperty: vi.fn()
    });

    render(<RealEstateMain />);

    expect(fetchProperties).toHaveBeenCalled();
  });

  it('should display property cards', async () => {
    render(<RealEstateMain />);

    await waitFor(() => {
      expect(screen.getByText('House')).toBeInTheDocument();
      expect(screen.getByText('Apartment')).toBeInTheDocument();
    });
  });

  it('should show loading spinner when loading', () => {
    useProperties.mockReturnValue({
      properties: [],
      loading: true,
      error: undefined,
      fetchProperties: vi.fn(),
      createProperty: vi.fn(),
      updateProperty: vi.fn(),
      deleteProperty: vi.fn()
    });

    render(<RealEstateMain />);

    expect(screen.getByText('Loading properties...')).toBeInTheDocument();
  });

  it('should show error message when there is an error', () => {
    const fetchProperties = vi.fn();
    useProperties.mockReturnValue({
      properties: [],
      loading: false,
      error: 'Failed to load properties',
      fetchProperties,
      createProperty: vi.fn(),
      updateProperty: vi.fn(),
      deleteProperty: vi.fn()
    });

    render(<RealEstateMain />);

    expect(screen.getByText('Failed to load properties')).toBeInTheDocument();
  });

  it('should show empty state when no properties', () => {
    useProperties.mockReturnValue({
      properties: [],
      loading: false,
      error: undefined,
      fetchProperties: vi.fn(),
      createProperty: vi.fn(),
      updateProperty: vi.fn(),
      deleteProperty: vi.fn()
    });

    render(<RealEstateMain />);

    expect(screen.getByText('No Properties Found')).toBeInTheDocument();
  });

  it('should not show sidebar for regular users', () => {
    render(<RealEstateMain />);

    expect(screen.queryByText('Admin Actions')).not.toBeInTheDocument();
  });

  it('should show sidebar for admin users', () => {
    useAuth.mockReturnValue({
      user: mockAdminUser
    });

    render(<RealEstateMain />);

    expect(screen.getByText('Admin Actions')).toBeInTheDocument();
    expect(screen.getByText('Add Property')).toBeInTheDocument();
    expect(screen.getByText('Add Owner')).toBeInTheDocument();
  });

  it('should not show delete buttons for regular users', async () => {
    render(<RealEstateMain />);

    await waitFor(() => {
      expect(screen.queryByLabelText('Delete property')).not.toBeInTheDocument();
    });
  });

  it('should show delete buttons for admin users', async () => {
    useAuth.mockReturnValue({
      user: mockAdminUser
    });

    render(<RealEstateMain />);

    await waitFor(() => {
      const deleteButtons = screen.getAllByLabelText('Delete property');
      expect(deleteButtons).toHaveLength(2);
    });
  });

  it('should open property details modal when property is clicked', async () => {
    const user = userEvent.setup();
    const fetchOwnerById = vi.fn().mockResolvedValue(mockOwner);
    
    useOwners.mockReturnValue({
      owners: [],
      loading: false,
      error: undefined,
      fetchOwnerById,
      createOwner: vi.fn()
    });

    render(<RealEstateMain />);

    await waitFor(() => {
      expect(screen.getByText('House')).toBeInTheDocument();
    });

    const propertyCards = screen.getAllByRole('button', { name: /Property:/ });
    await user.click(propertyCards[0]);

    await waitFor(() => {
      expect(fetchOwnerById).toHaveBeenCalledWith(1);
      expect(screen.getByText('Property Details')).toBeInTheDocument();
    });
  });

  it('should delete property when delete button is clicked and confirmed', async () => {
    const user = userEvent.setup();
    const deleteProperty = vi.fn().mockResolvedValue(true);
    
    useAuth.mockReturnValue({
      user: mockAdminUser
    });

    useProperties.mockReturnValue({
      properties: mockProperties,
      loading: false,
      error: undefined,
      fetchProperties: vi.fn(),
      createProperty: vi.fn(),
      updateProperty: vi.fn(),
      deleteProperty
    });

    render(<RealEstateMain />);

    await waitFor(() => {
      expect(screen.getByText('House')).toBeInTheDocument();
    });

    const deleteButtons = screen.getAllByLabelText('Delete property');
    await user.click(deleteButtons[0]);

    expect(window.confirm).toHaveBeenCalled();
    
    await waitFor(() => {
      expect(deleteProperty).toHaveBeenCalledWith(1);
    });
  });

  it('should open add property modal when add property button is clicked', async () => {
    const user = userEvent.setup();
    
    useAuth.mockReturnValue({
      user: mockAdminUser
    });

    render(<RealEstateMain />);

    const addPropertyButton = screen.getByText('Add Property');
    await user.click(addPropertyButton);

    await waitFor(() => {
      expect(screen.getByText('Add New Property')).toBeInTheDocument();
    });
  });

  it('should open add owner modal when add owner button is clicked', async () => {
    const user = userEvent.setup();
    
    useAuth.mockReturnValue({
      user: mockAdminUser
    });

    render(<RealEstateMain />);

    const addOwnerButton = screen.getByText('Add Owner');
    await user.click(addOwnerButton);

    await waitFor(() => {
      expect(screen.getByText('Add New Owner')).toBeInTheDocument();
    });
  });

  it('should show empty state with add button for admin', () => {
    useAuth.mockReturnValue({
      user: mockAdminUser
    });

    useProperties.mockReturnValue({
      properties: [],
      loading: false,
      error: undefined,
      fetchProperties: vi.fn(),
      createProperty: vi.fn(),
      updateProperty: vi.fn(),
      deleteProperty: vi.fn()
    });

    render(<RealEstateMain />);

    expect(screen.getByText('No Properties Found')).toBeInTheDocument();
    expect(screen.getByText('Add Your First Property')).toBeInTheDocument();
  });

  it('should render properties in grid layout', async () => {
    const { container } = render(<RealEstateMain />);

    await waitFor(() => {
      const grid = container.querySelector('.properties-grid');
      expect(grid).toBeInTheDocument();
    });
  });

  it('should handle error when fetching owner fails', async () => {
    const user = userEvent.setup();
    const fetchOwnerById = vi.fn().mockRejectedValue(new Error('Owner not found'));
    
    useOwners.mockReturnValue({
      owners: [],
      loading: false,
      error: undefined,
      fetchOwnerById,
      createOwner: vi.fn()
    });

    const consoleError = vi.spyOn(console, 'error').mockImplementation(() => {});

    render(<RealEstateMain />);

    await waitFor(() => {
      expect(screen.getByText('House')).toBeInTheDocument();
    });

    const propertyCards = screen.getAllByRole('button', { name: /Property:/ });
    await user.click(propertyCards[0]);

    await waitFor(() => {
      expect(consoleError).toHaveBeenCalled();
    });

    consoleError.mockRestore();
  });
});
