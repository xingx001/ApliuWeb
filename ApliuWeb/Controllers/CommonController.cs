using ApliuTools;
using ApliuTools.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ApliuWeb.Controllers
{
    public class CommonController : ApiController
    {
        [HttpPost]
        public string Register()
        {
            string username = HttpContextRequest.Form["username"];
            string smscode = HttpContextRequest.Form["smscode"];
            string password = HttpContextRequest.Form["password"];
            string passwordag = HttpContextRequest.Form["passwordag"];
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";

            if (string.IsNullOrEmpty(username))
            {
                result.msg = "用户名不能为空";
                return result.ToString();
            }
            CodeCase cc = SessionHelper.GetSessionValue(CodeType.Register.ToString()) as CodeCase;
            if (cc == null || (TimeHelper.DataTimeNow - cc.CreateTime).Seconds > cc.Timeout)
            {
                result.msg = "请重新获取短信验证码";
                return result.ToString();
            }
            if (cc.User != username || cc.Code != smscode)
            {
                result.msg = "短信验证码错误";
                return result.ToString();
            }
            if (string.IsNullOrEmpty(password))
            {
                result.msg = "密码不能为空";
                return result.ToString();
            }
            if (password.Length < 3)
            {
                result.msg = "密码长度必须大于等于3";
                return result.ToString();
            }
            if (string.IsNullOrEmpty(passwordag))
            {
                result.msg = "请再次输入密码";
                return result.ToString();
            }
            if (password != passwordag)
            {
                result.msg = "两次密码输入不一致";
                return result.ToString();
            }

            if (!UserInfo.UserCheck(username))
            {
                result.msg = "该用户已注册";
                return result.ToString();
            }

            string id = Guid.NewGuid().ToString().ToUpper();
            string regs = string.Format(@"insert into ApUserInfo (ID,UserId,UserName,RealName,MobileNumber,UserType,Password ,CreateTime,Project,IsLocked)
     VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');", id, username, username, "", username, "Normal", SecurityHelper.MD5Encrypt(password, Encoding.UTF8), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "ApliuWeb", "0");
            if (DataAccess.Instance.PostData(regs))
            {
                result.code = "0";
                result.msg = "注册成功";
            }
            else
            {
                result.msg = "注册失败";
            }

            return result.ToString();
        }

        [HttpPost]
        public string Login()
        {
            string username = HttpContextRequest.Form["username"];
            string password = HttpContextRequest.Form["password"];
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "发生异常";

            if (string.IsNullOrEmpty(username))
            {
                result.msg = "用户名不能为空";
                return result.ToString();
            }
            if (string.IsNullOrEmpty(password))
            {
                result.msg = "密码不能为空";
                return result.ToString();
            }

            if (UserInfo.LoginCheck(username, password))
            {
                UserInfo.SetUserInfo(username, username, username, "", "");
                result.code = "0";
                result.msg = "登录成功";
            }
            else
            {
                result.msg = "用户名或密码错误";
            }

            return result.ToString();
        }

        [HttpGet]
        public string User()
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "用户未登录";
            string userid = UserInfo.GetUserInfo().UserId;
            string username = UserInfo.GetUserInfo().UserName;
            if (!string.IsNullOrEmpty(userid) && !string.IsNullOrEmpty(username))
            {
                result.code = "0";
                result.msg = username;
            }
            return result.ToString();
        }

        [HttpPost]
        public string Logout()
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "清空信息失败";
            if (UserInfo.Logout())
            {
                result.code = "0";
                result.msg = "";
            }
            return result.ToString();
        }

        [HttpPost]
        public string SetGameData()
        {
            ResponseMessage result = new ResponseMessage();
            result.code = "-1";
            result.msg = "设置失败";

            string gamename = HttpContextRequest.Form["gamename"];
            string score = HttpContextRequest.Form["score"];
            string usetime = HttpContextRequest.Form["usetime"];
            string stage = HttpContextRequest.Form["stage"];

            string userid = UserInfo.GetUserInfo().UserId;
            if (string.IsNullOrEmpty(userid))
            {
                result.code = "1";
                result.msg = "用户未登录";
                return result.ToString();
            }

            if (string.IsNullOrEmpty(gamename) || string.IsNullOrEmpty(score) || string.IsNullOrEmpty(usetime) || string.IsNullOrEmpty(stage))
            {
                result.msg = "用户输入非法";
                return result.ToString();
            }

            string id = Guid.NewGuid().ToString().ToUpper();
            string insertgame = string.Format(@"INSERT INTO GameData (ID ,UserId ,GameName ,Score,Stage ,UseTime
,Remark,CreateTime)  VALUES ('{0}','{1}','{2}',{3},{4},{5},'{6}','{7}')", id, userid, gamename, score, stage, usetime, "", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (DataAccess.Instance.PostData(insertgame))
            {
                result.code = "0";
                result.msg = "保存成绩成功";
            }
            return result.ToString();
        }

        // GET api/common
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/common/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/common
        public void Post([FromBody]string value)
        {
        }

        // PUT api/common/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/common/5
        public void Delete(int id)
        {
        }
    }
}
