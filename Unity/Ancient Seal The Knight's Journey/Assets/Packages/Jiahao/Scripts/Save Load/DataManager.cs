using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;


[DefaultExecutionOrder(-100)] //����ִ��

public class DataManager : MonoBehaviour
{
    //�۲���ģʽ
    public static DataManager instance; //����д��

    [Header("�¼�����")]
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;

    private List<IsSaveable> saveableList = new List<IsSaveable>(); //�����б�

    private Data saveData;
    private string jsonFolder;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject); //Ψһд��
        }

        saveData = new Data();

        jsonFolder = Application.persistentDataPath + "/SAVE DATA/";

        ReadSavedData();
    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }

    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }

    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame) //l��
        {
            Load();
        }
    }

    public void RegisterSaveData(IsSaveable saveable) //ͳһ֪ͨ
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(IsSaveable saveable) //ͳһ�޳�
    {
        saveableList.Remove(saveable);
    }

    public void Save()
    {
        foreach (var saveable in saveableList) //�洢������������ݣ�����
        {
            saveable.GetSaveData(saveData);
        }

        var resultPath = jsonFolder + "data.sav";

        var jsonData = JsonConvert.SerializeObject(saveData);

        if (!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }

        File.WriteAllText(resultPath, jsonData);
    }

    public void Load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }

    private void ReadSavedData()
    {
        var resultPath = jsonFolder + "data.sav";

        if (File.Exists(resultPath))
        {
            var stringData = File.ReadAllText(resultPath);

            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);

            saveData = jsonData;
        }
    }
}
