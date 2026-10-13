using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DB3 RID: 11699
	public class CompPollutionPump : ThingComp
	{
		// Token: 0x170029F0 RID: 10736
		// (get) Token: 0x06010727 RID: 67367 RVA: 0x004BE86C File Offset: 0x004BCA6C
		public CompProperties_PollutionPump Props
		{
			get
			{
				return (CompProperties_PollutionPump)this.props;
			}
		}

		// Token: 0x170029F1 RID: 10737
		// (get) Token: 0x06010728 RID: 67368 RVA: 0x004BE87C File Offset: 0x004BCA7C
		private bool Active
		{
			get
			{
				return this.parent.Spawned && !this.disabledByArtificialBuildings && (this.compPower == null || this.compPower.PowerOn) && this.GetCellToUnpollute().IsValid;
			}
		}

		// Token: 0x06010729 RID: 67369 RVA: 0x004BE8CC File Offset: 0x004BCACC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Pollution pump"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.ticksUntilPump = this.Props.intervalTicks;
			}
			this.compPower = this.parent.GetComp<CompPowerTrader>();
		}

		// Token: 0x0601072A RID: 67370 RVA: 0x004BE92C File Offset: 0x004BCB2C
		private IntVec3 GetCellToUnpollute()
		{
			int num = GenRadial.NumCellsInRadius(this.Props.radius);
			Map map = this.parent.Map;
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && intVec.CanUnpollute(map))
				{
					return intVec;
				}
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0601072B RID: 67371 RVA: 0x004BE998 File Offset: 0x004BCB98
		private void Pump()
		{
			IntVec3 cellToUnpollute = this.GetCellToUnpollute();
			if (cellToUnpollute.IsValid)
			{
				Map map = this.parent.Map;
				map.pollutionGrid.SetPolluted(cellToUnpollute, false, false);
				this.currentIntervalPumps++;
				EffecterDef pumpEffecterDef = this.Props.pumpEffecterDef;
				if (pumpEffecterDef != null)
				{
					pumpEffecterDef.Spawn(this.parent.Position, map, 1f).Cleanup();
				}
				if (this.Props.pumpsPerWastepack > 0 && this.currentIntervalPumps % this.Props.pumpsPerWastepack == 0)
				{
					GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.Wastepack, null), this.parent.Position, this.parent.Map, ThingPlaceMode.Near, null, delegate(IntVec3 p)
					{
						Building firstBuilding = p.GetFirstBuilding(map);
						return firstBuilding == null || firstBuilding.IsClearableFreeBuilding;
					}, null, 1);
					this.currentIntervalPumps = 0;
				}
				MoteMaker.MakeAttachedOverlay(this.parent, ThingDefOf.Mote_PollutionPump, Vector3.zero, 1f, -1f);
			}
		}

		// Token: 0x0601072C RID: 67372 RVA: 0x004BEAA9 File Offset: 0x004BCCA9
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Pump",
					action = new Action(this.Pump),
					Disabled = !this.Active
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set next pump time",
					action = delegate()
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						int[] array = new int[]
						{
							60,
							120,
							180,
							240,
							300,
							600,
							900,
							1200,
							1500,
							1800,
							3600
						};
						for (int i = 0; i < array.Length; i++)
						{
							int ticks = array[i];
							list.Add(new FloatMenuOption(ticks.ToStringSecondsFromTicks("F0"), delegate()
							{
								this.ticksUntilPump = ticks;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					},
					Disabled = !this.Active
				};
			}
			yield break;
		}

		// Token: 0x0601072D RID: 67373 RVA: 0x004BEABC File Offset: 0x004BCCBC
		public override string CompInspectStringExtra()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.CompInspectStringExtra());
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			if (this.Active)
			{
				stringBuilder.Append("AbsorbingPollutionNext".Translate(this.ticksUntilPump.ToStringTicksToPeriod(true, false, true, true, false)));
			}
			else if (this.disabledByArtificialBuildings)
			{
				stringBuilder.Append("CannotAbsorbPollution".Translate());
			}
			else
			{
				stringBuilder.Append("CanAbsorbPollution".Translate());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0601072E RID: 67374 RVA: 0x004BEB60 File Offset: 0x004BCD60
		public override void CompTick()
		{
			base.CompTick();
			if (this.Props.disabledByArtificialBuildings && this.parent.IsHashIntervalTick(60))
			{
				this.disabledByArtificialBuildings = (this.parent.Map.listerArtificialBuildingsForMeditation.GetForCell(this.parent.Position, this.Props.radius).Count > 0);
			}
			if (this.parent.IsHashIntervalTick(60) && this.Active)
			{
				this.ticksUntilPump -= 60;
				if (this.ticksUntilPump <= 0)
				{
					this.ticksUntilPump = this.Props.intervalTicks;
					this.Pump();
				}
			}
		}

		// Token: 0x0601072F RID: 67375 RVA: 0x004BEC0E File Offset: 0x004BCE0E
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksUntilPump, "ticksUntilPump", 0, false);
			Scribe_Values.Look<int>(ref this.currentIntervalPumps, "currentIntervalPumps", 0, false);
			Scribe_Values.Look<bool>(ref this.disabledByArtificialBuildings, "disabledByArtificialBuildings", false, false);
		}

		// Token: 0x0400AB8C RID: 43916
		private const int CheckInterval = 60;

		// Token: 0x0400AB8D RID: 43917
		private CompPowerTrader compPower;

		// Token: 0x0400AB8E RID: 43918
		private int ticksUntilPump;

		// Token: 0x0400AB8F RID: 43919
		private int currentIntervalPumps;

		// Token: 0x0400AB90 RID: 43920
		[LoadAlias("disabledByArtificalBuildings")]
		private bool disabledByArtificialBuildings;
	}
}
