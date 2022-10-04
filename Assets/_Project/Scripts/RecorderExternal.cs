using UnityEngine;

public class RecorderExternal : MonoBehaviour
{
	private string pathVideo = "";
	private string pathVideoCallback = "";
	public void RecordVideo()
	{
		NativeCamera.Permission permission = NativeCamera.RecordVideo((path) =>
		{
			Debug.Log("Video path: " + path);
			if (path != null)
			{
				// Play the recorded video
				pathVideo = "file://" + path;
				pathVideoCallback = path;
				Handheld.PlayFullScreenMovie(pathVideo);
			}
		});

		Debug.Log("Permission result: " + permission);
	}

	public void ShowPreviewVideo()
    {
		Debug.Log(pathVideo);
		if (pathVideo.Equals(""))
			return;

		Handheld.PlayFullScreenMovie(pathVideo);
	}

	public void ShareVideo()
    {
		Debug.Log(pathVideo);
		if (pathVideo.Equals(""))
			return;

		//var payload = new SharePayload();
		//payload.AddText("My text here");
		//payload.AddMedia(pathVideo);
		//payload.Commit();

		Debug.Log(Application.dataPath);
		Debug.Log(Application.persistentDataPath);
		Debug.Log(Application.temporaryCachePath);

		new NativeShare().AddFile(pathVideoCallback)
		.SetSubject("Subject goes here").SetText("Hello world!").SetUrl("https://github.com/yasirkula/UnityNativeShare")
		.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
		.Share();
	}
}