using System;
using System.Collections.Generic;
using System.Linq;
/*YoutubeToDiscordBot.YoutubeBot.cs
 * The Core of the Discord Bot
 * Created 3-13-2017 
 * Created by Jacob Douglas (Gamem)
 */
using Discord;
using System.Threading;
namespace YoutubeToDiscordBot
{
    class YoutubeBot
    {
        public Configuration BotConfig;//config file for the bot
        DiscordClient discordClient;//Discord Client
        private Thread VideoChecker;//Thread that checks for new videos

        public YoutubeBot(Configuration config)
        {
            BotConfig = config;//Set config file
            VideoDatabase.LoadVideos(BotConfig.VideoCasheFileName);//load video list from file
            discordClient = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;//What to log
                x.LogHandler = Log;//Log handler
               
            });//Discord Client
            VideoChecker = new Thread(new ThreadStart(CheckVideoList));//Set Thread
            discordClient.ExecuteAndWait(async () =>
            {
                await discordClient.Connect(BotConfig.DiscordAPIKey, TokenType.Bot);//Connect  to discord
            });//Wait to connect
            //Nothing happens past here            
        }//YoutubeBot Constuctor

        /// <summary>
        /// The log from the bot. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Log(object sender, LogMessageEventArgs e)
        {
            //Start the checker once the bot has connected and joined the channel.         
            if (e.Message.ToString().Contains("GUILD_AVAILABLE:"))
            {
                VideoChecker.Start();//start checking vids
                discordClient.SetGame(new Game("Gamem's Bot"));//Set game playing
            }//If bot connects to discord community
        }//Log (Handles logs of the bot)

        /// <summary>
        /// Thread to check for new videos on the desired youtube channel.
        /// </summary>
        public void CheckVideoList()
        {
            while (true)
            {
                string DateFormat = "(" + DateTime.Now + ") ";//Setup date format
                Console.WriteLine(DateFormat + "Checking video list for new videos...");
                Channel youtubeChannelChat = discordClient.GetChannel(BotConfig.DiscordChannel);//define channel to announce to
                VideoDatabase.LoadVideos(BotConfig.VideoCasheFileName);//load video list from file
                YoutubeHandler.FetchVideos(BotConfig.YoutubeAPIKey,BotConfig.YoutubeChannelID,BotConfig.YoutubeCasheFileName);//load video list from web
                foreach (YoutubeHandler.Items vid in YoutubeHandler.youtubeResponce.items)
                {
                    string vidID = vid.id.videoId;//video's id
                    if (!VideoDatabase.videoList.ContainsKey(vidID))
                    {
                        VideoDatabase.videoList.Add(vidID, false);//add vid to database
                    }//if video isn't in our databse
                }//foreach video in the responce
                VideoDatabase.SaveVideos(BotConfig.VideoCasheFileName);//save vids
                Console.WriteLine(DateFormat + "Video list updated successfuly!");//log
                Dictionary<string, bool> tempVid = new Dictionary<string, bool>();//stores changes to database
                foreach (KeyValuePair<string, bool> Video in VideoDatabase.videoList)
                {
                    if (Video.Value == false)
                    {                        
                        //youtubeChannelChat.SendMessage($"@everyone New Video! " + YoutubeHandler.YOUTUBE_FORMAT + Video.Key);//post message
                        Console.WriteLine(DateFormat + "New video posted!");
                    }//video hasn't been announced                    
                    tempVid.Add(Video.Key, true);//change list
                }//foreach video in our list of videos
                VideoDatabase.videoList = tempVid;//set vids
                VideoDatabase.SaveVideos(BotConfig.VideoCasheFileName);//save vids
                Console.WriteLine(DateFormat + "Video update completed! Sleeping for " + BotConfig.UpdateInterval + " minutes.");//log
                Thread.Sleep(TimeSpan.FromMinutes(BotConfig.UpdateInterval));//sleep for 3 hours
            }//While true
        }//CheckVideoList()
    }//class
}//namespace
