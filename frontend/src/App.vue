<template>
  <div id="app">
    <nav class="navbar">
      <div class="nav-container">
        <router-link to="/" class="nav-brand">💰 Personal Finance</router-link>
        <ul class="nav-menu">
          <li><router-link to="/" :class="{ active: $route.path === '/' }">Dashboard</router-link></li>
          <li><router-link to="/families" :class="{ active: $route.path === '/families' }">Families</router-link></li>
          <li><router-link to="/accounts" :class="{ active: $route.path === '/accounts' }">Accounts</router-link></li>
        </ul>
      </div>
    </nav>

    <main class="main-content">
      <router-view />
    </main>
  </div>
</template>

<script>
export default {
  name: 'App'
}
</script>

<style scoped>
.navbar {
  background-color: #2c3e50;
  color: white;
  padding: 1rem 0;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.nav-container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.nav-brand {
  font-size: 1.5rem;
  font-weight: bold;
  color: white;
  text-decoration: none;
}

.nav-menu {
  list-style: none;
  display: flex;
  gap: 2rem;
}

.nav-menu a {
  color: white;
  text-decoration: none;
  transition: color 0.3s;
}

.nav-menu a:hover,
.nav-menu a.active {
  color: #3498db;
}

.main-content {
  max-width: 1200px;
  margin: 2rem auto;
  padding: 0 15px;
  min-height: calc(100vh - 120px);
}
</style>
