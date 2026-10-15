using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public static class StorageGroupUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public IStorageGroupMember member;

		internal void _003CStorageGroupMemberGizmos_003Eb__0()
		{
			bool num = tmpMembers[0].Group != null;
			StorageGroup storageGroup = tmpMembers[0].Group ?? member.Map.storageGroups.NewGroup();
			if (!num)
			{
				storageGroup.InitFrom(tmpMembers[0]);
			}
			foreach (IStorageGroupMember tmpMember in tmpMembers)
			{
				tmpMember.SetStorageGroup(storageGroup);
			}
			if (tmpMembers.Count > 1)
			{
				Messages.Message("SettingsLinkedFor".Translate(tmpMembers.Count), null, MessageTypeDefOf.NeutralEvent, historical: false);
			}
			else
			{
				Messages.Message("SettingsLinkedForSingular".Translate(), null, MessageTypeDefOf.NeutralEvent, historical: false);
			}
		}

		internal void _003CStorageGroupMemberGizmos_003Eb__2()
		{
			if (member.Group != null)
			{
				StorageSettings storeSettings = member.Group.GetStoreSettings();
				member.Group.RemoveMember(member);
				member.Group = null;
				if (member is Building_Storage building_Storage)
				{
					building_Storage.settings.CopyFrom(storeSettings);
				}
			}
			if (tmpMembers.Count > 1)
			{
				Messages.Message("SettingsUnlinkedFor".Translate(tmpMembers.Count), null, MessageTypeDefOf.NeutralEvent, historical: false);
			}
			else
			{
				Messages.Message("SettingsUnlinkedForSingular".Translate(), null, MessageTypeDefOf.NeutralEvent, historical: false);
			}
		}

		internal void _003CStorageGroupMemberGizmos_003Eb__3()
		{
			bool flag = false;
			foreach (IStorageGroupMember member in member.Group.members)
			{
				if (!Find.Selector.IsSelected(member) && !(member is Thing { Destroyed: not false }))
				{
					Find.Selector.Select(member, playSound: false);
					flag = true;
				}
			}
			if (flag)
			{
				SoundDefOf.ThingSelected.PlayOneShotOnCamera();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CStorageGroupMemberGizmos_003Ed__5 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IStorageGroupMember member;

		public IStorageGroupMember _003C_003E3__member;

		private _003C_003Ec__DisplayClass5_0 _003C_003E8__1;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CStorageGroupMemberGizmos_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass5_0();
				_003C_003E8__1.member = member;
				bool flag2 = true;
				StorageGroup storageGroup = null;
				tmpMembers.Clear();
				foreach (object selectedObject in Find.Selector.SelectedObjects)
				{
					if (!(selectedObject is IStorageGroupMember storageGroupMember) || !(storageGroupMember.StorageGroupTag == _003C_003E8__1.member.StorageGroupTag) || storageGroupMember is Building_Storage { StorageTabVisible: false } || storageGroupMember.Map != _003C_003E8__1.member.Map)
					{
						continue;
					}
					tmpMembers.Add(storageGroupMember);
					if (flag2)
					{
						if (storageGroup == null && _003C_003E8__1.member.Group != null)
						{
							storageGroup = _003C_003E8__1.member.Group;
						}
						if (storageGroup != _003C_003E8__1.member.Group)
						{
							flag2 = false;
						}
					}
				}
				if (storageGroup == null)
				{
					flag2 = false;
				}
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "LinkStorageSettings".Translate(),
					defaultDesc = "LinkStorageSettingsDesc".Translate(),
					icon = LinkTex,
					action = delegate
					{
						bool num = tmpMembers[0].Group != null;
						StorageGroup storageGroup2 = tmpMembers[0].Group ?? _003C_003E8__1.member.Map.storageGroups.NewGroup();
						if (!num)
						{
							storageGroup2.InitFrom(tmpMembers[0]);
						}
						foreach (IStorageGroupMember tmpMember in tmpMembers)
						{
							tmpMember.SetStorageGroup(storageGroup2);
						}
						if (tmpMembers.Count > 1)
						{
							Messages.Message("SettingsLinkedFor".Translate(tmpMembers.Count), null, MessageTypeDefOf.NeutralEvent, historical: false);
						}
						else
						{
							Messages.Message("SettingsLinkedForSingular".Translate(), null, MessageTypeDefOf.NeutralEvent, historical: false);
						}
					},
					onHover = delegate
					{
						if (tmpMembers.Any() && tmpMembers[0] is Thing thing2)
						{
							TargetHighlighter.Highlight(thing2);
						}
					}
				};
				if (tmpMembers.Count < 2)
				{
					command_Action.Disable("LinkStorageDisabledSelectTwo".Translate());
				}
				else if (flag2)
				{
					command_Action.Disable("AlreadyLinked".Translate());
				}
				_003C_003E2__current = command_Action;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (_003C_003E8__1.member.Group == null)
				{
					break;
				}
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "UnlinkStorageSettings".Translate(),
					defaultDesc = "UnlinkStorageSettingsDesc".Translate(),
					icon = UnlinkTex,
					action = delegate
					{
						if (_003C_003E8__1.member.Group != null)
						{
							StorageSettings storeSettings = _003C_003E8__1.member.Group.GetStoreSettings();
							_003C_003E8__1.member.Group.RemoveMember(_003C_003E8__1.member);
							_003C_003E8__1.member.Group = null;
							if (_003C_003E8__1.member is Building_Storage building_Storage)
							{
								building_Storage.settings.CopyFrom(storeSettings);
							}
						}
						if (tmpMembers.Count > 1)
						{
							Messages.Message("SettingsUnlinkedFor".Translate(tmpMembers.Count), null, MessageTypeDefOf.NeutralEvent, historical: false);
						}
						else
						{
							Messages.Message("SettingsUnlinkedForSingular".Translate(), null, MessageTypeDefOf.NeutralEvent, historical: false);
						}
					}
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "SelectAllLinked".Translate(),
					defaultDesc = "SelectAllLinkedDesc".Translate(),
					icon = SelectLinkedTex,
					action = delegate
					{
						bool flag = false;
						foreach (IStorageGroupMember member in _003C_003E8__1.member.Group.members)
						{
							if (!Find.Selector.IsSelected(member) && !(member is Thing { Destroyed: not false }))
							{
								Find.Selector.Select(member, playSound: false);
								flag = true;
							}
						}
						if (flag)
						{
							SoundDefOf.ThingSelected.PlayOneShotOnCamera();
						}
					}
				};
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CStorageGroupMemberGizmos_003Ed__5 _003CStorageGroupMemberGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStorageGroupMemberGizmos_003Ed__ = this;
			}
			else
			{
				_003CStorageGroupMemberGizmos_003Ed__ = new _003CStorageGroupMemberGizmos_003Ed__5(0);
			}
			_003CStorageGroupMemberGizmos_003Ed__.member = _003C_003E3__member;
			return _003CStorageGroupMemberGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private static readonly Texture2D LinkTex = ContentFinder<Texture2D>.Get("UI/Commands/LinkStorageSettings");

	private static readonly Texture2D UnlinkTex = ContentFinder<Texture2D>.Get("UI/Commands/UnlinkStorageSettings");

	private static readonly Texture2D SelectLinkedTex = ContentFinder<Texture2D>.Get("UI/Commands/SelectAllLinked");

	public static readonly Material GroupedMat = MaterialPool.MatFrom("UI/Overlays/SelectionBracket", ShaderDatabase.MetaOverlay, new Color(1f, 1f, 0f, 0.2f));

	private static List<IStorageGroupMember> tmpMembers = new List<IStorageGroupMember>();

	[IteratorStateMachine(typeof(_003CStorageGroupMemberGizmos_003Ed__5))]
	public static IEnumerable<Gizmo> StorageGroupMemberGizmos(IStorageGroupMember member)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStorageGroupMemberGizmos_003Ed__5(-2)
		{
			_003C_003E3__member = member
		};
	}

	public static void DrawSelectionOverlaysFor(IStorageGroupMember member)
	{
		if (member.Group == null || SelectionDrawer.DrawnStorageGroupThisFrame(member.Group))
		{
			return;
		}
		foreach (IStorageGroupMember member2 in member.Group.members)
		{
			if (member2.DrawConnectionOverlay)
			{
				SelectionDrawer.DrawSelectionBracketFor(member2, GroupedMat);
			}
		}
		SelectionDrawer.Notify_DrawnStorageGroup(member.Group);
	}

	public static void SetStorageGroup(this IStorageGroupMember member, StorageGroup newGroup, bool removeIfEmpty = true)
	{
		if (member.Group == null || member.Group != newGroup)
		{
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
			if (member is IStoreSettingsParent storeSettingsParent)
			{
				storeSettingsParent.Notify_SettingsChanged();
			}
		}
	}

	public static bool IsHopper(this Thing thing)
	{
		if (thing?.def?.building == null)
		{
			return false;
		}
		return thing.def.building.isHopper;
	}
}
