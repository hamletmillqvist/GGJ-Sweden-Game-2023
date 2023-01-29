using UnityEngine;

namespace Gnagg {
    public class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            MountHorse();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        private void MountHorse()
        {
            FindObjectOfType<HorseController>()?.Mount(transform);
        }
    }
}
