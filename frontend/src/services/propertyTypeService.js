import api from './api';

export const getAllPropertyTypes = async () => {
  const response = await api.get('/propertytypes');
  return response.data;
};
