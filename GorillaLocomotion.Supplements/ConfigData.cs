using System;

// Token: 0x0200000D RID: 13
[Serializable]
public class ConfigData
{
	// Token: 0x0600001F RID: 31 RVA: 0x00002D24 File Offset: 0x00000F24
	public ConfigData()
	{
		this.PanicKeybind = 6;
		this.LongArmsAmount = 1.2f;
		this.LongArmsBypassKeybind = 6;
		this.LongArmsBypassLeftHand = true;
		this.LongArmsBypassRightHand = true;
		this.LongArmsBypassSmoothness = 12f;
		this.FakeQuestMenuKeybind = 8;
		this.FakeReportMenuKeybind = 6;
		this.FakePowerOffKeybind = 10;
		this.GrayScreenKeybind = 7;
		this.GrayScreenDuration = 0.5f;
		this.GrayScreenArmDistance = 3f;
		this.GrayScreenArmSpread = 1.2f;
		this.TargetHz = 72;
		this.SlipSlapMultiplier = 1f;
		this.SurfaceSlipLowerSlippery = 0.55f;
		this.SurfaceSlipUpperSlippery = 0.85f;
		this.ArmCapValue = 1.5f;
		this.PredsAlwaysAmount = 30f;
		this.PredsLeftHandEnabled = true;
		this.PredsRightHandEnabled = true;
		this.DesyncDelayMs = 100f;
		this.DesyncFakeLagFreezeMs = 500f;
		this.DesyncFakeLagIntervalMs = 1000f;
		this.ESPColorSat = 1f;
		this.ESPColorVal = 1f;
		this.RecolorTaggedColorSat = 1f;
		this.RecolorTaggedColorVal = 1f;
		this.FillESPOpacity = 0.3f;
		this.FillESPColorSat = 1f;
		this.FillESPColorVal = 1f;
		this.TrailMinSpeed = 1f;
		this.TrailSpeedScale = 8f;
		this.TrailMinTime = 0.15f;
		this.TrailMaxTime = 1.2f;
		this.TrailWidth = 0.25f;
		this.TrailUsePlayerColor = true;
		this.TrailColorSat = 1f;
		this.TrailColorVal = 1f;
		this.BeaconWidth = 0.08f;
		this.VibrationAlertDistance = 5f;
		this.VibrationAlertStrength = 0.8f;
		this.PlayerGlowIntensity = 1f;
		this.BreadcrumbSpacing = 1f;
		this.BreadcrumbSize = 0.08f;
		this.BreadcrumbColorHue = 0.33f;
		this.BreadcrumbColorSat = 1f;
		this.BreadcrumbColorVal = 1f;
		this.KillFeedDuration = 5f;
		this.TickRate = 1f;
		this.SoundESPDistance = 10f;
		this.SoundESPVolume = 0.7f;
		this.SoundESPCooldown = 1f;
		this.SoundESPPreset = 1;
		this.SoundESPCustomPath = "";
		this.SlideControlAmount = 0.5f;
		this.TagNotificationDistance = 3f;
		this.NameTagFontSize = 2f;
		this.NameTagOffset = 0.5f;
		this.NameTagRenderDistance = 50f;
		this.PlatformTagFontSize = 2f;
		this.PlatformTagOffset = 0.7f;
		this.FPSTagFontSize = 2f;
		this.FPSTagOffset = 0.9f;
		this.AutoBranchRecordKeybind = 8;
		this.AutoBranchStopKeybind = 6;
		this.AutoBranchReplayKeybind = 7;
		this.AutoBranchMovePlayer = true;
		this.AutoBranchMatchRadius = 6f;
		this.AutoBranchBlendTime = 0.3f;
		this.SkyboxColorHue = 0.58f;
		this.SkyboxColorSat = 0.5f;
		this.SkyboxColorVal = 0.9f;
		this.SkyboxColor2Hue = 0.75f;
		this.SkyboxColor2Sat = 0.5f;
		this.SkyboxColor2Val = 0.6f;
		this.SkyboxImagePath = "";
		this.SkyboxGradientSpeed = 0.15f;
		this.SkyboxStarCount = 150;
		this.SkyboxStarSize = 1f;
		this.JewishMusicEnabled = true;
		this.OneFramePullStrength = 1f;
		this.OneFramePullThreshold = 0.5f;
		this.OneFramePullActivation = 13;
		this.RealPullDistance = 0.9f;
		this.RealPullThreshold = 0.5f;
		this.RealPullActivation = 13;
		this.RealPullDownAmount = 0.2f;
		this.DownControllerKeybind = 4;
		this.DownControllerDistance = 10f;
		this.DownControllerSpeed = 10f;
		this.MainPullStrength = 0.7f;
		this.MainPullTpTime = 0.011f;
		this.MainPullSmoothing = 0.3f;
		this.MainPullActivation = 13;
		this.MainPullVelThreshold = 0.5f;
		this.MainPullThresholdTime = 50f;
		this.MainPullPredictPoints = 4;
		this.MainPullPredictCheckStart = 0.5f;
		this.MainPullPredictCheckEnd = 1f;
		this.MainPullPredictStopBack = 1;
		this.MainPullSurfaceAlignRadius = 0.6f;
		this.MainPullWallPullKeybind = 3;
		this.MainPullWallPullStrength = 0.7f;
		this.MainPullWallPullTpTime = 0.011f;
		this.MainPullWallPullSmoothing = 0.3f;
		this.WallPullModStrength = 0.7f;
		this.WallPullModTpTime = 0.011f;
		this.WallPullModSmoothing = 0.3f;
		this.WallPullModVelThreshold = 0.5f;
		this.WallPullModActivation = 3;
		this.PullV3Strength = 1f;
		this.PullV3Threshold = 0.5f;
		this.PullV3TpTime = 0.013f;
		this.PullV3Activation = 13;
		this.PullV3LeftHand = true;
		this.PullV3RightHand = true;
		this.PullV3MidPullRequired = true;
		this.PullV3ResetTime = 1.5f;
		this.PullV3MaxStacks = 4;
		this.PullV3FreezeStrength = 6.5f;
	}

