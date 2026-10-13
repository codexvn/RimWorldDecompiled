using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020027EA RID: 10218
	public class RitualVisualEffect : IExposable
	{
		// Token: 0x0600DFB5 RID: 57269 RVA: 0x0041B1E0 File Offset: 0x004193E0
		public void Setup(LordJob_Ritual r, bool loading)
		{
			this.ritual = r;
			if (!loading)
			{
				using (List<CompProperties_RitualVisualEffect>.Enumerator enumerator = this.def.comps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompProperties_RitualVisualEffect compProperties_RitualVisualEffect = enumerator.Current;
						RitualVisualEffectComp instance = compProperties_RitualVisualEffect.GetInstance();
						instance.props = compProperties_RitualVisualEffect;
						instance.OnSetup(this, this.ritual, false);
						this.comps.Add(instance);
					}
					return;
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				RitualVisualEffectComp ritualVisualEffectComp = this.comps[i];
				ritualVisualEffectComp.props = this.def.comps[i];
				ritualVisualEffectComp.OnSetup(this, this.ritual, true);
			}
		}

		// Token: 0x0600DFB6 RID: 57270 RVA: 0x0041B2AC File Offset: 0x004194AC
		public void Tick()
		{
			foreach (RitualVisualEffectComp ritualVisualEffectComp in this.comps)
			{
				ritualVisualEffectComp.TickInterval(1);
			}
			foreach (Mote mote in this.maintainedMotes)
			{
				mote.Maintain();
			}
			foreach (Pair<Effecter, TargetInfo> pair in this.maintainedEffecters)
			{
				pair.First.EffectTick(pair.Second, pair.Second);
			}
		}

		// Token: 0x0600DFB7 RID: 57271 RVA: 0x0041B394 File Offset: 0x00419594
		public void AddMoteToMaintain(Mote mote)
		{
			this.maintainedMotes.Add(mote);
		}

		// Token: 0x0600DFB8 RID: 57272 RVA: 0x0041B3A2 File Offset: 0x004195A2
		public void AddEffecterToMaintain(TargetInfo target, Effecter eff)
		{
			this.maintainedEffecters.Add(new Pair<Effecter, TargetInfo>(eff, target));
		}

		// Token: 0x0600DFB9 RID: 57273 RVA: 0x0041B3B8 File Offset: 0x004195B8
		public void Cleanup()
		{
			foreach (Pair<Effecter, TargetInfo> pair in this.maintainedEffecters)
			{
				pair.First.Cleanup();
			}
		}

		// Token: 0x0600DFBA RID: 57274 RVA: 0x0041B410 File Offset: 0x00419610
		public void ExposeData()
		{
			Scribe_Defs.Look<RitualVisualEffectDef>(ref this.def, "def");
			Scribe_Collections.Look<RitualVisualEffectComp>(ref this.comps, "comps", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x04009689 RID: 38537
		public LordJob_Ritual ritual;

		// Token: 0x0400968A RID: 38538
		public RitualVisualEffectDef def;

		// Token: 0x0400968B RID: 38539
		public List<RitualVisualEffectComp> comps = new List<RitualVisualEffectComp>();

		// Token: 0x0400968C RID: 38540
		private List<Mote> maintainedMotes = new List<Mote>();

		// Token: 0x0400968D RID: 38541
		private List<Pair<Effecter, TargetInfo>> maintainedEffecters = new List<Pair<Effecter, TargetInfo>>();
	}
}
