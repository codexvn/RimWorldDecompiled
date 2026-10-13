using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FC2 RID: 12226
	public struct AlertReport
	{
		// Token: 0x17002CA7 RID: 11431
		// (get) Token: 0x060113B0 RID: 70576 RVA: 0x004EFF54 File Offset: 0x004EE154
		public bool AnyCulpritValid
		{
			get
			{
				if (!this.culpritsThings.NullOrEmpty<Thing>() || !this.culpritsPawns.NullOrEmpty<Pawn>() || !this.culpritsCaravans.NullOrEmpty<Caravan>())
				{
					return true;
				}
				if (this.culpritTarget != null && this.culpritTarget.Value.IsValid)
				{
					return true;
				}
				if (this.culpritsTargets != null)
				{
					for (int i = 0; i < this.culpritsTargets.Count; i++)
					{
						if (this.culpritsTargets[i].IsValid)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17002CA8 RID: 11432
		// (get) Token: 0x060113B1 RID: 70577 RVA: 0x004EFFE5 File Offset: 0x004EE1E5
		public IEnumerable<GlobalTargetInfo> AllCulprits
		{
			get
			{
				if (this.culpritsThings != null)
				{
					int num;
					for (int i = 0; i < this.culpritsThings.Count; i = num + 1)
					{
						yield return this.culpritsThings[i];
						num = i;
					}
				}
				if (this.culpritsPawns != null)
				{
					int num;
					for (int i = 0; i < this.culpritsPawns.Count; i = num + 1)
					{
						yield return this.culpritsPawns[i];
						num = i;
					}
				}
				if (this.culpritsCaravans != null)
				{
					int num;
					for (int i = 0; i < this.culpritsCaravans.Count; i = num + 1)
					{
						yield return this.culpritsCaravans[i];
						num = i;
					}
				}
				if (this.culpritTarget != null)
				{
					yield return this.culpritTarget.Value;
				}
				if (this.culpritsTargets != null)
				{
					int num;
					for (int i = 0; i < this.culpritsTargets.Count; i = num + 1)
					{
						yield return this.culpritsTargets[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x060113B2 RID: 70578 RVA: 0x004EFFFC File Offset: 0x004EE1FC
		public static AlertReport CulpritIs(GlobalTargetInfo culp)
		{
			AlertReport result = default(AlertReport);
			result.active = culp.IsValid;
			if (culp.IsValid)
			{
				result.culpritTarget = new GlobalTargetInfo?(culp);
			}
			return result;
		}

		// Token: 0x060113B3 RID: 70579 RVA: 0x004F0038 File Offset: 0x004EE238
		public static AlertReport CulpritsAre(List<Thing> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsThings = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x060113B4 RID: 70580 RVA: 0x004F0064 File Offset: 0x004EE264
		public static AlertReport CulpritsAre(List<Pawn> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsPawns = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x060113B5 RID: 70581 RVA: 0x004F0090 File Offset: 0x004EE290
		public static AlertReport CulpritsAre(List<Caravan> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsCaravans = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x060113B6 RID: 70582 RVA: 0x004F00BC File Offset: 0x004EE2BC
		public static AlertReport CulpritsAre(List<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsTargets = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x060113B7 RID: 70583 RVA: 0x004F00E8 File Offset: 0x004EE2E8
		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		// Token: 0x060113B8 RID: 70584 RVA: 0x004F0106 File Offset: 0x004EE306
		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x060113B9 RID: 70585 RVA: 0x004F0113 File Offset: 0x004EE313
		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x060113BA RID: 70586 RVA: 0x004F0120 File Offset: 0x004EE320
		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x17002CA9 RID: 11433
		// (get) Token: 0x060113BB RID: 70587 RVA: 0x004F0128 File Offset: 0x004EE328
		public static AlertReport Active
		{
			get
			{
				return new AlertReport
				{
					active = true
				};
			}
		}

		// Token: 0x17002CAA RID: 11434
		// (get) Token: 0x060113BC RID: 70588 RVA: 0x004F0148 File Offset: 0x004EE348
		public static AlertReport Inactive
		{
			get
			{
				return new AlertReport
				{
					active = false
				};
			}
		}

		// Token: 0x0400B2E2 RID: 45794
		public bool active;

		// Token: 0x0400B2E3 RID: 45795
		public List<Thing> culpritsThings;

		// Token: 0x0400B2E4 RID: 45796
		public List<Pawn> culpritsPawns;

		// Token: 0x0400B2E5 RID: 45797
		public List<Caravan> culpritsCaravans;

		// Token: 0x0400B2E6 RID: 45798
		public List<GlobalTargetInfo> culpritsTargets;

		// Token: 0x0400B2E7 RID: 45799
		public GlobalTargetInfo? culpritTarget;
	}
}
