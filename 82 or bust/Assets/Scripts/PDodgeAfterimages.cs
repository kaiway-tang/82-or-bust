using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDodgeAfterimages : MonoBehaviour
{
    [SerializeField] GameObject image;
    [SerializeField] int amount = 3;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayEffect(Player.self.baseObj.transform, amount));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayEffect(Transform target, int amount)
    {
        transform.right = Vector3.right;
        List<GameObject> images = new List<GameObject>();
        for (int i = 0; i < amount; ++i)
        {
            GameObject obj = Instantiate(image, transform.position, Quaternion.identity);
            obj.GetComponent<Rigidbody2D>().AddForce(transform.right * 10f, ForceMode2D.Impulse);
            images.Add(obj);
            transform.Rotate(Vector3.forward, 360f / amount);
        }
        yield return new WaitForSeconds(0.2f);
        bool vanish = false;
        foreach (GameObject afterimage in images)
        {
            afterimage.GetComponent<Rigidbody2D>().drag = 10f;
        }
        yield return new WaitForSeconds(0.2f);
        while (!vanish)
        {
            foreach (GameObject afterimage in images)
            {
                afterimage.transform.position = Vector3.MoveTowards(afterimage.transform.position, transform.position, 30f * Time.fixedDeltaTime);
                // Debug.Log(target.position);
            }
            int ctr = 0;
            foreach (GameObject afterimage in images)
            {
                if (Vector3.SqrMagnitude(afterimage.transform.position - target.position) < 1f)
                {
                    ctr++;
                }
            }
            if (ctr >= amount - 1)
            {
                vanish = true;
            }
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < 20; ++i)
        {
            foreach (GameObject afterimage in images)
            {
                afterimage.transform.position = Vector3.MoveTowards(afterimage.transform.position, transform.position, 30f * Time.fixedDeltaTime);
                afterimage.transform.localScale -= Vector3.one * .05f;
            }
            yield return new WaitForFixedUpdate();
        }
        while (images.Count > 0)
        {
            var obj = images[0];
            images.RemoveAt(0);
            Destroy(obj);
        }
        Destroy(gameObject);
    }
}
