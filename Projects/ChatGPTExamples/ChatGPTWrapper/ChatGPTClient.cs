namespace ChatGPTWrapper
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class ChatGPTClient
    {
        private HttpClient _httpClient;
        private string _apiKey;
        private string _baseURL; 
        private List<string> context = new List<string>();

        private string _model = "";
        private string _prePrompt = "";


        public ChatGPTClient()
        {
            _httpClient = new HttpClient();
        }

        public void SetApiKey(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

        }

        public void SetBaseURL(string baseURL)
        {
            _baseURL = baseURL ?? throw new ArgumentNullException(nameof(baseURL));
        }

        public void SetModel(string model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));

        }

        public void SetPrePrompt(string preprompt)
{
            _prePrompt = preprompt ?? throw new ArgumentNullException(nameof(preprompt));
        }

        public void PrependContext(string context)
        {
            this.context.Add(context);
        }

        public string SendMessageAsync(string message)
        {
            EnsureConfiguration();

            string messageWithContext = "";

            foreach (string contextHint in this.context)
            {
                messageWithContext += contextHint + Environment.NewLine;
            }

            messageWithContext += message;

            var systemRoleMessage = new
            {
                role = "system",
                content = _prePrompt

            };

            var userRoleMessage = new
            {
                role = "user",
                content = messageWithContext

            };

            List<Object> messageList = new List<object>();
            messageList.Add(systemRoleMessage);
            messageList.Add(userRoleMessage);

            var requestBody = new
            {
                model = _model,
                messages = messageList,
                temperature = .7
            };


            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = _httpClient.PostAsync(_baseURL, content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;

                ChatGPTResponse ChatGPTResponse =  JsonConvert.DeserializeObject<ChatGPTResponse>(responseContent);

                return ChatGPTResponse.choices[0].message.content;
  
            }

            throw new HttpRequestException($"Request failed with status code {response.StatusCode}. \r \r {json}");
        }

        private void EnsureConfiguration()
        {
            if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_baseURL) || string.IsNullOrEmpty(_model))
                throw new InvalidOperationException("API Key and Base URL must be set before sending a request.");
        }

        // Add more methods here as needed for additional features, such as handling different types of requests, etc.
    }
}
