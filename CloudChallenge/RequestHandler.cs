using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CloudChallenge
{
    /*
     RequestHandler a static class for handeling http requests.
     */
    static class RequestHandler
    {
        /*
         params: fileName = file path, the file contains an http request
         in the format:
         {request type} {url} HTTP/1.1
         {header}: {value}
         {header}: {value}
        
         the function returns the response.
        */
        public static string readAndSendHttpRequest(string fileName)
        {
            HttpWebRequest request = null;
            WebHeaderCollection myWebHeaderCollection = null;
            string jsonResponse = string.Empty;
            try
            {
                string[] fileText = File.ReadAllLines(fileName);
                setRequestProperties(fileText,ref request,ref myWebHeaderCollection);
                //Get the associated response for the above request.
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    jsonResponse = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return jsonResponse;
        }

        /*
         This function gets a json string and converting it into List of User.
         Param: json - the http response.
         return List<User> - a list of the users described in the json.
         */
        public static List<User> jsonToUsersArray(string json)
        {
            int startIndex = json.IndexOf("edges") - 1;
            int endIndex = json.IndexOf("]");
            int length = endIndex - startIndex + 1;
            string jsResult = "{" + json.Substring(startIndex, length) + "}";
            JObject obj = JObject.Parse(jsResult);
            //return nodes array
            var token = (JArray)obj.SelectToken("edges");
            var list = new List<User>();
            foreach (var item in token)
            {
                string nodeJson = JsonConvert.SerializeObject(item.SelectToken("node"));
                list.Add(JsonConvert.DeserializeObject<User>(nodeJson));
            }
            return list;
        }

        /*
         This function populate the request properties and its headers.
         params : fileText - the request text.
                  request - the HttpWebRequest that will be executed.
                  myWebHeaderCollection - WebHeaderCollection for saving the request headers. 
         */
        private static void setRequestProperties(string[] fileText, ref HttpWebRequest request, ref WebHeaderCollection myWebHeaderCollection)
        {
            string domain = "www.instagram.com";
            CookieContainer coockies = new CookieContainer();
            Cookie cookie = new Cookie();
            // handle the first line in the file,get the request type and the ulr.
            string[] splitedLine = fileText[0].Split(' ');      
            request = (HttpWebRequest)WebRequest.Create(splitedLine[1]);
            myWebHeaderCollection = request.Headers;
            request.Method = splitedLine[0];
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.ContentType = "application/json";

            foreach (string line in fileText)
            {
                if (line == fileText.First())
                    continue;
                splitedLine = line.Split(':');
                //populate request headers or add a header to the header collection.
                switch (splitedLine[0].ToUpper().Trim()) 
                {
                    case "HOST":
                        request.Host = splitedLine[1].TrimStart();
                        break;
                    case "CONNECTION":
                        request.KeepAlive = splitedLine[1].TrimStart().Equals("keep-alive") ? true : false;
                        break;
                    case "ACCEPT":
                        request.Accept = splitedLine[1].TrimStart();
                        break;
                    case "USER-AGENT":
                        request.UserAgent = splitedLine[1].TrimStart();
                        break;
                    case "REFERER":
                        request.Referer = "https://www.instagram.com/cloudresearch11/following/";
                        break;
                    case "Cookie":
                        string[] cookiesSplit = splitedLine[1].Split(';');
                        foreach(string splitedCookie in cookiesSplit)
                        {
                            string[] cookieNameAndValue = splitedCookie.Split('=');
                            cookie.Name = cookieNameAndValue[0].TrimStart();
                            cookie.Value = cookieNameAndValue[1].TrimStart();
                            cookie.Domain = domain;
                            request.CookieContainer.Add(cookie);
                        }
                        break;
                    default:    //adding headers
                        myWebHeaderCollection.Add(splitedLine[0], splitedLine[1].TrimStart());
                        break;
                }
            }
           
        }

    }
}

    
