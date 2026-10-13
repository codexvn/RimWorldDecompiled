using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001976 RID: 6518
	public class CompProperties_OrbitalBeam : CompProperties
	{
		// Token: 0x060093EB RID: 37867 RVA: 0x002B7D8D File Offset: 0x002B5F8D
		public CompProperties_OrbitalBeam()
		{
			this.compClass = typeof(CompOrbitalBeam);
		}

		// Token: 0x060093EC RID: 37868 RVA: 0x002B7DBB File Offset: 0x002B5FBB
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (parentDef.drawerType != DrawerType.RealtimeOnly && parentDef.drawerType != DrawerType.MapMeshAndRealTime)
			{
				yield return "orbital beam requires realtime drawer";
			}
			yield break;
			yield break;
		}

		// Token: 0x04005F76 RID: 24438
		public float width = 8f;

		// Token: 0x04005F77 RID: 24439
		public Color color = Color.white;

		// Token: 0x04005F78 RID: 24440
		public SoundDef sound;
	}
}
