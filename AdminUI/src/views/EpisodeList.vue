<template>
<div>
  <el-button-group>
    <el-button type="primary" icon="el-icon-circle-plus-outline" @click="addnew">创建音频</el-button>  
  </el-button-group> 
  <el-table row-key='id' :data="state.encodingEpisodes"  v-if="state.encodingEpisodes.length>0" style="width: 100%">
    <el-table-column prop="episodeName" label="音频名称"></el-table-column>
    <el-table-column prop="durationInSecond" width="60px" label="秒数"> 
    </el-table-column>     
    <el-table-column label="转码状态" width="120px">
       <template #default="scope">
        {{renderEncodingStatus(scope.row.status)}}
      </template>       
    </el-table-column>    
  </el-table>

  <el-table row-key='id' :data="state.episodes"  style="width: 100%"> 
    <el-table-column prop="episodeName" label="音频名称"></el-table-column>
   <!--  <el-table-column prop="realSeconds" label="总时长"></el-table-column> -->
    <el-table-column prop="sentenceType" label="字幕类型"></el-table-column>
    <el-table-column label="秒数">
		<template #default="scope">
		  <span>{{parseInt(scope.row.realSeconds)}}</span>
		</template>        
    </el-table-column>     
    <el-table-column fixed="right" label="操作">
      <template #default="scope">
        <el-button type="text" size="small">
          <button @click="deleteItem(scope.row)">删除</button>
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
import {floatItem,sinkItem} from '../scripts/ArrayUtils';
import * as signalR from '@microsoft/signalr';

export default {
  name: 'EpisodeList',
  setup(){
    const router = useRouter();
    const {apiRoot} = getCurrentInstance().proxy;
    const albumName = router.currentRoute.value.query.albumName;
    const state=reactive({episodes:[],encodingEpisodes:[],albumName:albumName,isInSortMode:false});    
    onMounted(async function(){
      console.log(albumName,"episodeList_Onmounted")
      await reloadData();
      //禁用Negotiation，客户端一直连接初始的服务器，这样服务器搞负载均衡（不用Redis BackPlane等）也没问题
      const options = {
        skipNegotiation: true,
        transport: 1 // 强制WebSockets
      };
      const hub = new signalR.HubConnectionBuilder()
      .withUrl(`${apiRoot}/Listening.Admin/Hubs/EpisodeEncodingStatusHub`,options)
      .build();
      hub.start();
      hub.on('OnMediaEncodingStarted',id=>{        
        var episode = state.encodingEpisodes.find(e=>e.id==id);
        episode.status = "Started";
      });
      hub.on('OnMediaEncodingFailed',id=>{
        var episode = state.encodingEpisodes.find(e=>e.id==id);
        episode.status = "Failed";
      });
      hub.on('OnMediaEncodingCompleted',id=>{
        var episode = state.encodingEpisodes.find(e=>e.id==id);
        episode.status = "Completed";
        reloadData();//遇到由完成任务的就刷新数据
      });
    });
    const reloadData=async()=>{
      let resp = await axios.get(`${apiRoot}/Listening.Admin/Episode/FindEpisodesByAlbumName/${albumName}`);
      state.episodes = resp.data; 
      
      console.log(state.episodes,"loading_episodelist")

      resp = await axios.get(`${apiRoot}/Listening.Admin/Episode/FindEncodingEpisodesByAlbumId/${albumName}`);
      state.encodingEpisodes = resp.data;
      console.log(state.encodingEpisodes,"loading_Encodingepisodelist")

    };

    const deleteItem=async (episode)=>{      
      const name = episode.episodeName;
      console.log(state.episodes)
      if(!confirm(`真的要删除${name}吗？`))
        return;
      await axios.post(`${apiRoot}/Listening.Admin/Episode/DeleteByName`,{
        albumName:albumName,
        episodeName:name
      });      
      state.episodes = state.episodes.filter(e=>e.episodeName!=name);//刷新表格
    };
  
     
    const addnew=()=>{
      router.push({name:'播放文件添加',query:{albumName:state.albumName}});
    }; 
    const edit=(id)=>{
      router.push({name:'EpisodeEdit',query:{id:id}});
    };
    
    const renderEncodingStatus = (status)=>{
      const dict = {"Created":"等待转码","Started":"转码中","Failed":"转码失败","Completed":"转码完成"};
      const value = dict[status];
      return value?value:"未知";
    };
	  return {state,deleteItem,addnew,edit,
      floatItem,sinkItem, renderEncodingStatus,reloadData};
  },
}
</script>
<style lang="css">
  .inVisibleEpisode{text-decoration: line-through;}
  .visibleEpisode{text-decoration:inherit;}
</style>