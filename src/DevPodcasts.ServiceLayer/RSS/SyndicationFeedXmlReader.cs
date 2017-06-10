using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.Syndication;
using System.Xml;

namespace DevPodcasts.ServiceLayer.RSS
{
    public class SyndicationFeedXmlReader : XmlTextReader
    {
        private readonly string[] _rss20DateTimeHints = { "pubDate" };
        private readonly string[] _atom10DateTimeHints = { "updated", "published", "lastBuildDate" };
        private bool _isRss2DateTime;
        private bool _isAtomDateTime;

        public SyndicationFeedXmlReader(Stream stream) : base(stream) { }

        public override bool IsStartElement(string localname, string ns)
        {
            _isRss2DateTime = false;
            _isAtomDateTime = false;

            if (_rss20DateTimeHints.Any(localname.Contains)) _isRss2DateTime = true;
            if (_atom10DateTimeHints.Any(localname.Contains)) _isAtomDateTime = true;

            return base.IsStartElement(localname, ns);
        }

        public override string ReadString()
        {
            string dateVal = base.ReadString();

            try
            {
                if (_isRss2DateTime)
                {
                    MethodInfo objMethod = typeof(Rss20FeedFormatter).GetMethod("DateFromString", BindingFlags.NonPublic | BindingFlags.Static);
                    //Debug.Assert(objMethod != null);
                    objMethod.Invoke(null, new object[] { dateVal, this });

                }
                if (_isAtomDateTime)
                {
                    MethodInfo objMethod = typeof(Atom10FeedFormatter).GetMethod("DateFromString", BindingFlags.NonPublic | BindingFlags.Instance);
                    //Debug.Assert(objMethod != null);
                    objMethod.Invoke(new Atom10FeedFormatter(), new object[] { dateVal, this });
                }
            }
            catch (TargetInvocationException)
            {
                DateTimeFormatInfo dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
                return DateTimeOffset.UtcNow.ToString(dtfi.RFC1123Pattern);
            }

            return dateVal;

        }
    }
}
