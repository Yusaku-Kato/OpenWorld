using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary>
/// UI SamplesのMenu 3Dを適当に操作するAgent
/// </summary>
[CreateAssetMenu(fileName = "SampleUIControlAgent", menuName = "Anjin/UGUIControl Sample/SampleUIControlAgent")]
public class SampleUIControlAgent : UIControlAgent
{
    GameObject obj = GameObject.Find("DogPolyart");

public override async UniTask Run(CancellationToken token)
    {
        // 最初のアニメーションを待ってみる
        await UniTask.Delay(TimeSpan.FromSeconds(2d), cancellationToken: token);

        // Settingsを押す
        //ClickObject("Settings");
        //await UniTask.Delay(TimeSpan.FromSeconds(1.5d), cancellationToken: token);

        while (true)
        {
            RandomInput();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5d), cancellationToken: token);
        }
    }
}
