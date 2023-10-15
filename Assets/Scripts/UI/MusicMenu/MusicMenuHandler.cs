using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MusicMenuHandler : MonoBehaviour
{
    [HideInInspector]
    public AudioClip currentClip;

    [SerializeField] FileExplorer _fileExplorer;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] TMP_Text AudioClipName;
    [SerializeField] Slider AudioClipTimeSlider;
    [SerializeField] TMP_Text AudioClipActualTime;
    [SerializeField] TMP_Text AudioClipFullTime;
    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject PauseButton;
    [SerializeField] Slider Slider;
    [SerializeField] TMP_Text SliderValueText;
    [SerializeField] MessageBoxInfo _messageboxInfo;
    [SerializeField] string[] ReadmeMessage;

    // Checks if CustomMusic directory exists and also if Readme exists. Also auto changes volume slider to volume audio source
    private void Start()
    {

        string tempPath = Path.Combine(Application.persistentDataPath, "CustomMusic");
        if (!Directory.Exists(tempPath)) { Directory.CreateDirectory(tempPath); }

        if (!File.Exists(Path.Combine(tempPath, "Readme.txt"))) 
        {
            StringBuilder _sb = new StringBuilder();
            _sb.Clear();
            foreach (string s in ReadmeMessage)
            {
                _sb.AppendLine(s);
            }
            File.WriteAllText(Path.Combine(tempPath, "Readme.txt"), _sb.ToString());
        }

        Slider.value = _audioSource.volume * 100;
        ChangeVolumeSliderValue();
    }


    private void Update()
    {
        UpdateSongTimer();
    }

    private void UpdateSongTimer()
    {
        if (currentClip != null)
        {
            if (!_audioSource.isPlaying) { _audioSource.time = (AudioClipTimeSlider.value * currentClip.length); AudioClipActualTime.text = ConvertSecToTime(_audioSource.time); }
            else
            {
                AudioClipActualTime.text = ConvertSecToTime(_audioSource.time);
                AudioClipTimeSlider.value = GetPercentageToSlider(_audioSource.time, currentClip.length);
            }
        }
    }
    

    private float GetPercentageToSlider(float ActualSeconds, float MaxSeconds)
    {
        float calculatedFloat = (ActualSeconds / MaxSeconds);
        return calculatedFloat;
    }

    // Converter from secs to minutes format 4:32 4mins and 32secs
    private string ConvertSecToTime(float curTime)
    {
        int minutes = Mathf.FloorToInt(curTime / 60f);
        int seconds = Mathf.FloorToInt(curTime - minutes * 60);
        string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        return (formattedTime);

    }

    // Opens file explorer at current path
    public void AddMusic()
    {
        Application.OpenURL(_fileExplorer.CurrentPath);
    }
    
    public void HelpInfo()
    {
        _messageboxInfo.CreateMessageBox(new Vector2(0, -10), 5, new string[] { "You can play only .wav, .mp3 or .ogg files!" });
    }

    public void ChangeVolumeSliderValue()
    {
        SliderValueText.text = Slider.value.ToString();
        _audioSource.volume = Slider.value / 100;
    }

    public void OpenDirectory(string NameOfDirectory)
    {
        _fileExplorer.NextDirectory(NameOfDirectory);
    }

    public async void LoadAndPlay(string NameOfSong)
    {
        string path = Path.Combine(_fileExplorer.CurrentPath, NameOfSong);
        string pathExtension = Path.GetExtension(path);
        Debug.Log(path);
        Debug.Log(pathExtension);

        if (File.Exists(path + ".mp3")) { currentClip = await LoadClip(path + ".mp3", AudioType.MPEG); Debug.Log("mp3 song"); }
        else if (File.Exists(path + ".wav")) { currentClip = await LoadClip(path + ".wav", AudioType.WAV); Debug.Log("wav song"); }
        else if (File.Exists(path + ".ogg")) { currentClip = await LoadClip(path + ".ogg", AudioType.OGGVORBIS); Debug.Log("ogg song"); }
        if (currentClip == null) { Debug.LogError("Could not find song."); return;  }
        _audioSource.clip = currentClip;
        _audioSource.time = 0;
        AudioClipName.text = NameOfSong;
        AudioClipFullTime.text = ConvertSecToTime(_audioSource.clip.length);
        _audioSource.Play();
        PlayButton.SetActive(false);
        PauseButton.SetActive(true);
    }

    async Task<AudioClip> LoadClip(string path, AudioType Atype)
    {
        AudioClip clip = null;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, Atype))
        {
            uwr.SendWebRequest();

            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!uwr.isDone) await Task.Delay(5);

                if (uwr.isNetworkError || uwr.isHttpError) Debug.Log($"{uwr.error}");
                else
                {
                    clip = DownloadHandlerAudioClip.GetContent(uwr);

                }
            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace}");
            }
        }

        return clip;
    }
}
