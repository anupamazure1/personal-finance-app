<template>
  <div class="family-detail">
    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      <p>Loading family details...</p>
    </div>

    <div v-else-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <div v-else>
      <div class="header">
        <h1>{{ family.name }}</h1>
        <router-link to="/families" class="btn btn-secondary">← Back</router-link>
      </div>

      <div class="tabs">
        <button 
          @click="activeTab = 'members'"
          :class="{ active: activeTab === 'members' }"
          class="tab-btn"
        >
          Members
        </button>
        <button 
          @click="activeTab = 'accounts'"
          :class="{ active: activeTab === 'accounts' }"
          class="tab-btn"
        >
          Accounts
        </button>
      </div>

      <div v-if="activeTab === 'members'" class="tab-content">
        <div class="section-header">
          <h2>Family Members</h2>
          <button @click="showAddMember = true" class="btn btn-primary">+ Add Member</button>
        </div>

        <div v-if="showAddMember" class="form-card">
          <h3>Add Member</h3>
          <form @submit.prevent="addMember">
            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Name *</label>
                <input v-model="newMember.name" class="form-control" required />
              </div>
              <div class="form-group">
                <label class="form-label">Email</label>
                <input v-model="newMember.email" type="email" class="form-control" />
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Phone</label>
                <input v-model="newMember.phone" class="form-control" />
              </div>
              <div class="form-group">
                <label class="form-label">Relationship</label>
                <input v-model="newMember.relationship" class="form-control" />
              </div>
            </div>
            <div class="form-actions">
              <button type="submit" class="btn btn-success">Add</button>
              <button type="button" @click="showAddMember = false" class="btn btn-secondary">Cancel</button>
            </div>
          </form>
        </div>

        <div v-if="family.members.length === 0" class="text-center">
          <p>No members in this family yet.</p>
        </div>

        <div v-else class="members-grid">
          <div v-for="member in family.members" :key="member.id" class="member-card">
            <h3>{{ member.name }}</h3>
            <p v-if="member.email"><strong>Email:</strong> {{ member.email }}</p>
            <p v-if="member.phone"><strong>Phone:</strong> {{ member.phone }}</p>
            <p v-if="member.relationship"><strong>Relationship:</strong> {{ member.relationship }}</p>
            <p class="accounts-info">{{ member.accounts.length }} account(s)</p>
          </div>
        </div>
      </div>

      <div v-if="activeTab === 'accounts'" class="tab-content">
        <div class="section-header">
          <h2>Bank Accounts</h2>
          <button @click="showAddAccount = true" class="btn btn-primary">+ Add Account</button>
        </div>

        <div v-if="showAddAccount" class="form-card">
          <h3>Add Bank Account</h3>
          <form @submit.prevent="addAccount">
            <div class="form-group">
              <label class="form-label">Member *</label>
              <select v-model="newAccount.memberId" class="form-control" required>
                <option value="">Select a member</option>
                <option v-for="member in family.members" :key="member.id" :value="member.id">
                  {{ member.name }}
                </option>
              </select>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Bank Name *</label>
                <select v-model="newAccount.bankName" class="form-control" required>
                  <option value="">Select a bank</option>
                  <option value="HDFC">HDFC Bank</option>
                  <option value="ICICI">ICICI Bank</option>
                  <option value="Kotak">Kotak Bank</option>
                  <option value="Yes Bank">Yes Bank</option>
                </select>
              </div>
              <div class="form-group">
                <label class="form-label">Account Type *</label>
                <select v-model="newAccount.accountType" class="form-control" required>
                  <option value="">Select type</option>
                  <option value="Savings">Savings</option>
                  <option value="Current">Current</option>
                  <option value="FixedDeposit">Fixed Deposit</option>
                  <option value="MutualFund">Mutual Fund</option>
                  <option value="Trading">Trading</option>
                  <option value="Demat">Demat</option>
                </select>
              </div>
            </div>
            <div class="form-row">
              <div class="form-group">
                <label class="form-label">Account Number *</label>
                <input v-model="newAccount.accountNumber" class="form-control" required />
              </div>
              <div class="form-group">
                <label class="form-label">Current Balance *</label>
                <input v-model.number="newAccount.currentBalance" type="number" class="form-control" required />
              </div>
            </div>
            <div class="form-actions">
              <button type="submit" class="btn btn-success">Add Account</button>
              <button type="button" @click="showAddAccount = false" class="btn btn-secondary">Cancel</button>
            </div>
          </form>
        </div>

        <div v-if="allAccounts.length === 0" class="text-center">
          <p>No accounts in this family yet.</p>
        </div>

        <div v-else class="accounts-table-container">
          <table class="accounts-table">
            <thead>
              <tr>
                <th>Member</th>
                <th>Bank</th>
                <th>Type</th>
                <th>Account #</th>
                <th>Balance</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="account in allAccounts" :key="account.id">
                <td>{{ getMemberName(account.memberId) }}</td>
                <td>{{ account.bankName }}</td>
                <td>{{ account.accountType }}</td>
                <td>{{ account.accountNumber }}</td>
                <td>₹ {{ formatCurrency(account.currentBalance) }}</td>
                <td>
                  <router-link :to="`/transactions/${account.id}/${account.bankName.toLowerCase()}`" class="btn btn-primary" style="font-size: 0.8rem;">
                    Transactions
                  </router-link>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { familyService } from '../services/familyService'
