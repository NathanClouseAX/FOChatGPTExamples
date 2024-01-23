namespace ChatGPTWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using System.Linq;


    public class threadMessageListResponse
    {
        public string _object { get; set; }
        public threadMessageListResponseMessage[] data { get; set; }
        public string first_id { get; set; }
        public string last_id { get; set; }
        public bool has_more { get; set; }

        public threadMessageListResponseMessage getLatestMessagesByRole(string role)
        {
            var listByRole = data.Where(data => data.role == role);

            return listByRole.ToArray<threadMessageListResponseMessage>()[0];
        }

        public threadMessageListResponseMessage[] getAllMessagesByRole(string role)
        {
            var listByRole = data.Where(data => data.role == role);

            return listByRole.ToArray<threadMessageListResponseMessage>();
        }
    }

    public class threadMessageListResponseMessage
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created_at { get; set; }
        public string thread_id { get; set; }
        public string role { get; set; }
        public threadMessageListResponseContent[] content { get; set; }
        public object[] file_ids { get; set; }
        public string assistant_id { get; set; }
        public string run_id { get; set; }
        public threadMessageListResponseMetadata metadata { get; set; }


    }

    public class threadMessageListResponseMetadata
    {
    }

    public class threadMessageListResponseContent
    {
        public string type { get; set; }
        public threadMessageListResponseText text { get; set; }
    }

    public class threadMessageListResponseText
    {
        public string value { get; set; }
        public object[] annotations { get; set; }
    }

}
