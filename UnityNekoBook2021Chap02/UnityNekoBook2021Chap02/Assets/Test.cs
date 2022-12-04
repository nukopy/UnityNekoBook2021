using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random=System.Random;

interface IPlayer
{
    public void Attack();
    public void Damage(int damage);
}

public class Player: IPlayer
{
    private int _hp = 100;
    private int _power = 50;

    public void Attack()
    {
        Debug.Log($"{this._power} のダメージを与えた");
    }

    public void Damage(int damage)
    {
        this._hp -= damage;
        Debug.Log($"{damage} のダメージを受けた");
    }
}


public class Test : MonoBehaviour
{
    private readonly Random _rnd = new Random();
    
    // Start is called before the first frame update
    private void Start()
    {
        // PrintHello();
        // PrintHeight();
        // TestArray1();
        // TestArray2();
        TestVector2Sono1();
        TestVector2Sono2();
    }

    // Update is called once per frame
    private void Update()
    {
        // PrintRandomNumber();
    }
    
    // --------------------
    // methods
    // --------------------

    private void TestVector2Sono2()
    {
        Vector2 startPos = new Vector2(2.0f, 1.0f);
        Vector2 endPos = new Vector2(8.0f, 5.0f);
        Vector2 dir = endPos - startPos;
        Debug.Log($"dir: {dir}");

        float len = dir.magnitude;
        Debug.Log($"dir.magnitude: {len}");
    }
    
    private void TestVector2Sono1()
    {
        Vector2 playerPos = new Vector2(3.0f, 4.0f);
        Debug.Log($"Before: {playerPos}");
        playerPos.x += 8.0f;
        playerPos.y += 5.0f;
        Debug.Log($"After: {playerPos}");
    }
    
    private void TestPlayer()
    {
        // section 2-8-2
        Player myPlayer = new Player();
        myPlayer.Attack();
        myPlayer.Damage(30);
    }
    
    private void TestArray1()
    {
        int length = 5;
        int[] points = new int[length]; // 0 で初期化される

        // print before array initialization
        for (int i = 0; i < length; i++)
        {
            int p = points[i];
            Debug.Log($"points[{i}]: {p}");
        }

        // update array items
        points[0] = 2;
        points[1] = 10;
        points[2] = 5;
        points[3] = 15;
        points[4] = 3;
        
        // print after updating array
        for (int i = 0; i < length; i++)
        {
            int p = points[i];
            Debug.Log($"points[{i}]: {p}");
        }
    }
    
    private void TestArray2()
    {
        int[] points = {83, 99, 52, 93, 15, 100, 77};

        // print before array initialization
        for (int i = 0; i < points.Length; i++)
        {
            int p = points[i];
            string result = CheckPoint(p);
            
            Debug.Log($"points[{i}]: {p}\nresult: {result}");
        }
    }

    private string CheckPoint(int point)
    {
        if (point < 30)
        {
            return "赤点";
        }
        else if (point < 60)
        {
            return "不合格";
        }
        else if (point < 80)
        {
            return "まずまず";
        }
        else if (point < 100)
        {
            return "優秀";
        }
        else
        {
            return "Excellent!!";
        }
    }

    private void PrintHeight()
    {
        float height1 = 160.5f;
        float height2;
        height2 = height1;
        height2 += 3;
        
        Debug.Log($"height1: {height1}\nheight2: {height2}");
    }
    // test 2
    private void PrintHello()
    {
        StartCoroutine(PrintHelloRoutine());
    }

    private IEnumerator PrintHelloRoutine()
    {
        Debug.Log("Hello, World!");
        
        for (int i = 0; i < 10; i++)
        {
            Debug.Log($"coroutine num: {i}");
            yield return new WaitForSeconds(1);
        }
        
        Debug.Log("End of the World!");
    }
    
    // test 1 
    private void PrintRandomNumber()
    {
        int num = _rnd.Next(10);

        if (num % 7 == 0)
        {
            Debug.Log($"num: {num}");            
        }
    }
}
