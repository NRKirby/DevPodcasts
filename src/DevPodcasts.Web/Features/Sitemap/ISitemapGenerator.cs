using System.Collections.Generic;
using System.Xml.Linq;

namespace DevPodcasts.Web.Features.Sitemap
{
    public interface ISitemapGenerator
    {
        XDocument GenerateSiteMap(IEnumerable<ISitemapItem> items);
    }
}
