using MangaReader.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MangaReader
{
    public class MangaCrawler
    {
        /*private readonly HttpClient _httpClient;

        private readonly string _userAgentString = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36 Edg/115.0.1901.203";
        private readonly string _pageUrl = "https://www.nettruyenio.com/";
        private readonly string _mangaListXPath = "//*[@id=\"ctl00_divCenter\"]//div[@class=\"row\"]//div[@class=\"item\"]";
        private readonly string _mangaLinkXPath = ".//a";
        private readonly string _detailNodeXPath = "//*[@id=\"item-detail\"]";
        private readonly string _titleXPath = ".//h1[@class=\"title-detail\"]";
        private readonly string _coverImageXPath = ".//img";
        private readonly string _descriptionXPath = ".//div[@class=\"detail-content\"]//p";
        private readonly string _genreNodesXPath = ".//li[@class=\"kind row\"]//a";
        private readonly string _authorXPath = ".//li[@class=\"author row\"]/p[2]";
        private readonly string _chapterXPath = ".//div[@id=\"nt_listchapter\"]//nav//li";
        private readonly string _chapterNumberXPath = ".//a";
        private readonly string _chapterImgsXPath = "//div[@class=\"reading-detail box_doc\"]//div";


        public MangaCrawler()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _userAgentString);
        }

        public async Task<ObservableCollection<MangaModel>> CrawlNewUpdatedManga()
        {
            ObservableCollection<MangaModel> mangaItems = await CrawlMangaDataAsync();
            foreach(MangaModel manga in mangaItems)
            {
                manga.Chapters = await CrawlMangaChaptersAsync(manga);
            }
            return mangaItems;
        }

        public async Task<ObservableCollection<MangaModel>> CrawlMangaDataAsync()
        {
            var mangaList = new ObservableCollection<MangaModel>();

            var htmlDocument = await GetHtmlDocumentAsync(_pageUrl);

            var mangaNodes = htmlDocument.DocumentNode.SelectNodes(_mangaListXPath);

            if (mangaNodes != null)
            {
                foreach (var mangaNode in mangaNodes)
                {
                    var manga = new MangaModel();

                    manga.MangaLink = mangaNode.SelectSingleNode(_mangaLinkXPath).GetAttributeValue("href", "");

                    if (!string.IsNullOrEmpty(manga.MangaLink))
                    {
                        var mangaPageDocument = await GetHtmlDocumentAsync(manga.MangaLink);

                        // Detail node is <article id="item-detail">
                        var detailNode = mangaPageDocument.DocumentNode.SelectSingleNode(_detailNodeXPath);

                        manga.Title = detailNode.SelectSingleNode(_titleXPath).InnerText.Trim();
                        manga.CoverImageURL = detailNode.SelectSingleNode(_coverImageXPath).GetAttributeValue("src", "");
                        manga.Description = detailNode.SelectSingleNode(_descriptionXPath).InnerText.Trim();

                        var genreNodes = detailNode.SelectNodes(_genreNodesXPath);
                        if (genreNodes == null)
                            continue;
                        foreach (var genreNode in genreNodes)
                        {
                            manga.Genre.Add(genreNode.InnerText);
                        }

                        manga.Author = detailNode.SelectSingleNode(_authorXPath).InnerText;
                    }

                    mangaList.Add(manga);
                }
            }

            return mangaList;
        }

        public async Task<HtmlDocument> GetHtmlDocumentAsync(string url)
        {
            var htmlDocument = new HtmlDocument();

            try
            {
                var htmlContent = await _httpClient.GetStringAsync(url);
                htmlDocument.LoadHtml(htmlContent);
            }
            catch (Exception ex)
            {
                // Handle exceptions, such as network errors or invalid URLs
                Console.WriteLine($"Error loading HTML from {url}: {ex.Message}");
            }

            return htmlDocument;
        }

        public async Task<List<ChapterModel>> CrawlMangaChaptersAsync(MangaModel manga)
        {
            var mangaPageDocument = await GetHtmlDocumentAsync(manga.MangaLink);

            // Detail node is <article id="item-detail">
            var detailNode = mangaPageDocument.DocumentNode.SelectSingleNode(_detailNodeXPath);
            var chapterNodes = detailNode.SelectNodes(_chapterXPath);

            if (chapterNodes == null)
            {
                return null;
            }

            manga.Chapters = new List<ChapterModel>(chapterNodes.Count);

            for (int i = chapterNodes.Count - 1; i >= 0; i--)
            {
                ChapterModel chapter = new ChapterModel();
                var chapterNumberNode = chapterNodes[i].SelectSingleNode(_chapterNumberXPath);
                Match match = Regex.Match(chapterNumberNode.InnerText, @"\d+(\.\d+)?");

                chapter.ChapterNumber = float.Parse(match.Value);
                chapter.ChapterLink = chapterNodes[i].SelectSingleNode(".//a").GetAttributeValue("href", "");
                chapter.MangaModel = manga;
                manga.Chapters.Add(chapter);
            }

            manga.Chapters.Reverse();

            return manga.Chapters;
        }

        public async Task<List<string>> CrawlChapterImgUrlAsync(ChapterModel chapter)
        {
            var chapterPageDocument = await (GetHtmlDocumentAsync(chapter.ChapterLink));

            var chapterImgNodes = chapterPageDocument.DocumentNode.SelectNodes(_chapterImgsXPath);

            chapter.ChapterImageURLs = new List<string>(chapterImgNodes.Count);
            foreach (var chapterImgNode in chapterImgNodes)
            {
                chapter.ChapterImageURLs.Add(chapterImgNode.SelectSingleNode(".//img").GetAttributeValue("src", ""));
            }

            return chapter.ChapterImageURLs;
        }

        *//*public void PrintMangas(List<Manga> mangas)
        {
            var formattedOutput = "";
            foreach (var manga in mangas)
            {
                formattedOutput += $"Title: {manga.Title}\n";
                formattedOutput += $"Cover Image URL: {manga.CoverImageURL}\n";
                formattedOutput += $"Author: {manga.Author}\n";
                formattedOutput += $"Description: {manga.Description}\n";
                formattedOutput += $"Manga Link: {manga.MangaLink}\n";
                if (manga.Genre.Count > 0)
                {
                    formattedOutput += "Genres:\n";
                    foreach (var genre in manga.Genre)
                    {
                        formattedOutput += $"- {genre}\n";
                    }
                }

                //formattedOutput += $"Chapters: {manga.Chapters.Count}\n";
                formattedOutput += "\n\n";
            }

            testBox.Text = formattedOutput;
        }

        public async void PrintManga(Manga manga)
        {
            var formattedOutput = "";
            formattedOutput += $"Title: {manga.Title}\n";
            formattedOutput += $"Cover Image URL: {manga.CoverImageURL}\n";
            formattedOutput += $"Author: {manga.Author}\n";
            formattedOutput += $"Description: {manga.Description}\n";
            formattedOutput += $"Manga Link: {manga.MangaLink}\n";
            if (manga.Genre.Count > 0)
            {
                formattedOutput += "Genres:\n";
                foreach (var genre in manga.Genre)
                {
                    formattedOutput += $"- {genre}\n"; 
                }
            }


            manga.Chapters = await CrawlMangaChaptersAsync(manga);
            formattedOutput += $"Chapters: {manga.Chapters.Count}\n";

            formattedOutput += $"Chapter Number: {manga.Chapters[0].ChapterNumber + 1}\n";
            formattedOutput += "Chapter 1 IMGs URLs :\n";
            manga.Chapters[0].ChapterImageURLs = await CrawlChapterImgUrlAsync(manga.Chapters[0]);
            foreach (var imageUrl in manga.Chapters[0].ChapterImageURLs)
            {
                formattedOutput += $"---- {imageUrl}\n";
            }
            formattedOutput += "\n\n";
            testBox.Text = formattedOutput;
        }*/
    }
}
