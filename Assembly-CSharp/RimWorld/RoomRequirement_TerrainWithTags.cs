using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037A6 RID: 14246
	public class RoomRequirement_TerrainWithTags : RoomRequirement
	{
		// Token: 0x17003271 RID: 12913
		// (get) Token: 0x06013BC8 RID: 80840 RVA: 0x005DC934 File Offset: 0x005DAB34
		private HashSet<string> TagsSet
		{
			get
			{
				HashSet<string> result;
				if ((result = this.tagsSet) == null)
				{
					result = (this.tagsSet = new HashSet<string>(this.tags));
				}
				return result;
			}
		}

		// Token: 0x06013BC9 RID: 80841 RVA: 0x005DC960 File Offset: 0x005DAB60
		public override bool Met(Room r, Pawn p = null)
		{
			Map map = r.Map;
			foreach (IntVec3 c in r.Cells)
			{
				List<string> list = c.GetTerrain(map).tags;
				if (list.NullOrEmpty<string>())
				{
					return false;
				}
				bool flag = false;
				foreach (string item in list)
				{
					if (this.TagsSet.Contains(item))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06013BCA RID: 80842 RVA: 0x005DCA20 File Offset: 0x005DAC20
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_TerrainWithTags roomRequirement_TerrainWithTags = (RoomRequirement_TerrainWithTags)other;
			foreach (string item in this.tags)
			{
				if (!roomRequirement_TerrainWithTags.tags.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06013BCB RID: 80843 RVA: 0x005DCA94 File Offset: 0x005DAC94
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (string.IsNullOrEmpty(this.labelKey))
			{
				yield return "does not define a label key";
			}
			if (this.tags.NullOrEmpty<string>())
			{
				yield return "tags are null or empty";
			}
			yield break;
			yield break;
		}

		// Token: 0x06013BCC RID: 80844 RVA: 0x005DCAA4 File Offset: 0x005DACA4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.tagsSet = new HashSet<string>(this.tags);
			}
		}

		// Token: 0x0400DAAD RID: 55981
		public List<string> tags;

		// Token: 0x0400DAAE RID: 55982
		private HashSet<string> tagsSet;
	}
}
