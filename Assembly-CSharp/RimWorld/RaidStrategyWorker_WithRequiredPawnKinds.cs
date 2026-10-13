using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001758 RID: 5976
	public abstract class RaidStrategyWorker_WithRequiredPawnKinds : RaidStrategyWorker
	{
		// Token: 0x06008C3B RID: 35899
		protected abstract bool MatchesRequiredPawnKind(PawnKindDef kind);

		// Token: 0x06008C3C RID: 35900
		protected abstract int MinRequiredPawnsForPoints(float pointsTotal, Faction faction = null);

		// Token: 0x06008C3D RID: 35901 RVA: 0x0029B9C0 File Offset: 0x00299BC0
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return this.PawnGenOptionsWithRequiredPawns(parms.faction, groupKind).Any<PawnGroupMaker>() && base.CanUseWith(parms, groupKind);
		}

		// Token: 0x06008C3E RID: 35902 RVA: 0x0029B9E5 File Offset: 0x00299BE5
		public bool CanUseWithGroupMaker(PawnGroupMaker groupMaker)
		{
			return groupMaker.options != null && groupMaker.options.Any((PawnGenOption o) => this.MatchesRequiredPawnKind(o.kind));
		}

		// Token: 0x06008C3F RID: 35903 RVA: 0x0029BA08 File Offset: 0x00299C08
		public override float MinimumPoints(Faction faction, PawnGroupKindDef groupKind)
		{
			return Mathf.Max(base.MinimumPoints(faction, groupKind), this.CheapestRequiredPawnCost(faction, groupKind));
		}

		// Token: 0x06008C40 RID: 35904 RVA: 0x0029BA1F File Offset: 0x00299C1F
		public override float MinMaxAllowedPawnGenOptionCost(Faction faction, PawnGroupKindDef groupKind)
		{
			return this.CheapestRequiredPawnCost(faction, groupKind);
		}

		// Token: 0x06008C41 RID: 35905 RVA: 0x0029BA2C File Offset: 0x00299C2C
		private float CheapestRequiredPawnCost(Faction faction, PawnGroupKindDef groupKind)
		{
			IEnumerable<PawnGroupMaker> enumerable = this.PawnGenOptionsWithRequiredPawns(faction, groupKind);
			if (!enumerable.Any<PawnGroupMaker>())
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to get MinimumPoints for ",
					base.GetType().ToString(),
					" for faction ",
					faction.ToString(),
					" but the faction has no groups with the required pawn kind. groupKind=",
					(groupKind != null) ? groupKind.ToString() : null
				}));
				return 99999f;
			}
			float num = 9999999f;
			foreach (PawnGroupMaker pawnGroupMaker in enumerable)
			{
				foreach (PawnGenOption pawnGenOption in from op in pawnGroupMaker.options
				where this.MatchesRequiredPawnKind(op.kind)
				select op)
				{
					if (pawnGenOption.Cost < num)
					{
						num = pawnGenOption.Cost;
					}
				}
			}
			return num;
		}

		// Token: 0x06008C42 RID: 35906 RVA: 0x0029BB34 File Offset: 0x00299D34
		public override bool CanUsePawnGenOption(float pointsTotal, PawnGenOption opt, List<PawnGenOptionWithXenotype> chosenOpts, Faction faction = null)
		{
			return (chosenOpts == null || chosenOpts.Count >= this.MinRequiredPawnsForPoints(pointsTotal, faction) || this.MatchesRequiredPawnKind(opt.kind)) && base.CanUsePawnGenOption(pointsTotal, opt, chosenOpts, faction);
		}

		// Token: 0x06008C43 RID: 35907 RVA: 0x0029BB68 File Offset: 0x00299D68
		private IEnumerable<PawnGroupMaker> PawnGenOptionsWithRequiredPawns(Faction faction, PawnGroupKindDef groupKind)
		{
			if (faction.def.pawnGroupMakers == null)
			{
				return Enumerable.Empty<PawnGroupMaker>();
			}
			Predicate<PawnGenOption> <>9__1;
			return faction.def.pawnGroupMakers.Where(delegate(PawnGroupMaker gm)
			{
				if (gm.kindDef == groupKind && gm.options != null)
				{
					List<PawnGenOption> options = gm.options;
					Predicate<PawnGenOption> predicate;
					if ((predicate = <>9__1) == null)
					{
						predicate = (<>9__1 = ((PawnGenOption op) => this.MatchesRequiredPawnKind(op.kind)));
					}
					return options.Any(predicate);
				}
				return false;
			});
		}
	}
}
