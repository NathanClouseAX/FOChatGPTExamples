namespace ChatGPTWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class AssistantGPTThreadResponse
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created_at { get; set; }
        public string thread_id { get; set; }
        public string role { get; set; }
        public Content[] content { get; set; }
        public object[] file_ids { get; set; }
        public object assistant_id { get; set; }
        public object run_id { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Metadata
    {
    }

    public class Content
    {
        public string type { get; set; }
        public Text text { get; set; }
    }

    public class Text
    {
        public string value { get; set; }
        public object[] annotations { get; set; }
    }

}
