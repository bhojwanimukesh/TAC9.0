using TAC.Sitecore.Abstractions.Testing;

namespace events.tac.local.Tests
{
    class TestTree
    {
        private readonly ExpandoTestItemBuilder _b;

        public TestTree() : this(new ExpandoTestItemBuilder())
        {

        }
        public TestTree(ExpandoTestItemBuilder builder)
        {
            _b = builder;
        }
        public dynamic TreeA()
        {
            return
            _b.Build("sitecore",
                _b.Build("content",
                    _b.Build("home",
                        _b.Build("events",
                            _b.Build("hiking",
                                _b.Build("hikingChild1"),
                                _b.Build("hikingChild2")
                                ),
                            _b.Build("climbing",
                                _b.Build("climbingChild1"),
                                _b.Build("climbingChild2"
                                )
                            )
                        )
                    )
                )
            );
        }
    }
}
