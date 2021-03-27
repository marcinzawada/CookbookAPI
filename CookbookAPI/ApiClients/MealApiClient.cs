using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.ApiClients.Interfaces;
using CookbookAPI.DTOs.MealDB;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace CookbookAPI.ApiClients
{
    public class MealApiClient : ApiClient, IMealApiClient
    {
        public MealApiClient(IRestClient client, IConfiguration configuration) : base(client)
        {
            ChangeBaseUrl(configuration.GetSection("MealDbBaseUrl").Value);
        }

        public async Task<List<MealRecipeDto>> GetMealsRecipeByFirstLetterAsync(char firstLetter)
        {
            var response = await RequestAsync($"search.php?f={firstLetter}");
            
            var json = JObject.Parse(response.Content)["meals"];
            var dtos = JsonConvert.DeserializeObject<List<MealRecipeDto>>(json.ToString());

            return dtos;
        }

        public async Task<List<MealRecipeDto>> GetAllMealRecipes()
        {
            var alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();

            var tasks = new List<Task<List<MealRecipeDto>>>();

            var recipes = new List<MealRecipeDto>();

            foreach (var c in alphabet)
            {
                tasks.Add(GetMealsRecipeByFirstLetterAsync(c));
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                if (task.Result != null)
                {
                    recipes.AddRange(task.Result);
                }
            }

            return recipes;
        }

        public async Task<List<MealCategoryDto>> GetAllMealCategories()
        {
            var response = await RequestAsync("list.php?c=list");

            var json = JObject.Parse(response.Content)["meals"];
            var dtos = JsonConvert.DeserializeObject<List<MealCategoryDto>>(json.ToString());

            return dtos;
        }

        public async Task<List<MealAreaDto>> GetAllMealAreas()
        {
            var response = await RequestAsync("list.php?a=list");

            var json = JObject.Parse(response.Content)["meals"];
            var dtos = JsonConvert.DeserializeObject<List<MealAreaDto>>(json.ToString());

            return dtos;
        }

        public async Task<List<MealIngredientDto>> GetAllMealIngredients()
        {
            var response = await RequestAsync("list.php?i=list");

            var json = JObject.Parse(response.Content)["meals"];
            var dtos = JsonConvert.DeserializeObject<List<MealIngredientDto>>(json.ToString());

            return dtos;
        }
    }
}
