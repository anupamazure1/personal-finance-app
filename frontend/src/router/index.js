import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../views/Dashboard.vue'
import Families from '../views/Families.vue'
import FamilyDetail from '../views/FamilyDetail.vue'
import Accounts from '../views/Accounts.vue'
import Transactions from '../views/Transactions.vue'

const routes = [
  {
    path: '/',
    name: 'Dashboard',
    component: Dashboard
  },
  {
    path: '/families',
    name: 'Families',
    component: Families
  },
  {
    path: '/families/:id',
    name: 'FamilyDetail',
    component: FamilyDetail
  },
  {
    path: '/accounts',
    name: 'Accounts',
    component: Accounts
  },
  {
    path: '/transactions/:accountId/:bank',
    name: 'Transactions',
    component: Transactions
  }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

export default router
