using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AAC RID: 6828
	public class PawnRelationWorker
	{
		// Token: 0x060098F5 RID: 39157 RVA: 0x002C93D8 File Offset: 0x002C75D8
		public virtual bool InRelation(Pawn me, Pawn other)
		{
			if (this.def.implied)
			{
				PawnRelationDef pawnRelationDef = this.def;
				throw new NotImplementedException(((pawnRelationDef != null) ? pawnRelationDef.ToString() : null) + " lacks InRelation implementation.");
			}
			return me.relations.DirectRelationExists(this.def, other);
		}

		// Token: 0x060098F6 RID: 39158 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float GenerationChance(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			return 0f;
		}

		// Token: 0x060098F7 RID: 39159 RVA: 0x002C9428 File Offset: 0x002C7628
		public virtual void CreateRelation(Pawn generated, Pawn other, ref PawnGenerationRequest request)
		{
			if (!this.def.implied)
			{
				generated.relations.AddDirectRelation(this.def, other);
				return;
			}
			PawnRelationDef pawnRelationDef = this.def;
			throw new NotImplementedException(((pawnRelationDef != null) ? pawnRelationDef.ToString() : null) + " lacks CreateRelation implementation.");
		}

		// Token: 0x060098F8 RID: 39160 RVA: 0x002C9478 File Offset: 0x002C7678
		public float BaseGenerationChanceFactor(Pawn generated, Pawn other, PawnGenerationRequest request)
		{
			float num = 1f;
			if (other.IsDuplicate)
			{
				return 0f;
			}
			if (generated.Faction != other.Faction)
			{
				num *= 0.65f;
			}
			if (generated.HostileTo(other))
			{
				num *= 0.7f;
			}
			if (other.Faction != null && other.Faction.IsPlayer && (generated.Faction == null || !generated.Faction.IsPlayer))
			{
				num *= 0.5f;
			}
			if (other.Faction != null && other.Faction.IsPlayer)
			{
				num *= request.ColonistRelationChanceFactor;
			}
			if (other == request.ExtraPawnForExtraRelationChance)
			{
				num *= request.RelationWithExtraPawnChanceFactor;
			}
			TechLevel techLevel = (generated.Faction != null) ? generated.Faction.def.techLevel : TechLevel.Undefined;
			TechLevel techLevel2 = (other.Faction != null) ? other.Faction.def.techLevel : TechLevel.Undefined;
			if (techLevel != TechLevel.Undefined && techLevel2 != TechLevel.Undefined && techLevel != techLevel2)
			{
				num *= 0.85f;
			}
			if ((techLevel.IsNeolithicOrWorse() && !techLevel2.IsNeolithicOrWorse()) || (!techLevel.IsNeolithicOrWorse() && techLevel2.IsNeolithicOrWorse()))
			{
				num *= 0.03f;
			}
			return num;
		}

		// Token: 0x060098F9 RID: 39161 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnRelationCreated(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x060098FA RID: 39162 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnRelationRemoved(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x060098FB RID: 39163 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PostRemovedByDeath(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x060098FC RID: 39164 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PostRemovedLeftBehind(Pawn firstPawn, Pawn secondPawn)
		{
		}

		// Token: 0x040067D0 RID: 26576
		public PawnRelationDef def;
	}
}
