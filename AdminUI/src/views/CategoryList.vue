<template>
<div>
  <el-button-group>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="addnew">创建分类</el-button>
  </el-button-group> 
  <el-table row-key='id' ref="table" :data="state.categories" style="width: 100%">
    
    <el-table-column prop="categoryName" label="分类名称"  width="150px"></el-table-column>
    <el-table-column prop="showIndex" label="显示序号" width="120px"></el-table-column>
    <el-table-column prop="createTime" label="创建时间" width="180px"></el-table-column>
    <el-table-column fixed="right" label="操作">
      <template #default="scope">
        <el-button type="text" size="small">
          <button @click="deleteItem(scope.row)">删除</button>
        </el-button>
        <!-- <el-button type="text" size="small">
          <button @click="edit(scope.row.id)">修改</button>
        </el-button> -->
        <el-button type="text" size="small">
          <button @click="manageChildren(scope.row.categoryName)">管理专辑</button>
        </el-button>
      </template>
    </el-table-column>    
  </el-table>  
</div>
</template>

<script>
import axios from 'axios';
import {reactive,onMounted, getCurrentInstance} from 'vue' ;
import {useRouter } from 'vue-router';
import {floatItem,sinkItem} from '../scripts/ArrayUtils'

export default {
  name: 'CategoryList',
  setup(){
    //axios.defaults.headers.Authorization  = "Bearer eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImNkNGIwY2U1LTIwZDgtNDkxYS04YjIxLWQ2N2M3NjY5YjY1OCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJBZG1pbiIsIlVzZXIiXSwiZXhwIjoxNjYzMzE4Mzc1LCJpc3MiOiJteUlzc3VlciIsImF1ZCI6Im15QXVkaWVuY2UifQ.VECL0niSsS2zpVFGYmwaoTRgYedbCig1cwbglb0Gbps";

    const state=reactive({categories:[]});  
    const {apiRoot} = getCurrentInstance().proxy;
    const router = useRouter();
    onMounted(async function(){
      const {data} = await axios.get(`${apiRoot}/Listening.Admin/Category/FindAll`);
     
      state.categories = data;
    });

    const deleteItem=async (category)=>{ 
      console.log(category,"deletecategory")     
      
      const categoryName = category.categoryName;
      if(!confirm(`真的要删除${categoryName}吗？`))
        return;
      await axios.get(`${apiRoot}/Listening.Admin/Category/DeleteByName/${categoryName}`);      
      state.categories = state.categories.filter(e=>e.categoryName!=categoryName);//刷新表格
    };
    const manageChildren =(id)=>{
      console.log(id)
      router.push({name: '专辑列表',query:{categoryName:id}});
    };
    const addnew=()=>{
      router.push({name:'分类添加'});
    }; 
    const edit=(id)=>{
      console.log(id,"CategoryEdit")
      router.push({name:'分类编辑',query:{id:id}});
    };
   
	  return {state,deleteItem,edit,manageChildren,addnew};
  },
}
</script>
<style scoped>
</style>