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
    public class GamerEmptyException:ApplicationException
    {

    }

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
        public static HtmlDocument LoadPage(string url)
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

        #region Работа с матчем

        /// <summary>
        /// Получитть название команды
        /// </summary>
        /// <param name="team">Узел с командой</param>
        /// <returns></returns>
        private static string GetTeamName(HtmlNode team)
        {
            string teamName = team.SelectSingleNode(".//span[@class='title']").InnerText.Replace("\t", "").Replace("\r", "").Replace("\n", "");
            return teamName;
        }
        /// <summary>
        /// Получить результат матча
        /// </summary>
        /// <param name="URL">Ссылка на матч</param>
        /// <returns></returns>
        public static MatchResult GetMatchResult(string URL)
        {
            HtmlDocument document = LoadPage(URL);

            //left side
            HtmlNode left = document.DocumentNode.SelectSingleNode("//div[@class='" + ClassLeftTeam + "']");
            string leftTeamName = GetTeamName(left);
            Gamer[] leftGamers = GetGamers(left);
            Team leftTeam = new Team(leftTeamName, leftGamers);

            //rigth side
            HtmlNode right = document.DocumentNode.SelectSingleNode("//div[@class='" + ClassRightTeam + "']");
            string rightTeamName = GetTeamName(right);
            Gamer[] rightGamers = GetGamers(right);
            Team rightTeam = new Team(rightTeamName, rightGamers);

            var score = GetScore(document);

            MatchResult matchResult = new MatchResult(leftTeam, rightTeam, score.left, score.right);
            return matchResult;
        }
        /// <summary>
        /// Получить счет игры
        /// </summary>
        /// <param name="document">Страница с счетом</param>
        /// <returns>Счет</returns>
        private static (int left, int right) GetScore(HtmlDocument document)
        {
            HtmlNode scoreNode = document.DocumentNode.SelectSingleNode("//div[@class='match-shop-result']");
            string[] score = scoreNode.Attributes["data-value"].Value.Split(":");

            int scoreLeft = int.Parse(score[0]);
            int scoreRight = int.Parse(score[1]);

            return (scoreLeft, scoreRight);
        }
        /// <summary>
        /// Получить игроков команды
        /// </summary>
        /// <param name="team">Узел с командой</param>
        /// <returns>Игроки</returns>
        private static Gamer[] GetGamers(HtmlNode team)
        {
            var  gamersNode=team.SelectNodes(".//div[@class='esport-match-view-map-single-side-picks-single-info']");
            if (gamersNode == null)
            {
                throw new GamerEmptyException();
            }

            List<Gamer> gamers = new List<Gamer>();
            foreach(HtmlNode gamer in gamersNode)
            {
                try
                {
                    gamers.Add(GetGamer(gamer));
                }
                catch { }
            }
            return gamers.ToArray();
        }
        /// <summary>
        /// Получитть игрока из узла
        /// </summary>
        /// <param name="gamer">узел с игроком</param>
        /// <returns></returns>
        private static Gamer GetGamer(HtmlNode gamer)
        {
            string name= gamer.ChildNodes.FindFirst("a")?.ChildNodes.FindFirst("b").InnerText;
            if(name==null)
            {
                throw new GamerEmptyException();
            }
            Gamer result = new Gamer(name);
            return result;
        }

        #endregion

        #region Работа с страницей матчей

        /// <summary>
        /// Получить матч из узла
        /// </summary>
        /// <param name="node">узел с матчем</param>
        /// <returns></returns>
        private static Match GetMatch(HtmlNode node)
        {
            string URL = node.ChildNodes.FindFirst("a").Attributes["href"].Value;
            Match result= new Match(site + URL);
            return result;
        }
        /// <summary>
        /// Функция получает матчи из документа
        /// </summary>
        /// <param name="page">Документ с матчами</param>
        /// <returns>Матчи из узлов</returns>
        private static List<Match> GetMatches(HtmlDocument page)
        {
            List<Match> matches = new List<Match>();
            foreach (HtmlNode node in page.DocumentNode.SelectNodes("//div[@class='" + ClassMatchName + "']"))
            {
                try
                {
                    Match newMatch = GetMatch(node);
                    matches.Add(GetMatch(node));
                }
                catch (GamerEmptyException) { }
            }
            return matches;
        }
        /// <summary>
        /// Перейти на следующую страницу
        /// </summary>
        /// <param name="page">текущая страница</param>
        /// <returns>Следующая страница или null</returns>
        private static HtmlDocument NextPage(HtmlDocument page)
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
        public static List<Match> Parse()
        {
            HtmlDocument page = LoadPage(site + startUrl);

            List<Match> matches = new List<Match>();

            do
            {
                matches.AddRange(GetMatches(page));

            } while ((page=NextPage(page))!=null);

            return matches;
        }

        public static List<Match> ParseTest()
        {
            string testURL = startUrl;
            HtmlDocument page = LoadPage(site + testURL);

            List<Match> matches = new List<Match>();

            matches.AddRange(GetMatches(page));

            return matches;
        }

        #endregion
    }
}
