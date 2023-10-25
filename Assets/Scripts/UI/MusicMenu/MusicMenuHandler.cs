using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(FileExplorer))]
public class MusicMenuHandler : MonoBehaviour
{
    [HideInInspector]
    public AudioClip currentClip { get; private set; }
    private int currentClipID = 0;

    private bool AudioRandomness = false;
    private bool AudioLoop = false;
    [SerializeField] FileExplorer _fileExplorer;

    [Header("MusicMenu")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] TMP_Text AudioClipName;
    [SerializeField] Slider AudioClipTimeSlider;
    [SerializeField] TMP_Text AudioClipActualTime;
    [SerializeField] TMP_Text AudioClipFullTime;
    [SerializeField] GameObject PlayButton;
    [SerializeField] GameObject PauseButton;
    [SerializeField] Slider Volume_Slider;
    [SerializeField] TMP_Text Volume_SliderValueText;
    [SerializeField] GameObject VolumeOff;
    [SerializeField] GameObject VolumeMid;
    [SerializeField] GameObject VolumeMax;

    [Header("Others")]
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

        Volume_Slider.value = _audioSource.volume * 100;
        ChangeVolumeSliderValue();
    }


    private void Update()
    {
        if (_audioSource.clip != null)
        {
            if (_audioSource.time >= _audioSource.clip.length) { OnAudioEnd(); }
            UpdateSongTimer();
        }
    }

    private void OnAudioEnd()
    {

        if (AudioLoop && _audioSource.clip != null) { _audioSource.time = 0; _audioSource.Play(); }
        if (AudioRandomness && !AudioLoop) { while (GetRandomAudioFromList()) ; }
        if (!AudioLoop) { PlayButton.SetActive(true); PauseButton.SetActive(false); }
    }

    private void UpdateSongTimer()
    {
        if (!_audioSource.isPlaying) { _audioSource.time = (AudioClipTimeSlider.value * currentClip.length); AudioClipActualTime.text = ConvertSecToTime(_audioSource.time); }
        else
        {
            AudioClipActualTime.text = ConvertSecToTime(_audioSource.time);
            AudioClipTimeSlider.value = GetPercentageToSlider(_audioSource.time, currentClip.length);
        }
    }

    private bool GetRandomAudioFromList()
    {
            int RandomNumber = UnityEngine.Random.Range(1, _fileExplorer.LoadedSongs.Count + 1);
            if (!_fileExplorer.AlreadyPlayed.Contains(_fileExplorer.LoadedSongs[RandomNumber]))
            {
                LoadAndPlay(_fileExplorer.LoadedSongs[RandomNumber]);
                if (_fileExplorer.AlreadyPlayed.Count == _fileExplorer.LoadedSongs.Count - 1)
                {
                    _fileExplorer.AlreadyPlayed.Clear();
                    _fileExplorer.AlreadyPlayed.Add(_fileExplorer.LoadedSongs[RandomNumber]);                }
                else
                {
                    _fileExplorer.AlreadyPlayed.Add(_fileExplorer.LoadedSongs[RandomNumber]);
                }
                return false;
            }
            else { return true; }
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

    


    public void ChangeVolumeSliderValue()
    {
        Volume_SliderValueText.text = Volume_Slider.value.ToString();
        _audioSource.volume = Volume_Slider.value / 100;
        if (Volume_Slider.value == 100) { VolumeOff.SetActive(false); VolumeMid.SetActive(false); VolumeMax.SetActive(true); }
        else if (Volume_Slider.value == 0) { VolumeMax.SetActive(false); VolumeMid.SetActive(false); VolumeOff.SetActive(true); }
        else { VolumeMax.SetActive(false); VolumeOff.SetActive(false); VolumeMid.SetActive(true); }
    }

    // User Controls

    public void ChangeAudioLoop(bool status) { AudioLoop = status; }

    public void ChangeAudioRandomness(bool status) { AudioRandomness = status; }

    public void HelpInfo()
    {
        _messageboxInfo.CreateMessageBox(new Vector2(0, -10), 5, new string[] { "You can play only .wav, .mp3 or .ogg files!" });
    }

    public void AddMusic()
    {
        Application.OpenURL(_fileExplorer.CurrentPath);
    }

    public void OpenDirectory(string NameOfDirectory)
    {
        _fileExplorer.NextDirectory(NameOfDirectory);
    }

    public void PlayBackwards()
    {
        if (currentClipID == 1) { Debug.Log("Can not go backwards more"); return; }
        LoadAndPlay(_fileExplorer.LoadedSongs[currentClipID -1]);
    }

    public void PlayForwards()
    {
        if (currentClipID == _fileExplorer.LoadedSongs.Count) { Debug.Log("Can not go forwards more"); Debug.Log(currentClipID + " " + _fileExplorer.LoadedSongs.Count); return; }
        LoadAndPlay(_fileExplorer.LoadedSongs[currentClipID +1]);
    }

    public void PlayAgainOnEnd()
    {
        if (_audioSource.clip != null && _audioSource.time == 0 && !_audioSource.isPlaying) { _audioSource.time = 0; _audioSource.Play(); }
    }

    // Playing Music
    public async void LoadAndPlay(string NameOfSong)
    {
        Destroy(_audioSource.clip);
        string path = Path.Combine(_fileExplorer.CurrentPath, NameOfSong);
        string pathExtension = Path.GetExtension(path);

        if (File.Exists(path + ".mp3")) { currentClip = await LoadClip(path + ".mp3", AudioType.MPEG); }
        else if (File.Exists(path + ".wav")) { currentClip = await LoadClip(path + ".wav", AudioType.WAV); }
        else if (File.Exists(path + ".ogg")) { currentClip = await LoadClip(path + ".ogg", AudioType.OGGVORBIS); }
        if (currentClip == null) { Debug.LogError("Could not find song."); return;  }
        _audioSource.clip = currentClip;
        _audioSource.time = 0;
        AudioClipName.text = NameOfSong;
        AudioClipFullTime.text = ConvertSecToTime(_audioSource.clip.length);
        _audioSource.Play();
        PlayButton.SetActive(false);
        PauseButton.SetActive(true);
        currentClipID = _fileExplorer.LoadedSongs.FirstOrDefault(x => x.Value == NameOfSong).Key;
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
