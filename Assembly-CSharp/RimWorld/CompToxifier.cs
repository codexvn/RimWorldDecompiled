using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002E9D RID: 11933
	public class CompToxifier : ThingComp
	{
		// Token: 0x17002B37 RID: 11063
		// (get) Token: 0x06010CEA RID: 68842 RVA: 0x004D6633 File Offset: 0x004D4833
		private CompProperties_Toxifier Props
		{
			get
			{
				return (CompProperties_Toxifier)this.props;
			}
		}

		// Token: 0x17002B38 RID: 11064
		// (get) Token: 0x06010CEB RID: 68843 RVA: 0x004D6640 File Offset: 0x004D4840
		public bool CanPolluteNow
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				if (!this.nextPollutionCell.CanPollute(this.parent.Map))
				{
					this.UpdateNextPolluteCell();
				}
				return this.nextPollutionCell.CanPollute(this.parent.Map);
			}
		}

		// Token: 0x06010CEC RID: 68844 RVA: 0x004D6690 File Offset: 0x004D4890
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Toxifier"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.pollutingProgressTicks = this.Props.pollutionIntervalTicks;
			}
		}

		// Token: 0x06010CED RID: 68845 RVA: 0x004D66E0 File Offset: 0x004D48E0
		public override string CompInspectStringExtra()
		{
			TaggedString t = base.CompInspectStringExtra();
			if (this.CanPolluteNow)
			{
				if (!t.NullOrEmpty())
				{
					t += "\n";
				}
				t += "PollutingTerrainProgress".Translate() + ": " + (this.Props.pollutionIntervalTicks - this.pollutingProgressTicks).ToStringTicksToPeriod(true, false, true, true, false) + " (" + this.Props.cellsToPollute.ToString() + " " + "TilesLower".Translate() + ")";
			}
			return t.Resolve();
		}

		// Token: 0x06010CEE RID: 68846 RVA: 0x004D67A0 File Offset: 0x004D49A0
		private void UpdateNextPolluteCell()
		{
			if (this.nextPollutionCell.CanPollute(this.parent.Map))
			{
				return;
			}
			this.nextPollutionCell = IntVec3.Invalid;
			int num = GenRadial.NumCellsInRadius(this.Props.radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
				if (this.<UpdateNextPolluteCell>g__NextPolluteCellValidator|8_0(c))
				{
					this.nextPollutionCell = c;
					return;
				}
			}
		}

		// Token: 0x06010CEF RID: 68847 RVA: 0x004D681C File Offset: 0x004D4A1C
		private void PolluteNextCell(bool silent = false)
		{
			if (this.CanPolluteNow)
			{
				int num = GenRadial.NumCellsInRadius(this.Props.radius);
				int num2 = 0;
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
					if (c.InBounds(this.parent.Map) && c.CanPollute(this.parent.Map))
					{
						c.Pollute(this.parent.Map, silent);
						num2++;
						if (num2 >= this.Props.cellsToPollute)
						{
							break;
						}
					}
				}
				if (!silent)
				{
					this.DoEffects();
				}
			}
		}

		// Token: 0x06010CF0 RID: 68848 RVA: 0x004D68C2 File Offset: 0x004D4AC2
		private void DoEffects()
		{
			FleckMaker.Static(this.parent.TrueCenter(), this.parent.Map, FleckDefOf.Fleck_ToxifierPollutionSource, 1f);
			SoundDefOf.Toxifier_Pollute.PlayOneShot(this.parent);
		}

		// Token: 0x06010CF1 RID: 68849 RVA: 0x004D68FE File Offset: 0x004D4AFE
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Pollute",
					action = delegate()
					{
						this.PolluteNextCell(false);
					},
					Disabled = !this.CanPolluteNow
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Pollute all",
					action = delegate()
					{
						while (this.CanPolluteNow)
						{
							this.PolluteNextCell(true);
						}
					},
					Disabled = !this.CanPolluteNow
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set next pollution time",
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
								this.pollutingProgressTicks = this.Props.pollutionIntervalTicks - ticks;
							}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					},
					Disabled = !this.CanPolluteNow
				};
			}
			yield break;
		}

		// Token: 0x06010CF2 RID: 68850 RVA: 0x004D690E File Offset: 0x004D4B0E
		public override void CompTickInterval(int delta)
		{
			if (!this.CanPolluteNow)
			{
				return;
			}
			this.pollutingProgressTicks += delta;
			if (this.pollutingProgressTicks >= this.Props.pollutionIntervalTicks)
			{
				this.pollutingProgressTicks = 0;
				this.PolluteNextCell(false);
			}
		}

		// Token: 0x06010CF3 RID: 68851 RVA: 0x004D6948 File Offset: 0x004D4B48
		public override void PostExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.nextPollutionCell, "nextPollutionCell", IntVec3.Invalid, false);
			Scribe_Values.Look<int>(ref this.pollutingProgressTicks, "pollutingProgressTicks", 0, false);
		}

		// Token: 0x06010CF5 RID: 68853 RVA: 0x004D6985 File Offset: 0x004D4B85
		[CompilerGenerated]
		private bool <UpdateNextPolluteCell>g__NextPolluteCellValidator|8_0(IntVec3 c)
		{
			return c.InBounds(this.parent.Map) && c.CanPollute(this.parent.Map);
		}

		// Token: 0x0400AEFA RID: 44794
		private IntVec3 nextPollutionCell = IntVec3.Invalid;

		// Token: 0x0400AEFB RID: 44795
		private int pollutingProgressTicks;
	}
}
