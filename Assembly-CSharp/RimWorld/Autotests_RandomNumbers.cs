using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class Autotests_RandomNumbers
{
	[CompilerGenerated]
	private sealed class _003CRandomFloats_003Ed__2 : IEnumerable<float>, IEnumerable, IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private int count;

		public int _003C_003E3__count;

		private int _003Ci_003E5__2;

		float IEnumerator<float>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CRandomFloats_003Ed__2(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < count)
			{
				_003C_003E2__current = Rand.Value;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<float> IEnumerable<float>.GetEnumerator()
		{
			_003CRandomFloats_003Ed__2 _003CRandomFloats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CRandomFloats_003Ed__ = this;
			}
			else
			{
				_003CRandomFloats_003Ed__ = new _003CRandomFloats_003Ed__2(0);
			}
			_003CRandomFloats_003Ed__.count = _003C_003E3__count;
			return _003CRandomFloats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<float>)this).GetEnumerator();
		}
	}

	public static void Run()
	{
		Log.Message("Running random numbers tests.");
		CheckSimpleFloats();
		CheckIntsRange();
		CheckIntsDistribution();
		CheckSeed();
		Log.Message("Finished.");
	}

	private static void CheckSimpleFloats()
	{
		List<float> list = RandomFloats(500).ToList();
		if (list.Any((float x) => x < 0f || x > 1f))
		{
			Log.Error("Float out of range.");
		}
		if (!list.Any((float x) => x < 0.1f) || !list.Any((float x) => (double)x > 0.5 && (double)x < 0.6) || !list.Any((float x) => (double)x > 0.9))
		{
			Log.Warning("Possibly uneven distribution.");
		}
		list = RandomFloats(1300000).ToList();
		int num = list.Count((float x) => (double)x < 0.1);
		Log.Message("< 0.1 count (should be ~10%): " + (float)num / (float)list.Count() * 100f + "%");
		num = list.Count((float x) => (double)x < 0.0001);
		Log.Message("< 0.0001 count (should be ~0.01%): " + (float)num / (float)list.Count() * 100f + "%");
	}

	[IteratorStateMachine(typeof(_003CRandomFloats_003Ed__2))]
	private static IEnumerable<float> RandomFloats(int count)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CRandomFloats_003Ed__2(-2)
		{
			_003C_003E3__count = count
		};
	}

	private static void CheckIntsRange()
	{
		int num = -7;
		int num2 = 4;
		int num3 = 0;
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		while (true)
		{
			bool flag = true;
			for (int i = num; i <= num2; i++)
			{
				if (!dictionary.ContainsKey(i))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				break;
			}
			num3++;
			if (num3 == 200000)
			{
				Log.Error("Failed to find all numbers in a range.");
				return;
			}
			int num4 = Rand.RangeInclusive(num, num2);
			if (num4 < num || num4 > num2)
			{
				Log.Error("Value out of range.");
			}
			if (dictionary.ContainsKey(num4))
			{
				dictionary[num4]++;
			}
			else
			{
				dictionary.Add(num4, 1);
			}
		}
		Log.Message("Values between " + num + " and " + num2 + " (value: number of occurrences):");
		for (int j = num; j <= num2; j++)
		{
			Log.Message(j + ": " + dictionary[j]);
		}
	}

	private static void CheckIntsDistribution()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < 1000000; i++)
		{
			int num = Rand.RangeInclusive(-2, 1);
			list.Add(num + 2);
		}
		Log.Message("Ints distribution (should be even):");
		int j;
		for (j = 0; j < 4; j++)
		{
			Log.Message(j + ": " + (float)list.Count((int x) => x == j) / (float)list.Count() * 100f + "%");
		}
	}

	private static void CheckSeed()
	{
		int seed = (Rand.Seed = 10);
		int @int = Rand.Int;
		int int2 = Rand.Int;
		Rand.Seed = seed;
		int int3 = Rand.Int;
		int int4 = Rand.Int;
		if (@int != int3 || int2 != int4)
		{
			Log.Error("Same seed, different values.");
		}
		TestPushSeed(15, 20);
		TestPushSeed(-2147483645, 20);
		TestPushSeed(6, int.MaxValue);
		TestPushSeed(-2147483645, 2147483642);
		TestPushSeed(-1947483645, 1147483642);
		TestPushSeed(455, 648023);
	}

	private static void TestPushSeed(int seed1, int seed2)
	{
		Rand.Seed = seed1;
		int @int = Rand.Int;
		int int2 = Rand.Int;
		Rand.PushState();
		Rand.Seed = seed2;
		int int3 = Rand.Int;
		Rand.PopState();
		Rand.Seed = seed1;
		int int4 = Rand.Int;
		Rand.PushState();
		Rand.Seed = seed2;
		int int5 = Rand.Int;
		Rand.PopState();
		int int6 = Rand.Int;
		if (@int != int4 || int2 != int6 || int3 != int5)
		{
			Log.Error("PushSeed broken.");
		}
	}
}
