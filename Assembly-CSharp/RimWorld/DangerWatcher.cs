using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001E97 RID: 7831
	public class DangerWatcher
	{
		// Token: 0x17001A9F RID: 6815
		// (get) Token: 0x0600ADD5 RID: 44501 RVA: 0x00319350 File Offset: 0x00317550
		public StoryDanger DangerRating
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastUpdateTick + 101)
				{
					this.dangerRatingInt = this.CalculateDangerRating();
					this.lastUpdateTick = Find.TickManager.TicksGame;
				}
				return this.dangerRatingInt;
			}
		}

		// Token: 0x0600ADD6 RID: 44502 RVA: 0x0031938C File Offset: 0x0031758C
		private StoryDanger CalculateDangerRating()
		{
			float num = this.map.attackTargetsCache.TargetsHostileToColony.Where(DangerWatcher.CachedAffectsStoryDangerDelegate).Sum(delegate(IAttackTarget t)
			{
				Pawn pawn = t as Pawn;
				if (pawn != null)
				{
					return pawn.kindDef.combatPower;
				}
				Building_TurretGun building_TurretGun = t as Building_TurretGun;
				if (building_TurretGun != null && building_TurretGun.def.building.IsMortar && !building_TurretGun.IsMannable)
				{
					return building_TurretGun.def.building.combatPower;
				}
				return 0f;
			});
			if (num == 0f)
			{
				return StoryDanger.None;
			}
			int num2 = this.map.mapPawns.FreeColonistsSpawned.Count((Pawn p) => !p.Downed);
			if (num < 150f && num <= (float)num2 * 18f)
			{
				return StoryDanger.Low;
			}
			if (num > 400f)
			{
				return StoryDanger.High;
			}
			if (this.lastColonistHarmedTick > Find.TickManager.TicksGame - 900)
			{
				return StoryDanger.High;
			}
			foreach (Lord lord in this.map.lordManager.lords)
			{
				if (lord.faction.HostileTo(Faction.OfPlayer) && lord.CurLordToil.ForceHighStoryDanger && lord.AnyActivePawn)
				{
					return StoryDanger.High;
				}
			}
			return StoryDanger.Low;
		}

		// Token: 0x0600ADD7 RID: 44503 RVA: 0x003194CC File Offset: 0x003176CC
		public DangerWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x0600ADD8 RID: 44504 RVA: 0x003194F1 File Offset: 0x003176F1
		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600ADD9 RID: 44505 RVA: 0x00319504 File Offset: 0x00317704
		private static bool AffectsStoryDanger(IAttackTarget t)
		{
			Pawn pawn = t.Thing as Pawn;
			if (pawn != null)
			{
				Lord lord = pawn.GetLord();
				if (lord != null && (lord.LordJob is LordJob_DefendPoint || lord.LordJob is LordJob_MechanoidDefendBase) && pawn.CurJobDef != JobDefOf.AttackMelee && pawn.CurJobDef != JobDefOf.AttackStatic)
				{
					return false;
				}
				CompCanBeDormant comp = pawn.GetComp<CompCanBeDormant>();
				if (comp != null && !comp.Awake)
				{
					return false;
				}
			}
			return GenHostility.IsActiveThreatToPlayer(t, false);
		}

		// Token: 0x04007788 RID: 30600
		private Map map;

		// Token: 0x04007789 RID: 30601
		private StoryDanger dangerRatingInt;

		// Token: 0x0400778A RID: 30602
		private int lastUpdateTick = -10000;

		// Token: 0x0400778B RID: 30603
		private int lastColonistHarmedTick = -10000;

		// Token: 0x0400778C RID: 30604
		private const int UpdateInterval = 101;

		// Token: 0x0400778D RID: 30605
		private static readonly Func<IAttackTarget, bool> CachedAffectsStoryDangerDelegate = new Func<IAttackTarget, bool>(DangerWatcher.AffectsStoryDanger);
	}
}