	// Token: 0x0400003D RID: 61
	public bool PullModEnabled = true;

	// Token: 0x0400003E RID: 62
	public float PullStrength = 1f;

	// Token: 0x0400003F RID: 63
	public float PullThreshold;

	// Token: 0x04000040 RID: 64
	public float PullTpTime = 0.013f;

	// Token: 0x04000041 RID: 65
	public int PullActivation = 13;

	// Token: 0x04000042 RID: 66
	public bool TestPullModEnabled;

	// Token: 0x04000043 RID: 67
	public float TestPullStrength = 0.7f;

	// Token: 0x04000044 RID: 68
	public float TestPullThreshold = 0.7f;

	// Token: 0x04000045 RID: 69
	public float TestPullTpTime = 0.011f;

	// Token: 0x04000046 RID: 70
	public int TestPullActivation = 13;

	// Token: 0x04000047 RID: 71
	public bool TestPullModToggleMode;

	// Token: 0x04000048 RID: 72
	public float TestPullSmoothing = 0.5f;

	// Token: 0x04000049 RID: 73
	public bool TestPullRaycastEnabled;

	// Token: 0x0400004A RID: 74
	public bool WallWalkEnabled;

	// Token: 0x0400004B RID: 75
	public float WallWalkDistance = 0.5f;

	// Token: 0x0400004C RID: 76
	public float WallWalkPower = 2f;

	// Token: 0x0400004D RID: 77
	public bool WallWalkLeftHand = true;

	// Token: 0x0400004E RID: 78
	public bool WallWalkRightHand = true;

	// Token: 0x0400004F RID: 79
	public int WallWalkKeybind = 4;

	// Token: 0x04000050 RID: 80
	public bool WallWalkPerHandBinds;

	// Token: 0x04000051 RID: 81
	public int WallWalkLeftKeybind = 3;

	// Token: 0x04000052 RID: 82
	public int WallWalkRightKeybind = 4;

	// Token: 0x04000053 RID: 83
	public bool CGTWallWalkEnabled;

	// Token: 0x04000054 RID: 84
	public float CGTWallWalkDistance = 0.5f;

	// Token: 0x04000055 RID: 85
	public float CGTWallWalkPower = 2f;

	// Token: 0x04000056 RID: 86
	public float CGTWallWalkReach = 1f;

	// Token: 0x04000057 RID: 87
	public int CGTWallWalkKeybind = 4;

	// Token: 0x04000058 RID: 88
	public bool DCFlickEnabled;

	// Token: 0x04000059 RID: 89
	public bool DcFlickAutoEnabled;

