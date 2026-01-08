<script setup>

import { useRouter } from 'vue-router';



const prop = defineProps(["menuData"]);
const router = useRouter();

function handleClick(item) {

    console.log(prop.menuData.filter(item => item.isShow),"menu_change")
    console.log(item,"menu_change")
    router.push(item.path);
   
}


</script>

<template>
<div v-for="item in prop.menuData.filter(item => item.isShow)"  :key="item.id" >
    <el-menu-item @click="handleClick(item)" v-if="!item.children || item.children.length === 0" :index="item.id">
        <el-icon size="20">
        <component :is="item.icon"></component>
      </el-icon>
        <span>{{ item.name }}</span>
    </el-menu-item>

    <el-sub-menu  v-else :index="item.id">
        <template #title>
            <el-icon size="20">
        <component :is="item.icon"></component>
      </el-icon>
            <span>{{ item.name }}</span>
        </template>
        <el-menu-item @click="handleClick(m)" v-for="m in item.children" :key="m.id" :index="m.id">
            <el-icon size="20">
        <component :is="item.icon"></component>
      </el-icon>
            <span>{{ m.name }}</span>
        </el-menu-item>

    </el-sub-menu>
</div>
</template>