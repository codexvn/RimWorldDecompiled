using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E52 RID: 11858
	public class CompSpreadSludge : ThingComp
	{
		// Token: 0x17002AC7 RID: 10951
		// (get) Token: 0x06010B0F RID: 68367 RVA: 0x004CFC7F File Offset: 0x004CDE7F
		private CompProperties_SpreadSludge Props
		{
			get
			{
				return (CompProperties_SpreadSludge)this.props;
			}
		}

		// Token: 0x17002AC8 RID: 10952
		// (get) Token: 0x06010B10 RID: 68368 RVA: 0x0048AE48 File Offset: 0x00489048
		private Pawn Pawn
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x06010B11 RID: 68369 RVA: 0x004CFC8C File Offset: 0x004CDE8C
		public override void CompTick()
		{
			CompSpreadSludge.<>c__DisplayClass4_0 CS$<>8__locals1 = new CompSpreadSludge.<>c__DisplayClass4_0();
			CS$<>8__locals1.<>4__this = this;
			if (!this.Pawn.Spawned || this.Pawn.Downed)
			{
				return;
			}
			if (this.Props.abilityDef == AbilityDefOf.EggSpew)
			{
				Pawn_TrainingTracker training = this.Pawn.training;
				if (training != null && training.HasLearned(TrainableDefOf.EggSpew))
				{
					return;
				}
			}
			if (this.Props.abilityDef == AbilityDefOf.SludgeSpew)
			{
				Pawn_TrainingTracker training2 = this.Pawn.training;
				if (training2 != null && training2.HasLearned(TrainableDefOf.SludgeSpew))
				{
					return;
				}
			}
			if (!Rand.MTBEventOccurs((float)this.Props.mtbTicks, 1f, 1f))
			{
				return;
			}
			Pawn_AbilityTracker abilities = this.Pawn.abilities;
			Ability ability = (abilities != null) ? abilities.GetAbility(this.Props.abilityDef, false) : null;
			CS$<>8__locals1.range = this.Props.abilityDef.verbProperties.range;
			if (ability == null)
			{
				return;
			}
			IntVec3 c;
			if ((!this.Props.tryAvoidSludge || !CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, Mathf.CeilToInt(CS$<>8__locals1.range), (IntVec3 x) => base.<CompTick>g__ValidCell|0(x) && x.GetTerrain(CS$<>8__locals1.<>4__this.parent.Map) != TerrainDefOf.InsectSludge, out c, -1)) && !CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, Mathf.CeilToInt(CS$<>8__locals1.range), new Predicate<IntVec3>(CS$<>8__locals1.<CompTick>g__ValidCell|0), out c, -1))
			{
				return;
			}
			ability.verb.TryStartCastOn(c, c, false, true, true, false);
			return;
		}
	}
}
