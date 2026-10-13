using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003072 RID: 12402
	public class ColonistBarDrawLocsFinder
	{
		// Token: 0x17002D0F RID: 11535
		// (get) Token: 0x06011693 RID: 71315 RVA: 0x004FFA1B File Offset: 0x004FDC1B
		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		// Token: 0x17002D10 RID: 11536
		// (get) Token: 0x06011694 RID: 71316 RVA: 0x00500A12 File Offset: 0x004FEC12
		private static float MaxColonistBarWidth
		{
			get
			{
				return (float)UI.screenWidth - 520f;
			}
		}

		// Token: 0x06011695 RID: 71317 RVA: 0x00500A20 File Offset: 0x004FEC20
		public void CalculateDrawLocs(List<Vector2> outDrawLocs, out float scale, int groupsCount)
		{
			if (this.ColonistBar.Entries.Count == 0)
			{
				outDrawLocs.Clear();
				scale = 1f;
				return;
			}
			this.CalculateColonistsInGroup(groupsCount);
			bool onlyOneRow;
			int maxPerGlobalRow;
			scale = this.FindBestScale(out onlyOneRow, out maxPerGlobalRow, groupsCount);
			this.CalculateDrawLocs(outDrawLocs, scale, onlyOneRow, maxPerGlobalRow, groupsCount);
		}

		// Token: 0x06011696 RID: 71318 RVA: 0x00500A70 File Offset: 0x004FEC70
		private void CalculateColonistsInGroup(int groupsCount)
		{
			this.entriesInGroup.Clear();
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			for (int i = 0; i < groupsCount; i++)
			{
				this.entriesInGroup.Add(0);
			}
			for (int j = 0; j < entries.Count; j++)
			{
				List<int> list = this.entriesInGroup;
				int group = entries[j].group;
				int num = list[group];
				list[group] = num + 1;
			}
		}

		// Token: 0x06011697 RID: 71319 RVA: 0x00500AE4 File Offset: 0x004FECE4
		private int CalculateGroupsCount()
		{
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num = -1;
			int num2 = 0;
			for (int i = 0; i < entries.Count; i++)
			{
				if (num != entries[i].group)
				{
					num2++;
					num = entries[i].group;
				}
			}
			return num2;
		}

		// Token: 0x06011698 RID: 71320 RVA: 0x00500B34 File Offset: 0x004FED34
		private float FindBestScale(out bool onlyOneRow, out int maxPerGlobalRow, int groupsCount)
		{
			float num = 1f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			for (;;)
			{
				float num2 = (ColonistBar.BaseSize.x + 24f) * num;
				float num3 = ColonistBarDrawLocsFinder.MaxColonistBarWidth - (float)(groupsCount - 1) * 25f * num;
				maxPerGlobalRow = Mathf.FloorToInt(num3 / num2);
				onlyOneRow = true;
				if (this.TryDistributeHorizontalSlotsBetweenGroups(maxPerGlobalRow, groupsCount))
				{
					int allowedRowsCountForScale = ColonistBarDrawLocsFinder.GetAllowedRowsCountForScale(num);
					bool flag = true;
					int num4 = -1;
					for (int i = 0; i < entries.Count; i++)
					{
						if (num4 != entries[i].group)
						{
							num4 = entries[i].group;
							int num5 = Mathf.CeilToInt((float)this.entriesInGroup[entries[i].group] / (float)this.horizontalSlotsPerGroup[entries[i].group]);
							if (num5 > 1)
							{
								onlyOneRow = false;
							}
							if (num5 > allowedRowsCountForScale)
							{
								flag = false;
								break;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
				num *= 0.95f;
			}
			return num;
		}

		// Token: 0x06011699 RID: 71321 RVA: 0x00500C34 File Offset: 0x004FEE34
		private bool TryDistributeHorizontalSlotsBetweenGroups(int maxPerGlobalRow, int groupsCount)
		{
			this.horizontalSlotsPerGroup.Clear();
			for (int k = 0; k < groupsCount; k++)
			{
				this.horizontalSlotsPerGroup.Add(0);
			}
			GenMath.DHondtDistribution(this.horizontalSlotsPerGroup, (int i) => (float)this.entriesInGroup[i], maxPerGlobalRow);
			for (int j = 0; j < this.horizontalSlotsPerGroup.Count; j++)
			{
				if (this.horizontalSlotsPerGroup[j] == 0)
				{
					int num = this.horizontalSlotsPerGroup.Max();
					if (num <= 1)
					{
						return false;
					}
					int num2 = this.horizontalSlotsPerGroup.IndexOf(num);
					List<int> list = this.horizontalSlotsPerGroup;
					int num3 = num2;
					int num4 = list[num3];
					list[num3] = num4 - 1;
					List<int> list2 = this.horizontalSlotsPerGroup;
					num4 = j;
					num3 = list2[num4];
					list2[num4] = num3 + 1;
				}
			}
			return true;
		}

		// Token: 0x0601169A RID: 71322 RVA: 0x00500CFC File Offset: 0x004FEEFC
		private static int GetAllowedRowsCountForScale(float scale)
		{
			if (scale > 0.58f)
			{
				return 1;
			}
			if (scale > 0.42f)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x0601169B RID: 71323 RVA: 0x00500D14 File Offset: 0x004FEF14
		private void CalculateDrawLocs(List<Vector2> outDrawLocs, float scale, bool onlyOneRow, int maxPerGlobalRow, int groupsCount)
		{
			outDrawLocs.Clear();
			int num = maxPerGlobalRow;
			if (onlyOneRow)
			{
				for (int i = 0; i < this.horizontalSlotsPerGroup.Count; i++)
				{
					this.horizontalSlotsPerGroup[i] = Mathf.Min(this.horizontalSlotsPerGroup[i], this.entriesInGroup[i]);
				}
				num = this.ColonistBar.Entries.Count;
			}
			float num2 = (ColonistBar.BaseSize.x + 24f) * scale;
			float num3 = (float)num * num2 + (float)(groupsCount - 1) * 25f * scale;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			int num4 = -1;
			int num5 = -1;
			float num6 = ((float)UI.screenWidth - num3) / 2f;
			for (int j = 0; j < entries.Count; j++)
			{
				if (num4 != entries[j].group)
				{
					if (num4 >= 0)
					{
						num6 += 25f * scale;
						num6 += (float)this.horizontalSlotsPerGroup[num4] * scale * (ColonistBar.BaseSize.x + 24f);
					}
					num5 = 0;
					num4 = entries[j].group;
				}
				else
				{
					num5++;
				}
				Vector2 drawLoc = this.GetDrawLoc(num6, 21f, entries[j].group, num5, scale);
				outDrawLocs.Add(drawLoc);
			}
		}

		// Token: 0x0601169C RID: 71324 RVA: 0x00500E70 File Offset: 0x004FF070
		private Vector2 GetDrawLoc(float groupStartX, float groupStartY, int group, int numInGroup, float scale)
		{
			float num = groupStartX + (float)(numInGroup % this.horizontalSlotsPerGroup[group]) * scale * (ColonistBar.BaseSize.x + 24f);
			float y = groupStartY + (float)(numInGroup / this.horizontalSlotsPerGroup[group]) * scale * (ColonistBar.BaseSize.y + 32f);
			if (numInGroup >= this.entriesInGroup[group] - this.entriesInGroup[group] % this.horizontalSlotsPerGroup[group])
			{
				int num2 = this.horizontalSlotsPerGroup[group] - this.entriesInGroup[group] % this.horizontalSlotsPerGroup[group];
				num += (float)num2 * scale * (ColonistBar.BaseSize.x + 24f) * 0.5f;
			}
			return new Vector2(num, y);
		}

		// Token: 0x0400B468 RID: 46184
		private List<int> entriesInGroup = new List<int>();

		// Token: 0x0400B469 RID: 46185
		private List<int> horizontalSlotsPerGroup = new List<int>();

		// Token: 0x0400B46A RID: 46186
		private const float MarginTop = 21f;
	}
}