	// Token: 0x0400005A RID: 90
	public float DcFlickAutoHoldTime = 0.3f;

	// Token: 0x0400005B RID: 91
	public bool DcFlickAutoLOS;

	// Token: 0x0400005C RID: 92
	public float DcFlickAutoMaxDistance = 10f;

	// Token: 0x0400005D RID: 93
	public float DcFlickAutoFov = 90f;

	// Token: 0x0400005E RID: 94
	public int DcFlickAutoKeybind = 1;

	// Token: 0x0400005F RID: 95
	public bool TagAuraEnabled;

	// Token: 0x04000060 RID: 96
	public float TagAuraDistance = 1f;

	// Token: 0x04000061 RID: 97
	public int TagAuraKeybind = 9;

	// Token: 0x04000062 RID: 98
	public bool TagAuraFovEnabled;

	// Token: 0x04000063 RID: 99
	public float TagAuraFov = 90f;

	// Token: 0x04000064 RID: 100
	public bool HitboxExpanderEnabled;

	// Token: 0x04000065 RID: 101
	public float HitboxExpanderSize = 0.5f;

	// Token: 0x04000066 RID: 102
	public float HitboxExpanderOpacity = 0.5f;

	// Token: 0x04000067 RID: 103
	public bool PSAEnabled;

	// Token: 0x04000068 RID: 104
	public float PSASpeed = 1f;

	// Token: 0x04000069 RID: 105
	public int PSAKeybind = 1;

	// Token: 0x0400006A RID: 106
	public bool VeloPSAEnabled;

	// Token: 0x0400006B RID: 107
	public float VeloPSASpeed = 1f;

	// Token: 0x0400006C RID: 108
	public int VeloPSAKeybind = 3;

	// Token: 0x0400006D RID: 109
	public float VeloPSAMinSpeed = 0.12f;

	// Token: 0x0400006E RID: 110
	public bool VeloPSAStickyDirection;

	// Token: 0x0400006F RID: 111
	public bool VeloPSAAirTurnAssist;

	// Token: 0x04000070 RID: 112
	public float VeloPSAAirTurnBlend = 0.6f;

	// Token: 0x04000071 RID: 113
	public bool BurstPSAEnabled;

	// Token: 0x04000072 RID: 114
	public float BurstPSASpeed = 1f;

	// Token: 0x04000073 RID: 115
	public int BurstPSAKeybind = 3;

	// Token: 0x04000074 RID: 116
	public float BurstPSAMinSpeed = 0.12f;

	// Token: 0x04000075 RID: 117
	public bool BurstPSAStickyDirection;

	// Token: 0x04000076 RID: 118
	public bool BurstPSAAirTurnAssist;

	// Token: 0x04000077 RID: 119
	public float BurstPSAAirTurnBlend = 0.6f;

	// Token: 0x04000078 RID: 120
	public float BurstPSADuration = 200f;

	// Token: 0x04000079 RID: 121
	public bool HighJumpEnabled;

	// Token: 0x0400007A RID: 122
	public float HighJumpSpeed = 5f;

	// Token: 0x0400007B RID: 123
	public int HighJumpKeybind = 7;

	// Token: 0x0400007C RID: 124
	public bool Cr1ptsPSAEnabled;

	// Token: 0x0400007D RID: 125
	public float Cr1ptsPSAStrength = 0.5f;

	// Token: 0x0400007E RID: 126
	public float Cr1ptsPSALerpSpeed = 0.35f;

	// Token: 0x0400007F RID: 127
	public int Cr1ptsPSAKeybind = 10;

	// Token: 0x04000080 RID: 128
	public bool VelmaxEnabled;

	// Token: 0x04000081 RID: 129
	public float VelmaxMaxJumpSpeed = 6.5f;

	// Token: 0x04000082 RID: 130
	public float VelmaxJumpMultiplier = 1.1f;

	// Token: 0x04000083 RID: 131
	public bool ForceTagFreezeEnabled;

	// Token: 0x04000084 RID: 132
	public int ForceTagFreezeKeybind = 4;

	// Token: 0x04000085 RID: 133
	public bool PanicEnabled;

	// Token: 0x04000086 RID: 134
	public int PanicKeybind;

	// Token: 0x04000087 RID: 135
	public bool LongArmsEnabled;

	// Token: 0x04000088 RID: 136
	public float LongArmsAmount;

	// Token: 0x04000089 RID: 137
	public bool LongArmsBypassEnabled;

