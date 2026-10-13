using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002D43 RID: 11587
	public class CompMechRelay : ThingComp
	{
		// Token: 0x17002962 RID: 10594
		// (get) Token: 0x0601046D RID: 66669 RVA: 0x004B45B0 File Offset: 0x004B27B0
		public CompProperties_MechRelay Props
		{
			get
			{
				return (CompProperties_MechRelay)this.props;
			}
		}

		// Token: 0x17002963 RID: 10595
		// (get) Token: 0x0601046E RID: 66670 RVA: 0x004B45BD File Offset: 0x004B27BD
		public int DestabilizationTick
		{
			get
			{
				return this.destabilizationTick;
			}
		}

		// Token: 0x0601046F RID: 66671 RVA: 0x004B45C8 File Offset: 0x004B27C8
		public void Deactivate()
		{
			if (!this.active || this.inert)
			{
				return;
			}
			this.active = false;
			if (!this.Props.destabilizationMessage.NullOrEmpty())
			{
				Messages.Message(this.Props.destabilizationMessage, this.parent, MessageTypeDefOf.PositiveEvent, true);
			}
			this.destabilizationTick = Find.TickManager.TicksGame + 7500;
		}

		// Token: 0x06010470 RID: 66672 RVA: 0x004B4638 File Offset: 0x004B2838
		public override void CompTick()
		{
			if (this.inert)
			{
				return;
			}
			if (this.destabilizationTick > 0 && Find.TickManager.TicksGame >= this.destabilizationTick)
			{
				this.destabilizationTick = -1;
				Map map = this.parent.Map;
				IntVec3 position = this.parent.Position;
				this.parent.Destroy(DestroyMode.Vanish);
				if (this.Props.crashedThingDef != null)
				{
					Thing thing = ThingMaker.MakeThing(this.Props.crashedThingDef, null);
					GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Direct, null, null, null, 1);
					CellRect cellRect = this.parent.OccupiedRect();
					for (int i = 0; i < cellRect.Area; i++)
					{
						FleckMaker.ThrowDustPuff(cellRect.RandomVector3.WithY(AltitudeLayer.MoteLow.AltitudeFor()), map, 2f);
					}
					if (thing.Faction != this.parent.Faction)
					{
						thing.SetFaction(this.parent.Faction, null);
					}
					if (!this.Props.crashedLetterLabel.NullOrEmpty())
					{
						Find.LetterStack.ReceiveLetter(this.Props.crashedLetterLabel, this.Props.crashedLetterText, LetterDefOf.NeutralEvent, thing, null, null, null, null, 0, true);
					}
				}
			}
		}

		// Token: 0x06010471 RID: 66673 RVA: 0x004B4788 File Offset: 0x004B2988
		public override string CompInspectStringExtra()
		{
			if (this.inert)
			{
				return null;
			}
			if (this.active && !this.Props.activeInspectString.NullOrEmpty())
			{
				return this.Props.activeInspectString;
			}
			if (this.destabilizationTick > 0 && !this.Props.destabilizationInspectString.NullOrEmpty())
			{
				return this.Props.destabilizationInspectString + ": " + (this.destabilizationTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
			}
			return base.CompInspectStringExtra();
		}

		// Token: 0x06010472 RID: 66674 RVA: 0x004B4821 File Offset: 0x004B2A21
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.ShowDevGizmos && !this.inert)
			{
				if (this.active)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Deactivate",
						action = new Action(this.Deactivate)
					};
				}
				else if (this.destabilizationTick > 0)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Destabilize now",
						action = delegate()
						{
							this.destabilizationTick = Find.TickManager.TicksGame;
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06010473 RID: 66675 RVA: 0x004B4831 File Offset: 0x004B2A31
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.active, "active", true, false);
			Scribe_Values.Look<bool>(ref this.inert, "inert", false, false);
			Scribe_Values.Look<int>(ref this.destabilizationTick, "destabilizationTick", -1, false);
		}

		// Token: 0x0400AA09 RID: 43529
		public const int DestabilizationTicks = 7500;

		// Token: 0x0400AA0A RID: 43530
		private const int CrashFlecksPerCell = 1;

		// Token: 0x0400AA0B RID: 43531
		private bool active = true;

		// Token: 0x0400AA0C RID: 43532
		public bool inert;

		// Token: 0x0400AA0D RID: 43533
		private int destabilizationTick = -1;
	}
}
