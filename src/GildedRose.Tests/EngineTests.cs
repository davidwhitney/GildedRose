using System.Collections.Generic;
using GildedRose.Console;
using NUnit.Framework;

namespace GildedRose.Tests
{
    [TestFixture]
    public class EngineTests
    {
        private List<Item> _items;
        private Engine _engine;

        [SetUp]
        public void Setup()
        {
            _items = new List<Item>();
            _engine = new Engine(_items);
        }

        [Test]
        public void IncrementAge_SellInTimerDecreases()
        {
            _items.Add(new Item {Name = "Thing", Quality = 5, SellIn = 1});

            _engine.IncrementAge();

            Assert.That(_items[0].SellIn, Is.EqualTo(0));
        }

        [Test]
        public void IncrementAge_QualityDecreases()
        {
            _items.Add(new Item {Name = "Thing", Quality = 5, SellIn = 1});

            _engine.IncrementAge();

            Assert.That(_items[0].Quality, Is.EqualTo(4));
        }

        [Test]
        public void IncrementAge_ConjuredItem_QualityDecreasesTwiceAsFast()
        {
            _items.Add(new Item {Name = "Conjured Thing", Quality = 5, SellIn = 1});

            _engine.IncrementAge();

            Assert.That(_items[0].Quality, Is.EqualTo(3));
        }

        [Test]
        public void IncrementAge_SellByHasPast_QualityDecreasesTwiceAsFast()
        {
            _items.Add(new Item {Name = "Thing", Quality = 5, SellIn = -1});

            _engine.IncrementAge();

            Assert.That(_items[0].Quality, Is.EqualTo(3));
        }

        [Test]
        public void AgedBrie_GetsBetterWithAge()
        {
            _items.Add(new Item {Name = "Aged Brie", Quality = 5, SellIn = 1});

            _engine.IncrementAge();

            Assert.That(_items[0].Quality, Is.EqualTo(6));
        }

        [Test]
        public void IncrementAge_IncreasingQualityItem_CantGetBetterThan50()
        {
            _items.Add(new Item {Name = "Aged Brie", Quality = 50, SellIn = 10});

            _engine.IncrementAge();

            Assert.That(_items[0].Quality, Is.EqualTo(50));
        }

        [Test]
        public void IncrementAge_BackStagePass_IncreasesBy2WithinTenDaysOfSellIn()
        {
            _items.Add(new Item {Name = "Backstage passes to a TAFKAL80ETC concert", Quality = 0, SellIn = 10});

            _engine.IncrementAge();

            Assert.That(_items[0].Quality, Is.EqualTo(2));
        }

        [Test]
        public void IncrementAge_BackStagePass_IncreasesBy3WithinFiveDaysOfSellIn()
        {
            _items.Add(new Item {Name = "Backstage passes to a TAFKAL80ETC concert", Quality = 0, SellIn = 5});

            _engine.IncrementAge();

            Assert.That(_items[0].Quality, Is.EqualTo(3));
        }

        [Test]
        public void IncrementAge_BackStagePass_ZeroQualityOnceExpired()
        {
            _items.Add(new Item {Name = "Backstage passes to a TAFKAL80ETC concert", Quality = 10, SellIn = 0});

            _engine.IncrementAge();

            Assert.That(_items[0].Quality, Is.EqualTo(0));
        }

        [Test]
        public void IncrementAge_LegendaryItem_DoesNotDegrade()
        {
            _items.Add(new Item {Name = "Sulfuras, Hand of Ragnaros", Quality = 40, SellIn = 10});

            _engine.IncrementAge();

            Assert.That(_items[0].Quality, Is.EqualTo(40));
            Assert.That(_items[0].SellIn, Is.EqualTo(10));
        }
    }
}