using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037B7 RID: 14263
	public class RoomRequirement_ForbiddenBuildings : RoomRequirement
	{
		// Token: 0x17003282 RID: 12930
		// (get) Token: 0x06013C46 RID: 80966 RVA: 0x005DDE0C File Offset: 0x005DC00C
		private HashSet<string> BuildingTagsSet
		{
			get
			{
				HashSet<string> result;
				if ((result = this.buildingTagsSet) == null)
				{
					result = (this.buildingTagsSet = new HashSet<string>(this.buildingTags));
				}
				return result;
			}
		}

		// Token: 0x06013C47 RID: 80967 RVA: 0x005DDE38 File Offset: 0x005DC038
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (Region region in r.Regions)
			{
				List<Thing> allThings = region.ListerThings.AllThings;
				for (int i = 0; i < allThings.Count; i++)
				{
					Thing thing = allThings[i];
					if (thing.def.building != null && thing.def.building.buildingTags != null)
					{
						foreach (string item in thing.def.building.buildingTags)
						{
							if (this.BuildingTagsSet.Contains(item))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06013C48 RID: 80968 RVA: 0x005DDF30 File Offset: 0x005DC130
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.buildingTags, "buildingTags", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.buildingTagsSet = new HashSet<string>(this.buildingTags);
			}
		}

		// Token: 0x0400DADC RID: 56028
		public List<string> buildingTags = new List<string>();

		// Token: 0x0400DADD RID: 56029
		private HashSet<string> buildingTagsSet;
	}
}
