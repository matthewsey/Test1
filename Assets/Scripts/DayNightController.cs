using UnityEngine;
using System.Collections;
using System;

public class DayNightController : MonoBehaviour {

	protected const int DAY_IN_SECONDS = 24 * 60 * 60;

	public Light sun;
	public float secondsInFullDay = 120f;
	[Range(0,1)]
	public float currentTimeOfDay = 0;
	[HideInInspector]
	public float timeMultiplier = 1f;

	float sunInitialIntensity;

	float CurrentTimeInDay () {

		int hour_seconds = DateTime.Now.Hour * 60 * 60;
		int minute_seconds = DateTime.Now.Minute * 60;
		int seconds = DateTime.Now.Second + minute_seconds + hour_seconds;

		return (float) (seconds / DAY_IN_SECONDS);

	}

	void Start() {
		sun = GetComponent <Light> ();

		sunInitialIntensity = sun.intensity;
	}

	void Update() {
		UpdateSun();

		currentTimeOfDay += (Time.deltaTime / secondsInFullDay) * timeMultiplier;

		if (currentTimeOfDay >= 1) {
			currentTimeOfDay = 0;
		}
	}

	void UpdateSun() {
		sun.transform.localRotation = Quaternion.Euler((CurrentTimeInDay() * 360f) - 90, 170, 0);

		float intensityMultiplier = 1;
		if (currentTimeOfDay <= 0.23f || currentTimeOfDay >= 0.75f) {
			intensityMultiplier = 0;
		}
		else if (currentTimeOfDay <= 0.25f) {
			intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - 0.23f) * (1 / 0.02f));
		}
		else if (currentTimeOfDay >= 0.73f) {
			intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.73f) * (1 / 0.02f)));
		}

		sun.intensity = sunInitialIntensity * intensityMultiplier;
	}
}
