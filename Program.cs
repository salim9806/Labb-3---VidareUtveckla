﻿using System;
using System.Collections.Generic;

namespace VarmDrinkStation
{
	public interface IWarmDrink
	{
		void Consume();
	}
	internal class Water : IWarmDrink
	{
		public void Consume()
		{
			Console.WriteLine("Warm water is served.");
		}
	}

	internal class Coffe : IWarmDrink
	{
		public void Consume()
		{
			Console.WriteLine("Coffe is served.");
		}
	}

	internal class Tea : IWarmDrink
	{
		public void Consume()
		{
			Console.WriteLine("Tea is served.");
		}
	}

	internal class Coppuccino : IWarmDrink
	{
		public void Consume()
		{
			Console.WriteLine("Coppuccino is served.");
		}
	}

	internal class Chocolate : IWarmDrink
	{
		public void Consume()
		{
			Console.WriteLine("Hot Chocolate drink is served.");
		}
	}

	public interface IWarmDrinkFactory
	{
		IWarmDrink Prepare(int total);
	}

	internal class HotWaterFactory : IWarmDrinkFactory
	{
		public IWarmDrink Prepare(int total)
		{
			Console.WriteLine($"Pour {total} ml hot water in your cup");
			return new Water();
		}
	}

	internal class CoffeFactory : IWarmDrinkFactory
	{
		public IWarmDrink Prepare(int total)
		{
			Console.WriteLine($"Put one spoon of sugar");
			Console.WriteLine($"Pour {total} ml of Coffe in your cup");
			return new Coffe();
		}
	}

	internal class CoppuccinoFactory : IWarmDrinkFactory
	{
		public IWarmDrink Prepare(int total)
		{
			Console.WriteLine($"Put one spoon of sugar");
			Console.WriteLine($"Pour {total} ml of Coppuccino in your cup");
			return new Coppuccino();
		}
	}

	internal class TeaFactory : IWarmDrinkFactory
	{
		public IWarmDrink Prepare(int total)
		{
			Console.WriteLine($"Put one spoon of sugar");
			Console.WriteLine($"Pour {total} ml of Tea in your cup");
			return new Tea();
		}
	}

	internal class HotChocolateDrinkFactory : IWarmDrinkFactory
	{
		public IWarmDrink Prepare(int total)
		{
			Console.WriteLine($"Get harvested cocoa bean from south africa");
			Console.WriteLine($"Turn cocoa bean into chocolate powder");
			Console.WriteLine($"Add sweeteners and hidden chemicals");
			Console.WriteLine($"Pour {total} ml of Hot Chocolate in your cup");
			return new Chocolate();
		}
	}

	public class WarmDrinkMachine
	{
		public enum AvailableDrink // violates open-closed
		{
			Coffee, Tea
		}
		private Dictionary<AvailableDrink, IWarmDrinkFactory> factories =
		  new Dictionary<AvailableDrink, IWarmDrinkFactory>();

		private List<Tuple<string, IWarmDrinkFactory>> namedFactories = new List<Tuple<string, IWarmDrinkFactory>>();

		public WarmDrinkMachine()
		{
			foreach (var t in typeof(WarmDrinkMachine).Assembly.GetTypes())
			{
				if (typeof(IWarmDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
				{
					namedFactories.Add(Tuple.Create(
					  t.Name.Replace("Factory", string.Empty), (IWarmDrinkFactory)Activator.CreateInstance(t)));
				}
			}
		}
		public IWarmDrink MakeDrink()
		{
			Console.WriteLine("This is what we serve today:");
			for (var index = 0; index < namedFactories.Count; index++)
			{
				var tuple = namedFactories[index];
				Console.WriteLine($"{index}: {tuple.Item1}");
			}
			Console.WriteLine("Select a number to continue:");
			while (true)
			{
				string s;
				if ((s = Console.ReadLine()) != null
					&& int.TryParse(s, out int i) // c# 7
					&& i >= 0
					&& i < namedFactories.Count)
				{
					Console.Write("How much: ");
					s = Console.ReadLine();
					if (s != null
						&& int.TryParse(s, out int total)
						&& total > 0)
					{
						return namedFactories[i].Item2.Prepare(total);
					}
				}
				Console.WriteLine("Something went wrong with your input, try again.");
			}
		}
	}
	class Program
	{
		static void Main(string[] args)
		{
			var machine = new WarmDrinkMachine();
			IWarmDrink drink = machine.MakeDrink();
			drink.Consume();
		}
	}
}