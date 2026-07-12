import apiClient from './api'

export const accountService = {
  getAllAccounts: () => apiClient.get('/accounts'),
  getAccountById: (id) => apiClient.get(`/accounts/${id}`),
  getAccountsByMember: (memberId) => apiClient.get(`/accounts/member/${memberId}`),
  createAccount: (data) => apiClient.post('/accounts', data),
  updateAccount: (id, data) => apiClient.put(`/accounts/${id}`, data),
  deleteAccount: (id) => apiClient.delete(`/accounts/${id}`)
}
