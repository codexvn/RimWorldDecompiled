using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200020D RID: 525
	public class BodyDef : Def
	{
		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000DFD RID: 3581 RVA: 0x00042293 File Offset: 0x00040493
		public List<BodyPartRecord> AllParts
		{
			get
			{
				return this.cachedAllParts;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000DFE RID: 3582 RVA: 0x0004229B File Offset: 0x0004049B
		public List<BodyPartRecord> AllPartsVulnerableToFrostbite
		{
			get
			{
				return this.cachedPartsVulnerableToFrostbite;
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x000422A4 File Offset: 0x000404A4
		public List<BodyPartRecord> GetPartsWithTag(BodyPartTagDef tag)
		{
			if (!this.cachedPartsByTag.ContainsKey(tag))
			{
				this.cachedPartsByTag[tag] = new List<BodyPartRecord>();
				for (int i = 0; i < this.AllParts.Count; i++)
				{
					BodyPartRecord bodyPartRecord = this.AllParts[i];
					if (bodyPartRecord.def.tags.Contains(tag))
					{
						this.cachedPartsByTag[tag].Add(bodyPartRecord);
					}
				}
			}
			return this.cachedPartsByTag[tag];
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00042324 File Offset: 0x00040524
		public List<BodyPartRecord> GetPartsWithDef(BodyPartDef def)
		{
			if (!this.cachedPartsByDef.ContainsKey(def))
			{
				this.cachedPartsByDef[def] = new List<BodyPartRecord>();
				for (int i = 0; i < this.AllParts.Count; i++)
				{
					BodyPartRecord bodyPartRecord = this.AllParts[i];
					if (bodyPartRecord.def == def)
					{
						this.cachedPartsByDef[def].Add(bodyPartRecord);
					}
				}
			}
			return this.cachedPartsByDef[def];
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x0004239C File Offset: 0x0004059C
		public bool HasPartWithTag(BodyPartTagDef tag)
		{
			for (int i = 0; i < this.AllParts.Count; i++)
			{
				if (this.AllParts[i].def.tags.Contains(tag))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000423E0 File Offset: 0x000405E0
		public BodyPartRecord GetPartAtIndex(int index)
		{
			if (index < 0 || index >= this.cachedAllParts.Count)
			{
				return null;
			}
			return this.cachedAllParts[index];
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00042404 File Offset: 0x00040604
		public int GetIndexOfPart(BodyPartRecord rec)
		{
			for (int i = 0; i < this.cachedAllParts.Count; i++)
			{
				if (this.cachedAllParts[i] == rec)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00042439 File Offset: 0x00040639
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.cachedPartsVulnerableToFrostbite.NullOrEmpty<BodyPartRecord>())
			{
				yield return "no parts vulnerable to frostbite";
			}
			foreach (BodyPartRecord bodyPartRecord in this.AllParts)
			{
				if (bodyPartRecord.def.conceptual && bodyPartRecord.coverageAbs != 0f)
				{
					yield return string.Format("part {0} is tagged conceptual, but has nonzero coverage", bodyPartRecord);
				}
				else if (Prefs.DevMode && !bodyPartRecord.def.conceptual)
				{
					float num = 0f;
					foreach (BodyPartRecord bodyPartRecord2 in bodyPartRecord.parts)
					{
						num += bodyPartRecord2.coverage;
					}
					if (num >= 1f)
					{
						Log.Warning(string.Concat(new string[]
						{
							"BodyDef ",
							this.defName,
							" has BodyPartRecord of ",
							bodyPartRecord.def.defName,
							" whose children have more or equal coverage than 100% (",
							(num * 100f).ToString("0.00"),
							"%)"
						}));
					}
				}
			}
			List<BodyPartRecord>.Enumerator enumerator2 = default(List<BodyPartRecord>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x0004244C File Offset: 0x0004064C
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.corePart != null)
			{
				this.CacheDataRecursive(this.corePart);
			}
			this.cachedPartsVulnerableToFrostbite = new List<BodyPartRecord>();
			List<BodyPartRecord> allParts = this.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				if (allParts[i].def.frostbiteVulnerability > 0f)
				{
					this.cachedPartsVulnerableToFrostbite.Add(allParts[i]);
				}
			}
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x000424C0 File Offset: 0x000406C0
		private void CacheDataRecursive(BodyPartRecord node)
		{
			if (node.def == null)
			{
				Log.Error("BodyPartRecord with null def. body=" + ((this != null) ? this.ToString() : null));
				return;
			}
			node.body = this;
			for (int i = 0; i < node.parts.Count; i++)
			{
				node.parts[i].parent = node;
			}
			if (node.parent != null)
			{
				node.coverageAbsWithChildren = node.parent.coverageAbsWithChildren * node.coverage;
			}
			else
			{
				node.coverageAbsWithChildren = 1f;
			}
			float num = 1f;
			for (int j = 0; j < node.parts.Count; j++)
			{
				num -= node.parts[j].coverage;
			}
			if (Mathf.Abs(num) < 1E-05f)
			{
				num = 0f;
			}
			if (num <= 0f)
			{
				num = 0f;
			}
			node.coverageAbs = node.coverageAbsWithChildren * num;
			if (node.height == BodyPartHeight.Undefined)
			{
				node.height = BodyPartHeight.Middle;
			}
			if (node.depth == BodyPartDepth.Undefined)
			{
				node.depth = BodyPartDepth.Outside;
			}
			for (int k = 0; k < node.parts.Count; k++)
			{
				if (node.parts[k].height == BodyPartHeight.Undefined)
				{
					node.parts[k].height = node.height;
				}
				if (node.parts[k].depth == BodyPartDepth.Undefined)
				{
					node.parts[k].depth = node.depth;
				}
			}
			this.cachedAllParts.Add(node);
			for (int l = 0; l < node.parts.Count; l++)
			{
				this.CacheDataRecursive(node.parts[l]);
			}
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0004266E File Offset: 0x0004086E
		public override void ClearCachedData()
		{
			base.ClearCachedData();
			this.cachedPartsByTag.Clear();
			this.cachedPartsByDef.Clear();
		}

		// Token: 0x0400084D RID: 2125
		public BodyPartRecord corePart;

		// Token: 0x0400084E RID: 2126
		[Unsaved(false)]
		private List<BodyPartRecord> cachedAllParts = new List<BodyPartRecord>();

		// Token: 0x0400084F RID: 2127
		[Unsaved(false)]
		private List<BodyPartRecord> cachedPartsVulnerableToFrostbite;

		// Token: 0x04000850 RID: 2128
		[Unsaved(false)]
		public Dictionary<BodyPartTagDef, List<BodyPartRecord>> cachedPartsByTag = new Dictionary<BodyPartTagDef, List<BodyPartRecord>>();

		// Token: 0x04000851 RID: 2129
		[Unsaved(false)]
		public Dictionary<BodyPartDef, List<BodyPartRecord>> cachedPartsByDef = new Dictionary<BodyPartDef, List<BodyPartRecord>>();
	}
}
