<template>
  <h2 style="text-align: center;">专辑列表</h2>
  <el-row :gutter="10">
    <el-col :span="6" v-for="c in state.albums" :key="c.id">
      <router-link :to="{path:'Album',query:{name:c.name}}">
        <div class="album-card">
          <div class="album-title">{{ c.name }}</div>
        </div>
      </router-link>
    </el-col>
  </el-row>
</template>


<script>
import axios from 'axios';
import {useRouter } from 'vue-router';
import {reactive,onMounted, getCurrentInstance} from 'vue' ;

export default {
  name: 'Category',
  setup(){
    const state=reactive({albums:[]});  
    const {apiRoot} = getCurrentInstance().proxy;
    const router = useRouter();
    var name = router.currentRoute.value.query.name;
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/Album/FindAlbumsByCategoryName/${name}`);
      state.albums = data;
    });
	  return {state};
  },
}
</script>
<style scoped>
.album-card {
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

.album-card:hover {
  transform: translateY(-8px) scale(1.02);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.12);
  background: linear-gradient(135deg, #f0f3f7 0%, #ffffff 100%);
}

.album-title {
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

.album-card:hover .album-title {
  color: #409eff;
}

/* 响应式调整 */
@media (max-width: 768px) {
  .album-card {
    padding: 15px;
  }

  .album-title {
    font-size: 16px;
  }
}
</style>