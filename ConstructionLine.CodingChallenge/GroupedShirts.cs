using System;
using System.Collections.Generic;

namespace ConstructionLine.CodingChallenge
{
    public class GroupedShirts
    {
        public Guid ColorId { get; set; }
        public Guid SizeId { get; set; }
        public IEnumerable<Shirt> Shirts { get; set; }

    }
}