using System;
using System.Reflection;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200002D RID: 45
internal class RealPull
{
	// Token: 0x0600014A RID: 330 RVA: 0x00018440 File Offset: 0x00016640
	public static void Update()
	{
		bool flag3 = !Settings.RealPullEnabled;
		if (!flag3)
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag4 = instance == null;
			if (!flag4)
			{
				Transform controllerTransform = instance.LeftHand.controllerTransform;
				Transform controllerTransform2 = instance.RightHand.controllerTransform;
				bool flag5 = controllerTransform == null || controllerTransform2 == null;
				if (!flag5)
				{
					bool realPullToggleMode = Settings.RealPullToggleMode;
					if (realPullToggleMode)
					{
						bool flag = Settings.IsPressed(Settings.RealPullActivation);
						bool flag6 = flag && !RealPull.activationHeld;
						if (flag6)
						{
							Settings.RealPullToggleActive = !Settings.RealPullToggleActive;
						}
						RealPull.activationHeld = flag;
						bool flag7 = !Settings.RealPullToggleActive;
						if (flag7)
						{
							RealPull.leftWasColliding = instance.LeftHand.wasColliding;
							RealPull.rightWasColliding = instance.RightHand.wasColliding;
							RealPull.appliedOffset = Vector3.zero;
							return;
						}
					}
					bool flag8 = !Settings.RealPullToggleMode && !Settings.IsPressed(Settings.RealPullActivation);
					if (flag8)
					{
						RealPull.appliedOffset = Vector3.zero;
						RealPull.leftWasColliding = instance.LeftHand.wasColliding;
						RealPull.rightWasColliding = instance.RightHand.wasColliding;
					}
					else
					{
						bool wasColliding = instance.LeftHand.wasColliding;
						bool wasColliding2 = instance.RightHand.wasColliding;
						bool flag9 = wasColliding;
						if (flag9)
						{
							RealPull.useLeftHand = true;
						}
						else
						{
							bool flag10 = wasColliding2;
							if (flag10)
							{
								RealPull.useLeftHand = false;
							}
							else
							{
								bool flag11 = RealPull.leftWasColliding && !RealPull.rightWasColliding;
								if (flag11)
								{
									RealPull.useLeftHand = true;
								}
								else
								{
									bool flag12 = RealPull.rightWasColliding && !RealPull.leftWasColliding;
									if (flag12)
									{
										RealPull.useLeftHand = false;
									}
								}
							}
						}
						Rigidbody val = RealPull.GetRigidbody(instance);
						Vector3 val2 = ((val != null) ? RealPull.GetVelocity(val) : Vector3.zero);
						float magnitude = val2.magnitude;
						float num = 6.5f * Settings.RealPullThreshold;
						bool flag2 = wasColliding || wasColliding2;
						Vector3 val3 = Vector3.zero;
						bool flag13 = flag2 && magnitude / instance.scale > num;
						if (flag13)
						{
							Vector3 val4 = default(Vector3);
							val4..ctor(val2.x, 0f, val2.z);
							bool flag14 = val4.sqrMagnitude > 0.0001f;
							if (flag14)
							{
								Vector3 val5 = -val4.normalized;
								bool realPullSlopeFix = Settings.RealPullSlopeFix;
								if (realPullSlopeFix)
								{
									Vector3 val6 = RealPull.GetGroundNormal(instance);
									bool flag15 = val6 != Vector3.zero;
									if (flag15)
									{
										Vector3 val7 = Vector3.ProjectOnPlane(val5, val6);
										bool flag16 = val7.sqrMagnitude > 0.0001f;
										if (flag16)
										{
											val5 = val7.normalized;
										}
									}
								}
								Vector3 val8 = val5 * Mathf.Max(0f, Settings.RealPullDistance);
								val8 += Vector3.down * Mathf.Max(0f, Settings.RealPullDownAmount);
								float magnitude2 = val8.magnitude;
								bool flag17 = magnitude2 > 0.0001f;
								if (flag17)
								{
									val3 = RealPull.ClampByObstacles(instance, val8 / magnitude2, magnitude2);
								}
							}
						}
						float num2 = Mathf.Clamp01(0.2f);
						RealPull.appliedOffset = Vector3.Lerp(RealPull.appliedOffset, val3, num2);
						bool flag18 = RealPull.appliedOffset.sqrMagnitude < 1E-07f;
						if (flag18)
						{
							RealPull.appliedOffset = Vector3.zero;
						}
						Transform val9 = (RealPull.useLeftHand ? controllerTransform : controllerTransform2);
						RealPull.appliedLeftHand = RealPull.useLeftHand;
						bool flag19 = val9 != null && RealPull.appliedOffset.sqrMagnitude > 0f;
						if (flag19)
						{
							val9.position += RealPull.appliedOffset;
						}
						RealPull.leftWasColliding = wasColliding;
						RealPull.rightWasColliding = wasColliding2;
					}
				}
			}
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0001880C File Offset: 0x00016A0C
	public static void LateUpdate()
	{
		bool flag = !Settings.RealPullEnabled || RealPull.appliedOffset.sqrMagnitude <= 0f;
		if (!flag)
		{
			try
			{
				bool flag2 = !(GorillaTagger.Instance == null) && !(GorillaTagger.Instance.offlineVRRig == null);
				if (flag2)
				{
					VRRig offlineVRRig = GorillaTagger.Instance.offlineVRRig;
					VRMap val = (RealPull.appliedLeftHand ? offlineVRRig.leftHand : offlineVRRig.rightHand);
					bool flag3 = val != null && val.rigTarget != null;
					if (flag3)
					{
						Transform rigTarget = val.rigTarget;
						rigTarget.position -= RealPull.appliedOffset;
					}
				}
			}
			catch
			{
			}
		}
	}

	// Token: 0x0600014C RID: 332 RVA: 0x000188E0 File Offset: 0x00016AE0
	private static Vector3 ClampByObstacles(GTPlayer player, Vector3 delta, float distance)
	{
		Vector3 val = delta * distance;
		bool flag = distance < 0.0001f;
		Vector3 vector;
		if (flag)
		{
			vector = val;
		}
		else
		{
			Transform val2 = (RealPull.useLeftHand ? player.LeftHand.controllerTransform : player.RightHand.controllerTransform);
			Vector3 val3 = ((val2 != null) ? val2.position : player.transform.position);
			float num = Mathf.Clamp01(0.7f);
			float num2 = Mathf.Clamp01(1f);
			bool flag2 = num2 < num;
			if (flag2)
			{
				num2 = num;
			}
			int num3 = Mathf.Clamp(5, 0, 9);
			Vector3[] array = (Vector3[])new Vector3[11];
			array[0] = val3;
			for (int i = 1; i <= 10; i++)
			{
				float num4 = (float)i / 10f;
				array[i] = val3 + val * num4;
			}
			int num5 = Mathf.Max(1, Mathf.RoundToInt(num * 10f));
			int num6 = Mathf.Clamp(Mathf.RoundToInt(num2 * 10f), num5, 10);
			int num7 = 10;
			int j = num5;
			RaycastHit val4 = default(RaycastHit);
			while (j <= num6)
			{
				int num8 = j;
				bool flag3 = Physics.Linecast(array[num8 - 1], array[j], ref val4);
				if (flag3)
				{
					int num9 = j;
					num7 = Mathf.Max(0, num9 - 1 - num3);
					break;
				}
				j++;
			}
			bool flag4 = num7 < 10;
			if (flag4)
			{
				float num10 = (float)num7 / 10f;
				val *= num10;
			}
			bool flag5 = val.sqrMagnitude > 0.0001f;
			if (flag5)
			{
				Vector3 normalized = val.normalized;
				float magnitude = val.magnitude;
				RaycastHit val5 = default(RaycastHit);
				bool flag6 = Physics.SphereCast(val3, 0.1f, normalized, ref val5, magnitude);
				if (flag6)
				{
					float num11 = Mathf.Max(0f, val5.distance);
					val = normalized * num11;
				}
			}
			vector = val;
		}
		return vector;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x00018AF0 File Offset: 0x00016CF0
	private static Vector3 GetGroundNormal(GTPlayer player)
	{
		RaycastHit val = default(RaycastHit);
		bool flag = !(player == null);
		Vector3 vector;
		if (flag)
		{
			Vector3 val2 = player.transform.position + Vector3.up * (0.5f * player.scale);
			float num8 = 3f * player.scale;
			bool flag2 = !Physics.Raycast(val2, Vector3.down, ref val, num8);
			if (flag2)
			{
				vector = Vector3.zero;
			}
			else
			{
				bool flag3 = val.normal.y <= 0.1f;
				if (flag3)
				{
					vector = Vector3.zero;
				}
				else
				{
					vector = val.normal;
				}
			}
		}
		else
		{
			vector = Vector3.zero;
		}
		return vector;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x00018BAC File Offset: 0x00016DAC
	private static Rigidbody GetRigidbody(GTPlayer player)
	{
		bool flag = RealPull.cachedRigidbody == null;
		if (flag)
		{
			RealPull.cachedRigidbody = player.GetComponent<Rigidbody>();
		}
		return RealPull.cachedRigidbody;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x00018BE0 File Offset: 0x00016DE0
	private static Vector3 GetVelocity(Rigidbody body)
	{
		bool flag = GTPlayer.Instance != null && GTPlayer.Instance.disableMovement;
		Vector3 vector;
		if (flag)
		{
			vector = GTPlayer.Instance.AveragedVelocity;
		}
		else
		{
			bool flag2 = body == null;
			if (flag2)
			{
				vector = Vector3.zero;
			}
			else
			{
				bool flag3 = !RealPull.velocityReflectionReady;
				if (flag3)
				{
					RealPull.velocityReflectionReady = true;
					Type typeFromHandle = typeof(Rigidbody);
					RealPull.velocityProperty = typeFromHandle.GetProperty("linearVelocity", BindingFlags.Instance | BindingFlags.Public);
					bool flag4 = RealPull.velocityProperty == null;
					if (flag4)
					{
						Type typeFromHandle2 = typeof(Rigidbody);
						RealPull.velocityProperty = typeFromHandle2.GetProperty("velocity", BindingFlags.Instance | BindingFlags.Public);
					}
				}
				bool flag5 = RealPull.velocityProperty != null;
				if (flag5)
				{
					try
					{
						return (Vector3)RealPull.velocityProperty.GetValue(body);
					}
					catch
					{
					}
				}
				vector = Vector3.zero;
			}
		}
		return vector;
	}

	// Token: 0x04000277 RID: 631
	private const bool DefaultObstacleCheck = true;

	// Token: 0x04000278 RID: 632
	private const bool DefaultSphereCast = true;

	// Token: 0x04000279 RID: 633
	private const bool DefaultSlopeFix = true;

	// Token: 0x0400027A RID: 634
	private const int RaySteps = 10;

	// Token: 0x0400027B RID: 635
	private const float CheckStart = 0.7f;

	// Token: 0x0400027C RID: 636
	private const float CheckEnd = 1f;

	// Token: 0x0400027D RID: 637
	private const int StopBackSteps = 5;

	// Token: 0x0400027E RID: 638
	private const float SphereRadius = 0.1f;

	// Token: 0x0400027F RID: 639
	private static bool leftWasColliding;

	// Token: 0x04000280 RID: 640
	private static bool rightWasColliding;

	// Token: 0x04000281 RID: 641
	private static bool activationHeld;

	// Token: 0x04000282 RID: 642
	private static bool useLeftHand = true;

	// Token: 0x04000283 RID: 643
	private static Vector3 appliedOffset = Vector3.zero;

	// Token: 0x04000284 RID: 644
	private static bool appliedLeftHand = true;

	// Token: 0x04000285 RID: 645
	private static Rigidbody cachedRigidbody = null;

	// Token: 0x04000286 RID: 646
	private static PropertyInfo velocityProperty = null;

	// Token: 0x04000287 RID: 647
	private static bool velocityReflectionReady = false;
}
