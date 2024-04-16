// ---------------------------------------------------------  
// GameManager.cs  
//   
// ゲームマネージャー
//
// 作成日: 2024/4/16
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    #region 変数  

    #region 定数

    // プレイヤーのターンのときのテキスト
    private const string PLAYER_TRUN = "あなたの番";

    // 相手のターンのときのテキスト
    private const string ENEMY_TRUN = "相手の番";

    #endregion

    [SerializeField, Header("SurfacePlateオブジェクト")]
    private GameObject _surfacePlate = default;

    [SerializeField, Header("PutStoneオブジェクト")]
    private GameObject _putStone = default;

    [SerializeField]
    private TMP_Text _trunText = default;

    // SeachPutPossible取得用
    private SeachPutPossible _seachPutPossible = default;

    // StoneControl取得用
    private StoneControl _stoneControl = default;

    // プレイヤーが勝ったかどうか
    private int _playerWin = default;

    // ターンのフラグ
    private bool _isTurn = false;

    // プレイヤーのターンかどうか
    private bool _isPlayerTurn = false;

    // 終了フラグ
    private bool _isFin = false;

    // プレイヤーが先手かどうか
    private bool _isPlayerBlack = false;

    #endregion

    #region プロパティ  

    public int PlayerWin
    {
        get => _playerWin;
    }

    public bool IsFin
    {
        get => _isFin;
    }

    public bool IsTurn
    {
        get => _isTurn;
    }

    public bool IsPlayerTurn
    {
        get => _isPlayerTurn;
    }

    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary> 
    private void Awake()
    {
        // 初期設定
        _seachPutPossible = _surfacePlate.GetComponent<SeachPutPossible>();
        _stoneControl = _putStone.GetComponent<StoneControl>();

        // ターン決め
        int preceding = Random.Range(0, 2);
        if (preceding == 0)
        {
            _isPlayerTurn = false;
            _isPlayerBlack = false;
            _trunText.text = ENEMY_TRUN;
        }
        else if (preceding == 1)
        {
            _isPlayerTurn = true;
            _isPlayerBlack = true;
            _trunText.text = PLAYER_TRUN;
        }
    }

    /// <summary>
    /// ターン交代処理
    /// </summary>
    public void TurnChange()
    {
        // マスが埋まっていたらスキップ
        if (_isFin)
        {
            return;
        }

        // ターン交代 
        _isTurn = !_isTurn;
        _isPlayerTurn = !_isPlayerTurn;

        if(_isPlayerTurn)
        {
            _trunText.text = PLAYER_TRUN;
        }
        else
        {
            _trunText.text = ENEMY_TRUN;
        }


        // 盤面情報更新
        _seachPutPossible.Seach();

        // 白のターンのとき
        if (!IsTurn)
        {
            _stoneControl.WhitePutStone();
        }
        // 黒のターンのとき
        else
        {
            _stoneControl.BlackPutStone();
        }
    }

    public void GameEnd(int outCome)
    {
        // 終了フラグを立てる
        _isFin = true;;
        
        // プレイヤーか相手どっちが勝ったかの判定
        if(_isPlayerBlack && outCome == 1)
        {
            _playerWin = 1;
        }
        else if(!_isPlayerBlack && outCome == 1)
        {
            _playerWin = -1;
        }
        else if (_isPlayerBlack && outCome == -1)
        {
            _playerWin = -1;
        }
        else if (!_isPlayerBlack && outCome == -1)
        {
            _playerWin = 1;
        }
        else
        {
            _playerWin = 0;
        }

        // 置けるマスを見えなくする
        foreach (Transform chlid in _putStone.transform)
        {
            chlid.gameObject.SetActive(false);
        }
    }

    #endregion

}