	// Token: 0x0400008A RID: 138
	public int LongArmsBypassKeybind;

	// Token: 0x0400008B RID: 139
	public bool LongArmsBypassLeftHand;

	// Token: 0x0400008C RID: 140
	public bool LongArmsBypassRightHand;

	// Token: 0x0400008D RID: 141
	public bool LongArmsBypassSmoothEnabled;

	// Token: 0x0400008E RID: 142
	public float LongArmsBypassSmoothness;

	// Token: 0x0400008F RID: 143
	public bool LongArmsBypassYOffsetEnabled;

	// Token: 0x04000090 RID: 144
	public float LongArmsBypassYOffset;

	// Token: 0x04000091 RID: 145
	public bool NoFingerMovementEnabled;

	// Token: 0x04000092 RID: 146
	public bool FakeQuestMenuEnabled;

	// Token: 0x04000093 RID: 147
	public int FakeQuestMenuKeybind;

	// Token: 0x04000094 RID: 148
	public bool FakeReportMenuEnabled;

	// Token: 0x04000095 RID: 149
	public int FakeReportMenuKeybind;

	// Token: 0x04000096 RID: 150
	public bool FakePowerOffEnabled;

	// Token: 0x04000097 RID: 151
	public int FakePowerOffKeybind;

	// Token: 0x04000098 RID: 152
	public bool GrayScreenEnabled;

	// Token: 0x04000099 RID: 153
	public int GrayScreenKeybind;

	// Token: 0x0400009A RID: 154
	public int GrayScreenDirection;

	// Token: 0x0400009B RID: 155
	public float GrayScreenDuration;

	// Token: 0x0400009C RID: 156
	public float GrayScreenArmDistance;

	// Token: 0x0400009D RID: 157
	public float GrayScreenArmSpread;

	// Token: 0x0400009E RID: 158
	public bool HzSliderEnabled;

	// Token: 0x0400009F RID: 159
	public int TargetHz;

	// Token: 0x040000A0 RID: 160
	public bool NoSlipEnabled;

	// Token: 0x040000A1 RID: 161
	public bool SlipSlapEnabled;

	// Token: 0x040000A2 RID: 162
	public float SlipSlapMultiplier;

	// Token: 0x040000A3 RID: 163
	public bool SurfaceSlipEnabled;

	// Token: 0x040000A4 RID: 164
	public float SurfaceSlipWalls;

	// Token: 0x040000A5 RID: 165
	public float SurfaceSlipLowerSlippery;

	// Token: 0x040000A6 RID: 166
	public float SurfaceSlipUpperSlippery;

	// Token: 0x040000A7 RID: 167
	public bool RemoveWindBarrierEnabled;

	// Token: 0x040000A8 RID: 168
	public bool NoArmCapEnabled;

	// Token: 0x040000A9 RID: 169
	public bool ArmCapEnabled;

	// Token: 0x040000AA RID: 170
	public float ArmCapValue;

	// Token: 0x040000AB RID: 171
	public bool PredsEnabled;

	// Token: 0x040000AC RID: 172
	public float PredsAmount;

	// Token: 0x040000AD RID: 173
	public bool PredsAlwaysOn;

	// Token: 0x040000AE RID: 174
	public float PredsAlwaysAmount;

	// Token: 0x040000AF RID: 175
	public int PredsHand;

	// Token: 0x040000B0 RID: 176
	public bool PredsLeftHandEnabled;

	// Token: 0x040000B1 RID: 177
	public bool PredsRightHandEnabled;

	// Token: 0x040000B2 RID: 178
	public bool AntiPredEnabled;

	// Token: 0x040000B3 RID: 179
	public bool VisualiseServerEnabled;

	// Token: 0x040000B4 RID: 180
	public bool VisualiseClientEnabled;

	// Token: 0x040000B5 RID: 181
	public bool DesyncEnabled;

	// Token: 0x040000B6 RID: 182
	public int DesyncMode;

	// Token: 0x040000B7 RID: 183
	public float DesyncDelayMs;

	// Token: 0x040000B8 RID: 184
	public bool DesyncVisualise;

	// Token: 0x040000B9 RID: 185
	public int DesyncLagSwitchKeybind;

	// Token: 0x040000BA RID: 186
	public float DesyncFakeLagFreezeMs;

	// Token: 0x040000BB RID: 187
	public float DesyncFakeLagIntervalMs;

