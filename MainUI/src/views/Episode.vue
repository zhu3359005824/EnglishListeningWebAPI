<template>
  <div v-if="state.episode">
    <div>
      <el-button type="primary" style="width:100vw" v-on:click="addToSelectedSentences">这句听不懂</el-button>
     <!-- <div>{{state.currentSentence.value}}</div> -->
    </div>
    <div>
      <audio :src="state.episode.audioUrl" style="width:100vw" controls ref="mainPlayer" autoplay
       v-on:timeupdate="updateCurrentSentence"/>
    </div>
    <el-row :gutter="10" v-for="(s,index) in state.selectedSentences" v-bind:key="index">
      <span v-on:click="playSentence(s)" style="cursor:pointer">{{s.value}}</span>
      <el-divider></el-divider>
    </el-row>
  </div>
</template>
<script>

/*
///
//需要在"E:\DDDProjectUpload"路径下cmd 然后输入http-server启动node.js服务,前端才可以正常访问文件
//
*/ 
import axios from 'axios';
import {useRouter } from 'vue-router';
import {reactive,ref,onMounted, getCurrentInstance} from 'vue' ;

export default {
  name: 'Episode',
  setup(){
    const state=reactive({episode:{},selectedSentences:[],currentSentence:{}});  
    const {apiRoot} = getCurrentInstance().proxy;
    const router = useRouter();
    const mainPlayer = ref(null);

    var name = router.currentRoute.value.query.name;
    onMounted(async function(){
      
      console.log(name)
      const {data} = await axios.get(`${apiRoot}/Episode/FindEpisodeByName/${name}`);
      console.log(data)
      state.episode = data;
    });
    const querySentence=(position)=>
    {
        const sentences = state.episode.sentenceModels;
        
        for (var i = 0; i < sentences.length; i++)
        {
            var sentence = sentences[i];
           
            if (position >= sentence.start && position <= sentence.end)
            {
             
                return sentence;
            }
        }              
    };
    const sentenceEqual = function (s1, s2) {
        const floatEqual = function (n1, n2) {
            return Math.abs(n1-n2) < 0.1
        };
        return (floatEqual(s1.start, s2.start) && s1.value == s2.value);
    }
    const exisitsInSelectedSentences =(itemToSearch)=>{
        if (state.selectedSentences.find(e=>sentenceEqual(e,state.currentSentence)) >= 0) {
            return true;
        }
        const selectedSentences = state.selectedSentences;
        for (var i = 0; i < selectedSentences.length; i++) {
            var item = selectedSentences[i];
            if (sentenceEqual(item, itemToSearch)) {
                return true;
            }
        }
        return false;
    };
    const addToSelectedSentences =()=>{
        if (state.currentSentence && !exisitsInSelectedSentences(state.currentSentence)) {
            state.selectedSentences.push(state.currentSentence);
        }
    };
    const updateCurrentSentence=()=>{
        var position = mainPlayer.value.currentTime;
        var foundSentence = querySentence(position);
        if (foundSentence && !sentenceEqual(foundSentence,state.currentSentence))
        {
            state.currentSentence = foundSentence;
        }    
    };
    const playSentence= (sentence)=> {
        mainPlayer.value.currentTime = sentence.start;
        mainPlayer.value.play();
    };
	  return {state,mainPlayer,querySentence,sentenceEqual,exisitsInSelectedSentences,
        addToSelectedSentences,updateCurrentSentence,playSentence};
  },
}
</script>
<style scoped>
</style>