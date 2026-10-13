using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020030D8 RID: 12504
	public abstract class Designator_Place : DesignatorWithEyedropper
	{
		// Token: 0x17002D8C RID: 11660
		// (get) Token: 0x060118FB RID: 71931
		public abstract BuildableDef PlacingDef { get; }

		// Token: 0x17002D8D RID: 11661
		// (get) Token: 0x060118FC RID: 71932
		public abstract ThingStyleDef ThingStyleDefForPreview { get; }

		// Token: 0x17002D8E RID: 11662
		// (get) Token: 0x060118FD RID: 71933
		public abstract ThingDef StuffDef { get; }

		// Token: 0x17002D8F RID: 11663
		// (get) Token: 0x060118FE RID: 71934 RVA: 0x0050B621 File Offset: 0x00509821
		public override DrawStyleCategoryDef DrawStyleCategory
		{
			get
			{
				return this.PlacingDef.drawStyleCategory;
			}
		}

		// Token: 0x060118FF RID: 71935 RVA: 0x0050B62E File Offset: 0x0050982E
		public Designator_Place()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragBuilding;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_PlaceBuilding;
		}

		// Token: 0x06011900 RID: 71936 RVA: 0x0050B660 File Offset: 0x00509860
		public override void DrawMouseAttachments()
		{
			base.DrawMouseAttachments();
			Map currentMap = Find.CurrentMap;
			currentMap.deepResourceGrid.DrawPlacingMouseAttachments(this.PlacingDef);
			Vector2 vector = Event.current.mousePosition + Designator_Place.PlaceMouseAttachmentDrawOffset;
			float x = vector.x;
			float y = vector.y;
			this.DrawPlaceMouseAttachments(x, ref y);
			if (this.PlacingDef.PlaceWorkers != null)
			{
				foreach (PlaceWorker placeWorker in this.PlacingDef.PlaceWorkers)
				{
					placeWorker.DrawPlaceMouseAttachments(x, ref y, this.PlacingDef, UI.MouseCell(), this.placingRot);
				}
				foreach (PlaceWorker placeWorker2 in this.PlacingDef.PlaceWorkers)
				{
					placeWorker2.DrawMouseAttachments(this.PlacingDef);
				}
			}
			if (currentMap != null)
			{
				ThingDef thingDef = this.PlacingDef as ThingDef;
				if (thingDef != null && thingDef.displayNumbersBetweenSameDefDistRange.max > 0f)
				{
					IntVec3 intVec = UI.MouseCell();
					Designator_Place.tmpThings.Clear();
					Designator_Place.tmpThings.AddRange(currentMap.listerThings.ThingsOfDef(thingDef));
					Designator_Place.tmpThings.AddRange(currentMap.listerThings.ThingsInGroup(ThingRequestGroup.Blueprint));
					foreach (Thing thing in Designator_Place.tmpThings)
					{
						if ((thing.def == thingDef || thing.def.entityDefToBuild == thingDef) && (thing.Position.x == intVec.x || thing.Position.z == intVec.z) && this.CanDrawNumbersBetween(thing, thingDef, intVec, thing.Position, currentMap))
						{
							IntVec3 intVec2 = thing.Position - intVec;
							intVec2.x = Mathf.Abs(intVec2.x) + 1;
							intVec2.z = Mathf.Abs(intVec2.z) + 1;
							if (intVec2.x >= 3)
							{
								Vector2 screenPos = (thing.Position.ToUIPosition() + intVec.ToUIPosition()) / 2f;
								screenPos.y = thing.Position.ToUIPosition().y;
								Color textColor = thingDef.displayNumbersBetweenSameDefDistRange.Includes((float)intVec2.x) ? Color.white : Color.red;
								Widgets.DrawNumberOnMap(screenPos, intVec2.x, textColor);
							}
							if (intVec2.z >= 3)
							{
								Vector2 screenPos2 = (thing.Position.ToUIPosition() + intVec.ToUIPosition()) / 2f;
								screenPos2.x = thing.Position.ToUIPosition().x;
								Color textColor2 = thingDef.displayNumbersBetweenSameDefDistRange.Includes((float)intVec2.z) ? Color.white : Color.red;
								Widgets.DrawNumberOnMap(screenPos2, intVec2.z, textColor2);
							}
						}
					}
					Designator_Place.tmpThings.Clear();
					return;
				}
			}
		}

		// Token: 0x06011901 RID: 71937 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void DrawPlaceMouseAttachments(float curX, ref float curY)
		{
		}

		// Token: 0x06011902 RID: 71938 RVA: 0x0050B9DC File Offset: 0x00509BDC
		protected virtual bool CanDrawNumbersBetween(Thing thing, ThingDef def, IntVec3 a, IntVec3 b, Map map)
		{
			return !GenThing.CloserThingBetween(def, a, b, map, null);
		}

		// Token: 0x06011903 RID: 71939 RVA: 0x0050B9F0 File Offset: 0x00509BF0
		public override void DoExtraGuiControls(float leftX, float bottomY)
		{
			if (this.PlacingDef.PlaceWorkers != null)
			{
				foreach (PlaceWorker placeWorker in this.PlacingDef.PlaceWorkers)
				{
					placeWorker.DrawOnGUIExtra(this.PlacingDef);
				}
			}
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				DesignatorUtility.GUIDoRotationControls(leftX, bottomY, this.placingRot, delegate(Rot4 rot)
				{
					this.placingRot = rot;
				});
				return;
			}
			base.DoExtraGuiControls(leftX, bottomY);
		}

		// Token: 0x06011904 RID: 71940 RVA: 0x0050BA94 File Offset: 0x00509C94
		public override void SelectedProcessInput(Event ev)
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.rotatable)
			{
				this.HandleRotationShortcuts();
				return;
			}
			base.SelectedProcessInput(ev);
		}

		// Token: 0x06011905 RID: 71941 RVA: 0x0050BAC8 File Offset: 0x00509CC8
		public override void SelectedUpdate()
		{
			if (!base.Map.IsPocketMap)
			{
				GenDraw.DrawNoBuildEdgeLines();
			}
			IntVec3 intVec = UI.MouseCell();
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted) && intVec.InBounds(base.Map))
			{
				if (this.PlacingDef is TerrainDef)
				{
					GenUI.RenderMouseoverBracket();
					this.DrawPlaceWorkers();
					return;
				}
				this.DrawBeforeGhost();
				Color ghostCol;
				if (this.CanDesignateCell(intVec).Accepted)
				{
					ghostCol = Designator_Place.CanPlaceColor;
				}
				else
				{
					ghostCol = Designator_Place.CannotPlaceColor;
				}
				this.DrawGhost(ghostCol);
				if (this.CanDesignateCell(intVec).Accepted && this.PlacingDef.specialDisplayRadius > 0.01f)
				{
					GenDraw.DrawRadiusRing(intVec, this.PlacingDef.specialDisplayRadius);
				}
				GenDraw.DrawInteractionCells((ThingDef)this.PlacingDef, intVec, this.placingRot);
				ThingDef thingDef = this.PlacingDef as ThingDef;
				if (thingDef != null && thingDef.building != null && thingDef.building.isAttachment && GenConstruct.GetWallAttachedTo(intVec, this.placingRot, Find.CurrentMap) == null && this.HasPotentialAttachment(intVec))
				{
					this.HandleRotation(RotationDirection.Clockwise);
				}
			}
		}

		// Token: 0x06011906 RID: 71942 RVA: 0x0050BBF0 File Offset: 0x00509DF0
		protected virtual void DrawBeforeGhost()
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null)
			{
				MeditationUtility.DrawMeditationFociAffectedByBuildingOverlay(base.Map, thingDef, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
				GauranlenUtility.DrawConnectionsAffectedByBuildingOverlay(base.Map, thingDef, Faction.OfPlayer, UI.MouseCell(), this.placingRot);
				PsychicRitualUtility.DrawPsychicRitualSpotsAffectedByThingOverlay(base.Map, thingDef, UI.MouseCell(), this.placingRot);
			}
		}

		// Token: 0x06011907 RID: 71943 RVA: 0x0050BC5C File Offset: 0x00509E5C
		protected virtual void DrawGhost(Color ghostCol)
		{
			IntVec3 center = UI.MouseCell();
			Rot4 rot = this.placingRot;
			ThingDef thingDef = (ThingDef)this.PlacingDef;
			ThingStyleDef thingStyleDefForPreview = this.ThingStyleDefForPreview;
			GhostDrawer.DrawGhostThing(center, rot, thingDef, (thingStyleDefForPreview != null) ? thingStyleDefForPreview.Graphic : null, ghostCol, AltitudeLayer.Blueprint, null, true, this.StuffDef);
		}

		// Token: 0x06011908 RID: 71944 RVA: 0x0050BCA4 File Offset: 0x00509EA4
		protected virtual void DrawPlaceWorkers()
		{
			if (this.PlacingDef.PlaceWorkers != null)
			{
				foreach (PlaceWorker placeWorker in this.PlacingDef.PlaceWorkers)
				{
					placeWorker.DrawGhost(null, UI.MouseCell(), this.placingRot, Designator_Place.CanPlaceColor, null);
				}
			}
		}

		// Token: 0x06011909 RID: 71945 RVA: 0x0050BD18 File Offset: 0x00509F18
		private void HandleRotationShortcuts()
		{
			RotationDirection rotationDirection = RotationDirection.None;
			if (Event.current.button == 2)
			{
				if (Event.current.type == EventType.MouseDown)
				{
					Event.current.Use();
					Designator_Place.middleMouseDownTime = Time.realtimeSinceStartup;
				}
				if (Event.current.type == EventType.MouseUp && Time.realtimeSinceStartup - Designator_Place.middleMouseDownTime < 0.15f)
				{
					rotationDirection = RotationDirection.Clockwise;
				}
			}
			if (KeyBindingDefOf.Designator_RotateRight.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Clockwise;
			}
			if (KeyBindingDefOf.Designator_RotateLeft.KeyDownEvent)
			{
				rotationDirection = RotationDirection.Counterclockwise;
			}
			if (rotationDirection != RotationDirection.None)
			{
				this.HandleRotation(rotationDirection);
			}
		}

		// Token: 0x0601190A RID: 71946 RVA: 0x0050BD9C File Offset: 0x00509F9C
		private void HandleRotation(RotationDirection dir)
		{
			IntVec3 intVec = UI.MouseCell();
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef != null && thingDef.building.isAttachment && this.HasPotentialAttachment(intVec))
			{
				this.placingRot.Rotate(dir);
				while (GenConstruct.GetWallAttachedTo(intVec, this.placingRot, Find.CurrentMap) == null)
				{
					this.placingRot.Rotate(dir);
				}
				return;
			}
			SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			this.placingRot.Rotate(dir);
		}

		// Token: 0x0601190B RID: 71947 RVA: 0x0050BE1C File Offset: 0x0050A01C
		private bool HasPotentialAttachment(IntVec3 cell)
		{
			ThingDef thingDef = this.PlacingDef as ThingDef;
			if (thingDef == null || !thingDef.building.isAttachment)
			{
				return false;
			}
			foreach (Rot4 rot in Rot4.AllRotations)
			{
				if (GenConstruct.GetWallAttachedTo(cell, rot, Find.CurrentMap) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601190C RID: 71948 RVA: 0x0050BE94 File Offset: 0x0050A094
		public override void Selected()
		{
			this.placingRot = this.PlacingDef.defaultPlacingRot;
		}

		// Token: 0x0400B527 RID: 46375
		protected Rot4 placingRot = Rot4.North;

		// Token: 0x0400B528 RID: 46376
		protected static float middleMouseDownTime;

		// Token: 0x0400B529 RID: 46377
		public static readonly Color CanPlaceColor = new Color(0.5f, 1f, 0.6f, 0.4f);

		// Token: 0x0400B52A RID: 46378
		public static readonly Color CannotPlaceColor = new Color(1f, 0f, 0f, 0.4f);

		// Token: 0x0400B52B RID: 46379
		public static readonly Vector2 PlaceMouseAttachmentDrawOffset = new Vector2(19f, 17f);

		// Token: 0x0400B52C RID: 46380
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
