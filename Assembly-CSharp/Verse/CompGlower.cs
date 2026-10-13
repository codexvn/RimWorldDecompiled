using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000413 RID: 1043
	public class CompGlower : ThingComp
	{
		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06001A63 RID: 6755 RVA: 0x0008583F File Offset: 0x00083A3F
		public CompProperties_Glower Props
		{
			get
			{
				return (CompProperties_Glower)this.props;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06001A64 RID: 6756 RVA: 0x0008584C File Offset: 0x00083A4C
		// (set) Token: 0x06001A65 RID: 6757 RVA: 0x0008587C File Offset: 0x00083A7C
		public virtual ColorInt GlowColor
		{
			get
			{
				ColorInt? colorInt = this.glowColorOverride;
				if (colorInt == null)
				{
					return this.Props.glowColor;
				}
				return colorInt.GetValueOrDefault();
			}
			set
			{
				this.SetGlowColorInternal(new ColorInt?(value));
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001A66 RID: 6758 RVA: 0x0008588C File Offset: 0x00083A8C
		// (set) Token: 0x06001A67 RID: 6759 RVA: 0x000858BC File Offset: 0x00083ABC
		public virtual float GlowRadius
		{
			get
			{
				float? num = this.glowRadiusOverride;
				if (num == null)
				{
					return this.Props.glowRadius;
				}
				return num.GetValueOrDefault();
			}
			set
			{
				this.glowRadiusOverride = new float?(value);
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001A68 RID: 6760 RVA: 0x000858CC File Offset: 0x00083ACC
		protected virtual bool ShouldBeLitNow
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return false;
				}
				if (!FlickUtility.WantsToBeOn(this.parent))
				{
					return false;
				}
				IThingGlower thingGlower = this.parent as IThingGlower;
				if (thingGlower != null && !thingGlower.ShouldBeLitNow())
				{
					return false;
				}
				ThingWithComps parent = this.parent;
				if (parent != null)
				{
					foreach (ThingComp thingComp in parent.AllComps)
					{
						IThingGlower thingGlower2 = thingComp as IThingGlower;
						if (thingGlower2 != null && !thingGlower2.ShouldBeLitNow())
						{
							return false;
						}
					}
					return true;
				}
				return true;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001A69 RID: 6761 RVA: 0x00085974 File Offset: 0x00083B74
		public bool Glows
		{
			get
			{
				return this.glowOnInt;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001A6A RID: 6762 RVA: 0x0008597C File Offset: 0x00083B7C
		public bool HasGlowColorOverride
		{
			get
			{
				return this.glowColorOverride != null;
			}
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x0008598C File Offset: 0x00083B8C
		public void UpdateLit(Map map)
		{
			bool shouldBeLitNow = this.ShouldBeLitNow;
			if (this.glowOnInt == shouldBeLitNow)
			{
				return;
			}
			this.glowOnInt = shouldBeLitNow;
			if (!this.glowOnInt)
			{
				map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Things);
				map.glowGrid.DeRegisterGlower(this);
				return;
			}
			map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Things);
			map.glowGrid.RegisterGlower(this);
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x00085A12 File Offset: 0x00083C12
		public void ForceRegister(Map map)
		{
			map.glowGrid.DeRegisterGlower(this);
			map.mapDrawer.MapMeshDirty(this.parent.Position, MapMeshFlagDefOf.Things);
			map.glowGrid.RegisterGlower(this);
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x00085A4C File Offset: 0x00083C4C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!this.parent.BeingTransportedOnGravship)
			{
				this.RefreshGlower();
			}
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x00085A61 File Offset: 0x00083C61
		public override void PostSwapMap()
		{
			this.RefreshGlower();
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00085A61 File Offset: 0x00083C61
		public override void PostMapInit()
		{
			this.RefreshGlower();
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x00085A69 File Offset: 0x00083C69
		private void RefreshGlower()
		{
			this.UpdateLit(this.parent.Map);
			if (this.ShouldBeLitNow)
			{
				this.parent.Map.glowGrid.RegisterGlower(this);
			}
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00085A9C File Offset: 0x00083C9C
		public override void ReceiveCompSignal(string signal)
		{
			if (signal == "PowerTurnedOn" || signal == "PowerTurnedOff" || signal == "FlickedOn" || signal == "FlickedOff" || signal == "Refueled" || signal == "RanOutOfFuel" || signal == "ScheduledOn" || signal == "ScheduledOff" || signal == "MechClusterDefeated" || signal == "Hacked" || signal == "RitualTargetChanged" || signal == "CrateContentsChanged" || signal == "Vacuum")
			{
				this.UpdateLit(this.parent.Map);
			}
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00085B6C File Offset: 0x00083D6C
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.glowOnInt, "glowOn", false, false);
			Scribe_Values.Look<ColorInt?>(ref this.glowColorOverride, "glowColorOverride", null, false);
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00085BA5 File Offset: 0x00083DA5
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			this.UpdateLit(map);
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00085BB8 File Offset: 0x00083DB8
		private List<CompGlower> ExtraSelectedGlowers(CompGlower.GlowerColorChangeType type)
		{
			CompGlower.tmpExtraGlowers.Clear();
			foreach (object obj in Find.Selector.SelectedObjectsListForReading)
			{
				if (obj != this)
				{
					ThingWithComps thingWithComps = obj as ThingWithComps;
					if (thingWithComps != null)
					{
						foreach (CompGlower compGlower in thingWithComps.GetComps<CompGlower>())
						{
							if (type == CompGlower.GlowerColorChangeType.AllGlowers || (type == CompGlower.GlowerColorChangeType.ColorPickerEnabled && compGlower.Props.colorPickerEnabled) || (type == CompGlower.GlowerColorChangeType.DarklightToggle && compGlower.Props.darklightToggle))
							{
								CompGlower.tmpExtraGlowers.Add(compGlower);
							}
						}
					}
				}
			}
			return CompGlower.tmpExtraGlowers;
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00085C90 File Offset: 0x00083E90
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in base.CompGetGizmosExtra())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.parent.Faction != Faction.OfPlayer)
			{
				yield break;
			}
			bool doCopyPasteGizmos = false;
			CompGlower.GlowerColorChangeType type = CompGlower.GlowerColorChangeType.None;
			if (this.Props.colorPickerEnabled)
			{
				type = CompGlower.GlowerColorChangeType.ColorPickerEnabled;
			}
			else if (DebugSettings.editableGlowerColors)
			{
				type = CompGlower.GlowerColorChangeType.AllGlowers;
			}
			List<CompGlower> extraGlowers = this.ExtraSelectedGlowers(type);
			Color32 value = this.GlowColor.ProjectToColor32();
			value.a = byte.MaxValue;
			Color32? color = new Color32?(value);
			using (List<CompGlower>.Enumerator enumerator2 = extraGlowers.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.GlowColor != this.GlowColor)
					{
						color = null;
					}
				}
			}
			Command_ColorIcon command_ColorIcon = new Command_ColorIcon
			{
				defaultLabel = "GlowerChangeColor".Translate(),
				defaultDesc = "GlowerChangeColorDescription".Translate(),
				icon = ContentFinder<Texture2D>.Get("UI/Commands/ChangeColor", true),
				color = color,
				action = delegate()
				{
					Widgets.ColorComponents visibleTextfields = DebugSettings.editableGlowerColors ? CompGlower.visibleDebugColorTextfields : CompGlower.visibleColorTextfields;
					Widgets.ColorComponents editableTextfields = DebugSettings.editableGlowerColors ? CompGlower.editableDebugColorTextfields : CompGlower.editableColorTextfields;
					Dialog_GlowerColorPicker window = new Dialog_GlowerColorPicker(this, extraGlowers, visibleTextfields, editableTextfields);
					Find.WindowStack.Add(window);
				}
			};
			if (this.Props.colorPickerEnabled)
			{
				bool flag = DebugSettings.editableGlowerColors || ResearchProjectDefOf.ColoredLights.IsFinished;
				doCopyPasteGizmos = flag;
				command_ColorIcon.Disabled = !flag;
				command_ColorIcon.disabledReason = "GlowerChangeColorNeedsResearch".Translate(ResearchProjectDefOf.ColoredLights.label);
				yield return command_ColorIcon;
			}
			else if (DebugSettings.editableGlowerColors)
			{
				doCopyPasteGizmos = true;
				yield return command_ColorIcon;
			}
			if (doCopyPasteGizmos)
			{
				Command_ColorIcon command_ColorIcon2 = new Command_ColorIcon();
				command_ColorIcon2.icon = ContentFinder<Texture2D>.Get("UI/Commands/CopyColor", true);
				command_ColorIcon2.defaultLabel = "CommandCopyColorLabel".Translate();
				command_ColorIcon2.defaultDesc = "CommandCopyColorDesc".Translate();
				Color32 value2 = this.GlowColor.ProjectToColor32();
				value2.a = byte.MaxValue;
				command_ColorIcon2.color = new Color32?(value2);
				command_ColorIcon2.action = delegate()
				{
					CompGlower.ColorClipboard = new ColorInt?(this.GlowColor);
					Messages.Message("ColorCopiedSuccessfully".Translate(), MessageTypeDefOf.PositiveEvent, false);
				};
				command_ColorIcon2.hotKey = KeyBindingDefOf.Misc4;
				yield return command_ColorIcon2;
				bool flag2 = true;
				float hue = 0f;
				float sat = 0f;
				if (CompGlower.ColorClipboard != null)
				{
					float num;
					Color.RGBToHSV(CompGlower.ColorClipboard.Value.ProjectToColor32(), out hue, out sat, out num);
					flag2 = false;
				}
				Command_ColorIcon command_ColorIcon3 = new Command_ColorIcon();
				command_ColorIcon3.icon = ContentFinder<Texture2D>.Get("UI/Commands/PasteColor", true);
				command_ColorIcon3.defaultLabel = "CommandPasteColorLabel".Translate();
				command_ColorIcon3.defaultDesc = "CommandPasteColorDesc".Translate();
				if (!flag2)
				{
					command_ColorIcon3.color = new Color32?(Color.HSVToRGB(hue, sat, 1f));
				}
				command_ColorIcon3.Disabled = flag2;
				command_ColorIcon3.disabledReason = "ClipboardInvalidColor".Translate();
				command_ColorIcon3.action = delegate()
				{
					try
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						ColorInt glowColor = this.GlowColor;
						glowColor.SetHueSaturation(hue, sat);
						this.GlowColor = glowColor;
						foreach (CompGlower compGlower in extraGlowers)
						{
							glowColor = compGlower.GlowColor;
							glowColor.SetHueSaturation(hue, sat);
							compGlower.GlowColor = glowColor;
						}
						Messages.Message("ColorPastedSuccessfully".Translate(), MessageTypeDefOf.PositiveEvent, false);
					}
					catch (Exception)
					{
						Messages.Message("ClipboardInvalidColor".Translate(), MessageTypeDefOf.RejectInput, false);
					}
				};
				command_ColorIcon3.hotKey = KeyBindingDefOf.Misc5;
				yield return command_ColorIcon3;
			}
			if (ModsConfig.IdeologyActive && this.Props.darklightToggle)
			{
				bool darklight = DarklightUtility.IsDarklight(this.GlowColor.ToColor);
				yield return new Command_Action
				{
					icon = ContentFinder<Texture2D>.Get(darklight ? "UI/Commands/SetNormalLight" : "UI/Commands/SetDarklight", true),
					defaultLabel = (darklight ? "ToggleDarklightOff" : "ToggleDarklightOn").Translate(),
					defaultDesc = (darklight ? "ToggleDarklightOffDesc" : "ToggleDarklightOnDesc").Translate(),
					action = delegate()
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						if (darklight)
						{
							this.SetGlowColorInternal(null);
						}
						else
						{
							this.GlowColor = new ColorInt(DarklightUtility.DefaultDarklight);
						}
						foreach (CompGlower compGlower in this.ExtraSelectedGlowers(CompGlower.GlowerColorChangeType.DarklightToggle))
						{
							if (darklight)
							{
								compGlower.SetGlowColorInternal(null);
							}
							else
							{
								compGlower.GlowColor = new ColorInt(DarklightUtility.DefaultDarklight);
							}
						}
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00085CA0 File Offset: 0x00083EA0
		protected virtual void SetGlowColorInternal(ColorInt? color)
		{
			if (this.ShouldBeLitNow)
			{
				this.parent.MapHeld.glowGrid.DeRegisterGlower(this);
			}
			this.glowColorOverride = color;
			if (this.ShouldBeLitNow)
			{
				this.parent.MapHeld.glowGrid.RegisterGlower(this);
			}
		}

		// Token: 0x0400174C RID: 5964
		public static Widgets.ColorComponents visibleColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

		// Token: 0x0400174D RID: 5965
		public static Widgets.ColorComponents editableColorTextfields = Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

		// Token: 0x0400174E RID: 5966
		public static Widgets.ColorComponents visibleDebugColorTextfields = Widgets.ColorComponents.All;

		// Token: 0x0400174F RID: 5967
		public static Widgets.ColorComponents editableDebugColorTextfields = Widgets.ColorComponents.Red | Widgets.ColorComponents.Green | Widgets.ColorComponents.Blue | Widgets.ColorComponents.Hue | Widgets.ColorComponents.Sat;

		// Token: 0x04001750 RID: 5968
		public static ColorInt? ColorClipboard;

		// Token: 0x04001751 RID: 5969
		private bool glowOnInt;

		// Token: 0x04001752 RID: 5970
		private ColorInt? glowColorOverride;

		// Token: 0x04001753 RID: 5971
		private float? glowRadiusOverride;

		// Token: 0x04001754 RID: 5972
		private static List<CompGlower> tmpExtraGlowers = new List<CompGlower>(64);

		// Token: 0x02000414 RID: 1044
		private enum GlowerColorChangeType
		{
			// Token: 0x04001756 RID: 5974
			None,
			// Token: 0x04001757 RID: 5975
			ColorPickerEnabled,
			// Token: 0x04001758 RID: 5976
			DarklightToggle,
			// Token: 0x04001759 RID: 5977
			AllGlowers
		}
	}
}
