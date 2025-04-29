<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="专辑名称">
    <el-input 
    v-model="state.CategoryName"
    :disabled="true"  ></el-input>
  </el-form-item>
  <el-form-item label="分类名称">
    <el-input v-model="categoryName"></el-input>
  </el-form-item>  
  <el-form-item label="展示索引">
    <el-input v-model="state.ShowIndex"></el-input>
  </el-form-item> 
  <el-form-item>
    <el-button type="primary" @click="save">保存</el-button>
  </el-form-item>
</el-form>
</template>

<script>
import axios from 'axios';
import {reactive, getCurrentInstance} from 'vue' 
import {useRouter } from 'vue-router'
export default {
  name: 'AlbumAdd',
  setup(){
    const router = useRouter();
    const categoryName = router.currentRoute.value.query.name;//读取页面参数
    console.log(categoryName)
    const state=reactive({AlbumName:"",CategoryName:categoryName,ShowIndex:0});  
    const {apiRoot} = getCurrentInstance().proxy;
    const save=async ()=>{      
      await axios.post(`${apiRoot}/Listening.Admin/Album/AddAlbum`,state);
      history.back(); 
    }
	  return {state,save};
  },
}
</script>
<style scoped>
</style>