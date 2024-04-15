// ---------------------------------------------------------  
// PlayerInput.cs  
//   
// プレイヤー入力１
//
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{

    #region 変数

    [SerializeField]
    private GameObject _putObj;

    private StoneControl _myScript = default;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        _myScript = _putObj.GetComponent<StoneControl>();
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "OK")
            {
                _myScript.PutStone(Mathf.FloorToInt(hit.collider.transform.localPosition.x), Mathf.FloorToInt(hit.collider.transform.localPosition.z));
            }
        }
    }

    #endregion

}
