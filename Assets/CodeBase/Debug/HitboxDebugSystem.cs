using CodeBase.Logic.Enemy;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public partial class HitboxDebugSystem : SystemBase
{
    protected override void OnUpdate()
    {
        foreach (var (localTransform, hitbox) in
                 SystemAPI.Query<RefRO<LocalTransform>, RefRO<ColliderComponent>>())
        {
            Vector3 center = localTransform.ValueRO.Position;
            float radius = hitbox.ValueRO.Radius;

            DrawDebugSphere(center, radius, Color.red);
        }
    }

    private void DrawDebugSphere(Vector3 center, float radius, Color color)
    {
        int segments = 12;
        for (int i = 0; i < segments; i++)
        {
            float theta = (i / (float)segments) * Mathf.PI * 2f;
            float nextTheta = ((i + 1) / (float)segments) * Mathf.PI * 2f;

            Vector3 startPoint = center + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0f) * radius;
            Vector3 endPoint = center + new Vector3(Mathf.Cos(nextTheta), Mathf.Sin(nextTheta), 0f) * radius;
            Debug.DrawLine(startPoint, endPoint, color);

            startPoint = center + new Vector3(Mathf.Cos(theta), 0f, Mathf.Sin(theta)) * radius;
            endPoint = center + new Vector3(Mathf.Cos(nextTheta), 0f, Mathf.Sin(nextTheta)) * radius;
            Debug.DrawLine(startPoint, endPoint, color);

            startPoint = center + new Vector3(0f, Mathf.Cos(theta), Mathf.Sin(theta)) * radius;
            endPoint = center + new Vector3(0f, Mathf.Cos(nextTheta), Mathf.Sin(nextTheta)) * radius;
            Debug.DrawLine(startPoint, endPoint, color);
        }
    }
}