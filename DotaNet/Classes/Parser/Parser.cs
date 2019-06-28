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
        private const string site = "https://dota2.ru";
        private const string startUrl = "/esport/matches/?page=30";
        private const string ClassMatchName = "esport-match-single";
        private const string ClassNextPage = "pagination_next";
        private const string ClassLeftTeam = "esport-match-view-map-single-side esport-match-view-map-single-side-left";
        private const string ClassRightTeam = "esport-match-view-map-single-side esport-match-view-map-single-side-right";
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
        /// Получает команды
        /// </summary>
        /// <param name="URL">Ссылка на матч</param>
        /// <returns></returns>
        public Team[] GetTeams(string URL)
        {
            HtmlDocument document = LoadPage(URL);

            //left side
            HtmlNode left = document.DocumentNode.SelectSingleNode("//div[@class='" + ClassLeftTeam + "']");
            string leftTeamName = left.SelectSingleNode(".//span[@class='title']").InnerText.Replace(" ", "");
            var leftGamers = left.SelectNodes(".//div[@class='esport-match-view-map-single-side-picks-single-info']");  //Выбор всех героев слева
            string[] leftGamersName = GetNameGamers(leftGamers);

            //rigth side
            HtmlNode right = document.DocumentNode.SelectSingleNode("//div[@class='" + ClassRightTeam + "']");
            string rightTeamName = right.SelectSingleNode(".//span[@class='title']").InnerText.Replace(" ", "");
            var rightGamers = right.SelectNodes(".//div[@class='esport-match-view-map-single-side-picks-single-info']");
            string[] rightGamersName = GetNameGamers(rightGamers);

            return new Team[2];
        }
        private string[] GetNameGamers(HtmlNodeCollection nodes)
        {
            string[] gamers = new string[5];
            int i = 0;
            foreach(HtmlNode gamer in nodes)
            {
                gamers[i] = gamer.ChildNodes.FindFirst("a").ChildNodes.FindFirst("b").InnerText;
                i++;
            }
            return gamers;
        }
        /// <summary>
        /// Функция получает матчи из узлов
        /// </summary>
        /// <param name="nodes">Узлы матчей</param>
        /// <returns>Матчи из узлов</returns>
        private List<Match> GetMatches()
        {
            HtmlDocument page = LoadPage(site + startUrl);

            List<Match> matchsURL = new List<Match>();

            do
            {
                foreach (HtmlNode node in page.DocumentNode.SelectNodes("//div[@class='" + ClassMatchName + "']"))
                {
                    string URL = node.ChildNodes.FindFirst("a").Attributes["href"].Value;
                    matchsURL.Add(new Match(site+URL));
                }

                try
                {
                    HtmlNode nextPage = page.DocumentNode.SelectSingleNode("//div[@class='" + ClassNextPage + "']").ChildNodes.FindFirst("a");
                    string URL = nextPage.Attributes["href"].Value;

                    page = LoadPage(site + URL);
                }
                catch
                {
                    break;
                }

            } while (true);

            return matchsURL;
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
        /// <summary>
        /// Начинает парсинг
        /// </summary>
        /// <returns></returns>
        public List<Match> Parse()
        {
            return GetMatches();
        }

        public Parser()
        {
        }
    }
}
