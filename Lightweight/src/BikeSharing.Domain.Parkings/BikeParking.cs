using BikeSharing.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeSharing.Domain.Parkings
{
    public class BikeParking
    {
        public int FreeSlots { get; private set; }
        private readonly List<int> _bikes;
        public BikeParking(int slots)
        {
            FreeSlots = slots;
            _bikes = new List<int>();
        }

        public bool HasBike(int id)
        {
            return _bikes.Contains(id);
        }

        public int Park(int bikeid)
        {
            if (FreeSlots == 0)
            {
                throw new DomainException("Full!");
            }
            if (!HasBike(bikeid))
            {
                _bikes.Add(bikeid);
                FreeSlots--;
            }
            return FreeSlots;
        }

        public int Free(int bikeid)
        {
            if (HasBike(bikeid))
            {
                _bikes.Remove(bikeid);
                FreeSlots++;
            }

            return FreeSlots;
        }

    }
}
