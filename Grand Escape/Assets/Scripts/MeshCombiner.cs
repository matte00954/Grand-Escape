using UnityEngine;
public class MeshCombiner : MonoBehaviour
{
    private void Start() => StaticBatchingUtility.Combine(gameObject);
}