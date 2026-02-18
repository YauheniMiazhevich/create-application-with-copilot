import { render, screen } from '@testing-library/react';
import App from './App.jsx';

test('renders login page', () => {
  render(<App />);
  const loginHeading = screen.getByRole('heading', { name: /login/i });
  expect(loginHeading).toBeInTheDocument();
});
