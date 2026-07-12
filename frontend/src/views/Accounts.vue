<template>
  <div class="accounts">
    <h1>Bank Accounts</h1>

    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      <p>Loading accounts...</p>
    </div>

    <div v-else-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <div v-else>
      <div v-if="accounts.length === 0" class="text-center">
        <p>No accounts found. <router-link to="/families">Create one</router-link></p>
      </div>

      <div v-else class="accounts-grid">
        <div v-for="account in accounts" :key="account.id" class="account-card">
          <div class="card-header">
            <h3>{{ account.bankName }}</h3>
            <span class="account-type">{{ account.accountType }}</span>
          </div>
          <div class="card-body">
            <p><strong>Account #:</strong> {{ maskAccountNumber(account.accountNumber) }}</p>
            <p v-if="account.accountHolder"><strong>Holder:</strong> {{ account.accountHolder }}</p>
            <p class="balance"><strong>Balance:</strong> ₹ {{ formatCurrency(account.currentBalance) }}</p>
          </div>
          <div class="card-footer">
            <router-link 
              :to="`/transactions/${account.id}/${account.bankName.toLowerCase()}`" 
              class="btn btn-primary"
            >
              View Transactions
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { accountService } from '../services/accountService'

export default {
  name: 'Accounts',
  data() {
    return {
      accounts: [],
      loading: true,
      error: null
    }
  },
  mounted() {
    this.loadAccounts()
  },
  methods: {
    async loadAccounts() {
      try {
        this.loading = true
        const response = await accountService.getAllAccounts()
        this.accounts = response.data
      } catch (err) {
        this.error = 'Failed to load accounts'
        console.error(err)
      } finally {
        this.loading = false
      }
    },
    maskAccountNumber(accountNumber) {
      if (accountNumber.length <= 4) return accountNumber
      return '*'.repeat(accountNumber.length - 4) + accountNumber.slice(-4)
    },
    formatCurrency(value) {
      return new Intl.NumberFormat('en-IN', {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      }).format(value)
    }
  }
}
</script>

<style scoped>
.accounts {
  padding: 2rem 0;
}

h1 {
  margin-bottom: 2rem;
  color: #2c3e50;
}

.accounts-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

.account-card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  display: flex;
  flex-direction: column;
}

.card-header {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.card-header h3 {
  margin: 0;
  font-size: 1.3rem;
}

.account-type {
  background: rgba(255, 255, 255, 0.2);
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.85rem;
}

.card-body {
  padding: 1.5rem;
  flex-grow: 1;
}

.card-body p {
  margin: 0.5rem 0;
  color: #7f8c8d;
}

.balance {
  font-size: 1.3rem;
  color: #27ae60;
  margin-top: 1rem;
}

.card-footer {
  padding: 1rem 1.5rem;
  background-color: #f8f9fa;
  border-top: 1px solid #ecf0f1;
}

.text-center {
  text-align: center;
  padding: 2rem;
  color: #7f8c8d;
}

.text-center a {
  color: #3498db;
  text-decoration: none;
}

.text-center a:hover {
  text-decoration: underline;
}
</style>
