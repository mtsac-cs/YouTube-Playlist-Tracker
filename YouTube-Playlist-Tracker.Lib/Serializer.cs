﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows;

namespace YouTube_Playlist_Tracker.Lib
{
    /// <summary>
    /// This class saves other classes to json file
    /// </summary>
    public static class Serializer
    {
        public static T LoadFromFile<T>(string filePath) where T : class
        {
            if (!IsPathValid(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            if (String.IsNullOrEmpty(json))
                return null;

            return JsonConvert.DeserializeObject<T>(json);
        }

        private static bool IsPathValid(string filePath)
        {
            Guard.ThrowIfArgumentIsNull(filePath, "Can't load file, path is null", "filePath");
            if (!File.Exists(filePath))
                return false;

            return true;
        }


        public static void SaveToFile<T>(T jsonObject, string savePath, bool overwriteExisting = true) where T : class
        {
            Guard.ThrowIfArgumentIsNull(savePath, "Can't save file, save path is null", "savePath");
            CreateDirIfNotFound(savePath);
            string json = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);

            bool keepOriginal = !overwriteExisting;
            StreamWriter serialize = new StreamWriter(savePath, keepOriginal);
            serialize.Write(json);
            serialize.Close();
        }

        private static void CreateDirIfNotFound(string dir)
        {
            FileInfo f = new FileInfo(dir);
            Directory.CreateDirectory(f.Directory.FullName);
        }
    }
}
