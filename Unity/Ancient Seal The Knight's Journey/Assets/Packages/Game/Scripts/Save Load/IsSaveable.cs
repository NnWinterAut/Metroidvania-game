using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IsSaveable
{
    DataDefination GetDataID(); //�������ID
    void RegisterSaveData() => DataManager.instance.RegisterSaveData(this); //������

    void UnRegisterSaveData() => DataManager.instance.UnRegisterSaveData(this); //ע����, �޳�

    void GetSaveData(Data data); //������
    void LoadData(Data data); //��ȡ��
}

