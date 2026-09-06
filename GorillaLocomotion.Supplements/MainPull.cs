using System;
using System.Reflection;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200001C RID: 28
internal class MainPull
{
	// Token: 0x060000C6 RID: 198 RVA: 0x0000F40C File Offset: 0x0000D60C
	private static bool IsMovementDisabled()
	{
		bool flag = GTPlayer.Instance != null;
		return flag && GTPlayer.Instance.disableMovement;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x0000F440 File Offset: 0x0000D640
	public static void Update()
	{
		bool flag3 = !Settings.MainPullEnabled;
		if (!flag3)
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag4 = instance == null;
			if (!flag4)
			{
				bool mainPullToggleMode = Settings.MainPullToggleMode;
				if (mainPullToggleMode)
				{
					bool flag = Settings.IsPressed(Settings.MainPullActivation);
					bool flag5 = flag && !MainPull.toggleHeld;
					if (flag5)
					{
						Settings.MainPullToggleActive = !Settings.MainPullToggleActive;
					}
					MainPull.toggleHeld = flag;
					bool flag6 = !Settings.MainPullToggleActive;
					if (flag6)
					{
						MainPull.smoothedOffset = Vector3.zero;
						MainPull.smoothedDirection = Vector3.zero;
						MainPull.thresholdArmed = false;
						MainPull.hasCachedOrigin = false;
						MainPull.leftHandWasColliding = instance.LeftHand.wasColliding;
						MainPull.rightHandWasColliding = instance.RightHand.wasColliding;
						return;
					}
				}
				MainPull.timer += Time.deltaTime;
				float num = 6.5f * Settings.MainPullVelThreshold;
				bool flag7 = Settings.MainPullToggleMode || Settings.IsPressed(Settings.MainPullActivation);
				if (flag7)
				{
					bool wasColliding = instance.LeftHand.wasColliding;
					bool wasColliding2 = instance.RightHand.wasColliding;
					int num2 = ((wasColliding && !MainPull.leftHandWasColliding) ? 1 : (wasColliding2 ? ((!MainPull.rightHandWasColliding) ? 1 : 0) : 0));
					bool flag2 = (MainPull.leftHandWasColliding && !wasColliding) || (MainPull.rightHandWasColliding && !wasColliding2);
					bool flag8 = num2 != 0;
					if (flag8)
					{
						int num3 = (wasColliding ? ((!MainPull.leftHandWasColliding) ? 1 : 0) : 0);
						MainPull.CachePullOrigin(instance, (byte)num3 > 0);
					}
					bool mainPullThresholdEnabled = Settings.MainPullThresholdEnabled;
					if (mainPullThresholdEnabled)
					{
						bool flag9 = MainPull.thresholdArmed;
						if (flag9)
						{
							MainPull.thresholdTimer += Time.deltaTime * 1000f;
							bool flag10 = wasColliding || wasColliding2;
							if (flag10)
							{
								MainPull.thresholdArmed = false;
								MainPull.hasCachedOrigin = false;
							}
							else
							{
								bool flag11 = MainPull.thresholdTimer >= Settings.MainPullThresholdTime;
								if (flag11)
								{
									MainPull.thresholdArmed = false;
									MainPull.timer = 0f;
								}
							}
						}
						bool flag12 = flag2 && !MainPull.thresholdArmed;
						if (flag12)
						{
							Rigidbody val = MainPull.GetRigidbody(instance);
							bool flag13 = val != null;
							if (flag13)
							{
								bool flag14 = MainPull.GetAveragedVelocity(val).magnitude / instance.scale > num;
								if (flag14)
								{
									MainPull.thresholdArmed = true;
									MainPull.thresholdTimer = 0f;
								}
							}
						}
					}
					else
					{
						MainPull.thresholdArmed = false;
						bool flag15 = wasColliding || wasColliding2;
						if (flag15)
						{
							Rigidbody val2 = MainPull.GetRigidbody(instance);
							bool flag16 = val2 != null;
							if (flag16)
							{
								bool flag17 = MainPull.GetAveragedVelocity(val2).magnitude / instance.scale > num;
								if (flag17)
								{
									MainPull.timer = 0f;
								}
							}
						}
						bool flag18 = flag2;
						if (flag18)
						{
							Rigidbody val3 = MainPull.GetRigidbody(instance);
							bool flag19 = val3 != null;
							if (flag19)
							{
								bool flag20 = MainPull.GetAveragedVelocity(val3).magnitude / instance.scale > num;
								if (flag20)
								{
									MainPull.timer = 0f;
								}
							}
						}
					}
					float mainPullTpTime = Settings.MainPullTpTime;
					bool flag21 = MainPull.timer < mainPullTpTime;
					if (flag21)
					{
						MainPull.ApplyPull(instance);
					}
					else
					{
						MainPull.smoothedOffset = Vector3.zero;
						MainPull.smoothedDirection = Vector3.zero;
						MainPull.hasCachedOrigin = false;
					}
				}
				else
				{
					MainPull.smoothedOffset = Vector3.zero;
					MainPull.smoothedDirection = Vector3.zero;
					MainPull.thresholdArmed = false;
					MainPull.hasCachedOrigin = false;
				}
				MainPull.leftHandWasColliding = instance.LeftHand.wasColliding;
				MainPull.rightHandWasColliding = instance.RightHand.wasColliding;
			}
		}
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x0000F7DC File Offset: 0x0000D9DC
	private static Rigidbody GetRigidbody(GTPlayer player)
	{
		bool flag = MainPull.cachedRigidbody == null;
		Rigidbody rigidbody;
		if (flag)
		{
			MainPull.cachedRigidbody = player.GetComponent<Rigidbody>();
			rigidbody = MainPull.cachedRigidbody;
		}
		else
		{
			rigidbody = MainPull.cachedRigidbody;
		}
		return rigidbody;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x0000F818 File Offset: 0x0000DA18
	private static void ApplyPull(GTPlayer player)
	{
		Rigidbody val = MainPull.GetRigidbody(player);
		bool flag3 = val == null;
		if (!flag3)
		{
			Vector3 val2 = MainPull.GetAveragedVelocity(val);
			bool flag4 = val2.magnitude > 0.5f;
			if (flag4)
			{
				MainPull.lastVelocity = val2;
			}
			Vector3 normalized = val2.normalized;
			bool flag5 = MainPull.smoothedDirection.sqrMagnitude < 0.001f;
			if (flag5)
			{
				MainPull.smoothedDirection = normalized;
			}
			else
			{
				MainPull.smoothedDirection = Vector3.Lerp(MainPull.smoothedDirection, normalized, 0.55f).normalized;
			}
			Vector3 val3 = MainPull.smoothedDirection * val2.magnitude * Settings.MainPullStrength * (Settings.MainPullTpTime * 4f);
			Vector3 position = player.transform.position;
			bool flag6;
			bool flag = (flag6 = Settings.MainPullWallPullEnabled && Settings.IsPressed(Settings.MainPullWallPullKeybind));
			if (flag6)
			{
				float num = Mathf.Abs(val2.y);
				bool flag7 = num < 0.5f;
				if (flag7)
				{
					num = val2.magnitude * 0.5f;
				}
				float num2 = num * Settings.MainPullWallPullStrength * (Settings.MainPullWallPullTpTime * 4f);
				val3..ctor(0f, Mathf.Abs(num2), 0f);
			}
			else
			{
				bool mainPullSurfaceAlignEnabled = Settings.MainPullSurfaceAlignEnabled;
				if (mainPullSurfaceAlignEnabled)
				{
					Vector3 val4 = MainPull.ClampByObstacles(position, Settings.MainPullSurfaceAlignRadius);
					bool flag8 = val4.sqrMagnitude > 0.001f;
					if (flag8)
					{
						float magnitude = val3.magnitude;
						Vector3 val5 = val3 - Vector3.Dot(val3, val4) * val4;
						bool flag9 = val5.sqrMagnitude > 0.0001f;
						if (flag9)
						{
							val3 = val5.normalized * magnitude;
							val3 = MainPull.AlignToSurface(player, val3, val4);
						}
					}
				}
				else
				{
					RaycastHit val6 = default(RaycastHit);
					bool flag10 = Physics.Raycast(position + Vector3.up * 0.5f, Vector3.down, ref val6, 2f);
					if (flag10)
					{
						Vector3 normal = val6.normal;
						Vector3 val7 = val3 - Vector3.Dot(val3, normal) * normal;
						bool flag11 = normal.y > 0.5f;
						if (flag11)
						{
							val3 = val7;
						}
					}
					float num3 = Mathf.Sqrt(val3.x * val3.x + val3.z * val3.z);
					bool flag12 = val3.y < (0f - num3) * 0.5f;
					if (flag12)
					{
						val3.y = (0f - num3) * 0.5f;
					}
					bool flag13 = val3.y > num3 * 1.5f;
					if (flag13)
					{
						val3.y = num3 * 1.5f;
					}
				}
			}
			bool flag14 = val3.sqrMagnitude > 0.0001f;
			if (flag14)
			{
				int num4 = (MainPull.leftHandWasColliding ? 1 : ((!MainPull.rightHandWasColliding) ? 1 : 0));
				bool flag2 = (byte)num4 > 0;
				Transform controllerTransform = player.GetControllerTransform(flag2);
				Vector3 val8 = ((controllerTransform != null) ? controllerTransform.position : position);
				Vector3[] array = (Vector3[])new Vector3[11];
				array[0] = val8;
				for (int i = 1; i <= 10; i++)
				{
					float num5 = (float)i / 10f;
					array[i] = position + val3 * num5;
				}
				int num6 = Mathf.Max(1, Mathf.RoundToInt(7f));
				int num7 = Mathf.Clamp(Mathf.RoundToInt(10f), num6, 10);
				int num8 = 10;
				RaycastHit val9 = default(RaycastHit);
				for (int j = num6; j <= num7; j++)
				{
					int num9 = j;
					bool flag15 = Physics.Linecast(array[num9 - 1], array[j], ref val9);
					if (flag15)
					{
						int num10 = j;
						num8 = Mathf.Max(0, num10 - 1 - 5);
						break;
					}
				}
				bool flag16 = num8 < 10;
				if (flag16)
				{
					float num11 = (float)num8 / 10f;
					val3 *= num11;
				}
			}
			bool flag17 = val3.sqrMagnitude > 0.0001f;
			if (flag17)
			{
				Vector3 normalized2 = val3.normalized;
				float magnitude2 = val3.magnitude;
				RaycastHit val10 = default(RaycastHit);
				bool flag18 = Physics.SphereCast(position, 0.12f, normalized2, ref val10, magnitude2);
				if (flag18)
				{
					bool flag19 = val10.normal.y > 0.3f;
					if (flag19)
					{
						val3 -= Vector3.Dot(val3, val10.normal) * val10.normal;
					}
					else
					{
						float num12 = Mathf.Max(0f, val10.distance);
						bool flag20 = num12 < 0.01f;
						if (flag20)
						{
							return;
						}
						val3 = normalized2 * num12;
					}
				}
			}
			float num13 = (flag ? Settings.MainPullWallPullSmoothing : Settings.MainPullSmoothing);
			bool flag21 = num13 > 0.01f;
			if (flag21)
			{
				MainPull.smoothedOffset += val3;
				float num14 = Mathf.Lerp(1f, 0.15f, num13);
				Vector3 val11 = MainPull.smoothedOffset * num14;
				Transform transform = player.transform;
				transform.position += val11;
				MainPull.smoothedOffset -= val11;
				bool flag22 = MainPull.smoothedOffset.magnitude < 0.001f;
				if (flag22)
				{
					MainPull.smoothedOffset = Vector3.zero;
				}
			}
			else
			{
				Transform transform2 = player.transform;
				transform2.position += val3;
			}
		}
	}

	// Token: 0x060000CA RID: 202 RVA: 0x0000FDBC File Offset: 0x0000DFBC
	private static void CachePullOrigin(GTPlayer player, bool useLeftHand)
	{
		Transform val = player.GetControllerTransform(useLeftHand);
		bool flag = !(val == null);
		if (flag)
		{
		}
	}

	// Token: 0x060000CB RID: 203 RVA: 0x0000FDE8 File Offset: 0x0000DFE8
	private static Vector3 AlignToSurface(GTPlayer player, Vector3 delta, Vector3 surfaceNormal)
	{
		bool flag = delta.sqrMagnitude >= 0.0001f && surfaceNormal.sqrMagnitude >= 0.001f;
		Vector3 vector;
		if (flag)
		{
			Vector3 val = MainPull.GetPullOrigin(player);
			float magnitude = delta.magnitude;
			Vector3 val2 = delta / magnitude;
			Vector3 val3 = val + surfaceNormal * 0.045f;
			float num = magnitude;
			RaycastHit val4 = default(RaycastHit);
			bool flag2 = !Physics.SphereCast(val3, 0.115f, val2, ref val4, num, -1, 1);
			if (flag2)
			{
				vector = delta;
			}
			else
			{
				Vector3 val5 = ((val4.normal.sqrMagnitude > 0.001f) ? val4.normal : surfaceNormal);
				Vector3 val6 = Vector3.ProjectOnPlane(delta, val5);
				val6 = ((val6.sqrMagnitude >= 0.0001f) ? (val6.normalized * magnitude) : (Vector3.ProjectOnPlane(Vector3.up, val5) * magnitude));
				bool flag3 = Vector3.Dot(val6, delta) < 0f;
				if (flag3)
				{
					val6 = -val6;
				}
				Vector3 result = val6 + val5 * 0.045f;
				Vector3 normalized = result.normalized;
				float magnitude2 = result.magnitude;
				bool flag4 = magnitude2 > 0.0001f;
				if (flag4)
				{
					float num2 = magnitude2;
					RaycastHit val7 = default(RaycastHit);
					bool flag5 = Physics.SphereCast(val3, 0.115f, normalized, ref val7, num2, -1, 1);
					if (flag5)
					{
						float num3 = Mathf.Max(0f, val7.distance);
						bool flag6 = num3 < 0.01f;
						if (flag6)
						{
							return val5 * 0.045f;
						}
						result = normalized * num3 + val5 * 0.045f;
					}
				}
				vector = result;
			}
		}
		else
		{
			vector = delta;
		}
		return vector;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x0000FFB4 File Offset: 0x0000E1B4
	private static Vector3 GetPullOrigin(GTPlayer player)
	{
		bool flag2 = MainPull.hasCachedOrigin;
		Vector3 vector;
		if (flag2)
		{
			vector = MainPull.cachedOrigin;
		}
		else
		{
			int num = (MainPull.leftHandWasColliding ? 1 : ((!MainPull.rightHandWasColliding) ? 1 : 0));
			bool flag = (byte)num > 0;
			Transform controllerTransform = player.GetControllerTransform(flag);
			bool flag3 = controllerTransform == null;
			if (flag3)
			{
				vector = player.transform.position;
			}
			else
			{
				vector = controllerTransform.position;
			}
		}
		return vector;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00010024 File Offset: 0x0000E224
	private static Vector3 ClampByObstacles(Vector3 delta, float distance)
	{
		Vector3 val = default(Vector3);
		RaycastHit val2 = default(RaycastHit);
		Vector3 val3 = default(Vector3);
		Vector3 val4 = default(Vector3);
		RaycastHit val5 = default(RaycastHit);
		val = delta + Vector3.up * 0.3f;
		val3 = Vector3.zero;
		int num4 = 0;
		int num5 = 8;
		int num6 = 0;
		for (;;)
		{
			bool flag = num6 >= num5;
			if (flag)
			{
				break;
			}
			float num7 = (float)num6 / (float)num5 * 360f * 0.0174532924f;
			val4..ctor(Mathf.Cos(num7) * distance, 0f, Mathf.Sin(num7) * distance);
			bool flag2 = !Physics.Raycast(val + val4, Vector3.down, ref val5, 2f);
			if (!flag2)
			{
				val3 += val5.normal;
				num4++;
			}
			num6++;
		}
		bool flag3 = Physics.Raycast(val, Vector3.down, ref val2, 2f);
		if (flag3)
		{
			val3 += val2.normal;
			num4++;
		}
		bool flag4 = num4 != 0;
		Vector3 vector;
		if (flag4)
		{
			vector = (val3 / (float)num4).normalized;
		}
		else
		{
			vector = Vector3.zero;
		}
		return vector;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00010178 File Offset: 0x0000E378
	private static Vector3 GetAveragedVelocity(Rigidbody body)
	{
		bool flag = MainPull.IsMovementDisabled();
		Vector3 vector;
		if (flag)
		{
			vector = GTPlayer.Instance.AveragedVelocity;
		}
		else
		{
			vector = MainPull.GetVelocity(body);
		}
		return vector;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x000101A8 File Offset: 0x0000E3A8
	private static Vector3 GetVelocity(Rigidbody body)
	{
		bool flag = body == null;
		Vector3 vector;
		if (flag)
		{
			vector = Vector3.zero;
		}
		else
		{
			bool flag2 = !MainPull.velocityReflectionReady;
			if (flag2)
			{
				MainPull.velocityReflectionReady = true;
				Type typeFromHandle = typeof(Rigidbody);
				MainPull.velocityProperty = typeFromHandle.GetProperty("linearVelocity", BindingFlags.Instance | BindingFlags.Public);
				bool flag3 = MainPull.velocityProperty == null;
				if (flag3)
				{
					Type typeFromHandle2 = typeof(Rigidbody);
					MainPull.velocityProperty = typeFromHandle2.GetProperty("velocity", BindingFlags.Instance | BindingFlags.Public);
				}
			}
			bool flag4 = MainPull.velocityProperty != null;
			if (flag4)
			{
				try
				{
					return (Vector3)MainPull.velocityProperty.GetValue(body);
				}
				catch
				{
				}
			}
			vector = Vector3.zero;
		}
		return vector;
	}

	// Token: 0x040001F9 RID: 505
	public static bool leftHandWasColliding;

	// Token: 0x040001FA RID: 506
	public static bool rightHandWasColliding;

	// Token: 0x040001FB RID: 507
	public static float timer = 35f;

	// Token: 0x040001FC RID: 508
	private static PropertyInfo velocityProperty = null;

	// Token: 0x040001FD RID: 509
	private static bool velocityReflectionReady = false;

	// Token: 0x040001FE RID: 510
	private static Rigidbody cachedRigidbody = null;

	// Token: 0x040001FF RID: 511
	private static Vector3 lastVelocity = Vector3.zero;

	// Token: 0x04000200 RID: 512
	private static bool toggleHeld = false;

	// Token: 0x04000201 RID: 513
	private static Vector3 smoothedOffset = Vector3.zero;

	// Token: 0x04000202 RID: 514
	private static Vector3 smoothedDirection = Vector3.zero;

	// Token: 0x04000203 RID: 515
	private static bool thresholdArmed = false;

	// Token: 0x04000204 RID: 516
	private static float thresholdTimer = 0f;

	// Token: 0x04000205 RID: 517
	private static Vector3 cachedOrigin = Vector3.zero;

	// Token: 0x04000206 RID: 518
	private static bool hasCachedOrigin = false;
}
