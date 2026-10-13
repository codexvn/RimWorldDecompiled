using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020032B7 RID: 12983
	public abstract class FloatMenuOptionProvider
	{
		// Token: 0x17002F23 RID: 12067
		// (get) Token: 0x0601245D RID: 74845
		protected abstract bool Drafted { get; }

		// Token: 0x17002F24 RID: 12068
		// (get) Token: 0x0601245E RID: 74846
		protected abstract bool Undrafted { get; }

		// Token: 0x17002F25 RID: 12069
		// (get) Token: 0x0601245F RID: 74847
		protected abstract bool Multiselect { get; }

		// Token: 0x17002F26 RID: 12070
		// (get) Token: 0x06012460 RID: 74848 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool RequiresManipulation
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F27 RID: 12071
		// (get) Token: 0x06012461 RID: 74849 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool MechanoidCanDo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F28 RID: 12072
		// (get) Token: 0x06012462 RID: 74850 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool CanSelfTarget
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F29 RID: 12073
		// (get) Token: 0x06012463 RID: 74851 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CanTargetDespawned
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F2A RID: 12074
		// (get) Token: 0x06012464 RID: 74852 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool IgnoreFogged
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012465 RID: 74853 RVA: 0x00562930 File Offset: 0x00560B30
		public virtual bool SelectedPawnValid(Pawn pawn, FloatMenuContext context)
		{
			return (!pawn.IsMutant || pawn.mutant.Def.whitelistedFloatMenuProviders == null || pawn.mutant.Def.whitelistedFloatMenuProviders.Contains(FloatMenuMakerMap.currentProvider.GetType())) && (this.Drafted || !pawn.Drafted) && (this.Undrafted || pawn.Drafted) && (this.MechanoidCanDo || !pawn.RaceProps.IsMechanoid) && (!this.RequiresManipulation || pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
		}

		// Token: 0x06012466 RID: 74854 RVA: 0x005629D8 File Offset: 0x00560BD8
		public virtual bool TargetThingValid(Thing thing, FloatMenuContext context)
		{
			if (!this.CanTargetDespawned && !thing.Spawned)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			return pawn == null || this.TargetPawnValid(pawn, context);
		}

		// Token: 0x06012467 RID: 74855 RVA: 0x00562A0E File Offset: 0x00560C0E
		public virtual bool TargetPawnValid(Pawn pawn, FloatMenuContext context)
		{
			return this.CanSelfTarget || pawn != context.FirstSelectedPawn;
		}

		// Token: 0x06012468 RID: 74856 RVA: 0x00562A24 File Offset: 0x00560C24
		public virtual bool Applies(FloatMenuContext context)
		{
			return (this.Multiselect || !context.IsMultiselect) && (!this.IgnoreFogged || !context.ClickedCell.Fogged(context.map)) && this.AppliesInt(context);
		}

		// Token: 0x06012469 RID: 74857 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool AppliesInt(FloatMenuContext context)
		{
			return true;
		}

		// Token: 0x0601246A RID: 74858 RVA: 0x00562A61 File Offset: 0x00560C61
		public virtual IEnumerable<FloatMenuOption> GetOptions(FloatMenuContext context)
		{
			FloatMenuOption singleOption = this.GetSingleOption(context);
			if (singleOption != null)
			{
				yield return singleOption;
			}
			yield break;
		}

		// Token: 0x0601246B RID: 74859 RVA: 0x00562A78 File Offset: 0x00560C78
		public virtual IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			FloatMenuOption singleOptionFor = this.GetSingleOptionFor(clickedThing, context);
			if (singleOptionFor != null)
			{
				yield return singleOptionFor;
			}
			yield break;
		}

		// Token: 0x0601246C RID: 74860 RVA: 0x00562A96 File Offset: 0x00560C96
		public virtual IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			FloatMenuOption singleOptionFor = this.GetSingleOptionFor(clickedPawn, context);
			if (singleOptionFor != null)
			{
				yield return singleOptionFor;
			}
			yield break;
		}

		// Token: 0x0601246D RID: 74861 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual FloatMenuOption GetSingleOption(FloatMenuContext context)
		{
			return null;
		}

		// Token: 0x0601246E RID: 74862 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			return null;
		}

		// Token: 0x0601246F RID: 74863 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			return null;
		}
	}
}
