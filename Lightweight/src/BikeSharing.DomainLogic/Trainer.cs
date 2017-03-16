using BikeSharing.Domain.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Trainer
{
	public class Trainer
	{
		private List<WorkOut> _workOuts;
		public int Goal { get; set; }

		public int MilesTravelled
		{
			get
			{
				int count = 0;
				foreach (var work in _workOuts)
				{
					count += work.Miles;
				}
				return count;
			}
		}

		public Trainer(int goal)
		{
			_workOuts = new List<WorkOut>();
			Goal = goal; ;
		}

		public Trainer()
		{
			_workOuts = new List<WorkOut>();
		}

		public void RegisterWorkout(int miles, TimeSpan duration, string notes)
		{
			_workOuts.Add(new WorkOut(miles, duration, notes));
		}

		public bool HasMetGoal()
		{
			if (MilesTravelled == Goal)
			{
				return true;
			}
			return false;
		}

		public static double GetMilePace(WorkOut workout) => workout.Duration.TotalMinutes / (double)workout.Miles;

		public WorkOut GetMostMilesTraveled()
		{
			int mostMiles = 0;
			WorkOut FurthestWorkout = null;
			foreach (var workout in _workOuts)
			{
				if (workout.Miles > mostMiles)
				{
					FurthestWorkout = workout;
					mostMiles = workout.Miles;
				}
			}
			return FurthestWorkout;
		}

		public static Intensity GetWorkoutIntensity(WorkOut workout)
		{
			double milePace = GetMilePace(workout);

			if (workout == null)
				return Intensity.None;

			if (milePace < 3.5)
				return Intensity.Hard;
			else if (milePace < 6.0)
				return Intensity.Medium;
			else
				return Intensity.Easy;
		}

		public int GetWorkoutIntensityCount(Intensity desiredIntensity)
		{
			int intensityCount = 0;
			foreach (var workout in _workOuts)
			{
				var intensity = GetWorkoutIntensity(workout);
				if (desiredIntensity == intensity)
				{
					intensityCount++;
				}
			}
			return intensityCount;
		}

		public Dictionary<Intensity, int> GetAllIntensities()
		{
			Dictionary<Intensity, int> dictionary = new Dictionary<Intensity, int>();
			foreach (var workout in _workOuts)
			{
				var intensity = GetWorkoutIntensity(workout);
				if (dictionary.ContainsKey(intensity))
					dictionary[intensity] += 1;
				else
					dictionary.Add(intensity, 1);
			}

			return dictionary;
		}

		public (Intensity, int) MostFrequentIntensity()
		{
			var IntensityDictionary = GetAllIntensities();
			var highestCount = (intensity:Intensity.None, count:0);
			foreach (var (key, val) in IntensityDictionary)
			{
				if (val > highestCount.Item2)
				{
					highestCount = (key, val);
				}
			}
			return highestCount;
		}

		public async Task<bool> SaveIntensitySummary(string url)
		{
			using (StreamWriter writer = File.CreateText(url))
			{
				await writer.WriteLineAsync("Intensity, Count");
				var intensities = GetAllIntensities();

				foreach (var (k,v) in intensities)
				{
					await writer.WriteLineAsync(string.Format("{0},{1}", k, v));
				}
			}
			return true;
		}

		public List<string> TweetifyWorkouts()
		{
			var listOfTweets = new List<string>();
			foreach (var workout in _workOuts)
			{
				var intensity = GetWorkoutIntensity(workout);
				if (intensity == Intensity.Easy || intensity == Intensity.None)
				{
					listOfTweets.Add("Pumping iron at the gym!");
				}
				else
				{
					var buffer = 11;
					var charRemaining = 140 - (workout.Miles.ToString().Length + 
											   workout.Duration.Minutes.ToString().Length)
											   - buffer;

					var tweetReady = workout.Notes.Length < 120 ? workout.Notes : workout.Notes.Substring(0, 120);
					listOfTweets.Add(string.Format("{0} mi/{1} min : {2}", 
										workout.Miles, workout.Duration.Minutes, tweetReady));
				}
			}

			return listOfTweets;
		}
	}

	public class WorkOut
	{
		public string Notes;
		public int Miles { get; }
		public TimeSpan Duration { get; }

		public WorkOut(int miles, TimeSpan duration, string notes)
		{
			Miles = miles;
			Duration = duration;
			Notes = notes;
		}

		public override string ToString()
		{
			return string.Format("Workout: {0} Miles, {1} Minutes", Miles, Duration.TotalMinutes);
		}
	}
}
