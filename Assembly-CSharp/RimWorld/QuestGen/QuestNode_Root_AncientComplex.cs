using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B1F RID: 15135
	public abstract class QuestNode_Root_AncientComplex : QuestNode
	{
		// Token: 0x17003354 RID: 13140
		// (get) Token: 0x0601496A RID: 84330 RVA: 0x00624977 File Offset: 0x00622B77
		protected virtual SimpleCurve ComplexSizeOverPointsCurve
		{
			get
			{
				return new SimpleCurve
				{
					{
						new CurvePoint(0f, 30f),
						true
					},
					{
						new CurvePoint(10000f, 50f),
						true
					}
				};
			}
		}

		// Token: 0x17003355 RID: 13141
		// (get) Token: 0x0601496B RID: 84331 RVA: 0x006249AC File Offset: 0x00622BAC
		protected virtual SimpleCurve TerminalsOverRoomCountCurve
		{
			get
			{
				return new SimpleCurve
				{
					{
						new CurvePoint(0f, 1f),
						true
					},
					{
						new CurvePoint(10f, 4f),
						true
					},
					{
						new CurvePoint(20f, 6f),
						true
					},
					{
						new CurvePoint(50f, 10f),
						true
					}
				};
			}
		}

		// Token: 0x17003356 RID: 13142
		// (get) Token: 0x0601496C RID: 84332 RVA: 0x005FC448 File Offset: 0x005FA648
		protected virtual LayoutDef LayoutDef
		{
			get
			{
				return LayoutDefOf.AncientComplex;
			}
		}

		// Token: 0x17003357 RID: 13143
		// (get) Token: 0x0601496D RID: 84333 RVA: 0x00624A16 File Offset: 0x00622C16
		protected virtual SitePartDef SitePartDef
		{
			get
			{
				return SitePartDefOf.AncientComplex;
			}
		}

		// Token: 0x17003358 RID: 13144
		// (get) Token: 0x0601496E RID: 84334 RVA: 0x00624A20 File Offset: 0x00622C20
		protected static List<LandmarkDef> AllowedLandmarks
		{
			get
			{
				if (ModsConfig.OdysseyActive && QuestNode_Root_AncientComplex.allowedLandmarksCached == null)
				{
					QuestNode_Root_AncientComplex.allowedLandmarksCached = new List<LandmarkDef>
					{
						LandmarkDefOf.Oasis,
						LandmarkDefOf.Lake,
						LandmarkDefOf.LakeWithIsland,
						LandmarkDefOf.LakeWithIslands,
						LandmarkDefOf.Pond,
						LandmarkDefOf.DryLake,
						LandmarkDefOf.ToxicLake,
						LandmarkDefOf.Wetland,
						LandmarkDefOf.HotSprings,
						LandmarkDefOf.CoastalIsland,
						LandmarkDefOf.Peninsula,
						LandmarkDefOf.Valley,
						LandmarkDefOf.Cavern,
						LandmarkDefOf.Chasm,
						LandmarkDefOf.IceDunes,
						LandmarkDefOf.Cliffs,
						LandmarkDefOf.Hollow,
						LandmarkDefOf.TerraformingScar,
						LandmarkDefOf.LavaFlow,
						LandmarkDefOf.Dunes,
						LandmarkDefOf.AncientSmokeVent,
						LandmarkDefOf.AncientToxVent
					};
				}
				return QuestNode_Root_AncientComplex.allowedLandmarksCached;
			}
		}

		// Token: 0x0601496F RID: 84335
		public abstract LayoutStructureSketch QuestSetupComplex(Quest quest, float points);

		// Token: 0x06014970 RID: 84336 RVA: 0x00624B44 File Offset: 0x00622D44
		protected virtual LayoutStructureSketch GenerateStructureSketch(float points, bool generateTerminals = true)
		{
			int num = (int)this.ComplexSizeOverPointsCurve.Evaluate(points);
			StructureGenParams parms = new StructureGenParams
			{
				size = new IntVec2(num, num)
			};
			LayoutStructureSketch layoutStructureSketch = this.LayoutDef.Worker.GenerateStructureSketch(parms);
			if (generateTerminals)
			{
				int num2 = Mathf.FloorToInt(this.TerminalsOverRoomCountCurve.Evaluate((float)layoutStructureSketch.structureLayout.Rooms.Count));
				for (int i = 0; i < num2; i++)
				{
					layoutStructureSketch.thingsToSpawn.Add(ThingMaker.MakeThing(ThingDefOf.AncientTerminal, null));
				}
			}
			return layoutStructureSketch;
		}

		// Token: 0x0400E49A RID: 58522
		private static List<LandmarkDef> allowedLandmarksCached;

		// Token: 0x0400E49B RID: 58523
		protected static readonly SimpleCurve ThreatPointsOverPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(35f, 38.5f),
				true
			},
			{
				new CurvePoint(400f, 165f),
				true
			},
			{
				new CurvePoint(10000f, 4125f),
				true
			}
		};
	}
}
