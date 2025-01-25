using UnityEngine;

namespace SceneHandling
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;

        public static T Instance {
            get { if ((object)_instance == null) {
                    _instance = (T)FindAnyObjectByType(typeof(T));
                    if (!_instance) {
                        var singletonObject = new GameObject(typeof(T).ToString());
                        _instance = singletonObject.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }
    }
}