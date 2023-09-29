using MangaReader.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MangaReader
{
    public class OtakuSancCrawler
    {
        /*private readonly HttpClient _httpClient;

        *//*private readonly string _userAgentString = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36 Edg/115.0.1901.203";
        private readonly string _pageUrl = "https://saytruyenhay.com/";
        private readonly string _mangaListXPath = "//*[@class=\"manga-content\"]/div//div[@class=\"page-item-detail\"]";
        private readonly string _mangaLinkXPath = ".//a";
        private readonly string _detailNodeXPath = "//*[@class=\"col-lg-9 manga-top-main\"]";
        private readonly string _titleXPath = ".//h1[@class=\"title text-lg-left\"]";
        private readonly string _coverImageXPath = ".//img[@class=\"img-fluid not-lazy\"]";
        private readonly string _descriptionXPath = ".//div[@class=\"summary off\"]//p";
        private readonly string _genreNodesXPath = ".//div[@class=\"genres\"]//a";
        private readonly string _authorXPath = ".//table[@class=\"table table-striped table-sm text-center table-info\"]/tbody/tr[5]//a";
        private readonly string _chaptersXPath = ".//*[@class=\"read-chapter\"]";
        private readonly string _chapterNumberXPath = ".//a";
        private readonly string _chapterImgsXPath = "//div[@class=\"image-wraper\"]";*//*

        private readonly string _userAgentString = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.0.0 Safari/537.36 Edg/115.0.1901.203";
        private readonly string _pageUrl = "https://saytruyenhay.com/";
        private readonly string _mangaListXPath = "//*[@class=\"manga-content\"]/div//div[@class=\"page-item-detail\"]";
        private readonly string _mangaLinkXPath = ".//a";
        private readonly string _detailNodeXPath = "//*[@class=\"tab-summary\"]//div[@class=\"post-content\"]";
        private readonly string _titleXPath = "//div[@class=\"post-title\"]//h1";
        private readonly string _coverImageXPath = "//div[@class=\"summary_image\"]//img";
        private readonly string _descriptionXPath = "//div[@class=\"description-summary\"]/div/p";
        private readonly string _genreNodesXPath = "./div[8]//a";
        private readonly string _authorXPath = "./div[5]//div[2]";
        private readonly string _chaptersXPath = "//ul[@class=\"list-item box-list-chapter limit-height\"]//li";
        private readonly string _chapterNumberXPath = ".//a";
        private readonly string _chapterImgsXPath = "//div[@id=\"chapter_content\"]//div[@class=\"page-break \"]";

        public OtakuSancCrawler()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", _userAgentString);
        }

*//*        public async Task<ObservableCollection<MangaModel>> CrawlNewUpdatedManga()
        {
            ObservableCollection<MangaModel> mangaItems = await CrawlMangaDataAsync();
            foreach (MangaModel manga in mangaItems)
            {
                manga.Chapters = await CrawlMangaChaptersAsync(manga);
            }
            return mangaItems;
        }*//*

        public async IAsyncEnumerable<MangaModel> CrawlMangaDataAsync()
        {
            var htmlDocument = await GetHtmlDocumentAsync(_pageUrl);

            if (htmlDocument == null)
            {
                yield break; // No manga data to return
            }

            var mangaNodes = htmlDocument.DocumentNode.SelectNodes(_mangaListXPath);

            if (mangaNodes != null)
            {
                var channel = Channel.CreateUnbounded<MangaModel>();

                async Task ProcessMangaNode(HtmlNode mangaNode)
                {
                    var manga = new MangaModel();
                    manga.MangaLink = mangaNode.SelectSingleNode(_mangaLinkXPath).GetAttributeValue("href", "");

                    if (!string.IsNullOrEmpty(manga.MangaLink))
                    {
                        var mangaPageNode = (await GetHtmlDocumentAsync(manga.MangaLink)).DocumentNode;

                        if (mangaPageNode != null)
                        {
                            var detailNode = mangaPageNode.SelectSingleNode(_detailNodeXPath);
                            manga.Title = mangaPageNode.SelectSingleNode(_titleXPath).InnerText.Trim();
                            manga.CoverImageURL = mangaPageNode.SelectSingleNode(_coverImageXPath).GetAttributeValue("src", "");
                            manga.Description = mangaPageNode.SelectSingleNode(_descriptionXPath).InnerText.Trim();

                            var genreNodes = detailNode.SelectNodes(_genreNodesXPath);
                            if (genreNodes != null)
                            {
                                foreach (var genreNode in genreNodes)
                                {
                                    manga.Genre.Add(genreNode.InnerText);
                                }
                            }

                            manga.Author = detailNode.SelectSingleNode(_authorXPath).InnerText;
                        }
                    }

                    await channel.Writer.WriteAsync(manga);
                }

                var tasks = mangaNodes.Select(ProcessMangaNode).ToList();

                // Wait for all tasks to complete
                await Task.WhenAll(tasks);

                // Close the channel to indicate that no more items will be written
                channel.Writer.Complete();

                await foreach (var manga in channel.Reader.ReadAllAsync())
                {
                    yield return manga;
                }
            }
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
            var mangaPageDocument = (await GetHtmlDocumentAsync(manga.MangaLink)).DocumentNode;

            var chapterNodes = mangaPageDocument.SelectNodes(_chaptersXPath);

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

        public string PrintMangas(ObservableCollection<MangaModel> mangas)
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

            return formattedOutput;
        }

        public async Task<string> PrintManga(MangaModel manga)
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

            formattedOutput += $"Chapter Number: {manga.Chapters[0].ChapterNumber}\n";
            formattedOutput += "Chapter 1 IMGs URLs :\n";
            manga.Chapters[0].ChapterImageURLs = await CrawlChapterImgUrlAsync(manga.Chapters[0]);
            foreach (var imageUrl in manga.Chapters[0].ChapterImageURLs)
            {
                formattedOutput += $"---- {imageUrl}\n";
            }
            formattedOutput += "\n\n";
            return formattedOutput;
        }*/
    }
}
