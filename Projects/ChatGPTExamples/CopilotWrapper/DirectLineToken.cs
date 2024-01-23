namespace CopilotWrapper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class DirectLineToken
    {
        public string token { get; set; }
        public int expires_in { get; set; }
        public string conversationId { get; set; }
    }

}
