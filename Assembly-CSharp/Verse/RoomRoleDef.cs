using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002D8 RID: 728
	public class RoomRoleDef : Def
	{
		// Token: 0x17000329 RID: 809
		// (get) Token: 0x060011FA RID: 4602 RVA: 0x000540D4 File Offset: 0x000522D4
		public RoomRoleWorker Worker
		{
			get
			{
				RoomRoleWorker result;
				if ((result = this.workerInt) == null)
				{
					result = (this.workerInt = (RoomRoleWorker)Activator.CreateInstance(this.workerClass));
				}
				return result;
			}
		}

		// Token: 0x060011FB RID: 4603 RVA: 0x00054104 File Offset: 0x00052304
		public string PostProcessedLabel(Room room)
		{
			return this.Worker.PostProcessedLabel(this.label, room);
		}

		// Token: 0x060011FC RID: 4604 RVA: 0x00054118 File Offset: 0x00052318
		public string PostProcessedLabelCap(Room room)
		{
			return this.PostProcessedLabel(room).CapitalizeFirst();
		}

		// Token: 0x060011FD RID: 4605 RVA: 0x00054128 File Offset: 0x00052328
		public bool IsStatRelated(RoomStatDef def)
		{
			if (this.relatedStats == null)
			{
				return false;
			}
			for (int i = 0; i < this.relatedStats.Count; i++)
			{
				if (this.relatedStats[i] == def)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001037 RID: 4151
		public Type workerClass;

		// Token: 0x04001038 RID: 4152
		public bool avoidViewingArtIfUnowned;

		// Token: 0x04001039 RID: 4153
		private List<RoomStatDef> relatedStats;

		// Token: 0x0400103A RID: 4154
		private RoomRoleWorker workerInt;
	}
}
