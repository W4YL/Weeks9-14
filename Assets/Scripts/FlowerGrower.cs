using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlowerGrower : MonoBehaviour
{
    public Transform farmLand;
    public Transform flowerStem;
    public Transform flowerHead;

    public AnimationCurve growCurve;

    Coroutine placeFarmCoroutine;
    Coroutine growStemCoroutine;
    Coroutine growFlowerCoroutine;
    Coroutine doTheGrowingCoroutine;

    public GameObject flowerFarm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        farmLand.localScale = Vector2.zero;
        flowerStem.localScale = Vector2.zero;
        flowerHead.localScale = Vector2.zero;

        StartFarmGrowing();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFarmGrowing()
    {
        if (doTheGrowingCoroutine != null)
        {
            StopCoroutine(doTheGrowingCoroutine);
        }
        if (placeFarmCoroutine != null)
        {
            StopCoroutine(placeFarmCoroutine);
        }
        if (growStemCoroutine != null)
        {
            StopCoroutine(growStemCoroutine);
        }
        if (growFlowerCoroutine != null)
        {
            StopCoroutine(growFlowerCoroutine);
        }

        doTheGrowingCoroutine = StartCoroutine(DoTheGrowing());
    }

    IEnumerator DoTheGrowing()
    {
        yield return placeFarmCoroutine = StartCoroutine(PlaceFarm());
        growStemCoroutine = StartCoroutine(GrowStem());
        yield return growFlowerCoroutine = StartCoroutine(GrowFlower());
    }

    IEnumerator PlaceFarm()
    {
        farmLand.localScale = Vector2.zero;
        flowerStem.localScale = Vector2.zero;
        flowerHead.localScale = Vector2.zero;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            farmLand.localScale = Vector2.one * growCurve.Evaluate(t);

            yield return null;
        }
    }

    IEnumerator GrowStem()
    {
        flowerStem.localScale = Vector2.zero;
        flowerHead.localScale = Vector2.zero;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            flowerStem.localScale = Vector2.one * growCurve.Evaluate(t);

            yield return null;
        }

    }

    IEnumerator GrowFlower()
    {
        flowerHead.localScale = Vector2.zero;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            flowerHead.localScale = Vector2.one * growCurve.Evaluate(t)/2;

            yield return null;
        }
    }
}
