/*
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;

public class Android_com_connect : MonoBehaviour
{
    private AndroidJavaObject usbManager;
    private AndroidJavaObject currentActivity;

    // Serial port settings
    public string deviceName = "/dev/ttyUSB0";
    public int baudRate = 9600;
    public int parity = 0; // 0 = None, 1 = Odd, 2 = Even
    public int dataBits = 8;
    public StopBits stopBits = StopBits.One;

    private bool isReading = false;
    private Thread readThread;
    private FileStream serialStream;
    private byte[] readBuffer = new byte[4096];
    private StringBuilder comData = new StringBuilder();

    // Start is called before the first frame update
    void Start()
    {
        // Get the Android USB manager and current activity
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        usbManager = currentActivity.Call<AndroidJavaObject>("getSystemService", "usb");

        // Request permission to access the USB device
        requestPermission(deviceName);
    }

    // Request permission to access the USB device
    void requestPermission(string deviceName)
    {
        // Get the USB device
        AndroidJavaObject device = findDevice(deviceName);
        if (device != null)
        {
            // Check if we have permission to access the device
            if (usbManager.Call<bool>("hasPermission", device))
            {
                // We have permission, open the serial port
                openSerialPort(device);
            }
            else
            {
                // We don't have permission, request it
                PendingIntent pendingIntent = PendingIntent.getBroadcast(currentActivity, 0, new Intent("com.android.example.USB_PERMISSION"), 0);
                usbManager.Call("requestPermission", device, pendingIntent);
            }
        }
    }

    // Find the USB device with the specified device name
    AndroidJavaObject findDevice(string deviceName)
    {
        // Get a list of all USB devices
        AndroidJavaObject deviceList = usbManager.Call<AndroidJavaObject>("getDeviceList");

        // Iterate through the device list and find the device with the specified name
        AndroidJavaObject device = null;
        foreach (string key in deviceList.Call<bool>("isEmpty") ? new string[0] : deviceList.Call<string[]>("keySet"))
        {
            AndroidJavaObject usbDevice = deviceList.Call<AndroidJavaObject>("get", key);
            if (usbDevice.Call<string>("getDeviceName").Equals(deviceName))
            {
                device = usbDevice;
                break;
            }
        }

        return device;
    }

    // Open the serial port and start reading data
    void openSerialPort(AndroidJavaObject device)
    {
        try
        {
            // Open the serial port
            serialStream = new FileStream(device.Call<AndroidJavaObject>("getFileDescriptor"), FileAccess.ReadWrite);

            // Configure the serial port
            serialStream.SetLength(0);
            serialStream.ReadTimeout = 500;
            serialStream.WriteTimeout = 500;
            serialStream.BaudRate = baudRate;
            serialStream.Parity = parity;
            serialStream.DataBits = dataBits;
            serialStream.StopBits = stopBits;

            // Start reading data from the serial port
            isReading = true;
            readThread = new Thread(new ThreadStart(readSerialPort));
            readThread.Start();

            // Debug log
            Debug.Log("AndroidConnectSuccesed");
        }
        catch (IOException e)
        {
            Debug.LogError("Error opening serial");
        }
    }
}
*/