import apiClient from './api'

export const dashboardService = {
  getDashboardSummary: () => apiClient.get('/dashboard/summary'),
  getFamilyNetWorth: (familyId) => apiClient.get(`/dashboard/family/${familyId}/net-worth`),
  getMemberNetWorth: (memberId) => apiClient.get(`/dashboard/member/${memberId}/net-worth`)
}
