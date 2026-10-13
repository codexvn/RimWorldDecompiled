using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200078D RID: 1933
	public class ImmunityHandler : IExposable
	{
		// Token: 0x17000894 RID: 2196
		// (get) Token: 0x06002F88 RID: 12168 RVA: 0x000F434F File Offset: 0x000F254F
		public List<ImmunityRecord> ImmunityListForReading
		{
			get
			{
				return this.immunityList;
			}
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x000F4357 File Offset: 0x000F2557
		public ImmunityHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x000F4371 File Offset: 0x000F2571
		public void ExposeData()
		{
			Scribe_Collections.Look<ImmunityRecord>(ref this.immunityList, "imList", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06002F8B RID: 12171 RVA: 0x000F438C File Offset: 0x000F258C
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, BodyPartRecord part = null)
		{
			HediffDef hediffDef = null;
			return this.DiseaseContractChanceFactor(diseaseDef, out hediffDef, part);
		}

		// Token: 0x06002F8C RID: 12172 RVA: 0x000F43A8 File Offset: 0x000F25A8
		public float DiseaseContractChanceFactor(HediffDef diseaseDef, out HediffDef immunityCause, BodyPartRecord part = null)
		{
			immunityCause = null;
			if (!this.pawn.RaceProps.IsFlesh || this.pawn.RaceProps.isImmuneToInfections)
			{
				return 0f;
			}
			if (this.pawn.IsMutant && this.pawn.mutant.Def.preventIllnesses && this.pawn.mutant.Def.isImmuneToInfections)
			{
				return 0f;
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo(diseaseDef, out hediff))
			{
				immunityCause = hediff.def;
				return 0f;
			}
			if (this.AnyGeneMakesFullyImmuneTo(diseaseDef))
			{
				return 0f;
			}
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def == diseaseDef && hediffs[i].Part == part)
				{
					return 0f;
				}
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				if (this.immunityList[j].hediffDef == diseaseDef)
				{
					immunityCause = this.immunityList[j].source;
					return Mathf.Lerp(1f, 0f, this.immunityList[j].immunity / 0.6f);
				}
			}
			return 1f;
		}

		// Token: 0x06002F8D RID: 12173 RVA: 0x000F4500 File Offset: 0x000F2700
		public float GetImmunity(HediffDef def, bool naturalImmunityOnly = false)
		{
			float num = 0f;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				ImmunityRecord immunityRecord = this.immunityList[i];
				if (immunityRecord.hediffDef == def)
				{
					num = immunityRecord.immunity;
					break;
				}
			}
			Hediff hediff;
			if (!naturalImmunityOnly && (this.AnyHediffMakesFullyImmuneTo(def, out hediff) || this.AnyGeneMakesFullyImmuneTo(def)) && num < 0.65000004f)
			{
				num = 0.65000004f;
			}
			return num;
		}

		// Token: 0x06002F8E RID: 12174 RVA: 0x000F4570 File Offset: 0x000F2770
		internal void ImmunityHandlerTickInterval(int delta)
		{
			List<ImmunityHandler.ImmunityInfo> list = this.NeededImmunitiesNow();
			for (int i = 0; i < list.Count; i++)
			{
				this.TryAddImmunityRecord(list[i].immunity, list[i].source);
			}
			for (int j = 0; j < this.immunityList.Count; j++)
			{
				ImmunityRecord immunityRecord = this.immunityList[j];
				Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(immunityRecord.hediffDef, false);
				immunityRecord.ImmunityTickInterval(this.pawn, firstHediffOfDef != null, firstHediffOfDef, delta);
			}
			for (int k = this.immunityList.Count - 1; k >= 0; k--)
			{
				if (this.immunityList[k].immunity <= 0f)
				{
					bool flag = false;
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].immunity == this.immunityList[k].hediffDef)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.immunityList.RemoveAt(k);
					}
				}
			}
		}

		// Token: 0x06002F8F RID: 12175 RVA: 0x000F468C File Offset: 0x000F288C
		private List<ImmunityHandler.ImmunityInfo> NeededImmunitiesNow()
		{
			ImmunityHandler.tmpNeededImmunitiesNow.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff hediff = hediffs[i];
				if (hediff.def.PossibleToDevelopImmunityNaturally())
				{
					ImmunityHandler.tmpNeededImmunitiesNow.Add(new ImmunityHandler.ImmunityInfo
					{
						immunity = hediff.def,
						source = hediff.def
					});
				}
			}
			return ImmunityHandler.tmpNeededImmunitiesNow;
		}

		// Token: 0x06002F90 RID: 12176 RVA: 0x000F4714 File Offset: 0x000F2914
		private bool AnyHediffMakesFullyImmuneTo(HediffDef def, out Hediff sourceHediff)
		{
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						if (curStage.makeImmuneTo[j] == def)
						{
							sourceHediff = hediffs[i];
							return true;
						}
					}
				}
			}
			sourceHediff = null;
			return false;
		}

		// Token: 0x06002F91 RID: 12177 RVA: 0x000F4794 File Offset: 0x000F2994
		public bool AnyGeneMakesFullyImmuneTo(HediffDef def)
		{
			if (!ModsConfig.BiotechActive || this.pawn.genes == null)
			{
				return false;
			}
			for (int i = 0; i < this.pawn.genes.GenesListForReading.Count; i++)
			{
				Gene gene = this.pawn.genes.GenesListForReading[i];
				if (gene.def.makeImmuneTo != null)
				{
					for (int j = 0; j < gene.def.makeImmuneTo.Count; j++)
					{
						if (gene.def.makeImmuneTo[j] == def)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002F92 RID: 12178 RVA: 0x000F4830 File Offset: 0x000F2A30
		public void TryAddImmunityRecord(HediffDef def, HediffDef source)
		{
			if (def.CompProps<HediffCompProperties_Immunizable>() == null)
			{
				return;
			}
			if (this.ImmunityRecordExists(def))
			{
				return;
			}
			ImmunityRecord immunityRecord = new ImmunityRecord();
			immunityRecord.hediffDef = def;
			immunityRecord.source = source;
			this.immunityList.Add(immunityRecord);
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x000F4870 File Offset: 0x000F2A70
		public ImmunityRecord GetImmunityRecord(HediffDef def)
		{
			ImmunityRecord immunityRecord = null;
			for (int i = 0; i < this.immunityList.Count; i++)
			{
				if (this.immunityList[i].hediffDef == def)
				{
					immunityRecord = this.immunityList[i];
					break;
				}
			}
			Hediff hediff;
			if (this.AnyHediffMakesFullyImmuneTo(def, out hediff) && (immunityRecord == null || immunityRecord.immunity < 0.65000004f))
			{
				immunityRecord = new ImmunityRecord
				{
					immunity = 0.65000004f,
					hediffDef = def,
					source = hediff.def
				};
			}
			return immunityRecord;
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x000F48F8 File Offset: 0x000F2AF8
		public bool ImmunityRecordExists(HediffDef def)
		{
			return this.GetImmunityRecord(def) != null;
		}

		// Token: 0x040024FF RID: 9471
		public Pawn pawn;

		// Token: 0x04002500 RID: 9472
		private List<ImmunityRecord> immunityList = new List<ImmunityRecord>();

		// Token: 0x04002501 RID: 9473
		private const float ForcedImmunityLevel = 0.65000004f;

		// Token: 0x04002502 RID: 9474
		private static readonly List<ImmunityHandler.ImmunityInfo> tmpNeededImmunitiesNow = new List<ImmunityHandler.ImmunityInfo>();

		// Token: 0x0200078E RID: 1934
		public struct ImmunityInfo
		{
			// Token: 0x04002503 RID: 9475
			public HediffDef immunity;

			// Token: 0x04002504 RID: 9476
			public HediffDef source;
		}
	}
}
