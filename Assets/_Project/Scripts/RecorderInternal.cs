using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class RecorderInternal : MonoBehaviour
{
    private bool camAvailable;
    private WebCamTexture backCam;
    private WebCamTexture frontCam;
    private int status = 0;
    //private Texture defaultBackground;

    public RawImage background;
    public AndroidUtils androidUtils;
    //public AspectRatioFitter fit;

    private void Start()
    {
        androidUtils.onStopRecord += OnStopRecord;
    }

    public void StartCam()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif

        //defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {
            Debug.Log("No Camera Found");
            camAvailable = false;
            return;
        }

        for (int i = 0; i < devices.Length; i++)
        {

            if (devices[i].isFrontFacing)
            {
                frontCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
            else
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
        {
            Debug.Log("no backCamera");
            return;
        }

        if (frontCam == null)
        {
            Debug.Log("no backCamera");
            return;
        }
    }

    public void StartWebcamBack()
    {
        StartCam();

        if (!camAvailable)
        {
            frontCam.Stop();
        }

        backCam.Play();
        background.texture = backCam;

        camAvailable = true;
        status = 0;
    }

    public void StartWebcamFront()
    {
        StartCam();

        if (!camAvailable)
        {
            backCam.Stop();
        }

        frontCam.Play();
        background.texture = frontCam;

        camAvailable = true;
        status = 1;
    }

    private void Update()
    {
        if (!camAvailable)
            return;

        //float ratio = (float)backCam.width / (float)backCam.height;
        //fit.aspectRatio = ratio;

        if (status == 0)
        {
            float scaleY = backCam.videoVerticallyMirrored ? -1f : 1f;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            int orient = -backCam.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0f, 0f, orient);
        }
        else
        {
            float scaleY = frontCam.videoVerticallyMirrored ? -1f : 1f;
            background.rectTransform.localScale = new Vector3(1f, scaleY, 1f);

            int orient = -frontCam.videoRotationAngle;
            background.rectTransform.localEulerAngles = new Vector3(0f, 0f, orient);
        }
    }

    public void StartRecordVideo()
    {
        androidUtils.StartRecording();
    }

    public void StopRecordVideo()
    {
        androidUtils.StopRecording();
    }

    private void OnStopRecord()
    {
        AndroidUtils.ShowToast("OnStopRecord");
        Debug.Log("OnStopRecord");
        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.dataPath);
        Debug.Log(Application.temporaryCachePath);
        Debug.Log(Application.streamingAssetsPath);
        Debug.Log(Application.consoleLogPath);
    }
}
