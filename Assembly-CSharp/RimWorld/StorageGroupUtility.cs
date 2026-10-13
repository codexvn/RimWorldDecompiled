using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020029B8 RID: 10680
	[StaticConstructorOnStartup]
	public static class StorageGroupUtility
	{
		// Token: 0x0600EB7E RID: 60286 RVA: 0x00446793 File Offset: 0x00444993
		public static IEnumerable<Gizmo> StorageGroupMemberGizmos(IStorageGroupMember member)
		{
			bool flag = true;
			StorageGroup storageGroup = null;
			StorageGroupUtility.tmpMembers.Clear();
			foreach (object obj in Find.Selector.SelectedObjects)
			{
				IStorageGroupMember storageGroupMember = obj as IStorageGroupMember;
				if (storageGroupMember != null && storageGroupMember.StorageGroupTag == member.StorageGroupTag)
				{
					Building_Storage building_Storage = storageGroupMember as Building_Storage;
					if ((building_Storage == null || building_Storage.StorageTabVisible) && storageGroupMember.Map == member.Map)
					{
						StorageGroupUtility.tmpMembers.Add(storageGroupMember);
						if (flag)
						{
							if (storageGroup == null && member.Group != null)
							{
								storageGroup = member.Group;
							}
							if (storageGroup != member.Group)
							{
								flag = false;
							}
						}
					}
				}
			}
			if (storageGroup == null)
			{
				flag = false;
			}
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "LinkStorageSettings".Translate();
			command_Action.defaultDesc = "LinkStorageSettingsDesc".Translate();
			command_Action.icon = StorageGroupUtility.LinkTex;
			command_Action.action = delegate()
			{
				bool flag2 = StorageGroupUtility.tmpMembers[0].Group != null;
				StorageGroup storageGroup2 = StorageGroupUtility.tmpMembers[0].Group ?? member.Map.storageGroups.NewGroup(null);
				if (!flag2)
				{
					storageGroup2.InitFrom(StorageGroupUtility.tmpMembers[0]);
				}
				foreach (IStorageGroupMember member2 in StorageGroupUtility.tmpMembers)
				{
					member2.SetStorageGroup(storageGroup2, true);
				}
				if (StorageGroupUtility.tmpMembers.Count > 1)
				{
					Messages.Message("SettingsLinkedFor".Translate(StorageGroupUtility.tmpMembers.Count), null, MessageTypeDefOf.NeutralEvent, false);
					return;
				}
				Messages.Message("SettingsLinkedForSingular".Translate(), null, MessageTypeDefOf.NeutralEvent, false);
			};
			command_Action.onHover = delegate()
			{
				if (StorageGroupUtility.tmpMembers.Any<IStorageGroupMember>())
				{
					Thing thing = StorageGroupUtility.tmpMembers[0] as Thing;
					if (thing != null)
					{
						TargetHighlighter.Highlight(thing, true, true, false);
					}
				}
			};
			Command_Action command_Action2 = command_Action;
			if (StorageGroupUtility.tmpMembers.Count < 2)
			{
				command_Action2.Disable("LinkStorageDisabledSelectTwo".Translate());
			}
			else if (flag)
			{
				command_Action2.Disable("AlreadyLinked".Translate());
			}
			yield return command_Action2;
			if (member.Group != null)
			{
				yield return new Command_Action
				{
					defaultLabel = "UnlinkStorageSettings".Translate(),
					defaultDesc = "UnlinkStorageSettingsDesc".Translate(),
					icon = StorageGroupUtility.UnlinkTex,
					action = delegate()
					{
						if (member.Group != null)
						{
							StorageSettings storeSettings = member.Group.GetStoreSettings();
							member.Group.RemoveMember(member, true);
							member.Group = null;
							Building_Storage building_Storage2 = member as Building_Storage;
							if (building_Storage2 != null)
							{
								building_Storage2.settings.CopyFrom(storeSettings);
							}
						}
						if (StorageGroupUtility.tmpMembers.Count > 1)
						{
							Messages.Message("SettingsUnlinkedFor".Translate(StorageGroupUtility.tmpMembers.Count), null, MessageTypeDefOf.NeutralEvent, false);
							return;
						}
						Messages.Message("SettingsUnlinkedForSingular".Translate(), null, MessageTypeDefOf.NeutralEvent, false);
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "SelectAllLinked".Translate(),
					defaultDesc = "SelectAllLinkedDesc".Translate(),
					icon = StorageGroupUtility.SelectLinkedTex,
					action = delegate()
					{
						bool flag2 = false;
						foreach (IStorageGroupMember storageGroupMember2 in member.Group.members)
						{
							if (!Find.Selector.IsSelected(storageGroupMember2))
							{
								Thing thing = storageGroupMember2 as Thing;
								if (thing == null || !thing.Destroyed)
								{
									Find.Selector.Select(storageGroupMember2, false, true);
									flag2 = true;
								}
							}
						}
						if (flag2)
						{
							SoundDefOf.ThingSelected.PlayOneShotOnCamera(null);
						}
					}
				};
			}
			yield break;
		}

		// Token: 0x0600EB7F RID: 60287 RVA: 0x004467A4 File Offset: 0x004449A4
		public static void DrawSelectionOverlaysFor(IStorageGroupMember member)
		{
			if (member.Group != null && !SelectionDrawer.DrawnStorageGroupThisFrame(member.Group))
			{
				foreach (IStorageGroupMember storageGroupMember in member.Group.members)
				{
					if (storageGroupMember.DrawConnectionOverlay)
					{
						SelectionDrawer.DrawSelectionBracketFor(storageGroupMember, StorageGroupUtility.GroupedMat);
					}
				}
				SelectionDrawer.Notify_DrawnStorageGroup(member.Group);
			}
		}

		// Token: 0x0600EB80 RID: 60288 RVA: 0x00446828 File Offset: 0x00444A28
		public static void SetStorageGroup(this IStorageGroupMember member, StorageGroup newGroup, bool removeIfEmpty = true)
		{
			if (member.Group != null && member.Group == newGroup)
			{
				return;
			}
			if (member.Group != null)
			{
				StorageSettings storeSettings = member.Group.GetStoreSettings();
				member.Group.RemoveMember(member, removeIfEmpty);
				member.Group = null;
				member.StoreSettings.CopyFrom(storeSettings);
			}
			if (newGroup != null)
			{
				member.Group = newGroup;
				member.Group.members.Add(member);
			}
			IStoreSettingsParent storeSettingsParent = member as IStoreSettingsParent;
			if (storeSettingsParent != null)
			{
				storeSettingsParent.Notify_SettingsChanged();
			}
		}

		// Token: 0x0600EB81 RID: 60289 RVA: 0x004468A6 File Offset: 0x00444AA6
		public static bool IsHopper(this Thing thing)
		{
			bool flag;
			if (thing == null)
			{
				flag = (null != null);
			}
			else
			{
				ThingDef def = thing.def;
				flag = (((def != null) ? def.building : null) != null);
			}
			return flag && thing.def.building.isHopper;
		}

		// Token: 0x04009C44 RID: 40004
		private static readonly Texture2D LinkTex = ContentFinder<Texture2D>.Get("UI/Commands/LinkStorageSettings", true);

		// Token: 0x04009C45 RID: 40005
		private static readonly Texture2D UnlinkTex = ContentFinder<Texture2D>.Get("UI/Commands/UnlinkStorageSettings", true);

		// Token: 0x04009C46 RID: 40006
		private static readonly Texture2D SelectLinkedTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllLinked", true);

		// Token: 0x04009C47 RID: 40007
		public static readonly Material GroupedMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay, new Color(1f, 1f, 0f, 0.2f));

		// Token: 0x04009C48 RID: 40008
		private static List<IStorageGroupMember> tmpMembers = new List<IStorageGroupMember>();
	}
}
