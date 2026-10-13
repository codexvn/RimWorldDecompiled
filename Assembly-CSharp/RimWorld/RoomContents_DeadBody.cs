using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EF6 RID: 7926
	public abstract class RoomContents_DeadBody : RoomContentsWorker
	{
		// Token: 0x17001ACC RID: 6860
		// (get) Token: 0x0600AF73 RID: 44915 RVA: 0x0032109A File Offset: 0x0031F29A
		protected virtual FloatRange CorpseAgeDaysRange
		{
			get
			{
				return new FloatRange(3f, 30f);
			}
		}

		// Token: 0x17001ACD RID: 6861
		// (get) Token: 0x0600AF74 RID: 44916 RVA: 0x003210AB File Offset: 0x0031F2AB
		protected virtual IntRange SurvivalPacksCountRange
		{
			get
			{
				return new IntRange(0, 2);
			}
		}

		// Token: 0x17001ACE RID: 6862
		// (get) Token: 0x0600AF75 RID: 44917 RVA: 0x003210B4 File Offset: 0x0031F2B4
		protected virtual IntRange BloodFilthRange
		{
			get
			{
				return new IntRange(1, 5);
			}
		}

		// Token: 0x17001ACF RID: 6863
		// (get) Token: 0x0600AF76 RID: 44918 RVA: 0x0001BC7B File Offset: 0x00019E7B
		protected virtual IntRange CorpseRange
		{
			get
			{
				return new IntRange(1, 1);
			}
		}

		// Token: 0x17001AD0 RID: 6864
		// (get) Token: 0x0600AF77 RID: 44919 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool AllHaveSameDeathAge
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001AD1 RID: 6865
		// (get) Token: 0x0600AF78 RID: 44920
		protected abstract ThingDef KillerThing { get; }

		// Token: 0x17001AD2 RID: 6866
		// (get) Token: 0x0600AF79 RID: 44921
		protected abstract DamageDef DamageType { get; }

		// Token: 0x17001AD3 RID: 6867
		// (get) Token: 0x0600AF7A RID: 44922
		protected abstract Tool ToolUsed { get; }

		// Token: 0x0600AF7B RID: 44923 RVA: 0x003210BD File Offset: 0x0031F2BD
		protected virtual IEnumerable<PawnKindDef> GetPossibleKinds()
		{
			yield return PawnKindDefOf.Pirate;
			yield return PawnKindDefOf.Villager;
			yield return PawnKindDefOf.PirateBoss;
			yield break;
		}

		// Token: 0x0600AF7C RID: 44924 RVA: 0x003210C6 File Offset: 0x0031F2C6
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.SpawnCorpses(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AF7D RID: 44925 RVA: 0x003210DC File Offset: 0x0031F2DC
		protected virtual void SpawnCorpses(Map map, LayoutRoom room)
		{
			int randomInRange = this.CorpseRange.RandomInRange;
			if (this.AllHaveSameDeathAge)
			{
				this.setAgeTicks = new int?(Mathf.RoundToInt(this.CorpseAgeDaysRange.RandomInRange * 60000f));
			}
			else
			{
				this.setAgeTicks = null;
			}
			for (int i = 0; i < randomInRange; i++)
			{
				Corpse corpse = this.SpawnCorpse(map, room);
				int randomInRange2 = this.SurvivalPacksCountRange.RandomInRange;
				for (int j = 0; j < randomInRange2; j++)
				{
					Thing thing;
					GenDrop.TryDropSpawn(ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack, null), corpse.Position, map, ThingPlaceMode.Near, out thing, null, null, true);
				}
			}
		}

		// Token: 0x0600AF7E RID: 44926 RVA: 0x00321188 File Offset: 0x0031F388
		protected Corpse SpawnCorpse(Map map, LayoutRoom room)
		{
			IntVec3 cell;
			if (!room.TryGetRandomCellInRoom(map, out cell, 1, 0, null, false))
			{
				return null;
			}
			return this.SpawnCorpse(cell, map);
		}

		// Token: 0x0600AF7F RID: 44927 RVA: 0x003211B0 File Offset: 0x0031F3B0
		protected Corpse SpawnCorpse(IntVec3 cell, Map map)
		{
			PawnKindDef kind = this.GetPossibleKinds().RandomElement<PawnKindDef>();
			int deadTicks = this.setAgeTicks ?? Mathf.RoundToInt(this.CorpseAgeDaysRange.RandomInRange * 60000f);
			return this.SpawnCorpse(cell, kind, deadTicks, map, null, false);
		}

		// Token: 0x0600AF80 RID: 44928 RVA: 0x00321214 File Offset: 0x0031F414
		protected Corpse SpawnCorpse(IntVec3 cell, PawnKindDef kind, int deadTicks, Map map, float? fixedAge = null, bool forceNoGear = false)
		{
			return RoomGenUtility.SpawnCorpse(cell, kind, deadTicks, map, this.DamageType, fixedAge, forceNoGear, this.KillerThing, this.ToolUsed, new IntRange?(this.BloodFilthRange), null);
		}

		// Token: 0x04007873 RID: 30835
		private int? setAgeTicks;
	}
}
