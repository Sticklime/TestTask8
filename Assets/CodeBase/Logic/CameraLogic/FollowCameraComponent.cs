using Unity.Entities;

namespace CodeBase.Logic.CameraLogic
{
    public struct FollowCameraComponent : IComponentData
    {
        public float RotationAngleX;
        public float Distance;
        public float OffsetY;
    }
}