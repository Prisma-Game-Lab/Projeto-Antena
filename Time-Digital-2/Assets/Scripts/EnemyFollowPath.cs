using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPath : MonoBehaviour
{
    //Lista do caminho que a AI deve fazer
    public List<AIPath> pathPoints = new List<AIPath>();
    [Tooltip("Segue um caminho baseado nas posições dos filhos desse GameObject, ordem da hierarquia representa a ordem do caminho a ser seguido.")]
    public GameObject path;
    [HideInInspector]
    public int pathIndex;
    [HideInInspector]
    public Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        if (path != null)
        {
            //Preenche as posições da lista do caminho
            fillPathPoints();
        }
        pathIndex = 0;
        initialPos = transform.position;
    }

    //Preenche a lista do caminho com as posições dos gameObjects filhos de path 
    private void fillPathPoints()
    {
        int i = 0;
        int pathPointsCount = pathPoints.Count;
        //Preenche lista do caminho
        for (; i < pathPointsCount && i < path.transform.childCount; i++)
        {
            pathPoints[i].destinationPos = path.transform.GetChild(i).transform.position;
        }
        //Caso tenha mais elementos na lista do caminho do que filhos de path, elimina o excesso
        if (pathPointsCount > path.transform.childCount)
        {
            for (; i < pathPointsCount; pathPointsCount--)
            {
                pathPoints.RemoveAt(i);
            }
        }
    }

    public int getPreviousIndex()
    {
        //Pega o index atual
        int index;
        //Se o tiver algum caminho a fazer
        if (pathPoints.Count > 0)
        {
            //Calcula o index de pathIndex-1
            if (pathIndex == 0)
                index = pathPoints.Count - 1;
            else
                index = pathIndex - 1;
        }
        else index = 0;
        return index;
    }

    public Vector3 getNextPos(Vector3 center)
    {
        Vector3 nextPosition;
        //Se lista do caminho nao for vazia
        if (pathPoints.Count > 0)
        {
            //Pega posição do próximo destino
            nextPosition = pathPoints[pathIndex].destinationPos;
            pathIndex++;

            //Se passou do ultimo elemento do caminho, volta para o primeiro
            if (pathIndex >= pathPoints.Count)
            {
                pathIndex = 0;
            }
        }
        else nextPosition = center;
        return nextPosition;
    }
}
