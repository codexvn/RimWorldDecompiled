using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000232 RID: 562
	public class CompProperties
	{
		// Token: 0x06000EA8 RID: 3752 RVA: 0x0004446B File Offset: 0x0004266B
		public CompProperties()
		{
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x00044483 File Offset: 0x00042683
		public CompProperties(Type compClass)
		{
			this.compClass = compClass;
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawGhost(IntVec3 center, Rot4 rot, ThingDef thingDef, Color ghostCol, AltitudeLayer drawAltitude, Thing thing = null)
		{
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x000444A2 File Offset: 0x000426A2
		public virtual IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has CompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x06000EAC RID: 3756 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveReferences(ThingDef parentDef)
		{
		}

		// Token: 0x06000EAD RID: 3757 RVA: 0x000444B9 File Offset: 0x000426B9
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			return Enumerable.Empty<StatDrawEntry>();
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostLoadSpecial(ThingDef parent)
		{
		}

		// Token: 0x06000EAF RID: 3759 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_PostUnlockedByResearch(ThingDef parent)
		{
		}

		// Token: 0x04000973 RID: 2419
		[TranslationHandle]
		public Type compClass = typeof(ThingComp);
	}
}
