using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002457 RID: 9303
	public class MechanitorControlGroupGizmo : Gizmo
	{
		// Token: 0x17001E6A RID: 7786
		// (get) Token: 0x0600C981 RID: 51585 RVA: 0x003AFA32 File Offset: 0x003ADC32
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				return MechanitorControlGroupGizmo.GetWorkModeOptions(this.controlGroup);
			}
		}

		// Token: 0x17001E6B RID: 7787
		// (get) Token: 0x0600C982 RID: 51586 RVA: 0x003AFA3F File Offset: 0x003ADC3F
		public override bool Visible
		{
			get
			{
				return this.controlGroup.MechsForReading.Count > 0 || Find.Selector.SelectedPawns.Count == 1;
			}
		}

		// Token: 0x17001E6C RID: 7788
		// (get) Token: 0x0600C983 RID: 51587 RVA: 0x003AFA68 File Offset: 0x003ADC68
		public override float Order
		{
			get
			{
				if (this.controlGroup.MechsForReading.Count > 0)
				{
					return -89f;
				}
				return -88f;
			}
		}

		// Token: 0x0600C984 RID: 51588 RVA: 0x003AFA88 File Offset: 0x003ADC88
		public MechanitorControlGroupGizmo(MechanitorControlGroup controlGroup)
		{
			this.controlGroup = controlGroup;
			this.Order = -89f;
		}

		// Token: 0x0600C985 RID: 51589 RVA: 0x003AFAA2 File Offset: 0x003ADCA2
		public static IEnumerable<FloatMenuOption> GetWorkModeOptions(MechanitorControlGroup controlGroup)
		{
			using (IEnumerator<MechWorkModeDef> enumerator = (from d in DefDatabase<MechWorkModeDef>.AllDefsListForReading
			orderby d.uiOrder
			select d).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MechWorkModeDef wm = enumerator.Current;
					yield return new FloatMenuOption(wm.LabelCap, delegate()
					{
						controlGroup.SetWorkMode(wm);
					}, wm.uiIcon, Color.white, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false)
					{
						tooltip = new TipSignal?(new TipSignal(wm.description, (int)wm.index ^ 234784353))
					};
				}
			}
			IEnumerator<MechWorkModeDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600C986 RID: 51590 RVA: 0x003AFAB2 File Offset: 0x003ADCB2
		public override void GizmoUpdateOnMouseover()
		{
			base.GizmoUpdateOnMouseover();
			this.controlGroup.WorkMode.Worker.DrawControlGroupMouseOverExtra(this.controlGroup);
		}

		// Token: 0x0600C987 RID: 51591 RVA: 0x003AFAD8 File Offset: 0x003ADCD8
		public override bool GroupsWith(Gizmo other)
		{
			MechanitorControlGroupGizmo mechanitorControlGroupGizmo = other as MechanitorControlGroupGizmo;
			if (mechanitorControlGroupGizmo == null)
			{
				return false;
			}
			if (mechanitorControlGroupGizmo.controlGroup == this.controlGroup)
			{
				return true;
			}
			if (this.controlGroup.Tracker == mechanitorControlGroupGizmo.controlGroup.Tracker && this.controlGroup.MechsForReading.Count == 0 && mechanitorControlGroupGizmo.controlGroup.MechsForReading.Count == 0)
			{
				return true;
			}
			if (this.mergedControlGroups.NotNullAndContains(mechanitorControlGroupGizmo.controlGroup))
			{
				this.mergedControlGroups.Remove(mechanitorControlGroupGizmo.controlGroup);
			}
			return false;
		}

		// Token: 0x0600C988 RID: 51592 RVA: 0x003AFB68 File Offset: 0x003ADD68
		public override void MergeWith(Gizmo gizmo)
		{
			MechanitorControlGroupGizmo mechanitorControlGroupGizmo = gizmo as MechanitorControlGroupGizmo;
			if (mechanitorControlGroupGizmo == null)
			{
				Log.ErrorOnce("Tried to merge MechanitorControlGroupGizmo with unexpected type", 345234235);
				return;
			}
			if (mechanitorControlGroupGizmo.controlGroup != this.controlGroup)
			{
				if (this.mergedControlGroups == null)
				{
					this.mergedControlGroups = new List<MechanitorControlGroup>();
				}
				if (!this.mergedControlGroups.Contains(mechanitorControlGroupGizmo.controlGroup))
				{
					this.mergedControlGroups.Add(mechanitorControlGroupGizmo.controlGroup);
				}
			}
		}

		// Token: 0x0600C989 RID: 51593 RVA: 0x003AFBD4 File Offset: 0x003ADDD4
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
		{
			if (!ModLister.CheckBiotech("Mechanitor control group gizmo"))
			{
				return new GizmoResult(GizmoState.Clear);
			}
			AcceptanceReport canControlMechs = this.controlGroup.Tracker.CanControlMechs;
			this.disabled = !canControlMechs;
			this.disabledReason = canControlMechs.Reason;
			Rect rect = new Rect(topLeft.x, topLeft.y, this.GetWidth(maxWidth), 75f);
			Rect rect2 = rect.ContractedBy(6f);
			bool flag = Mouse.IsOver(rect2);
			List<Pawn> mechsForReading = this.controlGroup.MechsForReading;
			Color white = Color.white;
			Material material = (this.disabled || parms.lowLight || mechsForReading.Count <= 0) ? TexUI.GrayscaleGUI : null;
			GUI.color = (parms.lowLight ? Command.LowLightBgColor : white);
			GenUI.DrawTextureWithMaterial(rect, parms.shrunk ? Command.BGTexShrunk : Command.BGTex, material, default(Rect));
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect3 = rect2;
			TaggedString taggedString = (!this.mergedControlGroups.NullOrEmpty<MechanitorControlGroup>()) ? "Groups".Translate() : "Group".Translate();
			taggedString += " " + this.controlGroup.Index.ToString();
			if (!this.mergedControlGroups.NullOrEmpty<MechanitorControlGroup>())
			{
				this.mergedControlGroups.SortBy((MechanitorControlGroup c) => c.Index);
				for (int i = 0; i < this.mergedControlGroups.Count; i++)
				{
					taggedString += ", " + this.mergedControlGroups[i].Index.ToString();
				}
			}
			taggedString = taggedString.Truncate(rect2.width, null);
			Vector2 vector = Text.CalcSize(taggedString);
			rect3.width = vector.x;
			rect3.height = vector.y;
			Widgets.Label(rect3, taggedString);
			if (mechsForReading.Count <= 0)
			{
				GUI.color = ColoredText.SubtleGrayColor;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect2, "(" + "NoMechs".Translate() + ")");
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return new GizmoResult(GizmoState.Clear);
			}
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				if (Widgets.ButtonInvisible(rect3, true))
				{
					Find.Selector.ClearSelection();
					for (int j = 0; j < mechsForReading.Count; j++)
					{
						Find.Selector.Select(mechsForReading[j], true, true);
					}
				}
			}
			bool flag2 = false;
			Rect rect4 = new Rect(rect.x + rect.width - 26f - 6f, rect.y + 6f, 26f, 26f);
			Widgets.DrawTextureFitted(rect4, MechanitorControlGroupGizmo.PowerIcon.Texture, 1f, 1f);
			if (!this.disabled && Mouse.IsOver(rect4))
			{
				flag2 = true;
				Widgets.DrawHighlight(rect4);
				if (Widgets.ButtonInvisible(rect4, true))
				{
					Find.WindowStack.Add(new Dialog_RechargeSettings(this.controlGroup));
				}
			}
			bool flag3 = false;
			Rect rect5 = new Rect(rect.x + rect.width - 52f - 6f, rect.y + 6f, 26f, 26f);
			Widgets.DrawTextureFitted(rect5, this.controlGroup.WorkMode.uiIcon, 1f, 1f);
			if (!this.disabled && Mouse.IsOver(rect5))
			{
				flag3 = true;
				Widgets.DrawHighlight(rect5);
			}
			Rect rect6 = new Rect(rect2.x, rect2.y + 26f + 4f, rect2.width, rect2.height - 26f - 4f);
			float num = rect6.height;
			int num2 = 0;
			int num3 = 0;
			for (float num4 = num; num4 >= 0f; num4 -= 1f)
			{
				num2 = Mathf.FloorToInt(rect6.width / num4);
				num3 = Mathf.FloorToInt(rect6.height / num4);
				if (num2 * num3 >= mechsForReading.Count)
				{
					num = num4;
					break;
				}
			}
			float num5 = (rect6.width - (float)num2 * num) / 2f;
			float num6 = (rect6.height - (float)num3 * num) / 2f;
			int num7 = 0;
			for (int k = 0; k < num2; k++)
			{
				int num8 = 0;
				while (num8 < num2 && num7 < mechsForReading.Count)
				{
					Rect rect7 = new Rect(rect6.x + (float)num8 * num + num5, rect6.y + (float)k * num + num6, num, num);
					Pawn pawn = mechsForReading[num7];
					Vector2 size = rect7.size;
					Rot4 east = Rot4.East;
					float controlGroupPortraitZoom = mechsForReading[num7].kindDef.controlGroupPortraitZoom;
					RenderTexture image = PortraitsCache.Get(pawn, size, east, default(Vector3), controlGroupPortraitZoom, true, true, true, true, null, null, false, null);
					if (!this.controlGroup.Tracker.ControlledPawns.Contains(mechsForReading[num7]))
					{
						Widgets.DrawRectFast(rect7, MechanitorControlGroupGizmo.UncontrolledMechBackgroundColor, null);
					}
					GUI.DrawTexture(rect7, image);
					if (Mouse.IsOver(rect7))
					{
						Widgets.DrawHighlight(rect7);
						MouseoverSounds.DoRegion(rect7, SoundDefOf.Mouseover_Command);
						if (Event.current.type == EventType.MouseDown)
						{
							if (Event.current.shift)
							{
								Find.Selector.Select(mechsForReading[num7], true, true);
							}
							else
							{
								CameraJumper.TryJumpAndSelect(mechsForReading[num7], CameraJumper.MovementMode.Pan);
							}
						}
						TargetHighlighter.Highlight(mechsForReading[num7], true, false, false);
					}
					if (Find.Selector.IsSelected(mechsForReading[num7]))
					{
						SelectionDrawerUtility.DrawSelectionOverlayOnGUI(mechsForReading[num7], rect7, 0.8f / (float)num2, 20f);
					}
					num7++;
					num8++;
				}
				if (num7 >= mechsForReading.Count)
				{
					break;
				}
			}
			if (Find.WindowStack.FloatMenu == null && !flag2)
			{
				TooltipHandler.TipRegion(rect, delegate()
				{
					string text = ("ControlGroup".Translate() + " #" + this.controlGroup.Index.ToString()).Colorize(ColoredText.TipSectionTitleColor) + "\n\n";
					text = string.Concat(new string[]
					{
						text,
						("CurrentMechWorkMode".Translate() + ": " + this.controlGroup.WorkMode.LabelCap).Colorize(ColoredText.TipSectionTitleColor),
						"\n",
						this.controlGroup.WorkMode.description,
						"\n\n"
					});
					IEnumerable<string> entries = from m in this.controlGroup.MechsForReading.Where(delegate(Pawn x)
					{
						Pawn_NeedsTracker needs = x.needs;
						return ((needs != null) ? needs.energy : null) != null;
					})
					select (m.LabelCap + " (" + m.needs.energy.CurLevelPercentage.ToStringPercent() + " " + "EnergyLower".Translate() + ")").Resolve();
					text = text + "AssignedMechs".Translate().Colorize(ColoredText.TipSectionTitleColor) + "\n" + entries.ToLineList(" - ", false);
					if (this.disabled && !this.disabledReason.NullOrEmpty())
					{
						text += ("\n\n" + "DisabledCommand".Translate() + ": " + this.disabledReason).Colorize(ColorLibrary.RedReadable);
					}
					return text;
				}, 2545872);
			}
			if (flag3 && Event.current.type == EventType.MouseDown)
			{
				return new GizmoResult(GizmoState.OpenedFloatMenu, Event.current);
			}
			return new GizmoResult(flag ? GizmoState.Mouseover : GizmoState.Clear);
		}

		// Token: 0x0600C98A RID: 51594 RVA: 0x003B0262 File Offset: 0x003AE462
		public override float GetWidth(float maxWidth)
		{
			return 130f;
		}

		// Token: 0x0400893D RID: 35133
		public const int InRectPadding = 6;

		// Token: 0x0400893E RID: 35134
		private const float Width = 130f;

		// Token: 0x0400893F RID: 35135
		private const int IconButtonSize = 26;

		// Token: 0x04008940 RID: 35136
		private const float BaseSelectedTexJump = 20f;

		// Token: 0x04008941 RID: 35137
		private const float BaseSelectedTextScale = 0.8f;

		// Token: 0x04008942 RID: 35138
		private static readonly CachedTexture PowerIcon = new CachedTexture("UI/Icons/MechRechargeSettings");

		// Token: 0x04008943 RID: 35139
		private static readonly Color UncontrolledMechBackgroundColor = new Color32(byte.MaxValue, 25, 25, 55);

		// Token: 0x04008944 RID: 35140
		private MechanitorControlGroup controlGroup;

		// Token: 0x04008945 RID: 35141
		private List<MechanitorControlGroup> mergedControlGroups;
	}
}
