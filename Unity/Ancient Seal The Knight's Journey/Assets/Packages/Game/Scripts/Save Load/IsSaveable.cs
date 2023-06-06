using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiahao
{
    public interface IsSaveable
    {
        DataDefination GetDataID(); //声明获得ID
        void RegisterSaveData() => DataManager.instance.RegisterSaveData(this); //管理类

        void UnRegisterSaveData() => DataManager.instance.UnRegisterSaveData(this); //注销类, 剔除

        void GetSaveData(Data data); //传递类
        void LoadData(Data data); //读取类
    }
}

