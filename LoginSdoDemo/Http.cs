using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace LoginSdoDemo
{
    class Http
    {
        public static string GetHtml(string URL)
        {
            WebRequest wrt;
            wrt = WebRequest.Create(URL);
            wrt.Credentials = CredentialCache.DefaultCredentials;
            WebResponse wrp;
            wrp = wrt.GetResponse();
            return new StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
        }
        public static string GetHtml(string URL, out string cookie)
        {
            WebRequest wrt;
            wrt = WebRequest.Create(URL);
            wrt.Credentials = CredentialCache.DefaultCredentials;
            WebResponse wrp;

            wrp = wrt.GetResponse();

            string html = new StreamReader(wrp.GetResponseStream(), Encoding.UTF8).ReadToEnd();
            cookie = wrp.Headers.Get("Set-Cookie");
            return html;
        }

        public static string GetHtml(string URL, string cookie, out string header)
        {
            return GetHtml(URL, cookie, out header, "http://yueche.woiche.com/");
        }
        public static string GetHtml(string URL, string cookie, out string header, string server)
        {
            HttpWebRequest httpWebRequest;
            HttpWebResponse webResponse;
            Stream getStream;
            StreamReader streamReader;
            string getString = "";
            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            httpWebRequest.Accept = "*/*";
            httpWebRequest.Referer = "http://yueche.woiche.com/";
            CookieContainer co = new CookieContainer();
            co.SetCookies(new Uri(server), cookie);
            httpWebRequest.CookieContainer = co;
            httpWebRequest.UserAgent =
                "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            httpWebRequest.Method = "GET";
            webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            header = webResponse.Headers.ToString();
            getStream = webResponse.GetResponseStream();
            streamReader = new StreamReader(getStream, Encoding.UTF8);
            getString = streamReader.ReadToEnd();

            streamReader.Close();
            getStream.Close();
            return getString;
        }

        public static string GetHtml(string URL, string postData, string cookie, out string header)
        {
            return GetHtml("http://yueche.woiche.com/", URL, postData, cookie, out header);
        }
        public static string GetHtml(string server, string URL, string postData, string cookie, out string header)
        {
            byte[] byteRequest = Encoding.Default.GetBytes(postData);

            byte[] bytes = GetHtmlByBytes(server, URL, byteRequest, cookie, out header);

            Stream getStream = new MemoryStream(bytes);
            StreamReader streamReader = new StreamReader(getStream, Encoding.UTF8);
            string getString = streamReader.ReadToEnd();
            streamReader.Close();
            getStream.Close();
            return getString;
        }
     
        public static byte[] GetHtmlByBytes(string server, string URL, byte[] byteRequest, string cookie,out string header)
        {
            long contentLength;
            HttpWebRequest httpWebRequest;
            HttpWebResponse webResponse;
            Stream getStream;

            httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(URL);
            CookieContainer co = new CookieContainer();
            co.SetCookies(new Uri(server), cookie);
            httpWebRequest.CookieContainer = co;
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            httpWebRequest.Accept ="image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, */*";
            httpWebRequest.Referer = "http://yueche.woiche.com/";
            httpWebRequest.UserAgent ="Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; Maxthon; .NET CLR 1.1.4322)";
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentLength = byteRequest.Length;
            Stream stream;
            stream = httpWebRequest.GetRequestStream();
            stream.Write(byteRequest, 0, byteRequest.Length);
            stream.Close();
            webResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            header = webResponse.Headers.ToString();
            getStream = webResponse.GetResponseStream();
            contentLength = webResponse.ContentLength;

            byte[] outBytes = new byte[contentLength];
            outBytes = ReadFully(getStream);
            getStream.Close();
            return outBytes;
        }
        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[128];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }





        #region --stream--
        public static Stream GetStreamByBytes(string server, string URL, byte[] byteRequest, string cookie,
                                              out string header)
        {
            Stream stream = new MemoryStream(GetHtmlByBytes(server, URL, byteRequest, cookie, out header));
            return stream;
        }
        #endregion
    }
}
