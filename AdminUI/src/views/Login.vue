
<template>

  <div class="content">
    <div class="login">
    <el-form
  
    style="max-width: 600px"
    model:loginMessage
    status-icon
    label-width="auto"
    class="demo-ruleForm">



<el-form-item label="手机号" prop="username" v-if="isLogin">
<el-input v-model="state.userName" placeholder="请输入手机号" prefix-icon="user" />
</el-form-item>

<el-form-item label="密码" prop="password" v-if="isLogin">
  <el-input type="password" v-model="state.password" placeholder="请输入密码" prefix-icon="lock" />
</el-form-item>


<el-form-item label="昵称" prop="nickname" v-if="!isLogin">
  <el-input v-model="registerMessage.userName" placeholder="请输入用户名" prefix-icon="user" />
</el-form-item>
    <el-form-item label="手机号" prop="username" v-if="!isLogin">
  <el-input v-model="registerMessage.phoneNum" placeholder="请输入手机号" prefix-icon="user" />
</el-form-item>
<el-form-item label="密码" prop="password" v-if="!isLogin">
  <el-input type="password" v-model="registerMessage.password" placeholder="请输入密码" prefix-icon="lock" />
</el-form-item>



    <el-form-item>
      <el-button type="primary" @click="login"  v-if="!isRegister" >
        登录
      </el-button>
      <el-button :type="isRegister ? 'primary' : ''"    @click="register">注册</el-button>
    </el-form-item>
  </el-form>
  </div>


  </div>
  
</template>

  <script>
  import axios from 'axios';
  import {reactive,onMounted, getCurrentInstance,ref} from 'vue' 
  import {useRouter } from 'vue-router'
  import { VueCookieNext } from 'vue-cookie-next'
  
  export default {
    name: 'Login',
    setup(){
      const state=reactive({userName:"",password:""});  
      const registerMessage=reactive({userName:"",phoneNum:"",password:""}); 
      const isLogin=ref(true)
      const isRegister=ref(false) 
      const router = useRouter();
      const {apiRoot} = getCurrentInstance().proxy;
      onMounted(async function(){
      });

      const login=async ()=>{   
        const data = {
          "phoneNumber": state.userName,
          "password": state.password,
        };   
        const jwtToken = await axios.post(`${apiRoot}/IdentityService/Login/LoginByPhoneNumberAndPwd`,data);
        console.log(jwtToken)
        axios.defaults.headers.common['Authorization']="Bearer "+jwtToken.data;
        VueCookieNext.setCookie("Authorization","Bearer "+jwtToken.data);//Authorization保存到cookie中，这样刷新或者退出后仍然能用 
        router.push({name:'zhucaidan'});
       
      }
      const register= async()=> {
        isLogin.value = false; // 隐藏登录表单
        isRegister.value = true; // 显示注册表单
        
        if (!registerMessage.userName || !registerMessage.phoneNum||!registerMessage.password) {
          return ElMessage({
            message: '请确认全部填写后再操作',
            type: 'warning',
          });
        } 
        else {
          if(registerMessage.userName&&registerMessage.password&&registerMessage.phoneNum){
                    const response = await axios.post(`${apiRoot}/IdentityService/Login/AddUser`,registerMessage)
                    .catch((error) => {
                    ElMessage.error('注册失败');
                    console.error(error);
                    return Promise.reject(error);
                  });
            console.log(response)
            if (response.data ==="添加成功") {
              isLogin.value = true; // 显示登录表单
              isRegister.value = false; //隐藏注册表单
              registerMessage.userName=""
              registerMessage.phoneNum=""
              registerMessage.password=""

            } 
            else {
              ElMessage.error('注册信息不正确');
            }
            
    }
   
  }

  
}
      return {state,registerMessage,isLogin,isRegister,login,register};
    },
  }
  </script>

<style scoped>
.content {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100vh;
  background-image: url('login.png'); /* 设置背景图片 */
  background-size: cover; /* 背景图片覆盖整个容器 */
  background-position: center; /* 背景图片居中显示 */
  color: white;
  font-family: 'Arial', sans-serif;
}


.login {

  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh; /* 使容器高度占满整个视口高度 */
 
}

.demo-ruleForm {
  border-radius: 10px; /* 设置圆角 */
 
  padding: 20px; /* 内边距 */
  background: white; /* 背景色 */
  
  max-width: 600px; /* 最大宽度 */
  border-radius: 10px; /* 设置圆角 */
 
  padding: 40px; /* 内边距 */
  
  opacity: 0.7; /* 设置透明度，0.9 表示稍微透明 */
  background-color: transparent; /* 设置背景色为透明 */
  backdrop-filter: blur(5px); /* 添加模糊效果 */
  max-width: 600px; /* 最大宽度 */
  background: rgba(0, 0, 0, 0.6);	/*rgba设置透明层*/
background-color: #00000060;	/*八位颜色位设置透明层*/
font-weight: 500;
}
</style>
  