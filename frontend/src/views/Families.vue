<template>
  <div class="families">
    <div class="header">
      <h1>Families</h1>
      <button @click="showCreateForm = true" class="btn btn-primary">+ New Family</button>
    </div>

    <div v-if="loading" class="loading">
      <div class="spinner"></div>
      <p>Loading families...</p>
    </div>

    <div v-else-if="error" class="alert alert-danger">
      {{ error }}
    </div>

    <div v-else>
      <div v-if="showCreateForm" class="form-modal">
        <div class="form-container">
          <h2>Create New Family</h2>
          <form @submit.prevent="createFamily">
            <div class="form-group">
              <label class="form-label">Family Name *</label>
              <input v-model="newFamily.name" class="form-control" required />
            </div>
            <div class="form-group">
              <label class="form-label">Description</label>
              <textarea v-model="newFamily.description" class="form-control" rows="3"></textarea>
            </div>
            <div class="form-actions">
              <button type="submit" class="btn btn-success">Create</button>
              <button type="button" @click="showCreateForm = false" class="btn btn-secondary">Cancel</button>
            </div>
          </form>
        </div>
      </div>

      <div v-if="families.length === 0" class="text-center">
        <p>No families found. Create one to get started!</p>
      </div>

      <div v-else class="families-list">
        <div v-for="family in families" :key="family.id" class="family-item">
          <div class="family-info">
            <h3>{{ family.name }}</h3>
            <p v-if="family.description">{{ family.description }}</p>
            <p class="members-count">{{ family.members.length }} member(s)</p>
          </div>
          <div class="family-actions">
            <router-link :to="`/families/${family.id}`" class="btn btn-primary">View</router-link>
            <button @click="editFamily(family)" class="btn btn-primary">Edit</button>
            <button @click="deleteFamily(family.id)" class="btn btn-danger">Delete</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { familyService } from '../services/familyService'

export default {
  name: 'Families',
  data() {
    return {
      families: [],
      newFamily: { name: '', description: '' },
      showCreateForm: false,
      loading: true,
      error: null
    }
  },
  mounted() {
    this.loadFamilies()
  },
  methods: {
    async loadFamilies() {
      try {
        this.loading = true
        const response = await familyService.getAllFamilies()
        this.families = response.data
      } catch (err) {
        this.error = 'Failed to load families'
        console.error(err)
      } finally {
        this.loading = false
      }
    },
    async createFamily() {
      try {
        await familyService.createFamily(this.newFamily)
        this.newFamily = { name: '', description: '' }
        this.showCreateForm = false
        await this.loadFamilies()
      } catch (err) {
        this.error = 'Failed to create family'
        console.error(err)
      }
    },
    async deleteFamily(id) {
      if (confirm('Are you sure?')) {
        try {
          await familyService.deleteFamily(id)
          await this.loadFamilies()
        } catch (err) {
          this.error = 'Failed to delete family'
          console.error(err)
        }
      }
    },
    editFamily(family) {
      this.newFamily = { name: family.name, description: family.description }
      this.showCreateForm = true
    }
  }
}
</script>

<style scoped>
.families {
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

.form-modal {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.form-container {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  max-width: 500px;
  width: 90%;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.form-container h2 {
  margin-bottom: 1.5rem;
  color: #2c3e50;
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
  border-color: #7f8c8d;
}

.families-list {
  display: grid;
  gap: 1rem;
}

.family-item {
  background: white;
  padding: 1.5rem;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
}

.family-info h3 {
  margin-bottom: 0.5rem;
  color: #2c3e50;
}

.family-info p {
  color: #7f8c8d;
  margin: 0.25rem 0;
}

.members-count {
  font-size: 0.9rem;
  color: #3498db;
  font-weight: bold;
}

.family-actions {
  display: flex;
  gap: 0.5rem;
}

.text-center {
  text-align: center;
  padding: 2rem;
  color: #7f8c8d;
}
</style>
