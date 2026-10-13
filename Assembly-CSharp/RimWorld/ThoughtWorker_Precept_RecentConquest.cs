using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E8 RID: 6120
	public class ThoughtWorker_Precept_RecentConquest : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x06008E44 RID: 36420 RVA: 0x002A2944 File Offset: 0x002A0B44
		private float DaysSinceLastRaid
		{
			get
			{
				return (float)(Find.TickManager.TicksGame - Find.History.lastTickPlayerRaidedSomeone) / 60000f;
			}
		}

		// Token: 0x06008E45 RID: 36421 RVA: 0x002A2964 File Offset: 0x002A0B64
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			if (p.IsSlave)
			{
				return false;
			}
			int num = (this.DaysSinceLastRaid > (float)this.DaysSinceLastRaidThreshold) ? 1 : 0;
			if (num == 1)
			{
				int num2 = Find.TickManager.SettleTick;
				if (p.Ideo.Fluid && p.Ideo.development.lastTickRaidingApproved > num2)
				{
					num2 = p.Ideo.development.lastTickRaidingApproved;
				}
				if ((float)(Find.TickManager.TicksGame - num2) < 1800000f)
				{
					return false;
				}
			}
			return ThoughtState.ActiveAtStage(num);
		}

		// Token: 0x06008E46 RID: 36422 RVA: 0x002A2A04 File Offset: 0x002A0C04
		public override float MoodMultiplier(Pawn p)
		{
			return ThoughtWorker_Precept_RecentConquest.MoodMultiplierCurve.Evaluate(this.DaysSinceLastRaid);
		}

		// Token: 0x06008E47 RID: 36423 RVA: 0x002A2A16 File Offset: 0x002A0C16
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return this.DaysSinceLastRaidThreshold.Named("DAYSSINCELASTRAIDTHRESHOLD");
			yield break;
		}

		// Token: 0x04005BF4 RID: 23540
		private static readonly SimpleCurve MoodMultiplierCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(25f, 0f),
				true
			},
			{
				new CurvePoint(50f, 1f),
				true
			},
			{
				new CurvePoint(75f, 1.5f),
				true
			}
		};

		// Token: 0x04005BF5 RID: 23541
		private int DaysSinceLastRaidThreshold = 25;
	}
}
