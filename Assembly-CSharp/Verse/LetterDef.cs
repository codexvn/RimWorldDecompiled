using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029D RID: 669
	public class LetterDef : Def
	{
		// Token: 0x170002CE RID: 718
		// (get) Token: 0x060010A6 RID: 4262 RVA: 0x0004EDCF File Offset: 0x0004CFCF
		public Texture2D Icon
		{
			get
			{
				if (this.iconTex == null && !this.icon.NullOrEmpty())
				{
					this.iconTex = ContentFinder<Texture2D>.Get(this.icon, true);
				}
				return this.iconTex;
			}
		}

		// Token: 0x060010A7 RID: 4263 RVA: 0x0004EE04 File Offset: 0x0004D004
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			if (this.arriveSound == null)
			{
				this.arriveSound = SoundDefOf.LetterArrive;
			}
		}

		// Token: 0x04000DD3 RID: 3539
		public Type letterClass = typeof(StandardLetter);

		// Token: 0x04000DD4 RID: 3540
		public Color color = Color.white;

		// Token: 0x04000DD5 RID: 3541
		public Color flashColor = Color.white;

		// Token: 0x04000DD6 RID: 3542
		public float flashInterval = 90f;

		// Token: 0x04000DD7 RID: 3543
		public bool bounce;

		// Token: 0x04000DD8 RID: 3544
		public SoundDef arriveSound;

		// Token: 0x04000DD9 RID: 3545
		[NoTranslate]
		public string icon = "UI/Letters/LetterUnopened";

		// Token: 0x04000DDA RID: 3546
		public AutomaticPauseMode pauseMode = AutomaticPauseMode.AnyLetter;

		// Token: 0x04000DDB RID: 3547
		public bool forcedSlowdown;

		// Token: 0x04000DDC RID: 3548
		[Unsaved(false)]
		private Texture2D iconTex;
	}
}
