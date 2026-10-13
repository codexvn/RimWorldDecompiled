using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200283D RID: 10301
	public class SitePartWorker_AlphaThrumboSighting : SitePartWorker
	{
		// Token: 0x0600E168 RID: 57704 RVA: 0x004218AA File Offset: 0x0041FAAA
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}
	}
}
