using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

public class NavMeshTriangle : MonoBehaviour
{
    public NavMeshAgent agent; // NavMeshAgentを格納する変数
    NavMeshTriangulation triangulation;
    int polyNum;
    bool[] coverage; //カバレッジ計算用

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation(); // NavMesh上の三角形情報を取得
        polyNum = triangulation.indices.Length / 3;
        Debug.Log("NavMesh has " + polyNum + " polygons.");

        coverage = new bool[triangulation.indices.Length / 3];
    }

    void Update()
    {
        if (agent.isOnNavMesh) // エージェントがNavMesh上にいる場合
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(agent.transform.position, out hit, 0.1f, NavMesh.AllAreas)) // エージェントの位置から最も近いNavMesh上の位置を取得
            {
                //NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation(); // NavMesh上の三角形情報を取得
                for (int i = 0; i < triangulation.indices.Length; i += 3) // 三角形情報を順番に調べる
                {
                    int index0 = triangulation.indices[i];
                    int index1 = triangulation.indices[i + 1];
                    int index2 = triangulation.indices[i + 2];

                    Vector3 vertex0 = triangulation.vertices[index0];
                    Vector3 vertex1 = triangulation.vertices[index1];
                    Vector3 vertex2 = triangulation.vertices[index2];

                    if (IsPointInTriangle(hit.position, vertex0, vertex1, vertex2)) // エージェントが三角形内にいる場合
                    {
                        Debug.Log("Agent is in triangle " + i / 3); // デバッグログにエージェントがいる三角形の番号を表示

                        //カバレッジ(仮)
                        coverage[i / 3] = true;
                        Debug.Log("カバー率 " + (double)coverage.Count(b => b) / coverage.Length * 100 + " %"); // デバッグログにエージェントがいる三角形の番号を表示
                        break;
                    }
                }
            }
        }
    }

    bool IsPointInTriangle(Vector3 p, Vector3 p0, Vector3 p1, Vector3 p2) // 点pが三角形p0-p1-p2内にあるかどうかを判定する関数
    {
        float area = 0.5f * (-p1.z * p2.x + p0.z * (-p1.x + p2.x) + p0.x * (p1.z - p2.z) + p1.x * p2.z);
        float s = 1 / (2 * area) * (p0.z * p2.x - p0.x * p2.z + (p2.z - p0.z) * p.x + (p0.x - p2.x) * p.z);
        float t = 1 / (2 * area) * (p0.x * p1.z - p0.z * p1.x + (p0.z - p1.z) * p.x + (p1.x - p0.x) * p.z);
        return s > 0 && t > 0 && 1 - s - t > 0;
    }
}
