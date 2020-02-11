using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FpsLock : MonoBehaviour
{
	[FormerlySerializedAs("targetFPS")] [SerializeField]
	int targetFps = 60;
	[SerializeField]
	bool vSyncEnabled = false;

	void Awake()
	{
		QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;
		Application.targetFrameRate = targetFps;
	}
}
