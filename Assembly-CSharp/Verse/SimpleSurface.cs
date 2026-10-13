using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C86 RID: 3206
	public class SimpleSurface : IEnumerable<SurfaceColumn>, IEnumerable
	{
		// Token: 0x06005241 RID: 21057 RVA: 0x001A9E30 File Offset: 0x001A8030
		public float Evaluate(float x, float y)
		{
			if (this.columns.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve2D with no columns.");
				return 0f;
			}
			if (x <= this.columns[0].x)
			{
				return this.columns[0].y.Evaluate(y);
			}
			if (x >= this.columns[this.columns.Count - 1].x)
			{
				return this.columns[this.columns.Count - 1].y.Evaluate(y);
			}
			SurfaceColumn surfaceColumn = this.columns[0];
			SurfaceColumn surfaceColumn2 = this.columns[this.columns.Count - 1];
			int i = 0;
			while (i < this.columns.Count)
			{
				if (x <= this.columns[i].x)
				{
					surfaceColumn2 = this.columns[i];
					if (i > 0)
					{
						surfaceColumn = this.columns[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - surfaceColumn.x) / (surfaceColumn2.x - surfaceColumn.x);
			return Mathf.Lerp(surfaceColumn.y.Evaluate(y), surfaceColumn2.y.Evaluate(y), t);
		}

		// Token: 0x06005242 RID: 21058 RVA: 0x001A9F71 File Offset: 0x001A8171
		public void Add(SurfaceColumn newColumn)
		{
			this.columns.Add(newColumn);
		}

		// Token: 0x06005243 RID: 21059 RVA: 0x001A9F7F File Offset: 0x001A817F
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06005244 RID: 21060 RVA: 0x001A9F87 File Offset: 0x001A8187
		public IEnumerator<SurfaceColumn> GetEnumerator()
		{
			foreach (SurfaceColumn surfaceColumn in this.columns)
			{
				yield return surfaceColumn;
			}
			List<SurfaceColumn>.Enumerator enumerator = default(List<SurfaceColumn>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06005245 RID: 21061 RVA: 0x001A9F96 File Offset: 0x001A8196
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.columns.Count - 1; i++)
			{
				if (this.columns[i + 1].x < this.columns[i].x)
				{
					yield return prefix + ": columns are out of order";
					break;
				}
			}
			yield break;
		}

		// Token: 0x04003AF9 RID: 15097
		private List<SurfaceColumn> columns = new List<SurfaceColumn>();
	}
}
