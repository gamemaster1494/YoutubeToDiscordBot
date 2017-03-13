using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace YoutubeToDiscordBot
{
    public static class VideoDatabase
    {
        /* Format of videos
         * <URL><PostedYN>
         * 
         * 
         */
        public static Dictionary<string, bool> videoList;//Contains the video list
        static char SEPERATOR = '|';
        static public void LoadVideos(string sFileName)
        {
            try
            {
                if (!File.Exists(sFileName))
                {
                    File.Create(sFileName);//create the file
                }//if file doesn't exist
                videoList = new Dictionary<string, bool>();//rest var
                FileStream file = new FileStream(sFileName, FileMode.Open, FileAccess.Read);//file stream
                StreamReader srRead = new StreamReader(file);//Stream reader          
                     
                string sLines = srRead.ReadToEnd();//Read the first line
                string[] testList = sLines.Split('\n');
                srRead.Close();
                file.Close();
                foreach(string sLine in testList)
                {
                    if (sLine.Length > 3)
                    {
                        string[] splitstring = sLine.Split(SEPERATOR);//Space seperator
                        string url = splitstring[0];//First section = URL
                        bool posted = Convert.ToBoolean(splitstring[1]);//second section = posted or not
                        videoList.Add(url, posted);//add to list
                    }//if line isn't blank
                }//foreach vid in the list
            }//try
            catch(Exception ex)
            {
                Console.WriteLine("Unknown error occured loading video list. " + ex.Message.ToString());//show error             
            }//catch            
        }//LoadVideos()

        static public void SaveVideos(string sFileName)
        {
            try
            {
                
                FileStream file = new FileStream(sFileName, FileMode.Create, FileAccess.Write);//filestream
                StreamWriter swWrite = new StreamWriter(file);//stream writer
                foreach(KeyValuePair<string,bool> data in videoList)
                {
                    swWrite.WriteLine(data.Key + SEPERATOR + data.Value);
                }
                swWrite.Close();//close streamwriter
                file.Close();//close filestream
            }//try
            catch (Exception ex)
            {
                Console.WriteLine("Unknown error occured Saving video list. " + ex.Message.ToString());//show error
            }//catch
        }//SaveVideos()
    }//class
}//namespace
