using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClosePassThrough : MonoBehaviour
{
    private void Start()
    {
        OVRCameraRig ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        var centerCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();

        // 恢复相机设置为天空盒模式
        centerCamera.clearFlags = CameraClearFlags.Skybox;

        // 确保天空盒已启用
        RenderSettings.skybox = RenderSettings.skybox;  // 使用当前的天空盒材质
                                                        // 如果你有特定的天空盒材质，可以直接指定
                                                        // RenderSettings.skybox = yourSkyboxMaterial;
    }
   
}