	// Token: 0x040000BC RID: 188
	public bool ESPEnabled;

	// Token: 0x040000BD RID: 189
	public float ESPColorHue;

	// Token: 0x040000BE RID: 190
	public float ESPColorSat;

	// Token: 0x040000BF RID: 191
	public float ESPColorVal;

	// Token: 0x040000C0 RID: 192
	public bool TracersEnabled;

	// Token: 0x040000C1 RID: 193
	public bool HitboxesEnabled;

	// Token: 0x040000C2 RID: 194
	public bool NameTagsEnabled;

	// Token: 0x040000C3 RID: 195
	public bool CornerESPEnabled;

	// Token: 0x040000C4 RID: 196
	public bool BoneESPEnabled;

	// Token: 0x040000C5 RID: 197
	public bool ChamsEnabled;

	// Token: 0x040000C6 RID: 198
	public bool RecolorTaggedEnabled;

	// Token: 0x040000C7 RID: 199
	public float RecolorTaggedColorHue;

	// Token: 0x040000C8 RID: 200
	public float RecolorTaggedColorSat;

	// Token: 0x040000C9 RID: 201
	public float RecolorTaggedColorVal;

	// Token: 0x040000CA RID: 202
	public bool RGBESPEnabled;

	// Token: 0x040000CB RID: 203
	public int BoxESPMode;

	// Token: 0x040000CC RID: 204
	public bool FillESPEnabled;

	// Token: 0x040000CD RID: 205
	public float FillESPOpacity;

	// Token: 0x040000CE RID: 206
	public float FillESPColorHue;

	// Token: 0x040000CF RID: 207
	public float FillESPColorSat;

	// Token: 0x040000D0 RID: 208
	public float FillESPColorVal;

	// Token: 0x040000D1 RID: 209
	public bool TrailsEnabled;

	// Token: 0x040000D2 RID: 210
	public float TrailMinSpeed;

	// Token: 0x040000D3 RID: 211
	public float TrailSpeedScale;

	// Token: 0x040000D4 RID: 212
	public float TrailMinTime;

	// Token: 0x040000D5 RID: 213
	public float TrailMaxTime;

	// Token: 0x040000D6 RID: 214
	public float TrailWidth;

	// Token: 0x040000D7 RID: 215
	public bool TrailUsePlayerColor;

	// Token: 0x040000D8 RID: 216
	public float TrailColorHue;

	// Token: 0x040000D9 RID: 217
	public float TrailColorSat;

	// Token: 0x040000DA RID: 218
	public float TrailColorVal;

	// Token: 0x040000DB RID: 219
	public bool BeaconsEnabled;

	// Token: 0x040000DC RID: 220
	public float BeaconWidth;

	// Token: 0x040000DD RID: 221
	public bool ChinaHatESPEnabled;

	// Token: 0x040000DE RID: 222
	public bool RingESPEnabled;

	// Token: 0x040000DF RID: 223
	public bool RemoveLeavesEnabled;

	// Token: 0x040000E0 RID: 224
	public bool VibrationAlertsEnabled;

	// Token: 0x040000E1 RID: 225
	public float VibrationAlertDistance;

	// Token: 0x040000E2 RID: 226
	public float VibrationAlertStrength;

	// Token: 0x040000E3 RID: 227
	public bool PlayerGlowEnabled;

	// Token: 0x040000E4 RID: 228
	public float PlayerGlowColorHue;

	// Token: 0x040000E5 RID: 229
	public float PlayerGlowIntensity;

	// Token: 0x040000E6 RID: 230
	public bool BreadcrumbsEnabled;

	// Token: 0x040000E7 RID: 231
	public float BreadcrumbSpacing;

	// Token: 0x040000E8 RID: 232
	public float BreadcrumbSize;

	// Token: 0x040000E9 RID: 233
	public float BreadcrumbColorHue;

	// Token: 0x040000EA RID: 234
	public float BreadcrumbColorSat;

	// Token: 0x040000EB RID: 235
	public float BreadcrumbColorVal;

	// Token: 0x040000EC RID: 236
	public bool KillFeedEnabled;

	// Token: 0x040000ED RID: 237
	public float KillFeedDuration;

	// Token: 0x040000EE RID: 238
	public bool KillFeedWorldSpace;

	// Token: 0x040000EF RID: 239
	public bool TickRateEnabled;

