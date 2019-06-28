using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using DotaNet.Classes.Gamers;
namespace DotaNet.Classes.Database
{
    public class Serialaizer
    {
        public static string Serialize(Match match)
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Match));
            serializer.WriteObject(stream, match);
            stream.Position = 0;
            byte[] json = stream.ToArray();
            

            StreamReader streamReader = new StreamReader(stream);
            Console.WriteLine(streamReader.ReadToEnd());
            stream.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
        public static Match Deserialize(string json)
        {
            Match match = new Match();
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(match.GetType());
            match = serializer.ReadObject(stream) as Match;
            stream.Close();
            return match;
        }
        public Serialaizer()
        {
        }
    }
}
