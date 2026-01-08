<script setup>

import { useRouter } from 'vue-router';
import Menu from './Menu.vue';
import { reactive, ref } from 'vue';

const router=useRouter()

let menuData=reactive(router.options.routes[0].children)


function handleOpen(){

}

function handleClose(){

}

</script>

<template>
    <el-menu :style="{width:'200px' }"
        default-active="2"
        class="aside-container"
        @open="handleOpen"
        @close="handleClose"

    
      >
      <p class="logo-menu">听力训练管理端</p>
      <Menu :menuData="menuData"/>
      </el-menu>
  </template>

  <style scoped lang="less">
  .aside-container{
    height: 100%;
    background-color:#F0F8FF;
    .logo-menu{
      font-size: 20px;
      text-align: center;
      height: 50px;
      line-height: 20px;
      margin-top: 20px;

    }
  }


</style>