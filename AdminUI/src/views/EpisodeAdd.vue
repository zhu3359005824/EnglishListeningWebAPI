<template>
<el-form ref="form" label-width="80px">
  <el-form-item label="专辑名称">
    <el-input 
    v-model="state.albumName"
    :disabled="true"  ></el-input>
    <!-- 专辑名称自动传值,禁用输入 -->

  </el-form-item>
  <el-form-item label="单曲名称">
    <el-input v-model="state.episodeName"></el-input>
  </el-form-item>  
  <el-form-item label="音频路径">
    <audio id="player" style="display:none" :src="state.audioUrl" controls></audio>
    <ZackUploader v-model="state.audioUrl"></ZackUploader>
  </el-form-item>   
   
  <el-form-item label="字幕类型">
    <el-select v-model="state.sentenceType" placeholder="请选择字幕类型">
      <el-option label="srt" value="srt"></el-option>
      <el-option label="vtt" value="vtt"></el-option>
	  <el-option label="lrc" value="lrc"></el-option>
	  <el-option label="json" value="json"></el-option>
    </el-select>
  </el-form-item> 
  <el-form-item label="字幕">
    <el-input type="textarea" v-model="state.sentenceContext" rows="5"></el-input>
  </el-form-item>  
  <el-form-item>
    <el-button type="primary" @click="save">保存</el-button>
  </el-form-item>
</el-form>
</template>
<script>
import axios from 'axios';
import {reactive, getCurrentInstance} from 'vue';
import {useRouter } from 'vue-router';
import ZackUploader from '../components/ZackUploader.vue';

export default {
  name: 'EpisodeAdd',
  components:{ZackUploader},
  setup(){
    const router = useRouter();
    const albumName = router.currentRoute.value.query.albumName;//读取页面参数
   
    const state=reactive(
      {albumName:albumName,
      sentenceContext:"",
      sentenceType:"srt",
      episodeName:'',
      audioUrl:""
    });  
    const {apiRoot} = getCurrentInstance().proxy;
    const save=async()=>{
     const seccess=  await axios.post(`${apiRoot}/Listening.Admin/Episode/AddEpisode`,state);
      console.log(seccess)
      if(seccess.data===true){
        history.back(); 
      }
         
    };
	  return {state,save};
  },
}
</script>
<style scoped>
</style>