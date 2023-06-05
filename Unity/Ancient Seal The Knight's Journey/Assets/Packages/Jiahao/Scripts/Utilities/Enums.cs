namespace Jiahao
{
    //状态Enums切换
    public enum NPCState
    {
        Patrol, Chase, Skill
        
    }

    public enum SceneType { 
    
        Loaction, Menu //场景类型
    
    }

    public enum PersistentType //限制ID变化
    {
        ReadWrite, DoNotPersist
    }
}