using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E45 RID: 3653
	public class Sustainer
	{
		// Token: 0x17000ECA RID: 3786
		// (get) Token: 0x06005C09 RID: 23561 RVA: 0x001D6C2A File Offset: 0x001D4E2A
		public bool Ended
		{
			get
			{
				return this.endRealTime >= 0f;
			}
		}

		// Token: 0x17000ECB RID: 3787
		// (get) Token: 0x06005C0A RID: 23562 RVA: 0x001D6C3C File Offset: 0x001D4E3C
		public float TimeSinceEnd
		{
			get
			{
				return Time.realtimeSinceStartup - this.endRealTime;
			}
		}

		// Token: 0x17000ECC RID: 3788
		// (get) Token: 0x06005C0B RID: 23563 RVA: 0x001D6C4C File Offset: 0x001D4E4C
		public float CameraDistanceSquared
		{
			get
			{
				if (this.info.IsOnCamera)
				{
					return 0f;
				}
				if (this.worldRootObject == null)
				{
					if (Prefs.DevMode)
					{
						Log.Error(string.Format("Sustainer {0} info is {1} but its worldRootObject is null", this.def, this.info));
					}
					return 0f;
				}
				return (float)(Find.CameraDriver.MapPosition - this.worldRootObject.transform.position.ToIntVec3()).LengthHorizontalSquared;
			}
		}

		// Token: 0x06005C0C RID: 23564 RVA: 0x001D6CD4 File Offset: 0x001D4ED4
		public Sustainer(SoundDef def, SoundInfo info)
		{
			this.def = def;
			this.info = info;
			if (def.subSounds.Count > 0)
			{
				foreach (KeyValuePair<string, float> keyValuePair in info.DefinedParameters)
				{
					this.externalParams[keyValuePair.Key] = keyValuePair.Value;
				}
				if (def.HasSubSoundsInWorld)
				{
					if (!info.forcedPlayOnCamera && info.IsOnCamera)
					{
						Log.Error(string.Format("Playing sound {0} on camera, but it has sub-sounds in the world.", def));
					}
					this.worldRootObject = new GameObject("SustainerRootObject_" + def.defName);
					this.UpdateRootObjectPosition();
				}
				else if (!info.IsOnCamera)
				{
					info = SoundInfo.OnCamera(info.Maintenance);
				}
				Find.SoundRoot.sustainerManager.RegisterSustainer(this);
				if (!info.IsOnCamera)
				{
					Find.SoundRoot.sustainerManager.UpdateAllSustainerScopes();
				}
				for (int i = 0; i < def.subSounds.Count; i++)
				{
					this.subSustainers.Add(new SubSustainer(this, def.subSounds[i]));
				}
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.lastMaintainTick = Find.TickManager.TicksGame;
					this.lastMaintainFrame = Time.frameCount;
				});
				return;
			}
			this.lastMaintainTick = Find.TickManager.TicksGame;
			this.lastMaintainFrame = Time.frameCount;
		}

		// Token: 0x06005C0D RID: 23565 RVA: 0x001D6E78 File Offset: 0x001D5078
		public void SustainerUpdate()
		{
			if (!this.Ended)
			{
				if (this.info.Maintenance == MaintenanceType.PerTick)
				{
					if (Find.TickManager.TicksGame > this.lastMaintainTick + 1)
					{
						this.End();
						return;
					}
				}
				else if (this.info.Maintenance == MaintenanceType.PerTickRare)
				{
					if (Find.TickManager.TicksGame > this.lastMaintainTick + 250)
					{
						this.End();
						return;
					}
				}
				else if (this.info.Maintenance == MaintenanceType.PerFrame && Time.frameCount > this.lastMaintainFrame + 1)
				{
					this.End();
					return;
				}
			}
			else if (this.TimeSinceEnd > this.def.sustainFadeoutTime)
			{
				this.Cleanup();
			}
			if (this.def.subSounds.Count > 0)
			{
				if (!this.info.IsOnCamera && this.info.Maker.HasThing)
				{
					this.UpdateRootObjectPosition();
				}
				this.scopeFader.SustainerScopeUpdate();
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].SubSustainerUpdate();
				}
			}
		}

		// Token: 0x06005C0E RID: 23566 RVA: 0x001D6F90 File Offset: 0x001D5190
		private void UpdateRootObjectPosition()
		{
			Vector3 position = Vector3.zero;
			if (this.info.forcedPlayOnCamera)
			{
				position = Find.Camera.gameObject.transform.position;
			}
			else
			{
				position = this.info.Maker.Cell.ToVector3ShiftedWithAltitude(0f);
				Thing thing = this.info.Maker.Thing;
				if (thing != null)
				{
					position = thing.DrawPos.Yto0();
				}
			}
			if (this.worldRootObject != null)
			{
				this.worldRootObject.transform.position = position;
			}
		}

		// Token: 0x06005C0F RID: 23567 RVA: 0x001D702C File Offset: 0x001D522C
		public void Maintain()
		{
			if (this.Ended)
			{
				Log.Error(string.Format("Tried to maintain ended sustainer: {0}", this.def));
				return;
			}
			if (this.info.Maintenance == MaintenanceType.PerTick || this.info.Maintenance == MaintenanceType.PerTickRare)
			{
				this.lastMaintainTick = Find.TickManager.TicksGame;
				return;
			}
			if (this.info.Maintenance == MaintenanceType.PerFrame)
			{
				this.lastMaintainFrame = Time.frameCount;
			}
		}

		// Token: 0x06005C10 RID: 23568 RVA: 0x001D70A0 File Offset: 0x001D52A0
		public void End()
		{
			this.endRealTime = Time.realtimeSinceStartup;
			if (this.def.sustainFadeoutStartSound != null && this.def.sustainFadeoutTime >= 0.001f)
			{
				this.PlaySoundOnMapOrCamera(this.def.sustainFadeoutStartSound);
			}
			if (this.def.sustainFadeoutTime < 0.001f)
			{
				this.Cleanup();
			}
		}

		// Token: 0x06005C11 RID: 23569 RVA: 0x001D7100 File Offset: 0x001D5300
		private void Cleanup()
		{
			if (this.def.subSounds.Count > 0)
			{
				Find.SoundRoot.sustainerManager.DeregisterSustainer(this);
				for (int i = 0; i < this.subSustainers.Count; i++)
				{
					this.subSustainers[i].Cleanup();
				}
			}
			if (this.def.sustainStopSound != null)
			{
				this.PlaySoundOnMapOrCamera(this.def.sustainStopSound);
			}
			if (this.worldRootObject != null)
			{
				UnityEngine.Object.Destroy(this.worldRootObject);
			}
			DebugSoundEventsLog.Notify_SustainerEnded(this, this.info);
		}

		// Token: 0x06005C12 RID: 23570 RVA: 0x001D719C File Offset: 0x001D539C
		private void PlaySoundOnMapOrCamera(SoundDef soundDef)
		{
			if (this.worldRootObject != null)
			{
				Map map = this.info.Maker.Map;
				if (map != null)
				{
					SoundInfo soundInfo = SoundInfo.InMap(new TargetInfo(this.worldRootObject.transform.position.ToIntVec3(), map, false), MaintenanceType.None);
					soundDef.PlayOneShot(soundInfo);
					return;
				}
			}
			else
			{
				soundDef.PlayOneShot(SoundInfo.OnCamera(MaintenanceType.None));
			}
		}

		// Token: 0x06005C13 RID: 23571 RVA: 0x001D7208 File Offset: 0x001D5408
		public string DebugString()
		{
			string text = this.def.defName;
			text += string.Format("\n  inScopePercent={0}", this.scopeFader.inScopePercent);
			text += string.Format("\n  CameraDistanceSquared={0}", this.CameraDistanceSquared);
			foreach (SubSustainer arg in this.subSustainers)
			{
				text += string.Format("\n  sub: {0}", arg);
			}
			return text;
		}

		// Token: 0x040042B3 RID: 17075
		public SoundDef def;

		// Token: 0x040042B4 RID: 17076
		public SoundInfo info;

		// Token: 0x040042B5 RID: 17077
		internal GameObject worldRootObject;

		// Token: 0x040042B6 RID: 17078
		private int lastMaintainTick;

		// Token: 0x040042B7 RID: 17079
		private int lastMaintainFrame;

		// Token: 0x040042B8 RID: 17080
		private float endRealTime = -1f;

		// Token: 0x040042B9 RID: 17081
		private List<SubSustainer> subSustainers = new List<SubSustainer>();

		// Token: 0x040042BA RID: 17082
		public SoundParams externalParams = new SoundParams();

		// Token: 0x040042BB RID: 17083
		public SustainerScopeFader scopeFader = new SustainerScopeFader();
	}
}
