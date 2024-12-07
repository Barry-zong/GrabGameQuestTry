using UnityEngine;
using System.Collections;

public class FireworkSpawner : MonoBehaviour
{
    public float Radius = 0.3f;
    public GameObject fireworkPrefab;
    public bool fire = false;
    private AudioSource fireworkSound;
    private void Start()
    {
        fireworkSound = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if ( fire )
        {
            SpawnFirework();
            fire = false;
        }
    }
    public void SpawnFirework()
    {
        // ��Բ�η�Χ���������һ��λ��
        Vector2 randomCircle = Random.insideUnitCircle * Radius;
        Vector3 spawnPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        fireworkSound.Play();
        Debug.Log("fireworkSoundPlay");
        // ʵ�����̻�
        GameObject firework = Instantiate(fireworkPrefab, spawnPosition, Quaternion.identity);

        // ���������
        StartCoroutine(DestroyAfterDelay(firework));
    }

    private IEnumerator DestroyAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        Destroy(obj);
    }
}