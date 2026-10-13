using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B6E RID: 15214
	public class QuestNode_Root_Mission_BanditCamp : QuestNode_Root_Mission
	{
		// Token: 0x1700336A RID: 13162
		// (get) Token: 0x06014AC9 RID: 84681 RVA: 0x0062EAB9 File Offset: 0x0062CCB9
		protected override string QuestTag
		{
			get
			{
				return "BanditCamp";
			}
		}

		// Token: 0x1700336B RID: 13163
		// (get) Token: 0x06014ACA RID: 84682 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool AddCampLootReward
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700336C RID: 13164
		// (get) Token: 0x06014ACB RID: 84683 RVA: 0x0062EAC0 File Offset: 0x0062CCC0
		private QuestGen_Pawns.GetPawnParms GetAskerParms
		{
			get
			{
				return new QuestGen_Pawns.GetPawnParms
				{
					mustBeOfKind = PawnKindDefOf.Empire_Royal_NobleWimp,
					mustHaveRoyalTitleInCurrentFaction = true,
					canGeneratePawn = true
				};
			}
		}

		// Token: 0x06014ACC RID: 84684 RVA: 0x0062EAF2 File Offset: 0x0062CCF2
		protected override Pawn GetAsker(Quest quest)
		{
			if (Rand.Chance(0.1f))
			{
				return (from f in Find.FactionManager.AllFactions
				where this.factionsToDrawLeaderFrom.Contains(f.def)
				select f).RandomElement<Faction>().leader;
			}
			return quest.GetPawn(this.GetAskerParms);
		}

		// Token: 0x06014ACD RID: 84685 RVA: 0x0062EB34 File Offset: 0x0062CD34
		protected override bool CanGetAsker()
		{
			Pawn pawn;
			return QuestGen_Pawns.GetPawnTest(this.GetAskerParms, out pawn);
		}

		// Token: 0x06014ACE RID: 84686 RVA: 0x0062EB4E File Offset: 0x0062CD4E
		private float GetSiteThreatPoints(float threatPoints, int population, int pawnCount)
		{
			return threatPoints * ((float)pawnCount / (float)population) * QuestNode_Root_Mission_BanditCamp.PawnCountToSitePointsFactorCurve.Evaluate((float)pawnCount);
		}

		// Token: 0x06014ACF RID: 84687 RVA: 0x0062EB64 File Offset: 0x0062CD64
		protected override int GetRequiredPawnCount(int population, float threatPoints)
		{
			if (population == 0)
			{
				return -1;
			}
			int num = -1;
			for (int i = 1; i < population; i++)
			{
				if (this.GetSiteThreatPoints(threatPoints, population, i) >= 200f)
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				return -1;
			}
			int maxInclusive = Math.Max(num, (int)(0.5f * (float)population));
			return Rand.RangeInclusive(num, maxInclusive);
		}

		// Token: 0x06014AD0 RID: 84688 RVA: 0x0062EBB8 File Offset: 0x0062CDB8
		protected override Site GenerateSite(Pawn asker, float threatPoints, int pawnCount, int population, PlanetTile tile)
		{
			Faction faction;
			this.TryGetSiteFaction(out faction);
			Site site = QuestGen_Sites.GenerateSite(new SitePartDefWithParams[]
			{
				new SitePartDefWithParams(SitePartDefOf.BanditCamp, new SitePartParams
				{
					threatPoints = this.GetSiteThreatPoints(threatPoints, population, pawnCount)
				})
			}, tile, faction, false, null, null);
			site.factionMustRemainHostile = true;
			site.desiredThreatPoints = site.ActualThreatPoints;
			site.preventGravshipLanding = true;
			return site;
		}

		// Token: 0x06014AD1 RID: 84689 RVA: 0x0062EC1B File Offset: 0x0062CE1B
		private bool TryGetSiteFaction(out Faction faction)
		{
			return (from f in Find.FactionManager.AllFactions
			where !f.temporary && this.siteFactions.Contains(f.def)
			select f).TryRandomElement(out faction);
		}

		// Token: 0x06014AD2 RID: 84690 RVA: 0x0062EC3E File Offset: 0x0062CE3E
		protected override bool DoesPawnCountAsAvailableForFight(Pawn p)
		{
			return QuestNode_Root_Mission.PawnCanFight(p);
		}

		// Token: 0x06014AD3 RID: 84691 RVA: 0x0062EC48 File Offset: 0x0062CE48
		protected override bool TestRunInt(Slate slate)
		{
			Faction faction;
			return base.TestRunInt(slate) && this.TryGetSiteFaction(out faction);
		}

		// Token: 0x0400E5C6 RID: 58822
		private const float LeaderChance = 0.1f;

		// Token: 0x0400E5C7 RID: 58823
		private const float MinSiteThreatPoints = 200f;

		// Token: 0x0400E5C8 RID: 58824
		private static readonly SimpleCurve PawnCountToSitePointsFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.33f),
				true
			},
			{
				new CurvePoint(3f, 0.37f),
				true
			},
			{
				new CurvePoint(5f, 0.45f),
				true
			},
			{
				new CurvePoint(10f, 0.5f),
				true
			}
		};

		// Token: 0x0400E5C9 RID: 58825
		public List<FactionDef> factionsToDrawLeaderFrom;

		// Token: 0x0400E5CA RID: 58826
		public List<FactionDef> siteFactions;
	}
}
