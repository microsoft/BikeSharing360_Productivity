using System;

namespace Training
{
    public class DistanceWorkout : Workout
    {
        public double Distance { get; }
        public double Pace { get; }
        public DistanceWorkout(double distance, DateTime datetime, TimeSpan duration, double rate, string notes) 
            : base(datetime, duration, rate, notes)
        {
            Distance = distance;
            Pace = distance / duration.TotalHours;
        }
    }
}