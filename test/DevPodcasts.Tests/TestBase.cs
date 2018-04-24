using DevPodcasts.DataLayer.Models;
using Respawn;
using System;
using System.Configuration;

namespace DevPodcasts.Tests
{
    public abstract class TestBase : IDisposable
    {
        private static Checkpoint _checkpoint;
        protected ApplicationDbContext Context;

        protected TestBase()
        {
            Context = GetTestContext();
            _checkpoint = new Checkpoint();
        }

        private static ApplicationDbContext GetTestContext()
        {
            return new ApplicationDbContext(ConfigurationManager.ConnectionStrings["IntegrationTest"].ConnectionString);
        }

        public void Dispose()
        {
            _checkpoint.Reset(ConfigurationManager.ConnectionStrings["IntegrationTest"].ConnectionString)
                .GetAwaiter()
                .GetResult();

            Context?.Dispose();
        }
    }
}
