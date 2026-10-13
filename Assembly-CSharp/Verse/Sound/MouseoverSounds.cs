using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E2D RID: 3629
	public static class MouseoverSounds
	{
		// Token: 0x06005B8B RID: 23435 RVA: 0x001D49B0 File Offset: 0x001D2BB0
		public static void SilenceForNextFrame()
		{
			MouseoverSounds.forceSilenceUntilFrame = Time.frameCount + 1;
		}

		// Token: 0x06005B8C RID: 23436 RVA: 0x001D49BE File Offset: 0x001D2BBE
		public static void DoRegion(Rect rect)
		{
			MouseoverSounds.DoRegion(rect, SoundDefOf.Mouseover_Standard);
		}

		// Token: 0x06005B8D RID: 23437 RVA: 0x001D49CC File Offset: 0x001D2BCC
		public static void DoRegion(Rect rect, SoundDef sound)
		{
			if (sound == null)
			{
				return;
			}
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			Rect rect2 = new Rect(GUIUtility.GUIToScreenPoint(rect.position), rect.size);
			MouseoverSounds.MouseoverRegionCall item = default(MouseoverSounds.MouseoverRegionCall);
			item.rect = rect2;
			item.sound = sound;
			item.mouseIsOver = Mouse.IsOver(rect);
			MouseoverSounds.frameCalls.Add(item);
		}

		// Token: 0x06005B8E RID: 23438 RVA: 0x001D4A38 File Offset: 0x001D2C38
		public static void ResolveFrame()
		{
			for (int i = 0; i < MouseoverSounds.frameCalls.Count; i++)
			{
				if (MouseoverSounds.frameCalls[i].mouseIsOver)
				{
					if (MouseoverSounds.lastUsedCallInd != i && !MouseoverSounds.frameCalls[i].Matches(MouseoverSounds.lastUsedCall) && MouseoverSounds.forceSilenceUntilFrame < Time.frameCount)
					{
						MouseoverSounds.frameCalls[i].sound.PlayOneShotOnCamera(null);
					}
					MouseoverSounds.lastUsedCallInd = i;
					MouseoverSounds.lastUsedCall = MouseoverSounds.frameCalls[i];
					MouseoverSounds.frameCalls.Clear();
					return;
				}
			}
			MouseoverSounds.lastUsedCall = MouseoverSounds.MouseoverRegionCall.Invalid;
			MouseoverSounds.lastUsedCallInd = -1;
			MouseoverSounds.frameCalls.Clear();
		}

		// Token: 0x0400426B RID: 17003
		private static List<MouseoverSounds.MouseoverRegionCall> frameCalls = new List<MouseoverSounds.MouseoverRegionCall>();

		// Token: 0x0400426C RID: 17004
		private static int lastUsedCallInd = -1;

		// Token: 0x0400426D RID: 17005
		private static MouseoverSounds.MouseoverRegionCall lastUsedCall;

		// Token: 0x0400426E RID: 17006
		private static int forceSilenceUntilFrame = -1;

		// Token: 0x02000E2E RID: 3630
		private struct MouseoverRegionCall
		{
			// Token: 0x17000EA2 RID: 3746
			// (get) Token: 0x06005B90 RID: 23440 RVA: 0x001D4B06 File Offset: 0x001D2D06
			public bool IsValid
			{
				get
				{
					return this.rect.x >= 0f;
				}
			}

			// Token: 0x17000EA3 RID: 3747
			// (get) Token: 0x06005B91 RID: 23441 RVA: 0x001D4B20 File Offset: 0x001D2D20
			public static MouseoverSounds.MouseoverRegionCall Invalid
			{
				get
				{
					return new MouseoverSounds.MouseoverRegionCall
					{
						rect = new Rect(-1000f, -1000f, 0f, 0f)
					};
				}
			}

			// Token: 0x06005B92 RID: 23442 RVA: 0x001D4B56 File Offset: 0x001D2D56
			public bool Matches(MouseoverSounds.MouseoverRegionCall other)
			{
				return this.rect.Equals(other.rect);
			}

			// Token: 0x06005B93 RID: 23443 RVA: 0x001D4B6C File Offset: 0x001D2D6C
			public override string ToString()
			{
				if (!this.IsValid)
				{
					return "(Invalid)";
				}
				string str = "(rect=";
				Rect rect = this.rect;
				return str + rect.ToString() + (this.mouseIsOver ? "mouseIsOver" : "") + ")";
			}

			// Token: 0x0400426F RID: 17007
			public bool mouseIsOver;

			// Token: 0x04004270 RID: 17008
			public Rect rect;

			// Token: 0x04004271 RID: 17009
			public SoundDef sound;
		}
	}
}
