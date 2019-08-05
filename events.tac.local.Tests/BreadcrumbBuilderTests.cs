using events.tac.local.Business;
using events.tac.local.Models;
using System.Collections.Generic;
using System.Linq;
using TAC.Sitecore.Abstractions.Testing;
using Xunit;

namespace events.tac.local.Tests
{
    public class BreadcrumbBuilderSimpleTests
    {
        [Fact]
        public void BreadcrumbBuilderCanBeCreated()
        {
            Assert.NotNull(new BreadcrumbBuilder(new TestRenderingContext()));
        }

        [Fact]
        public void BreadcrumbBuilderDoesNotReturnNullWithEmptyContext()
        {
            var builder = new BreadcrumbBuilder(null);
            Assert.NotNull(builder.Build());
        }

        [Fact]
        public void BreadcrumbBuilderDoesNotReturnNullWithEmptyHomeItem()
        {
            var context = new TestRenderingContext();
            var builder = new BreadcrumbBuilder(context);
            Assert.NotNull(builder.Build());
        }

        [Fact]
        public void BreadcrumbBuilderDoesNotReturnNullWithEmptyContextItem()
        {
            var context = new TestRenderingContext()
            {
                HomeItem = new TestItem()
            };
            var builder = new BreadcrumbBuilder(context);
            Assert.NotNull(builder.Build());
        }
    }

    public class BreadcrumbBuilderDataTests
    {       
        private readonly IEnumerable<NavigationItem> _sut;
        private readonly dynamic _sitecore;

        public BreadcrumbBuilderDataTests()
        {
            _sitecore = new TestTree().TreeA();

            var context = new TestRenderingContext()
            {
                HomeItem = _sitecore.content.home.Item,
                ContextItem = _sitecore.content.home.events.hiking.hikingChild1.Item
            };

            _sut = new BreadcrumbBuilder(context).Build();
        }

        [Fact]
        public void BreadcrumbBuilderReturnsCorrectNumberofItems()
        {
            Assert.Equal(4, _sut.Count());
        }

        [Fact]
        public void BreadcrumbBuilderReturnsUrls()
        {
            Assert.Collection(_sut,
                ni => Assert.Equal(_sitecore.content.home.Item.Url, ni.URL),
                ni => Assert.Equal(_sitecore.content.home.events.Item.Url, ni.URL),
                ni => Assert.Equal(_sitecore.content.home.events.hiking.Item.Url, ni.URL),
                ni => Assert.Equal(_sitecore.content.home.events.hiking.hikingChild1.Item.Url, ni.URL)
                );
        }

        [Fact]
        public void BreadcumbBuilderMarksCurrentAsActive()
        {
            Assert.Collection(_sut,
                ni => Assert.False(ni.Active),
                ni => Assert.False(ni.Active),
                ni => Assert.False(ni.Active),
                ni => Assert.True(ni.Active)
                );
        }

        [Fact]
        public void BreadcumbBuilderGetsCorrectText()
        {
            Assert.Collection(_sut,
                ni => Assert.Equal(_sitecore.content.home.Item.DisplayName, ni.Title),
                ni => Assert.Equal(_sitecore.content.home.events.Item.DisplayName, ni.Title),
                ni => Assert.Equal(_sitecore.content.home.events.hiking.Item.DisplayName, ni.Title),
                ni => Assert.Equal(_sitecore.content.home.events.hiking.hikingChild1.Item.DisplayName, ni.Title)
                );
        }
    }
}
