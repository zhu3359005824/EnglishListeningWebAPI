<template>
  <h2 style="text-align: center;">播放列表</h2>
  <el-row :gutter="10">
    <el-col :span="6" v-for="e in state.episodes" v-bind:key="e.id">
      <router-link :to="{path:'Episode',query:{name:e.episodeName}}">
        <div class="episode-card">
          <div class="episode-title">{{e.episodeName}}</div>
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
  name: 'Album',
  setup(){
    const state=reactive({episodes:[]});  
    const {apiRoot} = getCurrentInstance().proxy;
    const router = useRouter();
    var name = router.currentRoute.value.query.name;
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/Episode/FindEpisodesByAlbumName/${name}`);
      console.log(data)
      state.episodes = data;
    });
	  return {state};
  },
}
</script>
<style scoped>
.episode-card {
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

.episode-card:hover {
  transform: translateY(-8px) scale(1.02);
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.12);
  background: linear-gradient(135deg, #f0f3f7 0%, #ffffff 100%);
}
.episode-title {
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

.episode-card:hover .episode-title {
  color: #409eff;
}

/* 响应式调整 */
@media (max-width: 768px) {
  .episode-card {
    padding: 15px;
  }

  .episode-title {
    font-size: 16px;
  }
}
</style>