using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Visualizer : MonoBehaviour {

	//	 Reference to the audio source
	AudioSource aSource;

	//	 No of samples and bands
	private int nSamples = 512;
	private int nBands = 8;

	//Array reference to the arrays
	// sampleArray[] has reference to the 512 samples,	bands[] refer to the 8 bands,	bandBuffer[] has refernce to the buffered bands
	private float[] sampleArrayLeft, sampleArrayRight;
	private float[] bands, bandBuffer, bufferDecrease;

	//Array references to audioBand (Normalised bands)
	private float[] bandHighest;
	public float profilevalue;

//	[HideInInspector]
	public float[] audioBand,audioBandBuffer;

	// Final amplitudes
//	[HideInInspector]
	public float amplitude, amplitudeBuffer;
	private float amplitudeHighest;

	//Microphone Input
	public bool useMicrophone;
	public AudioClip aClip;
	string microphoneName;


	void Start () {
		#region initialising arrays
		sampleArrayLeft = new float[nSamples];
		sampleArrayRight = new float[nSamples];

		bands = new float[nBands];
		bandBuffer = new float[nBands];

		bufferDecrease = new float[nBands];

		bandHighest = new float[nBands];
		audioBand = new float[nBands];
		audioBandBuffer = new float[nBands];
		#endregion

		aSource = GetComponent<AudioSource> ();
		AudioProfile (profilevalue);


		if(useMicrophone)
		{
			if(Microphone.devices.Length > 0)
			{
//				Debug.Log (Microphone.devices[1].ToString());
				microphoneName = Microphone.devices [1].ToString ();
				aSource.clip = Microphone.Start (microphoneName, true, 10, AudioSettings.outputSampleRate);
			}
			else
			{
				useMicrophone = false;
			}
		}

		else if(!useMicrophone)
		{
			aSource.clip = aClip;
		}

		aSource.Play ();

	}

	void AudioProfile(float highest){
		for (int i = 0; i < nBands; i++) {
			bandHighest [i] = highest;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		aSource.GetSpectrumData (sampleArrayLeft, 0, FFTWindow.BlackmanHarris);
		aSource.GetSpectrumData (sampleArrayRight, 1, FFTWindow.BlackmanHarris);
		FrequencyDivider ();
		CreateBandBuffer ();
		CreateAudioBands ();
	}

	void CreateAudioBands()
	{
		// Getting the highest band value
		for (int i = 0; i < nBands; i++) {
			if(bands[i] > bandHighest[i])
			{
				bandHighest [i] = bands [i];
			}
		}

		for (int i = 0; i < nBands; i++) {
			audioBand [i] = Mathf.Abs(bands [i] / bandHighest [i]);
			audioBandBuffer [i] = Mathf.Abs(bandBuffer [i] / bandHighest [i]);
		}
	}
		
	void CreateBandBuffer ()
	{
		for (int i = 0; i < nBands; i++) {

			if(bands[i] > bandBuffer [i])
			{
				bandBuffer [i] = bands [i];
				bufferDecrease [i] = 0.005f;
			} 
			else if(bands[i] < bandBuffer [i])
			{
				bandBuffer [i] -= bufferDecrease [i];
				bufferDecrease [i] *= 1.05f; // 20% increase
			}
		}
	}

	void FrequencyDivider(){
	//	for i=0, Samples=2     Interval= (0,86)            	Total Samples=2
	//	for i=1, Samples=4     Interval= (87,259)         Total Samples=6
	//	for i=2, Samples=8     Interval= (260,604)        Total Samples=14
	//	for i=3, Samples=16    Interval= (605,1293)       Total Samples=30
	//	for i=4, Samples=32    Interval= (1294,2670)      Total Samples=62
	//	for i=5, Samples=64    Interval= (2671,5423)      Total Samples=126
	//	for i=6, Samples=128   Interval= (5424,10928)     Total Samples=254
	//	for i=7, Samples=256   Interval= (10929,21937)    Total Samples=510 + 2 (Add extra)

		int sampleCount = 0;
		int currentCount = 0;
		float average = 0;
		for (int i = 0; i < nBands; i++) {
			sampleCount = (int)Mathf.Pow(2, i + 1);

			for (int j = 0; j < sampleCount; j++) {
				average += sampleArrayLeft [currentCount] + sampleArrayRight[currentCount];
				currentCount++;
			}
			average = average / currentCount;
			bands [i] = average;
		}
	}

	void GetAmplitude()
	{
		float currentAmplitude = 0;
		float currentAmpBuffer = 0;
		for (int i = 0; i < nBands; i++) {
			currentAmplitude += audioBand [i];
			currentAmpBuffer += audioBandBuffer [i];
		}

		if(currentAmplitude > amplitudeHighest)
		{
			amplitudeHighest = currentAmplitude;
		}

		amplitude = (currentAmplitude / amplitudeHighest);
		amplitudeBuffer = (currentAmpBuffer / amplitudeHighest);
	}
}