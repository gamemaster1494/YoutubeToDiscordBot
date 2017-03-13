/*YoutubeToDiscordBot.YoutubeHandler.cs
 * Handles the interaction between the YoutubeAPI V3
 * Created 3-13-2017 
 * Created by Jacob Douglas (Gamem)
 */
using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.IO;
namespace YoutubeToDiscordBot
{
    public static class YoutubeHandler
    {
        public static string YOUTUBE_FORMAT = $"http://www.youtube.com/watch?v=";//Format for the youtube video to be posted in
        public static Responce youtubeResponce = null;//responce

        /// <summary>
        /// Gets the 20 most recent youtube videos from the youtube channel.
        /// </summary>
        /// <param name="APIKey">Youtube API key (V3)</param>
        /// <param name="ChannelID">Youtube Channel ID</param>
        /// <param name="CasheFile">Cashe File to store the most recent responce</param>
        public static void FetchVideos(string APIKey, string ChannelID, string CasheFile)
        {
            //api key
            var apiKey = APIKey;            
            //Url for responce
            var url = $"https://www.googleapis.com/youtube/v3/search?key=" + apiKey + $"&channelId=" + ChannelID + $"&part=snippet,id&order=date&maxResults=20";
            string cacheFile = CasheFile;//name of cashe file
            string sResults = "";//holds results
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//create request info
            HttpWebResponse responce = null;//holds responce from web
            try
            {
                responce = (HttpWebResponse)request.GetResponse();//try to get the response
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);//show error
                responce = null;//set to null
            }//catch
            try
            {
                if (responce != null)
                {
                    StreamReader srRead = new StreamReader(responce.GetResponseStream());//create reader
                    sResults = srRead.ReadToEnd();//get results
                    srRead.Close();//close stream
                    File.WriteAllText(cacheFile, sResults);//write responce to file
                }//if result != null
                else
                {
                    TextReader reader = new StreamReader(cacheFile);//reader
                    sResults = reader.ReadToEnd();//read entire file
                    reader.Close();//close reader
                }//else
                responce.Close();//close the responce                
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);//show issue
                responce.Close();//close responce
                TextReader reader = new StreamReader(cacheFile);//reader
                sResults = reader.ReadToEnd();//read entire file
                reader.Close();//close reader
            }//catch
            youtubeResponce = JsonConvert.DeserializeObject<Responce>(sResults);//convert and store
        }//FetchVideos()

        //Responce info from youtube's API
        [Serializable]
        public class Responce
        {        
            public string kind { get; set; }
            public string etag { get; set; }
            public string nextPageToken { get; set; }
            public string regionCode { get; set; }
            public pageInfo pageinfo { get; set; }
            public List<Items> items { get; set; }
        }//Responce
        [Serializable]
        public class pageInfo
        {
            public int totalResults { get; set; }
            public int resultsPerPage { get; set; }
        }//pageInfo
        [Serializable]
        public class Items
        {
            public string ikind { get; set; }
            public string ietag { get; set; }
            public ID id { get; set; }
        }//Items
        [Serializable]
        public class ID
        {
            public string idkind { get; set; }
            public string videoId { get; set; }
        }//ID
    }//class
}//namespace

