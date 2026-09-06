using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace GorillaLocomotion.Supplements
{
	// Token: 0x02000045 RID: 69
	public class Plugin : MonoBehaviour
	{
		// Token: 0x060001FB RID: 507 RVA: 0x000212C4 File Offset: 0x0001F4C4
		internal static Key KeyCodeToInputSystemKey(KeyCode kc)
		{
			bool flag = kc <= 13;
			if (flag)
			{
				bool flag2 = kc == 8;
				if (flag2)
				{
					return 65;
				}
				bool flag3 = kc == 9;
				if (flag3)
				{
					return 3;
				}
				bool flag4 = kc == 13;
				if (flag4)
				{
					return 2;
				}
			}
			else
			{
				bool flag5 = kc <= 122;
				if (flag5)
				{
					bool flag6 = kc == 19;
					if (flag6)
					{
						return 76;
					}
					switch (kc)
					{
					case 27:
						return 60;
					case 32:
						return 1;
					case 39:
						return 5;
					case 44:
						return 7;
					case 45:
						return 13;
					case 46:
						return 8;
					case 47:
						return 9;
					case 48:
						return 50;
					case 49:
						return 41;
					case 50:
						return 42;
					case 51:
						return 43;
					case 52:
						return 44;
					case 53:
						return 45;
					case 54:
						return 46;
					case 55:
						return 47;
					case 56:
						return 48;
					case 57:
						return 49;
					case 59:
						return 6;
					case 61:
						return 14;
					case 91:
						return 11;
					case 92:
						return 10;
					case 93:
						return 12;
					case 96:
						return 4;
					case 97:
						return 15;
					case 98:
						return 16;
					case 99:
						return 17;
					case 100:
						return 18;
					case 101:
						return 19;
					case 102:
						return 20;
					case 103:
						return 21;
					case 104:
						return 22;
					case 105:
						return 23;
					case 106:
						return 24;
					case 107:
						return 25;
					case 108:
						return 26;
					case 109:
						return 27;
					case 110:
						return 28;
					case 111:
						return 29;
					case 112:
						return 30;
					case 113:
						return 31;
					case 114:
						return 32;
					case 115:
						return 33;
					case 116:
						return 34;
					case 117:
						return 35;
					case 118:
						return 36;
					case 119:
						return 37;
					case 120:
						return 38;
					case 121:
						return 39;
					case 122:
						return 40;
					}
				}
				else
				{
					bool flag7 = kc == 127;
					if (flag7)
					{
						return 71;
					}
					switch (kc)
					{
					case 256:
						return 84;
					case 257:
						return 85;
					case 258:
						return 86;
					case 259:
						return 87;
					case 260:
						return 88;
					case 261:
						return 89;
					case 262:
						return 90;
					case 263:
						return 91;
					case 264:
						return 92;
					case 265:
						return 93;
					case 266:
						return 82;
					case 267:
						return 78;
					case 268:
						return 79;
					case 269:
						return 81;
					case 270:
						return 80;
					case 271:
						return 77;
					case 273:
						return 63;
					case 274:
						return 64;
					case 275:
						return 62;
					case 276:
						return 61;
					case 277:
						return 70;
					case 278:
						return 68;
					case 279:
						return 69;
					case 280:
						return 67;
					case 281:
						return 66;
					case 282:
						return 94;
					case 283:
						return 95;
					case 284:
						return 96;
					case 285:
						return 97;
					case 286:
						return 98;
					case 287:
						return 99;
					case 288:
						return 100;
					case 289:
						return 101;
					case 290:
						return 102;
					case 291:
						return 103;
					case 292:
						return 104;
					case 293:
						return 105;
					case 300:
						return 73;
					case 301:
						return 72;
					case 302:
						return 75;
					case 303:
						return 52;
					case 304:
						return 51;
					case 305:
						return 56;
					case 306:
						return 55;
					case 307:
						return 54;
					case 308:
						return 53;
					case 309:
						return 58;
					case 310:
						return 57;
					case 311:
						return 57;
					case 312:
						return 58;
					case 316:
						return 74;
					}
				}
			}
			return 0;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x000218D4 File Offset: 0x0001FAD4
		private void Awake()
		{
			Plugin.DetectVRPlatform();
			try
			{
				Menu.Start();
			}
			catch
			{
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00021908 File Offset: 0x0001FB08
		private static void DetectVRPlatform()
		{
			try
			{
				string loadedDeviceName = XRSettings.loadedDeviceName;
				VrInput.IsOculus = loadedDeviceName.ToLower().Contains("oculus") || loadedDeviceName.ToLower().Contains("meta");
			}
			catch
			{
				VrInput.IsOculus = false;
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00021968 File Offset: 0x0001FB68
		private void Update()
		{
			ArmCap.Apply();
			Menu.Update();
			bool flag = !Plugin.hasAutoLoaded;
			if (flag)
			{
				Plugin.hasAutoLoaded = true;
				ConfigManager.AutoLoad();
			}
			bool flag2 = !Plugin.hasSyncedMacros;
			if (flag2)
			{
				Plugin.hasSyncedMacros = true;
				Macros.SyncMacros();
			}
			Desync.Update();
			bool wallWalkEnabled = Settings.WallWalkEnabled;
			if (wallWalkEnabled)
			{
				WallWalk.Apply();
			}
			bool cgtwallWalkEnabled = Settings.CGTWallWalkEnabled;
			if (cgtwallWalkEnabled)
			{
				CgtWallWalk.Apply();
			}
			TagAura.Update();
			bool tagAuraEnabled = Settings.TagAuraEnabled;
			if (tagAuraEnabled)
			{
				TagAura.ApplyTagAura();
			}
			TagAura.ApplyHitboxExpander();
			bool psaenabled = Settings.PSAEnabled;
			if (psaenabled)
			{
				PullSystem.ApplyPSA();
			}
			bool veloPSAEnabled = Settings.VeloPSAEnabled;
			if (veloPSAEnabled)
			{
				PullSystem.ApplyVeloPSA();
			}
			bool burstPSAEnabled = Settings.BurstPSAEnabled;
			if (burstPSAEnabled)
			{
				PullSystem.ApplyBurstPSA();
			}
			bool psaskiddedEnabled = Settings.PSASkiddedEnabled;
			if (psaskiddedEnabled)
			{
				PullSystem.ApplyPSASkidded();
			}
			bool highJumpEnabled = Settings.HighJumpEnabled;
			if (highJumpEnabled)
			{
				PullSystem.ApplyHighJump();
			}
			bool cr1ptsPSAEnabled = Settings.Cr1ptsPSAEnabled;
			if (cr1ptsPSAEnabled)
			{
				PullSystem.ApplyCr1ptsPSA();
			}
			bool isVelmaxActive = TagState.IsVelmaxActive;
			if (isVelmaxActive)
			{
				TagState.Update();
			}
			bool predsEnabled = Settings.PredsEnabled;
			if (predsEnabled)
			{
				Predictions.Update();
			}
			RealPull.Update();
			bool espenabled = Settings.ESPEnabled;
			if (espenabled)
			{
				Esp.Update();
			}
			bool trailsEnabled = Settings.TrailsEnabled;
			if (trailsEnabled)
			{
				Trails.Update();
			}
			else
			{
				Trails.Clear();
			}
			Skybox.Update();
			RemoveLeaves.Update();
			VibrationAlerts.Update();
			Weather.Update();
			bool flag3 = Settings.WASDFlyEnabled && Settings.IsPressed(Settings.WASDFlyKeybind);
			if (flag3)
			{
				WasdFly.Apply();
			}
			else
			{
				WasdFly.Stop();
			}
			SessionStats.Update();
			NameTags.Update();
			Macros.Update();
			bool tickRateEnabled = Settings.TickRateEnabled;
			if (tickRateEnabled)
			{
				TickRate.Apply();
			}
			bool forceTagFreezeEnabled = Settings.ForceTagFreezeEnabled;
			if (forceTagFreezeEnabled)
			{
				bool flag4 = Settings.IsPressed(Settings.ForceTagFreezeKeybind) && TagFreeze.IsHandOnSlab();
				if (flag4)
				{
					TagState.Freeze();
				}
				else
				{
					TagState.Unfreeze();
				}
			}
			TagFreeze.Update();
			FingerMovement.FakePowerOff();
			FingerMovement.ApplyLongArms();
			bool noFingerMovementEnabled = Settings.NoFingerMovementEnabled;
			if (noFingerMovementEnabled)
			{
				FingerMovement.FreezeInputs();
			}
			TagFreeze.ApplyLongArms();
			TagFreeze.ApplyLongArmsBypass();
			TagFreeze.ApplyGrayScreen();
			HandOffset.Apply();
			NoSlip.Apply();
			SurfaceSlip.Update();
			Desync.Update();
			SurfaceProbe.Update();
			TagAura.AutoDcFlick();
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00021BC8 File Offset: 0x0001FDC8
		private void LateUpdate()
		{
			ArmCap.Apply();
			bool testPullModEnabled = Settings.TestPullModEnabled;
			if (testPullModEnabled)
			{
				TestPull.Update();
			}
			MainPull.Update();
			OneFramePull.Update();
			WallPull.Update();
			PullV3.Update();
			RemoveWindBarrier.Update();
			TagFreeze.LateUpdate();
			FingerMovement.ApplyLongArmsBypass();
			Predictions.LateUpdate();
			RealPull.LateUpdate();
			Desync.LateUpdate();
			Macros.LateUpdate();
		}

		// Token: 0x040004AA RID: 1194
		private static bool hasAutoLoaded;

		// Token: 0x040004AB RID: 1195
		private static bool hasSyncedMacros;
	}
}
