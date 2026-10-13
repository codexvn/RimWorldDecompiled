using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003720 RID: 14112
	public class StatPart_RoomStat : StatPart
	{
		// Token: 0x060138FD RID: 80125 RVA: 0x005CD059 File Offset: 0x005CB259
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x060138FE RID: 80126 RVA: 0x005CD068 File Offset: 0x005CB268
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					val *= room.GetStat(this.roomStat);
				}
			}
		}

		// Token: 0x060138FF RID: 80127 RVA: 0x005CD0A4 File Offset: 0x005CB2A4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					string str;
					if (!this.customLabel.NullOrEmpty())
					{
						str = this.customLabel;
					}
					else
					{
						str = this.roomStat.LabelCap;
					}
					return str + ": x" + room.GetStat(this.roomStat).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0400D8FD RID: 55549
		private RoomStatDef roomStat;

		// Token: 0x0400D8FE RID: 55550
		[MustTranslate]
		private string customLabel;

		// Token: 0x0400D8FF RID: 55551
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;
	}
}
