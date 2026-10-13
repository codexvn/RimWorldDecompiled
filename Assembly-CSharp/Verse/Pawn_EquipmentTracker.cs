using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x020007D2 RID: 2002
	public class Pawn_EquipmentTracker : IThingHolder, IExposable
	{
		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x060032D6 RID: 13014 RVA: 0x00109C9C File Offset: 0x00107E9C
		// (set) Token: 0x060032D7 RID: 13015 RVA: 0x00109CE8 File Offset: 0x00107EE8
		public ThingWithComps Primary
		{
			get
			{
				for (int i = 0; i < this.equipment.Count; i++)
				{
					if (this.equipment[i].def.equipmentType == EquipmentType.Primary)
					{
						return this.equipment[i];
					}
				}
				return null;
			}
			private set
			{
				if (this.Primary == value)
				{
					return;
				}
				if (value != null && value.def.equipmentType != EquipmentType.Primary)
				{
					Log.Error("Tried to set non-primary equipment as primary.");
					return;
				}
				if (this.Primary != null)
				{
					this.equipment.Remove(this.Primary);
				}
				if (value != null)
				{
					this.equipment.TryAdd(value, true);
				}
				if (this.pawn.drafter != null)
				{
					this.pawn.drafter.Notify_PrimaryWeaponChanged();
				}
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x060032D8 RID: 13016 RVA: 0x00109D63 File Offset: 0x00107F63
		public CompEquippable PrimaryEq
		{
			get
			{
				ThingWithComps primary = this.Primary;
				if (primary == null)
				{
					return null;
				}
				return primary.GetComp<CompEquippable>();
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x060032D9 RID: 13017 RVA: 0x00109D76 File Offset: 0x00107F76
		public List<ThingWithComps> AllEquipmentListForReading
		{
			get
			{
				return this.equipment.InnerListForReading;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x060032DA RID: 13018 RVA: 0x00109D83 File Offset: 0x00107F83
		public IEnumerable<Verb> AllEquipmentVerbs
		{
			get
			{
				List<ThingWithComps> list = this.AllEquipmentListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					ThingWithComps thingWithComps = list[i];
					List<Verb> verbs = thingWithComps.GetComp<CompEquippable>().AllVerbs;
					for (int j = 0; j < verbs.Count; j = num + 1)
					{
						yield return verbs[j];
						num = j;
					}
					verbs = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x060032DB RID: 13019 RVA: 0x00109D93 File Offset: 0x00107F93
		public IThingHolder ParentHolder
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x00109D9B File Offset: 0x00107F9B
		public Pawn_EquipmentTracker(Pawn newPawn)
		{
			this.pawn = newPawn;
			this.equipment = new ThingOwner<ThingWithComps>(this);
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x00109DB8 File Offset: 0x00107FB8
		public void ExposeData()
		{
			Scribe_Deep.Look<ThingOwner<ThingWithComps>>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_References.Look<Thing>(ref this.bondedWeapon, "bondedWeapon", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					foreach (Verb verb in allEquipmentListForReading[i].GetComp<CompEquippable>().AllVerbs)
					{
						verb.caster = this.pawn;
					}
				}
			}
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x00109E64 File Offset: 0x00108064
		public void EquipmentTrackerTick()
		{
			List<ThingWithComps> allEquipmentListForReading = this.AllEquipmentListForReading;
			for (int i = 0; i < allEquipmentListForReading.Count; i++)
			{
				ThingWithComps thingWithComps = allEquipmentListForReading[i];
				if (thingWithComps.def.tickerType != TickerType.Normal)
				{
					thingWithComps.GetComp<CompEquippable>().verbTracker.VerbsTick();
				}
			}
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x00109EAF File Offset: 0x001080AF
		public bool HasAnything()
		{
			return this.equipment.Any;
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x00109EBC File Offset: 0x001080BC
		public void MakeRoomFor(ThingWithComps eq)
		{
			ThingWithComps thingWithComps;
			this.MakeRoomFor(eq, out thingWithComps);
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x00109ED4 File Offset: 0x001080D4
		public void MakeRoomFor(ThingWithComps eq, out ThingWithComps dropped)
		{
			dropped = null;
			if (eq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				if (this.TryDropEquipment(this.Primary, out dropped, this.pawn.Position, true))
				{
					if (dropped != null)
					{
						dropped.SetForbidden(false, true);
						return;
					}
				}
				else
				{
					Pawn pawn = this.pawn;
					Log.Error(((pawn != null) ? pawn.ToString() : null) + " couldn't make room for equipment " + ((eq != null) ? eq.ToString() : null));
				}
			}
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x00109F51 File Offset: 0x00108151
		public void Remove(ThingWithComps eq)
		{
			this.equipment.Remove(eq);
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x00109F60 File Offset: 0x00108160
		public bool TryDropEquipment(ThingWithComps eq, out ThingWithComps resultingEq, IntVec3 pos, bool forbid = true)
		{
			if (!pos.IsValid)
			{
				Pawn pawn = this.pawn;
				Log.Error(((pawn != null) ? pawn.ToString() : null) + " tried to drop " + ((eq != null) ? eq.ToString() : null) + " at invalid cell.");
				resultingEq = null;
				return false;
			}
			if (this.equipment.TryDrop(eq, pos, this.pawn.MapHeld, ThingPlaceMode.Near, out resultingEq, null, null))
			{
				if (resultingEq != null)
				{
					resultingEq.SetForbidden(forbid, false);
				}
				return true;
			}
			return false;
		}

		// Token: 0x060032E4 RID: 13028 RVA: 0x00109FE0 File Offset: 0x001081E0
		public void DropAllEquipment(IntVec3 pos, bool forbid = true, bool rememberPrimary = false)
		{
			for (int i = this.equipment.Count - 1; i >= 0; i--)
			{
				bool flag = this.equipment[i] == this.Primary;
				ThingWithComps droppedWeapon;
				if (this.TryDropEquipment(this.equipment[i], out droppedWeapon, pos, forbid) && rememberPrimary && flag)
				{
					this.pawn.mindState.droppedWeapon = droppedWeapon;
				}
			}
		}

		// Token: 0x060032E5 RID: 13029 RVA: 0x0010A047 File Offset: 0x00108247
		public bool TryTransferEquipmentToContainer(ThingWithComps eq, ThingOwner container)
		{
			return this.equipment.TryTransferToContainer(eq, container, true);
		}

		// Token: 0x060032E6 RID: 13030 RVA: 0x0010A057 File Offset: 0x00108257
		public void DestroyEquipment(ThingWithComps eq)
		{
			if (!this.equipment.Contains(eq))
			{
				Log.Warning("Tried to destroy equipment " + ((eq != null) ? eq.ToString() : null) + " but it's not here.");
				return;
			}
			this.Remove(eq);
			eq.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x060032E7 RID: 13031 RVA: 0x0010A097 File Offset: 0x00108297
		public void DestroyAllEquipment(DestroyMode mode = DestroyMode.Vanish)
		{
			this.equipment.ClearAndDestroyContents(mode);
		}

		// Token: 0x060032E8 RID: 13032 RVA: 0x0010A0A5 File Offset: 0x001082A5
		public bool Contains(Thing eq)
		{
			return this.equipment.Contains(eq);
		}

		// Token: 0x060032E9 RID: 13033 RVA: 0x0010A0B3 File Offset: 0x001082B3
		internal void Notify_PrimaryDestroyed()
		{
			if (this.Primary != null)
			{
				this.Remove(this.Primary);
			}
			if (this.pawn.Spawned)
			{
				this.pawn.stances.CancelBusyStanceSoft();
			}
		}

		// Token: 0x060032EA RID: 13034 RVA: 0x0010A0E8 File Offset: 0x001082E8
		public void AddEquipment(ThingWithComps newEq)
		{
			if (newEq.def.equipmentType == EquipmentType.Primary && this.Primary != null)
			{
				string[] array = new string[6];
				array[0] = "Pawn ";
				array[1] = this.pawn.LabelCap;
				array[2] = " got primaryInt equipment ";
				array[3] = ((newEq != null) ? newEq.ToString() : null);
				array[4] = " while already having primaryInt equipment ";
				int num = 5;
				ThingWithComps primary = this.Primary;
				array[num] = ((primary != null) ? primary.ToString() : null);
				Log.Error(string.Concat(array));
				return;
			}
			if (this.equipment.TryAdd(newEq, true) && newEq.def.equipmentType == EquipmentType.Primary)
			{
				this.pawn.mindState.droppedWeapon = null;
			}
		}

		// Token: 0x060032EB RID: 13035 RVA: 0x0010A195 File Offset: 0x00108395
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (this.Primary != null)
			{
				foreach (Gizmo gizmo in this.PrimaryEq.CompGetEquippedGizmosExtra())
				{
					yield return gizmo;
				}
				IEnumerator<Gizmo> enumerator = null;
			}
			if (PawnAttackGizmoUtility.CanShowEquipmentGizmos())
			{
				try
				{
					Pawn_EquipmentTracker.tmpKeybindings.Add(KeyBindingDefOf.Misc1);
					Pawn_EquipmentTracker.tmpKeybindings.Add(KeyBindingDefOf.Misc2);
					Pawn_EquipmentTracker.tmpKeybindings.Add(KeyBindingDefOf.Misc3);
					List<ThingWithComps> list = this.AllEquipmentListForReading;
					ThingWithComps primaryMelee = list.FirstOrDefault((ThingWithComps w) => w.def.IsMeleeWeapon);
					ThingWithComps primaryRanged = list.FirstOrDefault((ThingWithComps w) => w.def.IsRangedWeapon);
					if (primaryMelee != null)
					{
						KeyBindingDef misc = KeyBindingDefOf.Misc2;
						foreach (Gizmo gizmo2 in Pawn_EquipmentTracker.<GetGizmos>g__YieldGizmos|30_0(primaryMelee, misc))
						{
							yield return gizmo2;
						}
						IEnumerator<Gizmo> enumerator = null;
					}
					if (primaryRanged != null)
					{
						KeyBindingDef misc2 = KeyBindingDefOf.Misc1;
						foreach (Gizmo gizmo3 in Pawn_EquipmentTracker.<GetGizmos>g__YieldGizmos|30_0(primaryRanged, misc2))
						{
							yield return gizmo3;
						}
						IEnumerator<Gizmo> enumerator = null;
					}
					int num;
					for (int i = 0; i < list.Count; i = num + 1)
					{
						ThingWithComps thingWithComps = list[i];
						if (thingWithComps != primaryMelee && thingWithComps != primaryRanged)
						{
							foreach (Gizmo gizmo4 in Pawn_EquipmentTracker.<GetGizmos>g__YieldGizmos|30_0(thingWithComps, null))
							{
								yield return gizmo4;
							}
							IEnumerator<Gizmo> enumerator = null;
						}
						num = i;
					}
					list = null;
					primaryMelee = null;
					primaryRanged = null;
				}
				finally
				{
					Pawn_EquipmentTracker.tmpKeybindings.Clear();
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x0010A1A8 File Offset: 0x001083A8
		public void Notify_EquipmentAdded(ThingWithComps eq)
		{
			foreach (Verb verb in eq.GetComp<CompEquippable>().AllVerbs)
			{
				verb.caster = this.pawn;
				verb.Notify_PickedUp();
			}
			eq.Notify_Equipped(this.pawn);
			if (ModsConfig.RoyaltyActive && eq.def.equipmentType == EquipmentType.Primary && this.bondedWeapon != null && !this.bondedWeapon.Destroyed)
			{
				CompBladelinkWeapon compBladelinkWeapon = this.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.Notify_WieldedOtherWeapon();
				}
			}
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x0010A254 File Offset: 0x00108454
		public void Notify_EquipmentRemoved(ThingWithComps eq)
		{
			eq.Notify_Unequipped(this.pawn);
			if (ModsConfig.RoyaltyActive)
			{
				CompBladelinkWeapon compBladelinkWeapon = eq.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.Notify_EquipmentLost(this.pawn);
				}
			}
			if (ModsConfig.OdysseyActive)
			{
				CompUniqueWeapon compUniqueWeapon = eq.TryGetComp<CompUniqueWeapon>();
				if (compUniqueWeapon == null)
				{
					return;
				}
				compUniqueWeapon.Notify_EquipmentLost(this.pawn);
			}
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x0010A2A8 File Offset: 0x001084A8
		public void Notify_AbilityUsed(Ability ability)
		{
			CompEquippableAbility compEquippableAbility = this.PrimaryEq as CompEquippableAbility;
			if (compEquippableAbility != null && ability == compEquippableAbility.AbilityForReading)
			{
				compEquippableAbility.UsedOnce();
			}
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x0010A2D4 File Offset: 0x001084D4
		public void Notify_PawnSpawned()
		{
			if (this.pawn.BeingTransportedOnGravship)
			{
				return;
			}
			if (this.HasAnything() && this.pawn.Downed && !this.pawn.GetPosture().InBed())
			{
				if (this.pawn.kindDef.destroyGearOnDrop)
				{
					this.DestroyAllEquipment(DestroyMode.Vanish);
					return;
				}
				this.DropAllEquipment(this.pawn.Position, true, false);
			}
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x0010A344 File Offset: 0x00108544
		public void Notify_PawnDied()
		{
			if (ModsConfig.RoyaltyActive && this.bondedWeapon != null)
			{
				CompBladelinkWeapon compBladelinkWeapon = this.bondedWeapon.TryGetComp<CompBladelinkWeapon>();
				if (compBladelinkWeapon != null)
				{
					compBladelinkWeapon.UnCode();
				}
			}
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x0010A378 File Offset: 0x00108578
		public void Notify_KilledPawn()
		{
			foreach (ThingWithComps thingWithComps in this.equipment)
			{
				thingWithComps.Notify_KilledPawn(this.pawn);
			}
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x0010A3D0 File Offset: 0x001085D0
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.equipment;
		}

		// Token: 0x060032F3 RID: 13043 RVA: 0x0010A3D8 File Offset: 0x001085D8
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x0010A3F2 File Offset: 0x001085F2
		[CompilerGenerated]
		internal static IEnumerable<Gizmo> <GetGizmos>g__YieldGizmos|30_0(ThingWithComps eq, KeyBindingDef preferredHotKey = null)
		{
			foreach (Command command in eq.GetComp<CompEquippable>().GetVerbsCommands())
			{
				if (Pawn_EquipmentTracker.tmpKeybindings.Count > 0)
				{
					if (preferredHotKey != null && Pawn_EquipmentTracker.tmpKeybindings.Contains(preferredHotKey))
					{
						command.hotKey = preferredHotKey;
						Pawn_EquipmentTracker.tmpKeybindings.Remove(preferredHotKey);
					}
					else
					{
						command.hotKey = Pawn_EquipmentTracker.tmpKeybindings.Pop<KeyBindingDef>();
					}
				}
				yield return command;
			}
			IEnumerator<Command> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04002696 RID: 9878
		public Pawn pawn;

		// Token: 0x04002697 RID: 9879
		private ThingOwner<ThingWithComps> equipment;

		// Token: 0x04002698 RID: 9880
		public Thing bondedWeapon;

		// Token: 0x04002699 RID: 9881
		private static List<KeyBindingDef> tmpKeybindings = new List<KeyBindingDef>();
	}
}
