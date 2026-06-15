using UnityEngine;

namespace MegaRaketa.Gameplay.VisualObjects
{
    public class SceneVisualObjects : MonoBehaviour, ISceneVisualObjects
    {
        [SerializeField] private Transform _asteroids;

        public Transform AsteroidsContainer => _asteroids;
    }
}
