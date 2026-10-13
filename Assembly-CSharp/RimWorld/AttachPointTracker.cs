using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C00 RID: 11264
	public class AttachPointTracker
	{
		// Token: 0x170027A6 RID: 10150
		// (get) Token: 0x0600FC5F RID: 64607 RVA: 0x0049313D File Offset: 0x0049133D
		public string ThingId
		{
			get
			{
				return this.parent.ThingID;
			}
		}

		// Token: 0x0600FC60 RID: 64608 RVA: 0x0049314C File Offset: 0x0049134C
		public AttachPointTracker(List<AttachPoint> points, ThingWithComps parent)
		{
			this.parent = parent;
			this.cache = new Dictionary<AttachPointType, AttachPoint>();
			foreach (AttachPoint attachPoint in points)
			{
				this.cache[attachPoint.type] = attachPoint;
			}
			this.cache[AttachPointType.RootNone] = new AttachPoint
			{
				offset = Vector3.zero,
				type = AttachPointType.RootNone
			};
		}

		// Token: 0x0600FC61 RID: 64609 RVA: 0x004931E0 File Offset: 0x004913E0
		public void Add(AttachPointTracker other, bool overwrite = true)
		{
			foreach (KeyValuePair<AttachPointType, AttachPoint> keyValuePair in other.cache)
			{
				if (overwrite || !this.cache.ContainsKey(keyValuePair.Key))
				{
					this.cache[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		// Token: 0x0600FC62 RID: 64610 RVA: 0x0049325C File Offset: 0x0049145C
		private void TryCacheKindDefOverrides()
		{
			if (!this.didReadOverrides)
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn != null)
				{
					AlternateGraphic alternateGraphic;
					int num;
					if (pawn.TryGetAlternate(out alternateGraphic, out num) && alternateGraphic.attachPoints != null)
					{
						foreach (AttachPoint attachPoint in alternateGraphic.attachPoints)
						{
							this.cache[attachPoint.type] = attachPoint;
						}
					}
					Graphic bodyGraphic = pawn.Drawer.renderer.BodyGraphic;
					GraphicData graphicData = (bodyGraphic != null) ? bodyGraphic.data : null;
					if (graphicData == null)
					{
						return;
					}
					if (graphicData.attachPoints != null && graphicData.attachPoints.Count > 0)
					{
						foreach (AttachPoint attachPoint2 in graphicData.attachPoints)
						{
							this.cache[attachPoint2.type] = attachPoint2;
						}
					}
					this.didReadOverrides = true;
				}
			}
		}

		// Token: 0x0600FC63 RID: 64611 RVA: 0x00493380 File Offset: 0x00491580
		public Vector3 GetRotatedOffset(AttachPointType type)
		{
			return this.GetRotatedOffset(type, this.parent.Rotation);
		}

		// Token: 0x0600FC64 RID: 64612 RVA: 0x00493394 File Offset: 0x00491594
		public Vector3 GetRotatedOffset(AttachPointType type, Rot4 rot)
		{
			Vector3 vector = this.GetPointOrDefault(type).offset;
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				vector *= pawn.ageTracker.CurLifeStage.attachPointScaleFactor;
			}
			return vector.RotatedBy(rot);
		}

		// Token: 0x0600FC65 RID: 64613 RVA: 0x004933DB File Offset: 0x004915DB
		public Vector3 GetWorldPos(AttachPointType type)
		{
			return this.parent.DrawPos + this.GetRotatedOffset(type);
		}

		// Token: 0x0600FC66 RID: 64614 RVA: 0x004933F4 File Offset: 0x004915F4
		public IEnumerable<AttachPointType> PointTypes(int min = 0, int max = 2147483647)
		{
			this.TryCacheKindDefOverrides();
			foreach (KeyValuePair<AttachPointType, AttachPoint> keyValuePair in this.cache)
			{
				int key = (int)keyValuePair.Key;
				if (key >= min && key <= max)
				{
					yield return keyValuePair.Key;
				}
			}
			Dictionary<AttachPointType, AttachPoint>.Enumerator enumerator = default(Dictionary<AttachPointType, AttachPoint>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600FC67 RID: 64615 RVA: 0x00493412 File Offset: 0x00491612
		public IEnumerable<AttachPoint> Points(int min = 0, int max = 2147483647)
		{
			foreach (KeyValuePair<AttachPointType, AttachPoint> keyValuePair in this.cache)
			{
				int key = (int)keyValuePair.Key;
				if (key >= min && key <= max)
				{
					yield return keyValuePair.Value;
				}
			}
			Dictionary<AttachPointType, AttachPoint>.Enumerator enumerator = default(Dictionary<AttachPointType, AttachPoint>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600FC68 RID: 64616 RVA: 0x00493430 File Offset: 0x00491630
		public AttachPoint GetPointOrDefault(AttachPointType type)
		{
			AttachPoint result;
			if (this.TryGetPoint(type, out result))
			{
				return result;
			}
			return this.cache[AttachPointType.RootNone];
		}

		// Token: 0x0600FC69 RID: 64617 RVA: 0x00493456 File Offset: 0x00491656
		public bool TryGetPoint(AttachPointType type, out AttachPoint pt)
		{
			return this.cache.TryGetValue(type, out pt);
		}

		// Token: 0x0400A55D RID: 42333
		private ThingWithComps parent;

		// Token: 0x0400A55E RID: 42334
		private Dictionary<AttachPointType, AttachPoint> cache;

		// Token: 0x0400A55F RID: 42335
		private bool didReadOverrides;
	}
}
