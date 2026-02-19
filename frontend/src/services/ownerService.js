import api from './api';

export const getAllOwners = async () => {
  const response = await api.get('/owners');
  return response.data;
};

export const getOwnerById = async (id) => {
  const response = await api.get(`/owners/${id}`);
  return response.data;
};

export const createOwner = async (data) => {
  const response = await api.post('/owners', data);
  return response.data;
};

export const updateOwner = async (id, data) => {
  const response = await api.patch(`/owners/${id}`, data);
  return response.data;
};

export const deleteOwner = async (id) => {
  const response = await api.delete(`/owners/${id}`);
  return response.data;
};
