using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000208 RID: 520
	public class BodyPartRecord
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000DCB RID: 3531 RVA: 0x00041A53 File Offset: 0x0003FC53
		public bool IsCorePart
		{
			get
			{
				return this.parent == null;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000DCC RID: 3532 RVA: 0x00041A5E File Offset: 0x0003FC5E
		public string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				return this.def.label;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x00041A7F File Offset: 0x0003FC7F
		public string LabelCap
		{
			get
			{
				if (this.customLabel.NullOrEmpty())
				{
					return this.def.LabelCap;
				}
				if (this.cachedCustomLabelCap == null)
				{
					this.cachedCustomLabelCap = this.customLabel.CapitalizeFirst();
				}
				return this.cachedCustomLabelCap;
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000DCE RID: 3534 RVA: 0x00041ABE File Offset: 0x0003FCBE
		public string LabelShort
		{
			get
			{
				return this.def.LabelShort;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x00041ACB File Offset: 0x0003FCCB
		public string LabelShortCap
		{
			get
			{
				return this.def.LabelShortCap;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000DD0 RID: 3536 RVA: 0x00041AD8 File Offset: 0x0003FCD8
		public int Index
		{
			get
			{
				return this.body.GetIndexOfPart(this);
			}
		}

		// Token: 0x06000DD1 RID: 3537 RVA: 0x00041AE8 File Offset: 0x0003FCE8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"BodyPartRecord(",
				(this.def != null) ? this.def.defName : "NULL_DEF",
				" parts.Count=",
				this.parts.Count.ToString(),
				")"
			});
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x00041B4B File Offset: 0x0003FD4B
		public void PostLoad()
		{
			this.untranslatedCustomLabel = this.customLabel;
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x00041B5C File Offset: 0x0003FD5C
		public bool IsInGroup(BodyPartGroupDef group)
		{
			for (int i = 0; i < this.groups.Count; i++)
			{
				if (this.groups[i] == group)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DD4 RID: 3540 RVA: 0x00041B91 File Offset: 0x0003FD91
		public IEnumerable<BodyPartRecord> GetChildParts(BodyPartTagDef tag)
		{
			if (this.def.tags.Contains(tag))
			{
				yield return this;
			}
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				foreach (BodyPartRecord bodyPartRecord in this.parts[i].GetChildParts(tag))
				{
					yield return bodyPartRecord;
				}
				IEnumerator<BodyPartRecord> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00041BA8 File Offset: 0x0003FDA8
		public IEnumerable<BodyPartRecord> GetPartAndAllChildParts()
		{
			yield return this;
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				foreach (BodyPartRecord bodyPartRecord in this.parts[i].GetPartAndAllChildParts())
				{
					yield return bodyPartRecord;
				}
				IEnumerator<BodyPartRecord> enumerator = null;
				num = i + 1;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000DD6 RID: 3542 RVA: 0x00041BB8 File Offset: 0x0003FDB8
		public IEnumerable<BodyPartRecord> GetDirectChildParts()
		{
			int num;
			for (int i = 0; i < this.parts.Count; i = num)
			{
				yield return this.parts[i];
				num = i + 1;
			}
			yield break;
		}

		// Token: 0x06000DD7 RID: 3543 RVA: 0x00041BC8 File Offset: 0x0003FDC8
		public bool HasChildParts(BodyPartTagDef tag)
		{
			return this.GetChildParts(tag).Any<BodyPartRecord>();
		}

		// Token: 0x06000DD8 RID: 3544 RVA: 0x00041BD6 File Offset: 0x0003FDD6
		public IEnumerable<BodyPartRecord> GetConnectedParts(BodyPartTagDef tag)
		{
			BodyPartRecord bodyPartRecord = this;
			while (bodyPartRecord.parent != null && bodyPartRecord.parent.def.tags.Contains(tag))
			{
				bodyPartRecord = bodyPartRecord.parent;
			}
			foreach (BodyPartRecord bodyPartRecord2 in bodyPartRecord.GetChildParts(tag))
			{
				yield return bodyPartRecord2;
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000823 RID: 2083
		public BodyDef body;

		// Token: 0x04000824 RID: 2084
		[TranslationHandle]
		public BodyPartDef def;

		// Token: 0x04000825 RID: 2085
		[MustTranslate]
		public string customLabel;

		// Token: 0x04000826 RID: 2086
		[Unsaved(false)]
		[TranslationHandle(Priority = 100)]
		public string untranslatedCustomLabel;

		// Token: 0x04000827 RID: 2087
		public List<BodyPartRecord> parts = new List<BodyPartRecord>();

		// Token: 0x04000828 RID: 2088
		public BodyPartHeight height;

		// Token: 0x04000829 RID: 2089
		public BodyPartDepth depth;

		// Token: 0x0400082A RID: 2090
		public float coverage = 1f;

		// Token: 0x0400082B RID: 2091
		public List<BodyPartGroupDef> groups = new List<BodyPartGroupDef>();

		// Token: 0x0400082C RID: 2092
		[NoTranslate]
		public string woundAnchorTag;

		// Token: 0x0400082D RID: 2093
		public bool flipGraphic;

		// Token: 0x0400082E RID: 2094
		public List<Rot4> visibleHediffRots;

		// Token: 0x0400082F RID: 2095
		[Unsaved(false)]
		public BodyPartRecord parent;

		// Token: 0x04000830 RID: 2096
		[Unsaved(false)]
		public float coverageAbsWithChildren;

		// Token: 0x04000831 RID: 2097
		[Unsaved(false)]
		public float coverageAbs;

		// Token: 0x04000832 RID: 2098
		[Unsaved(false)]
		private string cachedCustomLabelCap;
	}
}
