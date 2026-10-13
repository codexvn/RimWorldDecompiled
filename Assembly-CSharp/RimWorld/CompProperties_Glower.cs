using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001970 RID: 6512
	public class CompProperties_Glower : CompProperties
	{
		// Token: 0x060093D7 RID: 37847 RVA: 0x002B79D4 File Offset: 0x002B5BD4
		public CompProperties_Glower()
		{
			this.compClass = typeof(CompGlower);
		}

		// Token: 0x060093D8 RID: 37848 RVA: 0x002B7A27 File Offset: 0x002B5C27
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (Mathf.CeilToInt(this.glowRadius) > 40)
			{
				yield return string.Format("{0} has glow radius {1} is higher than max possible value {2}", parentDef.defName, this.glowRadius, 40);
			}
			yield break;
			yield break;
		}

		// Token: 0x04005F5F RID: 24415
		public float overlightRadius;

		// Token: 0x04005F60 RID: 24416
		public float glowRadius = 14f;

		// Token: 0x04005F61 RID: 24417
		public ColorInt glowColor = new ColorInt(255, 255, 255, 0) * 1.45f;

		// Token: 0x04005F62 RID: 24418
		public bool colorPickerEnabled;

		// Token: 0x04005F63 RID: 24419
		public bool darklightToggle;

		// Token: 0x04005F64 RID: 24420
		public bool overrideIsCavePlant;
	}
}
