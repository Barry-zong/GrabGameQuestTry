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
        // 在圆形范围内随机生成一个位置
        Vector2 randomCircle = Random.insideUnitCircle * Radius;
        Vector3 spawnPosition = transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);
        fireworkSound.Play();
        Debug.Log("fireworkSoundPlay");
        // 实例化烟花
        GameObject firework = Instantiate(fireworkPrefab, spawnPosition, Quaternion.identity);

        // 两秒后销毁
        StartCoroutine(DestroyAfterDelay(firework));
    }

    private IEnumerator DestroyAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        Destroy(obj);
    }
}