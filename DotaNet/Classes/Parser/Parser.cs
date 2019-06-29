using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using DotaNet.Classes.Gamers;
using System.Linq;
using System.Threading;

namespace DotaNet.Classes.Parser
{
    public class MatchParseException:ApplicationException
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
            try
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
            catch
            {
                throw new MatchParseException();
            }
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
                throw new MatchParseException();
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
                throw new MatchParseException();
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
        private static MatchResult GetMatchResultByNode(HtmlNode node)
        {
            string URL = node.ChildNodes.FindFirst("a").Attributes["href"].Value;
            MatchResult result = GetMatchResult(site + URL);
            return result;
        }
        /// <summary>
        /// Функция получает матчи из документа
        /// </summary>
        /// <param name="page">Документ с матчами</param>
        /// <returns>Матчи из узлов</returns>
        private static List<MatchResult> GetMatchResults(HtmlDocument page)
        {
            List<MatchResult> matchResults = new List<MatchResult>();
            foreach (HtmlNode node in page.DocumentNode.SelectNodes("//div[@class='" + ClassMatchName + "']"))
            {
                try
                {
                    MatchResult newMatch = GetMatchResultByNode(node);
                    matchResults.Add(newMatch);
                }
                catch (MatchParseException) { }
            }
            return matchResults;
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
        public static List<MatchResult> Parse()
        {
            HtmlDocument page = LoadPage(site + startUrl);

            List<MatchResult> matchResults = new List<MatchResult>();

            do
            {
                matchResults.AddRange(GetMatchResults(page));

            } while ((page=NextPage(page))!=null);

            return matchResults;
        }

        #endregion

        #region Многопоточчная выгрузка матчей

        public static List<MatchResult> ParseThread()
        {
            List<HtmlDocument> pages = GetAllPages();
            List<Thread> threads = new List<Thread>();
            List<ScanerPage> scanerPages = new List<ScanerPage>();
            List<MatchResult> result = new List<MatchResult>();

            foreach (var page in pages)
            {
                ScanerPage scaner = new ScanerPage(page);
                scanerPages.Add(scaner);
                Thread thread = new Thread(new ThreadStart(scaner.GetMathes));
                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            int countRunningThreads;
            do
            {
                countRunningThreads = (from thread in threads
                                       where thread.ThreadState != ThreadState.Stopped
                                       select thread).Count();
                Thread.Sleep(10000);
            } while (countRunningThreads != 0);

            foreach (var scaner in scanerPages)
            {
                result.AddRange(scaner.matches);
            }

            return result;
        }

        public static List<HtmlDocument> GetAllPages()
        {
            HtmlDocument document = LoadPage(site + startUrl);
            List<HtmlDocument> pagesUrl = new List<HtmlDocument>();

            do
            {
                pagesUrl.Add(document);
            } while ((document = NextPage(document)) != null);

            return pagesUrl;
        }

        class ScanerPage
        {
            public HtmlDocument document;
            public List<MatchResult> matches;

            public ScanerPage(HtmlDocument Document)
            {
                this.document = Document;
                matches = new List<MatchResult>();
            }

            public void GetMathes()
            {
                matches = Parser.GetMatchResults(document);
            }
        }

        #endregion

        #region Test

        //Test

        public static List<MatchResult> ParseTest()
        {
            string testURL = startUrl;
            HtmlDocument page = LoadPage(site + testURL);

            List<MatchResult> matches = new List<MatchResult>();

            matches.AddRange(GetMatchResults(page));

            return matches;
        }

        #endregion
    }
}
