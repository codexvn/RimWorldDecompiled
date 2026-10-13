using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000C11 RID: 3089
	[StaticConstructorOnStartup]
	public static class Widgets
	{
		// Token: 0x17000C14 RID: 3092
		// (get) Token: 0x06004ECE RID: 20174 RVA: 0x00191D28 File Offset: 0x0018FF28
		public static bool Painting
		{
			get
			{
				return Widgets.dropdownPainting || Widgets.checkboxPainting;
			}
		}

		// Token: 0x06004ECF RID: 20175 RVA: 0x00191D38 File Offset: 0x0018FF38
		static Widgets()
		{
			Color color = new Color(1f, 1f, 1f, 0f);
			Widgets.LineTexAA = new Texture2D(1, 3, TextureFormat.ARGB32, false);
			Widgets.LineTexAA.name = "LineTexAA";
			Widgets.LineTexAA.SetPixel(0, 0, color);
			Widgets.LineTexAA.SetPixel(0, 1, Color.white);
			Widgets.LineTexAA.SetPixel(0, 2, color);
			Widgets.LineTexAA.Apply();
		}

		// Token: 0x06004ED0 RID: 20176 RVA: 0x0000CBAD File Offset: 0x0000ADAD
		public static void BeginGroup(Rect rect)
		{
			GUI.BeginGroup(rect);
			UnityGUIBugsFixer.Notify_BeginGroup();
		}

		// Token: 0x06004ED1 RID: 20177 RVA: 0x0000CBBA File Offset: 0x0000ADBA
		public static void EndGroup()
		{
			GUI.EndGroup();
			UnityGUIBugsFixer.Notify_EndGroup();
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x0019236F File Offset: 0x0019056F
		public static void ClearLabelCache()
		{
			Widgets.LabelCache.Clear();
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x0019237C File Offset: 0x0019057C
		public static bool CanDrawIconFor(Def def)
		{
			BuildableDef buildableDef = def as BuildableDef;
			if (buildableDef != null)
			{
				return buildableDef.uiIcon != null;
			}
			FactionDef factionDef = def as FactionDef;
			return factionDef != null && factionDef.FactionIcon != null;
		}

		// Token: 0x06004ED4 RID: 20180 RVA: 0x001923B8 File Offset: 0x001905B8
		public static void DefIcon(Rect rect, Def def, ThingDef stuffDef = null, float scale = 1f, ThingStyleDef thingStyleDef = null, bool drawPlaceholder = false, Color? color = null, Material material = null, int? graphicIndexOverride = null, float alpha = 1f)
		{
			BuildableDef buildableDef = def as BuildableDef;
			if (buildableDef != null)
			{
				rect.position += new Vector2(buildableDef.uiIconOffset.x * rect.size.x, buildableDef.uiIconOffset.y * rect.size.y);
			}
			ThingDef thingDef = def as ThingDef;
			if (thingDef != null && thingDef.IsFrame && thingDef.entityDefToBuild != null)
			{
				def = thingDef.entityDefToBuild;
			}
			ThingDef thingDef2 = def as ThingDef;
			if (thingDef2 != null)
			{
				Widgets.ThingIcon(rect, thingDef2, stuffDef, thingStyleDef, scale, color, graphicIndexOverride, alpha);
				return;
			}
			PawnKindDef pawnKindDef = def as PawnKindDef;
			if (pawnKindDef != null)
			{
				Widgets.ThingIcon(rect, pawnKindDef.race, stuffDef, thingStyleDef, scale, color, graphicIndexOverride, alpha);
				return;
			}
			RecipeDef recipeDef = def as RecipeDef;
			if (recipeDef != null && (recipeDef.UIIconThing != null || recipeDef.UIIcon != null))
			{
				if (recipeDef.UIIconThing != null)
				{
					Widgets.ThingIcon(rect, recipeDef.UIIconThing, null, thingStyleDef, scale, color, graphicIndexOverride, alpha);
					return;
				}
				if (recipeDef.UIIcon != null)
				{
					Widgets.DrawTextureFitted(rect, recipeDef.UIIcon, scale, material, alpha);
					return;
				}
			}
			else
			{
				TerrainDef terrainDef = def as TerrainDef;
				if (terrainDef != null && terrainDef.uiIcon != null)
				{
					GUI.color = terrainDef.uiIconColor;
					Rect texCoords = terrainDef.cropIcon ? Widgets.CroppedTerrainTextureRect(terrainDef.uiIcon) : new Rect(0f, 0f, 1f, 1f);
					Widgets.DrawTextureFitted(rect, terrainDef.uiIcon, scale, Vector2.one, texCoords, 0f, material, alpha);
					GUI.color = Color.white;
					return;
				}
				FactionDef factionDef = def as FactionDef;
				if (factionDef != null)
				{
					if (!factionDef.colorSpectrum.NullOrEmpty<Color>())
					{
						GUI.color = factionDef.colorSpectrum.FirstOrDefault<Color>();
					}
					Widgets.DrawTextureFitted(rect, factionDef.FactionIcon, scale, material, alpha);
					GUI.color = Color.white;
					return;
				}
				StyleItemDef styleItemDef = def as StyleItemDef;
				if (styleItemDef != null)
				{
					Widgets.DrawTextureFitted(rect, styleItemDef.Icon, scale, material, alpha);
					return;
				}
				BodyTypeDef bodyTypeDef = def as BodyTypeDef;
				if (bodyTypeDef != null)
				{
					Widgets.DrawTextureFitted(rect, bodyTypeDef.Icon, scale, material, alpha);
					return;
				}
				HeadTypeDef headTypeDef = def as HeadTypeDef;
				if (headTypeDef != null)
				{
					Widgets.DrawTextureFitted(rect, headTypeDef.Icon, scale, material, alpha);
					return;
				}
				GeneDef geneDef = def as GeneDef;
				if (geneDef != null)
				{
					GUI.color = (color ?? geneDef.IconColor);
					Widgets.DrawTextureFitted(rect, geneDef.Icon, scale, material, alpha);
					GUI.color = Color.white;
					return;
				}
				XenotypeDef xenotypeDef = def as XenotypeDef;
				if (xenotypeDef != null)
				{
					GUI.color = (color ?? XenotypeDef.IconColor);
					Widgets.DrawTextureFitted(rect, xenotypeDef.Icon, scale, material, alpha);
					GUI.color = Color.white;
					return;
				}
				PsychicRitualDef psychicRitualDef = def as PsychicRitualDef;
				if (psychicRitualDef != null)
				{
					Widgets.DrawTextureFitted(rect, psychicRitualDef.uiIcon, scale, material, alpha);
					return;
				}
				if (drawPlaceholder)
				{
					Widgets.DrawTextureFitted(rect, Widgets.PlaceholderIconTex, scale, material, alpha);
				}
			}
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x001926D0 File Offset: 0x001908D0
		public static void ThingIcon(Rect rect, Thing thing, float alpha = 1f, Rot4? rot = null, bool stackOfOne = false, float scale = 1f, bool grayscale = false)
		{
			thing = thing.GetInnerIfMinified();
			Blueprint blueprint = thing as Blueprint;
			if (blueprint != null && blueprint.EntityToBuild() != null)
			{
				Widgets.DefIcon(rect, blueprint.EntityToBuild(), blueprint.EntityToBuildStuff(), 1f, blueprint.EntityToBuildStyle(), false, null, null, null, alpha);
				return;
			}
			float num;
			float resolvedIconAngle;
			Vector2 vector;
			Color color;
			Material material;
			Texture iconFor = Widgets.GetIconFor(thing, new Vector2(rect.width, rect.height), rot, stackOfOne, out num, out resolvedIconAngle, out vector, out color, out material);
			Frame frame = thing as Frame;
			if (frame != null && frame.BuildDef != null)
			{
				iconFor = Widgets.GetIconFor(frame.BuildDef, frame.Stuff, frame.StyleDef, null);
			}
			if (iconFor == null || iconFor == BaseContent.BadTex)
			{
				return;
			}
			GUI.color = color;
			ThingStyleDef styleDef = thing.StyleDef;
			if ((styleDef != null && styleDef.UIIcon != null) || !thing.def.uiIconPath.NullOrEmpty())
			{
				rect.position += new Vector2(thing.def.uiIconOffset.x * rect.size.x, thing.def.uiIconOffset.y * rect.size.y);
			}
			Material mat = material;
			if (grayscale)
			{
				MaterialRequest req = new MaterialRequest
				{
					shader = ShaderDatabase.GrayscaleGUI,
					color = color
				};
				if (material != null)
				{
					req.maskTex = (Texture2D)material.GetTexture(ShaderPropertyIDs.MaskTex);
					req.color = material.GetColor(ShaderPropertyIDs.Color);
					req.colorTwo = material.GetColor(ShaderPropertyIDs.ColorTwo);
				}
				else
				{
					req.maskTex = Texture2D.redTexture;
				}
				mat = MaterialPool.MatFrom(req);
			}
			Widgets.ThingIconWorker(rect, thing.def, iconFor, resolvedIconAngle, num * scale, rot, mat, alpha);
			GUI.color = Color.white;
		}

		// Token: 0x06004ED6 RID: 20182 RVA: 0x001928DC File Offset: 0x00190ADC
		public static void ThingIcon(Rect rect, ThingDef thingDef, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null, float scale = 1f, Color? color = null, int? graphicIndexOverride = null, float alpha = 1f)
		{
			if (thingDef.uiIcon == null || thingDef.uiIcon == BaseContent.BadTex)
			{
				return;
			}
			Material material;
			Texture2D iconFor = Widgets.GetIconFor(thingDef, out material, stuffDef, thingStyleDef, graphicIndexOverride);
			if (iconFor == null)
			{
				return;
			}
			Color color2 = GUI.color;
			if (color != null)
			{
				GUI.color = color.Value;
			}
			else if (stuffDef != null)
			{
				GUI.color = thingDef.GetColorForStuff(stuffDef);
			}
			else if (material != null)
			{
				GUI.color = Color.white;
			}
			else
			{
				GUI.color = (thingDef.MadeFromStuff ? thingDef.GetColorForStuff(GenStuff.DefaultStuffFor(thingDef)) : thingDef.uiIconColor);
			}
			if (thingStyleDef != null)
			{
				scale *= thingStyleDef.uiIconScale;
			}
			else
			{
				scale *= GenUI.IconDrawScale(thingDef);
			}
			float num = (float)iconFor.width / (float)iconFor.height;
			rect = ((num < 1f) ? rect.MiddlePart(num, 1f) : rect.MiddlePart(1f, num));
			Rect rect2 = rect;
			Texture resolvedIcon = iconFor;
			float uiIconAngle = thingDef.uiIconAngle;
			float scale2 = scale;
			Material mat = material;
			Widgets.ThingIconWorker(rect2, thingDef, resolvedIcon, uiIconAngle, scale2, null, mat, alpha);
			GUI.color = color2;
		}

		// Token: 0x06004ED7 RID: 20183 RVA: 0x001929FC File Offset: 0x00190BFC
		public static Texture2D GetIconFor(ThingDef thingDef, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null, int? graphicIndexOverride = null)
		{
			Material material;
			return Widgets.GetIconFor(thingDef, out material, stuffDef, thingStyleDef, graphicIndexOverride);
		}

		// Token: 0x06004ED8 RID: 20184 RVA: 0x00192A14 File Offset: 0x00190C14
		public static Texture2D GetIconFor(ThingDef thingDef, out Material material, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null, int? graphicIndexOverride = null)
		{
			if (thingDef.IsCorpse)
			{
				IngestibleProperties ingestible = thingDef.ingestible;
				if (((ingestible != null) ? ingestible.sourceDef : null) != null)
				{
					thingDef = thingDef.ingestible.sourceDef;
				}
			}
			material = null;
			Texture2D result = thingDef.GetUIIconForStuff(stuffDef);
			if (thingStyleDef != null && thingStyleDef.UIIcon != null)
			{
				if (graphicIndexOverride != null)
				{
					result = thingStyleDef.IconForIndex(graphicIndexOverride.Value, null);
				}
				else
				{
					result = thingStyleDef.UIIcon;
				}
			}
			else
			{
				Graphic_Appearances graphic_Appearances = thingDef.graphic as Graphic_Appearances;
				if (graphic_Appearances != null)
				{
					result = (Texture2D)graphic_Appearances.SubGraphicFor(stuffDef ?? GenStuff.DefaultStuffFor(thingDef)).MatAt(thingDef.defaultPlacingRot, null).mainTexture;
				}
				else if (thingDef.uiIconMaterial != null)
				{
					material = thingDef.uiIconMaterial;
				}
			}
			return result;
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x00192AE4 File Offset: 0x00190CE4
		private static Color GetDrawColor(BuildableDef buildable, ThingDef stuff)
		{
			ThingDef thingDef = buildable as ThingDef;
			if (thingDef != null)
			{
				if (stuff != null)
				{
					return thingDef.GetColorForStuff(stuff);
				}
				if (thingDef.graphicData != null)
				{
					return thingDef.graphicData.color;
				}
			}
			else
			{
				TerrainDef terrainDef = buildable as TerrainDef;
				if (terrainDef != null)
				{
					return terrainDef.DrawColor;
				}
			}
			return Color.white;
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x00192B30 File Offset: 0x00190D30
		public static Texture GetIconFor(Thing thing, Vector2 size, Rot4? rot, bool stackOfOne, out float scale, out float angle, out Vector2 iconProportions, out Color color, out Material material)
		{
			if (thing == null)
			{
				scale = 1f;
				angle = 0f;
				iconProportions = Vector2.one;
				color = Color.white;
				material = null;
				return null;
			}
			material = null;
			thing = thing.GetInnerIfMinified();
			Corpse corpse = thing as Corpse;
			if (corpse != null)
			{
				thing = corpse.InnerPawn;
			}
			Texture result = null;
			ThingStyleDef styleDef = thing.StyleDef;
			iconProportions = thing.DrawSize;
			color = thing.DrawColor;
			scale = GenUI.IconDrawScale(thing.def);
			Blueprint blueprint = thing as Blueprint;
			if (blueprint != null)
			{
				color = Widgets.GetDrawColor(blueprint.EntityToBuild(), blueprint.EntityToBuildStuff());
			}
			if (rot != null && rot.Value.IsHorizontal)
			{
				iconProportions = new Vector2(iconProportions.y, iconProportions.x);
			}
			angle = 0f;
			if (thing.UIIconOverride != null)
			{
				result = thing.UIIconOverride;
				angle = thing.def.uiIconAngle;
			}
			else if (styleDef != null && styleDef.UIIcon != null)
			{
				Rot4 value = rot.GetValueOrDefault();
				if (rot == null)
				{
					value = thing.def.defaultPlacingRot;
					rot = new Rot4?(value);
				}
				result = styleDef.IconForIndex(thing.OverrideGraphicIndex ?? thing.thingIDNumber, rot);
				angle = thing.def.uiIconAngle;
			}
			else if (!thing.def.uiIconPath.NullOrEmpty())
			{
				result = thing.def.uiIcon;
				angle = thing.def.uiIconAngle;
			}
			else
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null)
				{
					if (!pawn.RaceProps.Humanlike)
					{
						Rot4 value = rot.GetValueOrDefault();
						if (rot == null)
						{
							value = Rot4.East;
							rot = new Rot4?(value);
						}
						Pawn_DrawTracker drawer = pawn.Drawer;
						if (drawer != null)
						{
							PawnRenderer renderer = drawer.renderer;
							if (renderer != null)
							{
								renderer.EnsureGraphicsInitialized();
							}
						}
						Pawn_DrawTracker drawer2 = pawn.Drawer;
						Material material2;
						if (drawer2 == null)
						{
							material2 = null;
						}
						else
						{
							PawnRenderer renderer2 = drawer2.renderer;
							if (renderer2 == null)
							{
								material2 = null;
							}
							else
							{
								Graphic bodyGraphic = renderer2.BodyGraphic;
								material2 = ((bodyGraphic != null) ? bodyGraphic.MatAt(rot.Value, null) : null);
							}
						}
						Material material3 = material2;
						if (material3 != null)
						{
							result = material3.mainTexture;
							Shader shader;
							MaterialRequest req;
							if (ShaderDatabase.TryGetUIShader(material3.shader, out shader) && MaterialPool.TryGetRequestForMat(material3, out req))
							{
								req.shader = shader;
								material = MaterialPool.MatFrom(req);
								color = Color.white;
							}
							else
							{
								color = material3.color;
							}
						}
					}
					else
					{
						Rot4 value = rot.GetValueOrDefault();
						if (rot == null)
						{
							value = Rot4.South;
							rot = new Rot4?(value);
						}
						Rect r = new Rect(0f, 0f, size.x, size.y).ScaledBy(1.8f);
						r = r.Rounded();
						float num = 1.8f;
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
						{
							num = 3f;
						}
						else
						{
							r.y += 3f;
						}
						Pawn pawn2 = pawn;
						Vector2 size2 = new Vector2(r.width, r.height);
						Rot4 value2 = rot.Value;
						float cameraZoom = num;
						result = PortraitsCache.Get(pawn2, size2, value2, default(Vector3), cameraZoom, true, true, true, true, null, null, false, null);
					}
				}
				else
				{
					Rot4 value = rot.GetValueOrDefault();
					if (rot == null)
					{
						value = thing.def.defaultPlacingRot;
						rot = new Rot4?(value);
					}
					Material material4;
					if (stackOfOne)
					{
						Graphic_StackCount graphic_StackCount = thing.Graphic as Graphic_StackCount;
						if (graphic_StackCount != null && !(thing.Graphic is Graphic_MealVariants))
						{
							material4 = graphic_StackCount.SubGraphicForStackCount(1, thing.def).MatSingleFor(thing);
						}
						else
						{
							material4 = thing.Graphic.ExtractInnerGraphicFor(thing, null).MatAt(rot.Value, null);
						}
					}
					else
					{
						material4 = thing.Graphic.ExtractInnerGraphicFor(thing, null).MatAt(rot.Value, null);
					}
					result = material4.mainTexture;
					Shader shader2;
					MaterialRequest req2;
					if (ShaderDatabase.TryGetUIShader(material4.shader, out shader2) && MaterialPool.TryGetRequestForMat(material4, out req2))
					{
						req2.shader = shader2;
						material = MaterialPool.MatFrom(req2);
						color = Color.white;
					}
				}
			}
			return result;
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x00192FA8 File Offset: 0x001911A8
		private static void ThingIconWorker(Rect rect, ThingDef thingDef, Texture resolvedIcon, float resolvedIconAngle, float scale = 1f, Rot4? rot = null, Material mat = null, float alpha = 1f)
		{
			Vector2 texProportions = new Vector2((float)resolvedIcon.width, (float)resolvedIcon.height);
			Rect texCoords = Widgets.DefaultTexCoords;
			Rot4 value = rot.GetValueOrDefault();
			if (rot == null)
			{
				value = thingDef.defaultPlacingRot;
				rot = new Rot4?(value);
			}
			if (thingDef.graphicData != null)
			{
				texProportions = (rot.Value.IsHorizontal ? thingDef.graphicData.drawSize.Rotated() : thingDef.graphicData.drawSize);
				if (thingDef.uiIconPath.NullOrEmpty() && thingDef.graphicData.linkFlags != LinkFlags.None)
				{
					texCoords = Widgets.LinkedTexCoords;
				}
			}
			Widgets.DrawTextureFitted(rect, resolvedIcon, scale, texProportions, texCoords, resolvedIconAngle, mat, alpha);
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x00193057 File Offset: 0x00191257
		public static Rect CroppedTerrainTextureRect(Texture2D tex)
		{
			return new Rect(0f, 0f, 64f / (float)tex.width, 64f / (float)tex.height);
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x00193082 File Offset: 0x00191282
		public static void DrawAltRect(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.AltTexture);
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x00193090 File Offset: 0x00191290
		public static void ListSeparator(ref RectDivider divider, string label)
		{
			RectDivider rect = divider.NewRow(25f, VerticalJustification.Top, null);
			GUI.BeginGroup(rect);
			float num = 0f;
			Widgets.ListSeparator(ref num, rect.Rect.width, label);
			GUI.EndGroup();
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x001930E0 File Offset: 0x001912E0
		public static void ListSeparator(ref float curY, float width, string label)
		{
			Color color = GUI.color;
			curY += 3f;
			GUI.color = Widgets.SeparatorLabelColor;
			Rect rect = new Rect(0f, curY, width, 30f);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, label);
			curY += 20f;
			GUI.color = Widgets.SeparatorLineColor;
			Widgets.DrawLineHorizontal(0f, curY, width);
			curY += 2f;
			GUI.color = color;
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x00193154 File Offset: 0x00191354
		public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
		{
			float num = end.x - start.x;
			float num2 = end.y - start.y;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 < 0.01f)
			{
				return;
			}
			width *= 3f;
			float num4 = width * num2 / num3;
			float num5 = width * num / num3;
			float z = -Mathf.Atan2(-num2, num) * 57.29578f;
			Vector2 vector = start + new Vector2(0.5f * num4, -0.5f * num5);
			Matrix4x4 m = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, z), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			Rect position = new Rect(start.x, start.y - 0.5f * num5, num3, width);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GUI.DrawTexture(position, Widgets.LineTexAA, ScaleMode.StretchToFill, true, 0f, color, 0f, 0f);
			GL.PopMatrix();
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x00193263 File Offset: 0x00191463
		public static void DrawLineHorizontal(float x, float y, float length, Color color)
		{
			Widgets.DrawBoxSolid(new Rect(x, y, length, 1f), color);
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x00193278 File Offset: 0x00191478
		public static void DrawLineHorizontal(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, length, 1f), BaseContent.WhiteTex);
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x00193291 File Offset: 0x00191491
		public static void DrawLineVertical(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, 1f, length), BaseContent.WhiteTex);
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x001932AA File Offset: 0x001914AA
		public static void DrawBoxSolid(Rect rect, Color color)
		{
			Color color2 = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = color2;
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x001932C7 File Offset: 0x001914C7
		public static void DrawBoxSolidWithOutline(Rect rect, Color solidColor, Color outlineColor, int outlineThickness = 1)
		{
			Widgets.DrawBoxSolid(rect, solidColor);
			Color color = GUI.color;
			GUI.color = outlineColor;
			Widgets.DrawBox(rect, outlineThickness, null);
			GUI.color = color;
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x001932E8 File Offset: 0x001914E8
		public static void DrawBox(Rect rect, int thickness = 1, Texture2D lineTexture = null)
		{
			Vector2 vector = new Vector2(rect.x, rect.y);
			Vector2 vector2 = new Vector2(rect.x + rect.width, rect.y + rect.height);
			if (vector.x > vector2.x)
			{
				ref float ptr = ref vector.x;
				float num = vector2.x;
				float num2 = vector.x;
				ptr = num;
				vector2.x = num2;
			}
			if (vector.y > vector2.y)
			{
				ref float ptr = ref vector.y;
				float num2 = vector2.y;
				float num = vector.y;
				ptr = num2;
				vector2.y = num;
			}
			Vector3 vector3 = vector2 - vector;
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector.x, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector2.x - (float)thickness, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector.y, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(UIScaling.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector2.y - (float)thickness, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x00193470 File Offset: 0x00191670
		public static void LabelCacheHeight(ref Rect rect, string label, bool renderLabel = true, bool forceInvalidation = false)
		{
			bool flag = Widgets.LabelCache.ContainsKey(label);
			if (forceInvalidation)
			{
				flag = false;
			}
			float height;
			if (flag)
			{
				height = Widgets.LabelCache[label];
			}
			else
			{
				height = Text.CalcHeight(label, rect.width);
			}
			rect.height = height;
			if (renderLabel)
			{
				Widgets.Label(rect, label);
			}
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x001934C8 File Offset: 0x001916C8
		public static void Label(Rect rect, GUIContent content)
		{
			GUI.Label(rect, content, Text.CurFontStyle);
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x001934D6 File Offset: 0x001916D6
		public static void LabelEllipses(Rect rect, string label)
		{
			label = Text.ClampTextWithEllipsis(rect, label);
			Widgets.Label(rect, label);
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x001934E8 File Offset: 0x001916E8
		public static void Label(Rect rect, string label)
		{
			Rect position = rect;
			float num = Prefs.UIScale / 2f;
			if (Prefs.UIScale > 1f && Math.Abs(num - Mathf.Floor(num)) > 1E-45f)
			{
				position.xMin = UIScaling.AdjustCoordToUIScalingFloor(rect.xMin);
				position.yMin = UIScaling.AdjustCoordToUIScalingFloor(rect.yMin);
				position.xMax = UIScaling.AdjustCoordToUIScalingCeil(rect.xMax + 1E-05f);
				position.yMax = UIScaling.AdjustCoordToUIScalingCeil(rect.yMax + 1E-05f);
			}
			GUI.Label(position, label, Text.CurFontStyle);
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x00193587 File Offset: 0x00191787
		public static void Label(Rect rect, TaggedString label)
		{
			Widgets.Label(rect, label.Resolve());
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x00193598 File Offset: 0x00191798
		public static void Label(float x, ref float curY, float width, string text, TipSignal tip = default(TipSignal))
		{
			if (text.NullOrEmpty())
			{
				return;
			}
			float num = Text.CalcHeight(text, width);
			Rect rect = new Rect(x, curY, width, num);
			if (!tip.text.NullOrEmpty() || tip.textGetter != null)
			{
				float x2 = Text.CalcSize(text).x;
				Rect rect2 = new Rect(rect.x, rect.y, x2, num);
				Widgets.DrawHighlightIfMouseover(rect2);
				TooltipHandler.TipRegion(rect2, tip);
			}
			Widgets.Label(rect, text);
			curY += num;
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x00193614 File Offset: 0x00191814
		public static void Label(Rect rect, ref float y, string text, TipSignal tip = default(TipSignal))
		{
			if (text.NullOrEmpty())
			{
				return;
			}
			Widgets.Label(rect.x, ref y, rect.width, text, tip);
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x00193638 File Offset: 0x00191838
		public static void LongLabel(float x, float width, string label, ref float curY, bool draw = true)
		{
			if (label.Length < 2500)
			{
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 1000f), label);
				}
				curY += Text.CalcHeight(label, width);
				return;
			}
			int num = 0;
			int num2 = -1;
			bool flag = false;
			for (int i = 0; i < label.Length; i++)
			{
				if (label[i] == '\n')
				{
					num++;
					if (num >= 50)
					{
						string text = label.Substring(num2 + 1, i - num2 - 1);
						num2 = i;
						num = 0;
						if (flag)
						{
							curY += Text.SpaceBetweenLines;
						}
						if (draw)
						{
							Widgets.Label(new Rect(x, curY, width, 10000f), text);
						}
						curY += Text.CalcHeight(text, width);
						flag = true;
					}
				}
			}
			if (num2 != label.Length - 1)
			{
				if (flag)
				{
					curY += Text.SpaceBetweenLines;
				}
				string text2 = label.Substring(num2 + 1);
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 10000f), text2);
				}
				curY += Text.CalcHeight(text2, width);
			}
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x00193738 File Offset: 0x00191938
		public static void LabelScrollable(Rect rect, string label, ref Vector2 scrollbarPosition, bool dontConsumeScrollEventsIfNoScrollbar = false, bool takeScrollbarSpaceEvenIfNoScrollbar = true, bool longLabel = false)
		{
			bool flag = takeScrollbarSpaceEvenIfNoScrollbar || Text.CalcHeight(label, rect.width) > rect.height;
			bool flag2 = flag && (!dontConsumeScrollEventsIfNoScrollbar || Text.CalcHeight(label, rect.width - 16f) > rect.height);
			float num = rect.width;
			if (flag)
			{
				num -= 16f;
			}
			float num2;
			if (longLabel)
			{
				num2 = 0f;
				Widgets.LongLabel(0f, num, label, ref num2, false);
			}
			else
			{
				num2 = Text.CalcHeight(label, num);
			}
			Rect rect2 = new Rect(0f, 0f, num, Mathf.Max(num2 + 5f, rect.height));
			if (flag2)
			{
				Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			}
			else
			{
				Widgets.BeginGroup(rect);
			}
			if (longLabel)
			{
				float y = rect2.y;
				Widgets.LongLabel(rect2.x, rect2.width, label, ref y, true);
			}
			else
			{
				Widgets.Label(rect2, label);
			}
			if (flag2)
			{
				Widgets.EndScrollView();
				return;
			}
			Widgets.EndGroup();
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x00193834 File Offset: 0x00191A34
		public static void LabelWithIcon(Rect rect, string label, Texture2D labelIcon, float labelIconScale = 1f)
		{
			float num = Mathf.Min((float)labelIcon.width, rect.height);
			Rect outerRect = new Rect(rect.x, rect.y, num, rect.height);
			rect.xMin += num;
			Widgets.DrawTextureFitted(outerRect, labelIcon, labelIconScale, 1f);
			Widgets.Label(rect, label);
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x00193894 File Offset: 0x00191A94
		public static void DefLabelWithIcon(Rect rect, Def def, float iconMargin = 2f, float textOffsetX = 6f)
		{
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, def.description);
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			Widgets.DefIcon(rect2, def, null, 1f, null, true, null, null, null, 1f);
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect3, def.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			Widgets.EndGroup();
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x00193960 File Offset: 0x00191B60
		public static bool LabelFit(Rect rect, string label)
		{
			bool result = false;
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			if (Text.CalcSize(label).x <= rect.width)
			{
				Widgets.Label(rect, label);
			}
			else
			{
				Text.Font = GameFont.Tiny;
				if (Text.CalcSize(label).x <= rect.width)
				{
					Widgets.Label(rect, label);
				}
				else
				{
					Widgets.LabelEllipses(rect, label);
					result = true;
				}
				Text.Font = GameFont.Small;
			}
			Text.Font = font;
			return result;
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x001939D0 File Offset: 0x00191BD0
		public static void HyperlinkWithIcon(Rect rect, Dialog_InfoCard.Hyperlink hyperlink, string text = null, float iconMargin = 2f, float textOffsetX = 6f, Color? color = null, bool truncateLabel = false, string textSuffix = null)
		{
			string text2 = text ?? hyperlink.Label.CapitalizeFirst();
			if (textSuffix != null)
			{
				text2 += textSuffix;
			}
			Widgets.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			if (hyperlink.IsHidden)
			{
				Widgets.DrawTextureFitted(rect2, Widgets.PlaceholderIconTex, 1f, 1f);
			}
			else if (hyperlink.thing != null)
			{
				Widgets.ThingIcon(rect2, hyperlink.thing, 1f, null, false, 1f, false);
			}
			else
			{
				Widgets.DefIcon(rect2, hyperlink.def, null, 1f, null, true, null, null, null, 1f);
			}
			float num = rect2.xMax + textOffsetX;
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width - num, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Color textColor = color ?? Widgets.NormalOptionColor;
			if (hyperlink.IsHidden)
			{
				textColor = Color.gray;
			}
			Widgets.ButtonText(rect3, truncateLabel ? text2.Truncate(rect3.width, null) : text2, false, false, textColor, false, null);
			if (Widgets.ButtonInvisible(rect3, true))
			{
				hyperlink.ActivateHyperlink();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			Widgets.EndGroup();
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x00193B64 File Offset: 0x00191D64
		public static void DrawNumberOnMap(Vector2 screenPos, int number, Color textColor)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			string text = number.ToStringCached();
			float val = Text.CalcSize(text).x + 8f;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 15f, Math.Max(40f, val), 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			GUI.color = textColor;
			Widgets.Label(rect, text);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x00193BF0 File Offset: 0x00191DF0
		public static void DrawStringOnMap(Vector2 screenPos, string str, Color textColor)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			float num = Text.CalcSize(str).x + 8f;
			Rect rect = new Rect(screenPos.x - num / 2f, screenPos.y - 15f, num, 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			GUI.color = textColor;
			Widgets.Label(rect, str);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x00193C6D File Offset: 0x00191E6D
		public static void Checkbox(Vector2 topLeft, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Widgets.Checkbox(topLeft.x, topLeft.y, ref checkOn, size, disabled, paintable, texChecked, texUnchecked);
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x00193C8C File Offset: 0x00191E8C
		public static void Checkbox(float x, float y, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Rect rect = new Rect(x, y, size, size);
			Widgets.CheckboxDraw(x, y, checkOn, disabled, size, texChecked, texUnchecked);
			if (!disabled)
			{
				Widgets.ToggleInvisibleDraggable(rect, ref checkOn, true, paintable);
			}
			if (disabled)
			{
				GUI.color = Color.white;
			}
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x00193CE0 File Offset: 0x00191EE0
		public static void CheckboxLabeled(Rect rect, string label, ref bool checkOn, bool disabled = false, Texture2D texChecked = null, Texture2D texUnchecked = null, bool placeCheckboxNearText = false, bool paintable = false)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (placeCheckboxNearText)
			{
				rect.width = Mathf.Min(rect.width, Text.CalcSize(label).x + 24f + 10f);
			}
			Rect rect2 = rect;
			rect2.xMax -= 24f;
			Widgets.Label(rect2, label);
			if (!disabled)
			{
				Widgets.ToggleInvisibleDraggable(rect, ref checkOn, true, paintable);
			}
			Widgets.CheckboxDraw(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, checkOn, disabled, 24f, texChecked, texUnchecked);
			Text.Anchor = anchor;
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x00193D98 File Offset: 0x00191F98
		public static void ToggleInvisibleDraggable(Rect rect, ref bool checkOn, bool doMouseoverSound = false, bool paintable = false)
		{
			Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect, doMouseoverSound);
			bool flag = false;
			if (draggableResult == Widgets.DraggableResult.Pressed)
			{
				checkOn = !checkOn;
				flag = true;
			}
			else if (draggableResult == Widgets.DraggableResult.Dragged && paintable)
			{
				checkOn = !checkOn;
				flag = true;
				Widgets.checkboxPainting = true;
				Widgets.checkboxPaintingState = checkOn;
			}
			if (paintable && Mouse.IsOver(rect) && Widgets.checkboxPainting && Input.GetMouseButton(0) && checkOn != Widgets.checkboxPaintingState)
			{
				checkOn = Widgets.checkboxPaintingState;
				flag = true;
			}
			if (doMouseoverSound && flag)
			{
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					return;
				}
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x00193E28 File Offset: 0x00192028
		public static bool CheckboxLabeledSelectable(Rect rect, string label, ref bool selected, ref bool checkOn, Texture2D labelIcon = null, float labelIconScale = 1f)
		{
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (labelIcon != null)
			{
				Rect outerRect = new Rect(rect.x, rect.y, (float)labelIcon.width, rect.height);
				rect.xMin += (float)labelIcon.width;
				Widgets.DrawTextureFitted(outerRect, labelIcon, labelIconScale, 1f);
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			bool flag = selected;
			Rect butRect = rect;
			butRect.width -= 24f;
			if (!selected && Widgets.ButtonInvisible(butRect, true))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				selected = true;
			}
			Color color = GUI.color;
			GUI.color = Color.white;
			Widgets.CheckboxDraw(rect.xMax - 24f, rect.y, checkOn, false, 24f, null, null);
			GUI.color = color;
			if (Widgets.ButtonInvisible(new Rect(rect.xMax - 24f, rect.y, 24f, 24f), true))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			return selected && !flag;
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x00193F68 File Offset: 0x00192168
		public static Texture2D GetCheckboxTexture(bool state)
		{
			if (state)
			{
				return Widgets.CheckboxOnTex;
			}
			return Widgets.CheckboxOffTex;
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x00193F78 File Offset: 0x00192178
		public static void CheckboxDraw(float x, float y, bool active, bool disabled, float size = 24f, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Color color = GUI.color;
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Texture2D image;
			if (active)
			{
				image = ((texChecked != null) ? texChecked : Widgets.CheckboxOnTex);
			}
			else
			{
				image = ((texUnchecked != null) ? texUnchecked : Widgets.CheckboxOffTex);
			}
			GUI.DrawTexture(new Rect(x, y, size, size), image);
			if (disabled)
			{
				GUI.color = color;
			}
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x00193FE0 File Offset: 0x001921E0
		public static MultiCheckboxState CheckboxMulti(Rect rect, MultiCheckboxState state, bool paintable = false)
		{
			Texture2D tex;
			if (state == MultiCheckboxState.On)
			{
				tex = Widgets.CheckboxOnTex;
			}
			else if (state == MultiCheckboxState.Off)
			{
				tex = Widgets.CheckboxOffTex;
			}
			else
			{
				tex = Widgets.CheckboxPartialTex;
			}
			MouseoverSounds.DoRegion(rect);
			MultiCheckboxState multiCheckboxState = (state == MultiCheckboxState.Off) ? MultiCheckboxState.On : MultiCheckboxState.Off;
			bool flag = false;
			Widgets.DraggableResult draggableResult = Widgets.ButtonImageDraggable(rect, tex);
			if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
			{
				Widgets.checkboxPainting = true;
				Widgets.checkboxPaintingState = (multiCheckboxState == MultiCheckboxState.On);
				flag = true;
			}
			else if (draggableResult.AnyPressed())
			{
				flag = true;
			}
			else if (paintable && Widgets.checkboxPainting && Mouse.IsOver(rect))
			{
				multiCheckboxState = (Widgets.checkboxPaintingState ? MultiCheckboxState.On : MultiCheckboxState.Off);
				if (state != multiCheckboxState)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (multiCheckboxState == MultiCheckboxState.On)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				return multiCheckboxState;
			}
			return state;
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x0019408E File Offset: 0x0019228E
		public static bool RadioButton(Vector2 topLeft, bool chosen, bool disabled = false)
		{
			return Widgets.RadioButton(topLeft.x, topLeft.y, chosen, disabled);
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x001940A3 File Offset: 0x001922A3
		public static bool RadioButton(float x, float y, bool chosen, bool disabled = false)
		{
			Rect butRect = new Rect(x, y, 24f, 24f);
			Widgets.RadioButtonDraw(x, y, chosen, disabled);
			bool flag = Widgets.ButtonInvisible(butRect, true);
			if (flag && !chosen)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			return flag;
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x001940D8 File Offset: 0x001922D8
		public static bool RadioButtonLabeled(Rect rect, string labelText, bool chosen, bool disabled = false)
		{
			using (new TextBlock(TextAnchor.MiddleLeft, disabled ? ColoredText.SubtleGrayColor : Color.white))
			{
				Widgets.Label(rect, labelText);
			}
			bool flag = Widgets.ButtonInvisible(rect, true);
			if (flag && !chosen && !disabled)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Widgets.RadioButtonDraw(rect.x + rect.width - 24f, rect.y + rect.height / 2f - 12f, chosen, disabled);
			return flag;
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x00194178 File Offset: 0x00192378
		private static void RadioButtonDraw(float x, float y, bool chosen, bool disabled)
		{
			Color color = GUI.color;
			GUI.color = Color.white;
			Texture2D image;
			if (chosen)
			{
				image = Widgets.RadioButOnTex;
			}
			else
			{
				image = Widgets.RadioButOffTex;
			}
			Rect position = new Rect(x, y, 24f, 24f);
			if (disabled)
			{
				GUI.color = Color.gray;
			}
			GUI.DrawTexture(position, image);
			GUI.color = color;
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x001941CE File Offset: 0x001923CE
		public static bool ButtonText(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = true, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, overrideTextAnchor);
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x001941E2 File Offset: 0x001923E2
		public static bool ButtonText(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, textColor, active, false, overrideTextAnchor).AnyPressed();
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x001941F9 File Offset: 0x001923F9
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = false, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonTextDraggable(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, overrideTextAnchor);
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x0019420D File Offset: 0x0019240D
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true, TextAnchor? overrideTextAnchor = null)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, true, overrideTextAnchor);
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x00194224 File Offset: 0x00192424
		public static void DrawButtonGraphic(Rect rect)
		{
			Texture2D atlas = Widgets.ButtonBGAtlas;
			if (Mouse.IsOver(rect))
			{
				atlas = Widgets.ButtonBGAtlasMouseover;
				if (Input.GetMouseButton(0))
				{
					atlas = Widgets.ButtonBGAtlasClick;
				}
			}
			Widgets.DrawAtlas(rect, atlas);
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x0019425C File Offset: 0x0019245C
		private static Widgets.DraggableResult ButtonTextWorker(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active, bool draggable, TextAnchor? overrideTextAnchor = null)
		{
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (drawBackground)
			{
				Widgets.DrawButtonGraphic(rect);
			}
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (!drawBackground)
			{
				GUI.color = textColor;
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
			}
			if (overrideTextAnchor != null)
			{
				Text.Anchor = overrideTextAnchor.Value;
			}
			else if (drawBackground)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleLeft;
			}
			bool wordWrap = Text.WordWrap;
			if (rect.height < Text.LineHeight * 2f)
			{
				Text.WordWrap = false;
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			Text.WordWrap = wordWrap;
			if (active && draggable)
			{
				return Widgets.ButtonInvisibleDraggable(rect, false);
			}
			if (!active)
			{
				return Widgets.DraggableResult.Idle;
			}
			if (!Widgets.ButtonInvisible(rect, false))
			{
				return Widgets.DraggableResult.Idle;
			}
			return Widgets.DraggableResult.Pressed;
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x00194323 File Offset: 0x00192523
		public static void DrawRectFast(Rect position, Color color, GUIContent content = null)
		{
			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUI.Box(position, content ?? GUIContent.none, TexUI.FastFillStyle);
			GUI.backgroundColor = backgroundColor;
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x0019434C File Offset: 0x0019254C
		public static bool CustomButtonText(ref Rect rect, string label, Color bgColor, Color textColor, Color borderColor, Color unfilledBgColor = default(Color), bool cacheHeight = false, float borderSize = 1f, bool doMouseoverSound = true, bool active = true, float fillPercent = 1f)
		{
			if (cacheHeight)
			{
				Widgets.LabelCacheHeight(ref rect, label, false, false);
			}
			Rect position = new Rect(rect);
			position.x += borderSize;
			position.y += borderSize;
			position.width -= borderSize * 2f;
			position.height -= borderSize * 2f;
			Widgets.DrawRectFast(rect, borderColor, null);
			if (unfilledBgColor != default(Color))
			{
				Widgets.DrawRectFast(position, unfilledBgColor, null);
			}
			position.width *= fillPercent;
			Widgets.DrawRectFast(position, bgColor, null);
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			GUI.color = textColor;
			if (Mouse.IsOver(rect))
			{
				GUI.color = Widgets.MouseoverOptionColor;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			return active && Widgets.ButtonInvisible(rect, false);
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x00194468 File Offset: 0x00192668
		public static bool ButtonTextSubtle(Rect rect, string label, float barPercent = 0f, float textLeftMargin = -1f, SoundDef mouseoverSound = null, Vector2 functionalSizeOffset = default(Vector2), Color? labelColor = null, bool highlight = false)
		{
			Rect rect2 = rect;
			rect2.width += functionalSizeOffset.x;
			rect2.height += functionalSizeOffset.y;
			bool flag = false;
			if (Mouse.IsOver(rect2))
			{
				flag = true;
				GUI.color = GenUI.MouseoverColor;
			}
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect2, mouseoverSound);
			}
			Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
			if (highlight)
			{
				GUI.color = Color.grey;
				Widgets.DrawBox(rect, 2, null);
			}
			GUI.color = Color.white;
			if (barPercent > 0.001f)
			{
				Widgets.FillableBar(rect.ContractedBy(1f), barPercent, Widgets.ButtonBarTex, null, false);
			}
			Rect rect3 = new Rect(rect);
			if (textLeftMargin < 0f)
			{
				textLeftMargin = rect.width * 0.15f;
			}
			rect3.x += textLeftMargin;
			if (flag)
			{
				rect3.x += 2f;
				rect3.y -= 2f;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Text.Font = GameFont.Small;
			GUI.color = (labelColor ?? Color.white);
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			return Widgets.ButtonInvisible(rect2, false);
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x001945BB File Offset: 0x001927BB
		public static bool ButtonImage(Rect butRect, Texture2D tex, bool doMouseoverSound = true, string tooltip = null)
		{
			return Widgets.ButtonImage(butRect, tex, Color.white, doMouseoverSound, tooltip);
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x001945CB File Offset: 0x001927CB
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, bool doMouseoverSound = true, string tooltip = null)
		{
			return Widgets.ButtonImage(butRect, tex, baseColor, GenUI.MouseoverColor, doMouseoverSound, tooltip);
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x001945E0 File Offset: 0x001927E0
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor, bool doMouseoverSound = true, string tooltip = null)
		{
			GUI.color = (Mouse.IsOver(butRect) ? mouseoverColor : baseColor);
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			if (!tooltip.NullOrEmpty())
			{
				TooltipHandler.TipRegion(butRect, tooltip);
			}
			bool result = Widgets.ButtonInvisible(butRect, doMouseoverSound);
			GUI.color = Color.white;
			return result;
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x00194633 File Offset: 0x00192833
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, Color.white);
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x00194641 File Offset: 0x00192841
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x00194650 File Offset: 0x00192850
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			return Widgets.ButtonInvisibleDraggable(butRect, false);
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x0019467C File Offset: 0x0019287C
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageFitted(butRect, tex, Color.white);
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x0019468A File Offset: 0x0019288A
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageFitted(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x00194699 File Offset: 0x00192899
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			Widgets.DrawTextureFitted(butRect, tex, 1f, 1f);
			GUI.color = baseColor;
			return Widgets.ButtonInvisible(butRect, true);
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x001946D0 File Offset: 0x001928D0
		public static bool ButtonImageWithBG(Rect butRect, Texture2D image, Vector2? imageSize = null)
		{
			bool result = Widgets.ButtonText(butRect, "", true, true, true, null);
			Rect position;
			if (imageSize != null)
			{
				position = new Rect(Mathf.Floor(butRect.x + butRect.width / 2f - imageSize.Value.x / 2f), Mathf.Floor(butRect.y + butRect.height / 2f - imageSize.Value.y / 2f), imageSize.Value.x, imageSize.Value.y);
			}
			else
			{
				position = butRect;
			}
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x00194784 File Offset: 0x00192984
		public static bool CloseButtonFor(Rect rectToClose)
		{
			return Widgets.ButtonImage(new Rect(rectToClose.x + rectToClose.width - 18f - 4f, rectToClose.y + 4f, 18f, 18f), TexButton.CloseXSmall, true, null);
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x001947D4 File Offset: 0x001929D4
		public static bool BackButtonFor(Rect rectToBack)
		{
			return Widgets.ButtonText(new Rect(rectToBack.x + rectToBack.width - 18f - 4f - 120f - 16f, rectToBack.y + 18f, 120f, 40f), "Back".Translate(), true, true, true, null);
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x00194844 File Offset: 0x00192A44
		public static bool ButtonInvisible(Rect butRect, bool doMouseoverSound = true)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			return GUI.Button(butRect, "", Widgets.EmptyStyle);
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x00194860 File Offset: 0x00192A60
		public static Widgets.DraggableResult ButtonInvisibleDraggable(Rect butRect, bool doMouseoverSound = false)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			int controlID = GUIUtility.GetControlID(FocusType.Passive, butRect);
			if (Input.GetMouseButtonDown(0) && Mouse.IsOver(butRect))
			{
				GUIUtility.keyboardControl = 0;
				Widgets.buttonInvisibleDraggable_activeControl = controlID;
				Widgets.buttonInvisibleDraggable_mouseStart = Input.mousePosition;
				Widgets.buttonInvisibleDraggable_dragged = false;
			}
			if (Widgets.buttonInvisibleDraggable_activeControl == controlID)
			{
				if (Input.GetMouseButtonUp(0))
				{
					Widgets.buttonInvisibleDraggable_activeControl = 0;
					if (!Mouse.IsOver(butRect))
					{
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged)
					{
						return Widgets.DraggableResult.Pressed;
					}
					return Widgets.DraggableResult.DraggedThenPressed;
				}
				else
				{
					if (!Input.GetMouseButton(0))
					{
						Widgets.buttonInvisibleDraggable_activeControl = 0;
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged && (Widgets.buttonInvisibleDraggable_mouseStart - Input.mousePosition).sqrMagnitude > Widgets.DragStartDistanceSquared)
					{
						Widgets.buttonInvisibleDraggable_dragged = true;
						return Widgets.DraggableResult.Dragged;
					}
				}
			}
			return Widgets.DraggableResult.Idle;
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x0000D0F2 File Offset: 0x0000B2F2
		public static string TextField(Rect rect, string text)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextField(rect, text, Text.CurTextFieldStyle);
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x00194914 File Offset: 0x00192B14
		public static string TextField(Rect rect, string text, int maxLength, Regex inputValidator = null)
		{
			string text2 = Widgets.TextField(rect, text);
			if (text2.Length <= maxLength && (inputValidator == null || inputValidator.IsMatch(text2)))
			{
				return text2;
			}
			return text;
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x00194941 File Offset: 0x00192B41
		public static string TextArea(Rect rect, string text, bool readOnly = false)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextArea(rect, text, readOnly ? Text.CurTextAreaReadOnlyStyle : Text.CurTextAreaStyle);
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x00194964 File Offset: 0x00192B64
		public static string TextEntryLabeled(Rect rect, string label, string text)
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			if (rect.height <= 30f)
			{
				return Widgets.TextField(rect3, text);
			}
			return Widgets.TextArea(rect3, text, false);
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x001949C0 File Offset: 0x00192BC0
		public static string DelayedTextField(Rect rect, string text, ref string buffer, string previousFocusedControlName, string controlName = null)
		{
			controlName = (controlName ?? string.Format("TextField{0},{1}", rect.x, rect.y));
			bool flag = previousFocusedControlName == controlName;
			bool flag2 = GUI.GetNameOfFocusedControl() == controlName;
			string text2 = controlName + "_unfocused";
			GUI.SetNextControlName(text2);
			GUI.Label(rect, "");
			GUI.SetNextControlName(controlName);
			bool flag3 = false;
			if (flag2 && Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter))
			{
				Event.current.Use();
				flag3 = true;
			}
			bool flag4 = false;
			if (Event.current.type == EventType.MouseDown && !rect.Contains(Event.current.mousePosition))
			{
				flag4 = true;
			}
			if (!flag)
			{
				buffer = Widgets.TextField(rect, text);
				return buffer;
			}
			buffer = Widgets.TextField(rect, buffer);
			if (!flag2)
			{
				return buffer;
			}
			if (flag4 || flag3)
			{
				GUI.FocusControl(text2);
				return buffer;
			}
			return text;
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x00194AC0 File Offset: 0x00192CC0
		public static void TextFieldVector(Rect rect, ref Vector3 vector, ref string[] buffer, float min = 0f, float max = 1E+09f)
		{
			if (buffer == null)
			{
				buffer = new string[3];
			}
			float width = rect.width / 3f - 4f;
			Rect rect2 = rect.LeftPartPixels(width);
			Rect rect3 = rect2;
			Rect rect4 = rect3;
			rect3.x = rect2.xMax + 4f;
			rect4.x = rect3.xMax + 4f;
			Widgets.TextFieldNumeric<float>(rect2, ref vector.x, ref buffer[0], min, max);
			Widgets.TextFieldNumeric<float>(rect3, ref vector.y, ref buffer[1], min, max);
			Widgets.TextFieldNumeric<float>(rect4, ref vector.z, ref buffer[2], min, max);
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x00194B68 File Offset: 0x00192D68
		public static void TextFieldNumeric<T>(Rect rect, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			if (buffer == null)
			{
				buffer = val.ToString();
			}
			GUI.SetNextControlName("TextField" + rect.y.ToString("F0") + rect.x.ToString("F0"));
			string text = Widgets.TextField(rect, buffer);
			if (text != buffer && Widgets.IsPartiallyOrFullyTypedNumber<T>(ref val, text, min, max))
			{
				buffer = text;
				if (text.IsFullyTypedNumber<T>())
				{
					Widgets.ResolveParseNow<T>(text, ref val, ref buffer, min, max, false);
				}
			}
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x00194BF4 File Offset: 0x00192DF4
		private static void ResolveParseNow<T>(string edited, ref T val, ref string buffer, float min, float max, bool force)
		{
			if (typeof(T) == typeof(int))
			{
				if (edited.NullOrEmpty())
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
				int num;
				if (int.TryParse(edited, out num))
				{
					val = (T)((object)Mathf.RoundToInt(Mathf.Clamp((float)num, min, max)));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else if (typeof(T) == typeof(float))
			{
				float value;
				if (float.TryParse(edited, out value))
				{
					val = (T)((object)Mathf.Clamp(value, min, max));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else
			{
				string str = "TextField<T> does not support ";
				Type typeFromHandle = typeof(T);
				Log.Error(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null));
			}
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x00194CFC File Offset: 0x00192EFC
		private static void ResetValue<T>(string edited, ref T val, ref string buffer, float min, float max)
		{
			val = default(T);
			if (min > 0f)
			{
				val = (T)((object)Mathf.RoundToInt(min));
			}
			if (max < 0f)
			{
				val = (T)((object)Mathf.RoundToInt(max));
			}
			buffer = Widgets.ToStringTypedIn<T>(val);
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x00194D5C File Offset: 0x00192F5C
		private static string ToStringTypedIn<T>(T val)
		{
			if (typeof(T) == typeof(float))
			{
				return ((float)((object)val)).ToString("0.##########");
			}
			return val.ToString();
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x00194DAC File Offset: 0x00192FAC
		private static bool IsPartiallyOrFullyTypedNumber<T>(ref T val, string s, float min, float max)
		{
			return s == "" || ((s[0] != '-' || min < 0f) && (s.Length <= 1 || s[s.Length - 1] != '-') && !(s == "00") && s.Length <= 12 && ((typeof(T) == typeof(float) && s.CharacterCount('.') <= 1 && s.ContainsOnlyCharacters("-.0123456789")) || s.IsFullyTypedNumber<T>()));
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x00194E58 File Offset: 0x00193058
		private static bool IsFullyTypedNumber<T>(this string s)
		{
			if (s == "")
			{
				return false;
			}
			if (typeof(T) == typeof(float))
			{
				string[] array = s.Split('.', StringSplitOptions.None);
				if (array.Length > 2 || array.Length < 1)
				{
					return false;
				}
				if (!array[0].ContainsOnlyCharacters("-0123456789"))
				{
					return false;
				}
				if (array.Length == 2 && (array[1].Length == 0 || !array[1].ContainsOnlyCharacters("0123456789")))
				{
					return false;
				}
			}
			return !(typeof(T) == typeof(int)) || s.ContainsOnlyCharacters("-0123456789");
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x00194F04 File Offset: 0x00193104
		private static bool ContainsOnlyCharacters(this string s, string allowedChars)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!allowedChars.Contains(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x00194F34 File Offset: 0x00193134
		private static int CharacterCount(this string s, char c)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == c)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x00194F64 File Offset: 0x00193164
		public static void TextFieldNumericLabeled<T>(Rect rect, string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			Widgets.TextFieldNumeric<T>(rect3, ref val, ref buffer, min, max);
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x00194FAC File Offset: 0x001931AC
		public static void TextFieldPercent(Rect rect, ref float val, ref string buffer, float min = 0f, float max = 1f)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 25f, rect.height);
			Widgets.Label(new Rect(rect2.xMax, rect.y, 25f, rect2.height), "%");
			float num = val * 100f;
			Widgets.TextFieldNumeric<float>(rect2, ref num, ref buffer, min * 100f, max * 100f);
			val = num / 100f;
			if (val > max)
			{
				val = max;
				buffer = val.ToString();
			}
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x00195048 File Offset: 0x00193248
		public static T ChangeType<T>(this object obj)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (T)((object)Convert.ChangeType(obj, typeof(T), invariantCulture));
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x00195071 File Offset: 0x00193271
		public static void ResetSliderDraggingIDs()
		{
			Widgets.sliderDraggingID = 0;
			Widgets.draggingId = 0;
			Widgets.curDragEnd = Widgets.RangeEnd.None;
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x00195088 File Offset: 0x00193288
		public static void HorizontalSlider(Rect rect, ref float value, FloatRange range, string label = null, float roundTo = -1f)
		{
			float trueMin = range.TrueMin;
			float trueMax = range.TrueMax;
			value = Widgets.HorizontalSlider(rect, value, trueMin, trueMax, false, label, trueMin.ToString(), trueMax.ToString(), roundTo);
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x001950C4 File Offset: 0x001932C4
		public static float HorizontalSlider(Rect rect, float value, float min, float max, bool middleAlignment = false, string label = null, string leftAlignedLabel = null, string rightAlignedLabel = null, float roundTo = -1f)
		{
			float num = value;
			if (middleAlignment || !label.NullOrEmpty())
			{
				rect.y += Mathf.Round((rect.height - 10f) / 2f);
			}
			if (!label.NullOrEmpty())
			{
				rect.y += 5f;
			}
			int num2 = UI.GUIToScreenPoint(new Vector2(rect.x, rect.y)).GetHashCode();
			num2 = Gen.HashCombine<float>(num2, rect.width);
			num2 = Gen.HashCombine<float>(num2, rect.height);
			num2 = Gen.HashCombine<float>(num2, min);
			num2 = Gen.HashCombine<float>(num2, max);
			Rect rect2 = rect;
			rect2.xMin += 6f;
			rect2.xMax -= 6f;
			GUI.color = Widgets.RangeControlTextColor;
			Rect rect3 = new Rect(rect2.x, rect2.y + 2f, rect2.width, 8f);
			Widgets.DrawAtlas(rect3, Widgets.SliderRailAtlas);
			GUI.color = Color.white;
			float x = Mathf.Clamp(rect2.x - 6f + rect2.width * Mathf.InverseLerp(min, max, num), rect2.xMin - 6f, rect2.xMax - 6f);
			GUI.DrawTexture(new Rect(x, rect3.center.y - 6f, 12f, 12f), Widgets.SliderHandle);
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect) && Widgets.sliderDraggingID != num2)
			{
				Widgets.sliderDraggingID = num2;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Event.current.Use();
			}
			if (Widgets.sliderDraggingID == num2 && UnityGUIBugsFixer.MouseDrag(0))
			{
				num = Mathf.Clamp((Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min, min, max);
				if (Event.current.type == EventType.MouseDrag)
				{
					Event.current.Use();
				}
			}
			if (!label.NullOrEmpty() || !leftAlignedLabel.NullOrEmpty() || !rightAlignedLabel.NullOrEmpty())
			{
				TextAnchor anchor = Text.Anchor;
				GameFont font = Text.Font;
				Text.Font = GameFont.Small;
				float num3 = label.NullOrEmpty() ? 18f : Text.CalcSize(label).y;
				rect.y = rect.y - num3 + 3f;
				if (!leftAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect, leftAlignedLabel);
				}
				if (!rightAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect, rightAlignedLabel);
				}
				if (!label.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, label);
				}
				Text.Anchor = anchor;
				Text.Font = font;
			}
			if (roundTo > 0f)
			{
				num = (float)Mathf.RoundToInt(num / roundTo) * roundTo;
			}
			if (value != num)
			{
				Widgets.CheckPlayDragSliderSound();
			}
			return num;
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x001953B4 File Offset: 0x001935B4
		public static float FrequencyHorizontalSlider(Rect rect, float freq, float minFreq, float maxFreq, bool roundToInt = false)
		{
			float num;
			if (freq < 1f)
			{
				float x = 1f / freq;
				num = GenMath.LerpDouble(1f, 1f / minFreq, 0.5f, 1f, x);
			}
			else
			{
				num = GenMath.LerpDouble(maxFreq, 1f, 0f, 0.5f, freq);
			}
			string label;
			if (freq == 1f)
			{
				label = "EveryDay".Translate();
			}
			else if (freq < 1f)
			{
				label = "TimesPerDay".Translate((1f / freq).ToString("0.##"));
			}
			else
			{
				label = "EveryDays".Translate(freq.ToString("0.##"));
			}
			float num2 = Widgets.HorizontalSlider(rect, num, 0f, 1f, true, label, null, null, -1f);
			if (num != num2)
			{
				float num3;
				if (num2 < 0.5f)
				{
					num3 = GenMath.LerpDouble(0.5f, 0f, 1f, maxFreq, num2);
					if (roundToInt)
					{
						num3 = Mathf.Round(num3);
					}
				}
				else
				{
					float num4 = GenMath.LerpDouble(1f, 0.5f, 1f / minFreq, 1f, num2);
					if (roundToInt)
					{
						num4 = Mathf.Round(num4);
					}
					num3 = 1f / num4;
				}
				freq = num3;
			}
			return freq;
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x00195500 File Offset: 0x00193700
		public static void IntEntry(Rect rect, ref int value, ref string editBuffer, int multiplier = 1)
		{
			int num = Mathf.Min(Widgets.IntEntryButtonWidth, (int)rect.width / 5);
			if (Widgets.ButtonText(new Rect(rect.xMin, rect.yMin, (float)num, rect.height), (-10 * multiplier).ToStringCached(), true, true, true, null))
			{
				value -= 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMin + (float)num, rect.yMin, (float)num, rect.height), (-1 * multiplier).ToStringCached(), true, true, true, null))
			{
				value -= multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)num, rect.yMin, (float)num, rect.height), "+" + (10 * multiplier).ToStringCached(), true, true, true, null))
			{
				value += 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)(num * 2), rect.yMin, (float)num, rect.height), "+" + multiplier.ToStringCached(), true, true, true, null))
			{
				value += multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			Widgets.TextFieldNumeric<int>(new Rect(rect.xMin + (float)(num * 2), rect.yMin, rect.width - (float)(num * 4), rect.height), ref value, ref editBuffer, 0f, 1E+09f);
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x001956E0 File Offset: 0x001938E0
		public static void FloatRange(Rect rect, int id, ref FloatRange range, float min = 0f, float max = 1f, string labelKey = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo, float gap = 0f, GameFont sliderLabelFont = GameFont.Small, Color? sliderLabelColor = null, float roundTo = 0f)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = (sliderLabelColor ?? Widgets.RangeControlTextColor);
			string text = range.min.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute) + " - " + range.max.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute);
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = sliderLabelFont;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			rect3.height = Mathf.Max(rect3.height, Text.CalcHeight(text, rect3.width));
			Widgets.LabelFit(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			float num = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.min);
			float num2 = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.max);
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(num, rect2.yMax - 8f - 2f, num2 - num, 4f), BaseContent.WhiteTex);
			float num3 = num;
			float num4 = num2;
			Rect position2 = new Rect(num3 - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num4 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Event.current.Use();
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num5 = Mathf.Abs(x - position2.xMax);
						float num6 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num5 < num6) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && UnityGUIBugsFixer.MouseDrag(0)))
				{
					float num7 = (Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min;
					num7 = Mathf.Clamp(num7, min, max);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num7 != range.min)
						{
							range.min = Mathf.Min(num7, max - gap);
							if (range.max < range.min + gap)
							{
								range.max = range.min + gap;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num7 != range.max)
					{
						range.max = Mathf.Max(num7, min + gap);
						if (range.min > range.max - gap)
						{
							range.min = range.max - gap;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					if (roundTo != 0f)
					{
						range.min = Mathf.Round(range.min / roundTo) * roundTo;
						range.max = Mathf.Round(range.max / roundTo) * roundTo;
					}
					if (Event.current.type == EventType.MouseDrag)
					{
						Event.current.Use();
					}
				}
			}
			Text.Font = font;
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x00195B30 File Offset: 0x00193D30
		public static void IntRange(Rect rect, int id, ref IntRange range, int min = 0, int max = 100, string labelKey = null, int minWidth = 0)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string text = range.min.ToStringCached() + " - " + range.max.ToStringCached();
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			float num = rect2.x + rect2.width * (float)(range.min - min) / (float)(max - min);
			float num2 = rect2.x + rect2.width * (float)(range.max - min) / (float)(max - min);
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(num, rect2.yMax - 8f - 2f, num2 - num, 4f), BaseContent.WhiteTex);
			float num3 = num;
			float num4 = num2;
			Rect position2 = new Rect(num3 - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num4 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num5 = Mathf.Abs(x - position2.xMax);
						float num6 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num5 < num6) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && UnityGUIBugsFixer.MouseDrag(0)))
				{
					int num7 = Mathf.RoundToInt(Mathf.Clamp((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(max - min) + (float)min, (float)min, (float)max));
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num7 != range.min)
						{
							range.min = num7;
							if (range.min > max - minWidth)
							{
								range.min = max - minWidth;
							}
							int num8 = Mathf.Max(min, range.min + minWidth);
							if (range.max < num8)
							{
								range.max = num8;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num7 != range.max)
					{
						range.max = num7;
						if (range.max < min + minWidth)
						{
							range.max = min + minWidth;
						}
						int num9 = Mathf.Min(max, range.max - minWidth);
						if (range.min > num9)
						{
							range.min = num9;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					if (Event.current.type == EventType.MouseDrag)
					{
						Event.current.Use();
					}
				}
			}
			Text.Font = font;
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x00195F2A File Offset: 0x0019412A
		private static void CheckPlayDragSliderSound()
		{
			if (Time.realtimeSinceStartup > Widgets.lastDragSliderSoundTime + 0.075f)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Widgets.lastDragSliderSoundTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x00195F54 File Offset: 0x00194154
		public static void QualityRange(Rect rect, int id, ref QualityRange range)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string label;
			if (range == RimWorld.QualityRange.All)
			{
				label = "AnyQuality".Translate();
			}
			else if (range.max == range.min)
			{
				label = "OnlyQuality".Translate(range.min.GetLabel());
			}
			else
			{
				label = range.min.GetLabel() + " - " + range.max.GetLabel();
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			int qualityCount = QualityUtility.QualityCount;
			float num = rect2.x + rect2.width / (float)(qualityCount - 1) * (float)range.min;
			float num2 = rect2.x + rect2.width / (float)(qualityCount - 1) * (float)range.max;
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(num, rect2.yMax - 8f - 2f, num2 - num, 4f), BaseContent.WhiteTex);
			float num3 = num;
			float num4 = num2;
			Rect position2 = new Rect(num3 - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num4 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || id == Widgets.draggingId)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num5 = Mathf.Abs(x - position2.xMax);
						float num6 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num5 < num6) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && UnityGUIBugsFixer.MouseDrag(0)))
				{
					int num7 = Mathf.RoundToInt((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(qualityCount - 1));
					num7 = Mathf.Clamp(num7, 0, qualityCount - 1);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (range.min != (QualityCategory)num7)
						{
							range.min = (QualityCategory)num7;
							if (range.max < range.min)
							{
								range.max = range.min;
							}
							SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && range.max != (QualityCategory)num7)
					{
						range.max = (QualityCategory)num7;
						if (range.min > range.max)
						{
							range.min = range.max;
						}
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					}
					if (Event.current.type == EventType.MouseDrag)
					{
						Event.current.Use();
					}
				}
			}
			Text.Font = font;
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x0019635C File Offset: 0x0019455C
		public static void FloatRangeWithTypeIn(Rect rect, int id, ref FloatRange fRange, float sliderMin = 0f, float sliderMax = 1f, ToStringStyle valueStyle = ToStringStyle.FloatTwo, string labelKey = null)
		{
			Rect rect2 = new Rect(rect);
			rect2.width = rect.width / 4f;
			Rect rect3 = new Rect(rect);
			rect3.width = rect.width / 2f;
			rect3.x = rect.x + rect.width / 4f;
			rect3.height = rect.height / 2f;
			rect3.width -= rect.height;
			Rect butRect = new Rect(rect3);
			butRect.x = rect3.xMax;
			butRect.height = rect.height;
			butRect.width = rect.height;
			Rect rect4 = new Rect(rect);
			rect4.x = rect.x + rect.width * 0.75f;
			rect4.width = rect.width / 4f;
			rect3.y += 4f;
			rect3.height += 4f;
			Widgets.FloatRange(rect3, id, ref fRange, sliderMin, sliderMax, labelKey, valueStyle, 0f, GameFont.Small, null, 0f);
			if (Widgets.ButtonImage(butRect, TexButton.RangeMatch, true, null))
			{
				fRange.max = fRange.min;
			}
			float.TryParse(Widgets.TextField(rect2, fRange.min.ToString()), out fRange.min);
			float.TryParse(Widgets.TextField(rect4, fRange.max.ToString()), out fRange.max);
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x001964EF File Offset: 0x001946EF
		public static Rect FillableBar(Rect rect, float fillPercent)
		{
			return Widgets.FillableBar(rect, fillPercent, Widgets.BarFullTexHor);
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x00196500 File Offset: 0x00194700
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex)
		{
			bool doBorder = rect.height > 15f && rect.width > 20f;
			return Widgets.FillableBar(rect, fillPercent, fillTex, Widgets.DefaultBarBgTex, doBorder);
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x0019653C File Offset: 0x0019473C
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex, Texture2D bgTex, bool doBorder)
		{
			if (doBorder)
			{
				GUI.DrawTexture(rect, BaseContent.BlackTex);
				rect = rect.ContractedBy(3f);
			}
			if (bgTex != null)
			{
				GUI.DrawTexture(rect, bgTex);
			}
			Rect result = rect;
			rect.width *= fillPercent;
			GUI.DrawTexture(rect, fillTex);
			return result;
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x0019658C File Offset: 0x0019478C
		public static void FillableBarLabeled(Rect rect, float fillPercent, int labelWidth, string label)
		{
			if (fillPercent < 0f)
			{
				fillPercent = 0f;
			}
			if (fillPercent > 1f)
			{
				fillPercent = 1f;
			}
			Rect rect2 = rect;
			rect2.width = (float)labelWidth;
			Widgets.Label(rect2, label);
			Rect rect3 = rect;
			rect3.x += (float)labelWidth;
			rect3.width -= (float)labelWidth;
			Widgets.FillableBar(rect3, fillPercent);
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x001965F4 File Offset: 0x001947F4
		public static void FillableBarChangeArrows(Rect barRect, float changeRate)
		{
			int changeRate2 = (int)(changeRate * Widgets.FillableBarChangeRateDisplayRatio);
			Widgets.FillableBarChangeArrows(barRect, changeRate2);
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x00196614 File Offset: 0x00194814
		public static void FillableBarChangeArrows(Rect barRect, int changeRate)
		{
			if (changeRate == 0)
			{
				return;
			}
			if (changeRate > Widgets.MaxFillableBarChangeRate)
			{
				changeRate = Widgets.MaxFillableBarChangeRate;
			}
			if (changeRate < -Widgets.MaxFillableBarChangeRate)
			{
				changeRate = -Widgets.MaxFillableBarChangeRate;
			}
			float num = barRect.height;
			if (num > 16f)
			{
				num = 16f;
			}
			int num2 = Mathf.Abs(changeRate);
			float y = barRect.y + barRect.height / 2f - num / 2f;
			float num3;
			float num4;
			Texture2D image;
			if (changeRate > 0)
			{
				num3 = barRect.x + barRect.width + 2f;
				num4 = 8f;
				image = Widgets.FillArrowTexRight;
			}
			else
			{
				num3 = barRect.x - 8f - 2f;
				num4 = -8f;
				image = Widgets.FillArrowTexLeft;
			}
			for (int i = 0; i < num2; i++)
			{
				GUI.DrawTexture(new Rect(num3, y, 8f, num), image);
				num3 += num4;
			}
		}

		// Token: 0x06004F3A RID: 20282 RVA: 0x001966F5 File Offset: 0x001948F5
		public static void DrawWindowBackground(Rect rect)
		{
			GUI.color = Widgets.WindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.WindowBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x00196728 File Offset: 0x00194928
		public static void DrawWindowBackground(Rect rect, Color colorFactor)
		{
			Color color = GUI.color;
			GUI.color = Widgets.WindowBGFillColor * colorFactor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.WindowBGBorderColor * colorFactor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = color;
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x00196767 File Offset: 0x00194967
		public static void DrawMenuSection(Rect rect)
		{
			GUI.color = Widgets.MenuSectionBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.MenuSectionBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x0019679A File Offset: 0x0019499A
		public static void DrawWindowBackgroundTutor(Rect rect)
		{
			GUI.color = Widgets.TutorWindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.TutorWindowBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x001967CD File Offset: 0x001949CD
		public static void DrawOptionUnselected(Rect rect)
		{
			GUI.color = Widgets.OptionUnselectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionUnselectedBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x00196800 File Offset: 0x00194A00
		public static void DrawOptionSelected(Rect rect)
		{
			GUI.color = Widgets.OptionSelectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionSelectedBGBorderColor;
			Widgets.DrawBox(rect.ExpandedBy(3f), 3, null);
			GUI.color = Color.white;
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x0019683D File Offset: 0x00194A3D
		public static void DrawOptionBackground(Rect rect, bool selected)
		{
			if (selected)
			{
				Widgets.DrawOptionSelected(rect);
			}
			else
			{
				Widgets.DrawOptionUnselected(rect);
			}
			Widgets.DrawHighlightIfMouseover(rect);
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x00196858 File Offset: 0x00194A58
		public static void DrawShadowAround(Rect rect)
		{
			Rect rect2 = rect.ContractedBy(-9f);
			rect2.x += 2f;
			rect2.y += 2f;
			Widgets.DrawAtlas(rect2, Widgets.ShadowAtlas);
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x001968A2 File Offset: 0x00194AA2
		public static void DrawAtlas(Rect rect, Texture2D atlas)
		{
			Widgets.DrawAtlas(rect, atlas, true);
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x001968AC File Offset: 0x00194AAC
		public static void DrawAtlas(Rect rect, Texture2D atlas, bool drawTop)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			rect.x = Mathf.Round(rect.x);
			rect.y = Mathf.Round(rect.y);
			rect.width = Mathf.Round(rect.width);
			rect.height = Mathf.Round(rect.height);
			rect = UIScaling.AdjustRectToUIScaling(rect);
			float num = (float)atlas.width * 0.25f;
			num = UIScaling.AdjustCoordToUIScalingCeil(GenMath.Min(num, rect.height / 2f, rect.width / 2f));
			Rect drawRect;
			if (drawTop)
			{
				drawRect = new Rect(rect.x, rect.y, num, num);
				Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_TopLeft, atlas);
				drawRect = new Rect(rect.x + rect.width - num, rect.y, num, num);
				Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_TopRight, atlas);
			}
			drawRect = new Rect(rect.x, rect.y + rect.height - num, num, num);
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_BottomLeft, atlas);
			drawRect = new Rect(rect.x + rect.width - num, rect.y + rect.height - num, num, num);
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_BottomRight, atlas);
			drawRect = new Rect(rect.x + num, rect.y + num, rect.width - num * 2f, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Center, atlas);
			if (drawTop)
			{
				drawRect = new Rect(rect.x + num, rect.y, rect.width - num * 2f, num);
				Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Top, atlas);
			}
			drawRect = new Rect(rect.x + num, rect.y + rect.height - num, rect.width - num * 2f, num);
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Bottom, atlas);
			drawRect = new Rect(rect.x, rect.y + num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Left, atlas);
			drawRect = new Rect(rect.x + rect.width - num, rect.y + num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			Widgets.DrawTexturePart(drawRect, Widgets.AtlasUV_Right, atlas);
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x00196B90 File Offset: 0x00194D90
		public static void DrawAtlasWithMaterial(Rect rect, Texture2D atlas, Material mat, bool drawTop = true)
		{
			rect.x = Mathf.Round(rect.x);
			rect.y = Mathf.Round(rect.y);
			rect.width = Mathf.Round(rect.width);
			rect.height = Mathf.Round(rect.height);
			rect = UIScaling.AdjustRectToUIScaling(rect);
			float num = (float)atlas.width * 0.25f;
			num = UIScaling.AdjustCoordToUIScalingCeil(GenMath.Min(num, rect.height / 2f, rect.width / 2f));
			Widgets.BeginGroup(rect);
			Rect rect2;
			if (drawTop)
			{
				rect2 = new Rect(0f, 0f, num, num);
				GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_TopLeft);
				rect2 = new Rect(rect.width - num, 0f, num, num);
				GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_TopRight);
			}
			rect2 = new Rect(0f, rect.height - num, num, num);
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_BottomLeft);
			rect2 = new Rect(rect.width - num, rect.height - num, num, num);
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_BottomRight);
			rect2 = new Rect(num, num, rect.width - num * 2f, rect.height - num * 2f);
			if (!drawTop)
			{
				rect2.height += num;
				rect2.y -= num;
			}
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Center);
			if (drawTop)
			{
				rect2 = new Rect(num, 0f, rect.width - num * 2f, num);
				GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Top);
			}
			rect2 = new Rect(num, rect.height - num, rect.width - num * 2f, num);
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Bottom);
			rect2 = new Rect(0f, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				rect2.height += num;
				rect2.y -= num;
			}
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Left);
			rect2 = new Rect(rect.width - num, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				rect2.height += num;
				rect2.y -= num;
			}
			GenUI.DrawTexturePartWithMaterial(rect2, atlas, mat, Widgets.AtlasUV_Right);
			Widgets.EndGroup();
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x00196E0D File Offset: 0x0019500D
		public static Rect ToUVRect(this Rect r, Vector2 texSize)
		{
			return new Rect(r.x / texSize.x, r.y / texSize.y, r.width / texSize.x, r.height / texSize.y);
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x00196E4C File Offset: 0x0019504C
		public static void DrawTexturePart(Rect drawRect, Rect uvRect, Texture2D tex)
		{
			uvRect.y = 1f - uvRect.y - uvRect.height;
			GUI.DrawTextureWithTexCoords(drawRect, tex, uvRect);
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x00196E74 File Offset: 0x00195074
		public static void ScrollHorizontal(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, float ScrollWheelSpeed = 20f)
		{
			if (Event.current.type == EventType.ScrollWheel && Mouse.IsOver(outRect))
			{
				scrollPosition.x += Event.current.delta.y * ScrollWheelSpeed;
				float num = 0f;
				float num2 = viewRect.width - outRect.width + 16f;
				if (scrollPosition.x < num)
				{
					scrollPosition.x = num;
				}
				if (scrollPosition.x > num2)
				{
					scrollPosition.x = num2;
				}
				Event.current.Use();
			}
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x00196EF8 File Offset: 0x001950F8
		public static void BeginScrollView(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, bool showScrollbars = true)
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Widgets.mouseOverScrollViewStack.Push(Widgets.mouseOverScrollViewStack.Peek() && outRect.Contains(Event.current.mousePosition));
			}
			else
			{
				Widgets.mouseOverScrollViewStack.Push(outRect.Contains(Event.current.mousePosition));
			}
			SteamDeck.HandleTouchScreenScrollViewScroll(outRect, ref scrollPosition);
			if (showScrollbars)
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect);
			}
			else
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);
			}
			UnityGUIBugsFixer.Notify_BeginScrollView();
		}

		// Token: 0x06004F49 RID: 20297 RVA: 0x00196F9C File Offset: 0x0019519C
		public static void AdjustRectsForScrollView(Rect parentRect, ref Rect outRect, ref Rect viewRect)
		{
			if (viewRect.height >= outRect.height)
			{
				viewRect.width -= 20f;
				outRect.xMax -= 4f;
				outRect.yMin = Mathf.Max(parentRect.yMin + 6f, outRect.yMin);
				outRect.yMax = Mathf.Min(parentRect.yMax - 6f, outRect.yMax);
			}
		}

		// Token: 0x06004F4A RID: 20298 RVA: 0x00197017 File Offset: 0x00195217
		public static void EndScrollView()
		{
			Widgets.mouseOverScrollViewStack.Pop();
			GUI.EndScrollView();
		}

		// Token: 0x06004F4B RID: 20299 RVA: 0x00197029 File Offset: 0x00195229
		public static void EnsureMousePositionStackEmpty()
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Log.Error("Mouse position stack is not empty. There were more calls to BeginScrollView than EndScrollView. Fixing.");
				Widgets.mouseOverScrollViewStack.Clear();
			}
		}

		// Token: 0x06004F4C RID: 20300 RVA: 0x0019704C File Offset: 0x0019524C
		public static void ColorSelectorIcon(Rect rect, Texture icon, Color color, bool drawColor = false)
		{
			if (icon != null)
			{
				GUI.color = color;
				GUI.DrawTexture(rect, icon);
				GUI.color = Color.white;
				return;
			}
			if (drawColor)
			{
				Widgets.DrawBoxSolid(rect, color);
			}
		}

		// Token: 0x06004F4D RID: 20301 RVA: 0x0019707C File Offset: 0x0019527C
		public static bool ColorBox(Rect rect, ref Color color, Color boxColor, int colorSize = 22, int colorPadding = 2, Action<Color, Rect> extraOnGUI = null)
		{
			Widgets.DrawLightHighlight(rect);
			Widgets.DrawHighlightIfMouseover(rect);
			if (color.IndistinguishableFrom(boxColor))
			{
				Widgets.DrawBox(rect, 1, null);
			}
			Rect rect2 = new Rect(rect.x + (float)colorPadding, rect.y + (float)colorPadding, (float)colorSize, (float)colorSize);
			Widgets.DrawBoxSolid(rect2, boxColor);
			if (extraOnGUI != null)
			{
				extraOnGUI(boxColor, rect2);
			}
			bool result = false;
			if (Widgets.ButtonInvisible(rect, true))
			{
				result = true;
				color = boxColor;
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			}
			return result;
		}

		// Token: 0x06004F4E RID: 20302 RVA: 0x00197100 File Offset: 0x00195300
		public static bool ColorSelector(Rect rect, ref Color color, List<Color> colors, out float height, Texture icon = null, int colorSize = 22, int colorPadding = 2, Action<Color, Rect> extraOnGUI = null)
		{
			height = 0f;
			bool result = false;
			int num = colorSize + colorPadding * 2;
			float num2 = (icon != null) ? ((float)(colorSize * 4) + 10f) : 0f;
			int num3 = Mathf.FloorToInt((rect.width - num2 + (float)colorPadding) / (float)(num + colorPadding));
			int num4 = Mathf.CeilToInt((float)colors.Count / (float)num3);
			Widgets.BeginGroup(rect);
			Widgets.ColorSelectorIcon(new Rect(5f, 5f, (float)(colorSize * 4), (float)(colorSize * 4)), icon, color, false);
			for (int i = 0; i < colors.Count; i++)
			{
				int num5 = i / num3;
				int num6 = i % num3;
				float num7 = (icon != null) ? ((num2 - (float)(num * num4) - (float)colorPadding) / 2f) : 0f;
				Rect rect2 = new Rect(num2 + (float)(num6 * num) + (float)(num6 * colorPadding), num7 + (float)(num5 * num) + (float)(num5 * colorPadding), (float)num, (float)num);
				if (Widgets.ColorBox(rect2, ref color, colors[i], colorSize, colorPadding, extraOnGUI))
				{
					result = true;
				}
				height = Mathf.Max(height, rect2.yMax);
			}
			Widgets.EndGroup();
			return result;
		}

		// Token: 0x06004F4F RID: 20303 RVA: 0x00197238 File Offset: 0x00195438
		private static void DrawColorSelectionCircle(Rect hsvColorWheelRect, Vector2Int center, Color color)
		{
			int num = (int)Mathf.Round(hsvColorWheelRect.width * 0.125f);
			GUI.DrawTexture(new Rect((float)(center.x - num / 2), (float)(center.y - num / 2), (float)num, (float)num), Widgets.ColorSelectionCircle, ScaleMode.ScaleToFit, true, 1f, color, 0f, 0f);
		}

		// Token: 0x06004F50 RID: 20304 RVA: 0x00197296 File Offset: 0x00195496
		private static bool ClickedInsideRect(Rect rect)
		{
			return Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition);
		}

		// Token: 0x06004F51 RID: 20305 RVA: 0x001972B8 File Offset: 0x001954B8
		public static void HSVColorWheel(Rect rect, ref Color color, ref bool currentlyDragging, float? colorValueOverride = null, string controlName = null)
		{
			if (rect.width != rect.height)
			{
				throw new ArgumentException("HSV color wheel must be drawn in a square rect.");
			}
			float num;
			float d;
			float num2;
			Color.RGBToHSV(color, out num, out d, out num2);
			float num3 = colorValueOverride ?? num2;
			GUI.DrawTexture(rect, Widgets.HSVColorWheelTex, ScaleMode.ScaleToFit, true, 1f, Color.HSVToRGB(0f, 0f, num3), 0f, 0f);
			num = (num + 0.25f) * 2f * 3.1415927f;
			Vector2 a = new Vector2(Mathf.Cos(num), -Mathf.Sin(num)) * d * rect.width / 2f;
			Widgets.DrawColorSelectionCircle(rect, Vector2Int.RoundToInt(a + rect.center), (num3 > 0.5f) ? Color.black : Color.white);
			if (!currentlyDragging)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (Event.current.isMouse && Event.current.button == 0)
			{
				if (currentlyDragging && Event.current.type == EventType.MouseUp)
				{
					currentlyDragging = false;
					return;
				}
				if (Widgets.ClickedInsideRect(rect) | currentlyDragging)
				{
					GUI.FocusControl(controlName);
					currentlyDragging = true;
					Vector2 vector = (Event.current.mousePosition - rect.center) / (rect.size / 2f);
					float num4 = Mathf.Atan2(-vector.y, vector.x) / 6.2831855f;
					num4 += 1.75f;
					num4 %= 1f;
					float s = Mathf.Clamp01(vector.magnitude);
					color = Color.HSVToRGB(num4, s, num3);
					Event.current.Use();
				}
			}
		}

		// Token: 0x06004F52 RID: 20306 RVA: 0x00197480 File Offset: 0x00195680
		public static void ColorTemperatureBar(Rect rect, ref Color color, ref bool dragging, float? colorValueOverride = null)
		{
			float num = colorValueOverride ?? Mathf.Max(new float[]
			{
				color.r,
				color.g,
				color.b
			});
			float? num2 = color.ColorTemperature();
			string label = ((num2 != null) ? num2.GetValueOrDefault().ToString("0.K") : null) ?? "";
			RectDivider rect2 = new RectDivider(rect, 661493905, new Vector2?(new Vector2(17f, 0f)));
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				string text = "ColorTemperature".Translate().CapitalizeFirst();
				Widgets.Label(rect2.NewCol(Text.CalcSize(text).x, HorizontalJustification.Left, null), text);
				Widgets.Label(rect2.NewCol(Text.CalcSize("XXXXXK").x, HorizontalJustification.Left, null), label);
			}
			if (!dragging)
			{
				TooltipHandler.TipRegion(rect, "ColorTemperatureTooltip".Translate());
				MouseoverSounds.DoRegion(rect);
			}
			if (Event.current.button == 0)
			{
				if (dragging && Event.current.type == EventType.MouseUp)
				{
					dragging = false;
				}
				else if (Widgets.ClickedInsideRect(rect2) || (dragging && UnityGUIBugsFixer.MouseDrag(0)))
				{
					dragging = true;
					if (Event.current.type == EventType.MouseDrag)
					{
						Event.current.Use();
					}
					float fraction = Mathf.Clamp01((Event.current.mousePosition.x - rect2.Rect.xMin) / rect2.Rect.width);
					num2 = new float?(GenMath.ExponentialWarpInterpolation(1000f, 40000f, fraction, new Vector2(0.5f, 6600f)));
					color = GenColor.FromColorTemperature(num2.Value);
					color *= num;
				}
			}
			rect2.NewRow(6f, VerticalJustification.Top, null);
			rect2.NewRow(6f, VerticalJustification.Bottom, null);
			GUI.DrawTexture(rect2, Widgets.ColorTemperatureExp, ScaleMode.StretchToFill, true, 1f, Color.HSVToRGB(0f, 0f, num), 0f, 0f);
			if (num2 != null)
			{
				float num3 = rect2.Rect.width * GenMath.InverseExponentialWarpInterpolation(1000f, 40000f, num2.Value, new Vector2(0.5f, 6600f));
				Rect position = new Rect(rect2.Rect.x + num3 - 6f, rect2.Rect.y - 6f, 12f, 12f);
				Rect position2 = new Rect(rect2.Rect.x + num3 - 6f, rect2.Rect.yMax - 6f, 12f, 12f);
				GUI.DrawTextureWithTexCoords(position, Widgets.SelectionArrow, new Rect(0f, 1f, 1f, -1f), true);
				GUI.DrawTextureWithTexCoords(position2, Widgets.SelectionArrow, new Rect(0f, 0f, 1f, 1f), true);
			}
		}

		// Token: 0x06004F53 RID: 20307 RVA: 0x0019782C File Offset: 0x00195A2C
		private static int ToIntegerRange(float fraction, int min, int max)
		{
			return Mathf.Clamp(Mathf.RoundToInt(fraction * (float)max), min, max);
		}

		// Token: 0x06004F54 RID: 20308 RVA: 0x00197840 File Offset: 0x00195A40
		public static bool ColorTextfields(ref RectAggregator aggregator, ref Color color, ref string[] buffers, ref Color colorBuffer, string previousFocusedControlName, string controlName = null, Widgets.ColorComponents editable = Widgets.ColorComponents.All, Widgets.ColorComponents visible = Widgets.ColorComponents.All)
		{
			if (visible == Widgets.ColorComponents.None)
			{
				return false;
			}
			if ((~(visible != Widgets.ColorComponents.None) & editable) != Widgets.ColorComponents.None)
			{
				throw new ArgumentException(string.Format("Cannot have editable but invisible components {0}.", ~visible & editable));
			}
			controlName = (controlName ?? string.Format("ColorTextfields{0}{1}", aggregator.Rect.x, aggregator.Rect.y));
			bool flag = previousFocusedControlName != null && previousFocusedControlName.StartsWith(controlName);
			bool flag2 = GUI.GetNameOfFocusedControl().StartsWith(controlName);
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				float num = 30f;
				float num2 = 0f;
				for (int i = 0; i < Widgets.colorComponentLabels.Length; i++)
				{
					Widgets.tmpTranslatedColorComponentLabels[i] = Widgets.colorComponentLabels[i].Translate().CapitalizeFirst();
					num = Mathf.Max(num, Widgets.tmpTranslatedColorComponentLabels[i].GetHeightCached());
					num2 = Mathf.Max(num2, Widgets.tmpTranslatedColorComponentLabels[i].GetWidthCached());
				}
				float fraction;
				float fraction2;
				float fraction3;
				Color.RGBToHSV(colorBuffer, out fraction, out fraction2, out fraction3);
				Widgets.intColorComponents[0] = Widgets.ToIntegerRange(colorBuffer.r, 0, Widgets.maxColorComponentValues[0]);
				Widgets.intColorComponents[1] = Widgets.ToIntegerRange(colorBuffer.g, 0, Widgets.maxColorComponentValues[1]);
				Widgets.intColorComponents[2] = Widgets.ToIntegerRange(colorBuffer.b, 0, Widgets.maxColorComponentValues[2]);
				Widgets.intColorComponents[3] = Widgets.ToIntegerRange(fraction, 0, Widgets.maxColorComponentValues[3]);
				Widgets.intColorComponents[4] = Widgets.ToIntegerRange(fraction2, 0, Widgets.maxColorComponentValues[4]);
				Widgets.intColorComponents[5] = Widgets.ToIntegerRange(fraction3, 0, Widgets.maxColorComponentValues[5]);
				for (int j = 0; j <= 5; j++)
				{
					Widgets.ColorComponents colorComponents = (Widgets.ColorComponents)(1 << j);
					if ((visible & colorComponents) != Widgets.ColorComponents.None)
					{
						RectDivider rect = aggregator.NewRow(num, VerticalJustification.Bottom);
						Widgets.Label(rect.NewCol(num2, HorizontalJustification.Left, null), Widgets.tmpTranslatedColorComponentLabels[j]);
						if ((editable & colorComponents) == Widgets.ColorComponents.None)
						{
							Widgets.Label(rect, Widgets.intColorComponents[j].ToString());
						}
						else
						{
							string text = Widgets.intColorComponents[j].ToString();
							string text2 = Widgets.DelayedTextField(rect, text, ref buffers[j], previousFocusedControlName, string.Format("{0}_{1}", controlName, j));
							int num3;
							if (text != text2 && int.TryParse(text2, out num3))
							{
								Widgets.intColorComponents[j] = num3;
								if (j < 3)
								{
									colorBuffer = new ColorInt(Widgets.intColorComponents[0], Widgets.intColorComponents[1], Widgets.intColorComponents[2]).ToColor;
								}
								else
								{
									colorBuffer = Color.HSVToRGB((float)Widgets.intColorComponents[3] / 360f, (float)Widgets.intColorComponents[4] / 100f, (float)Widgets.intColorComponents[5] / 100f);
								}
							}
						}
					}
				}
			}
			if (flag)
			{
				if (!flag2)
				{
					color = colorBuffer;
					return true;
				}
			}
			else
			{
				colorBuffer = color;
			}
			return false;
		}

		// Token: 0x06004F55 RID: 20309 RVA: 0x0000D1C0 File Offset: 0x0000B3C0
		public static void DrawHighlightSelected(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
		}

		// Token: 0x06004F56 RID: 20310 RVA: 0x00197B80 File Offset: 0x00195D80
		public static void DrawHighlightIfMouseover(Rect rect)
		{
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
		}

		// Token: 0x06004F57 RID: 20311 RVA: 0x0000D1DD File Offset: 0x0000B3DD
		public static void DrawHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightTex);
		}

		// Token: 0x06004F58 RID: 20312 RVA: 0x00197B90 File Offset: 0x00195D90
		public static void DrawHighlight(Rect rect, float opacity)
		{
			GUI.color = Color.white.ToTransparent(opacity);
			GUI.DrawTexture(rect, TexUI.HighlightTex);
			GUI.color = Color.white;
		}

		// Token: 0x06004F59 RID: 20313 RVA: 0x00197BB7 File Offset: 0x00195DB7
		public static void DrawLightHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.LightHighlight);
		}

		// Token: 0x06004F5A RID: 20314 RVA: 0x00197BC4 File Offset: 0x00195DC4
		public static void DrawStrongHighlight(Rect rect, Color? color = null)
		{
			Color color2 = GUI.color;
			GUI.color = color.GetValueOrDefault(Widgets.HighlightStrongBgColor);
			Widgets.DrawAtlas(rect, TexUI.RectHighlight);
			GUI.color = color2;
		}

		// Token: 0x06004F5B RID: 20315 RVA: 0x00197BEC File Offset: 0x00195DEC
		public static void DrawTextHighlight(Rect rect, float expandBy = 4f, Color? color = null)
		{
			rect.y -= expandBy;
			rect.height += expandBy * 2f;
			Color color2 = GUI.color;
			GUI.color = color.GetValueOrDefault(Widgets.HighlightTextBgColor);
			Widgets.DrawAtlas(rect, TexUI.RectHighlight);
			GUI.color = color2;
		}

		// Token: 0x06004F5C RID: 20316 RVA: 0x00197C43 File Offset: 0x00195E43
		public static void DrawTitleBG(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.TitleBGTex);
		}

		// Token: 0x06004F5D RID: 20317 RVA: 0x00197C50 File Offset: 0x00195E50
		public static bool InfoCardButton(float x, float y, Thing thing)
		{
			IConstructible constructible = thing as IConstructible;
			if (constructible != null)
			{
				ThingDef thingDef = thing.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					return Widgets.InfoCardButton(x, y, thingDef, constructible.EntityToBuildStuff());
				}
				return Widgets.InfoCardButton(x, y, thing.def.entityDefToBuild);
			}
			else
			{
				if (Widgets.InfoCardButtonWorker(x, y))
				{
					Find.WindowStack.Add(new Dialog_InfoCard(thing, null));
					return true;
				}
				return false;
			}
		}

		// Token: 0x06004F5E RID: 20318 RVA: 0x00197CBA File Offset: 0x00195EBA
		public static bool InfoCardButton(float x, float y, Def def)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, null));
				return true;
			}
			return false;
		}

		// Token: 0x06004F5F RID: 20319 RVA: 0x00197CD9 File Offset: 0x00195ED9
		public static bool InfoCardButton(Rect rect, Def def)
		{
			if (Widgets.InfoCardButtonWorker(rect))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, null));
				return true;
			}
			return false;
		}

		// Token: 0x06004F60 RID: 20320 RVA: 0x00197CF7 File Offset: 0x00195EF7
		public static bool InfoCardButton(float x, float y, Def def, Precept_ThingStyle precept)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, precept));
				return true;
			}
			return false;
		}

		// Token: 0x06004F61 RID: 20321 RVA: 0x00197D16 File Offset: 0x00195F16
		public static bool InfoCardButton(float x, float y, ThingDef thingDef, ThingDef stuffDef)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(thingDef, stuffDef, null));
				return true;
			}
			return false;
		}

		// Token: 0x06004F62 RID: 20322 RVA: 0x00197D36 File Offset: 0x00195F36
		public static bool InfoCardButton(float x, float y, WorldObject worldObject)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(worldObject));
				return true;
			}
			return false;
		}

		// Token: 0x06004F63 RID: 20323 RVA: 0x00197D54 File Offset: 0x00195F54
		public static bool InfoCardButton(Rect rect, Hediff hediff)
		{
			if (Widgets.InfoCardButtonWorker(rect))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(hediff));
				return true;
			}
			return false;
		}

		// Token: 0x06004F64 RID: 20324 RVA: 0x00197D71 File Offset: 0x00195F71
		public static bool InfoCardButton(float x, float y, Faction faction)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(faction));
				return true;
			}
			return false;
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x00197D8F File Offset: 0x00195F8F
		public static bool InfoCardButtonCentered(Rect rect, Thing thing)
		{
			return Widgets.InfoCardButton(rect.center.x - 12f, rect.center.y - 12f, thing);
		}

		// Token: 0x06004F66 RID: 20326 RVA: 0x00197DBB File Offset: 0x00195FBB
		public static bool InfoCardButtonCentered(Rect rect, Faction faction)
		{
			return Widgets.InfoCardButton(rect.center.x - 12f, rect.center.y - 12f, faction);
		}

		// Token: 0x06004F67 RID: 20327 RVA: 0x00197DE7 File Offset: 0x00195FE7
		private static bool InfoCardButtonWorker(float x, float y)
		{
			return Widgets.InfoCardButtonWorker(new Rect(x, y, 24f, 24f));
		}

		// Token: 0x06004F68 RID: 20328 RVA: 0x00197DFF File Offset: 0x00195FFF
		private static bool InfoCardButtonWorker(Rect rect)
		{
			MouseoverSounds.DoRegion(rect);
			TooltipHandler.TipRegionByKey(rect, "DefInfoTip");
			bool result = Widgets.ButtonImage(rect, TexButton.Info, GUI.color, true, null);
			UIHighlighter.HighlightOpportunity(rect, "InfoCard");
			return result;
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x00197E2F File Offset: 0x0019602F
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, float alpha = 1f)
		{
			Widgets.DrawTextureFitted(outerRect, tex, scale, new Vector2((float)tex.width, (float)tex.height), new Rect(0f, 0f, 1f, 1f), 0f, null, alpha);
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x00197E6C File Offset: 0x0019606C
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, Material mat, float alpha = 1f)
		{
			Widgets.DrawTextureFitted(outerRect, tex, scale, new Vector2((float)tex.width, (float)tex.height), new Rect(0f, 0f, 1f, 1f), 0f, mat, alpha);
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x00197EAC File Offset: 0x001960AC
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, Vector2 texProportions, Rect texCoords, float angle = 0f, Material mat = null, float alpha = 1f)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			Rect rect = new Rect(0f, 0f, texProportions.x, texProportions.y);
			float num;
			if (rect.width / rect.height < outerRect.width / outerRect.height)
			{
				num = outerRect.height / rect.height;
			}
			else
			{
				num = outerRect.width / rect.width;
			}
			num *= scale;
			rect.width *= num;
			rect.height *= num;
			rect.x = outerRect.x + outerRect.width / 2f - rect.width / 2f;
			rect.y = outerRect.y + outerRect.height / 2f - rect.height / 2f;
			Matrix4x4 matrix = Matrix4x4.identity;
			if (angle != 0f)
			{
				matrix = GUI.matrix;
				UI.RotateAroundPivot(angle, rect.center);
			}
			Color color = Color.white;
			if (!Mathf.Approximately(alpha, 1f))
			{
				Color color2;
				color = (color2 = GUI.color);
				color2.a *= alpha;
				GUI.color = color2;
			}
			GenUI.DrawTextureWithMaterial(rect, tex, mat, texCoords);
			if (angle != 0f)
			{
				GUI.matrix = matrix;
			}
			if (!Mathf.Approximately(alpha, 1f))
			{
				GUI.color = color;
			}
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x00198020 File Offset: 0x00196220
		public static void DrawTextureRotated(Vector2 center, Texture tex, float angle, float scale = 1f, Material material = null)
		{
			float num = (float)tex.width * scale;
			float num2 = (float)tex.height * scale;
			Widgets.DrawTextureRotated(new Rect(center.x - num / 2f, center.y - num2 / 2f, num, num2), tex, angle, material);
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x00198070 File Offset: 0x00196270
		public static void DrawTextureRotated(Rect rect, Texture tex, float angle, Material material = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (angle != 0f)
			{
				Matrix4x4 matrix = GUI.matrix;
				UI.RotateAroundPivot(angle, rect.center);
				if (material == null)
				{
					GUI.DrawTexture(rect, tex);
				}
				else
				{
					GenUI.DrawTextureWithMaterial(rect, tex, material, default(Rect));
				}
				GUI.matrix = matrix;
				return;
			}
			if (material == null)
			{
				GUI.DrawTexture(rect, tex);
				return;
			}
			GenUI.DrawTextureWithMaterial(rect, tex, material, default(Rect));
		}

		// Token: 0x06004F6E RID: 20334 RVA: 0x001980F0 File Offset: 0x001962F0
		public static void NoneLabel(float y, float width, string customLabel = null)
		{
			Widgets.NoneLabel(ref y, width, customLabel);
		}

		// Token: 0x06004F6F RID: 20335 RVA: 0x001980FC File Offset: 0x001962FC
		public static void NoneLabel(ref float curY, float width, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(0f, curY, width, 30f), customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			curY += 25f;
			GUI.color = Color.white;
		}

		// Token: 0x06004F70 RID: 20336 RVA: 0x0019815E File Offset: 0x0019635E
		public static void NoneLabelCenteredVertically(Rect rect, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x0019819C File Offset: 0x0019639C
		public static void DraggableBar(Rect barRect, Texture2D barTexture, Texture2D barHighlightTexture, Texture2D emptyBarTex, Texture2D dragBarTex, ref bool draggingBar, float barValue, ref float targetValue, IEnumerable<float> bandPercentages = null, int increments = 20, float min = 0f, float max = 1f)
		{
			bool flag = Mouse.IsOver(barRect);
			Widgets.FillableBar(barRect, Mathf.Min(barValue, 1f), flag ? barHighlightTexture : barTexture, emptyBarTex, true);
			if (bandPercentages != null)
			{
				foreach (float percent in bandPercentages)
				{
					Widgets.DrawDraggableBarThreshold(barRect, percent, barValue);
				}
			}
			float num = Mathf.Clamp(Mathf.Round((Event.current.mousePosition.x - barRect.x) / barRect.width * (float)increments) / (float)increments, min, max);
			Event current = Event.current;
			if (current.type == EventType.MouseDown && current.button == 0 && flag)
			{
				targetValue = num;
				draggingBar = true;
				current.Use();
			}
			if ((UnityGUIBugsFixer.MouseDrag(0) & draggingBar) && flag)
			{
				if (Math.Abs(num - targetValue) > 1E-45f)
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				targetValue = num;
				if (Event.current.type == EventType.MouseDrag)
				{
					current.Use();
				}
			}
			if ((current.type == EventType.MouseUp && current.button == 0) & draggingBar)
			{
				draggingBar = false;
				current.Use();
			}
			Widgets.DrawDraggableBarTarget(barRect, draggingBar ? num : targetValue, dragBarTex);
			GUI.color = Color.white;
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x001982F4 File Offset: 0x001964F4
		private static void DrawDraggableBarThreshold(Rect rect, float percent, float curValue)
		{
			Rect position = new Rect
			{
				x = rect.x + 3f + (rect.width - 8f) * percent,
				y = rect.y + rect.height - 9f,
				width = 2f,
				height = 6f
			};
			if (curValue < percent)
			{
				GUI.DrawTexture(position, BaseContent.GreyTex);
				return;
			}
			GUI.DrawTexture(position, BaseContent.BlackTex);
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x00198380 File Offset: 0x00196580
		private static void DrawDraggableBarTarget(Rect rect, float percent, Texture2D targetTex)
		{
			float num = Mathf.Round((rect.width - 8f) * percent);
			GUI.DrawTexture(new Rect
			{
				x = rect.x + 3f + num,
				y = rect.y,
				width = 2f,
				height = rect.height
			}, targetTex);
			float num2 = UIScaling.AdjustCoordToUIScalingFloor(rect.x + 2f + num);
			float xMax = UIScaling.AdjustCoordToUIScalingCeil(num2 + 4f);
			Rect rect2 = new Rect
			{
				y = rect.y - 3f,
				height = 5f,
				xMin = num2,
				xMax = xMax
			};
			GUI.DrawTexture(rect2, targetTex);
			Rect position = rect2;
			position.y = rect.yMax - 2f;
			GUI.DrawTexture(position, targetTex);
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x00198470 File Offset: 0x00196670
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false)
		{
			Widgets.Dropdown<Target, Payload>(rect, target, Color.white, getPayload, menuGenerator, buttonLabel, buttonIcon, dragLabel, dragIcon, dropdownOpened, paintable, null);
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x001984A0 File Offset: 0x001966A0
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Color iconColor, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false, float? contractButtonIcon = null)
		{
			MouseoverSounds.DoRegion(rect);
			Widgets.DraggableResult draggableResult;
			if (buttonIcon != null)
			{
				Widgets.DrawHighlightIfMouseover(rect);
				GUI.color = iconColor;
				Rect rect2 = rect;
				if (contractButtonIcon != null)
				{
					rect2 = rect2.ContractedBy(contractButtonIcon.Value);
				}
				Widgets.DrawTextureFitted(rect2, buttonIcon, 1f, 1f);
				GUI.color = Color.white;
				draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
			}
			else
			{
				draggableResult = Widgets.ButtonTextDraggable(rect, buttonLabel, true, false, true, null);
			}
			if (draggableResult == Widgets.DraggableResult.Pressed)
			{
				List<FloatMenuOption> options = (from opt in menuGenerator(target)
				select opt.option).ToList<FloatMenuOption>();
				Find.WindowStack.Add(new FloatMenu(options));
				if (dropdownOpened != null)
				{
					dropdownOpened();
					return;
				}
			}
			else
			{
				if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
				{
					Widgets.dropdownPainting = true;
					Widgets.dropdownPainting_Payload = getPayload(target);
					Widgets.dropdownPainting_Type = typeof(Payload);
					Widgets.dropdownPainting_Text = ((dragLabel != null) ? dragLabel : buttonLabel);
					Widgets.dropdownPainting_Icon = ((dragIcon != null) ? dragIcon : buttonIcon);
					return;
				}
				if (paintable && Widgets.dropdownPainting && Mouse.IsOver(rect) && Widgets.dropdownPainting_Type == typeof(Payload))
				{
					FloatMenuOption floatMenuOption = (from opt in menuGenerator(target)
					where object.Equals(opt.payload, Widgets.dropdownPainting_Payload)
					select opt.option).FirstOrDefault<FloatMenuOption>();
					if (floatMenuOption != null && !floatMenuOption.Disabled)
					{
						Payload x = getPayload(target);
						floatMenuOption.action();
						Payload y = getPayload(target);
						if (!EqualityComparer<Payload>.Default.Equals(x, y))
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
					}
				}
			}
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x00198697 File Offset: 0x00196897
		public static void MouseAttachedLabel(string label, float xOffset = 0f, float yOffset = 0f, Color? colorOverride = null)
		{
			Rect rect = Widgets.CreateMouseAttachedLabelRect(label, xOffset, yOffset);
			if (colorOverride != null)
			{
				GUI.color = colorOverride.Value;
			}
			Widgets.Label(rect, label);
			GUI.color = Color.white;
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x001986C6 File Offset: 0x001968C6
		public static void MouseAttachedLabel(TaggedString label, float xOffset = 0f, float yOffset = 0f, Color? colorOverride = null)
		{
			Rect rect = Widgets.CreateMouseAttachedLabelRect(label, xOffset, yOffset);
			if (colorOverride != null)
			{
				GUI.color = colorOverride.Value;
			}
			Widgets.Label(rect, label);
			GUI.color = Color.white;
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x001986FC File Offset: 0x001968FC
		public static void WorldAttachedLabel(Vector3 worldPos, string label, float xOffset = 0f, float yOffset = 0f, Color? colorOverride = null)
		{
			Vector3 vector = Find.WorldCamera.WorldToScreenPoint(worldPos);
			vector.y = (float)Screen.height - vector.y;
			vector /= Prefs.UIScale;
			Rect rect = Widgets.CreateAttachedLabelRect(vector, label, xOffset, yOffset);
			if (colorOverride != null)
			{
				GUI.color = colorOverride.Value;
			}
			Widgets.Label(rect, label);
			GUI.color = Color.white;
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x00198768 File Offset: 0x00196968
		private static Rect CreateMouseAttachedLabelRect(string label, float xOffset, float yOffset)
		{
			return Widgets.CreateAttachedLabelRect(Event.current.mousePosition, label, xOffset, yOffset);
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x0019877C File Offset: 0x0019697C
		private static Rect CreateAttachedLabelRect(Vector2 screenPosition, string label, float xOffset, float yOffset)
		{
			Rect rect = new Rect(screenPosition.x + 8f + xOffset, screenPosition.y + 8f + yOffset, 32f, 32f);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Rect result = new Rect(rect.xMax, rect.y, 9999f, 100f);
			Vector2 vector = Text.CalcSize(label);
			result.height = Mathf.Max(result.height, vector.y);
			GUI.DrawTexture(new Rect(result.x - vector.x * 0.1f, result.y, vector.x * 1.2f, vector.y), TexUI.GrayTextBG);
			return result;
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x00198844 File Offset: 0x00196A44
		public static void WidgetsOnGUI()
		{
			if (Event.current.rawType == EventType.MouseUp || Input.GetMouseButtonUp(0))
			{
				Widgets.checkboxPainting = false;
				Widgets.dropdownPainting = false;
			}
			if (Widgets.checkboxPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.checkboxPaintingState ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
			}
			if (Widgets.dropdownPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.dropdownPainting_Icon, Widgets.dropdownPainting_Text, 0f, default(Vector2), null, null, false, default(Color), null, null);
			}
		}

		// Token: 0x0400388B RID: 14475
		public static Stack<bool> mouseOverScrollViewStack = new Stack<bool>();

		// Token: 0x0400388C RID: 14476
		public static readonly GUIStyle EmptyStyle = new GUIStyle();

		// Token: 0x0400388D RID: 14477
		[TweakValue("Input", 0f, 100f)]
		private static float DragStartDistanceSquared = 20f;

		// Token: 0x0400388E RID: 14478
		public const int LeftMouseButton = 0;

		// Token: 0x0400388F RID: 14479
		public static readonly Color InactiveColor = new Color(0.37f, 0.37f, 0.37f, 0.8f);

		// Token: 0x04003890 RID: 14480
		public static readonly Color HighlightStrongBgColor = ColorLibrary.SkyBlue;

		// Token: 0x04003891 RID: 14481
		public static readonly Color HighlightTextBgColor = Widgets.HighlightStrongBgColor.ToTransparent(0.25f);

		// Token: 0x04003892 RID: 14482
		private static readonly Texture2D DefaultBarBgTex = BaseContent.BlackTex;

		// Token: 0x04003893 RID: 14483
		public static readonly Texture2D BarFullTexHor = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x04003894 RID: 14484
		public static readonly Texture2D CheckboxOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);

		// Token: 0x04003895 RID: 14485
		public static readonly Texture2D CheckboxOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOff", true);

		// Token: 0x04003896 RID: 14486
		public static readonly Texture2D CheckboxPartialTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckPartial", true);

		// Token: 0x04003897 RID: 14487
		public const float CheckboxSize = 24f;

		// Token: 0x04003898 RID: 14488
		public const float RadioButtonSize = 24f;

		// Token: 0x04003899 RID: 14489
		public static readonly Texture2D RadioButOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOn", true);

		// Token: 0x0400389A RID: 14490
		public static readonly Texture2D HSVColorWheelTex = ContentFinder<Texture2D>.Get("UI/Widgets/HSVColorWheel", true);

		// Token: 0x0400389B RID: 14491
		public static readonly Texture2D ColorSelectionCircle = ContentFinder<Texture2D>.Get("UI/Overlays/TargetHighlight_Square", true);

		// Token: 0x0400389C RID: 14492
		public static readonly Texture2D ColorTemperatureExp = ContentFinder<Texture2D>.Get("UI/Widgets/ColorTemperatureExp", true);

		// Token: 0x0400389D RID: 14493
		public static readonly Texture2D SelectionArrow = ContentFinder<Texture2D>.Get("Things/Mote/InteractionArrow", true);

		// Token: 0x0400389E RID: 14494
		private static readonly Texture2D RadioButOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOff", true);

		// Token: 0x0400389F RID: 14495
		private static readonly Texture2D FillArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowRight", true);

		// Token: 0x040038A0 RID: 14496
		private static readonly Texture2D FillArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowLeft", true);

		// Token: 0x040038A1 RID: 14497
		public static readonly Texture2D PlaceholderIconTex = ContentFinder<Texture2D>.Get("UI/Icons/MenuOptionNoIcon", true);

		// Token: 0x040038A2 RID: 14498
		private const int FillableBarBorderWidth = 3;

		// Token: 0x040038A3 RID: 14499
		private const int MaxFillChangeArrowHeight = 16;

		// Token: 0x040038A4 RID: 14500
		private const int FillChangeArrowWidth = 8;

		// Token: 0x040038A5 RID: 14501
		public const float CloseButtonSize = 18f;

		// Token: 0x040038A6 RID: 14502
		public const float CloseButtonMargin = 4f;

		// Token: 0x040038A7 RID: 14503
		public const float BackButtonWidth = 120f;

		// Token: 0x040038A8 RID: 14504
		public const float BackButtonHeight = 40f;

		// Token: 0x040038A9 RID: 14505
		public const float BackButtonMargin = 16f;

		// Token: 0x040038AA RID: 14506
		private const float ColorHighlightCircleFraction = 0.125f;

		// Token: 0x040038AB RID: 14507
		private const float ColorTextfieldHeight = 30f;

		// Token: 0x040038AC RID: 14508
		private const float SelectionArrowSize = 12f;

		// Token: 0x040038AD RID: 14509
		private static readonly Texture2D ShadowAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/DropShadow", true);

		// Token: 0x040038AE RID: 14510
		public static readonly Texture2D ButtonBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBG", true);

		// Token: 0x040038AF RID: 14511
		private static readonly Texture2D ButtonBGAtlasMouseover = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGMouseover", true);

		// Token: 0x040038B0 RID: 14512
		public static readonly Texture2D ButtonBGAtlasClick = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGClick", true);

		// Token: 0x040038B1 RID: 14513
		private static readonly Texture2D FloatRangeSliderTex = ContentFinder<Texture2D>.Get("UI/Widgets/RangeSlider", true);

		// Token: 0x040038B2 RID: 14514
		public static readonly Texture2D LightHighlight = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.04f));

		// Token: 0x040038B3 RID: 14515
		private static readonly Rect DefaultTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x040038B4 RID: 14516
		private static readonly Rect LinkedTexCoords = new Rect(0f, 0.5f, 0.25f, 0.25f);

		// Token: 0x040038B5 RID: 14517
		[TweakValue("Input", 0f, 100f)]
		private static int IntEntryButtonWidth = 40;

		// Token: 0x040038B6 RID: 14518
		private static Texture2D LineTexAA = null;

		// Token: 0x040038B7 RID: 14519
		private static readonly Texture2D AltTexture = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x040038B8 RID: 14520
		public static readonly Color NormalOptionColor = new Color(0.8f, 0.85f, 1f);

		// Token: 0x040038B9 RID: 14521
		public static readonly Color MouseoverOptionColor = Color.yellow;

		// Token: 0x040038BA RID: 14522
		private static Dictionary<string, float> LabelCache = new Dictionary<string, float>();

		// Token: 0x040038BB RID: 14523
		private const float TileSize = 64f;

		// Token: 0x040038BC RID: 14524
		public static readonly Color SeparatorLabelColor = new Color(0.8f, 0.8f, 0.8f, 1f);

		// Token: 0x040038BD RID: 14525
		public static readonly Color SeparatorLineColor = new Color(0.3f, 0.3f, 0.3f, 1f);

		// Token: 0x040038BE RID: 14526
		private const float SeparatorLabelHeight = 20f;

		// Token: 0x040038BF RID: 14527
		public const float ListSeparatorHeight = 25f;

		// Token: 0x040038C0 RID: 14528
		private static bool checkboxPainting = false;

		// Token: 0x040038C1 RID: 14529
		private static bool checkboxPaintingState = false;

		// Token: 0x040038C2 RID: 14530
		public static readonly Texture2D ButtonSubtleAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonSubtleAtlas", true);

		// Token: 0x040038C3 RID: 14531
		private static readonly Texture2D SliderRailAtlas = ContentFinder<Texture2D>.Get("UI/Buttons/SliderRail", true);

		// Token: 0x040038C4 RID: 14532
		private static readonly Texture2D SliderHandle = ContentFinder<Texture2D>.Get("UI/Buttons/SliderHandle", true);

		// Token: 0x040038C5 RID: 14533
		private static readonly Texture2D ButtonBarTex = SolidColorMaterials.NewSolidColorTexture(TexUI.FinishedResearchColorTransparent);

		// Token: 0x040038C6 RID: 14534
		public const float ButtonSubtleDefaultMarginPct = 0.15f;

		// Token: 0x040038C7 RID: 14535
		private static int buttonInvisibleDraggable_activeControl = 0;

		// Token: 0x040038C8 RID: 14536
		private static bool buttonInvisibleDraggable_dragged = false;

		// Token: 0x040038C9 RID: 14537
		private static Vector3 buttonInvisibleDraggable_mouseStart = Vector3.zero;

		// Token: 0x040038CA RID: 14538
		private static int sliderDraggingID;

		// Token: 0x040038CB RID: 14539
		private const float SliderHandleSize = 12f;

		// Token: 0x040038CC RID: 14540
		public const float RangeControlIdealHeight = 31f;

		// Token: 0x040038CD RID: 14541
		public const float RangeControlCompactHeight = 32f;

		// Token: 0x040038CE RID: 14542
		private const float RangeSliderSize = 16f;

		// Token: 0x040038CF RID: 14543
		private static readonly Color RangeControlTextColor = new Color(0.6f, 0.6f, 0.6f);

		// Token: 0x040038D0 RID: 14544
		private static int draggingId = 0;

		// Token: 0x040038D1 RID: 14545
		private static Widgets.RangeEnd curDragEnd = Widgets.RangeEnd.None;

		// Token: 0x040038D2 RID: 14546
		private static float lastDragSliderSoundTime = -1f;

		// Token: 0x040038D3 RID: 14547
		private static float FillableBarChangeRateDisplayRatio = 100000000f;

		// Token: 0x040038D4 RID: 14548
		public static int MaxFillableBarChangeRate = 3;

		// Token: 0x040038D5 RID: 14549
		private static readonly Color WindowBGBorderColor = new ColorInt(97, 108, 122).ToColor;

		// Token: 0x040038D6 RID: 14550
		public static readonly Color WindowBGFillColor = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x040038D7 RID: 14551
		public static readonly Color MenuSectionBGFillColor = new ColorInt(42, 43, 44).ToColor;

		// Token: 0x040038D8 RID: 14552
		private static readonly Color MenuSectionBGBorderColor = new ColorInt(135, 135, 135).ToColor;

		// Token: 0x040038D9 RID: 14553
		private static readonly Color TutorWindowBGFillColor = new ColorInt(133, 85, 44).ToColor;

		// Token: 0x040038DA RID: 14554
		private static readonly Color TutorWindowBGBorderColor = new ColorInt(176, 139, 61).ToColor;

		// Token: 0x040038DB RID: 14555
		private static readonly Color OptionUnselectedBGFillColor = new Color(0.21f, 0.21f, 0.21f);

		// Token: 0x040038DC RID: 14556
		private static readonly Color OptionUnselectedBGBorderColor = Widgets.OptionUnselectedBGFillColor * 1.8f;

		// Token: 0x040038DD RID: 14557
		private static readonly Color OptionSelectedBGFillColor = new Color(0.32f, 0.28f, 0.21f);

		// Token: 0x040038DE RID: 14558
		private static readonly Color OptionSelectedBGBorderColor = Widgets.OptionSelectedBGFillColor * 1.8f;

		// Token: 0x040038DF RID: 14559
		private static readonly Rect AtlasUV_TopLeft = new Rect(0f, 0f, 0.25f, 0.25f);

		// Token: 0x040038E0 RID: 14560
		private static readonly Rect AtlasUV_TopRight = new Rect(0.75f, 0f, 0.25f, 0.25f);

		// Token: 0x040038E1 RID: 14561
		private static readonly Rect AtlasUV_BottomLeft = new Rect(0f, 0.75f, 0.25f, 0.25f);

		// Token: 0x040038E2 RID: 14562
		private static readonly Rect AtlasUV_BottomRight = new Rect(0.75f, 0.75f, 0.25f, 0.25f);

		// Token: 0x040038E3 RID: 14563
		private static readonly Rect AtlasUV_Top = new Rect(0.25f, 0f, 0.5f, 0.25f);

		// Token: 0x040038E4 RID: 14564
		private static readonly Rect AtlasUV_Bottom = new Rect(0.25f, 0.75f, 0.5f, 0.25f);

		// Token: 0x040038E5 RID: 14565
		private static readonly Rect AtlasUV_Left = new Rect(0f, 0.25f, 0.25f, 0.5f);

		// Token: 0x040038E6 RID: 14566
		private static readonly Rect AtlasUV_Right = new Rect(0.75f, 0.25f, 0.25f, 0.5f);

		// Token: 0x040038E7 RID: 14567
		private static readonly Rect AtlasUV_Center = new Rect(0.25f, 0.25f, 0.5f, 0.5f);

		// Token: 0x040038E8 RID: 14568
		private static int[] maxColorComponentValues = new int[]
		{
			255,
			255,
			255,
			360,
			100,
			100
		};

		// Token: 0x040038E9 RID: 14569
		private static string[] colorComponentLabels = new string[]
		{
			"Red",
			"Green",
			"Blue",
			"Hue",
			"Saturation",
			"ColorValue"
		};

		// Token: 0x040038EA RID: 14570
		private static string[] tmpTranslatedColorComponentLabels = new string[6];

		// Token: 0x040038EB RID: 14571
		private static int[] intColorComponents = new int[6];

		// Token: 0x040038EC RID: 14572
		public const float InfoCardButtonSize = 24f;

		// Token: 0x040038ED RID: 14573
		private static bool dropdownPainting = false;

		// Token: 0x040038EE RID: 14574
		private static object dropdownPainting_Payload = null;

		// Token: 0x040038EF RID: 14575
		private static Type dropdownPainting_Type = null;

		// Token: 0x040038F0 RID: 14576
		private static string dropdownPainting_Text = "";

		// Token: 0x040038F1 RID: 14577
		private static Texture2D dropdownPainting_Icon = null;

		// Token: 0x02000C12 RID: 3090
		public enum DraggableResult
		{
			// Token: 0x040038F3 RID: 14579
			Idle,
			// Token: 0x040038F4 RID: 14580
			Pressed,
			// Token: 0x040038F5 RID: 14581
			Dragged,
			// Token: 0x040038F6 RID: 14582
			DraggedThenPressed
		}

		// Token: 0x02000C13 RID: 3091
		private enum RangeEnd : byte
		{
			// Token: 0x040038F8 RID: 14584
			None,
			// Token: 0x040038F9 RID: 14585
			Min,
			// Token: 0x040038FA RID: 14586
			Max
		}

		// Token: 0x02000C14 RID: 3092
		[Flags]
		public enum ColorComponents
		{
			// Token: 0x040038FC RID: 14588
			Red = 1,
			// Token: 0x040038FD RID: 14589
			Green = 2,
			// Token: 0x040038FE RID: 14590
			Blue = 4,
			// Token: 0x040038FF RID: 14591
			Hue = 8,
			// Token: 0x04003900 RID: 14592
			Sat = 16,
			// Token: 0x04003901 RID: 14593
			Value = 32,
			// Token: 0x04003902 RID: 14594
			None = 0,
			// Token: 0x04003903 RID: 14595
			All = 63
		}

		// Token: 0x02000C15 RID: 3093
		public struct DropdownMenuElement<Payload>
		{
			// Token: 0x04003904 RID: 14596
			public FloatMenuOption option;

			// Token: 0x04003905 RID: 14597
			public Payload payload;
		}
	}
}
