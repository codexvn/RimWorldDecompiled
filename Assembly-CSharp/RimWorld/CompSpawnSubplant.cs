using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002E4B RID: 11851
	public class CompSpawnSubplant : ThingComp
	{
		// Token: 0x17002ABD RID: 10941
		// (get) Token: 0x06010ADA RID: 68314 RVA: 0x004CECD8 File Offset: 0x004CCED8
		public CompProperties_SpawnSubplant Props
		{
			get
			{
				return (CompProperties_SpawnSubplant)this.props;
			}
		}

		// Token: 0x17002ABE RID: 10942
		// (get) Token: 0x06010ADB RID: 68315 RVA: 0x004CECE5 File Offset: 0x004CCEE5
		public List<Thing> SubplantsForReading
		{
			get
			{
				this.Cleanup();
				return this.subplants;
			}
		}

		// Token: 0x17002ABF RID: 10943
		// (get) Token: 0x06010ADC RID: 68316 RVA: 0x004CECF4 File Offset: 0x004CCEF4
		private float ProgressMultiplier
		{
			get
			{
				foreach (Pair<int, float> pair in CompSpawnSubplant.TicksToProgressMultipliers)
				{
					if (this.meditationTicksToday < pair.First)
					{
						return pair.Second;
					}
				}
				return CompSpawnSubplant.TicksToProgressMultipliers.Last<Pair<int, float>>().Second;
			}
		}

		// Token: 0x06010ADD RID: 68317 RVA: 0x004CED6C File Offset: 0x004CCF6C
		public void AddProgress(float progressPerTick, bool ignoreMultiplier = false)
		{
			if (!ModLister.CheckRoyalty("Subplant spawning"))
			{
				return;
			}
			if (!ignoreMultiplier)
			{
				progressPerTick *= this.ProgressMultiplier;
			}
			this.progressToNextSubplant += progressPerTick * (1f + this.parent.GetStatValue(StatDefOf.MeditationPlantGrowthOffset, true, -1));
			this.meditationTicksToday++;
			this.TryGrowSubplants();
		}

		// Token: 0x06010ADE RID: 68318 RVA: 0x004CEDCE File Offset: 0x004CCFCE
		public override void CompTickLong()
		{
			if (GenLocalDate.DayTick(this.parent.Map) < 2000)
			{
				this.meditationTicksToday = 0;
			}
		}

		// Token: 0x06010ADF RID: 68319 RVA: 0x004CEDEE File Offset: 0x004CCFEE
		public void Cleanup()
		{
			this.subplants.RemoveAll((Thing p) => !p.Spawned);
		}

		// Token: 0x06010AE0 RID: 68320 RVA: 0x004CEE1C File Offset: 0x004CD01C
		public override string CompInspectStringExtra()
		{
			return "TotalMeditationToday".Translate((this.meditationTicksToday / 2500).ToString() + "LetterHour".Translate(), this.ProgressMultiplier.ToStringPercent()) + "\n" + this.Props.subplant.LabelCap + ": " + this.SubplantsForReading.Count.ToString() + " (" + "ProgressToNextSubplant".Translate(this.progressToNextSubplant.ToStringPercent()) + ")";
		}

		// Token: 0x06010AE1 RID: 68321 RVA: 0x004CEEE4 File Offset: 0x004CD0E4
		private void TryGrowSubplants()
		{
			while (this.progressToNextSubplant >= 1f)
			{
				this.DoGrowSubplant();
				this.progressToNextSubplant -= 1f;
			}
		}

		// Token: 0x06010AE2 RID: 68322 RVA: 0x004CEF10 File Offset: 0x004CD110
		private void DoGrowSubplant()
		{
			IntVec3 position = this.parent.Position;
			for (int i = 0; i < 1000; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map) && WanderUtility.InSameRoom(position, intVec, this.parent.Map))
				{
					bool flag = false;
					List<Thing> thingList = intVec.GetThingList(this.parent.Map);
					foreach (Thing thing in thingList)
					{
						if (thing.def == this.Props.subplant)
						{
							flag = true;
							break;
						}
						if (!this.Props.plantsToNotOverwrite.NullOrEmpty<ThingDef>())
						{
							for (int j = 0; j < this.Props.plantsToNotOverwrite.Count; j++)
							{
								if (thing.def == this.Props.plantsToNotOverwrite[j])
								{
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag && this.Props.subplant.CanEverPlantAt(intVec, this.parent.Map, true, false))
					{
						for (int k = thingList.Count - 1; k >= 0; k--)
						{
							if (thingList[k].def.category == ThingCategory.Plant)
							{
								thingList[k].Destroy(DestroyMode.Vanish);
							}
						}
						Plant plant = (Plant)GenSpawn.Spawn(this.Props.subplant, intVec, this.parent.Map, WipeMode.Vanish);
						this.subplants.Add(plant);
						if (this.Props.initialGrowthRange != null)
						{
							plant.Growth = this.Props.initialGrowthRange.Value.RandomInRange;
						}
						if (this.Props.spawnSound != null)
						{
							this.Props.spawnSound.PlayOneShot(new TargetInfo(this.parent));
						}
						Action action = this.onGrassGrown;
						if (action == null)
						{
							return;
						}
						action();
						return;
					}
				}
			}
		}

		// Token: 0x06010AE3 RID: 68323 RVA: 0x004CF140 File Offset: 0x004CD340
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode || !DebugSettings.godMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Add 100% progress",
				action = delegate()
				{
					this.AddProgress(1f, true);
				}
			};
			yield break;
		}

		// Token: 0x06010AE4 RID: 68324 RVA: 0x004CF150 File Offset: 0x004CD350
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.progressToNextSubplant, "progressToNextSubplant", 0f, false);
			Scribe_Collections.Look<Thing>(ref this.subplants, "subplants", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.meditationTicksToday, "meditationTicksToday", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.subplants.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400ADEB RID: 44523
		private float progressToNextSubplant;

		// Token: 0x0400ADEC RID: 44524
		private List<Thing> subplants = new List<Thing>();

		// Token: 0x0400ADED RID: 44525
		private int meditationTicksToday;

		// Token: 0x0400ADEE RID: 44526
		public Action onGrassGrown;

		// Token: 0x0400ADEF RID: 44527
		private static readonly List<Pair<int, float>> TicksToProgressMultipliers = new List<Pair<int, float>>
		{
			new Pair<int, float>(30000, 1f),
			new Pair<int, float>(60000, 0.5f),
			new Pair<int, float>(120000, 0.25f),
			new Pair<int, float>(240000, 0.15f)
		};
	}
}
