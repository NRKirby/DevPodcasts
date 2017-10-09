using DevPodcasts.ServiceLayer.Podcast;
using Shouldly;
using Xunit;

namespace DevPodcasts.Tests
{
    public class SlugCreatorTests
    {
        [Fact]
        public void SoftwareEngineeringDailyShouldReturnCorrectSlug()
        {
            var sut = new SlugCreator();
            const string input = "Software Engineering Daily";
            const string expected = "software-engineering-daily";

            var result = sut.Create(input);

            result.ShouldBe(expected);
        }
    }
}
