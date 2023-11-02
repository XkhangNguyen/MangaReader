using MangaReader.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MangaReader.Services
{
    public class MangaService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        readonly string AllMangaAPIUrl = "https://2yd98ioj81.execute-api.ap-southeast-1.amazonaws.com/dev/all-mangas";
        readonly string AllGerneAPIUrl = "https://2yd98ioj81.execute-api.ap-southeast-1.amazonaws.com/dev/genres";
        readonly string MangaChaptersAPIUrl = "https://2yd98ioj81.execute-api.ap-southeast-1.amazonaws.com/dev/chapters/";
        readonly string ChapterImagesOfChapterAPIUrl = "https://2yd98ioj81.execute-api.ap-southeast-1.amazonaws.com/dev/images/";
        readonly string AllMangasOfGenreAPIUrl = "https://2yd98ioj81.execute-api.ap-southeast-1.amazonaws.com/dev/all-mangas/genre/";
        public MangaService(){ }

        public async Task<ObservableCollection<MangaModel>> GetAllMangas()
        {
            try
            {
                // Create settings with the custom converter
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    Converters = { new GenreConverter() }
                };

                HttpResponseMessage response = await _httpClient.GetAsync(AllMangaAPIUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    ObservableCollection<MangaModel>? mangas = JsonConvert.DeserializeObject<ObservableCollection<MangaModel>>(responseBody, settings);

                    return mangas!;
                }
                else
                {
                    // Handle API error (e.g., non-200 status code)
                    return null!;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any network-related errors
                return null!;
            }
        }

        public async Task<ObservableCollection<GenreModel>> GetAllGenres()
        {
            try
            {

                HttpResponseMessage response = await _httpClient.GetAsync(AllGerneAPIUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    ObservableCollection<GenreModel>? genres = JsonConvert.DeserializeObject<ObservableCollection<GenreModel>>(responseBody);

                    return genres!;
                }
                else
                {
                    // Handle API error (e.g., non-200 status code)
                    return null!;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any network-related errors
                return null!;
            }
        }

        public async Task<ObservableCollection<ChapterModel>> GetMangaChapters(int mangaId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(MangaChaptersAPIUrl + mangaId.ToString());
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<ObservableCollection<ChapterModel>>(responseBody);

                    return data!;
                }
                else
                {
                    // Handle API error (e.g., non-200 status code)
                    return null!;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any network-related errors
                return null!;
            }
        }

        public async Task<ObservableCollection<string>> GetChapterImagesOfChapter(int chapterId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(ChapterImagesOfChapterAPIUrl + chapterId.ToString());
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Parse the JSON string into a dynamic object (JObject)
                    JObject responseObj = JObject.Parse(responseBody);

                    // Get the chapter_image_urls as a string
                    string chapterImageUrls = responseObj["chapter_image_urls"]!.ToString();

                    // Split the chapter_image_urls into a string array
                    string[] imageUrls = chapterImageUrls.Split(';');

                    ObservableCollection<string> imageUrlsCollection = new ObservableCollection<string>(imageUrls);

                    return imageUrlsCollection;
                }
                else
                {
                    // Handle API error (e.g., non-200 status code)
                    return null!;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any network-related errors
                return null!;
            }
        }


        public async Task<ObservableCollection<MangaModel>> GetAllMangasOfGenre(int genreId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(AllMangasOfGenreAPIUrl + genreId);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<ObservableCollection<MangaModel>>(responseBody);

                    return data!;
                }
                else
                {
                    // Handle API error (e.g., non-200 status code)
                    return null!;
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle any network-related errors
                return null!;
            }
        }
    }

    class GenreConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ObservableCollection<GenreModel>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray jsonArray = JArray.Load(reader);

            ObservableCollection<GenreModel> genres = new ObservableCollection<GenreModel>(
                jsonArray.Select(j => new GenreModel { GenreName = j["genre_name"].ToString() })
            ); 

            return genres;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
