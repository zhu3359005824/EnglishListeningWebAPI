import { createRouter,createWebHashHistory} from "vue-router";
import CategoryList from "../views/CategoryList.vue";
import CategoryAdd from "../views/CategoryAdd.vue";
import CategoryEdit from "../views/CategoryEdit.vue";
import AlbumList from "../views/AlbumList.vue";
import AlbumAdd from "../views/AlbumAdd.vue";
import AlbumEdit from "../views/AlbumEdit.vue";
import EpisodeList from "../views/EpisodeList.vue";
import EpisodeAdd from "../views/EpisodeAdd.vue";
import EpisodeEdit from "../views/EpisodeEdit.vue";
import Login from "../views/Login.vue";
import AdminUserAdd from "../views/AdminUserAdd.vue";
import AdminUserEdit from "../views/AdminUserEdit.vue";
import AdminUserList from "../views/AdminUserList.vue";
import Main from "@/views/Main.vue";


/* 
const routes = [
  { 
    path: '/listen',
    name:"听力管理",
    id:1,
    icon:'Platform',
    component: Main ,
    children:[
      {

   
      },
     
      {
        path:'/user',
        name:"用户管理",
        id:2,
        icon:'Platform',
        component:AdminUserList
        
      },
     ]
   },
  
  {
    path: "/Login",
	  name:"Login",
    component: Login
  }
] */

  const routes = [
    { path: "/",
     name:"zhucaidan",
      component: Main ,
      children:[
        {
          path: "/CategoryList",
          name:"分类列表",
          isShow:true,
          component: CategoryList
        },
        {
          path: "/CategoryAdd",
          name:"分类添加",
          isShow:false,
          component: CategoryAdd
        },
        {
          path: "/CategoryEdit",
          name:"分类编辑",
          isShow:false,
          component: CategoryEdit
        },
        {
          path: "/AlbumList",
          name:"专辑列表",
          isShow:false,
          component: AlbumList
        },
        {
          path: "/AlbumAdd",
          name:"专辑添加",
          isShow:false,
          component: AlbumAdd
        },
        {
          path: "/AlbumEdit",
          name:"专辑编辑",
          isShow:false,
          component: AlbumEdit
        },
        {
          path: "/EpisodeList",
          name:"播放列表",
          isShow:false,
          component: EpisodeList
        },
        {
          path: "/EpisodeAdd",
          name:"播放文件添加",
          isShow:false,
          component: EpisodeAdd
        },
        {
          path: "/EpisodeEdit",
          name:"播放文件编辑",
          isShow:false,
          component: EpisodeEdit
        },
        {
          path: "/AdminUserAdd",
          name:"管理员添加",
          isShow:true,
          component: AdminUserAdd
        },
        {
          path: "/AdminUserEdit",
          name:"管理员编辑",
          isShow:false,
          component: AdminUserEdit
        },
        {
          path: "/AdminUserList",
          name:"管理员列表",
          isShow:true,
          component: AdminUserList
        }
      ]
    },
    
    
    
    {
      path: "/Login",
      name:"Login",
      component: Login
    },
    
    
  ]




const router = createRouter({
  history: createWebHashHistory(),
  routes: routes
});
export default router