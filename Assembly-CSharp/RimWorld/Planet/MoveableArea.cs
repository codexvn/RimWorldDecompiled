using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C52 RID: 15442
	public class MoveableArea : IExposable
	{
		// Token: 0x06014FA9 RID: 85929 RVA: 0x00647CCE File Offset: 0x00645ECE
		public MoveableArea()
		{
		}

		// Token: 0x06014FAA RID: 85930 RVA: 0x00647CE1 File Offset: 0x00645EE1
		public MoveableArea(Gravship gravship, string label, string renamableLabel, Color color, int id)
		{
			this.gravship = gravship;
			this.label = label;
			this.renamableLabel = renamableLabel;
			this.color = color;
			this.id = id;
		}

		// Token: 0x06014FAB RID: 85931 RVA: 0x00647D1C File Offset: 0x00645F1C
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<IntVec3>(ref this.relativeCells, "relativeCells", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.renamableLabel, "renamableLabel", null, false);
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<int>(ref this.id, "id", 0, false);
			Scribe_References.Look<Gravship>(ref this.gravship, "gravship", false);
		}

		// Token: 0x06014FAC RID: 85932 RVA: 0x00647DA0 File Offset: 0x00645FA0
		public void Add(IntVec3 relativeCell)
		{
			this.relativeCells.Add(relativeCell);
		}

		// Token: 0x1700340E RID: 13326
		// (get) Token: 0x06014FAD RID: 85933 RVA: 0x00647DAE File Offset: 0x00645FAE
		public IEnumerable<IntVec3> RelativeCells
		{
			get
			{
				foreach (IntVec3 local in this.relativeCells)
				{
					yield return PrefabUtility.GetAdjustedLocalPosition(local, this.gravship.Rotation);
				}
				List<IntVec3>.Enumerator enumerator = default(List<IntVec3>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0400E9DB RID: 59867
		protected List<IntVec3> relativeCells = new List<IntVec3>();

		// Token: 0x0400E9DC RID: 59868
		protected string label;

		// Token: 0x0400E9DD RID: 59869
		protected string renamableLabel;

		// Token: 0x0400E9DE RID: 59870
		protected Color color;

		// Token: 0x0400E9DF RID: 59871
		protected int id;

		// Token: 0x0400E9E0 RID: 59872
		private Gravship gravship;
	}
}
