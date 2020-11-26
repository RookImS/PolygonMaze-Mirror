
using UnityEngine;

public class LevelEditorSideColliderBehaviour : MonoBehaviour
{
    public GameObject parentObject;

    private void OnTriggerStay(Collider other)
    {
        ObstacleData.Neighbor neighbor = new ObstacleData.Neighbor();
        if (TagManager.Instance.isBuildingObjectTag(other.gameObject.tag))
        {
            if (!parentObject.Equals(other.gameObject))
            {
                if (GetComponent<Collider>().enabled == true) {
                    GetComponent<Collider>().enabled = false;

                    neighbor.obj
                        = this.gameObject
                        .transform
                        .parent
                        .parent
                        .gameObject;

                    neighbor.collider
                        = GetComponent<Collider>();

                    other
                        .gameObject
                        .GetComponent<ObstacleData>()
                        .NeighborList
                        .Add(neighbor);
                }
            }
        }
    }
}