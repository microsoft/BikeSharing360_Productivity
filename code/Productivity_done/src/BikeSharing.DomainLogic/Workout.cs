using System;

namespace Training
{
    public class Workout
    {
        public DateTime Date { get; }
        public TimeSpan Duration { get; set; }
        public double AverageHeartRate { get; set; }
        public string Notes;

        public Workout(DateTime date, TimeSpan duration, double averageHeartRate, string notes)
        {
            Date = date;
            Duration = duration;
            AverageHeartRate = averageHeartRate;
            Notes = notes ?? throw new ArgumentNullException(nameof(notes));
        }
    }
}