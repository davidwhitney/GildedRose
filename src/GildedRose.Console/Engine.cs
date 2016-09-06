using System;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Console
{
    public class Engine
    {
        private readonly IList<Item> _items;
        private List<string> Immutable { get; set; } = new List<string>();
        private List<string> IncreasesInQuality { get; set; } = new List<string>();
        private List<string> AccelleratesInQuality { get; set; } = new List<string>();
        private List<string> DegradesFast { get; set; } = new List<string>();
        private readonly Dictionary<Func<Item, bool>, Action<Item>> _rules;

        public Engine(IList<Item> items)
        {
            _items = items;
            Immutable.Add("Sulfuras, Hand of Ragnaros");
            IncreasesInQuality.Add("Aged Brie");
            AccelleratesInQuality.Add("Backstage passes");
            DegradesFast.Add("Conjured");

            _rules = new Dictionary<Func<Item, bool>, Action<Item>>
            {
                {
                    i => i.Name.StartsWithAny(IncreasesInQuality) || i.Name.StartsWithAny(AccelleratesInQuality),
                    i => i.Quality = i.Quality + 2
                },
                {i => i.Name.StartsWithAny(IncreasesInQuality) && i.SellIn < 0, i => i.Quality++},
                {i => i.Name.StartsWithAny(AccelleratesInQuality) && i.SellIn < 11, i => i.Quality++},
                {i => i.Name.StartsWithAny(AccelleratesInQuality) && i.SellIn < 6, i => i.Quality++},
                {i => i.Name.StartsWithAny(AccelleratesInQuality) && i.SellIn < 0, i => i.Quality = 0},
                {
                    i => !IncreasesInQuality.Contains(i.Name) && !AccelleratesInQuality.Contains(i.Name) && i.SellIn < 0,
                    i => i.Quality--
                },
                {i => i.Name.StartsWithAny(DegradesFast), i => i.Quality--}
            };
        }

        public void IncrementAge()
        {
            foreach (var item in _items)
            {
                if (Immutable.Any(x=>x.StartsWith(item.Name)))
                {
                    continue;
                }

                item.SellIn--;
                item.Quality = item.Quality - 1;

                foreach (var rule in _rules.Where(r=>r.Key(item)))
                {
                    rule.Value(item);
                }
            }

            CapQuality();
        }

        private void CapQuality()
        {
            foreach (var item in _items.Where(x => x.Quality > 50))
            {
                item.Quality = 50;
            }

            foreach (var item in _items.Where(x => x.Quality < 0))
            {
                item.Quality = 0;
            }
        }
    }

    public static class ListStringExtensions
    {
        public static bool StartsWithAny(this string testString, IEnumerable<string> collectionOfStubs)
        {
            return collectionOfStubs.Any(testString.StartsWith);
        }
    }
}