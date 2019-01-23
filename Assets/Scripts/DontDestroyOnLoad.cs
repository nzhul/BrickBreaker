using UnityEngine;

namespace Assets.Scripts
{
    public class DontDestroyOnLoad : MonoBehaviour
    {

        private void Awake()
        {
            transform.SetParent(null); // setting the parent to root in order for dontDesotryOnLoad to work
            DontDestroyOnLoad(gameObject);
        }
    }
}
