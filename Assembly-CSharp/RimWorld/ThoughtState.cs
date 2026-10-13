using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x020017DE RID: 6110
	public struct ThoughtState
	{
		// Token: 0x170015B5 RID: 5557
		// (get) Token: 0x06008E18 RID: 36376 RVA: 0x002A22CD File Offset: 0x002A04CD
		public bool Active
		{
			get
			{
				return this.stageIndex != -99999;
			}
		}

		// Token: 0x170015B6 RID: 5558
		// (get) Token: 0x06008E19 RID: 36377 RVA: 0x002A22DF File Offset: 0x002A04DF
		public int StageIndex
		{
			get
			{
				return this.stageIndex;
			}
		}

		// Token: 0x170015B7 RID: 5559
		// (get) Token: 0x06008E1A RID: 36378 RVA: 0x002A22E7 File Offset: 0x002A04E7
		public string Reason
		{
			get
			{
				return this.reason;
			}
		}

		// Token: 0x170015B8 RID: 5560
		// (get) Token: 0x06008E1B RID: 36379 RVA: 0x002A22EF File Offset: 0x002A04EF
		public static ThoughtState ActiveDefault
		{
			get
			{
				return ThoughtState.ActiveAtStage(0);
			}
		}

		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x06008E1C RID: 36380 RVA: 0x002A22F8 File Offset: 0x002A04F8
		public static ThoughtState Inactive
		{
			get
			{
				return new ThoughtState
				{
					stageIndex = -99999
				};
			}
		}

		// Token: 0x06008E1D RID: 36381 RVA: 0x002A231C File Offset: 0x002A051C
		public static ThoughtState ActiveAtStage(int stageIndex)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex
			};
		}

		// Token: 0x06008E1E RID: 36382 RVA: 0x002A233C File Offset: 0x002A053C
		public static ThoughtState ActiveAtStage(int stageIndex, string reason)
		{
			return new ThoughtState
			{
				stageIndex = stageIndex,
				reason = reason
			};
		}

		// Token: 0x06008E1F RID: 36383 RVA: 0x002A2364 File Offset: 0x002A0564
		public static ThoughtState ActiveWithReason(string reason)
		{
			ThoughtState activeDefault = ThoughtState.ActiveDefault;
			activeDefault.reason = reason;
			return activeDefault;
		}

		// Token: 0x06008E20 RID: 36384 RVA: 0x002A2380 File Offset: 0x002A0580
		public static implicit operator ThoughtState(bool value)
		{
			if (value)
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x06008E21 RID: 36385 RVA: 0x002A2390 File Offset: 0x002A0590
		public bool ActiveFor(ThoughtDef thoughtDef)
		{
			if (!this.Active)
			{
				return false;
			}
			int num = this.StageIndexFor(thoughtDef);
			return num >= 0 && thoughtDef.stages[num] != null;
		}

		// Token: 0x06008E22 RID: 36386 RVA: 0x002A23C4 File Offset: 0x002A05C4
		public int StageIndexFor(ThoughtDef thoughtDef)
		{
			return Mathf.Min(this.StageIndex, thoughtDef.stages.Count - 1);
		}

		// Token: 0x04005BED RID: 23533
		private int stageIndex;

		// Token: 0x04005BEE RID: 23534
		private string reason;

		// Token: 0x04005BEF RID: 23535
		private const int InactiveIndex = -99999;
	}
}
