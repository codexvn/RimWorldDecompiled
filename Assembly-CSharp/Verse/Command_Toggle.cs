using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B8B RID: 2955
	public class Command_Toggle : Command
	{
		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x06004BA0 RID: 19360 RVA: 0x00181462 File Offset: 0x0017F662
		public override SoundDef CurActivateSound
		{
			get
			{
				if (this.isActive())
				{
					return this.turnOffSound;
				}
				return this.turnOnSound;
			}
		}

		// Token: 0x06004BA1 RID: 19361 RVA: 0x0018147E File Offset: 0x0017F67E
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.toggleAction();
		}

		// Token: 0x06004BA2 RID: 19362 RVA: 0x00181494 File Offset: 0x0017F694
		public override GizmoResult GizmoOnGUI(Vector2 loc, float maxWidth, GizmoRenderParms parms)
		{
			GizmoResult result = base.GizmoOnGUI(loc, maxWidth, parms);
			if (!this.disabled || !this.hideIconIfDisabled)
			{
				Rect rect = new Rect(loc.x, loc.y, this.GetWidth(maxWidth), 75f);
				Rect position = new Rect(rect.x + rect.width - 24f, rect.y, 24f, 24f);
				Texture2D image = this.isActive() ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex;
				GUI.DrawTexture(position, image);
			}
			return result;
		}

		// Token: 0x06004BA3 RID: 19363 RVA: 0x00181524 File Offset: 0x0017F724
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			Command_Toggle command_Toggle = other as Command_Toggle;
			return command_Toggle != null && command_Toggle.isActive() == this.isActive();
		}

		// Token: 0x040035E2 RID: 13794
		public Func<bool> isActive;

		// Token: 0x040035E3 RID: 13795
		public Action toggleAction;

		// Token: 0x040035E4 RID: 13796
		public SoundDef turnOnSound = SoundDefOf.Checkbox_TurnedOn;

		// Token: 0x040035E5 RID: 13797
		public SoundDef turnOffSound = SoundDefOf.Checkbox_TurnedOff;

		// Token: 0x040035E6 RID: 13798
		public bool activateIfAmbiguous = true;

		// Token: 0x040035E7 RID: 13799
		public bool hideIconIfDisabled;
	}
}
