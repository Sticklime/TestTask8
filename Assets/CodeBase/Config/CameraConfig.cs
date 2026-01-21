using UnityEngine;

namespace CodeBase.Config
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Config/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [field: SerializeField] public float RotationAngleX { get; private set; }
        [field: SerializeField] public float Distance { get; private set; }
        [field: SerializeField] public float OffsetY { get; private set; }
    }
}