using System;
using System.Reflection;
using GorillaLocomotion;
using UnityEngine;

// Token: 0x0200003B RID: 59
internal class TestPull
{
	// Token: 0x060001BD RID: 445 RVA: 0x0001E2D0 File Offset: 0x0001C4D0
	private static bool IsMovementDisabled()
	{
		bool flag = GTPlayer.Instance != null;
		return flag && GTPlayer.Instance.disableMovement;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x0001E300 File Offset: 0x0001C500
	public static void Update()
	{
		bool flag2 = !Settings.TestPullModEnabled;
		if (!flag2)
		{
			GTPlayer instance = GTPlayer.Instance;
			bool flag3 = instance == null;
			if (!flag3)
			{
				bool testPullModToggleMode = Settings.TestPullModToggleMode;
				if (testPullModToggleMode)
				{
					bool flag = Settings.IsPressed(Settings.TestPullActivation);
					bool flag4 = flag && !TestPull.toggleHeld;
					if (flag4)
					{
						Settings.TestPullModToggleActive = !Settings.TestPullModToggleActive;
					}
					TestPull.toggleHeld = flag;
					bool flag5 = !Settings.TestPullModToggleActive;
					if (flag5)
					{
						return;
					}
				}
				TestPull.timer += Time.deltaTime;
				float num = instance.maxJumpSpeed * Settings.TestPullThreshold;
				bool flag6 = Settings.TestPullModToggleMode || Settings.IsPressed(Settings.TestPullActivation);
				if (flag6)
				{
					bool wasColliding = instance.LeftHand.wasColliding;
					bool wasColliding2 = instance.RightHand.wasColliding;
					bool flag7 = wasColliding || wasColliding2;
					if (flag7)
					{
						TestPull.timer = 0f;
					}
					bool flag8 = (TestPull.leftHandWasColliding && !wasColliding) || (TestPull.rightHandWasColliding && !wasColliding2);
					if (flag8)
					{
						Rigidbody val = TestPull.GetRigidbody(instance);
						bool flag9 = val != null;
						if (flag9)
						{
							bool flag10 = TestPull.GetAveragedVelocity(val).magnitude / instance.scale > num;
							if (flag10)
							{
								TestPull.timer = 0f;
							}
						}
					}
					float testPullTpTime = Settings.TestPullTpTime;
					bool flag11 = TestPull.timer < testPullTpTime;
					if (flag11)
					{
						TestPull.ApplyPull(instance);
					}
					else
					{
						TestPull.smoothedDirection = Vector3.zero;
					}
				}
				else
				{
					TestPull.smoothedDirection = Vector3.zero;
				}
				TestPull.leftHandWasColliding = instance.LeftHand.wasColliding;
				TestPull.rightHandWasColliding = instance.RightHand.wasColliding;
			}
		}
	}

	// Token: 0x060001BF RID: 447 RVA: 0x0001E4C0 File Offset: 0x0001C6C0
	private static Rigidbody GetRigidbody(GTPlayer player)
	{
		bool flag = TestPull.cachedRigidbody == null;
		if (flag)
		{
			TestPull.cachedRigidbody = player.GetComponent<Rigidbody>();
		}
		return TestPull.cachedRigidbody;
	}

	// Token: 0x060001C0 RID: 448 RVA: 0x0001E4F4 File Offset: 0x0001C6F4
	private static void ApplyPull(GTPlayer player)
	{
		Vector3 val = default(Vector3);
		RaycastHit val2 = default(RaycastHit);
		Vector3 val3 = default(Vector3);
		Vector3 val4 = default(Vector3);
		RaycastHit val5 = default(RaycastHit);
		Vector3 val6 = default(Vector3);
		Vector3 val7 = default(Vector3);
		Rigidbody val8 = TestPull.GetRigidbody(player);
		bool flag = val8 == null;
		if (!flag)
		{
			val7 = TestPull.GetAveragedVelocity(val8);
			bool flag2 = val7.magnitude <= 0.5f;
			if (flag2)
			{
			}
			val = val7 * Settings.TestPullStrength * (Settings.TestPullTpTime * 4f);
			val4 = player.transform.position;
			bool flag3 = !Physics.Raycast(val4 + Vector3.up * 0.5f, Vector3.down, ref val5, 2f);
			if (!flag3)
			{
				Vector3 normal = val5.normal;
				val6 = val - Vector3.Dot(val, normal) * normal;
				bool flag4 = normal.y > 0.5f;
				if (flag4)
				{
					val = val6;
				}
			}
			float num5 = Mathf.Sqrt(val.x * val.x + val.z * val.z);
			bool flag5 = val.y < (0f - num5) * 0.5f;
			if (flag5)
			{
				bool flag6 = val.y > num5 * 1.5f;
				if (flag6)
				{
					bool testPullRaycastEnabled = Settings.TestPullRaycastEnabled;
					if (testPullRaycastEnabled)
					{
						val3 = val.normalized;
						float magnitude = val.magnitude;
						float num6 = 0.12f;
						bool flag7 = Physics.SphereCast(val4, num6, val3, ref val2, magnitude);
						if (flag7)
						{
							bool flag8 = val2.normal.y > 0.3f;
							if (flag8)
							{
								val -= Vector3.Dot(val, val2.normal) * val2.normal;
								float num7 = Settings.TestPullSmoothing;
								bool flag9 = num7 > 0.01f;
								if (flag9)
								{
									TestPull.smoothedDirection += val;
									float num8 = Mathf.Lerp(1f, 0.15f, num7);
									Vector3 val9 = TestPull.smoothedDirection * num8;
									Transform transform = player.transform;
									transform.position += val9;
									TestPull.smoothedDirection -= val9;
									bool flag10 = TestPull.smoothedDirection.magnitude >= 0.001f;
									if (!flag10)
									{
										TestPull.smoothedDirection = Vector3.zero;
									}
								}
								else
								{
									Transform transform2 = player.transform;
									transform2.position += val;
								}
							}
							else
							{
								float num9 = Mathf.Max(0f, val2.distance - num6);
								bool flag11 = num9 < 0.01f;
								if (!flag11)
								{
									val = val3 * num9;
									float num7 = Settings.TestPullSmoothing;
									bool flag12 = num7 > 0.01f;
									if (flag12)
									{
										TestPull.smoothedDirection += val;
										float num8 = Mathf.Lerp(1f, 0.15f, num7);
										Vector3 val9 = TestPull.smoothedDirection * num8;
										Transform transform = player.transform;
										transform.position += val9;
										TestPull.smoothedDirection -= val9;
										bool flag13 = TestPull.smoothedDirection.magnitude >= 0.001f;
										if (!flag13)
										{
											TestPull.smoothedDirection = Vector3.zero;
										}
									}
									else
									{
										Transform transform2 = player.transform;
										transform2.position += val;
									}
								}
							}
						}
						else
						{
							float num7 = Settings.TestPullSmoothing;
							bool flag14 = num7 > 0.01f;
							if (flag14)
							{
								TestPull.smoothedDirection += val;
								float num8 = Mathf.Lerp(1f, 0.15f, num7);
								Vector3 val9 = TestPull.smoothedDirection * num8;
								Transform transform = player.transform;
								transform.position += val9;
								TestPull.smoothedDirection -= val9;
								bool flag15 = TestPull.smoothedDirection.magnitude >= 0.001f;
								if (!flag15)
								{
									TestPull.smoothedDirection = Vector3.zero;
								}
							}
							else
							{
								Transform transform2 = player.transform;
								transform2.position += val;
							}
						}
					}
					else
					{
						float num7 = Settings.TestPullSmoothing;
						bool flag16 = num7 > 0.01f;
						if (flag16)
						{
							TestPull.smoothedDirection += val;
							float num8 = Mathf.Lerp(1f, 0.15f, num7);
							Vector3 val9 = TestPull.smoothedDirection * num8;
							Transform transform = player.transform;
							transform.position += val9;
							TestPull.smoothedDirection -= val9;
							bool flag17 = TestPull.smoothedDirection.magnitude >= 0.001f;
							if (!flag17)
							{
								TestPull.smoothedDirection = Vector3.zero;
							}
						}
						else
						{
							Transform transform2 = player.transform;
							transform2.position += val;
						}
					}
				}
				else
				{
					bool testPullRaycastEnabled2 = Settings.TestPullRaycastEnabled;
					if (testPullRaycastEnabled2)
					{
						val3 = val.normalized;
						float magnitude = val.magnitude;
						float num6 = 0.12f;
						bool flag18 = Physics.SphereCast(val4, num6, val3, ref val2, magnitude);
						if (flag18)
						{
							bool flag19 = val2.normal.y > 0.3f;
							if (flag19)
							{
								val -= Vector3.Dot(val, val2.normal) * val2.normal;
								float num7 = Settings.TestPullSmoothing;
								bool flag20 = num7 > 0.01f;
								if (flag20)
								{
									TestPull.smoothedDirection += val;
									float num8 = Mathf.Lerp(1f, 0.15f, num7);
									Vector3 val9 = TestPull.smoothedDirection * num8;
									Transform transform = player.transform;
									transform.position += val9;
									TestPull.smoothedDirection -= val9;
									bool flag21 = TestPull.smoothedDirection.magnitude >= 0.001f;
									if (!flag21)
									{
										TestPull.smoothedDirection = Vector3.zero;
									}
								}
								else
								{
									Transform transform2 = player.transform;
									transform2.position += val;
								}
							}
							else
							{
								float num9 = Mathf.Max(0f, val2.distance - num6);
								bool flag22 = num9 < 0.01f;
								if (!flag22)
								{
									val = val3 * num9;
									float num7 = Settings.TestPullSmoothing;
									bool flag23 = num7 > 0.01f;
									if (flag23)
									{
										TestPull.smoothedDirection += val;
										float num8 = Mathf.Lerp(1f, 0.15f, num7);
										Vector3 val9 = TestPull.smoothedDirection * num8;
										Transform transform = player.transform;
										transform.position += val9;
										TestPull.smoothedDirection -= val9;
										bool flag24 = TestPull.smoothedDirection.magnitude >= 0.001f;
										if (!flag24)
										{
											TestPull.smoothedDirection = Vector3.zero;
										}
									}
									else
									{
										Transform transform2 = player.transform;
										transform2.position += val;
									}
								}
							}
						}
						else
						{
							float num7 = Settings.TestPullSmoothing;
							bool flag25 = num7 > 0.01f;
							if (flag25)
							{
								TestPull.smoothedDirection += val;
								float num8 = Mathf.Lerp(1f, 0.15f, num7);
								Vector3 val9 = TestPull.smoothedDirection * num8;
								Transform transform = player.transform;
								transform.position += val9;
								TestPull.smoothedDirection -= val9;
								bool flag26 = TestPull.smoothedDirection.magnitude >= 0.001f;
								if (!flag26)
								{
									TestPull.smoothedDirection = Vector3.zero;
								}
							}
							else
							{
								Transform transform2 = player.transform;
								transform2.position += val;
							}
						}
					}
					else
					{
						float num7 = Settings.TestPullSmoothing;
						bool flag27 = num7 > 0.01f;
						if (flag27)
						{
							TestPull.smoothedDirection += val;
							float num8 = Mathf.Lerp(1f, 0.15f, num7);
							Vector3 val9 = TestPull.smoothedDirection * num8;
							Transform transform = player.transform;
							transform.position += val9;
							TestPull.smoothedDirection -= val9;
							bool flag28 = TestPull.smoothedDirection.magnitude >= 0.001f;
							if (!flag28)
							{
								TestPull.smoothedDirection = Vector3.zero;
							}
						}
						else
						{
							Transform transform2 = player.transform;
							transform2.position += val;
						}
					}
				}
			}
			else
			{
				bool flag29 = val.y > num5 * 1.5f;
				if (flag29)
				{
					bool testPullRaycastEnabled3 = Settings.TestPullRaycastEnabled;
					if (testPullRaycastEnabled3)
					{
						val3 = val.normalized;
						float magnitude = val.magnitude;
						float num6 = 0.12f;
						bool flag30 = Physics.SphereCast(val4, num6, val3, ref val2, magnitude);
						if (flag30)
						{
							bool flag31 = val2.normal.y > 0.3f;
							if (flag31)
							{
								val -= Vector3.Dot(val, val2.normal) * val2.normal;
								float num7 = Settings.TestPullSmoothing;
								bool flag32 = num7 > 0.01f;
								if (flag32)
								{
									TestPull.smoothedDirection += val;
									float num8 = Mathf.Lerp(1f, 0.15f, num7);
									Vector3 val9 = TestPull.smoothedDirection * num8;
									Transform transform = player.transform;
									transform.position += val9;
									TestPull.smoothedDirection -= val9;
									bool flag33 = TestPull.smoothedDirection.magnitude >= 0.001f;
									if (!flag33)
									{
										TestPull.smoothedDirection = Vector3.zero;
									}
								}
								else
								{
									Transform transform2 = player.transform;
									transform2.position += val;
								}
							}
							else
							{
								float num9 = Mathf.Max(0f, val2.distance - num6);
								bool flag34 = num9 < 0.01f;
								if (!flag34)
								{
									val = val3 * num9;
									float num7 = Settings.TestPullSmoothing;
									bool flag35 = num7 > 0.01f;
									if (flag35)
									{
										TestPull.smoothedDirection += val;
										float num8 = Mathf.Lerp(1f, 0.15f, num7);
										Vector3 val9 = TestPull.smoothedDirection * num8;
										Transform transform = player.transform;
										transform.position += val9;
										TestPull.smoothedDirection -= val9;
										bool flag36 = TestPull.smoothedDirection.magnitude >= 0.001f;
										if (!flag36)
										{
											TestPull.smoothedDirection = Vector3.zero;
										}
									}
									else
									{
										Transform transform2 = player.transform;
										transform2.position += val;
									}
								}
							}
						}
						else
						{
							float num7 = Settings.TestPullSmoothing;
							bool flag37 = num7 > 0.01f;
							if (flag37)
							{
								TestPull.smoothedDirection += val;
								float num8 = Mathf.Lerp(1f, 0.15f, num7);
								Vector3 val9 = TestPull.smoothedDirection * num8;
								Transform transform = player.transform;
								transform.position += val9;
								TestPull.smoothedDirection -= val9;
								bool flag38 = TestPull.smoothedDirection.magnitude >= 0.001f;
								if (!flag38)
								{
									TestPull.smoothedDirection = Vector3.zero;
								}
							}
							else
							{
								Transform transform2 = player.transform;
								transform2.position += val;
							}
						}
					}
					else
					{
						float num7 = Settings.TestPullSmoothing;
						bool flag39 = num7 > 0.01f;
						if (flag39)
						{
							TestPull.smoothedDirection += val;
							float num8 = Mathf.Lerp(1f, 0.15f, num7);
							Vector3 val9 = TestPull.smoothedDirection * num8;
							Transform transform = player.transform;
							transform.position += val9;
							TestPull.smoothedDirection -= val9;
							bool flag40 = TestPull.smoothedDirection.magnitude >= 0.001f;
							if (!flag40)
							{
								TestPull.smoothedDirection = Vector3.zero;
							}
						}
						else
						{
							Transform transform2 = player.transform;
							transform2.position += val;
						}
					}
				}
				else
				{
					bool testPullRaycastEnabled4 = Settings.TestPullRaycastEnabled;
					if (testPullRaycastEnabled4)
					{
						val3 = val.normalized;
						float magnitude = val.magnitude;
						float num6 = 0.12f;
						bool flag41 = Physics.SphereCast(val4, num6, val3, ref val2, magnitude);
						if (flag41)
						{
							bool flag42 = val2.normal.y > 0.3f;
							if (flag42)
							{
								val -= Vector3.Dot(val, val2.normal) * val2.normal;
								float num7 = Settings.TestPullSmoothing;
								bool flag43 = num7 > 0.01f;
								if (flag43)
								{
									TestPull.smoothedDirection += val;
									float num8 = Mathf.Lerp(1f, 0.15f, num7);
									Vector3 val9 = TestPull.smoothedDirection * num8;
									Transform transform = player.transform;
									transform.position += val9;
									TestPull.smoothedDirection -= val9;
									bool flag44 = TestPull.smoothedDirection.magnitude >= 0.001f;
									if (!flag44)
									{
										TestPull.smoothedDirection = Vector3.zero;
									}
								}
								else
								{
									Transform transform2 = player.transform;
									transform2.position += val;
								}
							}
							else
							{
								float num9 = Mathf.Max(0f, val2.distance - num6);
								bool flag45 = num9 < 0.01f;
								if (!flag45)
								{
									val = val3 * num9;
									float num7 = Settings.TestPullSmoothing;
									bool flag46 = num7 > 0.01f;
									if (flag46)
									{
										TestPull.smoothedDirection += val;
										float num8 = Mathf.Lerp(1f, 0.15f, num7);
										Vector3 val9 = TestPull.smoothedDirection * num8;
										Transform transform = player.transform;
										transform.position += val9;
										TestPull.smoothedDirection -= val9;
										bool flag47 = TestPull.smoothedDirection.magnitude >= 0.001f;
										if (!flag47)
										{
											TestPull.smoothedDirection = Vector3.zero;
										}
									}
									else
									{
										Transform transform2 = player.transform;
										transform2.position += val;
									}
								}
							}
						}
						else
						{
							float num7 = Settings.TestPullSmoothing;
							bool flag48 = num7 > 0.01f;
							if (flag48)
							{
								TestPull.smoothedDirection += val;
								float num8 = Mathf.Lerp(1f, 0.15f, num7);
								Vector3 val9 = TestPull.smoothedDirection * num8;
								Transform transform = player.transform;
								transform.position += val9;
								TestPull.smoothedDirection -= val9;
								bool flag49 = TestPull.smoothedDirection.magnitude >= 0.001f;
								if (!flag49)
								{
									TestPull.smoothedDirection = Vector3.zero;
								}
							}
							else
							{
								Transform transform2 = player.transform;
								transform2.position += val;
							}
						}
					}
					else
					{
						float num7 = Settings.TestPullSmoothing;
						bool flag50 = num7 > 0.01f;
						if (flag50)
						{
							TestPull.smoothedDirection += val;
							float num8 = Mathf.Lerp(1f, 0.15f, num7);
							Vector3 val9 = TestPull.smoothedDirection * num8;
							Transform transform = player.transform;
							transform.position += val9;
							TestPull.smoothedDirection -= val9;
							bool flag51 = TestPull.smoothedDirection.magnitude >= 0.001f;
							if (!flag51)
							{
								TestPull.smoothedDirection = Vector3.zero;
							}
						}
						else
						{
							Transform transform2 = player.transform;
							transform2.position += val;
						}
					}
				}
			}
		}
	}

	// Token: 0x060001C1 RID: 449 RVA: 0x0001F5FC File Offset: 0x0001D7FC
	private static Vector3 GetAveragedVelocity(Rigidbody body)
	{
		bool flag = TestPull.IsMovementDisabled();
		Vector3 vector;
		if (flag)
		{
			vector = GTPlayer.Instance.AveragedVelocity;
		}
		else
		{
			vector = TestPull.GetVelocity(body);
		}
		return vector;
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0001F62C File Offset: 0x0001D82C
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
			bool flag2 = !TestPull.velocityReflectionReady;
			if (flag2)
			{
				TestPull.velocityReflectionReady = true;
				Type typeFromHandle = typeof(Rigidbody);
				TestPull.velocityProperty = typeFromHandle.GetProperty("linearVelocity", BindingFlags.Instance | BindingFlags.Public);
				bool flag3 = TestPull.velocityProperty == null;
				if (flag3)
				{
					Type typeFromHandle2 = typeof(Rigidbody);
					TestPull.velocityProperty = typeFromHandle2.GetProperty("velocity", BindingFlags.Instance | BindingFlags.Public);
				}
			}
			bool flag4 = TestPull.velocityProperty != null;
			if (flag4)
			{
				try
				{
					return (Vector3)TestPull.velocityProperty.GetValue(body);
				}
				catch
				{
				}
			}
			vector = Vector3.zero;
		}
		return vector;
	}

	// Token: 0x04000484 RID: 1156
	public static bool leftHandWasColliding;

	// Token: 0x04000485 RID: 1157
	public static bool rightHandWasColliding;

	// Token: 0x04000486 RID: 1158
	public static float timer = 35f;

	// Token: 0x04000487 RID: 1159
	private static PropertyInfo velocityProperty = null;

	// Token: 0x04000488 RID: 1160
	private static bool velocityReflectionReady = false;

	// Token: 0x04000489 RID: 1161
	private static Rigidbody cachedRigidbody = null;

	// Token: 0x0400048A RID: 1162
	private static Vector3 lastVelocity = Vector3.zero;

	// Token: 0x0400048B RID: 1163
	private static bool toggleHeld = false;

	// Token: 0x0400048C RID: 1164
	private static Vector3 smoothedDirection = Vector3.zero;
}
