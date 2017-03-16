using System;
using BikeSharing.DomainLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Training;

namespace Trainer.Tests
{
    [TestClass]
    public class TestWorkout
    {
        Workout _workout;

        public void Create(string workoutName)
        {
            switch (workoutName)
            {
                case "bike":
                    _workout = CreateBikeWorkout();
                    break;
                case "distance":
                    _workout = CreateDistanceWorkout();
                    break;
                case "workout":
                    _workout = CreateWorkout();
                    break;
                default:
                    break;

            }
        }

        private Workout CreateBikeWorkout()
        {
            return new BikeWorkout(WorkoutType.Outdoor, 21.6, DateTime.Now.AddDays(-5), TimeSpan.FromMinutes(83),  117, "Biking to Red Hook Brewery on the Burke-Gilman. What a day to be alive!");
        }

        private Workout CreateDistanceWorkout()
        {
            return new DistanceWorkout(5.2, DateTime.Now.AddDays(-2), TimeSpan.FromMinutes(37), 112, "5K run around Greenlake with the bf ;)");
        }

        private Workout CreateWorkout()
        {
            return new Workout(DateTime.Now, TimeSpan.FromMinutes(25), 93, "Pumpin' some iron.");
        }

        [TestMethod]
        public void TestBikeWorkoutPace()
        {
            Create("bike");
            var bike = (BikeWorkout)_workout;
            Assert.AreEqual(15.6144578, bike.Pace, .000001);
        }

        [TestMethod]
        public void TestBikeWorkoutNotes()
        {
            Create("bike");
            var bike = (BikeWorkout)_workout;
            Assert.AreEqual(71, bike.Notes.Length);
        }

        [TestMethod]
        public void TestBikeWorkoutHeartRate()
        {
            Create("bike");
            var bike = (BikeWorkout)_workout;
            Assert.AreEqual(117, bike.AverageHeartRate);
        }

        [TestMethod]
        public void TestDistanceWorkoutPace()
        {
            Create("distance");
            var distance = (DistanceWorkout)_workout;
            Assert.AreEqual(8.4324324, distance.Pace, .000001);
        }

        [TestMethod]
        public void TestDistanceWorkoutHeartRate()
        {
            Create("distance");
            var distance = (DistanceWorkout)_workout;
            Assert.AreEqual(112, distance.AverageHeartRate);
        }

        [TestMethod]
        public void TestWorkoutNotes()
        {
            Create("workout");
            var workout = (Workout)_workout;
            Assert.AreEqual(18, workout.Notes.Length);
        }
    }
}