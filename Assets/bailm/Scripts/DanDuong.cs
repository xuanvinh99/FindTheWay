using UnityEngine;
using UnityEngine.AI;

public class DanDuong : MonoBehaviour
{
    public Transform player; // Nhân vật di chuyển
    public Transform target; // Đối tượng mục tiêu
    public LineRenderer lineRenderer; // LineRenderer để vẽ đường

    private NavMeshPath path; // Đường đi trên NavMesh

    void Start()
    {
        // Khởi tạo NavMeshPath và LineRenderer
        path = new NavMeshPath();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }
    }

    void Update()
    {
        // Tính toán đường đi từ player đến target
        if (NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, path))
        {
            // Cập nhật các vị trí cho LineRenderer
            lineRenderer.positionCount = path.corners.Length;
            lineRenderer.SetPositions(path.corners);
        }
    }
}
