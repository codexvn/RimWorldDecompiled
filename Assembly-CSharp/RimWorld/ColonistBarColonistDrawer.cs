using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200306C RID: 12396
	[StaticConstructorOnStartup]
	public class ColonistBarColonistDrawer
	{
		// Token: 0x17002D0E RID: 11534
		// (get) Token: 0x0601167B RID: 71291 RVA: 0x004FFA1B File Offset: 0x004FDC1B
		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		// Token: 0x0601167C RID: 71292 RVA: 0x004FFA24 File Offset: 0x004FDC24
		public void DrawColonist(Rect rect, Pawn colonist, Map pawnMap, bool highlight, bool reordering)
		{
			float num = this.ColonistBar.GetEntryRectAlpha(rect);
			object obj;
			if (Prefs.VisibleMood)
			{
				Pawn_NeedsTracker needs = colonist.needs;
				if (((needs != null) ? needs.mood : null) != null && colonist.mindState.mentalBreaker.CanDoRandomMentalBreaks && !colonist.Dead)
				{
					obj = !colonist.Downed;
					goto IL_4E;
				}
			}
			obj = 0;
			IL_4E:
			MoodThreshold moodThreshold = MoodThresholdExtensions.CurrentMoodThresholdFor(colonist);
			Color color = moodThreshold.GetColor();
			color.a *= num;
			this.ApplyEntryInAnotherMapAlphaFactor(pawnMap, ref num);
			if (reordering)
			{
				num *= 0.5f;
			}
			Color color2 = new Color(1f, 1f, 1f, num);
			GUI.color = color2;
			object obj2 = obj;
			if (obj2 != null && num >= 1f)
			{
				float num2 = moodThreshold.EdgeExpansion();
				if (num2 > 0f)
				{
					GUI.color = color;
					Widgets.DrawAtlas(rect.ExpandedBy(num2), ColonistBarColonistDrawer.MoodAtlas);
					GUI.color = color2;
				}
			}
			GUI.DrawTexture(rect, ColonistBar.BGTex);
			if (colonist.needs != null && colonist.needs.mood != null)
			{
				Rect position = rect.ContractedBy(2f);
				float num3 = position.height * colonist.needs.mood.CurLevelPercentage;
				position.yMin = position.yMax - num3;
				position.height = num3;
				GUI.DrawTexture(position, ColonistBarColonistDrawer.MoodBGTex);
			}
			if (obj2 != null && num >= 1f)
			{
				float transparency = (moodThreshold < MoodThreshold.Major) ? 0.1f : 0.15f;
				Widgets.DrawBoxSolid(rect, moodThreshold.GetColor().ToTransparent(transparency));
			}
			if (highlight)
			{
				int thickness = (rect.width <= 22f) ? 2 : 3;
				GUI.color = Color.white;
				Widgets.DrawBox(rect, thickness, null);
				GUI.color = color2;
			}
			Rect rect2 = rect.ContractedBy(-2f * this.ColonistBar.Scale);
			if ((colonist.Dead ? Find.Selector.SelectedObjects.Contains(colonist.Corpse) : Find.Selector.SelectedObjects.Contains(colonist)) && !WorldRendererUtility.WorldSelected)
			{
				this.DrawSelectionOverlayOnGUI(colonist, rect2);
			}
			else if (WorldRendererUtility.WorldSelected && colonist.IsCaravanMember() && Find.WorldSelector.IsSelected(colonist.GetCaravan()))
			{
				this.DrawCaravanSelectionOverlayOnGUI(colonist.GetCaravan(), rect2);
			}
			GUI.DrawTexture(this.GetPawnTextureRect(rect.position), PortraitsCache.Get(colonist, ColonistBarColonistDrawer.PawnTextureSize, Rot4.South, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true, true, true, null, null, false, null));
			if (obj2 != null)
			{
				Rect rect3 = rect.ContractedBy(1f);
				Widgets.BeginGroup(rect3);
				Rect position2 = rect3.AtZero();
				position2.yMin = position2.yMax - 35f;
				GUI.color = color;
				GUI.DrawTexture(position2, ColonistBarColonistDrawer.MoodGradient);
				GUI.color = color2;
				Widgets.EndGroup();
			}
			GUI.color = new Color(1f, 1f, 1f, num * 0.8f);
			this.DrawIcons(rect, colonist);
			GUI.color = color2;
			if (colonist.Dead)
			{
				GUI.DrawTexture(rect, ColonistBarColonistDrawer.DeadColonistTex);
			}
			float num4 = 4f * this.ColonistBar.Scale;
			Vector2 pos = new Vector2(rect.center.x, rect.yMax - num4);
			GenMapUI.DrawPawnLabel(colonist, pos, num, rect.width + this.ColonistBar.SpaceBetweenColonistsHorizontal - 2f, this.pawnLabelsCache, GameFont.Tiny, true, true);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x0601167D RID: 71293 RVA: 0x004FFD9C File Offset: 0x004FDF9C
		private Rect GroupFrameRect(int group)
		{
			float num = 99999f;
			float num2 = 0f;
			float num3 = 0f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			List<Vector2> drawLocs = this.ColonistBar.DrawLocs;
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].group == group)
				{
					num = Mathf.Min(num, drawLocs[i].x);
					num2 = Mathf.Max(num2, drawLocs[i].x + this.ColonistBar.Size.x);
					num3 = Mathf.Max(num3, drawLocs[i].y + this.ColonistBar.Size.y);
				}
			}
			return new Rect(num, 0f, num2 - num, num3 - 0f).ContractedBy(-12f * this.ColonistBar.Scale);
		}

		// Token: 0x0601167E RID: 71294 RVA: 0x004FFE88 File Offset: 0x004FE088
		public void DrawGroupFrame(int group)
		{
			Rect position = this.GroupFrameRect(group);
			Map map = this.ColonistBar.Entries.Find((ColonistBar.Entry x) => x.group == group).map;
			float num;
			if (map == null)
			{
				if (WorldRendererUtility.WorldSelected)
				{
					num = 1f;
				}
				else
				{
					num = 0.75f;
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldSelected)
			{
				num = 0.75f;
			}
			else
			{
				num = 1f;
			}
			Widgets.DrawRectFast(position, new Color(0.5f, 0.5f, 0.5f, 0.4f * num), null);
		}

		// Token: 0x0601167F RID: 71295 RVA: 0x004FFF28 File Offset: 0x004FE128
		private void ApplyEntryInAnotherMapAlphaFactor(Map map, ref float alpha)
		{
			if (map == null)
			{
				if (!WorldRendererUtility.WorldSelected)
				{
					alpha = Mathf.Min(alpha, 0.4f);
					return;
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldSelected)
			{
				alpha = Mathf.Min(alpha, 0.4f);
			}
		}

		// Token: 0x06011680 RID: 71296 RVA: 0x004FFF60 File Offset: 0x004FE160
		public void HandleClicks(Rect rect, Pawn colonist, int reorderableGroup, out bool reordering)
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2 && Mouse.IsOver(rect))
			{
				Event.current.Use();
				CameraJumper.TryJump(colonist, CameraJumper.MovementMode.Pan);
			}
			reordering = ReorderableWidget.Reorderable(reorderableGroup, rect, true, true);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06011681 RID: 71297 RVA: 0x004FFFE8 File Offset: 0x004FE1E8
		public void HandleGroupFrameClicks(int group)
		{
			Rect rect = this.GroupFrameRect(group);
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && Mouse.IsOver(rect) && !this.ColonistBar.AnyColonistOrCorpseAt(UI.MousePositionOnUIInverted))
			{
				bool worldSelected = WorldRendererUtility.WorldSelected;
				if ((!worldSelected && !Find.Selector.dragBox.IsValidAndActive) || (worldSelected && !Find.WorldSelector.dragBox.IsValidAndActive))
				{
					Find.Selector.dragBox.active = false;
					Find.WorldSelector.dragBox.active = false;
					ColonistBar.Entry entry = this.ColonistBar.Entries.Find((ColonistBar.Entry x) => x.group == group);
					Map map = entry.map;
					if (map == null)
					{
						if (WorldRendererUtility.WorldSelected)
						{
							CameraJumper.TrySelect(entry.pawn, false);
						}
						else
						{
							CameraJumper.TryJumpAndSelect(entry.pawn, CameraJumper.MovementMode.Pan);
						}
					}
					else
					{
						if (!CameraJumper.TryHideWorld() && Find.CurrentMap != map)
						{
							SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
						}
						Current.Game.CurrentMap = map;
					}
				}
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06011682 RID: 71298 RVA: 0x0050014A File Offset: 0x004FE34A
		public void Notify_RecachedEntries()
		{
			this.pawnLabelsCache.Clear();
		}

		// Token: 0x06011683 RID: 71299 RVA: 0x0050014A File Offset: 0x004FE34A
		public void ClearLabelCache()
		{
			this.pawnLabelsCache.Clear();
		}

		// Token: 0x06011684 RID: 71300 RVA: 0x00500158 File Offset: 0x004FE358
		public Rect GetPawnTextureRect(Vector2 pos)
		{
			float x = pos.x;
			float y = pos.y;
			Vector2 vector = ColonistBarColonistDrawer.PawnTextureSize * this.ColonistBar.Scale;
			return new Rect(x + 1f, y - (vector.y - this.ColonistBar.Size.y) - 1f, vector.x, vector.y).ContractedBy(1f);
		}

		// Token: 0x06011685 RID: 71301 RVA: 0x005001CC File Offset: 0x004FE3CC
		private void DrawIcons(Rect rect, Pawn colonist)
		{
			if (colonist.Dead)
			{
				return;
			}
			ColonistBarColonistDrawer.tmpIconsToDraw.Clear();
			bool flag = false;
			if (colonist.CurJob != null)
			{
				JobDef def = colonist.CurJob.def;
				if (def == JobDefOf.AttackMelee || def == JobDefOf.AttackStatic)
				{
					flag = true;
				}
				else if (def == JobDefOf.Wait_Combat)
				{
					Stance_Busy stance_Busy = colonist.stances.curStance as Stance_Busy;
					if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
					{
						flag = true;
					}
				}
			}
			if (colonist.IsFormingCaravan())
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_FormingCaravan, "ActivityIconFormingCaravan".Translate(), null));
			}
			if (colonist.InAggroMentalState)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_MentalStateAggro, colonist.MentalStateDef.LabelCap, null));
			}
			else if (colonist.InMentalState)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_MentalStateNonAggro, colonist.MentalStateDef.LabelCap, null));
			}
			else if (colonist.InBed() && colonist.CurrentBed().Medical)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_MedicalRest, "ActivityIconMedicalRest".Translate(), null));
			}
			else
			{
				if (colonist.CurJob == null || !colonist.jobs.curDriver.asleep)
				{
					if (colonist.GetCaravan() != null)
					{
						Pawn_NeedsTracker needs = colonist.needs;
						bool flag2;
						if (needs == null)
						{
							flag2 = false;
						}
						else
						{
							Need_Rest rest = needs.rest;
							bool? flag3 = (rest != null) ? new bool?(rest.Resting) : null;
							bool flag4 = true;
							flag2 = (flag3.GetValueOrDefault() == flag4 & flag3 != null);
						}
						if (flag2)
						{
							goto IL_1C5;
						}
					}
					if (colonist.CurJob != null && colonist.CurJob.def == JobDefOf.FleeAndCower)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Fleeing, "ActivityIconFleeing".Translate(), null));
						goto IL_2B4;
					}
					if (flag)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Attacking, "ActivityIconAttacking".Translate(), null));
						goto IL_2B4;
					}
					if (colonist.mindState.IsIdle && GenDate.DaysPassed >= 1)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Idle, "ActivityIconIdle".Translate(), null));
						goto IL_2B4;
					}
					goto IL_2B4;
				}
				IL_1C5:
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Sleeping, "ActivityIconSleeping".Translate(), null));
			}
			IL_2B4:
			if (colonist.IsBurning())
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Burning, "ActivityIconBurning".Translate(), null));
			}
			if (colonist.Inspired)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Inspired, colonist.InspirationDef.LabelCap, null));
			}
			if (colonist.IsSlaveOfColony)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(colonist.guest.GetIcon(), null, null));
			}
			else
			{
				bool flag5 = false;
				if (colonist.Ideo != null)
				{
					Ideo ideo = colonist.Ideo;
					Precept_Role role = ideo.GetRole(colonist);
					if (role != null)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(role.Icon, null, new Color?(ideo.Color)));
						flag5 = true;
					}
				}
				if (!flag5)
				{
					Faction faction = null;
					if (colonist.HasExtraMiniFaction(null))
					{
						faction = colonist.GetExtraMiniFaction(null);
					}
					else if (colonist.HasExtraHomeFaction(null))
					{
						faction = colonist.GetExtraHomeFaction(null);
					}
					if (faction != null)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(faction.def.FactionIcon, null, new Color?(faction.Color)));
					}
				}
			}
			float num = Mathf.Min(ColonistBarColonistDrawer.BaseIconAreaWidth / (float)ColonistBarColonistDrawer.tmpIconsToDraw.Count, ColonistBarColonistDrawer.BaseIconMaxSize) * this.ColonistBar.Scale;
			Vector2 vector = new Vector2(rect.x + 1f, rect.yMax - num - 1f);
			foreach (ColonistBarColonistDrawer.IconDrawCall iconDrawCall in ColonistBarColonistDrawer.tmpIconsToDraw)
			{
				GUI.color = (iconDrawCall.color ?? Color.white);
				this.DrawIcon(iconDrawCall.texture, ref vector, num, iconDrawCall.tooltip);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06011686 RID: 71302 RVA: 0x0050069C File Offset: 0x004FE89C
		private void DrawIcon(Texture2D icon, ref Vector2 pos, float iconSize, string tooltip = null)
		{
			Rect rect = new Rect(pos.x, pos.y, iconSize, iconSize);
			GUI.DrawTexture(rect, icon);
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			pos.x += iconSize;
		}

		// Token: 0x06011687 RID: 71303 RVA: 0x005006E4 File Offset: 0x004FE8E4
		private void DrawSelectionOverlayOnGUI(Pawn colonist, Rect rect)
		{
			Thing target = colonist;
			if (colonist.Dead)
			{
				target = colonist.Corpse;
			}
			SelectionDrawerUtility.DrawSelectionOverlayOnGUI(target, rect, 0.4f * this.ColonistBar.Scale, 20f * this.ColonistBar.Scale);
		}

		// Token: 0x06011688 RID: 71304 RVA: 0x0050072B File Offset: 0x004FE92B
		private void DrawCaravanSelectionOverlayOnGUI(Caravan caravan, Rect rect)
		{
			SelectionDrawerUtility.DrawSelectionOverlayOnGUI(caravan, rect, 0.4f * this.ColonistBar.Scale, 20f * this.ColonistBar.Scale);
		}

		// Token: 0x0400B43F RID: 46143
		private Dictionary<string, string> pawnLabelsCache = new Dictionary<string, string>();

		// Token: 0x0400B440 RID: 46144
		private static readonly Texture2D MoodBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.4f, 0.47f, 0.53f, 0.44f));

		// Token: 0x0400B441 RID: 46145
		private static readonly Texture2D MoodAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/SubtleGradient", true);

		// Token: 0x0400B442 RID: 46146
		private static readonly Texture2D DeadColonistTex = ContentFinder<Texture2D>.Get("UI/Misc/DeadColonist", true);

		// Token: 0x0400B443 RID: 46147
		private static readonly Texture2D Icon_FormingCaravan = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/FormingCaravan", true);

		// Token: 0x0400B444 RID: 46148
		private static readonly Texture2D Icon_MentalStateNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		// Token: 0x0400B445 RID: 46149
		private static readonly Texture2D Icon_MentalStateAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);

		// Token: 0x0400B446 RID: 46150
		private static readonly Texture2D Icon_MedicalRest = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MedicalRest", true);

		// Token: 0x0400B447 RID: 46151
		private static readonly Texture2D Icon_Sleeping = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Sleeping", true);

		// Token: 0x0400B448 RID: 46152
		private static readonly Texture2D Icon_Fleeing = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Fleeing", true);

		// Token: 0x0400B449 RID: 46153
		private static readonly Texture2D Icon_Attacking = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Attacking", true);

		// Token: 0x0400B44A RID: 46154
		private static readonly Texture2D Icon_Idle = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Idle", true);

		// Token: 0x0400B44B RID: 46155
		private static readonly Texture2D Icon_Burning = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Burning", true);

		// Token: 0x0400B44C RID: 46156
		private static readonly Texture2D Icon_Inspired = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Inspired", true);

		// Token: 0x0400B44D RID: 46157
		private static readonly Texture2D MoodGradient = ContentFinder<Texture2D>.Get("UI/Widgets/MoodGradient", true);

		// Token: 0x0400B44E RID: 46158
		public static readonly Vector2 PawnTextureSize = new Vector2(ColonistBar.BaseSize.x - 2f, 75f);

		// Token: 0x0400B44F RID: 46159
		public static readonly Vector3 PawnTextureCameraOffset = new Vector3(0f, 0f, 0.3f);

		// Token: 0x0400B450 RID: 46160
		public const float PawnTextureCameraZoom = 1.28205f;

		// Token: 0x0400B451 RID: 46161
		private const float PawnTextureHorizontalPadding = 1f;

		// Token: 0x0400B452 RID: 46162
		private static readonly float BaseIconAreaWidth = ColonistBarColonistDrawer.PawnTextureSize.x;

		// Token: 0x0400B453 RID: 46163
		private static readonly float BaseIconMaxSize = 20f;

		// Token: 0x0400B454 RID: 46164
		private const float BaseGroupFrameMargin = 12f;

		// Token: 0x0400B455 RID: 46165
		public const float DoubleClickTime = 0.5f;

		// Token: 0x0400B456 RID: 46166
		public const float FactionIconSpacing = 2f;

		// Token: 0x0400B457 RID: 46167
		public const float IdeoRoleIconSpacing = 2f;

		// Token: 0x0400B458 RID: 46168
		public const float SlaveIconSpacing = 2f;

		// Token: 0x0400B459 RID: 46169
		private const float MoodGradientHeight = 35f;

		// Token: 0x0400B45A RID: 46170
		private static List<ColonistBarColonistDrawer.IconDrawCall> tmpIconsToDraw = new List<ColonistBarColonistDrawer.IconDrawCall>();

		// Token: 0x0200306D RID: 12397
		private struct IconDrawCall
		{
			// Token: 0x0601168B RID: 71307 RVA: 0x005008C7 File Offset: 0x004FEAC7
			public IconDrawCall(Texture2D texture, string tooltip = null, Color? color = null)
			{
				this.texture = texture;
				this.tooltip = tooltip;
				this.color = color;
			}

			// Token: 0x0400B45B RID: 46171
			public Texture2D texture;

			// Token: 0x0400B45C RID: 46172
			public string tooltip;

			// Token: 0x0400B45D RID: 46173
			public Color? color;
		}
	}
}
