using System.Linq;

namespace DevPodcasts.ServiceLayer.Podcast
{
    public class SlugCreator
    {
        public string Create(string input)
        {
            var arr = input.Where(c => char.IsLetterOrDigit(c) ||
                                         char.IsWhiteSpace(c) ||
                                         c == '-').ToArray();

            return new string(arr).Replace(" ", "-").ToLower();
        }
    }
}
