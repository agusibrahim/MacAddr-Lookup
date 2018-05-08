using System;
using System.Collections.Generic;
using System.Text;
using Prism.Events;

namespace MacLookup.Events
{
    public class DownloadEvent:PubSubEvent<string>
    {
        public static string EVENT_SUCCESS = "success_bro";
        public static string EVENT_FAILED = "gagal_bro";
    }    
}
