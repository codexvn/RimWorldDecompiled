using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002958 RID: 10584
	public class Building_PassengerShuttle : Building, IRenameable
	{
		// Token: 0x1700234C RID: 9036
		// (get) Token: 0x0600E81B RID: 59419 RVA: 0x00439650 File Offset: 0x00437850
		public CompRefuelable RefuelableComp
		{
			get
			{
				CompRefuelable result;
				if ((result = this.cachedRefuelableComp) == null)
				{
					result = (this.cachedRefuelableComp = base.GetComp<CompRefuelable>());
				}
				return result;
			}
		}

		// Token: 0x1700234D RID: 9037
		// (get) Token: 0x0600E81C RID: 59420 RVA: 0x00439678 File Offset: 0x00437878
		public CompLaunchable LaunchableComp
		{
			get
			{
				CompLaunchable result;
				if ((result = this.cachedLaunchableComp) == null)
				{
					result = (this.cachedLaunchableComp = base.GetComp<CompLaunchable>());
				}
				return result;
			}
		}

		// Token: 0x1700234E RID: 9038
		// (get) Token: 0x0600E81D RID: 59421 RVA: 0x004396A0 File Offset: 0x004378A0
		public CompTransporter TransporterComp
		{
			get
			{
				CompTransporter result;
				if ((result = this.cachedTransporterComp) == null)
				{
					result = (this.cachedTransporterComp = base.GetComp<CompTransporter>());
				}
				return result;
			}
		}

		// Token: 0x1700234F RID: 9039
		// (get) Token: 0x0600E81E RID: 59422 RVA: 0x004396C8 File Offset: 0x004378C8
		public CompShuttle ShuttleComp
		{
			get
			{
				CompShuttle result;
				if ((result = this.cachedShuttleComp) == null)
				{
					result = (this.cachedShuttleComp = base.GetComp<CompShuttle>());
				}
				return result;
			}
		}

		// Token: 0x17002350 RID: 9040
		// (get) Token: 0x0600E81F RID: 59423 RVA: 0x004396EE File Offset: 0x004378EE
		// (set) Token: 0x0600E820 RID: 59424 RVA: 0x00439700 File Offset: 0x00437900
		public string RenamableLabel
		{
			get
			{
				return this.shuttleName ?? this.BaseLabel;
			}
			set
			{
				this.shuttleName = value;
			}
		}

		// Token: 0x17002351 RID: 9041
		// (get) Token: 0x0600E821 RID: 59425 RVA: 0x00439709 File Offset: 0x00437909
		public string BaseLabel
		{
			get
			{
				return this.def.LabelCap;
			}
		}

		// Token: 0x17002352 RID: 9042
		// (get) Token: 0x0600E822 RID: 59426 RVA: 0x0043971B File Offset: 0x0043791B
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17002353 RID: 9043
		// (get) Token: 0x0600E823 RID: 59427 RVA: 0x0043971B File Offset: 0x0043791B
		public override string Label
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17002354 RID: 9044
		// (get) Token: 0x0600E824 RID: 59428 RVA: 0x00439723 File Offset: 0x00437923
		public float FuelLevel
		{
			get
			{
				return this.RefuelableComp.Fuel;
			}
		}

		// Token: 0x17002355 RID: 9045
		// (get) Token: 0x0600E825 RID: 59429 RVA: 0x00439730 File Offset: 0x00437930
		public float MaxFuelLevel
		{
			get
			{
				return this.RefuelableComp.Props.fuelCapacity;
			}
		}

		// Token: 0x0600E826 RID: 59430 RVA: 0x00439742 File Offset: 0x00437942
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.ShuttleComp.shipParent.Start();
			}
		}

		// Token: 0x0600E827 RID: 59431 RVA: 0x0043975F File Offset: 0x0043795F
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			int num = 0;
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				ThingWithComps thingWithComps = obj as ThingWithComps;
				if (thingWithComps != null && thingWithComps.HasComp<CompTransporter>())
				{
					num++;
				}
			}
			if (num > 1)
			{
				yield break;
			}
			float fuelInShuttle = this.FuelInShuttle();
			string text = null;
			if (fuelInShuttle <= 0f)
			{
				text = "NoFuelInShuttle".Translate();
			}
			if (Mathf.Approximately(this.FuelLevel, this.MaxFuelLevel))
			{
				text = "ShuttleFullyFueled".Translate();
			}
			Action<int> <>9__2;
			yield return new Command_Action
			{
				defaultLabel = "CommandRefuelShuttleFromCargo".Translate(),
				defaultDesc = "CommandRefuelShuttleFromCargoDesc".Translate(),
				icon = Building_PassengerShuttle.RefuelFromCargoIcon.Texture,
				action = delegate()
				{
					int num2 = Mathf.FloorToInt(Mathf.Min(fuelInShuttle, this.MaxFuelLevel - this.FuelLevel));
					Func<int, string> textGetter = (int val) => "RefuelShuttleCount".Translate(val);
					int from = 1;
					int to = num2;
					Action<int> confirmAction;
					if ((confirmAction = <>9__2) == null)
					{
						confirmAction = (<>9__2 = delegate(int count)
						{
							this.ConsumeFuelFromInventory(count);
							this.RefuelableComp.Refuel((float)count);
						});
					}
					Dialog_Slider window = new Dialog_Slider(textGetter, from, to, confirmAction, int.MinValue, 1f);
					Find.WindowStack.Add(window);
				},
				Disabled = !text.NullOrEmpty(),
				disabledReason = text
			};
			yield break;
			yield break;
		}

		// Token: 0x0600E828 RID: 59432 RVA: 0x0043976F File Offset: 0x0043796F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.shuttleName, "shuttleName", null, false);
		}

		// Token: 0x0600E829 RID: 59433 RVA: 0x0043978C File Offset: 0x0043798C
		private float FuelInShuttle()
		{
			float num = 0f;
			foreach (Thing thing in ((IEnumerable<Thing>)this.TransporterComp.innerContainer))
			{
				if (this.RefuelableComp.Props.fuelFilter.Allows(thing))
				{
					num += (float)thing.stackCount;
				}
			}
			return num;
		}

		// Token: 0x0600E82A RID: 59434 RVA: 0x00439800 File Offset: 0x00437A00
		private void ConsumeFuelFromInventory(int fuelAmount)
		{
			Building_PassengerShuttle.tmpContainedThings.Clear();
			Building_PassengerShuttle.tmpContainedThings.AddRange(this.TransporterComp.innerContainer);
			int num = fuelAmount;
			for (int i = Building_PassengerShuttle.tmpContainedThings.Count - 1; i >= 0; i--)
			{
				Thing thing = Building_PassengerShuttle.tmpContainedThings[i];
				if (this.RefuelableComp.Props.fuelFilter.Allows(thing))
				{
					Thing thing2 = thing.SplitOff(Mathf.Min(num, thing.stackCount));
					num -= thing2.stackCount;
				}
				if (num <= 0)
				{
					break;
				}
			}
		}

		// Token: 0x04009AC7 RID: 39623
		private string shuttleName;

		// Token: 0x04009AC8 RID: 39624
		private CompRefuelable cachedRefuelableComp;

		// Token: 0x04009AC9 RID: 39625
		private CompLaunchable cachedLaunchableComp;

		// Token: 0x04009ACA RID: 39626
		private CompTransporter cachedTransporterComp;

		// Token: 0x04009ACB RID: 39627
		private CompShuttle cachedShuttleComp;

		// Token: 0x04009ACC RID: 39628
		public static CachedTexture RefuelFromCargoIcon = new CachedTexture("UI/Commands/RefuelPassengerShuttle");

		// Token: 0x04009ACD RID: 39629
		private static List<Thing> tmpContainedThings = new List<Thing>();
	}
}
