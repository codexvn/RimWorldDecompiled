using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class CompSpawnSubplant : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__17 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompSpawnSubplant _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetGizmosExtra_003Ed__17(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			CompSpawnSubplant CS_0024_003C_003E8__locals1 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!Prefs.DevMode || !DebugSettings.godMode)
				{
					return false;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Add 100% progress",
					action = delegate
					{
						CS_0024_003C_003E8__locals1.AddProgress(1f, ignoreMultiplier: true);
					}
				};
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private float progressToNextSubplant;

	private List<Thing> subplants = new List<Thing>();

	private int meditationTicksToday;

	public Action onGrassGrown;

	private static readonly List<Pair<int, float>> TicksToProgressMultipliers = new List<Pair<int, float>>
	{
		new Pair<int, float>(30000, 1f),
		new Pair<int, float>(60000, 0.5f),
		new Pair<int, float>(120000, 0.25f),
		new Pair<int, float>(240000, 0.15f)
	};

	public CompProperties_SpawnSubplant Props => (CompProperties_SpawnSubplant)props;

	public List<Thing> SubplantsForReading
	{
		get
		{
			Cleanup();
			return subplants;
		}
	}

	private float ProgressMultiplier
	{
		get
		{
			foreach (Pair<int, float> ticksToProgressMultiplier in TicksToProgressMultipliers)
			{
				if (meditationTicksToday < ticksToProgressMultiplier.First)
				{
					return ticksToProgressMultiplier.Second;
				}
			}
			return TicksToProgressMultipliers.Last().Second;
		}
	}

	public void AddProgress(float progressPerTick, bool ignoreMultiplier = false)
	{
		if (ModLister.CheckRoyalty("Subplant spawning"))
		{
			if (!ignoreMultiplier)
			{
				progressPerTick *= ProgressMultiplier;
			}
			progressToNextSubplant += progressPerTick * (1f + parent.GetStatValue(StatDefOf.MeditationPlantGrowthOffset));
			meditationTicksToday++;
			TryGrowSubplants();
		}
	}

	public override void CompTickLong()
	{
		if (GenLocalDate.DayTick(parent.Map) < 2000)
		{
			meditationTicksToday = 0;
		}
	}

	public void Cleanup()
	{
		subplants.RemoveAll((Thing p) => !p.Spawned);
	}

	public override string CompInspectStringExtra()
	{
		return string.Concat("TotalMeditationToday".Translate((meditationTicksToday / 2500).ToString() + "LetterHour".Translate(), ProgressMultiplier.ToStringPercent()) + "\n" + Props.subplant.LabelCap + ": ", SubplantsForReading.Count.ToString(), " (") + "ProgressToNextSubplant".Translate(progressToNextSubplant.ToStringPercent()) + ")";
	}

	private void TryGrowSubplants()
	{
		while (progressToNextSubplant >= 1f)
		{
			DoGrowSubplant();
			progressToNextSubplant -= 1f;
		}
	}

	private void DoGrowSubplant()
	{
		IntVec3 position = parent.Position;
		for (int i = 0; i < 1000; i++)
		{
			IntVec3 intVec = position + GenRadial.RadialPattern[i];
			if (!intVec.InBounds(parent.Map) || !WanderUtility.InSameRoom(position, intVec, parent.Map))
			{
				continue;
			}
			bool flag = false;
			List<Thing> thingList = intVec.GetThingList(parent.Map);
			foreach (Thing item in thingList)
			{
				if (item.def == Props.subplant)
				{
					flag = true;
					break;
				}
				if (Props.plantsToNotOverwrite.NullOrEmpty())
				{
					continue;
				}
				for (int j = 0; j < Props.plantsToNotOverwrite.Count; j++)
				{
					if (item.def == Props.plantsToNotOverwrite[j])
					{
						flag = true;
						break;
					}
				}
			}
			if (flag || !Props.subplant.CanEverPlantAt(intVec, parent.Map, canWipePlantsExceptTree: true, checkMapTemperature: false))
			{
				continue;
			}
			for (int num = thingList.Count - 1; num >= 0; num--)
			{
				if (thingList[num].def.category == ThingCategory.Plant)
				{
					thingList[num].Destroy();
				}
			}
			Plant plant = (Plant)GenSpawn.Spawn(Props.subplant, intVec, parent.Map);
			subplants.Add(plant);
			if (Props.initialGrowthRange.HasValue)
			{
				plant.Growth = Props.initialGrowthRange.Value.RandomInRange;
			}
			if (Props.spawnSound != null)
			{
				Props.spawnSound.PlayOneShot(new TargetInfo(parent));
			}
			onGrassGrown?.Invoke();
			break;
		}
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__17))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__17(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref progressToNextSubplant, "progressToNextSubplant", 0f);
		Scribe_Collections.Look(ref subplants, "subplants", LookMode.Reference);
		Scribe_Values.Look(ref meditationTicksToday, "meditationTicksToday", 0);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			subplants.RemoveAll((Thing x) => x == null);
		}
	}
}
