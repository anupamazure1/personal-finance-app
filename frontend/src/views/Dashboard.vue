<template>
  <div class="dashboard">
    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      <p>Loading dashboard...</p>
    </div>

    <div v-else-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <div v-else>
      <h1>Dashboard</h1>
      
      <div class="stats-grid">
        <div class="stat-card">
          <h3>Total Net Worth</h3>
          <p class="stat-value">₹ {{ formatCurrency(summary.totalFamilyNetWorth) }}</p>
        </div>
        <div class="stat-card">
          <h3>Families</h3>
          <p class="stat-value">{{ summary.totalFamilies }}</p>
        </div>
        <div class="stat-card">
          <h3>Members</h3>
          <p class="stat-value">{{ summary.totalMembers }}</p>
        </div>
        <div class="stat-card">
          <h3>Accounts</h3>
          <p class="stat-value">{{ summary.totalAccounts }}</p>
        </div>
      </div>

      <div class="families-section">
        <h2>Family Overview</h2>
        <div v-if="summary.familiesNetWorth.length === 0" class="text-center">
          <p>No families found. <router-link to="/families">Create one now</router-link></p>
        </div>
        <div v-else class="families-grid">
          <div v-for="family in summary.familiesNetWorth" :key="family.familyId" class="family-card">
            <h3>{{ family.familyName }}</h3>
            <p class="family-networth">Net Worth: ₹ {{ formatCurrency(family.netWorth) }}</p>
            
            <div class="members-list">
              <h4>Members:</h4>
              <ul>
                <li v-for="member in family.members" :key="member.memberId">
                  <strong>{{ member.memberName }}</strong>
                  <span class="member-worth">₹ {{ formatCurrency(member.netWorth) }}</span>
                </li>
              </ul>
            </div>

            <div class="accounts-list">
              <h4>Accounts:</h4>
              <ul>
                <li v-for="account in family.members.flatMap(m => m.accounts)" :key="account.accountId">
                  <span>{{ account.bankName }} - {{ account.accountType }}</span>
                  <span class="account-balance">₹ {{ formatCurrency(account.balance) }}</span>
                </li>
              </ul>
            </div>

            <router-link :to="`/families/${family.familyId}`" class="btn btn-primary" style="margin-top: 1rem;">
              View Details
            </router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { dashboardService } from '../services/dashboardService'

export default {
  name: 'Dashboard',
  data() {
    return {
      summary: {
        totalFamilyNetWorth: 0,
        totalFamilies: 0,
        totalMembers: 0,
        totalAccounts: 0,
        familiesNetWorth: []
      },
      loading: true,
      error: null
    }
  },
  mounted() {
    this.loadDashboard()
  },
  methods: {
    async loadDashboard() {
      try {
        this.loading = true
        const response = await dashboardService.getDashboardSummary()
        this.summary = response.data
      } catch (err) {
        this.error = 'Failed to load dashboard data'
        console.error(err)
      } finally {
        this.loading = false
      }
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
.dashboard {
  padding: 2rem 0;
}

h1 {
  margin-bottom: 2rem;
  color: #2c3e50;
}

h2 {
  margin-bottom: 1.5rem;
  color: #2c3e50;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1.5rem;
  margin-bottom: 3rem;
}

.stat-card {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  text-align: center;
}

.stat-card h3 {
  color: #7f8c8d;
  font-size: 0.9rem;
  margin-bottom: 0.5rem;
  text-transform: uppercase;
}

.stat-value {
  font-size: 2rem;
  font-weight: bold;
  color: #3498db;
}

.families-section {
  margin-top: 2rem;
}

.families-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
  gap: 2rem;
}

.family-card {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border-left: 4px solid #3498db;
}

.family-card h3 {
  color: #2c3e50;
  margin-bottom: 0.5rem;
}

.family-networth {
  font-size: 1.3rem;
  font-weight: bold;
  color: #27ae60;
  margin-bottom: 1rem;
}

.members-list,
.accounts-list {
  margin-bottom: 1rem;
}

.members-list h4,
.accounts-list h4 {
  font-size: 0.9rem;
  color: #7f8c8d;
  margin-bottom: 0.5rem;
  text-transform: uppercase;
}

.members-list ul,
.accounts-list ul {
  list-style: none;
  padding-left: 0;
}

.members-list li,
.accounts-list li {
  padding: 0.5rem 0;
  font-size: 0.9rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #ecf0f1;
}

.member-worth,
.account-balance {
  font-weight: bold;
  color: #27ae60;
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
