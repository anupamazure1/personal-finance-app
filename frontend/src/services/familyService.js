import apiClient from './api'

export const familyService = {
  getAllFamilies: () => apiClient.get('/families'),
  getFamilyById: (id) => apiClient.get(`/families/${id}`),
  createFamily: (data) => apiClient.post('/families', data),
  updateFamily: (id, data) => apiClient.put(`/families/${id}`, data),
  deleteFamily: (id) => apiClient.delete(`/families/${id}`)
}
