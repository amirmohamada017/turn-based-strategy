using System;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            var mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            var startGridPosition = new GridPosition(0, 0);
            var path = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition, out var length);
            Debug.Log(length);

            for (var i = 0; i < length-1; i++)
            {
                Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(path[i]),
                    LevelGrid.Instance.GetWorldPosition(path[i + 1]), Color.blue, 10f);
            }
        }
    }
}
