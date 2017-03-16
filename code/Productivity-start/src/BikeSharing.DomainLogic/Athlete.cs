using System;
using System.Collections.Generic;
using System.Linq;

namespace Training
{
    public class Athlete
    {
        private const int TweetSize = 140;
        private const int GoodCalorieBurn = 200;
        #region Hashtags
        private readonly string[] HASHTAGS = new string[]{
"#transformationtuesday",
"#mcm",
"#wcw",
"#fitfam",
"#fitspo",
"#fitness",
"#gymtime",
"#treadmill",
"#gainz",
"#workout",
"#getStrong",
"#getfit",
"#justdoit",
"#youcandoit",
"#bodybuilding",
"#fitspiration",
"#cardio",
"#ripped",
"#gym",
"#geekabs",
"#crossfit",
"#beachbody",
"#exercise",
"#weightraining",
"#training",
"#shredded",
"#abs",
"#sixpacks",
"#muscle",
"#strong",
"#lift",
"#weights",
"#Getfit",
"#weightloss",
"#wod",
"#aesthetic",
"#squad",
"#shreadding",
"#personaltrainer",
"#dreambitviral",
"#quote",
"#quotes",
"#inspiring",
"#motivation",
"#fitnessquote",
"#youcandoit",
"#justbringit",
"#dreambig",
"#success",
"#staypositive",
"#noexcuses"
    };
        #endregion

        public string Username { get; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public double BasalMetabolicRate { get => GetBasalMetabolicRate(); }
        public List<Workout> Workouts { get; set; }

        public Athlete(string username, Gender gender, int age, double weight, double height)
        {
            Username = username;
            Gender = gender;
            Age = age;
            Weight = weight;
            Height = height;
        }

        public void AddWorkout(params Workout[] workouts)
        {
            if (Workouts != null)
            {
                Workouts.AddRange(workouts);
            }
            else
            {
                Workouts = workouts.ToList();
            }
        }

        private double GetBasalMetabolicRate()
        {
            var WeightInKg = Weight * 0.453592;
            var HeightInCm = Height * 2.54;

            switch (Gender)
            {
                case Gender.Male:
                    return 66.47 + (13.75 * WeightInKg) + (5.003 * HeightInCm) - (6.755 * Age);
                case Gender.Female:
                    return 655.1 + (9.563 * WeightInKg) + (1.850 * HeightInCm) - (4.676 * Age);
                default:
                    return 0.0;
            }
        }

        public double GetCaloriesBurned(Workout workout)
        {
            switch (Gender)
            {
                case Gender.Male:
                    return (-55.0969 + (0.6309 * workout.AverageHeartRate)
                           + (0.1988 * Weight) + (0.2017 * Age) / 4.184)
                           * 60 * workout.Duration.TotalHours;
                case Gender.Female:
                    return (-20.4022 + (0.4472 * workout.AverageHeartRate)
                           - (0.1263 * Weight) + (0.074 * Age) / 4.184)
                           * 60 * workout.Duration.TotalHours;
                default:
                    return 0.0;
            }
        }

        public (Workout workout, double calories) GetWeeksBestWorkout()
        {
            var lastWeekWorkouts = Workouts.Where(w => w.Date > DateTime.Now.Date.Subtract(TimeSpan.FromDays(7)));
            var workoutWithMostCalsBurned = lastWeekWorkouts.Aggregate((w1, w2) => GetCaloriesBurned(w1) > GetCaloriesBurned(w2) ? w1 : w2);
            return (workoutWithMostCalsBurned, GetCaloriesBurned(workoutWithMostCalsBurned));
        }

        public string TweetBestWorkoutOfWeek()
        {
            var best = GetWeeksBestWorkout();
            if (best.Item2 > GoodCalorieBurn)
            {
                return Tweetify(best.Item1.Notes);
            }
            return Tweetify("Casual workout week...");
        }

        public string tweetTodaysWorkout()
        {
            if (Workouts != null)
            {
                Workout todaysWorkout = Workouts.Where(w => w.Date.Date == DateTime.Now.Date).FirstOrDefault();
                if (todaysWorkout != null)
                {
                    var bike = todaysWorkout as BikeWorkout;
                    if (bike != null)
                    {
                        return Tweetify($"I biked {bike.Distance:0.0} miles @ {bike.Pace:0.0} mph ({bike.Type}). {bike.Notes}");
                    }

                    var dist = todaysWorkout as DistanceWorkout;
                    if (dist != null)
                    {
                        return Tweetify($"I ran {dist.Distance:0.0} miles @ {dist.Pace:0.0} mph. {dist.Notes}");
                    }

                    return todaysWorkout.Notes.Length == TweetSize ? todaysWorkout.Notes : Tweetify(todaysWorkout.Notes);
                }
            }
            return null;
        }

        private string Tweetify(string msg)
        {
            if (msg.Length >= TweetSize)
                return msg.Substring(0, TweetSize-3) + "...";
            else
                return msg + GetHashTags(TweetSize - msg.Length);
        }

        private string GetHashTags(int charsLeft)
        {
            var hashes = "";
            var random = new Random();
            while (charsLeft > 0)
            {
                var r = random.Next(0, HASHTAGS.Length);
                var hashtag = " " + HASHTAGS[r];
                if (hashtag.Length > charsLeft)
                {
                    charsLeft = -1;
                }
                else
                {
                    charsLeft -= hashtag.Length;
                    hashes += hashtag;
                }
            }
            return hashes;
        }
    }
}