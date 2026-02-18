import api from './api';

export const getAllProperties = async () => {
  const response = await api.get('/properties');
  return response.data;
};

export const getPropertyById = async (id) => {
  const response = await api.get(`/properties/${id}`);
  return response.data;
};

export const createProperty = async (data) => {
  const response = await api.post('/properties', data);
  return response.data;
};

export const updateProperty = async (id, data) => {
  const response = await api.patch(`/properties/${id}`, data);
  return response.data;
};

export const deleteProperty = async (id) => {
  const response = await api.delete(`/properties/${id}`);
  return response.data;
};
