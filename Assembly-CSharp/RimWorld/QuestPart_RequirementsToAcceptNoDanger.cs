using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001C8D RID: 7309
	public class QuestPart_RequirementsToAcceptNoDanger : QuestPart_RequirementsToAccept
	{
		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x0600A2DE RID: 41694 RVA: 0x002ECF32 File Offset: 0x002EB132
		public Map TargetMap
		{
			get
			{
				if (this.mapParent != null && this.mapParent.HasMap)
				{
					return this.mapParent.Map;
				}
				if (this.mapPawn != null)
				{
					return this.mapPawn.MapHeld;
				}
				return null;
			}
		}

		// Token: 0x17001908 RID: 6408
		// (get) Token: 0x0600A2DF RID: 41695 RVA: 0x002ECF6A File Offset: 0x002EB16A
		public override IEnumerable<GlobalTargetInfo> Culprits
		{
			get
			{
				IAttackTarget attackTarget;
				if (this.TargetMap != null && GenHostility.AnyHostileActiveThreatTo(this.TargetMap, this.dangerTo, out attackTarget, false, false))
				{
					yield return (Thing)attackTarget;
				}
				yield break;
			}
		}

		// Token: 0x0600A2E0 RID: 41696 RVA: 0x002ECF7A File Offset: 0x002EB17A
		public override AcceptanceReport CanAccept()
		{
			if (this.TargetMap != null && GenHostility.AnyHostileActiveThreatTo(this.TargetMap, this.dangerTo, false, false))
			{
				return new AcceptanceReport("QuestRequiresNoDangerOnMap".Translate());
			}
			return true;
		}

		// Token: 0x0600A2E1 RID: 41697 RVA: 0x002ECFB4 File Offset: 0x002EB1B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<Pawn>(ref this.mapPawn, "mapPawn", false);
			Scribe_References.Look<Faction>(ref this.dangerTo, "dangerTo", false);
		}

		// Token: 0x040070A0 RID: 28832
		public MapParent mapParent;

		// Token: 0x040070A1 RID: 28833
		public Pawn mapPawn;

		// Token: 0x040070A2 RID: 28834
		public Faction dangerTo;
	}
}
