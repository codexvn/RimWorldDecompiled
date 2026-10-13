using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000C33 RID: 3123
	public class Dialog_RenameZone : Dialog_Rename<Zone>
	{
		// Token: 0x0600500C RID: 20492 RVA: 0x0019C9D6 File Offset: 0x0019ABD6
		public Dialog_RenameZone(Zone zone) : base(zone)
		{
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x0019C9E0 File Offset: 0x0019ABE0
		protected override AcceptanceReport NameIsValid(string name)
		{
			AcceptanceReport result = base.NameIsValid(name);
			if (!result.Accepted)
			{
				return result;
			}
			if (this.renaming.Map.zoneManager.AllZones.Any((Zone z) => z != this.renaming && z.label == name) || Current.Game.CurrentMap.storageGroups.HasGroupWithName(name))
			{
				return "NameIsInUse".Translate();
			}
			return true;
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x0019CA73 File Offset: 0x0019AC73
		protected override void OnRenamed(string name)
		{
			Messages.Message("ZoneGainsName".Translate(name), MessageTypeDefOf.TaskCompletion, false);
		}
	}
}
