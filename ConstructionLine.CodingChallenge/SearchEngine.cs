using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<GroupedShirts> _groupedShirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _groupedShirts = new List<GroupedShirts>();

            foreach (var color in Color.All)
            {
                foreach (var size in Size.All)
                {
                    _groupedShirts.Add(new GroupedShirts()
                    {
                        Color = color,
                        Size = size,
                        Shirts = shirts.Where(s => s.Size.Id == size.Id && s.Color.Id == color.Id)
                    });
                }
            }
        }

        public SearchResults Search(SearchOptions options)
        {
            if (!options.Sizes.Any())
                options.Sizes = Size.All;

            if (!options.Colors.Any())
                options.Colors = Color.All;

            var searchResults = new SearchResults()
            {
                Shirts = new List<Shirt>(),
                ColorCounts = new List<ColorCount>(),
                SizeCounts = new List<SizeCount>()
            };

            foreach (var colorOption in options.Colors)
            {
                foreach (var sizeOption in options.Sizes)
                {
                    var matchedShirts = _groupedShirts.FirstOrDefault(s => s.Color == colorOption && s.Size == sizeOption);
                    searchResults.Shirts.AddRange(matchedShirts.Shirts);
                }
            }

            foreach (var size in Size.All)
            {
                searchResults.SizeCounts.Add(new SizeCount()
                {
                    Size = size,
                    Count = _groupedShirts.Where(s => s.Size == size && options.Colors.Contains(s.Color)).SelectMany(g => g.Shirts).Count()
                });
            }

            foreach (var color in Color.All)
            {
                searchResults.ColorCounts.Add(new ColorCount()
                {
                    Color = color,
                    Count = _groupedShirts.Where(s => s.Color == color && options.Sizes.Contains(s.Size)).SelectMany(g => g.Shirts).Count()
                });
            }

            return searchResults;
        }
    }

    public class GroupedShirts
    {
        public Color Color { get; set; }
        public Size Size { get; set; }
        public IEnumerable<Shirt> Shirts { get; set; }

    }
}