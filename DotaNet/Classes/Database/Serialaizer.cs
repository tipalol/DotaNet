﻿using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using DotaNet.Classes.Gamers;
namespace DotaNet.Classes.Database
{
    public class Serialaizer
    {
        public static Serialaizer instance;
        public static Serialaizer GetInstance()
        {
            if (instance == null)
                instance = new Serialaizer();
            return instance;
        }
        public Serialaizer() {}
        public string Serialize(Match match)
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Match));
            serializer.WriteObject(stream, match);
            stream.Position = 0;
            byte[] json = stream.ToArray();
            

            StreamReader streamReader = new StreamReader(stream);
            stream.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
        public Match Deserialize(string json)
        {
            Match match = new Match();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(match.GetType());
            match = serializer.ReadObject(stream) as Match;
            stream.Close();
            return match;
        }
        public string Serialize(Database database)
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Database));
            serializer.WriteObject(stream, database);
            stream.Position = 0;
            byte[] json = stream.ToArray();


            StreamReader streamReader = new StreamReader(stream);
            stream.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
        public Database DeserializeData(string json)
        {
            Database database = new Database();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(database.GetType());
            database = serializer.ReadObject(stream) as Database;
            stream.Close();
            return database;
        }
    }
}
