using UnityEngine;
using System.Runtime.InteropServices;

public class Link : MonoBehaviour
{
    public void OpenLinkJSPlugin(string url)
    {
		openWindow(url);
    }

    [DllImport("__Internal")]
    private static extern void openWindow(string url);
}