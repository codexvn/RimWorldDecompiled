using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001FF RID: 511
	public class AnimationDef : Def
	{
		// Token: 0x06000DA6 RID: 3494 RVA: 0x000411BC File Offset: 0x0003F3BC
		public bool TryGetPartForNodeTag(PawnRenderNodeTagDef nodeTag, out AnimationPart part)
		{
			KeyframeAnimationPart keyframeAnimationPart;
			if (this.keyframeParts != null && this.keyframeParts.TryGetValue(nodeTag, out keyframeAnimationPart))
			{
				part = keyframeAnimationPart;
				return true;
			}
			CurveKeyAnimationPart curveKeyAnimationPart;
			if (this.curveParts != null && this.curveParts.TryGetValue(nodeTag, out curveKeyAnimationPart))
			{
				part = curveKeyAnimationPart;
				return true;
			}
			part = null;
			return false;
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x00041207 File Offset: 0x0003F407
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.keyframeParts == null || this.curveParts == null)
			{
				yield break;
			}
			foreach (KeyValuePair<PawnRenderNodeTagDef, KeyframeAnimationPart> keyValuePair in this.keyframeParts)
			{
				PawnRenderNodeTagDef pawnRenderNodeTagDef;
				KeyframeAnimationPart keyframeAnimationPart;
				keyValuePair.Deconstruct(out pawnRenderNodeTagDef, out keyframeAnimationPart);
				PawnRenderNodeTagDef pawnRenderNodeTagDef2 = pawnRenderNodeTagDef;
				if (this.curveParts.ContainsKey(pawnRenderNodeTagDef2))
				{
					yield return string.Format("Animation def has a render node tag {0} which is driven by both keyframes and curve parts. You can only use one or the other for a given node tag", pawnRenderNodeTagDef2);
				}
			}
			Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator enumerator2 = default(Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x00041217 File Offset: 0x0003F417
		[return: TupleElementNames(new string[]
		{
			"state",
			"graphic"
		})]
		public virtual IEnumerable<ValueTuple<GraphicStateDef, Graphic>> GetGraphicStates(PawnRenderNode node)
		{
			if (this.keyframeParts != null)
			{
				foreach (KeyValuePair<PawnRenderNodeTagDef, KeyframeAnimationPart> keyValuePair in this.keyframeParts)
				{
					PawnRenderNodeTagDef pawnRenderNodeTagDef;
					KeyframeAnimationPart keyframeAnimationPart;
					keyValuePair.Deconstruct(out pawnRenderNodeTagDef, out keyframeAnimationPart);
					PawnRenderNodeTagDef pawnRenderNodeTagDef2 = pawnRenderNodeTagDef;
					KeyframeAnimationPart keyframeAnimationPart2 = keyframeAnimationPart;
					if (pawnRenderNodeTagDef2 == node.Props.tagDef && !keyframeAnimationPart2.keyframes.NullOrEmpty<Keyframe>())
					{
						foreach (Keyframe keyframe in keyframeAnimationPart2.keyframes)
						{
							Graphic coloredVersion;
							if (keyframe.graphicState != null && keyframe.graphicState.TryGetDefaultGraphic(out coloredVersion))
							{
								if (keyframe.graphicState.applySkinColorTint)
								{
									Color color = coloredVersion.Color;
									Color color2 = coloredVersion.ColorTwo;
									Pawn pawn = node.tree.pawn;
									if (pawn.IsMutant)
									{
										color = MutantUtility.GetMutantSkinColor(pawn, color);
										color2 = MutantUtility.GetMutantSkinColor(pawn, color2);
									}
									color = pawn.health.hediffSet.GetSkinColor(color);
									color2 = pawn.health.hediffSet.GetSkinColor(color2);
									coloredVersion = coloredVersion.GetColoredVersion(coloredVersion.Shader, color, color2);
								}
								yield return new ValueTuple<GraphicStateDef, Graphic>(keyframe.graphicState, coloredVersion);
							}
						}
						List<Keyframe>.Enumerator enumerator2 = default(List<Keyframe>.Enumerator);
					}
				}
				Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator enumerator = default(Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator);
			}
			if (this.curveParts != null)
			{
				foreach (KeyValuePair<PawnRenderNodeTagDef, CurveKeyAnimationPart> keyValuePair2 in this.curveParts)
				{
					PawnRenderNodeTagDef pawnRenderNodeTagDef;
					CurveKeyAnimationPart curveKeyAnimationPart;
					keyValuePair2.Deconstruct(out pawnRenderNodeTagDef, out curveKeyAnimationPart);
					PawnRenderNodeTagDef pawnRenderNodeTagDef3 = pawnRenderNodeTagDef;
					CurveKeyAnimationPart curveKeyAnimationPart2 = curveKeyAnimationPart;
					if (pawnRenderNodeTagDef3 == node.Props.tagDef && !curveKeyAnimationPart2.keyframes.NullOrEmpty<CurveKey>())
					{
						foreach (CurveKey curveKey in curveKeyAnimationPart2.keyframes)
						{
							Graphic item;
							if (curveKey.graphicState != null && curveKey.graphicState.TryGetDefaultGraphic(out item))
							{
								yield return new ValueTuple<GraphicStateDef, Graphic>(curveKey.graphicState, item);
							}
						}
						List<CurveKey>.Enumerator enumerator4 = default(List<CurveKey>.Enumerator);
					}
				}
				Dictionary<PawnRenderNodeTagDef, CurveKeyAnimationPart>.Enumerator enumerator3 = default(Dictionary<PawnRenderNodeTagDef, CurveKeyAnimationPart>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x040007FB RID: 2043
		public int durationTicks;

		// Token: 0x040007FC RID: 2044
		public bool startOnRandomTick;

		// Token: 0x040007FD RID: 2045
		public bool playWhenDowned;

		// Token: 0x040007FE RID: 2046
		public float posRotScale = 1f;

		// Token: 0x040007FF RID: 2047
		public LoopMode loopMode;

		// Token: 0x04000800 RID: 2048
		public Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart> keyframeParts;

		// Token: 0x04000801 RID: 2049
		public Dictionary<PawnRenderNodeTagDef, CurveKeyAnimationPart> curveParts;
	}
}
