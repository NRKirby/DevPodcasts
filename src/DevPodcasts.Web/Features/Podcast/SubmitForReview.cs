using DevPodcasts.DataLayer.Models;
using DevPodcasts.Logging;
using DevPodcasts.Models;
using DevPodcasts.Models.DTOs;
using DevPodcasts.ServiceLayer.Email;
using DevPodcasts.ServiceLayer.RSS;
using MediatR;
using System;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace DevPodcasts.Web.Features.Podcast
{
    public class SubmitForReview
    {
        public class Command : IRequest<Model>
        {
            public string RssFeedUrl { get; set; }
        }

        public class Model
        {
            public string RssFeedUrl { get; set; }

            public SuccessResult SuccessResult { get; set; }
        }

        public class CommandHandler : IAsyncRequestHandler<Command, Model>
        {
            private readonly ApplicationDbContext _context;
            private readonly RssParser _rssParser;
            private readonly PodcastEmailService _podcastEmailService;
            private readonly ILogger _logger;

            public CommandHandler(ApplicationDbContext context, 
                RssParser rssParser, 
                PodcastEmailService podcastEmailService,  
                ILogger logger)
            {
                _context = context;
                _rssParser = rssParser;
                _podcastEmailService = podcastEmailService;
                _logger = logger;
            }

            public async Task<Model> Handle(Command message)
            {
                var model = new Model();

                var rssFeedUrl = message.RssFeedUrl;
                if (!IsValidUrl(rssFeedUrl))
                {
                    model.SuccessResult = SuccessResult.InvalidUrl;
                    _logger.Error(rssFeedUrl + " : Invalid URL", null);
                    return model;
                }

                if (PodcastExistsInDatabase(rssFeedUrl))
                {
                    model.SuccessResult = SuccessResult.AlreadyExists;
                    _logger.Info(rssFeedUrl + " :: Podcast feed already exists");
                    return model;
                }

                var podcastDto = GetPodcastForReview(message.RssFeedUrl);
                if (podcastDto.SuccessResult == SuccessResult.Success)
                {
                    await SavePodcastForReview(podcastDto);
                    await _podcastEmailService.SendPodcastSubmittedEmailAsync(podcastDto.Title);
                }

                model.RssFeedUrl = podcastDto.FeedUrl;
                model.SuccessResult = podcastDto.SuccessResult;

                return model;
            }

            private bool PodcastExistsInDatabase(string rssFeedUrl)
            {
                return _context
                    .Podcasts
                    .AsNoTracking()
                    .Where(i => i.IsApproved == true)
                    .Any(i => i.FeedUrl == rssFeedUrl);
            }

            public async Task SavePodcastForReview(PodcastDto dto)
            {
                var podcast = new DataLayer.Models.Podcast
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    ImageUrl = dto.ImageUrl,
                    FeedUrl = dto.FeedUrl,
                    SiteUrl = dto.SiteUrl
                };
                _context.Podcasts.Add(podcast);
                await _context.SaveChangesAsync();
            }

            public PodcastDto GetPodcastForReview(string rssFeedUrl)
            {
                var dto = new PodcastDto
                {
                    FeedUrl = rssFeedUrl
                };

                var feed = new RssFeed();
                try
                {
                    feed = _rssParser.ParseRssFeed(rssFeedUrl);
                }
                catch (Exception ex)
                {
                    dto.SuccessResult = SuccessResult.Error;
                }

                if (feed == null) return dto;
                var siteUrl = GetSiteUrl(feed.SyndicationFeed);
                dto.Title = feed.SyndicationFeed.Title?.Text;
                dto.Description = feed.SyndicationFeed.Description?.Text;
                dto.ImageUrl = feed.SyndicationFeed.ImageUrl?.AbsoluteUri;
                dto.FeedUrl = rssFeedUrl;
                dto.SiteUrl = siteUrl;
                dto.SuccessResult = SuccessResult.Success;

                return dto;
            }

            private static bool IsValidUrl(string url)
            {
                bool result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                return result;
            }

            private static string GetSiteUrl(SyndicationFeed feed)
            {
                return feed.Links.FirstOrDefault(i => i.RelationshipType == "alternate")?.Uri.ToString();
            }
        }
    }
}