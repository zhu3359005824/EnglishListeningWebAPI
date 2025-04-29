<template>
    <div class="common-layout">
      <el-container>
        <Aside></Aside>
        <el-container>
          
          <el-header     style="background-color: #F0F8FF;"   ><Header/></el-header>
          <el-main style="background-color: #E6E6FA;">
            <router-view></router-view>

        </el-main>
        </el-container>
      </el-container>

      
    </div>
  </template>

  <script setup>


</script>
  
  <style scoped>
 .common-layout {
 
 height: 100vh;
 width: 100vw;
 .el-container {
   height: 100vh;
 }
}

  </style>
  