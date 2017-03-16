using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BikeSharing.Domain.Parkings;

namespace BikeSharing.Domain.Parking.Tests
{
    [TestClass]
    public class Parking_Tests
    {
        [TestMethod]
        public void CannotParkBikeTwice()
        {
            var slots = 10;
            var parking = new BikeParking(slots);
            parking.Park(100);
            parking.Park(100);
            Assert.AreEqual(slots - 1, parking.FreeSlots);
        }
    }
}
