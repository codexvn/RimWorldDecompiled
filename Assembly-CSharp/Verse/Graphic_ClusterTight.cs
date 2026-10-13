using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000890 RID: 2192
	public class Graphic_ClusterTight : Graphic_Cluster
	{
		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x060037A0 RID: 14240 RVA: 0x00129253 File Offset: 0x00127453
		protected override float PositionVariance
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x060037A1 RID: 14241 RVA: 0x0012950E File Offset: 0x0012770E
		protected override float SizeVariance
		{
			get
			{
				return 0.15f;
			}
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x00129518 File Offset: 0x00127718
		protected override int ScatterCount(Thing thing)
		{
			Filth filth = thing as Filth;
			if (filth != null && !filth.drawInstances.NullOrEmpty<Filth.FilthInstance>())
			{
				return filth.drawInstances.Count;
			}
			return 1;
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x0012954C File Offset: 0x0012774C
		protected override Vector3 GetCenter(Thing thing, int index)
		{
			Filth filth = thing as Filth;
			if (filth != null && filth.drawInstances != null && filth.drawInstances.Count > index)
			{
				return filth.drawInstances[index].drawPos;
			}
			return thing.DrawPos;
		}
	}
}
