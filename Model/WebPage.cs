using EDwI_lab1.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDwI_lab1.Model
{
    class WebPage
    {

        private string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                Uri url = null;
                if (Uri.TryCreate(_address, UriKind.Absolute, out url))
                {
                    Url = url;
                }
            }
        }

        public Uri Url { get; set; }

        public int Hits { get; set; }

        private string _content;
        public string Content
        {
            get
            {
                if (IsValid && string.IsNullOrEmpty(_content))
                {
                    _content = GatherWords.GetText(Url);
                    
                }
                return _content;
            }
        }

        public bool IsValid
        {
            get
            {
                return Url != null;
            }
        }

        public WebPage(string address)
        {
            Address = address;
        }


    }
}
