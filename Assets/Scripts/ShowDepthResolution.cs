using System;
using System.Collections;
using System.Collections.Generic;
using TofAr.V0.Tof;
using UnityEngine;
using UnityEngine.UI;

public class ShowDepthResolution : MonoBehaviour
{
    /// <summary>
	/// 初期化されたかどうかのフラグ
	/// </summary>
    private bool init = false;

    private Text debugText;

    // Start is called before the first frame update
    void Start()
    {
        debugText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            CameraConfigurationProperty config = TofArTofManager.Instance.GetProperty<CameraConfigurationProperty>();
            debugText.text = config.width + " x " + config.height;

            init = true;
        }
    }
}
