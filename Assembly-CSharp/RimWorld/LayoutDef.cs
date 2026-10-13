using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A67 RID: 6759
	public class LayoutDef : Def
	{
		// Token: 0x1700173A RID: 5946
		// (get) Token: 0x060097D4 RID: 38868 RVA: 0x002C51F4 File Offset: 0x002C33F4
		public LayoutWorker Worker
		{
			get
			{
				LayoutWorker result;
				if ((result = this.layoutWorker) == null)
				{
					result = (this.layoutWorker = (LayoutWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					}));
				}
				return result;
			}
		}

		// Token: 0x0400653D RID: 25917
		public List<LayoutDef.LayoutRoomWeight> roomDefs;

		// Token: 0x0400653E RID: 25918
		public Type workerClass;

		// Token: 0x0400653F RID: 25919
		public bool clearRoomsEntirely;

		// Token: 0x04006540 RID: 25920
		public bool canHaveMultipleLayoutsInRoom;

		// Token: 0x04006541 RID: 25921
		public float multipleLayoutRoomChance = 0.15f;

		// Token: 0x04006542 RID: 25922
		public bool shouldDamage;

		// Token: 0x04006543 RID: 25923
		public bool noRoof;

		// Token: 0x04006544 RID: 25924
		public float adjacentRoomMergeChance = 0.65f;

		// Token: 0x04006545 RID: 25925
		public float borderDoorRemoveChance = 0.8f;

		// Token: 0x04006546 RID: 25926
		public int minRoomWidth = 6;

		// Token: 0x04006547 RID: 25927
		public int minRoomHeight = 6;

		// Token: 0x04006548 RID: 25928
		public List<LayoutScatterParms> junkScaterrers = new List<LayoutScatterParms>();

		// Token: 0x04006549 RID: 25929
		public List<LayoutScatterTerrainParms> scatterTerrain = new List<LayoutScatterTerrainParms>();

		// Token: 0x0400654A RID: 25930
		public List<LayoutFillEdgesParms> fillEdges = new List<LayoutFillEdgesParms>();

		// Token: 0x0400654B RID: 25931
		public List<LayoutFillInteriorParms> fillInterior = new List<LayoutFillInteriorParms>();

		// Token: 0x0400654C RID: 25932
		public List<LayoutWallAttatchmentParms> wallAttachments = new List<LayoutWallAttatchmentParms>();

		// Token: 0x0400654D RID: 25933
		public List<LayoutPrefabParms> prefabs = new List<LayoutPrefabParms>();

		// Token: 0x0400654E RID: 25934
		public List<LayoutPartParms> parts = new List<LayoutPartParms>();

		// Token: 0x0400654F RID: 25935
		private LayoutWorker layoutWorker;

		// Token: 0x02001A68 RID: 6760
		public class LayoutRoomWeight
		{
			// Token: 0x060097D6 RID: 38870 RVA: 0x002C52BF File Offset: 0x002C34BF
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				XmlHelper.ParseElements(this, xmlRoot, "def", "weight");
			}

			// Token: 0x04006550 RID: 25936
			public LayoutRoomDef def;

			// Token: 0x04006551 RID: 25937
			public float weight = 1f;

			// Token: 0x04006552 RID: 25938
			public IntRange countRange = new IntRange(0, int.MaxValue);
		}
	}
}
