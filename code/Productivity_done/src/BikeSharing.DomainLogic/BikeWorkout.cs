using System;
using BikeSharing.DomainLogic;

namespace Training
{

    public class BikeWorkout : DistanceWorkout
    {
        public WorkoutType Type { get; }

        public BikeWorkout(WorkoutType type, double distance, DateTime datetime, TimeSpan duration, double rate, string notes) 
            : base(distance, datetime, duration, rate, notes)
        {
            Type = type;
        }
    }
}