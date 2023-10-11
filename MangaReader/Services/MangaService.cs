using MangaReader.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MangaReader.Services
{
    public class MangaService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        string AllMangaAPIUrl = "https://ba6edntvfl.execute-api.ap-southeast-1.amazonaws.com/Production/all-manga";
        string MangaChaptersAPIUrl = "https://s97qgvojxe.execute-api.ap-southeast-2.amazonaws.com/Production/mangas";
        string MangaOfChapterAPIUrl = "https://s97qgvojxe.execute-api.ap-southeast-2.amazonaws.com/Production/mangas";
        string ChapterImagesOfChapterAPIUrl = "https://s97qgvojxe.execute-api.ap-southeast-2.amazonaws.com/Production/mangas";
        string AllMangasOfGenreAPIUrl = "https://s97qgvojxe.execute-api.ap-southeast-2.amazonaws.com/Production/mangas";

        public MangaService()
        {

        }

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

                    ObservableCollection<MangaModel>? manga = JsonConvert.DeserializeObject<ObservableCollection<MangaModel>>(responseBody, settings);

                    return manga!;
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

        public async Task<ObservableCollection<ChapterModel>> GetMangaChapters()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(MangaChaptersAPIUrl);
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



        public async Task<MangaModel> GetMangaOfChapter(int chapterId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(MangaOfChapterAPIUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<MangaModel>(responseBody);

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

        public async Task<ObservableCollection<ChapterImageModel>> GetChapterImagesOfChapter(int chapterId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(ChapterImagesOfChapterAPIUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var data = JsonConvert.DeserializeObject<ObservableCollection<ChapterImageModel>>(responseBody);

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


        public async Task<ObservableCollection<MangaModel>> GetAllMangasOfGenre(int genreId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(AllMangasOfGenreAPIUrl);
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
            return objectType == typeof(List<GenreModel>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JArray jsonArray = JArray.Load(reader);
            List<GenreModel> genres = jsonArray.Select(j => new GenreModel { GenreName = j.ToString() }).ToList();
            return genres;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
