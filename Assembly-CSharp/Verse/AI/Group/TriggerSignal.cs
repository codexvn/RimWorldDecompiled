using System;
using System.Text;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010F2 RID: 4338
	public struct TriggerSignal
	{
		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x0600696A RID: 26986 RVA: 0x0021348B File Offset: 0x0021168B
		public Pawn Pawn
		{
			get
			{
				return this.thing as Pawn;
			}
		}

		// Token: 0x0600696B RID: 26987 RVA: 0x00213498 File Offset: 0x00211698
		public TriggerSignal(TriggerSignalType type)
		{
			this.type = type;
			this.memo = null;
			this.signal = default(Signal);
			this.thing = null;
			this.otherThing = null;
			this.dinfo = default(DamageInfo);
			this.condition = PawnLostCondition.Undefined;
			this.faction = null;
			this.clamorType = null;
			this.previousRelationKind = null;
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x0600696C RID: 26988 RVA: 0x002134FA File Offset: 0x002116FA
		public static TriggerSignal ForTick
		{
			get
			{
				return new TriggerSignal(TriggerSignalType.Tick);
			}
		}

		// Token: 0x0600696D RID: 26989 RVA: 0x00213504 File Offset: 0x00211704
		public static TriggerSignal ForMemo(string memo)
		{
			return new TriggerSignal(TriggerSignalType.Memo)
			{
				memo = memo
			};
		}

		// Token: 0x0600696E RID: 26990 RVA: 0x00213524 File Offset: 0x00211724
		public static TriggerSignal ForSignal(Signal signal)
		{
			return new TriggerSignal(TriggerSignalType.Signal)
			{
				signal = signal
			};
		}

		// Token: 0x0600696F RID: 26991 RVA: 0x00213544 File Offset: 0x00211744
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			stringBuilder.Append(this.type.ToString());
			if (this.memo != null)
			{
				stringBuilder.Append(", memo=" + this.memo);
			}
			if (this.Pawn != null)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = ", pawn=";
				Pawn pawn = this.Pawn;
				stringBuilder2.Append(str + ((pawn != null) ? pawn.ToString() : null));
			}
			if (this.dinfo.Def != null)
			{
				StringBuilder stringBuilder3 = stringBuilder;
				string str2 = ", dinfo=";
				DamageInfo damageInfo = this.dinfo;
				stringBuilder3.Append(str2 + damageInfo.ToString());
			}
			if (this.condition != PawnLostCondition.Undefined)
			{
				stringBuilder.Append(", condition=" + this.condition.ToString());
			}
			if (this.signal.tag != null)
			{
				StringBuilder stringBuilder4 = stringBuilder;
				string str3 = ", signal=";
				Signal signal = this.signal;
				stringBuilder4.Append(str3 + signal.ToString());
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		// Token: 0x04004A9C RID: 19100
		public TriggerSignalType type;

		// Token: 0x04004A9D RID: 19101
		public string memo;

		// Token: 0x04004A9E RID: 19102
		public Signal signal;

		// Token: 0x04004A9F RID: 19103
		public Thing thing;

		// Token: 0x04004AA0 RID: 19104
		public Thing otherThing;

		// Token: 0x04004AA1 RID: 19105
		public DamageInfo dinfo;

		// Token: 0x04004AA2 RID: 19106
		public PawnLostCondition condition;

		// Token: 0x04004AA3 RID: 19107
		public Faction faction;

		// Token: 0x04004AA4 RID: 19108
		public FactionRelationKind? previousRelationKind;

		// Token: 0x04004AA5 RID: 19109
		public ClamorDef clamorType;
	}
}
