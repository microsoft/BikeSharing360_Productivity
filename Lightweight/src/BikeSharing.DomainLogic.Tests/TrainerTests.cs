using BikeSharing.Domain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Trainer.Tests
{
    [TestClass()]
    public class TrainerTests
    {
        Trainer StubTrainer;
		private void CreateTrainerWith3Workouts()
        {
			StubTrainer = new Trainer() { Goal = 100 };
			StubTrainer.RegisterWorkout(1, TimeSpan.FromMinutes(10), "Feeling sluggish today :/ #cycle");
            StubTrainer.RegisterWorkout(15, TimeSpan.FromMinutes(60), "Had a wonderful bike ride through Central Park. What a beautiful morning. Yay fall! #cycle #fromwhereiride #outsideisfree #sockgameonfleek");
            StubTrainer.RegisterWorkout(10, TimeSpan.FromMinutes(30), "Crushed it.");
        }

		private void CreateTrainerHardWorkouts()
		{
			StubTrainer = new Trainer() { Goal = 100 };
			StubTrainer.RegisterWorkout(10, TimeSpan.FromMinutes(30), "Crushed it.");
			StubTrainer.RegisterWorkout(12, TimeSpan.FromMinutes(35), "I want to ride my bicycle <3");
		}

		private void CreateTrainerEasyWorkouts()
		{
			StubTrainer = new Trainer() { Goal = 100 };
			StubTrainer.RegisterWorkout(1, TimeSpan.FromMinutes(10), "Feeling sluggish today :/ #cycle");
			StubTrainer.RegisterWorkout(2, TimeSpan.FromMinutes(15), "Should have just gotten coffee and called it a day.");
		}

		[TestMethod]
		public void EmptyTrainerMustHaveCurrentMilesToZero()
		{
			StubTrainer = new Trainer(100);
			Assert.AreEqual(0, StubTrainer.MilesTravelled);
		}

		[TestMethod]
		public void TestNoWorkoutNotes()
		{
			Assert.ThrowsException<ArgumentNullException>(() => new WorkOut(10, TimeSpan.FromMinutes(45), null));
		}

		[TestMethod]
		public void AddingWorkoutAddsMilesTravelled()
		{
			StubTrainer = new Trainer();
			StubTrainer.Goal = 100;
			StubTrainer.RegisterWorkout(10, TimeSpan.FromMinutes(20), "Alright performance this morning");
			Assert.AreEqual(10, StubTrainer.MilesTravelled);
		}

		[TestMethod]
		public void TestGetAllIntensities()
		{
			CreateTrainerWith3Workouts();

			var expected = new Dictionary<Intensity, int>();
			expected.Add(Intensity.Easy, 1);
			expected.Add(Intensity.Medium, 1);
			expected.Add(Intensity.Hard, 1);

			var actual = StubTrainer.GetAllIntensities();

			var equal = actual.All(x => expected.Contains(x));

			Assert.AreEqual(true, equal);
		}

		[TestMethod]
		public void TestGetAllIntensitiesNone()
		{
			var trainer = new Trainer();
	
			var expected = new Dictionary<Intensity, int>();
			var actual = trainer.GetAllIntensities();

			var equal = actual.All(x => expected.Contains(x));

			Assert.AreEqual(true, equal);
		}


		[TestMethod]
        public void GettingWorkoutIntensityForEasyWorkouts()
        {
            CreateTrainerWith3Workouts();
            int intensity = StubTrainer.GetWorkoutIntensityCount(Intensity.Easy);
            Assert.AreEqual(1, intensity);
        }

        [TestMethod]
        public void GettingWorkoutIntensityForHardWorkouts()
        {
            CreateTrainerWith3Workouts();
            int intensity = StubTrainer.GetWorkoutIntensityCount(Intensity.Hard);
            Assert.AreEqual(1, intensity);
        }

        [TestMethod]
        public void GettingWorkoutIntensityForMediumWorkouts()
        {
            CreateTrainerWith3Workouts();
            int intensity = StubTrainer.GetWorkoutIntensityCount(Intensity.Medium);
            Assert.AreEqual(1, intensity);
        }

		[TestMethod]
		public void TestMostFreq()
		{
			CreateTrainerEasyWorkouts();
			var expected = (Intensity.Easy, 2);
			var actual = StubTrainer.MostFrequentIntensity();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestTweetifyHard()
		{
			CreateTrainerHardWorkouts();
			var expected = new List<string>();
			expected.Add("10 mi/30 min : Crushed it.");
			expected.Add("12 mi/35 min : I want to ride my bicycle <3");

			var actual = StubTrainer.TweetifyWorkouts();

			var equal = actual.All(x => expected.Contains(x));
			Assert.AreEqual(true, equal);
		}

		[TestMethod]
		public void TestTweetifyNoNotes()
		{
			StubTrainer = new Trainer();
			
			Assert.ThrowsException<ArgumentNullException>(() => 
			{
				StubTrainer.RegisterWorkout(10, TimeSpan.FromMinutes(30), null);
				StubTrainer.TweetifyWorkouts();
			});
		}

	}
}