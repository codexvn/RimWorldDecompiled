using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E8 RID: 6376
	public class ThoughtWorker_PyromaniacNearFlames : ThoughtWorker
	{
		// Token: 0x06009117 RID: 37143 RVA: 0x002AAEB4 File Offset: 0x002A90B4
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = this.NumFiresNear(p);
			if (num == 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x06009118 RID: 37144 RVA: 0x002AAEEF File Offset: 0x002A90EF
		public override float MoodMultiplier(Pawn p)
		{
			return (float)this.NumFiresNear(p);
		}

		// Token: 0x06009119 RID: 37145 RVA: 0x002AAEF9 File Offset: 0x002A90F9
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			return this.NumFiresNear(p) > 0;
		}

		// Token: 0x0600911A RID: 37146 RVA: 0x002AAF0C File Offset: 0x002A910C
		private int NumFiresNear(Pawn p)
		{
			int num = 0;
			Room room = p.Position.GetRoom(p.Map);
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(p.Position, 8f, true))
			{
				if (intVec.InBounds(p.Map) && !intVec.Fogged(p.Map))
				{
					Room room2 = intVec.GetRoom(p.Map);
					if (room2 != null && room == room2)
					{
						num += FireUtility.NumFiresAt(intVec, p.Map);
						if (num >= this.def.stackLimit)
						{
							break;
						}
					}
				}
			}
			return Mathf.Min(num, this.def.stackLimit);
		}

		// Token: 0x04005C70 RID: 23664
		private const float MaxDist = 8f;
	}
}
