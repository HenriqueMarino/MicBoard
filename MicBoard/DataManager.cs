﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using NAudio;

namespace MicBoard
{
    class DataManager
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "//MicBoardFileMap.xml";
        public void Save(string filename, string directory)
        {
            string duration = GetDuration(directory);   
            if (File.Exists(path)) Insert(filename, directory, duration);
            else
            {                
                XDocument doc = new XDocument(new XElement("Models", new XElement("Model", new XElement("FileName", filename), new XElement("Directory", directory),
                    new XElement("Duration", duration), new XElement("KeyShortcut", ""))) );
                doc.Save(path);
            }
        }

        public void Insert(string filename, string directory, string duration)
        {
            XDocument doc = XDocument.Load(path);
            doc.Root.Add(new XElement("Model", new XElement("FileName", filename), new XElement("Directory", directory),
                    new XElement("Duration", duration), new XElement("KeyShortcut", "")));
            doc.Save(path);
        }

        public List<Model> List()
        {
            List<Model> list = new List<Model>();
            if (!File.Exists(path)) return list;
            foreach (XElement level in XElement.Load(path).Elements("Model"))
            {
                Model m = new Model();
                m.FileName = level.Element("FileName").Value;
                m.Directory = level.Element("Directory").Value;
                m.Duration = level.Element("Duration").Value;
                m.KeyShortcut = level.Element("KeyShortcut").Value;

                list.Add(m);
            }            
            return list;
        }
        //removendo as casas decimais do valor em segundos
        public string GetDuration(string directory)
        {
            string[] duration = new NAudio.Wave.AudioFileReader(directory).TotalTime.ToString().Split(':');
            duration[2] = Math.Floor(double.Parse(duration[2], CultureInfo.InvariantCulture)).ToString();
            duration[2] = int.Parse(duration[2]) > 9 ? duration[2] : int.Parse(duration[2]) == 0 ? "0" + 1 : "0" + duration[2];
            return string.Join(":", duration);
        }
    }
}
