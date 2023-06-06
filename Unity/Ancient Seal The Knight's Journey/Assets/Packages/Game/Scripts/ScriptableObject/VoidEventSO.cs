using UnityEngine;
using UnityEngine.Events;

namespace Jiahao
{
    [CreateAssetMenu(menuName = "Event/VoidEventSO")]
    public class VoidEventSO : ScriptableObject
    {
        public UnityAction OnEventRaised;

        public void RaiseEvent()
        {
            OnEventRaised?.Invoke();
        }
    }
}
