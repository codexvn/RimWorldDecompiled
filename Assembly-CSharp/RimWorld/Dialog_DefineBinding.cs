using System;
using UnityEngine;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02003188 RID: 12680
	public class Dialog_DefineBinding : Window
	{
		// Token: 0x17002E56 RID: 11862
		// (get) Token: 0x06011D5E RID: 73054 RVA: 0x0052C9B0 File Offset: 0x0052ABB0
		public override Vector2 InitialSize
		{
			get
			{
				return this.windowSize;
			}
		}

		// Token: 0x17002E57 RID: 11863
		// (get) Token: 0x06011D5F RID: 73055 RVA: 0x00006AAC File Offset: 0x00004CAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x06011D60 RID: 73056 RVA: 0x0052C9B8 File Offset: 0x0052ABB8
		public Dialog_DefineBinding(KeyPrefsData keyPrefsData, KeyBindingDef keyDef, KeyPrefs.BindingSlot slot) : base(null)
		{
			this.keyDef = keyDef;
			this.slot = slot;
			this.keyPrefsData = keyPrefsData;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.forcePause = true;
			this.onlyOneOfTypeAllowed = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011D61 RID: 73057 RVA: 0x0052CA1C File Offset: 0x0052AC1C
		public override void DoWindowContents(Rect inRect)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				Widgets.Label(inRect, "PressAnyKeyOrEscController".Translate().Resolve().AdjustedForKeys(null, true));
			}
			else
			{
				Widgets.Label(inRect, "PressAnyKeyOrEsc".Translate());
			}
			Text.Anchor = TextAnchor.UpperLeft;
			if (Event.current.isKey && Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None)
			{
				if (Event.current.keyCode != KeyCode.Escape)
				{
					this.keyPrefsData.EraseConflictingBindingsForKeyCode(this.keyDef, Event.current.keyCode, delegate(KeyBindingDef oldDef)
					{
						Messages.Message("KeyBindingOverwritten".Translate(oldDef.LabelCap), MessageTypeDefOf.TaskCompletion, false);
					});
					this.keyPrefsData.SetBinding(this.keyDef, this.slot, Event.current.keyCode);
				}
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x0400B878 RID: 47224
		protected Vector2 windowSize = new Vector2(400f, 200f);

		// Token: 0x0400B879 RID: 47225
		protected KeyPrefsData keyPrefsData;

		// Token: 0x0400B87A RID: 47226
		protected KeyBindingDef keyDef;

		// Token: 0x0400B87B RID: 47227
		protected KeyPrefs.BindingSlot slot;
	}
}
