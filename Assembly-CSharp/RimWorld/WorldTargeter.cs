using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020034F8 RID: 13560
	[StaticConstructorOnStartup]
	public class WorldTargeter
	{
		// Token: 0x170031A2 RID: 12706
		// (get) Token: 0x060130DA RID: 78042 RVA: 0x005A63DD File Offset: 0x005A45DD
		public bool IsTargeting
		{
			get
			{
				return this.action != null;
			}
		}

		// Token: 0x170031A3 RID: 12707
		// (get) Token: 0x060130DB RID: 78043 RVA: 0x005A63E8 File Offset: 0x005A45E8
		public PlanetTile ClosestLayerTile
		{
			get
			{
				return this.closestLayerTile;
			}
		}

		// Token: 0x060130DC RID: 78044 RVA: 0x005A63F0 File Offset: 0x005A45F0
		public void BeginTargeting(Func<GlobalTargetInfo, bool> action, bool canTargetTiles, Texture2D mouseAttachment = null, bool closeWorldTabWhenFinished = false, Action onUpdate = null, Func<GlobalTargetInfo, TaggedString> extraLabelGetter = null, Func<GlobalTargetInfo, bool> canSelectTarget = null, PlanetTile? originForClosest = null, bool showCancelButton = false)
		{
			this.action = action;
			this.canTargetTiles = canTargetTiles;
			this.mouseAttachment = mouseAttachment;
			this.closeWorldTabWhenFinished = closeWorldTabWhenFinished;
			this.onUpdate = onUpdate;
			this.extraLabelGetter = extraLabelGetter;
			this.canSelectTarget = canSelectTarget;
			this.originForClosest = (originForClosest ?? PlanetTile.Invalid);
			this.showCancelButton = showCancelButton;
		}

		// Token: 0x060130DD RID: 78045 RVA: 0x005A645C File Offset: 0x005A465C
		public void StopTargeting()
		{
			if (this.closeWorldTabWhenFinished)
			{
				CameraJumper.TryHideWorld();
			}
			this.action = null;
			this.canTargetTiles = false;
			this.mouseAttachment = null;
			this.closeWorldTabWhenFinished = false;
			this.onUpdate = null;
			this.extraLabelGetter = null;
			this.originForClosest = PlanetTile.Invalid;
			this.closestLayerTile = PlanetTile.Invalid;
			this.showCancelButton = false;
			this.cachedLayer = null;
			this.cachedOrigin = PlanetTile.Invalid;
			this.cachedClosest = PlanetTile.Invalid;
		}

		// Token: 0x060130DE RID: 78046 RVA: 0x005A64DC File Offset: 0x005A46DC
		public void ProcessInputEvents()
		{
			if (Event.current.type == EventType.MouseDown)
			{
				if (Event.current.button == 0 && this.IsTargeting)
				{
					GlobalTargetInfo arg = this.CurrentTargetUnderMouse();
					if ((this.canSelectTarget == null || this.canSelectTarget(arg)) && this.action(arg))
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.StopTargeting();
					}
					Event.current.Use();
				}
				if (Event.current.button == 1 && this.IsTargeting)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsTargeting)
			{
				SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
				this.StopTargeting();
				Event.current.Use();
			}
		}

		// Token: 0x060130DF RID: 78047 RVA: 0x005A65B4 File Offset: 0x005A47B4
		public void TargeterOnGUI()
		{
			if (!this.IsTargeting || Mouse.IsInputBlockedNow)
			{
				return;
			}
			Vector2 mousePosition = Event.current.mousePosition;
			Texture2D image = this.mouseAttachment ? this.mouseAttachment : TexCommand.Attack;
			Rect position = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			GUI.DrawTexture(position, image);
			if (this.originForClosest.Valid && this.originForClosest.Layer != PlanetLayer.Selected)
			{
				if (this.cachedLayer != PlanetLayer.Selected || this.cachedOrigin != this.originForClosest)
				{
					this.cachedLayer = PlanetLayer.Selected;
					this.cachedOrigin = this.originForClosest;
					this.closestLayerTile = (this.cachedClosest = PlanetLayer.Selected.GetClosestTile(this.originForClosest));
				}
				else
				{
					this.closestLayerTile = this.cachedClosest;
				}
			}
			else
			{
				this.closestLayerTile = PlanetTile.Invalid;
			}
			if (this.extraLabelGetter == null)
			{
				return;
			}
			GUI.color = Color.white;
			TaggedString taggedString = this.extraLabelGetter(this.CurrentTargetUnderMouse());
			if (!taggedString.NullOrEmpty())
			{
				Color color = GUI.color;
				GUI.color = Color.white;
				Rect rect = new Rect(position.xMax, position.y, 9999f, 100f);
				Vector2 vector = Text.CalcSize(taggedString);
				GUI.DrawTexture(new Rect(rect.x - vector.x * 0.1f, rect.y, vector.x * 1.2f, vector.y), TexUI.GrayTextBG);
				GUI.color = color;
				Widgets.Label(rect, taggedString);
			}
			GUI.color = Color.white;
			if (this.showCancelButton)
			{
				Rect rect2 = new Rect((float)UI.screenWidth / 2f - WorldTargeter.ButtonSize.x / 2f - 8f, (float)UI.screenHeight - (WorldTargeter.ButtonSize.y + 8f) + -50f, WorldTargeter.ButtonSize.x + 16f, WorldTargeter.ButtonSize.y + 16f);
				Rect rect3 = rect2.ContractedBy(8f);
				Widgets.DrawWindowBackground(rect2);
				if (Widgets.ButtonText(rect3, "Cancel".Translate(), true, true, true, null))
				{
					SoundDefOf.Click.PlayOneShotOnCamera(null);
					this.StopTargeting();
				}
			}
		}

		// Token: 0x060130E0 RID: 78048 RVA: 0x005A6834 File Offset: 0x005A4A34
		public void TargeterUpdate()
		{
			if (!this.IsTargeting)
			{
				return;
			}
			Vector3 pos = Vector3.zero;
			GlobalTargetInfo arg = this.CurrentTargetUnderMouse();
			if (arg.HasWorldObject)
			{
				pos = arg.WorldObject.DrawPos;
			}
			else if (arg.Tile.Valid)
			{
				pos = Find.WorldGrid.GetTileCenter(arg.Tile);
			}
			if (arg.IsValid && !Mouse.IsInputBlockedNow && (this.canSelectTarget == null || this.canSelectTarget(arg)))
			{
				WorldRendererUtility.DrawQuadTangentialToPlanet(pos, 0.8f * Find.WorldGrid.AverageTileSize, 0.05f, WorldMaterials.CurTargetingMat, 0f, false, false, null);
			}
			if (this.onUpdate != null)
			{
				this.onUpdate();
			}
		}

		// Token: 0x060130E1 RID: 78049 RVA: 0x005A68F3 File Offset: 0x005A4AF3
		public bool IsTargetedNow(WorldObject o, List<WorldObject> worldObjectsUnderMouse = null)
		{
			if (!this.IsTargeting)
			{
				return false;
			}
			if (worldObjectsUnderMouse == null)
			{
				worldObjectsUnderMouse = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			}
			return worldObjectsUnderMouse.Any<WorldObject>() && o == worldObjectsUnderMouse[0];
		}

		// Token: 0x060130E2 RID: 78050 RVA: 0x005A6924 File Offset: 0x005A4B24
		private GlobalTargetInfo CurrentTargetUnderMouse()
		{
			if (!this.IsTargeting)
			{
				return GlobalTargetInfo.Invalid;
			}
			List<WorldObject> list = GenWorldUI.WorldObjectsUnderMouse(UI.MousePositionOnUI);
			if (list.Any<WorldObject>())
			{
				return list[0];
			}
			if (!this.canTargetTiles)
			{
				return GlobalTargetInfo.Invalid;
			}
			PlanetTile tile = GenWorld.MouseTile(false);
			if (tile.Valid)
			{
				return new GlobalTargetInfo(tile);
			}
			return GlobalTargetInfo.Invalid;
		}

		// Token: 0x0400C580 RID: 50560
		private Func<GlobalTargetInfo, bool> action;

		// Token: 0x0400C581 RID: 50561
		private bool canTargetTiles;

		// Token: 0x0400C582 RID: 50562
		private bool showCancelButton;

		// Token: 0x0400C583 RID: 50563
		private Texture2D mouseAttachment;

		// Token: 0x0400C584 RID: 50564
		public bool closeWorldTabWhenFinished;

		// Token: 0x0400C585 RID: 50565
		private PlanetTile originForClosest;

		// Token: 0x0400C586 RID: 50566
		private Action onUpdate;

		// Token: 0x0400C587 RID: 50567
		private Func<GlobalTargetInfo, TaggedString> extraLabelGetter;

		// Token: 0x0400C588 RID: 50568
		private Func<GlobalTargetInfo, bool> canSelectTarget;

		// Token: 0x0400C589 RID: 50569
		private PlanetTile closestLayerTile = PlanetTile.Invalid;

		// Token: 0x0400C58A RID: 50570
		private const float BaseFeedbackTexSize = 0.8f;

		// Token: 0x0400C58B RID: 50571
		private static readonly Vector2 ButtonSize = new Vector2(150f, 38f);

		// Token: 0x0400C58C RID: 50572
		private const int BottomPanelYOffset = -50;

		// Token: 0x0400C58D RID: 50573
		private const int Padding = 8;

		// Token: 0x0400C58E RID: 50574
		private PlanetLayer cachedLayer;

		// Token: 0x0400C58F RID: 50575
		private PlanetTile cachedOrigin;

		// Token: 0x0400C590 RID: 50576
		private PlanetTile cachedClosest;
	}
}
