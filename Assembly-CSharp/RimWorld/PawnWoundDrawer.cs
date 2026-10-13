using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022E3 RID: 8931
	public class PawnWoundDrawer : PawnOverlayDrawer
	{
		// Token: 0x0600C223 RID: 49699 RVA: 0x00383E33 File Offset: 0x00382033
		public PawnWoundDrawer(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x0600C224 RID: 49700 RVA: 0x00383E48 File Offset: 0x00382048
		protected override void WriteCache(PawnOverlayDrawer.CacheKey key, PawnDrawParms parms, List<PawnOverlayDrawer.DrawCall> writeTarget)
		{
			Rot4 pawnRot = key.pawnRot;
			Mesh bodyMesh = key.bodyMesh;
			PawnOverlayDrawer.OverlayLayer layer = key.layer;
			this.DebugDraw(writeTarget, parms.matrix, bodyMesh, pawnRot, layer);
			Graphic graphic = (layer == PawnOverlayDrawer.OverlayLayer.Body) ? this.pawn.Drawer.renderer.BodyGraphic : this.pawn.Drawer.renderer.HeadGraphic;
			List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
			int num = 0;
			while (num < this.pawn.health.hediffSet.hediffs.Count && (this.pawn.RaceProps.Humanlike || writeTarget.Count < 5))
			{
				Hediff hediff = this.pawn.health.hediffSet.hediffs[num];
				if (hediff.Part != null && hediff.Visible && hediff.def.displayWound && (!(hediff is Hediff_MissingPart) || missingPartsCommonAncestors.Contains(hediff)))
				{
					if (hediff is Hediff_AddedPart && this.pawn.apparel != null)
					{
						bool flag = false;
						foreach (Apparel apparel in this.pawn.apparel.WornApparel)
						{
							if (apparel.def.apparel.blocksAddedPartWoundGraphics && apparel.def.apparel.CoversBodyPart(hediff.Part))
							{
								flag = true;
								break;
							}
						}
						if (flag)
						{
							goto IL_6B1;
						}
					}
					float num2 = 0f;
					string b = null;
					Vector3 zero = Vector3.zero;
					Pawn_StoryTracker story = this.pawn.story;
					bool flag2;
					if (story == null)
					{
						flag2 = (null != null);
					}
					else
					{
						BodyTypeDef bodyType = story.bodyType;
						flag2 = (((bodyType != null) ? bodyType.woundAnchors : null) != null);
					}
					if (flag2)
					{
						this.tmpAnchors.AddRange(PawnDrawUtility.FindAnchors(this.pawn, hediff.Part));
						if (this.tmpAnchors.Count > 0)
						{
							for (int i = this.tmpAnchors.Count - 1; i >= 0; i--)
							{
								if (this.tmpAnchors[i].layer != layer || !PawnDrawUtility.AnchorUsable(this.pawn, this.tmpAnchors[i], pawnRot))
								{
									this.tmpAnchors.RemoveAt(i);
								}
							}
							if (this.tmpAnchors.Count == 0)
							{
								goto IL_6B1;
							}
							BodyTypeDef.WoundAnchor woundAnchor = this.tmpAnchors.RandomElement<BodyTypeDef.WoundAnchor>();
							PawnDrawUtility.CalcAnchorData(this.pawn, woundAnchor, pawnRot, out zero, out num2);
							num2 = (hediff.def.woundAnchorRange ?? num2);
							b = woundAnchor.tag;
						}
						this.tmpAnchors.Clear();
					}
					else
					{
						this.GetDefaultAnchor(bodyMesh, out zero, out num2);
					}
					Rand.PushState(this.pawn.thingIDNumber * num * pawnRot.AsInt);
					try
					{
						FleshTypeDef.ResolvedWound resolvedWound = this.pawn.RaceProps.FleshType.ChooseWoundOverlay(hediff);
						if (resolvedWound != null)
						{
							if (!resolvedWound.wound.displayPermanent)
							{
								Hediff_Injury hediff_Injury = hediff as Hediff_Injury;
								if (hediff_Injury != null && hediff_Injury.IsPermanent())
								{
									goto IL_6B1;
								}
							}
							Vector3 b2 = resolvedWound.wound.drawOffsetSouth;
							if (pawnRot.IsHorizontal)
							{
								b2 = resolvedWound.wound.drawOffsetEastWest.ScaledBy((pawnRot == Rot4.East) ? Vector3.one : PawnWoundDrawer.VecOneFlipped);
							}
							Vector3 vector = zero + b2;
							Vector4? maskTexOffset = null;
							Vector4? maskTexScale = null;
							bool flag3;
							Material material = resolvedWound.GetMaterial(pawnRot, out flag3);
							if (resolvedWound.wound.flipOnWoundAnchorTag != null && resolvedWound.wound.flipOnWoundAnchorTag == b && resolvedWound.wound.flipOnRotation == pawnRot)
							{
								flag3 = !flag3;
							}
							Mesh mesh = MeshPool.GridPlane(Vector2.one * 0.25f, flag3);
							if (!this.pawn.def.race.Humanlike)
							{
								Vector3 b3 = Rand.InsideUnitCircleVec3 * num2;
								if (flag3)
								{
									b3.x *= -1f;
								}
								vector += b3;
								material = MaterialPool.MatFrom(new MaterialRequest
								{
									maskTex = (Texture2D)graphic.MatAt(pawnRot, null).mainTexture,
									mainTex = material.mainTexture,
									color = material.color,
									shader = material.shader
								});
								Vector3 drawPos = this.pawn.DrawPos;
								Vector3 vector2 = drawPos + vector - mesh.bounds.extents;
								Vector3 vector3 = drawPos - bodyMesh.bounds.extents;
								Vector3 size = bodyMesh.bounds.size;
								Vector3 size2 = mesh.bounds.size;
								bool flag4 = (graphic.EastFlipped && pawnRot == Rot4.East) || (graphic.WestFlipped && pawnRot == Rot4.West);
								maskTexScale = new Vector4?(new Vector4(size2.x / size.x, size2.z / size.z));
								maskTexOffset = new Vector4?(new Vector4((vector2.x - vector3.x) / size.x, (vector2.z - vector3.z) / size.z, (float)(flag4 ? 1 : 0)));
							}
							Matrix4x4 matrix = Matrix4x4.TRS(vector, Quaternion.AngleAxis((float)Rand.Range(0, 360), Vector3.up), Vector3.one * resolvedWound.wound.scale);
							Color? colorOverride = null;
							if (resolvedWound.wound.tintWithSkinColor && this.pawn.story != null)
							{
								colorOverride = new Color?(this.pawn.story.SkinColor);
							}
							if (this.pawn.Corpse != null && this.pawn.Corpse.GetRotStage() == RotStage.Rotting)
							{
								colorOverride = new Color?(PawnWoundDrawer.OldWoundColor);
							}
							if (this.pawn.IsMutant && this.pawn.mutant.Def.woundColor != null)
							{
								colorOverride = this.pawn.mutant.Def.woundColor;
							}
							writeTarget.Add(new PawnOverlayDrawer.DrawCall
							{
								overlayMat = material,
								matrix = matrix,
								overlayMesh = mesh,
								displayOverApparel = resolvedWound.wound.displayOverApparel,
								colorOverride = colorOverride,
								maskTexScale = maskTexScale,
								maskTexOffset = maskTexOffset
							});
						}
					}
					finally
					{
						Rand.PopState();
					}
				}
				IL_6B1:
				num++;
			}
		}

		// Token: 0x0600C225 RID: 49701 RVA: 0x00384564 File Offset: 0x00382764
		private void GetDefaultAnchor(Mesh bodyMesh, out Vector3 anchorOffset, out float range)
		{
			anchorOffset = bodyMesh.bounds.center;
			range = Mathf.Min(bodyMesh.bounds.extents.x, bodyMesh.bounds.extents.z) / 2f;
		}

		// Token: 0x0600C226 RID: 49702 RVA: 0x003845B8 File Offset: 0x003827B8
		private void DebugDraw(List<PawnOverlayDrawer.DrawCall> writeTarget, Matrix4x4 matrix, Mesh bodyMesh, Rot4 pawnRot, PawnOverlayDrawer.OverlayLayer layer)
		{
			if (this.debugDrawPart != null)
			{
				bool flag = false;
				foreach (BodyTypeDef.WoundAnchor woundAnchor in PawnDrawUtility.FindAnchors(this.pawn, this.debugDrawPart))
				{
					if (PawnDrawUtility.AnchorUsable(this.pawn, woundAnchor, pawnRot))
					{
						flag = true;
						Material overlayMat = MaterialPool.MatFrom(new MaterialRequest(BaseContent.WhiteTex, ShaderDatabase.SolidColor, woundAnchor.debugColor));
						Vector3 vector;
						float num;
						PawnDrawUtility.CalcAnchorData(this.pawn, woundAnchor, pawnRot, out vector, out num);
						matrix *= Matrix4x4.Translate(vector) * Matrix4x4.Scale(Vector3.one * (num * this.pawn.story.bodyType.woundScale));
						if (woundAnchor.layer == layer)
						{
							writeTarget.Add(new PawnOverlayDrawer.DrawCall
							{
								overlayMat = overlayMat,
								matrix = matrix,
								overlayMesh = MeshPool.circle,
								displayOverApparel = true,
								maskTexOffset = new Vector4?(Vector4.zero),
								maskTexScale = new Vector4?(Vector4.one)
							});
						}
					}
				}
				if (!flag)
				{
					Vector3 vector2;
					float d;
					this.GetDefaultAnchor(bodyMesh, out vector2, out d);
					matrix *= Matrix4x4.Translate(vector2) * Matrix4x4.Scale(Vector3.one * d);
					writeTarget.Add(new PawnOverlayDrawer.DrawCall
					{
						overlayMat = BaseContent.BadMat,
						matrix = matrix,
						overlayMesh = MeshPool.circle,
						displayOverApparel = true,
						maskTexOffset = new Vector4?(Vector4.zero),
						maskTexScale = new Vector4?(Vector4.one)
					});
					return;
				}
			}
			else if (this.debugDrawAllParts)
			{
				Pawn_StoryTracker story = this.pawn.story;
				bool flag2;
				if (story == null)
				{
					flag2 = (null != null);
				}
				else
				{
					BodyTypeDef bodyType = story.bodyType;
					flag2 = (((bodyType != null) ? bodyType.woundAnchors : null) != null);
				}
				if (flag2)
				{
					using (List<BodyTypeDef.WoundAnchor>.Enumerator enumerator2 = this.pawn.story.bodyType.woundAnchors.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							BodyTypeDef.WoundAnchor woundAnchor2 = enumerator2.Current;
							if (PawnDrawUtility.AnchorUsable(this.pawn, woundAnchor2, pawnRot))
							{
								Material overlayMat2 = MaterialPool.MatFrom(new MaterialRequest(BaseContent.WhiteTex, ShaderDatabase.SolidColor, woundAnchor2.debugColor));
								Vector3 vector3;
								float d2;
								PawnDrawUtility.CalcAnchorData(this.pawn, woundAnchor2, pawnRot, out vector3, out d2);
								matrix *= Matrix4x4.Translate(vector3) * Matrix4x4.Scale(Vector3.one * d2);
								if (woundAnchor2.layer == layer)
								{
									writeTarget.Add(new PawnOverlayDrawer.DrawCall
									{
										overlayMat = overlayMat2,
										matrix = matrix,
										overlayMesh = MeshPool.circle,
										displayOverApparel = true,
										maskTexOffset = new Vector4?(Vector4.zero),
										maskTexScale = new Vector4?(Vector4.one)
									});
								}
							}
						}
						return;
					}
				}
				Vector3 vector4;
				float d3;
				this.GetDefaultAnchor(bodyMesh, out vector4, out d3);
				matrix *= Matrix4x4.Translate(vector4) * Matrix4x4.Scale(Vector3.one * d3);
				writeTarget.Add(new PawnOverlayDrawer.DrawCall
				{
					overlayMat = BaseContent.BadMat,
					matrix = matrix,
					overlayMesh = MeshPool.circle,
					displayOverApparel = true,
					maskTexOffset = new Vector4?(Vector4.zero),
					maskTexScale = new Vector4?(Vector4.one)
				});
			}
		}

		// Token: 0x04008484 RID: 33924
		public BodyPartRecord debugDrawPart;

		// Token: 0x04008485 RID: 33925
		public bool debugDrawAllParts;

		// Token: 0x04008486 RID: 33926
		private const int MaxVisibleHediffsNonHuman = 5;

		// Token: 0x04008487 RID: 33927
		private static Color OldWoundColor = new Color(0.3f, 0.3f, 0f, 1f);

		// Token: 0x04008488 RID: 33928
		private const float WoundScale = 0.25f;

		// Token: 0x04008489 RID: 33929
		private static Vector3 VecOneFlipped = new Vector3(-1f, 1f, 1f);

		// Token: 0x0400848A RID: 33930
		private readonly List<BodyTypeDef.WoundAnchor> tmpAnchors = new List<BodyTypeDef.WoundAnchor>();
	}
}
