using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D86 RID: 3462
	public class Verb_ArcSpray : Verb_Spray
	{
		// Token: 0x0600585F RID: 22623 RVA: 0x001C7EA8 File Offset: 0x001C60A8
		protected override void PreparePath()
		{
			this.path.Clear();
			Vector3 normalized = (this.currentTarget.CenterVector3 - this.caster.Position.ToVector3Shifted()).Yto0().normalized;
			Vector3 tan = normalized.RotatedBy(90f);
			for (int i = 0; i < this.verbProps.sprayNumExtraCells; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					float value = Rand.Value;
					float num = Rand.Value - 0.5f;
					float d = value * this.verbProps.sprayWidth * 2f - this.verbProps.sprayWidth;
					float d2 = num * (float)this.verbProps.sprayThicknessCells + num * 2f * this.verbProps.sprayArching;
					IntVec3 item = (this.currentTarget.CenterVector3 + d * tan - d2 * normalized).ToIntVec3();
					if (!this.path.Contains(item) || Rand.Value < 0.25f)
					{
						this.path.Add(item);
						break;
					}
				}
			}
			this.path.Add(this.currentTarget.Cell);
			this.path.SortBy((IntVec3 c) => (c.ToVector3Shifted() - this.caster.DrawPos).Yto0().normalized.AngleToFlat(tan));
		}
	}
}
