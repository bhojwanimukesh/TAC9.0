using events.tac.local.Models;
using System;
using TAC.Sitecore.Abstractions.Interfaces;
using TAC.Sitecore.Abstractions.SitecoreImplementation;
using System.Linq;
using System.Collections.Generic;

namespace events.tac.local.Business
{
    public class NavigationBuilder
    {
        private readonly IRenderingContext _context;

        public NavigationBuilder() : this(SitecoreRenderingContext.Create()) { }

        public NavigationBuilder(IRenderingContext context)
        {
            _context = context;
        }

        public NavigationMenuItem Build()
        {
            var root = GetRoot(_context?.DatasourceOrContextItem);

            return new NavigationMenuItem
            (
                title    : root?.DisplayName,
                url      : root?.Url,
                children : root != null && _context?.ContextItem != null ? Build(root, _context.ContextItem) : null
            );
        }

        private IEnumerable<NavigationMenuItem> Build(IItem node, IItem current)
        {
            return node
                .GetChildren()
                .Select(i => new NavigationMenuItem
                (
                    title: i.DisplayName,
                    url: i.Url,
                    children: i.IsAncestorOrSelf(current) ? Build(i, current) : null
                ));
        }

        private IItem GetRoot(IItem item)
        {
            if (item != null)
            {
                //Get "Events" parent item
                if (item.Name != "Events")
                {
                    return GetRoot(item.Parent);
                }
                else
                {
                    return item;
                }
            }

            return null;
        }

    }
}