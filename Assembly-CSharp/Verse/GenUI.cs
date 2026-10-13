using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BEA RID: 3050
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		// Token: 0x06004DFA RID: 19962 RVA: 0x0018C890 File Offset: 0x0018AA90
		public static void SetLabelAlign(TextAnchor a)
		{
			Text.Anchor = a;
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x0018C898 File Offset: 0x0018AA98
		public static void ResetLabelAlign()
		{
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06004DFC RID: 19964 RVA: 0x0018C8A0 File Offset: 0x0018AAA0
		public static float BackgroundDarkAlphaForText()
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			float num = GenCelestial.CurCelestialSunGlow(Find.CurrentMap);
			float num2 = (Find.CurrentMap.Biome == BiomeDefOf.IceSheet) ? 1f : Mathf.Clamp01(Find.CurrentMap.snowGrid.TotalDepth / 1000f);
			return num * num2 * 0.41f;
		}

		// Token: 0x06004DFD RID: 19965 RVA: 0x0018C900 File Offset: 0x0018AB00
		public static void DrawTextWinterShadow(Rect rect)
		{
			float num = GenUI.BackgroundDarkAlphaForText();
			if (num > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(rect, GenUI.UnderShadowTex);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06004DFE RID: 19966 RVA: 0x0018C94C File Offset: 0x0018AB4C
		public static void DrawTextureWithMaterial(Rect rect, Texture texture, Material material, Rect texCoords = default(Rect))
		{
			if (texCoords == default(Rect))
			{
				if (material == null)
				{
					GUI.DrawTexture(rect, texture);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Color color = material.shader.SupportsMaskTex() ? GUI.color : new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, GUI.color.a);
					Graphics.DrawTexture(rect, texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, color, material);
					return;
				}
			}
			else
			{
				if (material == null)
				{
					GUI.DrawTextureWithTexCoords(rect, texture, texCoords);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Color color2 = material.shader.SupportsMaskTex() ? GUI.color : new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, GUI.color.a);
					Graphics.DrawTexture(rect, texture, texCoords, 0, 0, 0, 0, color2, material);
				}
			}
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x0018CA8C File Offset: 0x0018AC8C
		public static void DrawTexturePartWithMaterial(Rect rect, Texture texture, Material material, Rect texCoords)
		{
			if (texCoords == default(Rect))
			{
				if (material == null)
				{
					GUI.DrawTexture(rect, texture);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Color color = material.shader.SupportsMaskTex() ? GUI.color : (GUI.color * 0.5f);
					Graphics.DrawTexture(rect, texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, color, material);
					return;
				}
			}
			else
			{
				texCoords.y = 1f - texCoords.y - texCoords.height;
				if (material == null)
				{
					GUI.DrawTextureWithTexCoords(rect, texture, texCoords);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Color color2 = material.shader.SupportsMaskTex() ? GUI.color : (GUI.color * 0.5f);
					Graphics.DrawTexture(rect, texture, texCoords, 0, 0, 0, 0, color2, material);
				}
			}
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x0018CB84 File Offset: 0x0018AD84
		public static float IconDrawScale(ThingDef tDef)
		{
			float num = tDef.uiIconScale;
			if (tDef.uiIconPath.NullOrEmpty() && tDef.graphicData != null)
			{
				num *= Mathf.Min(tDef.graphicData.drawSize.x / (float)tDef.Size.x, tDef.graphicData.drawSize.y / (float)tDef.Size.z);
			}
			return num;
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x0018CBF0 File Offset: 0x0018ADF0
		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
			}
		}

		// Token: 0x06004E02 RID: 19970 RVA: 0x0018CC24 File Offset: 0x0018AE24
		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect(centerX - size / 2f, centerY - size / 2f, size, size);
			Color color2 = color;
			color2.a = alpha;
			GUI.color = color2;
			GUI.DrawTexture(position, GenUI.UIFlash);
			GUI.color = Color.white;
		}

		// Token: 0x06004E03 RID: 19971 RVA: 0x0018CC70 File Offset: 0x0018AE70
		public static Vector2 GetSizeCached(this string s)
		{
			if (GenUI.labelWidthCache.Count > 2000 || (Time.frameCount % 40000 == 0 && GenUI.labelWidthCache.Count > 100))
			{
				GenUI.labelWidthCache.Clear();
			}
			s = s.StripTags();
			Vector2 vector;
			if (!GenUI.labelWidthCache.TryGetValue(s, out vector))
			{
				vector = Text.CalcSize(s);
				GenUI.labelWidthCache.Add(s, vector);
			}
			return vector;
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x0018CCDE File Offset: 0x0018AEDE
		public static float GetWidthCached(this string s)
		{
			return s.GetSizeCached().x;
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x0018CCEB File Offset: 0x0018AEEB
		public static float GetHeightCached(this string s)
		{
			return s.GetSizeCached().y;
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x0018CCF8 File Offset: 0x0018AEF8
		public static void ClearLabelWidthCache()
		{
			GenUI.labelWidthCache.Clear();
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x0018CD04 File Offset: 0x0018AF04
		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)((int)r.x), (float)((int)r.y), (float)((int)r.width), (float)((int)r.height));
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x0018CD2F File Offset: 0x0018AF2F
		public static Rect RoundedCeil(this Rect r)
		{
			return new Rect((float)Mathf.CeilToInt(r.x), (float)Mathf.CeilToInt(r.y), (float)Mathf.CeilToInt(r.width), (float)Mathf.CeilToInt(r.height));
		}

		// Token: 0x06004E09 RID: 19977 RVA: 0x0018CD6A File Offset: 0x0018AF6A
		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)((int)v.x), (float)((int)v.y));
		}

		// Token: 0x06004E0A RID: 19978 RVA: 0x0018CD84 File Offset: 0x0018AF84
		public static float DistFromRect(Rect r, Vector2 p)
		{
			float num = Mathf.Abs(p.x - r.center.x) - r.width / 2f;
			if (num < 0f)
			{
				num = 0f;
			}
			float num2 = Mathf.Abs(p.y - r.center.y) - r.height / 2f;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x06004E0B RID: 19979 RVA: 0x0018CE08 File Offset: 0x0018B008
		public static void DrawMouseAttachment(Texture iconTex, string text = "", float angle = 0f, Vector2 offset = default(Vector2), Rect? customRect = null, Color? textColor = null, bool drawTextBackground = false, Color textBgColor = default(Color), Color? iconColor = null, Action<Rect> postDrawAction = null)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float num = mousePosition.y + 12f;
			if (drawTextBackground && text != "")
			{
				Rect value;
				if (customRect != null)
				{
					value = customRect.Value;
				}
				else
				{
					float num2 = Mathf.Min(Text.CalcSize(text).x, 200f);
					float height = Text.CalcHeight(text, 200f);
					float num3 = (iconTex != null) ? 42f : 0f;
					value = new Rect(mousePosition.x + 12f - 4f, num + num3, num2 + 8f, height);
				}
				Widgets.DrawBoxSolid(value, textBgColor);
			}
			if (iconTex != null)
			{
				Rect mouseRect;
				if (customRect != null)
				{
					mouseRect = customRect.Value;
				}
				else
				{
					mouseRect = new Rect(mousePosition.x + 8f, num + 8f, 32f, 32f);
				}
				Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
				{
					Rect rect = mouseRect.AtZero();
					rect.position += new Vector2(offset.x * rect.size.x, offset.y * rect.size.y);
					GUI.color = (iconColor ?? Color.white);
					Widgets.DrawTextureRotated(rect, iconTex, angle, null);
					GUI.color = Color.white;
					Action<Rect> postDrawAction2 = postDrawAction;
					if (postDrawAction2 == null)
					{
						return;
					}
					postDrawAction2(rect);
				}, false, false, 0f, null, false);
				num += mouseRect.height + 10f;
			}
			if (text != "")
			{
				Rect textRect = new Rect(mousePosition.x + 12f, num, 200f, 9999f);
				Find.WindowStack.ImmediateWindow(34003429, textRect, WindowLayer.Super, delegate
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					GUI.color = (textColor ?? Color.white);
					Widgets.Label(textRect.AtZero(), text);
					GUI.color = Color.white;
					Text.Font = font;
				}, false, false, 0f, null, false);
			}
		}

		// Token: 0x06004E0C RID: 19980 RVA: 0x0018D024 File Offset: 0x0018B224
		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.DrawTexture(mouseRect.AtZero(), icon);
			}, false, false, 0f, null, false);
		}

		// Token: 0x06004E0D RID: 19981 RVA: 0x0018D0A4 File Offset: 0x0018B2A4
		public static void RenderMouseoverBracket()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenUI.MouseoverBracketMaterial, 0);
		}

		// Token: 0x06004E0E RID: 19982 RVA: 0x0018D0D8 File Offset: 0x0018B2D8
		public static void DrawStatusLevel(Need status, Rect rect)
		{
			Widgets.BeginGroup(rect);
			Widgets.Label(new Rect(0f, 2f, rect.width, 25f), status.LabelCap);
			Rect rect2 = new Rect(100f, 3f, GenUI.PieceBarSize.x, GenUI.PieceBarSize.y);
			Widgets.FillableBar(rect2, status.CurLevelPercentage);
			Widgets.FillableBarChangeArrows(rect2, status.GUIChangeArrow);
			Widgets.EndGroup();
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, status.GetTipString());
			}
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
		}

		// Token: 0x06004E0F RID: 19983 RVA: 0x0018D17D File Offset: 0x0018B37D
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse(TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			return GenUI.TargetsAt(UI.MouseMapPosition(), clickParams, thingsOnly, source);
		}

		// Token: 0x06004E10 RID: 19984 RVA: 0x0018D18C File Offset: 0x0018B38C
		public static IEnumerable<LocalTargetInfo> TargetsAt(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			List<Thing> clickableList = GenUI.ThingsUnderMouse(clickPos, 0.8f, clickParams, source);
			Thing caster = (source != null) ? source.Caster : null;
			int num;
			for (int i = 0; i < clickableList.Count; i = num + 1)
			{
				Pawn pawn = clickableList[i] as Pawn;
				if (pawn == null || !pawn.IsPsychologicallyInvisible() || caster == null || caster.Faction == pawn.Faction)
				{
					yield return clickableList[i];
				}
				num = i;
			}
			if (!thingsOnly)
			{
				IntVec3 intVec = UI.MouseCell();
				if (intVec.InBounds(Find.CurrentMap, clickParams.mapBoundsContractedBy) && clickParams.CanTarget(new TargetInfo(intVec, Find.CurrentMap, false), source))
				{
					yield return intVec;
				}
			}
			yield break;
		}

		// Token: 0x06004E11 RID: 19985 RVA: 0x0018D1B4 File Offset: 0x0018B3B4
		public static List<Thing> ThingsUnderMouse(Vector3 clickPos, float pawnWideClickRadius, TargetingParameters clickParams, ITargetingSource source = null)
		{
			IntVec3 intVec = IntVec3.FromVector3(clickPos);
			List<Thing> list = new List<Thing>();
			IReadOnlyList<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((pawn.DrawPos - clickPos).MagnitudeHorizontal() < 0.4f && clickParams.CanTarget(pawn, source))
				{
					list.Add(pawn);
					list.AddRange(ContainingSelectionUtility.SelectableContainedThings(pawn));
				}
			}
			list.Sort(new Comparison<Thing>(GenUI.CompareThingsByDistanceToMousePointer));
			GenUI.cellThings.Clear();
			foreach (Thing thing4 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
			{
				if (!list.Contains(thing4) && clickParams.CanTarget(thing4, source))
				{
					GenUI.cellThings.Add(thing4);
					GenUI.cellThings.AddRange(ContainingSelectionUtility.SelectableContainedThings(thing4));
				}
			}
			IntVec3[] adjacentCells = GenAdj.AdjacentCells;
			for (int j = 0; j < adjacentCells.Length; j++)
			{
				IntVec3 c = adjacentCells[j] + intVec;
				if (c.InBounds(Find.CurrentMap) && c.GetItemCount(Find.CurrentMap) > 1)
				{
					foreach (Thing thing2 in Find.CurrentMap.thingGrid.ThingsAt(c))
					{
						if (thing2.def.category == ThingCategory.Item && (thing2.TrueCenter() - UI.MouseMapPosition()).MagnitudeHorizontalSquared() <= 0.25f && !list.Contains(thing2) && clickParams.CanTarget(thing2, source))
						{
							GenUI.cellThings.Add(thing2);
						}
					}
				}
			}
			List<Thing> list2 = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.WithCustomRectForSelector);
			for (int k = 0; k < list2.Count; k++)
			{
				Thing thing3 = list2[k];
				if (thing3.CustomRectForSelector != null && thing3.CustomRectForSelector.Value.Contains(intVec) && !list.Contains(thing3) && clickParams.CanTarget(thing3, source))
				{
					GenUI.cellThings.Add(thing3);
				}
			}
			GenUI.cellThings.Sort(new Comparison<Thing>(GenUI.CompareThingsByDrawAltitudeOrDistToItem));
			list.AddRange(GenUI.cellThings);
			GenUI.cellThings.Clear();
			IReadOnlyList<Pawn> allPawnsSpawned2 = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int l = 0; l < allPawnsSpawned2.Count; l++)
			{
				Pawn pawn2 = allPawnsSpawned2[l];
				if ((pawn2.DrawPos - clickPos).MagnitudeHorizontal() < pawnWideClickRadius && clickParams.CanTarget(pawn2, source))
				{
					GenUI.cellThings.Add(pawn2);
				}
			}
			GenUI.cellThings.Sort(new Comparison<Thing>(GenUI.CompareThingsByDistanceToMousePointer));
			for (int m = 0; m < GenUI.cellThings.Count; m++)
			{
				if (!list.Contains(GenUI.cellThings[m]))
				{
					list.Add(GenUI.cellThings[m]);
					list.AddRange(ContainingSelectionUtility.SelectableContainedThings(GenUI.cellThings[m]));
				}
			}
			list.RemoveAll((Thing thing) => !clickParams.CanTarget(thing, source));
			list.RemoveAll(delegate(Thing thing)
			{
				Pawn pawn3 = thing as Pawn;
				return pawn3 != null && pawn3.IsHiddenFromPlayer();
			});
			GenUI.cellThings.Clear();
			return list;
		}

		// Token: 0x06004E12 RID: 19986 RVA: 0x0018D5D0 File Offset: 0x0018B7D0
		private static int CompareThingsByDistanceToMousePointer(Thing a, Thing b)
		{
			Vector3 b2 = UI.MouseMapPosition();
			float num = (a.DrawPosHeld.Value - b2).MagnitudeHorizontalSquared();
			float num2 = (b.DrawPosHeld.Value - b2).MagnitudeHorizontalSquared();
			if (num < num2)
			{
				return -1;
			}
			if (num == num2)
			{
				return b.Spawned.CompareTo(a.Spawned);
			}
			return 1;
		}

		// Token: 0x06004E13 RID: 19987 RVA: 0x0018D638 File Offset: 0x0018B838
		private static int CompareThingsByDrawAltitudeOrDistToItem(Thing A, Thing B)
		{
			if (A.def.category == ThingCategory.Item && B.def.category == ThingCategory.Item)
			{
				return (A.TrueCenter() - UI.MouseMapPosition()).MagnitudeHorizontalSquared().CompareTo((B.TrueCenter() - UI.MouseMapPosition()).MagnitudeHorizontalSquared());
			}
			Thing spawnedParentOrMe = A.SpawnedParentOrMe;
			Thing spawnedParentOrMe2 = B.SpawnedParentOrMe;
			if (spawnedParentOrMe.def.Altitude != spawnedParentOrMe2.def.Altitude)
			{
				return spawnedParentOrMe2.def.Altitude.CompareTo(spawnedParentOrMe.def.Altitude);
			}
			return B.Spawned.CompareTo(A.Spawned);
		}

		// Token: 0x06004E14 RID: 19988 RVA: 0x0018D6ED File Offset: 0x0018B8ED
		public static int CurrentAdjustmentMultiplier()
		{
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent && KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 1000;
			}
			if (KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 100;
			}
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent)
			{
				return 10;
			}
			return 1;
		}

		// Token: 0x06004E15 RID: 19989 RVA: 0x0018D72C File Offset: 0x0018B92C
		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		// Token: 0x06004E16 RID: 19990 RVA: 0x0018D739 File Offset: 0x0018B939
		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
		}

		// Token: 0x06004E17 RID: 19991 RVA: 0x0018D770 File Offset: 0x0018B970
		public static Rect ExpandedBy(this Rect rect, float marginX, float marginY)
		{
			return new Rect(rect.x - marginX, rect.y - marginY, rect.width + marginX * 2f, rect.height + marginY * 2f);
		}

		// Token: 0x06004E18 RID: 19992 RVA: 0x0018D7A7 File Offset: 0x0018B9A7
		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
		}

		// Token: 0x06004E19 RID: 19993 RVA: 0x0018D7DE File Offset: 0x0018B9DE
		public static Rect ContractedBy(this Rect rect, float marginX, float marginY)
		{
			return new Rect(rect.x + marginX, rect.y + marginY, rect.width - marginX * 2f, rect.height - marginY * 2f);
		}

		// Token: 0x06004E1A RID: 19994 RVA: 0x0018D818 File Offset: 0x0018BA18
		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= rect.width * (scale - 1f) / 2f;
			rect.y -= rect.height * (scale - 1f) / 2f;
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x0018D88A File Offset: 0x0018BA8A
		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect(otherRect.x + (otherRect.width - rect.width) / 2f, rect.y, rect.width, rect.height);
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x0018D8C3 File Offset: 0x0018BAC3
		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, otherRect.y + (otherRect.height - rect.height) / 2f, rect.width, rect.height);
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x0018D8FC File Offset: 0x0018BAFC
		public static Rect AtZero(this Rect rect)
		{
			return new Rect(0f, 0f, rect.width, rect.height);
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x0018D91C File Offset: 0x0018BB1C
		public static Rect Union(this Rect a, Rect b)
		{
			return new Rect
			{
				min = Vector2.Min(a.min, b.min),
				max = Vector2.Max(a.max, b.max)
			};
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x0018D966 File Offset: 0x0018BB66
		public static void AbsorbClicksInRect(Rect r)
		{
			if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x0018D991 File Offset: 0x0018BB91
		public static Rect LeftHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x0018D9BA File Offset: 0x0018BBBA
		public static Rect LeftPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x0018D9DF File Offset: 0x0018BBDF
		public static Rect LeftPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x, rect.y, width, rect.height);
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x0018D9FC File Offset: 0x0018BBFC
		public static Rect RightHalf(this Rect rect)
		{
			return new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x0018DA33 File Offset: 0x0018BC33
		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06004E25 RID: 20005 RVA: 0x0018DA68 File Offset: 0x0018BC68
		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x0018DA8F File Offset: 0x0018BC8F
		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x0018DAB8 File Offset: 0x0018BCB8
		public static Rect TopPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height * pct);
		}

		// Token: 0x06004E28 RID: 20008 RVA: 0x0018DADD File Offset: 0x0018BCDD
		public static Rect TopPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y, rect.width, height);
		}

		// Token: 0x06004E29 RID: 20009 RVA: 0x0018DAFA File Offset: 0x0018BCFA
		public static Rect BottomHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
		}

		// Token: 0x06004E2A RID: 20010 RVA: 0x0018DB31 File Offset: 0x0018BD31
		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
		}

		// Token: 0x06004E2B RID: 20011 RVA: 0x0018DB66 File Offset: 0x0018BD66
		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}

		// Token: 0x06004E2C RID: 20012 RVA: 0x0018DB90 File Offset: 0x0018BD90
		public static Rect MiddlePart(this Rect rect, float pctWidth, float pctHeight)
		{
			return new Rect(rect.x + rect.width / 2f - rect.width * pctWidth / 2f, rect.y + rect.height / 2f - rect.height * pctHeight / 2f, rect.width * pctWidth, rect.height * pctHeight);
		}

		// Token: 0x06004E2D RID: 20013 RVA: 0x0018DC00 File Offset: 0x0018BE00
		public static Rect MiddlePartPixels(this Rect rect, float width, float height)
		{
			return new Rect(rect.x + rect.width / 2f - width / 2f, rect.y + rect.height / 2f - height / 2f, width, height);
		}

		// Token: 0x06004E2E RID: 20014 RVA: 0x0018DC4E File Offset: 0x0018BE4E
		public static IEnumerable<Vector2> Corners(this Rect rect)
		{
			yield return new Vector2(rect.x, rect.y);
			yield return new Vector2(rect.x, rect.yMax);
			yield return new Vector2(rect.xMax, rect.yMax);
			yield return new Vector2(rect.xMax, rect.y);
			yield break;
		}

		// Token: 0x06004E2F RID: 20015 RVA: 0x0018DC60 File Offset: 0x0018BE60
		public static bool SplitHorizontallyWithMargin(this Rect rect, out Rect top, out Rect bottom, out float overflow, float compressibleMargin = 0f, float? topHeight = null, float? bottomHeight = null)
		{
			if (topHeight != null == (bottomHeight != null))
			{
				throw new ArgumentException("Exactly one null height and one non-null height must be provided.");
			}
			overflow = Mathf.Max(0f, (topHeight ?? bottomHeight.GetValueOrDefault()) - rect.height);
			float height = Mathf.Clamp(topHeight ?? (rect.height - bottomHeight.Value - compressibleMargin), 0f, rect.height);
			float num = Mathf.Clamp(bottomHeight ?? (rect.height - topHeight.Value - compressibleMargin), 0f, rect.height);
			top = new Rect(rect.x, rect.y, rect.width, height);
			bottom = new Rect(rect.x, rect.yMax - num, rect.width, num);
			return overflow == 0f;
		}

		// Token: 0x06004E30 RID: 20016 RVA: 0x0018DD78 File Offset: 0x0018BF78
		public static void SplitVerticallyWithMargin(this Rect rect, out Rect left, out Rect right, float margin)
		{
			float num = rect.width / 2f;
			left = new Rect(rect.x, rect.y, num - margin / 2f, rect.height);
			right = new Rect(left.xMax + margin, rect.y, num - margin / 2f, rect.height);
		}

		// Token: 0x06004E31 RID: 20017 RVA: 0x0018DDE8 File Offset: 0x0018BFE8
		public static bool SplitVerticallyWithMargin(this Rect rect, out Rect left, out Rect right, out float overflow, float compressibleMargin = 0f, float? leftWidth = null, float? rightWidth = null)
		{
			if (leftWidth != null == (rightWidth != null))
			{
				throw new ArgumentException("Exactly one null width and one non-null width must be provided.");
			}
			overflow = Mathf.Max(0f, (leftWidth ?? rightWidth.GetValueOrDefault()) - rect.width);
			float width = Mathf.Clamp(leftWidth ?? (rect.width - rightWidth.Value - compressibleMargin), 0f, rect.width);
			float num = Mathf.Clamp(rightWidth ?? (rect.width - leftWidth.Value - compressibleMargin), 0f, rect.width);
			left = new Rect(rect.x, rect.y, width, rect.height);
			right = new Rect(rect.xMax - num, rect.y, num, rect.height);
			return overflow == 0f;
		}

		// Token: 0x06004E32 RID: 20018 RVA: 0x0018DF00 File Offset: 0x0018C100
		public static void SplitHorizontally(this Rect rect, float topHeight, out Rect top, out Rect bottom)
		{
			float num;
			rect.SplitHorizontallyWithMargin(out top, out bottom, out num, 0f, new float?(topHeight), null);
		}

		// Token: 0x06004E33 RID: 20019 RVA: 0x0018DF2C File Offset: 0x0018C12C
		public static void SplitVertically(this Rect rect, float leftWidth, out Rect left, out Rect right)
		{
			float num;
			rect.SplitVerticallyWithMargin(out left, out right, out num, 0f, new float?(leftWidth), null);
		}

		// Token: 0x06004E34 RID: 20020 RVA: 0x0018DF58 File Offset: 0x0018C158
		public static Color LerpColor(List<Pair<float, Color>> colors, float value)
		{
			if (colors.Count == 0)
			{
				return Color.white;
			}
			int i = 0;
			while (i < colors.Count)
			{
				if (value < colors[i].First)
				{
					if (i == 0)
					{
						return colors[i].Second;
					}
					return Color.Lerp(colors[i - 1].Second, colors[i].Second, Mathf.InverseLerp(colors[i - 1].First, colors[i].First, value));
				}
				else
				{
					i++;
				}
			}
			return colors.Last<Pair<float, Color>>().Second;
		}

		// Token: 0x06004E35 RID: 20021 RVA: 0x0018E004 File Offset: 0x0018C204
		public static Vector2 GetMouseAttachedWindowPos(float width, float height)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float y;
			if (mousePosition.y + 14f + height < (float)UI.screenHeight)
			{
				y = mousePosition.y + 14f;
			}
			else if (mousePosition.y - 5f - height >= 0f)
			{
				y = mousePosition.y - 5f - height;
			}
			else
			{
				y = (float)UI.screenHeight - (14f + height);
			}
			float x;
			if (mousePosition.x + 16f + width < (float)UI.screenWidth)
			{
				x = mousePosition.x + 16f;
			}
			else
			{
				x = mousePosition.x - 4f - width;
			}
			return new Vector2(x, y);
		}

		// Token: 0x06004E36 RID: 20022 RVA: 0x0018E0C0 File Offset: 0x0018C2C0
		public static float GetCenteredButtonPos(int buttonIndex, int buttonsCount, float totalWidth, float buttonWidth, float pad = 10f)
		{
			float num = (float)buttonsCount * buttonWidth + (float)(buttonsCount - 1) * pad;
			return Mathf.Floor((totalWidth - num) / 2f + (float)buttonIndex * (buttonWidth + pad));
		}

		// Token: 0x06004E37 RID: 20023 RVA: 0x0018E0F0 File Offset: 0x0018C2F0
		public static void DrawArrowPointingAt(Rect rect)
		{
			Vector2 vector = new Vector2((float)UI.screenWidth, (float)UI.screenHeight) / 2f;
			float angle = Mathf.Atan2(rect.center.x - vector.x, vector.y - rect.center.y) * 57.29578f;
			Vector2 vector2 = new Bounds(rect.center, rect.size).ClosestPoint(vector);
			Rect position = new Rect(vector2 + Vector2.left * (float)GenUI.ArrowTex.width * 0.5f, new Vector2((float)GenUI.ArrowTex.width, (float)GenUI.ArrowTex.height));
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 center = GUIUtility.GUIToScreenPoint(vector2);
			GUI.matrix = matrix;
			UI.RotateAroundPivot(angle, center);
			GUI.DrawTexture(position, GenUI.ArrowTex);
			GUI.matrix = matrix;
			UnityGUIBugsFixer.Notify_GUIMatrixChanged();
		}

		// Token: 0x06004E38 RID: 20024 RVA: 0x0018E200 File Offset: 0x0018C400
		public static void DrawArrowPointingAtWorldspace(Vector3 worldspace, Camera camera)
		{
			Vector3 vector = camera.WorldToScreenPoint(worldspace) / Prefs.UIScale;
			GenUI.DrawArrowPointingAt(new Rect(new Vector2(vector.x, (float)UI.screenHeight - vector.y) + new Vector2(-2f, 2f), new Vector2(4f, 4f)));
		}

		// Token: 0x06004E39 RID: 20025 RVA: 0x0018E264 File Offset: 0x0018C464
		public static Rect DrawElementStack<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float rowMargin = 4f, float elementMargin = 5f, bool allowOrderOptimization = true)
		{
			GenUI.tmpRects.Clear();
			GenUI.tmpRects2.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int num = Mathf.FloorToInt(rect.height / rowHeight);
			List<GenUI.StackedElementRect> list = GenUI.tmpRects;
			float num3;
			float num2;
			if (allowOrderOptimization)
			{
				num2 = (num3 = 0f);
				while (num2 < (float)num)
				{
					GenUI.StackedElementRect item = default(GenUI.StackedElementRect);
					int num4 = -1;
					for (int j = 0; j < list.Count; j++)
					{
						GenUI.StackedElementRect stackedElementRect = list[j];
						if (num4 == -1 || (item.rect.width < stackedElementRect.rect.width && stackedElementRect.rect.width < rect.width - num3))
						{
							num4 = j;
							item = stackedElementRect;
						}
					}
					if (num4 == -1)
					{
						if (num3 == 0f)
						{
							break;
						}
						num3 = 0f;
						num2 += 1f;
					}
					else
					{
						num3 += item.rect.width + elementMargin;
						GenUI.tmpRects2.Add(item);
					}
					list.RemoveAt(num4);
					if (list.Count <= 0)
					{
						break;
					}
				}
				list = GenUI.tmpRects2;
			}
			num2 = (num3 = 0f);
			while (list.Count > 0)
			{
				GenUI.StackedElementRect stackedElementRect2 = list[0];
				if (num3 + stackedElementRect2.rect.width > rect.width)
				{
					num3 = 0f;
					num2 += rowHeight + rowMargin;
				}
				if (drawer != null)
				{
					drawer(new Rect(rect.x + num3, rect.y + num2, stackedElementRect2.rect.width, stackedElementRect2.rect.height), elements[stackedElementRect2.elementIndex]);
				}
				num3 += stackedElementRect2.rect.width + elementMargin;
				list.RemoveAt(0);
			}
			return new Rect(rect.x, rect.y, rect.width, num2 + rowHeight);
		}

		// Token: 0x06004E3A RID: 20026 RVA: 0x0018E478 File Offset: 0x0018C678
		public static Rect DrawElementStackVertical<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float elementMargin = 5f, bool highlightOdd = false)
		{
			GenUI.tmpRects.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int elem = Mathf.FloorToInt(rect.height / rowHeight);
			GenUI.spacingCache.Reset(elem);
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			for (int j = 0; j < GenUI.tmpRects.Count; j++)
			{
				GenUI.StackedElementRect stackedElementRect = GenUI.tmpRects[j];
				if (num3 + stackedElementRect.rect.height > rect.height)
				{
					num3 = 0f;
					num = 0;
				}
				Rect rect2 = new Rect(rect.x + GenUI.spacingCache.GetSpaceFor(num), rect.y + num3, stackedElementRect.rect.width, stackedElementRect.rect.height);
				if (highlightOdd && j % 2 == 1)
				{
					Widgets.DrawLightHighlight(rect2);
				}
				drawer(rect2, elements[stackedElementRect.elementIndex]);
				num3 += stackedElementRect.rect.height + elementMargin;
				GenUI.spacingCache.AddSpace(num, stackedElementRect.rect.width + elementMargin);
				num2 = Mathf.Max(num2, GenUI.spacingCache.GetSpaceFor(num));
				num++;
			}
			return new Rect(rect.x, rect.y, num2, num3 + rowHeight);
		}

		// Token: 0x04003769 RID: 14185
		public const float Pad = 10f;

		// Token: 0x0400376A RID: 14186
		public const float GapTiny = 4f;

		// Token: 0x0400376B RID: 14187
		public const float GapSmall = 10f;

		// Token: 0x0400376C RID: 14188
		public const float Gap = 17f;

		// Token: 0x0400376D RID: 14189
		public const float GapWide = 26f;

		// Token: 0x0400376E RID: 14190
		public const float GapLabel = 22f;

		// Token: 0x0400376F RID: 14191
		public const float ListSpacing = 28f;

		// Token: 0x04003770 RID: 14192
		public const float MouseAttachIconSize = 32f;

		// Token: 0x04003771 RID: 14193
		public const float MouseAttachIconOffset = 8f;

		// Token: 0x04003772 RID: 14194
		public const float ScrollBarWidth = 16f;

		// Token: 0x04003773 RID: 14195
		public const float HorizontalSliderHeight = 10f;

		// Token: 0x04003774 RID: 14196
		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		// Token: 0x04003775 RID: 14197
		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		// Token: 0x04003776 RID: 14198
		public static readonly Color SubtleMouseoverColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x04003777 RID: 14199
		public static readonly Color FillableBar_Green = new Color(0.40392157f, 0.7019608f, 0.28627452f);

		// Token: 0x04003778 RID: 14200
		public static readonly Color FillableBar_Empty = new Color(0.03f, 0.035f, 0.05f);

		// Token: 0x04003779 RID: 14201
		public static readonly Vector2 MaxWinSize = new Vector2(1010f, 754f);

		// Token: 0x0400377A RID: 14202
		public const float SmallIconSize = 24f;

		// Token: 0x0400377B RID: 14203
		public const int RootGUIDepth = 50;

		// Token: 0x0400377C RID: 14204
		private const float MouseIconSize = 32f;

		// Token: 0x0400377D RID: 14205
		private const float MouseIconOffset = 12f;

		// Token: 0x0400377E RID: 14206
		private static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", ShaderDatabase.MetaOverlay);

		// Token: 0x0400377F RID: 14207
		private static readonly Texture2D UnderShadowTex = ContentFinder<Texture2D>.Get("UI/Misc/ScreenCornerShadow", true);

		// Token: 0x04003780 RID: 14208
		private static readonly Texture2D UIFlash = ContentFinder<Texture2D>.Get("UI/Misc/Flash", true);

		// Token: 0x04003781 RID: 14209
		private static Dictionary<string, Vector2> labelWidthCache = new Dictionary<string, Vector2>();

		// Token: 0x04003782 RID: 14210
		private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

		// Token: 0x04003783 RID: 14211
		public const float PawnDirectClickRadius = 0.4f;

		// Token: 0x04003784 RID: 14212
		private static List<Thing> cellThings = new List<Thing>(32);

		// Token: 0x04003785 RID: 14213
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/Arrow", true);

		// Token: 0x04003786 RID: 14214
		private static List<GenUI.StackedElementRect> tmpRects = new List<GenUI.StackedElementRect>();

		// Token: 0x04003787 RID: 14215
		private static List<GenUI.StackedElementRect> tmpRects2 = new List<GenUI.StackedElementRect>();

		// Token: 0x04003788 RID: 14216
		public const float ElementStackDefaultElementMargin = 5f;

		// Token: 0x04003789 RID: 14217
		public const float ElementStackDefaultRowMargin = 4f;

		// Token: 0x0400378A RID: 14218
		private static GenUI.SpacingCache spacingCache;

		// Token: 0x02000BEB RID: 3051
		private struct StackedElementRect
		{
			// Token: 0x06004E3C RID: 20028 RVA: 0x0018E71F File Offset: 0x0018C91F
			public StackedElementRect(Rect rect, int elementIndex)
			{
				this.rect = rect;
				this.elementIndex = elementIndex;
			}

			// Token: 0x0400378B RID: 14219
			public Rect rect;

			// Token: 0x0400378C RID: 14220
			public int elementIndex;
		}

		// Token: 0x02000BEC RID: 3052
		public class AnonymousStackElement
		{
			// Token: 0x0400378D RID: 14221
			public Action<Rect> drawer;

			// Token: 0x0400378E RID: 14222
			public float width;
		}

		// Token: 0x02000BED RID: 3053
		private struct SpacingCache
		{
			// Token: 0x06004E3E RID: 20030 RVA: 0x0018E730 File Offset: 0x0018C930
			public void Reset(int elem = 16)
			{
				if (this.spaces == null || this.maxElements != elem)
				{
					this.maxElements = elem;
					this.spaces = new float[this.maxElements];
					return;
				}
				for (int i = 0; i < this.maxElements; i++)
				{
					this.spaces[i] = 0f;
				}
			}

			// Token: 0x06004E3F RID: 20031 RVA: 0x0018E785 File Offset: 0x0018C985
			public float GetSpaceFor(int elem)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					return this.spaces[elem];
				}
				return 0f;
			}

			// Token: 0x06004E40 RID: 20032 RVA: 0x0018E7BB File Offset: 0x0018C9BB
			public void AddSpace(int elem, float space)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					this.spaces[elem] += space;
				}
			}

			// Token: 0x0400378F RID: 14223
			private int maxElements;

			// Token: 0x04003790 RID: 14224
			private float[] spaces;
		}

		// Token: 0x02000BEE RID: 3054
		// (Invoke) Token: 0x06004E42 RID: 20034
		public delegate void StackElementDrawer<T>(Rect rect, T element);

		// Token: 0x02000BEF RID: 3055
		// (Invoke) Token: 0x06004E46 RID: 20038
		public delegate float StackElementWidthGetter<T>(T element);
	}
}
