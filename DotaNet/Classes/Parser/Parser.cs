using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using DotaNet.Classes.Gamers;
using System.Linq;

namespace DotaNet.Classes.Parser
{
    /// <summary>
    /// Класс <see cref="Parser"/> реализует выгрузку данных из сайта
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// Получает документ
        /// </summary>
        public HtmlDocument Document { get; private set; }
        /// <summary>
        /// Функция выгружает содержимое страницы
        /// </summary>
        /// <param name="url">Адресс загружаемой страницы</param>
        /// <returns>Содержимое страницы</returns>
        public HtmlDocument LoadPage(string url)
        {
            var result = "";
            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var recieveStream = response.GetResponseStream();
                if (recieveStream != null)
                {
                    StreamReader streamReader;
                    if (response.CharacterSet == null)
                        streamReader = new StreamReader(recieveStream);
                    else
                        streamReader = new StreamReader(recieveStream, Encoding.GetEncoding(response.CharacterSet));
                    result = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                response.Close();
            }

            var document = new HtmlDocument();
            document.LoadHtml(result);
            Document = document;

            return document;
        }
        /// <summary>
        /// Функция получает узлы матчей
        /// </summary>
        /// <returns>Узлы матчей</returns>
        private List<HtmlNode> GetMatchesNode()
        {
            List<HtmlNode> result = new List<HtmlNode>();
            foreach (HtmlNode node in Document.DocumentNode.SelectNodes("//div[@class=team-vs-team"))
                result.Add(node);
            return result;
        }
        /// <summary>
        /// Функция получает матчи из узлов
        /// </summary>
        /// <param name="nodes">Узлы матчей</param>
        /// <returns>Матчи из узлов</returns>
        private List<Match> GetMatches(List<HtmlNode> nodes)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Получает узлы с игроками
        /// </summary>
        /// <returns>Узлы с игроками</returns>
        private List<HtmlNode> GetGamersNodes()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Получает игроков из узлов
        /// </summary>
        /// <returns>Игроки</returns>
        private List<Gamer> GetGamers(List<HtmlNode> nodes)
        {
            throw new NotImplementedException();
        }
        public Parser()
        {
        }
    }
}
