using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class VolumeManager : MonoBehaviour
{
    public Slider VolumeSlider;

    public PostProcessProfile volume;
    public PostProcessLayer layer;
    AutoExposure exposureVol;
    // Start is called before the first frame update
    void Start()
    {
        volume.TryGetSettings(out exposureVol);
        if (PlayerPrefs.GetFloat("volume") != 0)
        {

            VolumeSlider.value = PlayerPrefs.GetFloat("volume");
            exposureVol.keyValue.value = PlayerPrefs.GetFloat("volume");

        }
        AjustarVolumen(VolumeSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AjustarVolumen(float value)
    {

        exposureVol.keyValue.value = value;
        PlayerPrefs.SetFloat("volume", value);

        //Debug.Log(value);
        Debug.Log(PlayerPrefs.GetFloat("volume"));

    }
}
