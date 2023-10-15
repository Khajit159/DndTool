using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class FileExplorer : MonoBehaviour
{
    public string CurrentPath = "";

    [SerializeField] GameObject Content;
    [SerializeField] GameObject DirectoryPrefab;
    [SerializeField] GameObject FilePrefab;

    private void Start()
    {
        CurrentPath = Path.Combine(Application.persistentDataPath, "CustomMusic");
        UpdateList();
    }

    // Opens folder by name, can go only folder by folder
    public void NextDirectory(string NameOfDirectory)
    {
        CurrentPath = Path.Combine(CurrentPath, NameOfDirectory);
        UpdateList();
    }

    // Basically deletes last part of path
    public void PreviousDirectory()
    {
        if (CurrentPath == Path.Combine(Application.persistentDataPath, "CustomMusic")) {Debug.Log("Can not more go backwards"); return; }
        string[] SplittedPath = CurrentPath.Split("\\");
        CurrentPath = "";
        for (int i = 0; i < SplittedPath.Length - 1; i++)
        {
           CurrentPath = Path.Combine (CurrentPath, SplittedPath[i]);
        }
        UpdateList();
    }


    public void UpdateList()
    {
        if (Content.transform.childCount != 0)
        {
            for (int i = 0; i < Content.transform.childCount; i++)
            {
                Destroy(Content.transform.GetChild(i).gameObject);
            }
        }
        ShowDirs();
        ShowFiles();
    }

    // Shows all directories in currentpath
    private void ShowDirs()
    {
        DirectoryInfo dir = new DirectoryInfo(CurrentPath);
        DirectoryInfo[] dirInfo = dir.GetDirectories();

        foreach (DirectoryInfo di in dirInfo)
        {
            GameObject newdir = Instantiate(DirectoryPrefab, Vector3.zero, Quaternion.identity, Content.transform);
            newdir.name = di.Name;
            newdir.transform.GetChild(0).GetComponent<TMP_Text>().text = di.Name;
        }
    }

    // Shows only files with extension .wav , .mp3 or .ogg at current path
    private void ShowFiles()
    {
        DirectoryInfo dir = new DirectoryInfo(CurrentPath);
        FileInfo[] fileInfo = dir.GetFiles();

        foreach (FileInfo fi in fileInfo)
        {
            if (fi.Extension == ".wav" || fi.Extension == ".mp3" || fi.Extension == ".ogg")
            {
                GameObject newfile = Instantiate(FilePrefab, Vector3.zero, Quaternion.identity, Content.transform);
                string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fi.FullName);
                newfile.name = FileNameWithoutExtension;
                newfile.transform.GetChild(0).transform.GetComponent<TMP_Text>().text = FileNameWithoutExtension;
            }

        }
    }

}
