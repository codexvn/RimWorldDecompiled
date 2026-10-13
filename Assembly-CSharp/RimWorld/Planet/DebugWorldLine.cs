using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C71 RID: 15473
	public class DebugWorldLine
	{
		// Token: 0x17003433 RID: 13363
		// (get) Token: 0x0601503B RID: 86075 RVA: 0x0064ADA0 File Offset: 0x00648FA0
		// (set) Token: 0x0601503C RID: 86076 RVA: 0x0064ADA8 File Offset: 0x00648FA8
		public int TicksLeft
		{
			get
			{
				return this.ticksLeft;
			}
			set
			{
				this.ticksLeft = value;
			}
		}

		// Token: 0x0601503D RID: 86077 RVA: 0x0064ADB1 File Offset: 0x00648FB1
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = 100;
		}

		// Token: 0x0601503E RID: 86078 RVA: 0x0064ADD6 File Offset: 0x00648FD6
		public DebugWorldLine(Vector3 a, Vector3 b, bool onPlanetSurface, int ticksLeft)
		{
			this.a = a;
			this.b = b;
			this.onPlanetSurface = onPlanetSurface;
			this.ticksLeft = ticksLeft;
		}

		// Token: 0x0601503F RID: 86079 RVA: 0x0064ADFC File Offset: 0x00648FFC
		public void Draw()
		{
			float num = Vector3.Distance(this.a, this.b);
			if (num < 0.001f)
			{
				return;
			}
			if (this.onPlanetSurface)
			{
				float averageTileSize = Find.WorldGrid.AverageTileSize;
				int num2 = Mathf.Max(Mathf.RoundToInt(num / averageTileSize), 0);
				float num3 = 0.08f;
				for (int i = 0; i < num2; i++)
				{
					Vector3 vector = Vector3.Lerp(this.a, this.b, (float)i / (float)num2);
					Vector3 vector2 = Vector3.Lerp(this.a, this.b, (float)(i + 1) / (float)num2);
					vector = vector.normalized * (100f + num3);
					vector2 = vector2.normalized * (100f + num3);
					GenDraw.DrawWorldLineBetween(vector, vector2, 1f);
				}
				return;
			}
			GenDraw.DrawWorldLineBetween(this.a, this.b, 1f);
		}

		// Token: 0x0400EA82 RID: 60034
		public Vector3 a;

		// Token: 0x0400EA83 RID: 60035
		public Vector3 b;

		// Token: 0x0400EA84 RID: 60036
		public int ticksLeft;

		// Token: 0x0400EA85 RID: 60037
		private bool onPlanetSurface;
	}
}
