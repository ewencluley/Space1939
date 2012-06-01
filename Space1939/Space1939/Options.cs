using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml.Serialization;

namespace Space1939
{
    public class Options
    {
        String settingsFilename = "settings.xml";
        OptionsData gameSettings;

        [Serializable]
        public struct OptionsData
        {
            public bool fullscreen;
            public bool invertY;
            public int musicVolume;
            public int soundVolume;
            public int clippingDistance;
            public bool gamePad;
            public PlayerIndex controller;
        }

        public Options()
        {
            try
            {
                Stream stream = File.OpenRead(settingsFilename); //open the settings file
                XmlSerializer serializer = new XmlSerializer(typeof(OptionsData)); //make a new serializer
                gameSettings = (OptionsData)serializer.Deserialize(stream); //set the gameSettings object to the deserialization of the file
                stream.Close();
            }
            catch (System.IO.FileNotFoundException) //if settings file doesnt exist
            {
                gameSettings = new OptionsData(); //set defaults
                gameSettings.musicVolume = 100;
                gameSettings.soundVolume = 100;
                gameSettings.invertY = false;
                gameSettings.fullscreen = true;
                gameSettings.clippingDistance = 100;
                gameSettings.gamePad = false;
                gameSettings.controller = PlayerIndex.One;
                new FileStream(settingsFilename, FileMode.Create, FileAccess.Write).Close();//create the file
                saveOptions();//save the defaults to the file.
            }
        }

        public void saveOptions()
        {
            Stream stream = File.Create(settingsFilename);
            XmlSerializer serializer = new
               XmlSerializer(typeof(OptionsData));
            serializer.Serialize(stream, gameSettings);
            stream.Close();
        }

        public OptionsData getOptionsData()
        {
            return gameSettings;
        }

        public void setOptionsData(OptionsData o)
        {
           gameSettings = o;
        }
    }
}
