using System.Text;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using System.Threading;
namespace wms
{
    public class WebApi
    {
        /// <summary>
        /// 系统登陆验证
        /// </summary>
        /// <returns></returns>
        public bool Login(string user_id, string password)
        {
            bool flag = false;
            try
            {
                #region requestJson
                StringBuilder reqJson = new StringBuilder();
                reqJson.Append("{");
                reqJson.Append("\"User_id\": \"" + user_id + "\",");
                reqJson.Append("\"User_password\": \"" + password + "\"");
                reqJson.Append("}");
                #endregion
                string strJson = reqJson.ToString();
                RestClient client = new RestClient(@"http://192.168.0.56/apitest/api/User");
                RestRequest request = new RestRequest(@"Login", Method.POST);
                var json = JObject.Parse(strJson);
                request.AddParameter("application/json; charset=utf-8", json, ParameterType.RequestBody);
                request.RequestFormat = DataFormat.Json;
                IRestResponse resp = client.Execute(request);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    var result = resp.Content;
                    var jResult = JObject.Parse(result);
                    if (jResult["status"].ToString() == "1")
                        flag = true;
                }
                return flag;
            }
            catch
            {
                return false;
            }
        }
    }
}