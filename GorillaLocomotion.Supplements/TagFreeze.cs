using System;
using System.Runtime.CompilerServices;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x02000039 RID: 57
public static class TagFreeze
{
	// Token: 0x060001AB RID: 427 RVA: 0x0001D4E0 File Offset: 0x0001B6E0
	public static bool IsHandOnSlab()
	{
		bool flag2;
		try
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag = instance == null;
			if (flag)
			{
				flag2 = false;
			}
			else
			{
				bool flag3 = !TagFreeze.slabSearched || TagFreeze.slabObject == null;
				if (flag3)
				{
					TagFreeze.slabObject = GameObject.Find(TagFreeze.SlabPath);
					TagFreeze.slabSearched = true;
					bool flag4 = TagFreeze.slabObject == null;
					if (flag4)
					{
						return false;
					}
				}
				Vector3 position = instance.LeftHand.controllerTransform.position;
				Vector3 position2 = instance.RightHand.controllerTransform.position;
				flag2 = TagFreeze.IsPointOverSlab(position) || TagFreeze.IsPointOverSlab(position2);
			}
		}
		catch
		{
			flag2 = false;
		}
		return flag2;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0001D5A4 File Offset: 0x0001B7A4
	public static void Update()
	{
		bool panicEnabled = Settings.PanicEnabled;
		if (panicEnabled)
		{
			bool flag = Settings.IsPressed(Settings.PanicKeybind);
			bool flag2 = flag && !TagFreeze.panicHeld;
			if (flag2)
			{
				TagFreeze.TriggerPanic();
			}
			TagFreeze.panicHeld = flag;
		}
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0001D5E8 File Offset: 0x0001B7E8
	public static void TriggerPanic()
	{
		Settings.PanicActive = !Settings.PanicActive;
		bool panicActive = Settings.PanicActive;
		if (panicActive)
		{
			Settings.TestPullModEnabled = false;
			Settings.MainPullEnabled = false;
			Settings.PullV3Enabled = false;
			Settings.PSAEnabled = false;
			Settings.VeloPSAEnabled = false;
			Settings.BurstPSAEnabled = false;
			Settings.HighJumpEnabled = false;
			Settings.Cr1ptsPSAEnabled = false;
			Settings.PSASkiddedEnabled = false;
			Settings.WallWalkEnabled = false;
			Settings.CGTWallWalkEnabled = false;
			Settings.VelmaxEnabled = false;
			Settings.PredsEnabled = false;
			Settings.PredsAlwaysOn = false;
			Settings.ESPEnabled = false;
			Settings.TagAuraEnabled = false;
			Settings.DCFlickEnabled = false;
			Settings.DcFlickAutoEnabled = false;
			Settings.HitboxExpanderEnabled = false;
			Settings.SlipSlapEnabled = false;
			Settings.NoSlipEnabled = false;
			Settings.RemoveWindBarrierEnabled = false;
			Settings.ForceTagFreezeEnabled = false;
			Settings.LongArmsEnabled = false;
			Settings.LongArmsBypassEnabled = false;
			Settings.GrayScreenEnabled = false;
			Settings.FakeQuestMenuEnabled = false;
			Settings.FakeReportMenuEnabled = false;
			Settings.FakePowerOffEnabled = false;
			Settings.NoFingerMovementEnabled = false;
		}
	}

	// Token: 0x060001AE RID: 430 RVA: 0x0001D6C8 File Offset: 0x0001B8C8
	public static void ApplyLongArms()
	{
		bool flag = !Settings.LongArmsEnabled;
		if (!flag)
		{
			try
			{
				bool flag2 = GorillaTagger.Instance == null || GorillaTagger.Instance.headCollider == null || GorillaTagger.Instance.leftHandTransform == null || GorillaTagger.Instance.rightHandTransform == null;
				if (!flag2)
				{
					GTPlayer instance = GTPlayer.Instance;
					bool flag3 = !(instance == null);
					if (flag3)
					{
						float longArmsAmount = Settings.LongArmsAmount;
						Vector3 position = GorillaTagger.Instance.headCollider.transform.position;
						Vector3 position2 = GorillaTagger.Instance.leftHandTransform.position;
						Vector3 position3 = GorillaTagger.Instance.rightHandTransform.position;
						Transform controllerTransform = instance.GetControllerTransform(true);
						Transform controllerTransform2 = instance.GetControllerTransform(false);
						bool flag4 = !(controllerTransform == null) && !(controllerTransform2 == null);
						if (flag4)
						{
							controllerTransform.position = position - (position - position2) * longArmsAmount;
							controllerTransform2.position = position - (position - position3) * longArmsAmount;
						}
					}
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x060001AF RID: 431 RVA: 0x0001D824 File Offset: 0x0001BA24
	public static void ApplyLongArmsBypass()
	{
		bool flag = Settings.LongArmsBypassEnabled && Settings.IsPressed(Settings.LongArmsBypassKeybind);
		if (flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = !(instance == null);
				if (flag2)
				{
					SphereCollider headCollider = instance.headCollider;
					bool flag3 = !(headCollider == null);
					if (flag3)
					{
						Transform transform = headCollider.transform;
						Bounds bounds = headCollider.bounds;
						Vector3 val = bounds.center + transform.up * bounds.extents.y - transform.forward * bounds.extents.z;
						bool longArmsBypassYOffsetEnabled = Settings.LongArmsBypassYOffsetEnabled;
						if (longArmsBypassYOffsetEnabled)
						{
							val += transform.up * Settings.LongArmsBypassYOffset;
						}
						bool longArmsBypassSmoothEnabled = Settings.LongArmsBypassSmoothEnabled;
						if (longArmsBypassSmoothEnabled)
						{
							bool flag4 = !TagFreeze.grayActive;
							if (flag4)
							{
								TagFreeze.savedLeftHandLocal = instance.LeftHand.controllerTransform.position;
								TagFreeze.savedRightHandLocal = instance.RightHand.controllerTransform.position;
								TagFreeze.grayActive = true;
							}
							float num = Mathf.Clamp01(Settings.LongArmsBypassSmoothness * Time.deltaTime);
							TagFreeze.savedLeftHandLocal = Vector3.Lerp(TagFreeze.savedLeftHandLocal, val, num);
							TagFreeze.savedRightHandLocal = Vector3.Lerp(TagFreeze.savedRightHandLocal, val, num);
							bool longArmsBypassLeftHand = Settings.LongArmsBypassLeftHand;
							if (longArmsBypassLeftHand)
							{
								instance.LeftHand.controllerTransform.position = TagFreeze.savedLeftHandLocal;
							}
							bool longArmsBypassRightHand = Settings.LongArmsBypassRightHand;
							if (longArmsBypassRightHand)
							{
								instance.RightHand.controllerTransform.position = TagFreeze.savedRightHandLocal;
							}
						}
						else
						{
							bool longArmsBypassLeftHand2 = Settings.LongArmsBypassLeftHand;
							if (longArmsBypassLeftHand2)
							{
								instance.LeftHand.controllerTransform.position = val;
							}
							bool longArmsBypassRightHand2 = Settings.LongArmsBypassRightHand;
							if (longArmsBypassRightHand2)
							{
								instance.RightHand.controllerTransform.position = val;
							}
						}
					}
				}
				return;
			}
			catch
			{
				return;
			}
		}
		TagFreeze.grayActive = false;
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x0001DA38 File Offset: 0x0001BC38
	private static void EnsureGrayOverlay()
	{
		bool flag = !(TagFreeze.grayOverlay != null);
		if (flag)
		{
		}
		TagFreeze.grayOverlay = null;
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x0001DA64 File Offset: 0x0001BC64
	public static void ApplyGrayScreen()
	{
		bool flag2 = !Settings.GrayScreenEnabled;
		if (flag2)
		{
			bool flag3 = TagFreeze.grayHeld;
			if (flag3)
			{
				TagFreeze.EnsureGrayOverlay();
			}
		}
		else
		{
			bool flag = Settings.IsPressed(Settings.GrayScreenKeybind);
			bool flag4 = flag && !TagFreeze.longArmsActive;
			if (flag4)
			{
				TagFreeze.grayHeld = true;
				TagFreeze.grayTimer = Settings.GrayScreenDuration;
				TagFreeze.bypassActive = false;
			}
			TagFreeze.longArmsActive = flag;
			bool flag5 = !TagFreeze.grayHeld;
			if (!flag5)
			{
				TagFreeze.grayTimer -= Time.deltaTime;
				bool flag6 = TagFreeze.grayTimer <= 0f;
				if (flag6)
				{
					TagFreeze.EnsureGrayOverlay();
				}
				else
				{
					try
					{
						GTPlayer instance = GTPlayer.Instance;
						bool flag7 = instance == null;
						if (!flag7)
						{
							bool flag8 = !TagFreeze.bypassActive;
							if (flag8)
							{
								bool flag9 = GorillaTagger.Instance == null || GorillaTagger.Instance.offlineVRRig == null;
								if (flag9)
								{
									return;
								}
								Transform val = null;
								bool flag10 = GorillaTagger.Instance.offlineVRRig.head != null && GorillaTagger.Instance.offlineVRRig.head.rigTarget != null;
								if (flag10)
								{
									val = GorillaTagger.Instance.offlineVRRig.head.rigTarget;
								}
								else
								{
									bool flag11 = GorillaTagger.Instance.offlineVRRig.headMesh != null;
									if (flag11)
									{
										val = GorillaTagger.Instance.offlineVRRig.headMesh.transform;
									}
								}
								bool flag12 = val == null;
								if (flag12)
								{
									return;
								}
								float num = ((Settings.GrayScreenDirection == HandSide.Left) ? (-90f) : 90f);
								TagFreeze.grayOverlay = val;
								TagFreeze.savedLeftRot = val.localRotation;
								TagFreeze.savedRightRot = TagFreeze.savedLeftRot * Quaternion.Euler(0f, num, 0f);
								Vector3 position = val.position;
								Vector3 val2 = -val.forward;
								float grayScreenArmDistance = Settings.GrayScreenArmDistance;
								Vector3 val3 = position + val2 * grayScreenArmDistance + Vector3.down * 0.3f;
								float grayScreenArmSpread = Settings.GrayScreenArmSpread;
								TagFreeze.grayLeftPos = val3 + -val.right * grayScreenArmSpread;
								TagFreeze.grayRightPos = val3 + val.right * grayScreenArmSpread;
								TagFreeze.bypassActive = true;
							}
							instance.LeftHand.controllerTransform.position = TagFreeze.grayLeftPos;
							instance.RightHand.controllerTransform.position = TagFreeze.grayRightPos;
							bool flag13 = GorillaTagger.Instance != null;
							if (flag13)
							{
								bool flag14 = GorillaTagger.Instance.leftHandTransform != null;
								if (flag14)
								{
									GorillaTagger.Instance.leftHandTransform.position = TagFreeze.grayLeftPos;
								}
								bool flag15 = GorillaTagger.Instance.rightHandTransform != null;
								if (flag15)
								{
									GorillaTagger.Instance.rightHandTransform.position = TagFreeze.grayRightPos;
								}
							}
						}
					}
					catch
					{
					}
				}
			}
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x0001DD9C File Offset: 0x0001BF9C
	public static void LateUpdate()
	{
		bool flag = !TagFreeze.grayHeld || !TagFreeze.bypassActive;
		if (!flag)
		{
			try
			{
				GTPlayer instance = GTPlayer.Instance;
				bool flag2 = instance != null;
				if (flag2)
				{
					instance.LeftHand.controllerTransform.position = TagFreeze.grayLeftPos;
					instance.RightHand.controllerTransform.position = TagFreeze.grayRightPos;
				}
				bool flag3 = GorillaTagger.Instance != null;
				if (flag3)
				{
					bool flag4 = GorillaTagger.Instance.leftHandTransform != null;
					if (flag4)
					{
						GorillaTagger.Instance.leftHandTransform.position = TagFreeze.grayLeftPos;
					}
					bool flag5 = GorillaTagger.Instance.rightHandTransform != null;
					if (flag5)
					{
						GorillaTagger.Instance.rightHandTransform.position = TagFreeze.grayRightPos;
					}
				}
				bool flag6 = TagFreeze.grayOverlay != null;
				if (flag6)
				{
					bool flag7 = TagFreeze.grayOverlay.parent != null;
					if (flag7)
					{
						TagFreeze.grayOverlay.localRotation = TagFreeze.savedRightRot;
					}
					else
					{
						TagFreeze.grayOverlay.rotation = TagFreeze.savedRightRot;
					}
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x0001DEDC File Offset: 0x0001C0DC
	[CompilerGenerated]
	internal static bool IsPointOverSlab(Vector3 grayDir)
	{
		Collider[] array = Physics.OverlapSphere(grayDir, 0.25f, -1, 1);
		int num2 = 0;
		for (;;)
		{
			bool flag = num2 >= array.Length;
			if (flag)
			{
				break;
			}
			Collider val2 = array[num2];
			bool flag2 = !(val2 == null);
			if (flag2)
			{
				Transform val3 = val2.transform;
				bool flag3 = !(val3 == TagFreeze.slabObject.transform);
				if (!flag3)
				{
					goto IL_008A;
				}
				bool flag4 = val3.IsChildOf(TagFreeze.slabObject.transform);
				if (flag4)
				{
					goto Block_4;
				}
				num2++;
			}
			else
			{
				num2++;
			}
		}
		return false;
		Block_4:
		return true;
		IL_008A:
		return true;
	}

	// Token: 0x0400044F RID: 1103
	private static readonly string SlabPath = "Environment Objects/LocalObjects_Prefab/Forest/Terrain/campgroundstructure/concrete slab";

	// Token: 0x04000450 RID: 1104
	private static GameObject slabObject;

	// Token: 0x04000451 RID: 1105
	private static bool slabSearched = false;

	// Token: 0x04000452 RID: 1106
	private static bool panicHeld = false;

	// Token: 0x04000453 RID: 1107
	private static bool savedTestPull;

	// Token: 0x04000454 RID: 1108
	private static bool savedMainPull;

	// Token: 0x04000455 RID: 1109
	private static bool savedPullV3;

	// Token: 0x04000456 RID: 1110
	private static bool savedPSA;

	// Token: 0x04000457 RID: 1111
	private static bool savedVeloPSA;

	// Token: 0x04000458 RID: 1112
	private static bool savedBurstPSA;

	// Token: 0x04000459 RID: 1113
	private static bool savedHighJump;

	// Token: 0x0400045A RID: 1114
	private static bool savedCr1ptsPSA;

	// Token: 0x0400045B RID: 1115
	private static bool savedWallWalk;

	// Token: 0x0400045C RID: 1116
	private static bool savedCgtWallWalk;

	// Token: 0x0400045D RID: 1117
	private static bool savedVelmax;

	// Token: 0x0400045E RID: 1118
	private static bool savedPreds;

	// Token: 0x0400045F RID: 1119
	private static bool savedPredsAlwaysOn;

	// Token: 0x04000460 RID: 1120
	private static bool savedEsp;

	// Token: 0x04000461 RID: 1121
	private static bool savedTagAura;

	// Token: 0x04000462 RID: 1122
	private static bool savedDcFlick;

	// Token: 0x04000463 RID: 1123
	private static bool savedDcFlickAuto;

	// Token: 0x04000464 RID: 1124
	private static bool savedHitboxExpander;

	// Token: 0x04000465 RID: 1125
	private static bool savedSlipSlap;

	// Token: 0x04000466 RID: 1126
	private static bool savedNoSlip;

	// Token: 0x04000467 RID: 1127
	private static bool savedRemoveWindBarrier;

	// Token: 0x04000468 RID: 1128
	private static bool savedPSASkidded;

	// Token: 0x04000469 RID: 1129
	private static bool savedForceTagFreeze;

	// Token: 0x0400046A RID: 1130
	private static bool savedLongArms;

	// Token: 0x0400046B RID: 1131
	private static bool savedLongArmsBypass;

	// Token: 0x0400046C RID: 1132
	private static bool savedGrayScreen;

	// Token: 0x0400046D RID: 1133
	private static bool savedFakeQuestMenu;

	// Token: 0x0400046E RID: 1134
	private static bool savedFakeReportMenu;

	// Token: 0x0400046F RID: 1135
	private static bool savedFakePowerOff;

	// Token: 0x04000470 RID: 1136
	private static bool savedNoFingerMovement;

	// Token: 0x04000471 RID: 1137
	private static Vector3 savedLeftHandLocal;

	// Token: 0x04000472 RID: 1138
	private static Vector3 savedRightHandLocal;

	// Token: 0x04000473 RID: 1139
	private static bool grayActive = false;

	// Token: 0x04000474 RID: 1140
	private static bool grayHeld = false;

	// Token: 0x04000475 RID: 1141
	private static float grayTimer = 0f;

	// Token: 0x04000476 RID: 1142
	private static bool longArmsActive = false;

	// Token: 0x04000477 RID: 1143
	private static bool bypassActive = false;

	// Token: 0x04000478 RID: 1144
	private static Vector3 grayLeftPos;

	// Token: 0x04000479 RID: 1145
	private static Vector3 grayRightPos;

	// Token: 0x0400047A RID: 1146
	private static Transform grayOverlay;

	// Token: 0x0400047B RID: 1147
	private static Quaternion savedLeftRot;

	// Token: 0x0400047C RID: 1148
	private static Quaternion savedRightRot;
}
