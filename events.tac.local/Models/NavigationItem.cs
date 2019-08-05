using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace events.tac.local.Models
{
    public class NavigationItem
    {
        public NavigationItem(string title, string url, bool active = false)
        {
            Title = title;
            URL = url;
            Active = active;
        }

        public string Title { get; private set; }
        public string URL { get; private set; }
        public bool Active { get; private set; }
    }
}