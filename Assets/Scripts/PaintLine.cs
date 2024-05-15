using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintLine : MonoBehaviour
{
    public LineRenderer lr;
    public MeshCollider lineCollider;
    public Mesh mesh;
    public float age, decayDelay = 2.0f, decayTickRate;
    float decayTimer = 0;
    public float linewidth;

    Queue<PolygonCollider2D> colliders;

    public void Awake()
    {
        age = 0;
        linewidth = 0.1f;
        lr = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lr.positionCount = 2;
        colliders = new Queue<PolygonCollider2D>();
        decayTimer = 0.0f;
    }

    private void Update()
    {
        age += Time.deltaTime;
        if (age > decayDelay) 
        {
            decayTimer += Time.deltaTime;
            if(decayTimer > decayTickRate)
            {
                decayTimer = 0.0f;
                if (colliders.Count == 0) { GameObject.Destroy(gameObject); }
                else { GameObject.Destroy(colliders.Dequeue()); removeFirstLineSegment(); }
            }
        }
    }

    // Update is called once per frame
    public void CreateLine(Vector3 startPos, float duration, float tickRate)
    {  
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, startPos);
        decayDelay = duration;
        decayTickRate = tickRate;
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

        colliders.Enqueue(segment);
    }

    public void setLinewidth(float width)
    {
        lr.startWidth = width;
        lr.endWidth = width;
        linewidth = width / 2.0f;
    }

    void removeFirstLineSegment()
    {
        int newVertextCount = lr.positionCount - 1;
        Vector3[] newPositions = new Vector3[newVertextCount];

        for(int i = 0; i < newVertextCount; i++)
        {
            newPositions[i] = lr.GetPosition(i + 1);
        }

        lr.positionCount = newVertextCount;
        lr.SetPositions(newPositions);
    }
}
