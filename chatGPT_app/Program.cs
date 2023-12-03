using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        string apiKey = "your_openai_api_key";
        string prompt = "Tell me a joke.";

        // Send request to OpenAI API
        string response = await GetOpenAIResponse(apiKey, prompt);

        // Deserialize the JSON response
        var apiResponse = JsonSerializer.Deserialize<OpenAIResponse>(response);

        // Display the generated message
        Console.WriteLine("ChatGPT Response: " + apiResponse.choices[0].message);
    }

    static async Task<string> GetOpenAIResponse(string apiKey, string prompt)
    {
        using (var httpClient = new HttpClient())
        {
            // Set up the API endpoint and headers
            string apiUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Prepare the request payload
            var requestData = new { prompt, max_tokens = 150 };

            // Convert request data to JSON
            var jsonRequest = JsonSerializer.Serialize(requestData);

            // Make a POST request to the OpenAI API
            var response = await httpClient.PostAsync(apiUrl, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

            // Read and return the response content
            return await response.Content.ReadAsStringAsync();
        }
    }
}

// Define a class to represent the response structure from the OpenAI API
public class OpenAIResponse
{
    public Choice[] choices { get; set; }
}

public class Choice
{
    public string message { get; set; }
}
