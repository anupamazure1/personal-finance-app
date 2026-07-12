import apiClient from './api'

export const transactionService = {
  getTransactionsByAccount: (accountId, bank) => 
    apiClient.get(`/transactions/account/${accountId}/${bank}`),
  uploadHdfcStatement: (accountId, file) => {
    const formData = new FormData()
    formData.append('accountId', accountId)
    formData.append('file', file)
    return apiClient.post('/transactions/hdfc', formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
  },
  uploadIciciStatement: (accountId, file) => {
    const formData = new FormData()
    formData.append('accountId', accountId)
    formData.append('file', file)
    return apiClient.post('/transactions/icici', formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
  },
  uploadKotakStatement: (accountId, file) => {
    const formData = new FormData()
    formData.append('accountId', accountId)
    formData.append('file', file)
    return apiClient.post('/transactions/kotak', formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
  },
  uploadYesBankStatement: (accountId, file) => {
    const formData = new FormData()
    formData.append('accountId', accountId)
    formData.append('file', file)
    return apiClient.post('/transactions/yesbank', formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    })
  }
}
