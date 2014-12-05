using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Motr.Net.Http
{
    /// <summary>
    /// http request
    /// </summary>
    public class Request
    {
        /// <summary>
        /// 
        /// </summary>
        public Request()
        {
            Encoding = Encoding.UTF8;
            CookieContainer = new CookieContainer();
        }
        public Request(Encoding encoding)
            : this()
        {
            Encoding = encoding;
        }
        /// <summary>
        /// 
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CookieContainer CookieContainer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public String CookieString { get; set; }
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public String Get(String uri)
        {
            return ResponseResult(CreateRequest(Method.GET, uri, CookieType.String, String.Empty));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public String Get(String uri, Dictionary<String, Object> param)
        {
            return Get(uri, param.ToQueryString());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public String Get(String uri, String queryString)
        {
            if (!String.IsNullOrEmpty(queryString))
                uri = String.Format("{0}?{1}", uri, queryString);
            return Get(uri);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        public String Post(String uri, params byte[] postData)
        {
            return ResponseResult(CreateRequest(Method.POST, uri, CookieType.String, String.Empty, postData));
        }
        public String Post(String uri, Dictionary<String, Object> par)
        {
            return Post(uri, this.Encoding.GetBytes(par.ToQueryString()));
        }
        public String Post(String uri, String queryString)
        {
            return Post(uri, this.Encoding.GetBytes(queryString));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        private Stream GetResponseStream(HttpWebResponse res)
        {
            if (res.StatusCode != HttpStatusCode.OK) return null;
            if (res.Headers["Set-Cookie"] != null) this.CookieString = res.Headers["Set-Cookie"];
            return res.GetResponseStream();
        }
        /// <summary>
        /// 返回请求响应文本
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private String ResponseResult(HttpWebRequest req)
        {
            using (HttpWebResponse res = req.GetResponse() as HttpWebResponse)
            {
                using (Stream stream = GetResponseStream(res))
                {
                    if (stream == null) return String.Empty;
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
        /// <summary>
        /// 创建请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="uri"></param>
        /// <param name="cookieType"></param>
        /// <param name="referrer"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        private HttpWebRequest CreateRequest(Method method, String uri, CookieType cookieType, String referrer, params byte[] postData)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            ServicePointManager.Expect100Continue = false;
            if (uriBuilder.Scheme == "https") ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; });
            HttpWebRequest req = WebRequest.Create(uriBuilder.Uri) as HttpWebRequest;
            if (method == Method.POST) PrePOST(req, postData);
            req.Method = method.ToString();
            req.Timeout = 0x4e20;
            req.UserAgent = "Mozilla/4.0 (compatible; MSIE 10.0; Windows NT 6.1)";
            req.Accept = "*/*";
            req.Referer = String.IsNullOrEmpty(referrer) ? uriBuilder.Uri.ToString() : referrer;
            if (cookieType == CookieType.String)
                req.Headers["Cookie"] = this.CookieString;
            else
                req.CookieContainer = this.CookieContainer;
            return req;
        }
        /// <summary>
        /// 将post数据写入请求流
        /// </summary>
        /// <param name="req"></param>
        /// <param name="postData"></param>
        private void PrePOST(HttpWebRequest req, byte[] postData)
        {
            req.ContentType = "application/x-www-form-urlencoded";
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(postData, 0, postData.Length);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cookie"></param>
        public void SetCookie(String cookie)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public String GetCookie()
        {
            return String.Empty;
        }
    }
}
