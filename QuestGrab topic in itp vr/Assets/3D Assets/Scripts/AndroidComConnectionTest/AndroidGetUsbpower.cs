using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
#if UNITY_ANDROID && !UNITY_EDITOR

using UnityEngine.Android;

#endif

public class AndroidGetUsbpower : MonoBehaviour
{
    private const string USB_PERMISSION = "com.android.example.USB_PERMISSION";

    private bool _permissionGranted = false;

    async void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
            {
                Permission.RequestUserPermission(Permission.Microphone);
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.UsbDevice))
            {
                PendingIntent pi = PendingIntent.GetBroadcast(this, 0, new Intent(USB_PERMISSION), 0);
                IntentFilter filter = new IntentFilter(USB_PERMISSION);
                Android.App.Application.Context.RegisterReceiver(new UsbPermissionReceiver(), filter);

                UsbManager usbManager = (UsbManager)Android.App.Application.Context.GetSystemService(Context.UsbService);
                Dictionary<string, UsbDevice> usbDevices = usbManager.DeviceList;
                UsbDevice device = usbDevices.Values.FirstOrDefault(d => d.VendorId == 0x1234 && d.ProductId == 0x5678);
                if (device != null)
                {
                    usbManager.RequestPermission(device, pi);
                    await Task.Delay(1000);
                    if (_permissionGranted)
                    {
                        GetSerialPortInfo();
                    }
                }
            }
            else
            {
                GetSerialPortInfo();
            }
        }
    }

    private void GetSerialPortInfo()
    {
        string[] ports = SerialPort.GetPortNames();
        foreach (string port in ports)
        {
            Debug.Log("Port: " + port);
        }
    }

    private class UsbPermissionReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == USB_PERMISSION)
            {
                bool granted = intent.GetBooleanExtra(UsbManager.ExtraPermissionGranted, false);
                _permissionGranted = granted;
            }
        }
    }
}
*/