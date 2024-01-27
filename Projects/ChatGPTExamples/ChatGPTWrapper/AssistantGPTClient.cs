namespace ChatGPTWrapper
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Runtime.Remoting.Messaging;
    using System.Text;
    using System.Threading.Tasks;

    public class AssistantGPTClient
    {
        string threadId { get; set; }   
        //string role = "user";
        //string content;

        JsonSerializerSettings settings;

        private AssistantGPTThreadResponse AssistantGPTThreadResponse;
        private ThreadRunResponse ThreadRunResponse;
        private AssistantGPTRunResponse AssistantGPTRunResponse;
        private threadMessageListResponse threadMessageListResponse;

        private HttpClient _httpClient;
        private string _apiKey;
        private string _baseURL;
        private string _assistantId;
        private int _timeOut = 120;

        private List<string> context = new List<string>();

        private string _prePrompt = "";

        private string messageWithContext;

        public AssistantGPTClient()
        {
            _httpClient = new HttpClient();

            settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
        }

        public void SetApiKey(string apiKey)
        {
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("OpenAI-Beta", "assistants=v1");
        }

        public void SetBaseURL(string baseURL)
        {
            _baseURL = baseURL ?? throw new ArgumentNullException(nameof(baseURL));
        }

        public void SetPrePrompt(string preprompt)
        {
            _prePrompt = preprompt ?? throw new ArgumentNullException(nameof(preprompt));
        }

        public void SetAssistantId(string assistantId)
        {
            _assistantId = assistantId ?? throw new ArgumentNullException(nameof(_assistantId));
        }

        public void PrependContext(string context)
        {
            this.context.Add(context);
        }

        public void SetTimeOut(int timeOut)
        {
            if(timeOut > 1)
            {
                _timeOut = timeOut;
            }

        }

        public void setMessageWithContext(string message)
        {
            messageWithContext = "";

            foreach (string contextHint in this.context)
            {
                messageWithContext += contextHint + Environment.NewLine;
            }

            messageWithContext += message;

        }

        public void createThread()
        {

            if(threadId == null)
            { 
                var threadContent = _httpClient.PostAsync(_baseURL, new StringContent("")).Result;
                var thread = JsonConvert.DeserializeObject<AssistantGPTThread>(threadContent.Content.ReadAsStringAsync().Result, settings);
                threadId = thread.id;
            }

            var data = new
            {
                role = "user",
                content = messageWithContext
            };

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = _httpClient.PostAsync(_baseURL + "/" + threadId + "/messages", content).Result;

            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;

                AssistantGPTThreadResponse = JsonConvert.DeserializeObject<AssistantGPTThreadResponse>(responseContent, settings);

                


            }
            else
            { 
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}. \r \r {json}");
            }
        }

        public void submitThread()
        {
            var ThreadRunRequest = new
            {
                assistant_id = _assistantId,
                instructions = _prePrompt
            };

            var ThreadRunRequestJson = JsonConvert.SerializeObject(ThreadRunRequest);
            var ThreadRunRequestContent = new StringContent(ThreadRunRequestJson, Encoding.UTF8, "application/json");

            //submit the run
            var ThreadRunRequestResponse = _httpClient.PostAsync(_baseURL + "/" + AssistantGPTThreadResponse.thread_id + "/runs", ThreadRunRequestContent).Result;
            if(ThreadRunRequestResponse.IsSuccessStatusCode) 
            { 
                var runJsonResult = ThreadRunRequestResponse.Content.ReadAsStringAsync().Result;
                ThreadRunResponse = JsonConvert.DeserializeObject<ThreadRunResponse>(runJsonResult, settings);
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {ThreadRunRequestResponse.StatusCode}. \r \r {ThreadRunRequestJson}");
            }
        }

        public void getThreadStatus()
        {
            //thread_abc123/runs/run_abc123
            var runStatusRequest = _httpClient.GetAsync(_baseURL + "/" + AssistantGPTThreadResponse.thread_id + "/runs/" + ThreadRunResponse.id).Result;
            //return result

            var status = runStatusRequest.Content.ReadAsStringAsync().Result;
            //deserialize the response 
            AssistantGPTRunResponse = JsonConvert.DeserializeObject<AssistantGPTRunResponse>(status, settings);

        }

        public void getThreadMessageList()
        {
            var threadMessageList = _httpClient.GetAsync(_baseURL + "/" + AssistantGPTThreadResponse.thread_id + "/messages").Result;
            var threadMessageListContent = threadMessageList.Content.ReadAsStringAsync().Result;

            threadMessageListResponse = JsonConvert.DeserializeObject<threadMessageListResponse>(threadMessageListContent, settings);
        }

        public string SendMessageAsync(string message)
        {
            EnsureConfiguration();

            setMessageWithContext(message);

            //create a thread with assistant API
            createThread();


            if (AssistantGPTThreadResponse != null)
            {
                submitThread();

                DateTime loopStart = DateTime.Now;

                // loop while waiting for run status
                System.Threading.Thread.Sleep(1000);
                getThreadStatus();

                while (AssistantGPTRunResponse.status != "completed")
                {
                    System.Threading.Thread.Sleep(1000);

                    getThreadStatus();

                    if(DateTime.Now > loopStart.AddSeconds(_timeOut))
                    {
                        throw new TimeoutException();
                    }
                }
                
                if(AssistantGPTRunResponse != null)
                { 
                    this.getThreadMessageList();
                }

                threadMessageListResponseMessage threadMessageListResponseMessage = threadMessageListResponse.getLatestMessagesByRole("assistant");

                string messages = "";
                //foreach(threadMessageListResponseMessage listMessage in threadMessageListResponseMessage)
                //{
                    foreach(threadMessageListResponseContent listContent in threadMessageListResponseMessage.content)
                    { 
                        messages += listContent.text.value + Environment.NewLine;
                    }
                //}

                return messages;
            }

            return("");
        
        }

        private void EnsureConfiguration()
        {
            if (string.IsNullOrEmpty(_apiKey) || string.IsNullOrEmpty(_baseURL))
                throw new InvalidOperationException("API Key and Base URL must be set before sending a request.");
        }
    }
}
