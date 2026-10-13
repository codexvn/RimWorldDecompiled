using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x0200072D RID: 1837
	public class HediffWithComps : Hediff
	{
		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002CA9 RID: 11433 RVA: 0x000E7128 File Offset: 0x000E5328
		public override string LabelBase
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<HediffComp> list = this.comps;
					int? num3 = (list != null) ? new int?(list.Count) : null;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					string compLabelPrefix = this.comps[num].CompLabelPrefix;
					if (!compLabelPrefix.NullOrEmpty())
					{
						stringBuilder.Append(compLabelPrefix);
						stringBuilder.Append(" ");
					}
					num++;
				}
				stringBuilder.Append(base.LabelBase);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002CAA RID: 11434 RVA: 0x000E71BC File Offset: 0x000E53BC
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<HediffComp> list = this.comps;
					int? num3 = (list != null) ? new int?(list.Count) : null;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					string compLabelInBracketsExtra = this.comps[num].CompLabelInBracketsExtra;
					if (!compLabelInBracketsExtra.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append(compLabelInBracketsExtra);
					}
					num++;
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002CAB RID: 11435 RVA: 0x000E7258 File Offset: 0x000E5458
		public override bool ShouldRemove
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompShouldRemove)
						{
							return true;
						}
					}
				}
				return base.ShouldRemove;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002CAC RID: 11436 RVA: 0x000E72A0 File Offset: 0x000E54A0
		public override bool Visible
		{
			get
			{
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						if (this.comps[i].CompDisallowVisible())
						{
							return false;
						}
					}
				}
				return base.Visible;
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06002CAD RID: 11437 RVA: 0x000E72E8 File Offset: 0x000E54E8
		public override string TipStringExtra
		{
			get
			{
				bool flag = false;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string compTipStringExtra = this.comps[i].CompTipStringExtra;
						if (!compTipStringExtra.NullOrEmpty())
						{
							if (stringBuilder.Length > 0 && !flag)
							{
								stringBuilder.AppendLine();
								flag = true;
							}
							stringBuilder.AppendLine(compTipStringExtra);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06002CAE RID: 11438 RVA: 0x000E7368 File Offset: 0x000E5568
		public override string Description
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder(base.Description);
				int num = 0;
				for (;;)
				{
					int num2 = num;
					List<HediffComp> list = this.comps;
					int? num3 = (list != null) ? new int?(list.Count) : null;
					if (!(num2 < num3.GetValueOrDefault() & num3 != null))
					{
						break;
					}
					string compDescriptionExtra = this.comps[num].CompDescriptionExtra;
					if (!compDescriptionExtra.NullOrEmpty())
					{
						stringBuilder.Append(" ");
						stringBuilder.Append(compDescriptionExtra);
					}
					num++;
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06002CAF RID: 11439 RVA: 0x000E73F4 File Offset: 0x000E55F4
		public override TextureAndColor StateIcon
		{
			get
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					TextureAndColor compStateIcon = this.comps[i].CompStateIcon;
					if (compStateIcon.HasValue)
					{
						return compStateIcon;
					}
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x06002CB0 RID: 11440 RVA: 0x000E7439 File Offset: 0x000E5639
		public override IEnumerable<Gizmo> GetGizmos()
		{
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				IEnumerable<Gizmo> enumerable = this.comps[i].CompGetGizmos();
				if (enumerable != null)
				{
					foreach (Gizmo gizmo in enumerable)
					{
						yield return gizmo;
					}
					IEnumerator<Gizmo> enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06002CB1 RID: 11441 RVA: 0x000E744C File Offset: 0x000E564C
		public override void CopyFrom(Hediff other)
		{
			base.CopyFrom(other);
			HediffWithComps hediffWithComps = other as HediffWithComps;
			if (hediffWithComps != null)
			{
				foreach (HediffComp hediffComp in this.comps)
				{
					foreach (HediffComp hediffComp2 in hediffWithComps.comps)
					{
						if (!(hediffComp.GetType() != hediffComp2.GetType()))
						{
							hediffComp.CopyFrom(hediffComp2);
						}
					}
				}
			}
		}

		// Token: 0x06002CB2 RID: 11442 RVA: 0x000E7504 File Offset: 0x000E5704
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostAdd(dinfo);
				}
			}
		}

		// Token: 0x06002CB3 RID: 11443 RVA: 0x000E7548 File Offset: 0x000E5748
		public override void PostRemoved()
		{
			base.PostRemoved();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostPostRemoved();
				}
			}
		}

		// Token: 0x06002CB4 RID: 11444 RVA: 0x000E758C File Offset: 0x000E578C
		public override void PostTick()
		{
			if (this.comps == null)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPostTick(ref num);
			}
			if (num != 0f)
			{
				this.Severity += num;
			}
		}

		// Token: 0x06002CB5 RID: 11445 RVA: 0x000E75E8 File Offset: 0x000E57E8
		public override void PostTickInterval(int delta)
		{
			if (this.comps == null)
			{
				return;
			}
			float num = 0f;
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompPostTickInterval(ref num, delta);
			}
			if (num != 0f)
			{
				this.Severity += num;
			}
		}

		// Token: 0x06002CB6 RID: 11446 RVA: 0x000E7644 File Offset: 0x000E5844
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompExposeData();
				}
			}
		}

		// Token: 0x06002CB7 RID: 11447 RVA: 0x000E7694 File Offset: 0x000E5894
		public override void Tended(float quality, float maxQuality, int batchPosition = 0)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompTended(quality, maxQuality, batchPosition);
			}
		}

		// Token: 0x06002CB8 RID: 11448 RVA: 0x000E76CC File Offset: 0x000E58CC
		public override bool TryMergeWith(Hediff other)
		{
			if (base.TryMergeWith(other))
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostMerged(other);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06002CB9 RID: 11449 RVA: 0x000E7710 File Offset: 0x000E5910
		public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
		{
			base.Notify_PawnDied(dinfo, culprit);
			for (int i = this.comps.Count - 1; i >= 0; i--)
			{
				this.comps[i].Notify_PawnDied(dinfo, culprit);
			}
		}

		// Token: 0x06002CBA RID: 11450 RVA: 0x000E7750 File Offset: 0x000E5950
		public override void Notify_PawnKilled()
		{
			base.Notify_PawnKilled();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnKilled();
			}
		}

		// Token: 0x06002CBB RID: 11451 RVA: 0x000E778C File Offset: 0x000E598C
		public override void Notify_KilledPawn(Pawn victim, DamageInfo? dinfo)
		{
			base.Notify_KilledPawn(victim, dinfo);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_KilledPawn(victim, dinfo);
			}
		}

		// Token: 0x06002CBC RID: 11452 RVA: 0x000E77CC File Offset: 0x000E59CC
		public override void Notify_PawnPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnPostApplyDamage(dinfo, totalDamageDealt);
			}
		}

		// Token: 0x06002CBD RID: 11453 RVA: 0x000E780C File Offset: 0x000E5A0C
		public override void ModifyChemicalEffect(ChemicalDef chem, ref float effect)
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].CompModifyChemicalEffect(chem, ref effect);
			}
		}

		// Token: 0x06002CBE RID: 11454 RVA: 0x000E7844 File Offset: 0x000E5A44
		public override void Notify_PawnUsedVerb(Verb verb, LocalTargetInfo target)
		{
			base.Notify_PawnUsedVerb(verb, target);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_PawnUsedVerb(verb, target);
			}
		}

		// Token: 0x06002CBF RID: 11455 RVA: 0x000E7884 File Offset: 0x000E5A84
		public override void Notify_EntropyGained(float baseAmount, float finalAmount, Thing src = null)
		{
			base.Notify_EntropyGained(baseAmount, finalAmount, src);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_EntropyGained(baseAmount, finalAmount, src);
			}
		}

		// Token: 0x06002CC0 RID: 11456 RVA: 0x000E78C4 File Offset: 0x000E5AC4
		public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
			base.Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_ImplantUsed(violationSourceName, detectionChance, violationSourceLevel);
			}
		}

		// Token: 0x06002CC1 RID: 11457 RVA: 0x000E7904 File Offset: 0x000E5B04
		public override void Notify_Spawned()
		{
			base.Notify_Spawned();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_Spawned();
			}
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x000E7940 File Offset: 0x000E5B40
		public override void Notify_SurgicallyRemoved(Pawn surgeon)
		{
			base.Notify_SurgicallyRemoved(surgeon);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_SurgicallyRemoved(surgeon);
			}
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x000E797C File Offset: 0x000E5B7C
		public override void Notify_SurgicallyReplaced(Pawn surgeon)
		{
			base.Notify_SurgicallyReplaced(surgeon);
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].Notify_SurgicallyReplaced(surgeon);
			}
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000E79B8 File Offset: 0x000E5BB8
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			for (int i = this.comps.Count - 1; i >= 0; i--)
			{
				try
				{
					this.comps[i].CompPostMake();
				}
				catch (Exception ex)
				{
					string str = "Error in HediffComp.CompPostMake(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
					this.comps.RemoveAt(i);
				}
			}
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x000E7A38 File Offset: 0x000E5C38
		private void InitializeComps()
		{
			if (this.def.comps != null)
			{
				this.comps = new List<HediffComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					HediffComp hediffComp = null;
					try
					{
						hediffComp = (HediffComp)Activator.CreateInstance(this.def.comps[i].compClass);
						hediffComp.props = this.def.comps[i];
						hediffComp.parent = this;
						this.comps.Add(hediffComp);
					}
					catch (Exception ex)
					{
						string str = "Could not instantiate or initialize a HediffComp: ";
						Exception ex2 = ex;
						Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
						this.comps.Remove(hediffComp);
					}
				}
			}
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x000E7B0C File Offset: 0x000E5D0C
		public T GetComp<T>() where T : HediffComp
		{
			for (int i = 0; i < this.comps.Count; i++)
			{
				if (this.comps[i] is T)
				{
					return this.comps[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x000E7B64 File Offset: 0x000E5D64
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.DebugString());
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					string str;
					if (this.comps[i].ToString().Contains('_'))
					{
						str = this.comps[i].ToString().Split('_', StringSplitOptions.None)[1];
					}
					else
					{
						str = this.comps[i].ToString();
					}
					stringBuilder.AppendLine("--" + str);
					string text = this.comps[i].CompDebugString();
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text.TrimEnd().Indented("    "));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040023B1 RID: 9137
		public List<HediffComp> comps = new List<HediffComp>();
	}
}
