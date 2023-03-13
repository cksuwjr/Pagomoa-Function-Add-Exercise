using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    Vector3 MousePosition;
    public LayerMask whatIsPlatform;
   
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D overCollider2d = Physics2D.OverlapCircle(MousePosition, 0.01f, whatIsPlatform);
            if(overCollider2d != null)
            {
                overCollider2d.transform.GetComponent<Ground>().Digged(MousePosition);
            }
        }

        
    }
}
