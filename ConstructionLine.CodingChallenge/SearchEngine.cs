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
                        ColorId = color.Id,
                        SizeId = size.Id,
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

            var optionColorIds = options.Colors.Select(c => c.Id);
            var optionSizeIds = options.Sizes.Select(c => c.Id);

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
                    var matchedShirts = _groupedShirts.FirstOrDefault(s => s.ColorId == colorOption.Id && s.SizeId == sizeOption.Id);
                    searchResults.Shirts.AddRange(matchedShirts.Shirts);
                }
            }

            foreach (var size in Size.All)
            {
                searchResults.SizeCounts.Add(new SizeCount()
                {
                    Size = size,
                    Count = _groupedShirts.Where(s => s.SizeId == size.Id && optionColorIds.Contains(s.ColorId)).SelectMany(g => g.Shirts).Count()
                });
            }

            foreach (var color in Color.All)
            {
                searchResults.ColorCounts.Add(new ColorCount()
                {
                    Color = color,
                    Count = _groupedShirts.Where(s => s.ColorId == color.Id && optionSizeIds.Contains(s.SizeId)).SelectMany(g => g.Shirts).Count()
                });
            }

            return searchResults;
        }
    }
}