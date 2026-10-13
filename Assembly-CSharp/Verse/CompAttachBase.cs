using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x020008F8 RID: 2296
	public class CompAttachBase : ThingComp
	{
		// Token: 0x06003B49 RID: 15177 RVA: 0x00139B64 File Offset: 0x00137D64
		public override void CompTick()
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					this.attachments[i].Position = this.parent.Position;
				}
			}
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x00139BAC File Offset: 0x00137DAC
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			base.PostDestroy(mode, previousMap);
			if (this.attachments != null)
			{
				for (int i = this.attachments.Count - 1; i >= 0; i--)
				{
					this.attachments[i].Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06003B4B RID: 15179 RVA: 0x00139BF4 File Offset: 0x00137DF4
		public override string CompInspectStringExtra()
		{
			if (this.attachments != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.attachments.Count; i++)
				{
					stringBuilder.AppendLine(this.attachments[i].InspectStringAddon);
				}
				return stringBuilder.ToString().TrimEndNewlines();
			}
			return null;
		}

		// Token: 0x06003B4C RID: 15180 RVA: 0x00139C4C File Offset: 0x00137E4C
		public Thing GetAttachment(ThingDef def)
		{
			if (this.attachments != null)
			{
				for (int i = 0; i < this.attachments.Count; i++)
				{
					if (this.attachments[i].def == def)
					{
						return this.attachments[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06003B4D RID: 15181 RVA: 0x00139C99 File Offset: 0x00137E99
		public bool HasAttachment(ThingDef def)
		{
			return this.GetAttachment(def) != null;
		}

		// Token: 0x06003B4E RID: 15182 RVA: 0x00139CA5 File Offset: 0x00137EA5
		public void AddAttachment(AttachableThing t)
		{
			if (this.attachments == null)
			{
				this.attachments = new List<AttachableThing>();
			}
			this.attachments.Add(t);
		}

		// Token: 0x06003B4F RID: 15183 RVA: 0x00139CC6 File Offset: 0x00137EC6
		public void RemoveAttachment(AttachableThing t)
		{
			this.attachments.Remove(t);
		}

		// Token: 0x04002AE6 RID: 10982
		public List<AttachableThing> attachments;
	}
}