	// Token: 0x040000F0 RID: 240
	public float TickRate;

	// Token: 0x040000F1 RID: 241
	public bool SoundESPEnabled;

	// Token: 0x040000F2 RID: 242
	public float SoundESPDistance;

	// Token: 0x040000F3 RID: 243
	public float SoundESPVolume;

	// Token: 0x040000F4 RID: 244
	public float SoundESPCooldown;

	// Token: 0x040000F5 RID: 245
	public int SoundESPPreset;

	// Token: 0x040000F6 RID: 246
	public string SoundESPCustomPath;

	// Token: 0x040000F7 RID: 247
	public bool SlideControlEnabled;

	// Token: 0x040000F8 RID: 248
	public float SlideControlAmount;

	// Token: 0x040000F9 RID: 249
	public bool TagNotificationEnabled;

	// Token: 0x040000FA RID: 250
	public float TagNotificationDistance;

	// Token: 0x040000FB RID: 251
	public bool NameTagsModEnabled;

	// Token: 0x040000FC RID: 252
	public float NameTagFontSize;

	// Token: 0x040000FD RID: 253
	public float NameTagOffset;

	// Token: 0x040000FE RID: 254
	public float NameTagRenderDistance;

	// Token: 0x040000FF RID: 255
	public bool PlatformTagsEnabled;

	// Token: 0x04000100 RID: 256
	public float PlatformTagFontSize;

	// Token: 0x04000101 RID: 257
	public float PlatformTagOffset;

	// Token: 0x04000102 RID: 258
	public bool FPSTagsModEnabled;

	// Token: 0x04000103 RID: 259
	public float FPSTagFontSize;

	// Token: 0x04000104 RID: 260
	public float FPSTagOffset;

	// Token: 0x04000105 RID: 261
	public bool AutoBranchEnabled;

	// Token: 0x04000106 RID: 262
	public int AutoBranchRecordKeybind;

	// Token: 0x04000107 RID: 263
	public int AutoBranchStopKeybind;

	// Token: 0x04000108 RID: 264
	public int AutoBranchReplayKeybind;

	// Token: 0x04000109 RID: 265
	public bool AutoBranchLoop;

	// Token: 0x0400010A RID: 266
	public bool AutoBranchMovePlayer;

	// Token: 0x0400010B RID: 267
	public bool AutoBranchSmartMode;

	// Token: 0x0400010C RID: 268
	public float AutoBranchMatchRadius;

	// Token: 0x0400010D RID: 269
	public float AutoBranchBlendTime;

	// Token: 0x0400010E RID: 270
	public bool SkyboxEnabled;

	// Token: 0x0400010F RID: 271
	public int SkyboxMode;

	// Token: 0x04000110 RID: 272
	public float SkyboxColorHue;

	// Token: 0x04000111 RID: 273
	public float SkyboxColorSat;

	// Token: 0x04000112 RID: 274
	public float SkyboxColorVal;

	// Token: 0x04000113 RID: 275
	public float SkyboxColor2Hue;

	// Token: 0x04000114 RID: 276
	public float SkyboxColor2Sat;

	// Token: 0x04000115 RID: 277
	public float SkyboxColor2Val;

	// Token: 0x04000116 RID: 278
	public string SkyboxImagePath;

	// Token: 0x04000117 RID: 279
	public float SkyboxGradientSpeed;

	// Token: 0x04000118 RID: 280
	public int SkyboxStarCount;

	// Token: 0x04000119 RID: 281
	public float SkyboxStarSize;

	// Token: 0x0400011A RID: 282
	public bool JewishMusicEnabled;

	// Token: 0x0400011B RID: 283
	public int PullModMode;

	// Token: 0x0400011C RID: 284
	public float PullRandomiseMin;

	// Token: 0x0400011D RID: 285
	public float PullRandomiseMax;

	// Token: 0x0400011E RID: 286
	public bool OneFramePullEnabled;

	// Token: 0x0400011F RID: 287
	public float OneFramePullStrength;

	// Token: 0x04000120 RID: 288
	public float OneFramePullThreshold;

	// Token: 0x04000121 RID: 289
	public int OneFramePullActivation;

	// Token: 0x04000122 RID: 290
	public bool OneFramePullToggleMode;

	// Token: 0x04000123 RID: 291
	public bool RealPullEnabled;

	// Token: 0x04000124 RID: 292
	public float RealPullDistance;

