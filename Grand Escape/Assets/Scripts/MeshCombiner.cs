//Author: William �rnquist
using UnityEngine;
public class MeshCombiner : MonoBehaviour
{
    private void Start() => StaticBatchingUtility.Combine(gameObject);
}