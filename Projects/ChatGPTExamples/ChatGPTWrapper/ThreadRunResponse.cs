﻿namespace ChatGPTWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ThreadRunResponse
    {
        public string id { get; set; }
        public string _object { get; set; }
        public int created_at { get; set; }
        public string assistant_id { get; set; }
        public string thread_id { get; set; }
        public string status { get; set; }
        public object started_at { get; set; }
        public int expires_at { get; set; }
        public object cancelled_at { get; set; }
        public object failed_at { get; set; }
        public object completed_at { get; set; }
        public object last_error { get; set; }
        public string model { get; set; }
        public string instructions { get; set; }
        public object[] tools { get; set; }
        public object[] file_ids { get; set; }
        public Metadata metadata { get; set; }
        public object usage { get; set; }
    }



}
