using System;
using System.Collections.Generic;
using System.Linq;

namespace GildedRose.Console
{
    public class Engine
    {
        private readonly IList<Item> _items;
        private readonly List<string> _immutable;
        private readonly List<string> _agesWell;
        private readonly List<string> _fastAgeWell;
        private readonly List<string> _degradesTwiceAsFast;

        public Engine(IList<Item> items)
        {
            _items = items;
            _immutable = new List<string>
            {
                "Sulfuras, Hand of Ragnaros"
            };
            _agesWell = new List<string>
            {
                "Aged Brie",
            };
            _fastAgeWell = new List<string>
            {
                "Backstage passes to a TAFKAL80ETC concert"
            };
            _degradesTwiceAsFast = new List<string>
            {
                "Conjured"
            };
        }

        public void IncrementAge()
        {
            foreach (var item in _items)
            {
                if (_immutable.Any(x=>x.StartsWith(item.Name)))
                {
                    continue;
                }

                item.SellIn--;
                item.Quality = item.Quality - 1;

                var rules = new Dictionary<Func<Item, bool>, Action<Item>>
                {
                    {i => _agesWell.Contains(i.Name) || _fastAgeWell.Contains(i.Name), i => i.Quality = i.Quality + 2},
                    {i => _fastAgeWell.Contains(i.Name) && i.SellIn < 11, i => i.Quality++},
                    {i => _fastAgeWell.Contains(i.Name) && i.SellIn < 6, i => i.Quality++},
                    {i => _agesWell.Contains(i.Name) && i.SellIn < 0, i => i.Quality++},
                    {i => _fastAgeWell.Contains(i.Name) && i.SellIn < 0, i => i.Quality = 0},
                    {
                        i => !_agesWell.Contains(item.Name) && !_fastAgeWell.Contains(item.Name) && item.SellIn < 0,
                        i => i.Quality--
                    },
                    {i => _degradesTwiceAsFast.Any(stub => item.Name.StartsWith(stub)), i => i.Quality--}
                };

                foreach (var rule in rules)
                {
                    if (rule.Key(item))
                    {
                        rule.Value(item);
                    }
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
}