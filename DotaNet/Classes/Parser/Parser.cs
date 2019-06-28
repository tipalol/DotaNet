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

        #region Работа с страницей матчей

        /// <summary>
        /// Получить матч из узла
        /// </summary>
        /// <param name="node">узел с матчем</param>
        /// <returns></returns>
        private Match GetMatch(HtmlNode node)
        {
            string URL = node.ChildNodes.FindFirst("a").Attributes["href"].Value;
            return new Match(site + URL);
        }
        /// <summary>
        /// Функция получает матчи из документа
        /// </summary>
        /// <param name="page">Документ с матчами</param>
        /// <returns>Матчи из узлов</returns>
        private List<Match> GetMatches(HtmlDocument page)
        {
            List<Match> matches = new List<Match>();
            foreach (HtmlNode node in page.DocumentNode.SelectNodes("//div[@class='" + ClassMatchName + "']"))
            {
                matches.Add(GetMatch(node));
            }
            return matches;
        }
        /// <summary>
        /// Перейти на следующую страницу
        /// </summary>
        /// <param name="page">текущая страница</param>
        /// <returns>Следующая страница или null</returns>
        private HtmlDocument NextPage(HtmlDocument page)
        {
            HtmlDocument nextPage;
            HtmlNode nextPageNode = page.DocumentNode.SelectSingleNode("//div[@class='" + ClassNextPage + "']");

            if (nextPageNode == null)
            {
                return null;
            }
            nextPageNode = nextPageNode.ChildNodes.FindFirst("a");
            string URL = nextPageNode.Attributes["href"].Value;
            nextPage = LoadPage(site + URL);
            return nextPage;
        }
        /// <summary>
        /// Начинает парсинг
        /// </summary>
        /// <returns></returns>
        public List<Match> Parse()
        {
            HtmlDocument page = LoadPage(site + startUrl);

            List<Match> matches = new List<Match>();

            do
            {
                matches.AddRange(GetMatches(page));

            } while ((page=NextPage(page))!=null);

            return matches;
        }

        #endregion

        public Parser()
        {
        }
    }
}
