<template>
  <div class="transactions">
    <div class="header">
      <router-link to="/accounts" class="btn btn-secondary">← Back</router-link>
      <h1>{{ bankName }} Transactions</h1>
      <div class="upload-section">
        <label class="file-input">
          <input 
            type="file" 
            @change="handleFileUpload"
            :accept="getFileExtensions()"
            style="display: none"
          />
          <span class="btn btn-success">📤 Upload Statement</span>
        </label>
      </div>
    </div>

    <div v-if="uploading" class="alert alert-info">
      <div class="spinner" style="display: inline-block; margin-right: 1rem;"></div>
      Uploading and processing statement...
    </div>

    <div v-if="uploadSuccess" class="alert alert-success">
      {{ uploadSuccess }}
    </div>

    <div v-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      <p>Loading transactions...</p>
    </div>

    <div v-else>
      <div v-if="transactions.length === 0" class="text-center">
        <p>No transactions found. Upload a bank statement to get started!</p>
      </div>

      <div v-else class="transactions-container">
        <div class="summary">
          <p><strong>Total Transactions:</strong> {{ transactions.length }}</p>
          <p><strong>Total Debits:</strong> ₹ {{ formatCurrency(totalDebits) }}</p>
          <p><strong>Total Credits:</strong> ₹ {{ formatCurrency(totalCredits) }}</p>
        </div>

        <div class="transactions-table-container">
          <table class="transactions-table">
            <thead>
              <tr>
                <th>Date</th>
                <th>Description</th>
                <th>Amount</th>
                <th>Balance</th>
                <th>Reference</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="transaction in sortedTransactions" :key="transaction.id" :class="{ debit: transaction.amount < 0, credit: transaction.amount > 0 }">
                <td>{{ formatDate(transaction.transactionDate) }}</td>
                <td>{{ transaction.description }}</td>
                <td :class="{ 'text-danger': transaction.amount < 0, 'text-success': transaction.amount > 0 }">
                  {{ transaction.amount < 0 ? '-' : '+' }} ₹ {{ formatCurrency(Math.abs(transaction.amount)) }}
                </td>
                <td>₹ {{ formatCurrency(transaction.balance) }}</td>
                <td>{{ transaction.referenceNumber || transaction.chequeNumber || '-' }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { transactionService } from '../services/transactionService'
import { accountService } from '../services/accountService'

export default {
  name: 'Transactions',
  data() {
    return {
      transactions: [],
      accountId: null,
      bankName: '',
      loading: true,
      uploading: false,
      error: null,
      uploadSuccess: null
    }
  },
  computed: {
    totalDebits() {
      return this.transactions
        .filter(t => t.amount < 0)
        .reduce((sum, t) => sum + Math.abs(t.amount), 0)
    },
    totalCredits() {
      return this.transactions
        .filter(t => t.amount > 0)
        .reduce((sum, t) => sum + t.amount, 0)
    },
    sortedTransactions() {
      return this.transactions.sort((a, b) => 
        new Date(b.transactionDate) - new Date(a.transactionDate)
      )
    }
  },
  mounted() {
    this.accountId = this.$route.params.accountId
    this.bankName = this.$route.params.bank
    this.loadTransactions()
  },
  methods: {
    async loadTransactions() {
      try {
        this.loading = true
        const response = await transactionService.getTransactionsByAccount(
          this.accountId,
          this.bankName
        )
        this.transactions = response.data
      } catch (err) {
        this.error = 'Failed to load transactions'
        console.error(err)
      } finally {
        this.loading = false
      }
    },
    async handleFileUpload(event) {
      const file = event.target.files[0]
      if (!file) return

      try {
        this.uploading = true
        this.error = null
        this.uploadSuccess = null

        let response
        const bankNameLower = this.bankName.toLowerCase()
        
        if (bankNameLower === 'hdfc') {
          response = await transactionService.uploadHdfcStatement(this.accountId, file)
        } else if (bankNameLower === 'icici') {
          response = await transactionService.uploadIciciStatement(this.accountId, file)
        } else if (bankNameLower === 'kotak') {
          response = await transactionService.uploadKotakStatement(this.accountId, file)
        } else if (bankNameLower === 'yes' || bankNameLower === 'yesbank') {
          response = await transactionService.uploadYesBankStatement(this.accountId, file)
        }

        this.uploadSuccess = response.data.message
        await this.loadTransactions()
        
        // Reset file input
        event.target.value = ''
      } catch (err) {
        this.error = err.response?.data?.message || 'Failed to upload statement'
        console.error(err)
      } finally {
        this.uploading = false
      }
    },
    getFileExtensions() {
      const bankNameLower = this.bankName.toLowerCase()
      if (bankNameLower === 'hdfc' || bankNameLower === 'kotak') {
        return '.csv'
      } else if (bankNameLower === 'icici' || bankNameLower === 'yesbank') {
        return '.xlsx,.csv'
      }
      return '.csv,.xlsx'
    },
    formatCurrency(value) {
      return new Intl.NumberFormat('en-IN', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      }).format(value)
    },
    formatDate(dateString) {
      return new Date(dateString).toLocaleDateString('en-IN')
    }
  }
}
</script>

<style scoped>
.transactions {
  padding: 2rem 0;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  gap: 1rem;
  flex-wrap: wrap;
}

h1 {
  flex: 1;
  color: #2c3e50;
  min-width: 200px;
}

.btn-secondary {
  background-color: #95a5a6;
  color: white;
  border-color: #95a5a6;
  white-space: nowrap;
}

.btn-secondary:hover {
  background-color: #7f8c8d;
}

.file-input {
  cursor: pointer;
}

.summary {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  margin-bottom: 1.5rem;
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
}

.summary p {
  margin: 0;
}

.transactions-table-container {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow-x: auto;
}

.transactions-table {
  width: 100%;
  border-collapse: collapse;
}

.transactions-table th {
  background-color: #f8f9fa;
  padding: 1rem;
  text-align: left;
  border-bottom: 2px solid #ecf0f1;
  font-weight: 600;
  color: #2c3e50;
}

.transactions-table td {
  padding: 1rem;
  border-bottom: 1px solid #ecf0f1;
}

.transactions-table tbody tr:hover {
  background-color: #f8f9fa;
}

.debit {
  background-color: #fff5f5;
}

.credit {
  background-color: #f5fff5;
}

.text-danger {
  color: #dc3545;
  font-weight: bold;
}

.text-success {
  color: #28a745;
  font-weight: bold;
}

.text-center {
  text-align: center;
  padding: 2rem;
  color: #7f8c8d;
}
</style>
