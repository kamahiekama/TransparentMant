using System.Collections;
using System.Collections.Generic;
using TofAr.V0.Color;
using TofAr.V0.Hand;
using TofAr.V0.Tof;
using UnityEngine;

public class CameraStarter : MonoBehaviour
{
    bool init = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            {
                CameraConfigurationProperty selected = null;

                CameraConfigurationsProperty configs = TofArTofManager.Instance.GetProperty<CameraConfigurationsProperty>();
                foreach (CameraConfigurationProperty config in configs.configurations)
                {
                    if (selected == null)
                    {
                        selected = config;
                    }

                    if (config.width == 640 && config.height == 480)
                    {
                        selected = config;
                    }
                }

                TofArTofManager.Instance.StartStream(selected, false);
            }
            {
                ResolutionProperty selected = null;

                AvailableResolutionsProperty configs = TofArColorManager.Instance.GetProperty<AvailableResolutionsProperty>();
                foreach (ResolutionProperty config in configs.resolutions)
                {
                    if (selected == null)
                    {
                        selected = config;
                    }

                    if (config.width == 1440 && config.height == 1080)
                    {
                        selected = config;
                    }
                }

                TofArColorManager.Instance.StartStream(selected, true);
            }
            TofArHandManager.Instance.StartStream();

            init = true;
        }
    }
}
