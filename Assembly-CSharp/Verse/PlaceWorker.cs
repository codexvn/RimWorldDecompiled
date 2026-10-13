using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C6 RID: 1222
	public abstract class PlaceWorker
	{
		// Token: 0x06001F93 RID: 8083 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsBuildDesignatorVisible(BuildableDef def)
		{
			return true;
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x000A5EF4 File Offset: 0x000A40F4
		public virtual AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
		{
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostPlace(Map map, BuildableDef def, IntVec3 loc, Rot4 rot)
		{
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ForceAllowPlaceOver(BuildableDef other)
		{
			return false;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000A5EFB File Offset: 0x000A40FB
		public virtual IEnumerable<TerrainAffordanceDef> DisplayAffordances()
		{
			return Enumerable.Empty<TerrainAffordanceDef>();
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawMouseAttachments(BuildableDef def)
		{
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawPlaceMouseAttachments(float curX, ref float curY, BuildableDef def, IntVec3 center, Rot4 rot)
		{
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawOnGUIExtra(BuildableDef def)
		{
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x000A5F04 File Offset: 0x000A4104
		protected static void DrawTextLine(float curX, ref float y, string text)
		{
			float lineHeight = Text.LineHeight;
			Widgets.Label(new Rect(curX, y, 999f, lineHeight), text);
			y += lineHeight;
		}
	}
}
