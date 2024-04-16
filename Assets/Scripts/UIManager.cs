// ---------------------------------------------------------  
// UIManager.cs  
// 
// UI管理スクリプト
//
// 作成日: 2024/4/16
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{

    #region 変数  

    #region 定数

    // 勝ち
    private const int WIN = 1;

    // 負け
    private const int LOSS = -1;

    // 引き分け
    private const int DRAW = 0;

    // 勝ったときのテキスト
    private const string WIN_TEXT = "  あ　な   た  の　勝  ち   ";

    // 負けたときのテキスト
    private const string LOSS_TEXT = "  あ　な   た  の　負  け   ";

    // 引き分けのときのテキスト
    private const string DRAW_TEXT = "        引  き   分   け";

    #endregion

    [SerializeField,Header("GameManagerオブジェクト")]
    private GameObject _gameManager = default;

    [SerializeField,Header("GameUIオブジェクト")]
    private GameObject _gameUI = default;

    [SerializeField,Header("OutComeオブジェクト")]
    private TMP_Text _outComeText = default;

    // GameManager取得用
    private GameManager _gameManagerScript = default;

    // 読み込んでいるシーン名取得用
    private string _sceneNmae = default;

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        // 初期設定
        _sceneNmae = SceneManager.GetActiveScene().name;
        _gameManagerScript = _gameManager.GetComponent<GameManager>();
        _gameUI.SetActive(false);
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    private void Update ()
    {
        // ゲームが終了したとき
        if (_gameManagerScript.IsFin)
        {
            // UI表示
            _gameUI.SetActive(true);
         
            switch(_gameManagerScript.PlayerWin)
            {
                // 負けたとき
                case LOSS:
                    _outComeText.text = LOSS_TEXT;
                    break;

                // 引き分けのとき
                case DRAW:
                    _outComeText.text = DRAW_TEXT;
                    break;

                // 勝ったとき
                case WIN:
                    _outComeText.text = WIN_TEXT;
                    break;
            }
        }
    }

    /// <summary>
    /// 最初からが押されたとき
    /// </summary>
    public void CilckReStart()
    {
        // シーン読み込み
        SceneManager.LoadScene(_sceneNmae);
    }

    /// <summary>
    /// 最初からが押されたとき
    /// </summary>
    public void CilckTitle()
    {
        // シーン読み込み
        SceneManager.LoadScene("Title");
    }


    /// <summary>
    /// 終了が押されたとき
    /// </summary>
    public void CilckEnd()
    {
        // ゲーム終了
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
        #else
            Application.Quit();//ゲームプレイ終了
        #endif
    }

    #endregion

}
