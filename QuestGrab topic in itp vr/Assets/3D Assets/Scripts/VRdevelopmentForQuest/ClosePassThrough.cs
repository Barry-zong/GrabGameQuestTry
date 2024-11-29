using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ClosePassThrough : MonoBehaviour
{
    private void Start()
    {
        OVRCameraRig ovrCameraRig = GameObject.Find("OVRCameraRig").GetComponent<OVRCameraRig>();
        var centerCamera = ovrCameraRig.centerEyeAnchor.GetComponent<Camera>();

        // �ָ��������Ϊ��պ�ģʽ
        centerCamera.clearFlags = CameraClearFlags.Skybox;

        // ȷ����պ�������
        RenderSettings.skybox = RenderSettings.skybox;  // ʹ�õ�ǰ����պв���
                                                        // ��������ض�����պв��ʣ�����ֱ��ָ��
                                                        // RenderSettings.skybox = yourSkyboxMaterial;
    }
   
}