	// Token: 0x04000125 RID: 293
	public float RealPullThreshold;

	// Token: 0x04000126 RID: 294
	public int RealPullActivation;

	// Token: 0x04000127 RID: 295
	public bool RealPullToggleMode;

	// Token: 0x04000128 RID: 296
	public float RealPullDownAmount;

	// Token: 0x04000129 RID: 297
	public bool RealPullSlopeFix;

	// Token: 0x0400012A RID: 298
	public bool DownControllerEnabled;

	// Token: 0x0400012B RID: 299
	public int DownControllerKeybind;

	// Token: 0x0400012C RID: 300
	public float DownControllerDistance;

	// Token: 0x0400012D RID: 301
	public float DownControllerSpeed;

	// Token: 0x0400012E RID: 302
	public bool MainPullEnabled;

	// Token: 0x0400012F RID: 303
	public float MainPullStrength;

	// Token: 0x04000130 RID: 304
	public float MainPullTpTime;

	// Token: 0x04000131 RID: 305
	public float MainPullSmoothing;

	// Token: 0x04000132 RID: 306
	public int MainPullActivation;

	// Token: 0x04000133 RID: 307
	public bool MainPullToggleMode;

	// Token: 0x04000134 RID: 308
	public bool MainPullRaycastEnabled;

	// Token: 0x04000135 RID: 309
	public bool MainPullVelThresholdEnabled;

	// Token: 0x04000136 RID: 310
	public float MainPullVelThreshold;

	// Token: 0x04000137 RID: 311
	public bool MainPullThresholdEnabled;

	// Token: 0x04000138 RID: 312
	public float MainPullThresholdTime;

	// Token: 0x04000139 RID: 313
	public bool MainPullPredictEnabled;

	// Token: 0x0400013A RID: 314
	public int MainPullPredictPoints;

	// Token: 0x0400013B RID: 315
	public float MainPullPredictCheckStart;

	// Token: 0x0400013C RID: 316
	public float MainPullPredictCheckEnd;

	// Token: 0x0400013D RID: 317
	public int MainPullPredictStopBack;

	// Token: 0x0400013E RID: 318
	public bool MainPullPredictFromHand;

	// Token: 0x0400013F RID: 319
	public bool MainPullSurfaceAlignEnabled;

	// Token: 0x04000140 RID: 320
	public float MainPullSurfaceAlignRadius;

	// Token: 0x04000141 RID: 321
	public bool MainPullWallPullEnabled;

	// Token: 0x04000142 RID: 322
	public int MainPullWallPullKeybind;

	// Token: 0x04000143 RID: 323
	public float MainPullWallPullStrength;

	// Token: 0x04000144 RID: 324
	public float MainPullWallPullTpTime;

	// Token: 0x04000145 RID: 325
	public float MainPullWallPullSmoothing;

	// Token: 0x04000146 RID: 326
	public bool WallPullModEnabled;

	// Token: 0x04000147 RID: 327
	public float WallPullModStrength;

	// Token: 0x04000148 RID: 328
	public float WallPullModTpTime;

	// Token: 0x04000149 RID: 329
	public float WallPullModSmoothing;

	// Token: 0x0400014A RID: 330
	public float WallPullModVelThreshold;

	// Token: 0x0400014B RID: 331
	public int WallPullModActivation;

	// Token: 0x0400014C RID: 332
	public bool WallPullModToggleMode;

	// Token: 0x0400014D RID: 333
	public bool PullV3Enabled;

	// Token: 0x0400014E RID: 334
	public float PullV3Strength;

	// Token: 0x0400014F RID: 335
	public float PullV3Threshold;

	// Token: 0x04000150 RID: 336
	public float PullV3TpTime;

	// Token: 0x04000151 RID: 337
	public int PullV3Activation;

	// Token: 0x04000152 RID: 338
	public bool PullV3ToggleMode;

	// Token: 0x04000153 RID: 339
	public bool PullV3LeftHand;

	// Token: 0x04000154 RID: 340
	public bool PullV3RightHand;

	// Token: 0x04000155 RID: 341
	public bool PullV3MidPullRequired;

	// Token: 0x04000156 RID: 342
	public float PullV3ResetTime;

	// Token: 0x04000157 RID: 343
	public int PullV3MaxStacks;

	// Token: 0x04000158 RID: 344
	public float PullV3FreezeStrength;
}
