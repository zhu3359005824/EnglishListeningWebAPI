
<script setup>


import { useRouter } from 'vue-router';
import { VueCookieNext } from 'vue-cookie-next'

const router = useRouter();

function logout() {
  router.push({name:'Login'});
  VueCookieNext.removeCookie("Authorization", { path: '/login' });
  // 使用 Vue Router 的方法进行路由跳转
  
}
</script>

<template>
<div class="header-container">
    <!-- <div class="header-left flex-box">
      <ul class="flex-box">
  <li v-for="item in menuStore.selectMenu" :key="item.id" class="tag flex-box" 
      :class="{ selected: route.path === item.path }">
    <el-icon size="12">
      <component :is="item.icon"></component>
    </el-icon>
    <router-link class="text flex-box" :to="item.path">
      {{ item.name }}
    </router-link>  
    <el-icon class="close" >
      <Close />
    </el-icon>
  </li>
</ul>
    </div> -->

    <div class="header-right">
        
        <el-dropdown>
            
    <div class="el-dropdown-link">
        <el-avatar > user </el-avatar>
      <el-icon class="el-icon--right">
        <arrow-down />
      </el-icon>
    </div>
    <template #dropdown>
      <el-dropdown-menu>
        <el-dropdown-item  @click="logout">退出</el-dropdown-item>
      </el-dropdown-menu>
    </template>
  </el-dropdown>
    </div>


</div>
</template>


<style lang="less" scoped>
.el-dropdown-link{
  background-color: #F0F8FF;
}
.flex-box{
    display: flex;
    align-items: center;
    height: 100%;
}

.header-container{
    display: flex;
    justify-content: space-between;
    align-items: center;
    height: 100%;
    padding-right: 25px;
    .header-right {
    margin-left: auto; 
    background-color: #F0F8FF;
    
  }
    .header-left{
        height: 100%;
        .icon{
            width: 45px;
            height: 100%;

        }
        .icon:hover{
            background-color: #f5f5f5;
            cursor: pointer;

        }
        .tag{
          padding:  0 10px;
          font-size: 15px;
          .text{
            margin: 0 5px;
          }
          .close{
            visibility: hidden;
           
          }
          &selected{
            a{
              color: #409eff;
            }
            i{
              background-color: #f5f5f5;
            }
          }


        }
        .tag:hover{
            background-color: #f5f5f5;
            cursor: pointer;
            .close{
            visibility: inherit;
            cursor: pointer;
            color: #000;
           
          }

        }


    }
}


</style>