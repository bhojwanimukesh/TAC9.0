using events.tac.local.Business;
using System.Collections.Generic;
using System.Linq;
using TAC.Sitecore.Abstractions.Testing;
using Xunit;
using events.tac.local.Models;
using TAC.Sitecore.Abstractions.Interfaces;

namespace events.tac.local.Tests
{

    public class RelatedEventsBasicTests
    {
        [Fact]
        public void CanBeInstantiated()
        {
            Assert.NotNull(new RelatedEventsProvider());
        }
        [Fact]
        public void DoesNotReturnNull()
        {
            Assert.NotNull(new RelatedEventsProvider(new TestRenderingContext()));
        }        
    }

    public class RelatedEventsProviderDataTests
    {
        private readonly dynamic _sitecore;
        private readonly IEnumerable<NavigationItem> _sut;

        public RelatedEventsProviderDataTests()
        {
            _sitecore = new TestTree().TreeA();

            TestItem climbing1 = _sitecore.content.home.events.climbing.climbingChild1.Item;
            climbing1.SetField("relatedEvents", new IItem[]
            {
               _sitecore.content.home.events.hiking.hikingChild1.Item,
               _sitecore.content.home.events.climbing.climbingChild2.Item
            });
            var context = new TestRenderingContext()
            {
                ContextItem = climbing1
            };
            _sut = new RelatedEventsProvider(context).GetRelatedEvents();
        }

        [Fact]
        public void ReturnsCorrectNumberOfItems()
        {
            Assert.Equal(2, _sut.Count());
        }

        [Fact]
        public void ReturnsCorrectUrls()
        {
            Assert.Collection(_sut,
               i => Assert.Equal(_sitecore.content.home.events.hiking.hikingChild1.Item.Url, i.URL),
               i => Assert.Equal(_sitecore.content.home.events.climbing.climbingChild2.Item.Url, i.URL)
            );
        }

        [Fact]
        public void ReturnsCorrectTitles()
        {
            Assert.Collection(_sut,
                i => Assert.Equal(_sitecore.content.home.events.hiking.hikingChild1.Item.DisplayName, i.Title),
                i => Assert.Equal(_sitecore.content.home.events.climbing.climbingChild2.Item.DisplayName, i.Title)
            );
        }
    }
    public class RelatedEventsProvideEmptyFieldEETests
    {
        protected readonly IEnumerable<NavigationItem> _sut;
        public RelatedEventsProvideEmptyFieldEETests()
        {
            TestRenderingContext context = PrepareContext();
            _sut = new RelatedEventsProvider(context).GetRelatedEvents();
        }

        protected virtual TestRenderingContext PrepareContext()
        {
            dynamic sitecore = new TestTree().TreeA();
            TestItem climbing1 = sitecore.content.home.events.climbing.climbingChild1.Item;
            climbing1.SetField("relatedEvents", "");
            var context = new TestRenderingContext()
            {
                IsExperienceEditorEditing = true,
                ContextItem = climbing1
            };
            return context;
        }

        [Fact(Skip="optional excercise")]
        private void ReturnsSingleElement()
        {
            Assert.Equal(1, _sut.Count());
        }
        [Fact(Skip = "optional excercise")]
        private void SingleRelatedEventHasHashUrl()
        {
            Assert.Equal("#", _sut.First().URL);
        }
        [Fact(Skip = "optional excercise")]
        private void SingleRelatedEventHasCorrectTitle()
        {
            Assert.Equal("No related events. Click here to edit.", _sut.First().Title);
        }

    }

    public class RelatedEventsProvideEmptyFieldNormalTests : RelatedEventsProvideEmptyFieldEETests
    {        
        protected override TestRenderingContext PrepareContext()
        {
            var context = base.PrepareContext();
            context.IsExperienceEditorEditing = false;
            return context;
        }

        [Fact(Skip = "optional excercise")]
        private void ReturnsNoElements()
        {
            Assert.Empty(_sut);
        }        

    }


}
