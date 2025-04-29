<template>
  <h2 style="text-align: center;">分类列表</h2>
  <el-row :gutter="10">
    <el-col :span="6" v-for="c in state.categories" :key="c.id">
      <router-link :to="{ path: 'Category', query: { name: c.name } }">
        <div class="category-card">
          <div class="category-image-container">
            <img
            v-if="c.coverUrl && c.coverUrl.trim() !== ''"
            :src="c.coverUrl"
            class="category-image"
          />
          </div>
          <div class="category-title">{{ c.name }}</div>
        </div>
      </router-link>
    </el-col>
  </el-row>
</template>

<script>
import axios from 'axios';
import { useRoute, useRouter } from 'vue-router';
import { reactive, onMounted, getCurrentInstance } from 'vue';

export default {
  name: 'Index',
  setup() {
    const state = reactive({ categories: [] });
    const { apiRoot } = getCurrentInstance().proxy;
    const router = useRouter();

    onMounted(async function () {
      try {
        const { data } = await axios.get(`${apiRoot}/Category/FindAllCategory`);
        state.categories = data;
      } catch (error) {
        console.error('Failed to load categories:', error);
      }
    });

    return { state };
  },
};
</script>

<style scoped>
.category-card {
  background: linear-gradient(135deg, #ffffff 0%, #f5f7fa 100%);
  border-radius: 16px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.08);
  padding: 20px;
  text-align: center;
  transition: all 0.3s ease;
  height: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  cursor: pointer;
}

.category-card:hover {
  transform: translateY(-8px) scale(1.02);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.12);
  background: linear-gradient(135deg, #f0f3f7 0%, #ffffff 100%);
}

.category-image-container {
  width: 100px;
  height: 100px;
  border-radius: 12px;
  overflow: hidden;
  margin-bottom: 15px;
  background-color: #f0f2f5;
  display: flex;
  align-items: center;
  justify-content: center;
}

.category-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.category-title {
  font-size: 18px;
  font-weight: 600;
  color: #303133;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 100%;
  margin: 0;
  transition: color 0.3s ease;
}

.category-card:hover .category-title {
  color: #409eff;
}

/* 响应式调整 */
@media (max-width: 768px) {
  .category-card {
    padding: 15px;
  }
  
  .category-image-container {
    width: 80px;
    height: 80px;
  }
  
  .category-title {
    font-size: 16px;
  }
}
</style>