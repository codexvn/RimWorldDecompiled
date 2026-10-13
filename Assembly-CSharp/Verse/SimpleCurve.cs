using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000DC RID: 220
	public class SimpleCurve : IEnumerable<CurvePoint>, IEnumerable, IExposable
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060005FC RID: 1532 RVA: 0x0001DD13 File Offset: 0x0001BF13
		public int PointsCount
		{
			get
			{
				return this.points.Count;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060005FD RID: 1533 RVA: 0x0001DD20 File Offset: 0x0001BF20
		public List<CurvePoint> Points
		{
			get
			{
				return this.points;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060005FE RID: 1534 RVA: 0x0001DD28 File Offset: 0x0001BF28
		public bool HasView
		{
			get
			{
				return this.view != null;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060005FF RID: 1535 RVA: 0x0001DD33 File Offset: 0x0001BF33
		public SimpleCurveView View
		{
			get
			{
				if (this.view == null)
				{
					this.view = new SimpleCurveView();
					this.view.SetViewRectAround(this);
				}
				return this.view;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0001DD5A File Offset: 0x0001BF5A
		public float MinY
		{
			get
			{
				return this.points.Min((CurvePoint point) => point.y);
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000601 RID: 1537 RVA: 0x0001DD86 File Offset: 0x0001BF86
		public float MaxY
		{
			get
			{
				return this.points.Max((CurvePoint point) => point.y);
			}
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x0001DDB2 File Offset: 0x0001BFB2
		public SimpleCurve(IEnumerable<CurvePoint> points)
		{
			this.SetPoints(points);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0001DDCC File Offset: 0x0001BFCC
		public SimpleCurve()
		{
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0001DDDF File Offset: 0x0001BFDF
		public void ExposeData()
		{
			Scribe_Collections.Look<CurvePoint>(ref this.points, "points", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0001DDF7 File Offset: 0x0001BFF7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0001DDFF File Offset: 0x0001BFFF
		public IEnumerator<CurvePoint> GetEnumerator()
		{
			foreach (CurvePoint curvePoint in this.points)
			{
				yield return curvePoint;
			}
			List<CurvePoint>.Enumerator enumerator = default(List<CurvePoint>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x17000125 RID: 293
		public CurvePoint this[int i]
		{
			get
			{
				return this.points[i];
			}
			set
			{
				this.points[i] = value;
			}
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x0001DE2C File Offset: 0x0001C02C
		public void SetPoints(IEnumerable<CurvePoint> newPoints)
		{
			this.points.Clear();
			foreach (CurvePoint item in newPoints)
			{
				this.points.Add(item);
			}
			this.SortPoints();
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x0001DE8C File Offset: 0x0001C08C
		public void Add(float x, float y, bool sort = true)
		{
			CurvePoint newPoint = new CurvePoint(x, y);
			this.Add(newPoint, sort);
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0001DEAA File Offset: 0x0001C0AA
		public void Add(CurvePoint newPoint, bool sort = true)
		{
			this.points.Add(newPoint);
			if (sort)
			{
				this.SortPoints();
			}
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0001DEC1 File Offset: 0x0001C0C1
		public void SortPoints()
		{
			this.points.Sort(SimpleCurve.CurvePointsComparer);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0001DED4 File Offset: 0x0001C0D4
		public float ClampToCurve(float value)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Clamping a value to an empty SimpleCurve.");
				return value;
			}
			return Mathf.Clamp(value, this.points[0].y, this.points[this.points.Count - 1].y);
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0001DF34 File Offset: 0x0001C134
		public void RemovePointNear(CurvePoint point)
		{
			for (int i = 0; i < this.points.Count; i++)
			{
				if ((this.points[i].Loc - point.Loc).sqrMagnitude < 0.001f)
				{
					this.points.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0001DF94 File Offset: 0x0001C194
		public float Evaluate(float x)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.");
				return 0f;
			}
			if (x <= this.points[0].x)
			{
				return this.points[0].y;
			}
			if (x >= this.points[this.points.Count - 1].x)
			{
				return this.points[this.points.Count - 1].y;
			}
			CurvePoint curvePoint = this.points[0];
			CurvePoint curvePoint2 = this.points[this.points.Count - 1];
			int i = 0;
			while (i < this.points.Count)
			{
				if (x <= this.points[i].x)
				{
					curvePoint2 = this.points[i];
					if (i > 0)
					{
						curvePoint = this.points[i - 1];
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			float t = (x - curvePoint.x) / (curvePoint2.x - curvePoint.x);
			return Mathf.Lerp(curvePoint.y, curvePoint2.y, t);
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0001E0DC File Offset: 0x0001C2DC
		public float EvaluateInverted(float y)
		{
			if (this.points.Count == 0)
			{
				Log.Error("Evaluating a SimpleCurve with no points.");
				return 0f;
			}
			if (this.points.Count == 1)
			{
				return this.points[0].x;
			}
			int i = 0;
			while (i < this.points.Count - 1)
			{
				if ((y >= this.points[i].y && y <= this.points[i + 1].y) || (y <= this.points[i].y && y >= this.points[i + 1].y))
				{
					if (y == this.points[i].y)
					{
						return this.points[i].x;
					}
					if (y == this.points[i + 1].y)
					{
						return this.points[i + 1].x;
					}
					return GenMath.LerpDouble(this.points[i].y, this.points[i + 1].y, this.points[i].x, this.points[i + 1].x, y);
				}
				else
				{
					i++;
				}
			}
			if (y < this.points[0].y)
			{
				float result = 0f;
				float num = 0f;
				for (int j = 0; j < this.points.Count; j++)
				{
					if (j == 0 || this.points[j].y < num)
					{
						num = this.points[j].y;
						result = this.points[j].x;
					}
				}
				return result;
			}
			float result2 = 0f;
			float num2 = 0f;
			for (int k = 0; k < this.points.Count; k++)
			{
				if (k == 0 || this.points[k].y > num2)
				{
					num2 = this.points[k].y;
					result2 = this.points[k].x;
				}
			}
			return result2;
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0001E360 File Offset: 0x0001C560
		public float PeriodProbabilityFromCumulative(float startX, float span)
		{
			if (this.points.Count < 2)
			{
				return 0f;
			}
			if (this.points[0].y != 0f)
			{
				Log.Warning("PeriodProbabilityFromCumulative should only run on curves whose first point is 0.");
			}
			float num = this.Evaluate(startX + span) - this.Evaluate(startX);
			if (num < 0f)
			{
				Log.Error("PeriodicProbability got negative probability from " + ((this != null) ? this.ToString() : null) + ": slope should never be negative.");
				num = 0f;
			}
			if (num > 1f)
			{
				num = 1f;
			}
			return num;
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0001E3F6 File Offset: 0x0001C5F6
		public IEnumerable<string> ConfigErrors(string prefix)
		{
			for (int i = 0; i < this.points.Count - 1; i++)
			{
				if (this.points[i + 1].x < this.points[i].x)
				{
					yield return prefix + ": points are out of order";
					break;
				}
			}
			yield break;
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0001E40D File Offset: 0x0001C60D
		public static SimpleCurve Empty()
		{
			return new SimpleCurve
			{
				{
					new CurvePoint(0f, 0f),
					true
				}
			};
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x0001E42C File Offset: 0x0001C62C
		public AnimationCurve ToAnimationCurve()
		{
			AnimationCurve animationCurve = new AnimationCurve();
			foreach (CurvePoint curvePoint in this.Points)
			{
				animationCurve.AddKey(curvePoint.x, curvePoint.y);
			}
			return animationCurve;
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x0001E494 File Offset: 0x0001C694
		public static SimpleCurve FromAnimationCurve(AnimationCurve curve)
		{
			SimpleCurve simpleCurve = new SimpleCurve();
			foreach (Keyframe keyframe in curve.keys)
			{
				simpleCurve.Points.Add(new CurvePoint(keyframe.time, keyframe.value));
			}
			return simpleCurve;
		}

		// Token: 0x04000315 RID: 789
		private List<CurvePoint> points = new List<CurvePoint>();

		// Token: 0x04000316 RID: 790
		[Unsaved(false)]
		private SimpleCurveView view;

		// Token: 0x04000317 RID: 791
		private static Comparison<CurvePoint> CurvePointsComparer = delegate(CurvePoint a, CurvePoint b)
		{
			if (a.x < b.x)
			{
				return -1;
			}
			if (b.x < a.x)
			{
				return 1;
			}
			return 0;
		};
	}
}
