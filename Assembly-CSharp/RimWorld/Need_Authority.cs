using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200246E RID: 9326
	public class Need_Authority : Need
	{
		// Token: 0x17001E9D RID: 7837
		// (get) Token: 0x0600CA5C RID: 51804 RVA: 0x003B44EE File Offset: 0x003B26EE
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (this.IsCurrentlyReigning || this.IsCurrentlyGivingSpeech)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x17001E9E RID: 7838
		// (get) Token: 0x0600CA5D RID: 51805 RVA: 0x003B4510 File Offset: 0x003B2710
		public AuthorityCategory CurCategory
		{
			get
			{
				float curLevel = this.CurLevel;
				if (curLevel < 0.01f)
				{
					return AuthorityCategory.Gone;
				}
				if (curLevel < 0.15f)
				{
					return AuthorityCategory.Weak;
				}
				if (curLevel < 0.3f)
				{
					return AuthorityCategory.Uncertain;
				}
				if (curLevel > 0.7f && curLevel < 0.85f)
				{
					return AuthorityCategory.Strong;
				}
				if (curLevel >= 0.85f)
				{
					return AuthorityCategory.Total;
				}
				return AuthorityCategory.Normal;
			}
		}

		// Token: 0x17001E9F RID: 7839
		// (get) Token: 0x0600CA5E RID: 51806 RVA: 0x003B4564 File Offset: 0x003B2764
		public bool IsActive
		{
			get
			{
				return this.pawn.royalty != null && this.pawn.Spawned && this.pawn.Map != null && this.pawn.Map.IsPlayerHome && this.pawn.royalty.CanRequireThroneroom();
			}
		}

		// Token: 0x17001EA0 RID: 7840
		// (get) Token: 0x0600CA5F RID: 51807 RVA: 0x003B45C3 File Offset: 0x003B27C3
		protected override bool IsFrozen
		{
			get
			{
				return this.pawn.Map == null || !this.pawn.Map.IsPlayerHome || this.FallPerDay <= 0f;
			}
		}

		// Token: 0x17001EA1 RID: 7841
		// (get) Token: 0x0600CA60 RID: 51808 RVA: 0x003B45F8 File Offset: 0x003B27F8
		public float FallPerDay
		{
			get
			{
				if (this.pawn.royalty == null || !this.pawn.Spawned)
				{
					return 0f;
				}
				if (this.pawn.Map == null || !this.pawn.Map.IsPlayerHome)
				{
					return 0f;
				}
				float num = 0f;
				foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesInEffectForReading)
				{
				}
				int num2 = this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction).Count<Pawn>();
				return num * this.FallFactorCurve.Evaluate((float)num2);
			}
		}

		// Token: 0x17001EA2 RID: 7842
		// (get) Token: 0x0600CA61 RID: 51809 RVA: 0x003B46D0 File Offset: 0x003B28D0
		public override bool ShowOnNeedList
		{
			get
			{
				return this.IsActive;
			}
		}

		// Token: 0x17001EA3 RID: 7843
		// (get) Token: 0x0600CA62 RID: 51810 RVA: 0x003B46D8 File Offset: 0x003B28D8
		public bool IsCurrentlyReigning
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.Reign;
			}
		}

		// Token: 0x17001EA4 RID: 7844
		// (get) Token: 0x0600CA63 RID: 51811 RVA: 0x003B46EC File Offset: 0x003B28EC
		public bool IsCurrentlyGivingSpeech
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.GiveSpeech;
			}
		}

		// Token: 0x0600CA64 RID: 51812 RVA: 0x003B4700 File Offset: 0x003B2900
		public Need_Authority(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600CA65 RID: 51813 RVA: 0x003B4764 File Offset: 0x003B2964
		public override void NeedInterval()
		{
			float num = 400f;
			float num2 = this.FallPerDay / num;
			if (this.IsFrozen)
			{
				this.CurLevel = 1f;
				return;
			}
			if (this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction).Count <= 1)
			{
				this.SetInitialLevel();
				return;
			}
			if (this.IsCurrentlyReigning)
			{
				this.CurLevel += 2f / num;
				return;
			}
			if (this.IsCurrentlyGivingSpeech)
			{
				this.CurLevel += 3f / num;
				return;
			}
			this.CurLevel -= num2;
		}

		// Token: 0x040089AF RID: 35247
		public const float LevelGainPerDayOfReigning = 2f;

		// Token: 0x040089B0 RID: 35248
		public const float LevelGainPerDayOfGivingSpeech = 3f;

		// Token: 0x040089B1 RID: 35249
		private readonly SimpleCurve FallFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(3f, 0.5f),
				true
			},
			{
				new CurvePoint(5f, 1f),
				true
			}
		};
	}
}
