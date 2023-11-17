using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DeNA.Anjin.Agents;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

/// <summary>
/// UIの操作を行う機能を持つAgent
/// </summary>
public abstract class UIControlAgent : AbstractAgent
{
    /// <summary>
    /// UIの対象となるカメラ
    /// </summary>
    protected Camera _camera;

    protected void ClickObject(string objectName)
    {
        var target = GameObject.Find(objectName);
        Assert.IsNotNull(target, $"対象が見つかりませんでした: {objectName}");

        SimulateClick(target.transform);
    }

    protected void ClickObject(string objectName, string parentName)
    {
        var parent = GameObject.Find(parentName);
        var target = parent.GetComponentsInChildren<Transform>()
            .FirstOrDefault(x => x.name == objectName);
        Assert.IsNotNull(target, $"対象が見つかりませんでした: {parentName}-{objectName}");

        SimulateClick(target.transform);
    }

    protected async UniTask DragObject(string objectName, Vector2 startOffset, Vector2 endOffset, double duration)
    {
        var target = GameObject.Find(objectName);
        Assert.IsNotNull(target, $"対象が見つかりませんでした: {objectName}");

        await SimulateDrag(target, startOffset, endOffset, duration);
    }

    protected void SimulateClick(Transform target)
    {
        // クリック位置の定義
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = RectTransformUtility.WorldToScreenPoint(_camera, target.position)
        };

        // EventSystems経由でクリック
        ExecuteEvents.Execute(target.gameObject, eventData, ExecuteEvents.pointerClickHandler);
    }

    protected async UniTask SimulateDrag(GameObject target, Vector3 startOffset, Vector3 endOffset, double duration)
    {
        // ドラッグの始点と終点の定義
        var targetPosition = target.transform.position;
        var startPositionWorld = targetPosition + startOffset;
        var endPositionWorld = targetPosition + endOffset;
        var startPosition = RectTransformUtility.WorldToScreenPoint(_camera, startPositionWorld);
        var endPosition = RectTransformUtility.WorldToScreenPoint(_camera, endPositionWorld);

        var eventData = new PointerEventData(EventSystem.current)
        {
            position = startPosition,
            pressPosition = startPosition,
        };

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        eventData.pointerCurrentRaycast = raycastResults.FirstOrDefault(x => x.gameObject == target.gameObject);

        // 始点からEventSystems経由でドラッグ開始
        ExecuteEvents.Execute(target.gameObject, eventData, ExecuteEvents.pointerDownHandler);
        ExecuteEvents.Execute(target.gameObject, eventData, ExecuteEvents.beginDragHandler);

        // durationにそって移動
        var elapsedTime = 0d;
        while (elapsedTime < duration)
        {
            // EventSystems経由でドラッグしたまま位置を調整
            eventData.position = Vector2.Lerp(startPosition, endPosition, (float)(elapsedTime / duration));
            ExecuteEvents.Execute(target.gameObject, eventData, ExecuteEvents.dragHandler);
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }

        // 終点でEventSystems経由でドラッグ終了
        ExecuteEvents.Execute(target.gameObject, eventData, ExecuteEvents.endDragHandler);
        ExecuteEvents.Execute(target.gameObject, eventData, ExecuteEvents.pointerUpHandler);
    }

    //protected void RandomInput()
    //{
    //    var input = new[] { "w", "a", "s", "d" };
    //    var randomIndex = UnityEngine.Random.Range(0, input.Length);
    //    var randomKey = input[randomIndex];
    //    Input.GetKeyDown(randomKey);
    //}

    protected void RandomInput()
    {
        var input = new[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
        var randomIndex = UnityEngine.Random.Range(0, input.Length);
        var randomKey = input[randomIndex];
        Input.GetKeyDown(randomKey);
        Debug.Log($"ランダムに選択されたキー {randomKey} が押されました。");
    }


}