// ---------------------------------------------------------  
// PlayerInput.cs  
//   
// プレイヤー入力
//
// 作成日: 2024/4/2
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{

    #region 変数

    [SerializeField]
    private GameObject _putObj;

    // stoneControl取得用
    private StoneControl _stoneControl = default;

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        // 初期設定
        _stoneControl = _putObj.GetComponent<StoneControl>();
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    private void Update()
    {
        // マウスでクリックしたとき
        if (Input.GetMouseButtonDown(0))
        {
            // Rayを飛ばす
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // クリックしたオブジェクトを取得
            RaycastHit hit = new RaycastHit();

            // クリックしたオブジェクトがあって石が置けるとき
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "OK")
            {
                // マスの座標を渡す
                _stoneControl.PutStone(Mathf.FloorToInt(hit.collider.transform.localPosition.x), Mathf.FloorToInt(hit.collider.transform.localPosition.z));
            }
        }
    }

    #endregion

}
