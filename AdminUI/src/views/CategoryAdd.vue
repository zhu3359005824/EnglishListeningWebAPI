<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="分类名称">
    <el-input v-model="state.name"></el-input>
  </el-form-item>
  <el-form-item label="显示序号">
    <el-input v-model="state.showIndex"></el-input>
  </el-form-item>  
  <el-form-item label="封面图片">
    <el-input v-model="state.coverUrl"></el-input>
  </el-form-item> 
  <el-form-item>
    <el-button type="primary" @click="save">保存</el-button>
  </el-form-item>
</el-form>
</template>

<script>
import axios from 'axios';
import {reactive,onMounted, getCurrentInstance} from 'vue' 
export default {
  name: 'CategoryAdd',
  setup(){
    const state=reactive({name:"",showIndex:0,coverUrl:"" });  
    const {apiRoot} = getCurrentInstance().proxy;
    const save=async ()=>{      
      await axios.post(`${apiRoot}/Listening.Admin/Category/Add`,state);
      history.back(); 
    }
	  return {state,save};
  },
}
</script>
<style scoped>
</style>