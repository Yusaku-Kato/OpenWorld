using UnityEngine;
using System.Collections;

public class ObjectPlacerCopied : MonoBehaviour //ここ戻してね
{
    public Vector2 screenClickPos; //Vector3→Vector2
    public Vector2 canvasClickPos;
    private GameObject preNavDestination;
    [SerializeField] private GameObject placedObject;
    [SerializeField] private Vector3 destinationPos;
    private RectTransform canvasRectTransform;

    // 参照したいオブジェクトをインスペクターからアサインする
    public GameObject targetObject;
    // 参照したいスクリプトを入れる変数
    private MiniMapZoom miniMapZoom;
    //canvas
    //public GameObject canvas;

    // 画面の大きさ
    public int width;
    public int height;

    private void Start()
    {
        // 画面の大きさを代入
        width = Screen.width;
        height = Screen.height;

        // 参照したいスクリプトを取得する
        miniMapZoom = targetObject.GetComponent<MiniMapZoom>();
        canvasRectTransform = this.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && miniMapZoom.isPushed == true)
        {
            //前回のゴールを削除
            preNavDestination = GameObject.Find("NavDestination(Clone)");
            GameObject.Destroy(preNavDestination);

            //float horizontalAdjust = 190.0f * (width / 738.0f);
            //float verticalAdjust = 30.0f * (width / 738.0f);
            //float sizeAdjust = 356.0f * (width / 738.0f);

            //ナビゲーションのゴール配置
            screenClickPos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenClickPos, null, out canvasClickPos);
            //960, 540はcanvasの解像度
            destinationPos.x = 1024.0f / 2.0f + canvasClickPos.x * (1024.0f / 500.0f);
            destinationPos.y = 300.0f;
            destinationPos.z = 1024.0f / 2.0f + canvasClickPos.y * (1024.0f / 500.0f);
            //destinationPos.x = (screenClickPos.x - horizontalAdjust) * (1024.0f / sizeAdjust);
            //destinationPos.y = 300.0f;
            //destinationPos.z = (screenClickPos.y - verticalAdjust) * (1024.0f / sizeAdjust);

            //座標がマップ中であればオブジェクト配置
            if (destinationPos.x >= 0 && destinationPos.x <= 1024 && destinationPos.z >= 0 && destinationPos.z <= 1024)
            {
                //Instantiate(placedObject, destinationPos, Quaternion.identity, transform);
                Instantiate(placedObject, destinationPos, Quaternion.identity);
            }
        }
    }
}
