/*YoutubeToDiscordBot.Configuration
 * Used to load the configuration file for the bot
 * Created 3-13-2017
 * Created by Jacob Douglas (Gamem)
 */
using Newtonsoft.Json;
using System;
using System.IO;

namespace YoutubeToDiscordBot
{
    public class Configuration
    {
        /// <summary>
        /// Loads the Configuration file
        /// </summary>
        /// <param name="sFileName">File path and Name of the configuration file.</param>
        /// <returns>Returns a Loaded Configuration File</returns>
        public static Configuration LoadConfig(string sFileName)
        {
            try
            {
                TextReader trReader = new StreamReader(sFileName);//Text Reader
                string sJson = trReader.ReadToEnd();//Reads the configuration file info
                trReader.Close();//close reader                                    
                Configuration config = JsonConvert.DeserializeObject<Configuration>(sJson);//deserialize Config File
                return config;//Return loaded Config File
            }//try
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);//show issue
                return null;//return nothing
            }//catch
        }//LoadConfig(string sFileName)

        public ulong DiscordChannel { get; set; }//Discord channel ID to broadcast to.
        public string DiscordAPIKey { get; set; }//Discord's API Key for the bot
        public string YoutubeAPIKey { get; set; }//Youtube's API key for the use of Googles Youtube API
        public string YoutubeChannelID { get; set; }//Youtube channel to check for new youtube videos
        public int UpdateInterval { get; set; }//Amount of time, in minutes, to check for new videos
        public string YoutubeCasheFileName { get; set; }//Name of the file to store a cashe of youtube's api information
        public string VideoCasheFileName { get; set; }//Name of the file to store a flat file database of videos.
        
    }//class
}//namespace
