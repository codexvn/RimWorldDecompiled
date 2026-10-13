using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003016 RID: 12310
	public class Alert_ThroneroomInvalidConfiguration : Alert
	{
		// Token: 0x0601151F RID: 70943 RVA: 0x004F7845 File Offset: 0x004F5A45
		public Alert_ThroneroomInvalidConfiguration()
		{
			this.defaultLabel = "ThroneroomInvalidConfiguration".Translate();
			this.defaultExplanation = "ThroneroomInvalidConfigurationDesc".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x06011520 RID: 70944 RVA: 0x004F787E File Offset: 0x004F5A7E
		public override TaggedString GetExplanation()
		{
			return base.GetExplanation() + "\n\n" + this.validationInfo;
		}

		// Token: 0x06011521 RID: 70945 RVA: 0x004F789C File Offset: 0x004F5A9C
		public override AlertReport GetReport()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Thing thing in maps[i].listerThings.ThingsInGroup(ThingRequestGroup.Throne))
				{
					if (!thing.Fogged() && thing.Faction == Faction.OfPlayer)
					{
						this.validationInfo = RoomRoleWorker_ThroneRoom.Validate(thing.GetRoom(RegionType.Set_All));
						if (this.validationInfo != null)
						{
							return AlertReport.CulpritIs(thing);
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0400B37B RID: 45947
		private string validationInfo;
	}
}
