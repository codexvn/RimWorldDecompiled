using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02002DE3 RID: 11747
	public class Gizmo_SetFuelLevel : Gizmo_Slider
	{
		// Token: 0x17002A3C RID: 10812
		// (get) Token: 0x0601087D RID: 67709 RVA: 0x004C426D File Offset: 0x004C246D
		// (set) Token: 0x0601087E RID: 67710 RVA: 0x004C428B File Offset: 0x004C248B
		protected override float Target
		{
			get
			{
				return this.refuelable.TargetFuelLevel / this.refuelable.Props.fuelCapacity;
			}
			set
			{
				this.refuelable.TargetFuelLevel = value * this.refuelable.Props.fuelCapacity;
			}
		}

		// Token: 0x17002A3D RID: 10813
		// (get) Token: 0x0601087F RID: 67711 RVA: 0x004C42AA File Offset: 0x004C24AA
		protected override float ValuePercent
		{
			get
			{
				return this.refuelable.FuelPercentOfMax;
			}
		}

		// Token: 0x17002A3E RID: 10814
		// (get) Token: 0x06010880 RID: 67712 RVA: 0x004C42B7 File Offset: 0x004C24B7
		protected override string Title
		{
			get
			{
				return this.refuelable.Props.FuelGizmoLabel;
			}
		}

		// Token: 0x17002A3F RID: 10815
		// (get) Token: 0x06010881 RID: 67713 RVA: 0x004C42C9 File Offset: 0x004C24C9
		protected override bool IsDraggable
		{
			get
			{
				return this.refuelable.Props.targetFuelLevelConfigurable;
			}
		}

		// Token: 0x17002A40 RID: 10816
		// (get) Token: 0x06010882 RID: 67714 RVA: 0x004C42DB File Offset: 0x004C24DB
		protected override string BarLabel
		{
			get
			{
				return this.refuelable.Fuel.ToStringDecimalIfSmall() + " / " + this.refuelable.Props.fuelCapacity.ToStringDecimalIfSmall();
			}
		}

		// Token: 0x17002A41 RID: 10817
		// (get) Token: 0x06010883 RID: 67715 RVA: 0x004C430C File Offset: 0x004C250C
		// (set) Token: 0x06010884 RID: 67716 RVA: 0x004C4313 File Offset: 0x004C2513
		protected override bool DraggingBar
		{
			get
			{
				return Gizmo_SetFuelLevel.draggingBar;
			}
			set
			{
				Gizmo_SetFuelLevel.draggingBar = value;
			}
		}

		// Token: 0x06010885 RID: 67717 RVA: 0x004C431B File Offset: 0x004C251B
		public Gizmo_SetFuelLevel(CompRefuelable refuelable)
		{
			this.refuelable = refuelable;
		}

		// Token: 0x06010886 RID: 67718 RVA: 0x004C432C File Offset: 0x004C252C
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				return base.GizmoOnGUI(topLeft, maxWidth, parms);
			}
			KeyCode keyCode = (KeyBindingDefOf.Command_ItemForbid == null) ? KeyCode.None : KeyBindingDefOf.Command_ItemForbid.MainKey;
			if (keyCode != KeyCode.None && !GizmoGridDrawer.drawnHotKeys.Contains(keyCode) && KeyBindingDefOf.Command_ItemForbid.KeyDownEvent)
			{
				this.ToggleAutoRefuel();
				Event.current.Use();
			}
			return base.GizmoOnGUI(topLeft, maxWidth, parms);
		}

		// Token: 0x06010887 RID: 67719 RVA: 0x004C4394 File Offset: 0x004C2594
		protected override void DrawHeader(Rect headerRect, ref bool mouseOverElement)
		{
			if (this.refuelable.Props.showAllowAutoRefuelToggle)
			{
				headerRect.xMax -= 24f;
				Rect rect = new Rect(headerRect.xMax, headerRect.y, 24f, 24f);
				GUI.DrawTexture(rect, this.refuelable.Props.FuelIcon);
				GUI.DrawTexture(new Rect(rect.center.x, rect.y, rect.width / 2f, rect.height / 2f), this.refuelable.allowAutoRefuel ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
				if (Widgets.ButtonInvisible(rect, true))
				{
					this.ToggleAutoRefuel();
				}
				if (Mouse.IsOver(rect))
				{
					Widgets.DrawHighlight(rect);
					TooltipHandler.TipRegion(rect, new Func<string>(this.RefuelTip), 828267373);
					mouseOverElement = true;
				}
			}
			base.DrawHeader(headerRect, ref mouseOverElement);
		}

		// Token: 0x06010888 RID: 67720 RVA: 0x004C448D File Offset: 0x004C268D
		private void ToggleAutoRefuel()
		{
			this.refuelable.allowAutoRefuel = !this.refuelable.allowAutoRefuel;
			if (this.refuelable.allowAutoRefuel)
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				return;
			}
			SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
		}

		// Token: 0x06010889 RID: 67721 RVA: 0x004C44CC File Offset: 0x004C26CC
		private string RefuelTip()
		{
			string str = string.Format("{0}", "CommandToggleAllowAutoRefuel".Translate()) + "\n\n";
			string str2 = this.refuelable.allowAutoRefuel ? "On".Translate() : "Off".Translate();
			string value = this.refuelable.TargetFuelLevel.ToString("F0").Colorize(ColoredText.TipSectionTitleColor);
			string t = str + "CommandToggleAllowAutoRefuelDesc".Translate(value, str2.UncapitalizeFirst().Named("ONOFF")).Resolve() + "\n\n";
			string t2 = KeyPrefs.KeyPrefsData.GetBoundKeyCode(KeyBindingDefOf.Command_ItemForbid, KeyPrefs.BindingSlot.A).ToStringReadable();
			return t + ("HotKeyTip".Translate() + ": " + t2);
		}

		// Token: 0x0601088A RID: 67722 RVA: 0x00035ADE File Offset: 0x00033CDE
		protected override string GetTooltip()
		{
			return "";
		}

		// Token: 0x0400AC6B RID: 44139
		private CompRefuelable refuelable;

		// Token: 0x0400AC6C RID: 44140
		private static bool draggingBar;
	}
}
