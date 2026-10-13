using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000919 RID: 2329
	[StaticConstructorOnStartup]
	public static class DesignatorUtility
	{
		// Token: 0x06003C6E RID: 15470 RVA: 0x0013CBC4 File Offset: 0x0013ADC4
		public static T FindAllowedDesignator<T>() where T : Designator
		{
			List<DesignationCategoryDef> allDefsListForReading = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;
			GameRules rules = Current.Game.Rules;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				List<Designator> allResolvedDesignators = allDefsListForReading[i].AllResolvedDesignators;
				for (int j = 0; j < allResolvedDesignators.Count; j++)
				{
					if (rules == null || rules.DesignatorAllowed(allResolvedDesignators[j]))
					{
						T t = allResolvedDesignators[j] as T;
						if (t != null)
						{
							return t;
						}
					}
				}
			}
			Designator designator = DesignatorUtility.StandaloneDesignators.TryGetValue(typeof(T), null);
			if (designator == null)
			{
				designator = (Activator.CreateInstance(typeof(T)) as Designator);
				DesignatorUtility.StandaloneDesignators[typeof(T)] = designator;
			}
			return (T)((object)designator);
		}

		// Token: 0x06003C6F RID: 15471 RVA: 0x0013CC98 File Offset: 0x0013AE98
		public static void RenderHighlightOverSelectableCells(Designator designator, List<IntVec3> dragCells)
		{
			foreach (IntVec3 intVec in dragCells)
			{
				Vector3 position = intVec.ToVector3Shifted();
				position.y = AltitudeLayer.MetaOverlays.AltitudeFor();
				Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, DesignatorUtility.DragHighlightCellMat, 0);
			}
		}

		// Token: 0x06003C70 RID: 15472 RVA: 0x0013CD0C File Offset: 0x0013AF0C
		public static void RenderHighlightOverSelectableThings(Designator designator, List<IntVec3> dragCells)
		{
			DesignatorUtility.selectedThings.Clear();
			foreach (IntVec3 c in dragCells)
			{
				List<Thing> thingList = c.GetThingList(designator.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					if (designator.CanDesignateThing(thingList[i]).Accepted && !DesignatorUtility.selectedThings.Contains(thingList[i]))
					{
						DesignatorUtility.selectedThings.Add(thingList[i]);
						Vector3 drawPos = thingList[i].DrawPos;
						drawPos.y = AltitudeLayer.MetaOverlays.AltitudeFor();
						Graphics.DrawMesh(MeshPool.plane10, drawPos, Quaternion.identity, DesignatorUtility.DragHighlightThingMat, 0);
					}
				}
			}
			DesignatorUtility.selectedThings.Clear();
		}

		// Token: 0x06003C71 RID: 15473 RVA: 0x0013CDF8 File Offset: 0x0013AFF8
		public static void GUIDoRotationControls(float leftX, float bottomY, Rot4 rot, Action<Rot4> rotSetter)
		{
			Rect winRect = new Rect(leftX, bottomY - 90f, 200f, 90f);
			Find.WindowStack.ImmediateWindow(73095, winRect, WindowLayer.GameUI, delegate
			{
				RotationDirection rotationDirection = RotationDirection.None;
				Text.Anchor = TextAnchor.MiddleCenter;
				Text.Font = GameFont.Medium;
				Rect rect = new Rect(winRect.width / 2f - 64f - 5f, 15f, 64f, 64f);
				if (Widgets.ButtonImage(rect, TexUI.RotLeftTex, true, null))
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					rotationDirection = RotationDirection.Counterclockwise;
					Event.current.Use();
				}
				if (!SteamDeck.IsSteamDeck)
				{
					Widgets.Label(rect, KeyBindingDefOf.Designator_RotateLeft.MainKeyLabel);
				}
				Rect rect2 = new Rect(winRect.width / 2f + 5f, 15f, 64f, 64f);
				if (Widgets.ButtonImage(rect2, TexUI.RotRightTex, true, null))
				{
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					rotationDirection = RotationDirection.Clockwise;
					Event.current.Use();
				}
				if (!SteamDeck.IsSteamDeck)
				{
					Widgets.Label(rect2, KeyBindingDefOf.Designator_RotateRight.MainKeyLabel);
				}
				if (rotationDirection != RotationDirection.None)
				{
					rot.Rotate(rotationDirection);
					rotSetter(rot);
				}
				Text.Anchor = TextAnchor.UpperLeft;
				Text.Font = GameFont.Small;
			}, true, false, 1f, null, false);
		}

		// Token: 0x04002B78 RID: 11128
		public static readonly Material DragHighlightCellMat = MaterialPool.MatFrom("UI/Overlays/DragHighlightCell", ShaderDatabase.MetaOverlay);

		// Token: 0x04002B79 RID: 11129
		public static readonly Material DragHighlightThingMat = MaterialPool.MatFrom("UI/Overlays/DragHighlightThing", ShaderDatabase.MetaOverlay);

		// Token: 0x04002B7A RID: 11130
		private static Dictionary<Type, Designator> StandaloneDesignators = new Dictionary<Type, Designator>();

		// Token: 0x04002B7B RID: 11131
		private static HashSet<Thing> selectedThings = new HashSet<Thing>();

		// Token: 0x04002B7C RID: 11132
		private const float RotButSize = 64f;

		// Token: 0x04002B7D RID: 11133
		private const float RotButSpacing = 10f;
	}
}
