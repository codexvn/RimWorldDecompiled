using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B41 RID: 6977
	public class DrugPolicyDatabase : IExposable
	{
		// Token: 0x170017FD RID: 6141
		// (get) Token: 0x06009BCE RID: 39886 RVA: 0x002D2ADE File Offset: 0x002D0CDE
		public List<DrugPolicy> AllPolicies
		{
			get
			{
				return this.policies;
			}
		}

		// Token: 0x06009BCF RID: 39887 RVA: 0x002D2AE6 File Offset: 0x002D0CE6
		public DrugPolicyDatabase()
		{
			this.GenerateStartingDrugPolicies();
		}

		// Token: 0x06009BD0 RID: 39888 RVA: 0x002D2AFF File Offset: 0x002D0CFF
		public void ExposeData()
		{
			Scribe_Collections.Look<DrugPolicy>(ref this.policies, "policies", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06009BD1 RID: 39889 RVA: 0x002D2B17 File Offset: 0x002D0D17
		public DrugPolicy DefaultDrugPolicy()
		{
			if (this.policies.Count == 0)
			{
				this.MakeNewDrugPolicy();
			}
			return this.policies[0];
		}

		// Token: 0x06009BD2 RID: 39890 RVA: 0x002D2B3C File Offset: 0x002D0D3C
		public void SetDefault(DrugPolicy policy)
		{
			int index = this.policies.IndexOf(policy);
			DrugPolicy value = this.policies[0];
			this.policies[0] = policy;
			this.policies[index] = value;
		}

		// Token: 0x06009BD3 RID: 39891 RVA: 0x002D2B80 File Offset: 0x002D0D80
		public void MakePolicyDefault(DrugPolicyDef policyDef)
		{
			if (this.DefaultDrugPolicy().sourceDef == policyDef)
			{
				return;
			}
			DrugPolicy drugPolicy = this.policies.FirstOrDefault((DrugPolicy x) => x.sourceDef == policyDef);
			if (drugPolicy != null)
			{
				this.policies.Remove(drugPolicy);
				this.policies.Insert(0, drugPolicy);
			}
		}

		// Token: 0x06009BD4 RID: 39892 RVA: 0x002D2BE4 File Offset: 0x002D0DE4
		public AcceptanceReport TryDelete(DrugPolicy policy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
			{
				if (pawn.drugs != null && pawn.drugs.CurrentPolicy == policy)
				{
					return new AcceptanceReport("DrugPolicyInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.drugs != null && pawn2.drugs.CurrentPolicy == policy)
				{
					pawn2.drugs.CurrentPolicy = null;
				}
			}
			this.policies.Remove(policy);
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06009BD5 RID: 39893 RVA: 0x002D2CD4 File Offset: 0x002D0ED4
		public DrugPolicy MakeNewDrugPolicy()
		{
			int num;
			if (!this.policies.Any<DrugPolicy>())
			{
				num = 1;
			}
			else
			{
				num = this.policies.Max((DrugPolicy o) => o.id) + 1;
			}
			int id = num;
			DrugPolicy drugPolicy = new DrugPolicy(id, "DrugPolicy".Translate() + " " + id.ToString());
			this.policies.Add(drugPolicy);
			return drugPolicy;
		}

		// Token: 0x06009BD6 RID: 39894 RVA: 0x002D2D58 File Offset: 0x002D0F58
		public DrugPolicy NewDrugPolicyFromDef(DrugPolicyDef def)
		{
			DrugPolicy drugPolicy = this.MakeNewDrugPolicy();
			drugPolicy.label = def.LabelCap;
			drugPolicy.sourceDef = def;
			if (def.allowPleasureDrugs)
			{
				for (int i = 0; i < drugPolicy.Count; i++)
				{
					if (drugPolicy[i].drug.IsPleasureDrug)
					{
						drugPolicy[i].allowedForJoy = true;
					}
				}
			}
			if (def.entries != null)
			{
				for (int j = 0; j < def.entries.Count; j++)
				{
					drugPolicy[def.entries[j].drug].CopyFrom(def.entries[j]);
				}
			}
			return drugPolicy;
		}

		// Token: 0x06009BD7 RID: 39895 RVA: 0x002D2E04 File Offset: 0x002D1004
		private void GenerateStartingDrugPolicies()
		{
			foreach (DrugPolicyDef def in DefDatabase<DrugPolicyDef>.AllDefs)
			{
				this.NewDrugPolicyFromDef(def);
			}
		}

		// Token: 0x04006C2D RID: 27693
		private List<DrugPolicy> policies = new List<DrugPolicy>();
	}
}
