using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EA8 RID: 7848
	public abstract class ComplexThreatWorker
	{
		// Token: 0x0600AE17 RID: 44567 RVA: 0x0031A59C File Offset: 0x0031879C
		public void Resolve(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings, StringBuilder debug = null)
		{
			try
			{
				bool flag = false;
				if (parms.triggerSignal.NullOrEmpty())
				{
					string triggerSignal;
					if (this.TryGetThingTriggerSignal(parms, out triggerSignal))
					{
						parms.triggerSignal = triggerSignal;
						if (debug != null)
						{
							debug.AppendLine("--> Threat trigger: Existing thing");
						}
					}
					else if (this.def.fallbackToRoomEnteredTrigger)
					{
						parms.triggerSignal = ComplexUtility.SpawnRoomEnteredTrigger(parms.room, parms.map);
						flag = true;
						if (debug != null)
						{
							debug.AppendLine("--> Threat trigger: Room entry");
						}
					}
					else
					{
						if (!this.def.allowPassive)
						{
							Log.Warning("Unable to generate a trigger for threat " + this.def.defName);
							return;
						}
						parms.triggerSignal = this.def.defName + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
						parms.passive = true;
						if (debug != null)
						{
							debug.AppendLine("--> Threat trigger: None. Passive threat.");
						}
					}
				}
				float points = parms.points;
				if (!parms.passive && Rand.Chance(this.def.delayChance))
				{
					int num = this.def.delayTickOptions.RandomElement<int>();
					parms.points = points;
					parms.delayTicks = new int?(num);
					float num2 = this.def.threatFactorOverDelayTicksCurve.Evaluate((float)num);
					parms.points *= num2;
					if (debug != null)
					{
						debug.AppendLine(string.Format("--> Threat delay ticks: {0}", num));
						debug.AppendLine(string.Format("--> Threat delay points factor: {0}", num2));
						debug.AppendLine(string.Format("--> Threat points post delay factor: {0}", parms.points));
					}
				}
				if (!parms.passive && !flag && Rand.Chance(this.def.spawnInOtherRoomChance))
				{
					ComplexResolveParams parms2 = parms;
					foreach (LayoutRoom room in parms.allRooms.InRandomOrder(null))
					{
						parms2.room = room;
						if (this.def.Worker.CanResolve(parms2))
						{
							parms.room = room;
							break;
						}
					}
				}
				float num3 = 0f;
				this.ResolveInt(parms, ref num3, outSpawnedThings);
				if (parms.passive)
				{
					num3 *= this.def.postSpawnPassiveThreatFactor;
					if (debug != null)
					{
						debug.AppendLine(string.Format("--> Threat post spawn passive factor: {0}", this.def.postSpawnPassiveThreatFactor));
					}
				}
				if (debug != null)
				{
					debug.AppendLine(string.Format("--> Total points used: {0}", num3));
				}
				threatPointsUsed += num3;
			}
			catch (Exception ex)
			{
				string str = "Exception resolving ";
				string name = base.GetType().Name;
				string str2 = ": ";
				Exception ex2 = ex;
				Log.Error(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
			}
		}

		// Token: 0x0600AE18 RID: 44568 RVA: 0x0031A8B0 File Offset: 0x00318AB0
		private bool TryGetThingTriggerSignal(ComplexResolveParams threatParams, out string triggerSignal)
		{
			if (threatParams.room == null || threatParams.spawnedThings.NullOrEmpty<Thing>())
			{
				triggerSignal = null;
				return false;
			}
			LayoutRoom room = threatParams.room;
			for (int i = 0; i < threatParams.spawnedThings.Count; i++)
			{
				Thing thing = threatParams.spawnedThings[i];
				if (room.rects.Any((CellRect r) => r.Contains(thing.Position)))
				{
					CompHackable compHackable = thing.TryGetComp<CompHackable>();
					if (compHackable != null && !compHackable.IsHacked)
					{
						if (Rand.Bool)
						{
							if (compHackable.hackingStartedSignal == null)
							{
								compHackable.hackingStartedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
							}
							triggerSignal = compHackable.hackingStartedSignal;
						}
						else
						{
							if (compHackable.hackingCompletedSignal == null)
							{
								compHackable.hackingCompletedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
							}
							triggerSignal = compHackable.hackingCompletedSignal;
						}
						return true;
					}
					Building_Casket building_Casket = thing as Building_Casket;
					if (building_Casket != null && building_Casket.CanOpen)
					{
						if (building_Casket.openedSignal.NullOrEmpty())
						{
							building_Casket.openedSignal = "ThreatTriggerSignal" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
						}
						triggerSignal = building_Casket.openedSignal;
						return true;
					}
				}
			}
			triggerSignal = null;
			return false;
		}

		// Token: 0x0600AE19 RID: 44569 RVA: 0x0031AA14 File Offset: 0x00318C14
		public bool CanResolve(ComplexResolveParams parms)
		{
			bool result;
			try
			{
				result = this.CanResolveInt(parms);
			}
			catch (Exception ex)
			{
				string str = "Exception test running ";
				string name = base.GetType().Name;
				string str2 = ": ";
				Exception ex2 = ex;
				Log.Error(str + name + str2 + ((ex2 != null) ? ex2.ToString() : null));
				result = false;
			}
			return result;
		}

		// Token: 0x0600AE1A RID: 44570
		protected abstract void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> spawnedThings);

		// Token: 0x0600AE1B RID: 44571 RVA: 0x0031AA70 File Offset: 0x00318C70
		protected virtual bool CanResolveInt(ComplexResolveParams parms)
		{
			if ((float)this.def.minPoints <= parms.points)
			{
				LayoutRoom room = parms.room;
				if (((room != null) ? room.requiredDef : null) == null)
				{
					return this.def.allowPassive || this.def.fallbackToRoomEnteredTrigger || !parms.triggerSignal.NullOrEmpty();
				}
			}
			return false;
		}

		// Token: 0x040077C6 RID: 30662
		public ComplexThreatDef def;

		// Token: 0x040077C7 RID: 30663
		private const string ThreatTriggerSignal = "ThreatTriggerSignal";
	}
}
