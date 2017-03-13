/*YoutubeToDiscordBot.Program.cs
 * The entry method into the Bot.
 * Created on 3-13-2017
 * Created by Jacob Douglas (Gamem)
 */
using System;

namespace YoutubeToDiscordBot
{
    class Program
    {
        public static Configuration BotConfig = Configuration.LoadConfig("config.cfg");//Configuration information
        static void Main(string[] args)
        {           
            if(BotConfig == null)
            {
                Console.WriteLine("Invalid Configuration file. Make sure the config file exists, and no extra spaces are present.");//Show issue
                Console.WriteLine("Hit enter to exit...");//show info
                Console.ReadKey();//wait for input
                return;//stop the program
            }//If config didn't load
            Console.WriteLine("Hit enter to start...");//Show info
            Console.ReadKey();//wait for input to start the bot
            YoutubeBot bot = new YoutubeBot(BotConfig);//Start the bot.
            Console.WriteLine("Hit enter to exit...");//Show info
            Console.ReadKey();//Wait for input in case there is an error
        }//Main()
    }//class
}//namespace
