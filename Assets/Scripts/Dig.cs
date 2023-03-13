using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dig : MonoBehaviour
{
    private PlayerController controller;
    private LineRenderer line;
    [SerializeField] private Material line_material;
    [SerializeField] private LayerMask diggableGround;

    [SerializeField] private List<Transform> digPoint;

    bool dig = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dig = true;
        }
    }
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        if (!line)
        {
            line = gameObject.AddComponent<LineRenderer>();
            line.positionCount = 4;
            line.loop = true;
            line.enabled = false;
            line.startWidth = 0.1f;
            line.endWidth = 0.1f;
            line.material = line_material;
        }
        if (!line_material)
        {
            Debug.LogWarning("Dig.cs 스크립트의 line_material이 할당되지 않았습니다! 핑크로 출력됩니다!");
        }
    }
    private void FixedUpdate()
    {
        DetectDig();
    }
    public void DetectDig()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            Vector3 DigPoint = transform.position + Vector3.down;
            Collider2D overColliderd = Physics2D.OverlapCircle(DigPoint, 0.01f, diggableGround);
            if (overColliderd != null)
            {
                Ground brick = overColliderd.GetComponent<Ground>();
                Vector3Int cellPosition = Vector3Int.zero;
                if (brick)
                    cellPosition = brick.tilemap.WorldToCell(DigPoint);


                if (!brick.tilemap.HasTile(cellPosition)) { line.enabled = false; dig = false; return; } // 없으면 리턴


                Vector3 Adjust_Z = Vector3.zero;
                //Adjust_Z = Vector3.back; //활성화시 블럭을 감싸게 할 수 있다.
                line.SetPosition(0, cellPosition + new Vector3(0, 0) + Adjust_Z);
                line.SetPosition(1, cellPosition + new Vector3(1, 0) + Adjust_Z);
                line.SetPosition(2, cellPosition + new Vector3(1, 1) + Adjust_Z);
                line.SetPosition(3, cellPosition + new Vector3(0, 1) + Adjust_Z);
                line.enabled = true;
            }
            else
            {
                line.enabled = false;
            }
            if (dig)
            {
                if (overColliderd != null)
                {
                    overColliderd.transform.GetComponent<Ground>().Digged(DigPoint);
                }
            }
        }
        else {
            for (int i = 0; i < digPoint.Count; i++)
            {
                Collider2D overColliderd = Physics2D.OverlapCircle(digPoint[i].position, 0.01f, diggableGround);
                if (overColliderd != null)
                {
                    Ground brick = overColliderd.GetComponent<Ground>();
                    Vector3Int cellPosition = Vector3Int.zero;
                    if (brick)
                        cellPosition = brick.tilemap.WorldToCell(digPoint[i].position);


                    if (!brick.tilemap.HasTile(cellPosition)) { line.enabled = false; dig = false; return; } // 없으면 리턴

                    if (i == 0)
                    {
                        line.SetPosition(0, cellPosition + new Vector3(0, 0));
                        line.SetPosition(1, cellPosition + new Vector3(1, 0));
                    }
                    if (i == digPoint.Count - 1)
                    {
                        line.SetPosition(2, cellPosition + new Vector3(1, i));
                        line.SetPosition(3, cellPosition + new Vector3(0, i));
                    }
                    line.enabled = true;

                }
                else
                {
                    line.enabled = false;
                }

                if (dig)
                {
                    if (overColliderd != null)
                    {
                        overColliderd.transform.GetComponent<Ground>().Digged(digPoint[i].position);
                    }
                }
            }
        }

    
        dig = false;
    }
}
