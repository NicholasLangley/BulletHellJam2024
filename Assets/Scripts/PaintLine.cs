using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintLine : MonoBehaviour
{
    public LineRenderer lr;
    public MeshCollider lineCollider;
    public Mesh mesh;
    public float age, maxAge = 10.0f;
    public float linewidth;

    public void Awake()
    {
        age = 0;
        linewidth = 0.1f;
        lr = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lr.positionCount = 2;
    }

    private void Update()
    {
        age += Time.deltaTime;
        if (age > maxAge) { GameObject.Destroy(gameObject); }
    }

    // Update is called once per frame
    public void CreateLine(Vector3 startPos, float duration)
    {
        
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, startPos);
        maxAge = duration;
    }

    public void addPoint(Vector3 pos)
    {
        lr.positionCount += 1;
        lr.SetPosition(lr.positionCount - 1, pos);
        generateSegmentCollider(lr.GetPosition(lr.positionCount - 2), pos);
    }

    void generateSegmentCollider(Vector3 s, Vector3 e)
    {
        PolygonCollider2D segment = gameObject.AddComponent(typeof(PolygonCollider2D)) as PolygonCollider2D;
        segment.isTrigger = true;

        Vector2 start = new Vector2(s.x, s.y);
        Vector2 end = new Vector2(e.x, e.y);

        Vector2 line = end - start;
        Vector2 normal = new Vector2(line.y, -line.x);
        Vector2 antinormal = new Vector2(-line.y, line.x);
        normal.Normalize();
        antinormal.Normalize();

        Vector2[] segmentPoints = new Vector2[4];
        segmentPoints[0] = start + normal * linewidth;
        segmentPoints[1] = end + normal * linewidth;
        segmentPoints[2] = end + antinormal * linewidth;
        segmentPoints[3] = start + antinormal * linewidth;
        segment.points = segmentPoints;
    }

    public void setLinewidth(float width)
    {
        lr.startWidth = width;
        lr.endWidth = width;
        linewidth = width / 2.0f;
    }
}
