using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000866 RID: 2150
	public static class Scribe_TargetInfo
	{
		// Token: 0x0600363D RID: 13885 RVA: 0x00122BBF File Offset: 0x00120DBF
		public static void Look(ref LocalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, LocalTargetInfo.Invalid, false);
		}

		// Token: 0x0600363E RID: 13886 RVA: 0x00122BCF File Offset: 0x00120DCF
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, LocalTargetInfo.Invalid, true);
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x00122BDF File Offset: 0x00120DDF
		public static void Look(ref LocalTargetInfo value, string label, LocalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x00122BEC File Offset: 0x00120DEC
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label, LocalTargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.LocalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveLocalTargetInfo(value, label);
				}
			}
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x00122C80 File Offset: 0x00120E80
		public static void Look(ref TargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, TargetInfo.Invalid, false);
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x00122C90 File Offset: 0x00120E90
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, TargetInfo.Invalid, true);
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x00122CA0 File Offset: 0x00120EA0
		public static void Look(ref TargetInfo value, string label, TargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x00122CAC File Offset: 0x00120EAC
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label, TargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.TargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveTargetInfo(value, label);
				}
			}
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x00122D89 File Offset: 0x00120F89
		public static void Look(ref GlobalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, GlobalTargetInfo.Invalid, false);
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x00122D99 File Offset: 0x00120F99
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, GlobalTargetInfo.Invalid, true);
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x00122DA9 File Offset: 0x00120FA9
		public static void Look(ref GlobalTargetInfo value, string label, GlobalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue, false);
		}

		// Token: 0x06003648 RID: 13896 RVA: 0x00122DB8 File Offset: 0x00120FB8
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label, GlobalTargetInfo defaultValue, bool preserveDefaultValues = false)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue) || preserveDefaultValues)
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (value.WorldObject != null && Scribe_References.CheckSaveReferenceToDestroyedWorldObject(value.WorldObject, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && !value.HasWorldObject && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.GlobalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveGlobalTargetInfo(value, label);
				}
			}
		}
	}
}
