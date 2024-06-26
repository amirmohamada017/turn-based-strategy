using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;

    public void SetUp(Transform originalRootBone)
    {
        MatchAllChileTransforms(ragdollRootBone, originalRootBone);
        
        var randomDir = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        ApplyExplosionToRagdoll(ragdollRootBone, 200f, transform.position + randomDir, 10f);
    }

    private void MatchAllChileTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            var cloneChild = clone.Find(child.name);

            if (cloneChild == null) continue;
            
            cloneChild.position = child.position;
            cloneChild.rotation = child.rotation;
                
            MatchAllChileTransforms(child, cloneChild);
        }
    }

    private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition,
        float explosionRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out var childRigidbody))
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            
            ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
        }
    }
}
