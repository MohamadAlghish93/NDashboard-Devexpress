using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.Xpo;

namespace NDashboard.DAL
{
    [DeferredDeletion(false)]
    public class TSOUTERREPORT : XPCustomObject
    {
        string url;
        string name;
        byte[] layout;

        public TSOUTERREPORT(Session session)
            : base(session)
        {
        }

        [Key]
        public string Url
        {
            get { return url; }
            set { SetPropertyValue("Url", ref url, value); }
        }

        public byte[] Layout
        {
            get { return layout; }
            set { SetPropertyValue("Layout", ref layout, value); }
        }
    }
}