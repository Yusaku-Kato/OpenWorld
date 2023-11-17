using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{
    [UnitySetUp]
    public IEnumerator UnitySetup()
    {
        Debug.Log("UnitySetup");
        //GameObject dogPolyart = (GameObject)Resources.Load("Assets/DogKnight/Prefab/DogPolyart.prefab");
        yield break;
    }

    // A Test behaves as an ordinary method
    [Test]
    public void NewTestScriptSimplePasses()
    {
        //GameObject obj = GameObject.Find("DogPolyart");
        //Assert.IsNotNull(obj);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses()
    {
        GameObject dogPolyart = (GameObject)Resources.Load("DogPolyart");
        Debug.Log(dogPolyart);
        Assert.IsNotNull(dogPolyart);

        //Test Example//
        //int value = 1 + 1;
        //Assert.AreEqual(value, 2);

        yield return null;
    }
}
