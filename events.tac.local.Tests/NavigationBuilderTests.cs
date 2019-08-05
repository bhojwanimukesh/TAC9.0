using events.tac.local.Business;
using events.tac.local.Models;
using System.Linq;
using TAC.Sitecore.Abstractions.Interfaces;
using TAC.Sitecore.Abstractions.Testing;
using Xunit;

namespace events.tac.local.Tests
{
    public class NavigationBuilderSimpleTests
    {
        [Fact]
        public void NavigationBuilderCanBeInstaced()
        {
            var builder = new NavigationBuilder(null);
            Assert.NotNull(builder);
        }

        [Fact]
        public void NavigationBuilderDoesNotReturnNullWithNoContextItem()
        {
            var context = new TestRenderingContext();
            var builder = new NavigationBuilder(context);
            Assert.NotNull(builder.Build());
        }

        public void NavigationBuilderDoesNotReturnNullWithNoContextItemItem()
        {
            var context = new TestRenderingContext() { ContextItem = null };
            var builder = new NavigationBuilder(context);
            Assert.NotNull(builder.Build());
        }
        public void NavigationBuilderDoesNotReturnNullWithNoDatasourceItem()
        {
            var context = new TestRenderingContext() { DatasourceOrContextItem = null };
            var builder = new NavigationBuilder(context);
            Assert.NotNull(builder.Build());
        }

    }

    public class NavigationBuilderDataTests
    {
        private readonly NavigationMenuItem _sut;
        protected readonly dynamic _sitecore = new TestTree().TreeA();

        public NavigationBuilderDataTests()
        {
            _sut = new NavigationBuilder(GetRenderingContext()).Build();
        }

        protected TestRenderingContext GetRenderingContext()
        {
            return new TestRenderingContext()
            {
                HomeItem = _sitecore.content.home.Item,
                DatasourceOrContextItem = _sitecore.content.home.events.Item,
                ContextItem = _sitecore.content.home.events.hiking.hikingChild1.Item
            };
        }

        [Fact]
        private void CorrectTopLevelNode()
        {
            IItem events = _sitecore.content.home.events.Item;
            Assert.Equal(events.Url, _sut.URL);
            Assert.Equal(events.DisplayName, _sut.Title);
        }

        [Fact]
        private void CorrectFirstLevelInformation()
        {
            IItem hiking = _sitecore.content.home.events.hiking.Item;
            IItem climbing = _sitecore.content.home.events.climbing.Item;
            Assert.Collection(_sut.Children,
                  m => { Assert.Equal(hiking.Url, m.URL); Assert.Equal(hiking.DisplayName, m.Title); },
                  m => { Assert.Equal(climbing.Url, m.URL); Assert.Equal(climbing.DisplayName, m.Title); }
                );
        }

        [Fact]
        private void CorrectSecondLevelInformation()
        {
            IItem hiking = _sitecore.content.home.events.hiking.Item;
            IItem hiking1 = _sitecore.content.home.events.hiking.hikingChild1.Item;
            IItem hiking2 = _sitecore.content.home.events.hiking.hikingChild2.Item;
            Assert.Collection(_sut.Children.First(m => m.Title == hiking.DisplayName).Children,
                  m => { Assert.Equal(hiking1.Url, m.URL); Assert.Equal(hiking1.DisplayName, m.Title); },
                  m => { Assert.Equal(hiking2.Url, m.URL); Assert.Equal(hiking2.DisplayName, m.Title); }
                );
        }

        [Fact]
        private void DoesNotTraverseNonAncestors()
        {
            IItem climbing = _sitecore.content.home.events.climbing.Item;
            Assert.Null(_sut.Children.First(m => m.Title == climbing.DisplayName).Children);
        }

    }
    public class NavigationBuilderExcludeFromNaviationFeature : NavigationBuilderDataTests
    {
        private readonly NavigationMenuItem _sut;
        private readonly TestItem _excludedItem;

        public NavigationBuilderExcludeFromNaviationFeature()
        {
            _excludedItem = _sitecore.content.home.events.hiking.hikingChild2.Item;
            _excludedItem.SetField("excludeFromNavigation", "1");
            _sut = new NavigationBuilder(GetRenderingContext()).Build();
        }

        [Fact(Skip="optional exercise")]
        private void DoesNotShowExcludeFromNavigation()
        {            
            Assert.All(
                _sut.Children
                    .First(i => i.URL == _sitecore.content.home.events.hiking.Item.Url)
                    .Children,
                i => Assert.NotEqual(_excludedItem.Url, i.URL));
        }
    }
}