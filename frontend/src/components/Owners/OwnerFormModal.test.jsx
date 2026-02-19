import { describe, it, expect, vi, beforeEach } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import OwnerFormModal from './OwnerFormModal.jsx';

describe('OwnerFormModal', () => {
  const mockPropertyTypes = [
    { id: 1, type: 'House' },
    { id: 2, type: 'Apartment' }
  ];

  beforeEach(() => {
    vi.clearAllMocks();
  });

  it('should not render when isOpen is false', () => {
    render(
      <OwnerFormModal
        isOpen={false}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        mode="create"
      />
    );

    expect(screen.queryByText('Add New Owner')).not.toBeInTheDocument();
  });

  it('should render create form when mode is create', () => {
    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        mode="create"
      />
    );

    expect(screen.getByText('Add New Owner')).toBeInTheDocument();
    expect(screen.getByText('Create Owner')).toBeInTheDocument();
  });

  it('should render edit form when mode is edit', () => {
    const initialData = {
      firstName: 'John',
      lastName: 'Doe',
      email: 'john@example.com',
      phone: '+1 234 567 8900'
    };

    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        initialData={initialData}
        mode="edit"
      />
    );

    expect(screen.getByText('Edit Owner')).toBeInTheDocument();
    expect(screen.getByText('Update Owner')).toBeInTheDocument();
  });

  it('should populate form with initial data in edit mode', () => {
    const initialData = {
      firstName: 'John',
      lastName: 'Doe',
      email: 'john@example.com',
      phone: '+1 234 567 8900',
      address: '123 Main St',
      description: 'Test description'
    };

    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        initialData={initialData}
        mode="edit"
      />
    );

    expect(screen.getByDisplayValue('John')).toBeInTheDocument();
    expect(screen.getByDisplayValue('Doe')).toBeInTheDocument();
    expect(screen.getByDisplayValue('john@example.com')).toBeInTheDocument();
    expect(screen.getByDisplayValue('+1 234 567 8900')).toBeInTheDocument();
  });

  it('should show validation error for empty first name', async () => {
    const user = userEvent.setup();
    const onSubmit = vi.fn();

    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={onSubmit}
        mode="create"
      />
    );

    const submitButton = screen.getByText('Create Owner');
    await user.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText('First name is required')).toBeInTheDocument();
    });
    expect(onSubmit).not.toHaveBeenCalled();
  });

  it('should show validation error for empty last name', async () => {
    const user = userEvent.setup();

    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        mode="create"
      />
    );

    const firstNameInput = screen.getByLabelText(/First Name/);
    await user.type(firstNameInput, 'John');

    const submitButton = screen.getByText('Create Owner');
    await user.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText('Last name is required')).toBeInTheDocument();
    });
  });

  it('should show validation error for invalid email', async () => {
    const user = userEvent.setup();

    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        mode="create"
      />
    );

    const firstNameInput = screen.getByLabelText(/First Name/);
    const lastNameInput = screen.getByLabelText(/Last Name/);
    const emailInput = screen.getByLabelText(/Email/);
    const phoneInput = screen.getByLabelText(/Phone/);

    await user.type(firstNameInput, 'John');
    await user.type(lastNameInput, 'Doe');
    await user.type(emailInput, 'invalid-email');
    await user.type(phoneInput, '1234567890');

    const submitButton = screen.getByText('Create Owner');
    await user.click(submitButton);

    expect(await screen.findByText('Email format is invalid')).toBeInTheDocument();
  });

  it('should show validation error for invalid phone format', async () => {
    const user = userEvent.setup();

    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        mode="create"
      />
    );

    const firstNameInput = screen.getByLabelText(/First Name/);
    const lastNameInput = screen.getByLabelText(/Last Name/);
    const emailInput = screen.getByLabelText(/Email/);
    const phoneInput = screen.getByLabelText(/Phone/);

    await user.type(firstNameInput, 'John');
    await user.type(lastNameInput, 'Doe');
    await user.type(emailInput, 'john@example.com');
    await user.type(phoneInput, 'abc123');

    const submitButton = screen.getByText('Create Owner');
    await user.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText(/Phone can only contain/)).toBeInTheDocument();
    });
  });

  it('should submit valid form data', async () => {
    const user = userEvent.setup();
    const onSubmit = vi.fn();

    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={onSubmit}
        mode="create"
      />
    );

    await user.type(screen.getByLabelText(/First Name/), 'John');
    await user.type(screen.getByLabelText(/Last Name/), 'Doe');
    await user.type(screen.getByLabelText(/Email/), 'john@example.com');
    await user.type(screen.getByLabelText(/Phone/), '+1 234 567 8900');
    await user.type(screen.getByLabelText(/Address/), '123 Main St');
    await user.type(screen.getByLabelText(/Description/), 'Test owner');

    const submitButton = screen.getByText('Create Owner');
    await user.click(submitButton);

    await waitFor(() => {
      expect(onSubmit).toHaveBeenCalledWith({
        firstName: 'John',
        lastName: 'Doe',
        email: 'john@example.com',
        phone: '+1 234 567 8900',
        address: '123 Main St',
        description: 'Test owner'
      });
    });
  });

  it('should call onClose when cancel button is clicked', async () => {
    const user = userEvent.setup();
    const onClose = vi.fn();

    render(
      <OwnerFormModal
        isOpen={true}
        onClose={onClose}
        onSubmit={vi.fn()}
        mode="create"
      />
    );

    const cancelButton = screen.getByText('Cancel');
    await user.click(cancelButton);

    expect(onClose).toHaveBeenCalledTimes(1);
  });

  it('should clear errors when user starts typing', async () => {
    const user = userEvent.setup();

    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        mode="create"
      />
    );

    const submitButton = screen.getByText('Create Owner');
    await user.click(submitButton);

    await waitFor(() => {
      expect(screen.getByText('First name is required')).toBeInTheDocument();
    });

    const firstNameInput = screen.getByLabelText(/First Name/);
    await user.type(firstNameInput, 'J');

    await waitFor(() => {
      expect(screen.queryByText('First name is required')).not.toBeInTheDocument();
    });
  });

  it('should disable form inputs when loading', () => {
    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        mode="create"
        loading={true}
      />
    );

    expect(screen.getByLabelText(/First Name/)).toBeDisabled();
    expect(screen.getByLabelText(/Last Name/)).toBeDisabled();
    expect(screen.getByLabelText(/Email/)).toBeDisabled();
  });

  it('should show saving text when loading', () => {
    render(
      <OwnerFormModal
        isOpen={true}
        onClose={vi.fn()}
        onSubmit={vi.fn()}
        mode="create"
        loading={true}
      />
    );

    expect(screen.getByText('Saving...')).toBeInTheDocument();
  });
});
