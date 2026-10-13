using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02002D27 RID: 11559
	public class CompMannable : ThingComp
	{
		// Token: 0x17002941 RID: 10561
		// (get) Token: 0x060103CB RID: 66507 RVA: 0x004B1C39 File Offset: 0x004AFE39
		public bool MannedNow
		{
			get
			{
				return Find.TickManager.TicksGame - this.lastManTick <= 1 && this.lastManPawn != null && this.lastManPawn.Spawned;
			}
		}

		// Token: 0x17002942 RID: 10562
		// (get) Token: 0x060103CC RID: 66508 RVA: 0x004B1C64 File Offset: 0x004AFE64
		public Pawn ManningPawn
		{
			get
			{
				if (!this.MannedNow)
				{
					return null;
				}
				return this.lastManPawn;
			}
		}

		// Token: 0x17002943 RID: 10563
		// (get) Token: 0x060103CD RID: 66509 RVA: 0x004B1C76 File Offset: 0x004AFE76
		public CompProperties_Mannable Props
		{
			get
			{
				return (CompProperties_Mannable)this.props;
			}
		}

		// Token: 0x060103CE RID: 66510 RVA: 0x004B1C83 File Offset: 0x004AFE83
		public void ManForATick(Pawn pawn)
		{
			this.lastManTick = Find.TickManager.TicksGame;
			this.lastManPawn = pawn;
			pawn.mindState.lastMannedThing = this.parent;
		}

		// Token: 0x060103CF RID: 66511 RVA: 0x004B1CB0 File Offset: 0x004AFEB0
		public override string CompInspectStringExtra()
		{
			if (this.parent.Spawned && !this.Props.planetLayerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.Props.planetLayerWhitelist.Contains(this.parent.Map.Tile.LayerDef))
			{
				return "CannotFunctionOnLayer".Translate(this.parent.Map.Tile.LayerDef.label).CapitalizeFirst().Colorize(ColoredText.WarningColor);
			}
			return null;
		}

		// Token: 0x060103D0 RID: 66512 RVA: 0x004B1D46 File Offset: 0x004AFF46
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (!pawn.RaceProps.ToolUser)
			{
				yield break;
			}
			if (!pawn.CanReserveAndReach(this.parent, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
			{
				yield break;
			}
			if (this.Props.manWorkType != WorkTags.None && pawn.WorkTagIsDisabled(this.Props.manWorkType))
			{
				if (this.Props.manWorkType == WorkTags.Violent)
				{
					yield return new FloatMenuOption("CannotManThing".Translate(this.parent.LabelShort, this.parent) + " (" + "IsIncapableOfViolenceLower".Translate(pawn.LabelShort, pawn) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				yield break;
			}
			if (!this.Props.planetLayerWhitelist.NullOrEmpty<PlanetLayerDef>() && !this.Props.planetLayerWhitelist.Contains(pawn.Map.Tile.LayerDef))
			{
				yield return new FloatMenuOption("CannotManThing".Translate(this.parent.LabelShort, this.parent) + " (" + "CannotFunctionOnLayer".Translate(pawn.Map.Tile.LayerDef.label) + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			FloatMenuOption floatMenuOption = new FloatMenuOption("OrderManThing".Translate(this.parent.LabelShort, this.parent), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.ManTurret, this.parent);
				pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x0400A997 RID: 43415
		private int lastManTick = -1;

		// Token: 0x0400A998 RID: 43416
		private Pawn lastManPawn;
	}
}
