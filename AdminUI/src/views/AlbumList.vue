<template>
<div>

  
  <el-button-group>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="addnew">创建专辑</el-button>
      
  </el-button-group> 

  <el-table row-key='id'
    :data="state.albums"
    
    style="width: 100%">  
    <el-table-column prop="albumname" label="专辑名称"></el-table-column>
    <el-table-column prop="categoryName" label="分类名称"></el-table-column>
    <el-table-column prop="createTime" label="创建时间"></el-table-column>

    <el-table-column fixed="right" label="操作">
      <template #default="scope">
        <el-button type="text" size="small">
          <button @click="deleteItem(scope.row)">删除</button>
        </el-button>
       <!--  <el-button type="text" size="small">
          <button @click="edit(scope.row.id)">修改</button>
        </el-button> -->
        <el-button type="text" size="small">
          <button @click="manageChildren(scope.row.albumname)">管理音频</button>
        </el-button>
      </template>
    </el-table-column>    
  </el-table>    
</div>
</template>

<script>
import axios from 'axios';
import {reactive,onMounted, getCurrentInstance} from 'vue' 
import {useRouter } from 'vue-router'
import {floatItem,sinkItem} from '../scripts/ArrayUtils'

export default {
  name: 'AlbumList',
  setup(){
    const router = useRouter();
    const {apiRoot} = getCurrentInstance().proxy;
    const categoryName = router.currentRoute.value.query.categoryName;
    const state=reactive({albums:[],categoryName:categoryName,isInSortMode:false});    
    onMounted(async function(){
   
      const {data}=await axios.get(`${apiRoot}/Listening.Admin/Album/FindAlbumsByCategoryName?categoryName=${categoryName}`);  
      console.log(categoryName,"albumlist")    
      state.albums = data;
    });
    const deleteItem=async (album)=>{      
      const id = album.id;
      const name = album.name.chinese;
      if(!confirm(`真的要删除${name}吗？`))
        return;
      await axios.delete(`${apiRoot}/Listening.Admin/Album/DeleteById/${id}`);      
      state.albums = state.albums.filter(e=>e.id!=id);//刷新表格
    };
 
    const addnew=()=>{
      router.push({name:'专辑添加',query:{name:categoryName}});
    };
    const edit=(id)=>{
      router.push({name:'专辑编辑',query:{id:id}});
    };
    const manageChildren=(albumName)=>{

      router.push({name:'播放列表',query:{albumName:albumName}});
    };
   
	  return {state,deleteItem,addnew,edit,manageChildren,floatItem,sinkItem};
  },
}
</script>
<style lang="css">
  .inVisibleAlbum{text-decoration: line-through;}
  .visibleAlbum{text-decoration:inherit;}
</style>