using System;

namespace Verse
{
	// Token: 0x020008F9 RID: 2297
	public static class AttachmentUtility
	{
		// Token: 0x06003B51 RID: 15185 RVA: 0x00139CD8 File Offset: 0x00137ED8
		public static Thing GetAttachment(this Thing t, ThingDef def)
		{
			CompAttachBase compAttachBase = t.TryGetComp<CompAttachBase>();
			if (compAttachBase == null)
			{
				return null;
			}
			return compAttachBase.GetAttachment(def);
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x00139CF8 File Offset: 0x00137EF8
		public static bool HasAttachment(this Thing t, ThingDef def)
		{
			return t.GetAttachment(def) != null;
		}
	}
}