import { accountService } from '../services/accountService'

export default {
  name: 'FamilyDetail',
  data() {
    return {
      family: { name: '', members: [] },
      allAccounts: [],
      newMember: { name: '', email: '', phone: '', relationship: '' },
      newAccount: { memberId: '', bankName: '', accountType: '', accountNumber: '', currentBalance: 0 },
      showAddMember: false,
      showAddAccount: false,
      activeTab: 'members',
      loading: true,
      error: null
    }
  },
  mounted() {
    this.loadFamilyDetails()
  },
  methods: {
    async loadFamilyDetails() {
      try {
        this.loading = true
        const familyId = this.$route.params.id
        const response = await familyService.getFamilyById(familyId)
        this.family = response.data
        await this.loadAccounts()
      } catch (err) {
        this.error = 'Failed to load family details'
        console.error(err)
      } finally {
        this.loading = false
      }
    },
    async loadAccounts() {
      try {
        const response = await accountService.getAllAccounts()
        this.allAccounts = response.data.filter(a => 
          this.family.members.some(m => m.id === a.memberId)
        )
      } catch (err) {
        console.error(err)
      }
    },
    async addMember() {
      try {
        // This would require a family member service
        // For now, reload the family details
        this.showAddMember = false
        this.newMember = { name: '', email: '', phone: '', relationship: '' }
        // await this.loadFamilyDetails()
      } catch (err) {
        this.error = 'Failed to add member'
      }
    },
    async addAccount() {
      try {
        const payload = {
          memberId: parseInt(this.newAccount.memberId),
          bankName: this.newAccount.bankName,
          accountType: this.newAccount.accountType,
          accountNumber: this.newAccount.accountNumber,
          currentBalance: this.newAccount.currentBalance
        }
        await accountService.createAccount(payload)
        this.showAddAccount = false
        this.newAccount = { memberId: '', bankName: '', accountType: '', accountNumber: '', currentBalance: 0 }
        await this.loadAccounts()
      } catch (err) {
        this.error = 'Failed to add account'
        console.error(err)
      }
    },
    getMemberName(memberId) {
      const member = this.family.members.find(m => m.id === memberId)
      return member ? member.name : 'Unknown'
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
.family-detail {
  padding: 2rem 0;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

h1 {
  color: #2c3e50;
}

.tabs {
  display: flex;
  gap: 0;
  border-bottom: 2px solid #ecf0f1;
  margin-bottom: 2rem;
}

.tab-btn {
  padding: 1rem 1.5rem;
  background: none;
  border: none;
  border-bottom: 3px solid transparent;
  cursor: pointer;
  color: #7f8c8d;
  font-weight: 500;
  transition: all 0.3s;
}

.tab-btn:hover {
  color: #3498db;
}

.tab-btn.active {
  color: #3498db;
  border-bottom-color: #3498db;
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.form-card {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  margin-bottom: 2rem;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.form-actions {
  display: flex;
  gap: 1rem;
  margin-top: 1.5rem;
}

.btn-secondary {
  background-color: #95a5a6;
  color: white;
  border-color: #95a5a6;
}

.btn-secondary:hover {
  background-color: #7f8c8d;
}

.members-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 1rem;
}

.member-card {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.member-card h3 {
  color: #2c3e50;
  margin-bottom: 0.75rem;
}

.member-card p {
  margin: 0.5rem 0;
  color: #7f8c8d;
  font-size: 0.9rem;
}

.accounts-info {
  color: #3498db;
  font-weight: bold;
  margin-top: 1rem;
}

.accounts-table-container {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  overflow-x: auto;
}

.accounts-table {
  width: 100%;
  border-collapse: collapse;
}

.accounts-table th {
  background-color: #f8f9fa;
  padding: 1rem;
  text-align: left;
  border-bottom: 2px solid #ecf0f1;
  font-weight: 600;
  color: #2c3e50;
}

.accounts-table td {
  padding: 1rem;
  border-bottom: 1px solid #ecf0f1;
}

.accounts-table tbody tr:hover {
  background-color: #f8f9fa;
}

.text-center {
  text-align: center;
  padding: 2rem;
  color: #7f8c8d;
}
</style>
