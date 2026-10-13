using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B85 RID: 2949
	[StaticConstructorOnStartup]
	public class Command_SetBedOwnerType : Command
	{
		// Token: 0x06004B85 RID: 19333 RVA: 0x00180960 File Offset: 0x0017EB60
		public Command_SetBedOwnerType(Building_Bed bed)
		{
			this.bed = bed;
			switch (bed.ForOwnerType)
			{
			case BedOwnerType.Colonist:
				this.defaultLabel = "CommandBedSetForColonistsLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForColonistsTex;
				break;
			case BedOwnerType.Prisoner:
				this.defaultLabel = "CommandBedSetForPrisonersLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForPrisonersTex;
				break;
			case BedOwnerType.Slave:
				this.defaultLabel = "CommandBedSetForSlavesLabel".Translate();
				this.icon = Command_SetBedOwnerType.ForSlavesTex;
				break;
			default:
				Log.Error(string.Format("Unknown owner type selected for bed: {0}", bed.ForOwnerType));
				break;
			}
			this.defaultDesc = "CommandBedSetForOwnerTypeDesc".Translate();
		}

		// Token: 0x06004B86 RID: 19334 RVA: 0x00180A2C File Offset: 0x0017EC2C
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			list.Add(new FloatMenuOption("CommandBedSetForColonistsLabel".Translate(), delegate()
			{
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Colonist);
			}, Command_SetBedOwnerType.ForColonistsTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandBedSetForPrisonersLabel".Translate(), delegate()
			{
				if (!Building_Bed.RoomCanBePrisonCell(this.bed.GetRoom(RegionType.Set_All)) && !this.bed.ForPrisoners)
				{
					Messages.Message("CommandBedSetForPrisonersFailOutdoors".Translate(), this.bed, MessageTypeDefOf.RejectInput, false);
					return;
				}
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Prisoner);
			}, Command_SetBedOwnerType.ForPrisonersTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			list.Add(new FloatMenuOption("CommandBedSetForSlavesLabel".Translate(), delegate()
			{
				this.bed.SetBedOwnerTypeByInterface(BedOwnerType.Slave);
			}, Command_SetBedOwnerType.ForSlavesTex, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x040035D2 RID: 13778
		private Building_Bed bed;

		// Token: 0x040035D3 RID: 13779
		private static readonly Texture2D ForColonistsTex = ContentFinder<Texture2D>.Get("UI/Commands/ForColonists", true);

		// Token: 0x040035D4 RID: 13780
		private static readonly Texture2D ForSlavesTex = ContentFinder<Texture2D>.Get("UI/Commands/ForSlaves", true);

		// Token: 0x040035D5 RID: 13781
		private static readonly Texture2D ForPrisonersTex = ContentFinder<Texture2D>.Get("UI/Commands/ForPrisoners", true);
	}
}
