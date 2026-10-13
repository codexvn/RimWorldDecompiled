using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200029C RID: 668
	public class KeyBindingDef : Def
	{
		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x0004EAB8 File Offset: 0x0004CCB8
		public KeyCode MainKey
		{
			get
			{
				KeyBindingData keyBindingData;
				if (KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData))
				{
					if (keyBindingData.keyBindingA != KeyCode.None)
					{
						return keyBindingData.keyBindingA;
					}
					if (keyBindingData.keyBindingB != KeyCode.None)
					{
						return keyBindingData.keyBindingB;
					}
				}
				return KeyCode.None;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600109E RID: 4254 RVA: 0x0004EAF8 File Offset: 0x0004CCF8
		public string MainKeyLabel
		{
			get
			{
				return this.MainKey.ToStringReadable();
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x0004EB08 File Offset: 0x0004CD08
		public bool KeyDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current.type == EventType.KeyDown && Event.current.keyCode != KeyCode.None && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && (keyBindingData.keyBindingA == KeyCode.LeftMeta || keyBindingData.keyBindingA == KeyCode.RightMeta || keyBindingData.keyBindingB == KeyCode.LeftMeta || keyBindingData.keyBindingB == KeyCode.RightMeta || !Event.current.command) && !Find.WindowStack.AnySearchWidgetFocused && (Event.current.keyCode == keyBindingData.keyBindingA || Event.current.keyCode == keyBindingData.keyBindingB);
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060010A0 RID: 4256 RVA: 0x0004EBC0 File Offset: 0x0004CDC0
		public bool IsDownEvent
		{
			get
			{
				KeyBindingData keyBindingData;
				return Event.current != null && KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && !Find.WindowStack.AnySearchWidgetFocused && (this.KeyDownEvent || (Event.current.shift && (keyBindingData.keyBindingA == KeyCode.LeftShift || keyBindingData.keyBindingA == KeyCode.RightShift || keyBindingData.keyBindingB == KeyCode.LeftShift || keyBindingData.keyBindingB == KeyCode.RightShift)) || (Event.current.control && (keyBindingData.keyBindingA == KeyCode.LeftControl || keyBindingData.keyBindingA == KeyCode.RightControl || keyBindingData.keyBindingB == KeyCode.LeftControl || keyBindingData.keyBindingB == KeyCode.RightControl)) || (Event.current.alt && (keyBindingData.keyBindingA == KeyCode.LeftAlt || keyBindingData.keyBindingA == KeyCode.RightAlt || keyBindingData.keyBindingB == KeyCode.LeftAlt || keyBindingData.keyBindingB == KeyCode.RightAlt)) || (Event.current.command && (keyBindingData.keyBindingA == KeyCode.LeftMeta || keyBindingData.keyBindingA == KeyCode.RightMeta || keyBindingData.keyBindingB == KeyCode.LeftMeta || keyBindingData.keyBindingB == KeyCode.RightMeta)) || this.IsDown);
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x0004ED14 File Offset: 0x0004CF14
		public bool JustPressed
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && !Find.WindowStack.AnySearchWidgetFocused && (Input.GetKeyDown(keyBindingData.keyBindingA) || Input.GetKeyDown(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x060010A2 RID: 4258 RVA: 0x0004ED60 File Offset: 0x0004CF60
		public bool IsDown
		{
			get
			{
				KeyBindingData keyBindingData;
				return KeyPrefs.KeyPrefsData.keyPrefs.TryGetValue(this, out keyBindingData) && !Find.WindowStack.AnySearchWidgetFocused && (Input.GetKey(keyBindingData.keyBindingA) || Input.GetKey(keyBindingData.keyBindingB));
			}
		}

		// Token: 0x060010A3 RID: 4259 RVA: 0x0004EDAB File Offset: 0x0004CFAB
		public KeyCode GetDefaultKeyCode(KeyPrefs.BindingSlot slot)
		{
			if (slot == KeyPrefs.BindingSlot.A)
			{
				return this.defaultKeyCodeA;
			}
			if (slot == KeyPrefs.BindingSlot.B)
			{
				return this.defaultKeyCodeB;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x060010A4 RID: 4260 RVA: 0x0004EDC7 File Offset: 0x0004CFC7
		public static KeyBindingDef Named(string name)
		{
			return DefDatabase<KeyBindingDef>.GetNamedSilentFail(name);
		}

		// Token: 0x04000DCD RID: 3533
		public KeyBindingCategoryDef category;

		// Token: 0x04000DCE RID: 3534
		public KeyCode defaultKeyCodeA;

		// Token: 0x04000DCF RID: 3535
		public KeyCode defaultKeyCodeB;

		// Token: 0x04000DD0 RID: 3536
		public bool devModeOnly;

		// Token: 0x04000DD1 RID: 3537
		public List<KeyBindingDef> ignoreConflictsWith;

		// Token: 0x04000DD2 RID: 3538
		[NoTranslate]
		public List<string> extraConflictTags;
	}
}
