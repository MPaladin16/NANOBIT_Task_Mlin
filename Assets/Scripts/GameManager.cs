using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GamePhase { Placement, Movement, Flying }
    public GamePhase currentPhase = GamePhase.Placement;

    [SerializeField] GameObject piecePrefab; 
    [SerializeField] Transform[] boardPositions;

    [SerializeField] UIManager uiManager;
    [SerializeField] AudioManager audioManager;
    private int _currentPlayer = 0; 
    private int[] _piecesToPlace = { 9, 9 };

    private bool _isRemoving = false;
    private GameObject _selectedPiece = null;

    //Additional task - Changing colors/names
    private string[] _playerName = new string[2];
    private UnityEngine.Color[] _playerColor = new UnityEngine.Color[2];

    int p1, p2;

    public event EventHandler<WinEvent> OnGameWon;
    void Start()
    {
        _playerName[0] = PlayerPrefs.GetString("Player1Name", "Player 1");
        _playerName[1] = PlayerPrefs.GetString("Player2Name", "Player 2");
        p1 = PlayerPrefs.GetInt("Player1Color", 0);
        p2 = PlayerPrefs.GetInt("Player2Color", 1);
        _playerColor[0] = playerColorList[p1];
        _playerColor[1] = playerColorList[p2];

        uiManager.UpdateTurnText(_playerName[_currentPlayer], _playerColor[_currentPlayer]);

        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }

        this.OnGameWon += audioManager.OnGameEnd;
        this.OnGameWon += uiManager.OnGameEnd;
    }

    //Try to place a piece only works if position is not occupied and we are in placement phase
    public void TryPlacePiece(int positionIndex)
    {
        BoardPosition position = boardPositions[positionIndex].GetComponent<BoardPosition>();
        if (position.IsOccupied || currentPhase != GamePhase.Placement) return;

        //Create object, add owner and set to a position
        GameObject piece = Instantiate(piecePrefab, boardPositions[positionIndex].position, Quaternion.identity);
        piece.GetComponent<Piece>().SetOwner(_currentPlayer);
        position.SetPiece(piece);

        AudioManager.Instance.PlayPlaceSound();

        _piecesToPlace[_currentPlayer]--;

        //Check for mill in placement phase
        if (CheckForMill(positionIndex, _currentPlayer))
        {
            Debug.Log("Mill formet test.");
            _isRemoving = true;
            uiManager.UpdateInstruction("Remove an opponent's piece");
            HighlightRemovablePieces();
        }
        else
        {
            SwitchTurnOrNextPhase();
        }
    }

    void SwitchTurnOrNextPhase()
    {
        if (currentPhase == GamePhase.Placement)
        {
            if (_piecesToPlace[0] == 0 && _piecesToPlace[1] == 0)
            {
                currentPhase = GamePhase.Movement;
                Debug.Log("Switching to Movement Phase!");
                _currentPlayer = 1 - _currentPlayer;
                Debug.Log("Switched to Player " + _currentPlayer);
            }
            else
            {
                _currentPlayer = 1 - _currentPlayer;
                Debug.Log("Switched to Player " + _currentPlayer);
            }
        }
        else
        {
            _currentPlayer = 1 - _currentPlayer;
            Debug.Log("Switched to Player " + _currentPlayer);
        }

        //Check if flying is enabled for the current player or go back to movement phase
        if (CountPlayerPieces(_currentPlayer) == 3 && currentPhase != GamePhase.Placement)
        {
            currentPhase = GamePhase.Flying;
            Debug.Log("Player " + _currentPlayer + " has the ability to fly pieces");
        }
        else if (currentPhase == GamePhase.Placement) { }
        else { currentPhase = GamePhase.Movement; }

        //Check if game is over
        if (IsGameOver())
        {
            int p = 1 - _currentPlayer;
            string s = (p == 0) ? "white" : "red";
            Debug.Log("Player " + p + " with a color " + s + " won!");
            //uiManager.EndGame(p); // opponent wins
            OnGameWon?.Invoke(this, new WinEvent(p));
            return;
        }

        //Change text
        uiManager.UpdateTurnText(_playerName[_currentPlayer], _playerColor[_currentPlayer]);
        int piecesLeft = _piecesToPlace[_currentPlayer];
        uiManager.UpdateInstruction(currentPhase == GamePhase.Placement ? $"Place a piece, {piecesLeft} left" :
                                    currentPhase == GamePhase.Movement ? "Move a piece" :
                                    "Fly piece to any position");

    }

    private bool CheckForMill(int positionIndex, int player)
    {
        foreach (var mill in mills)
        {
            if (System.Array.IndexOf(mill, positionIndex) >= 0)
            {
                bool isMill = true;
                foreach (var posIndex in mill)
                {
                    var pos = boardPositions[posIndex].GetComponent<BoardPosition>();
                    if (pos.GetPiece() == null || pos.GetPiece().GetComponent<Piece>().GetOwner() != player)
                    {
                        isMill = false;
                        break;
                    }
                }

                if (isMill) return true;
            }
        }
        return false;
    }

    public void HandleMovementClick(int positionIndex)
    {
        var pos = boardPositions[positionIndex].GetComponent<BoardPosition>();

        //Select piece
        if (_selectedPiece == null)
        {
            AudioManager.Instance.PlayPlaceSound();
            if (pos.GetPiece() != null && pos.GetPiece().GetComponent<Piece>().GetOwner() == _currentPlayer)
            {
                _selectedPiece = pos.GetPiece();
                _selectedPiece.GetComponent<Piece>().SetHighlight(true);

                int fromIndex = pos.GetIndex();

                for (int i = 0; i < boardPositions.Length; i++)
                {
                    var bp = boardPositions[i].GetComponent<BoardPosition>();

                    bool isGreen = false;

                    if (currentPhase == GamePhase.Flying)
                    {
                        isGreen = !bp.IsOccupied;
                    }
                    else if (availableMoves[fromIndex].Contains(i) && !bp.IsOccupied)
                    {
                        isGreen = true;
                    }

                    if (isGreen)
                    {
                        bp.SetHighlight(UnityEngine.Color.green, true);
                    }
                }
            }

        }
        //Move piece
        else
        {
            //Get index of piece to see if it can be moved to the new place
            var currentIndex = FindPieceIndex(_selectedPiece);
            bool canMove = false;
            AudioManager.Instance.PlayPlaceSound();
            if (currentPhase == GamePhase.Flying)
            {
                canMove = !pos.IsOccupied;
            }
            else
            {
                canMove = availableMoves[currentIndex].Contains(positionIndex) && !pos.IsOccupied;
            }

            if (canMove)
            {
                boardPositions[currentIndex].GetComponent<BoardPosition>().SetPiece(null);

                //_selectedPiece.transform.position = pos.transform.position;
                //Animirano pomicanje
                StartCoroutine(_selectedPiece.GetComponent<Piece>().MoveTo(pos.transform.position));

                pos.SetPiece(_selectedPiece);

                if (CheckForMill(positionIndex, _currentPlayer))
                {
                    _isRemoving = true;
                    uiManager.UpdateInstruction("Remove an opponent's piece");
                    ClearAllHighlights();
                    HighlightRemovablePieces();
                }
                else
                {
                    SwitchTurnOrNextPhase();
                    ClearAllHighlights();
                }
                _selectedPiece.GetComponent<Piece>().SetHighlight(false);
                _selectedPiece = null;
            }
            else
            {
                AudioManager.Instance.PlayIllegalSound();
                Debug.Log("Invalid move!");
                _selectedPiece.GetComponent<Piece>().SetHighlight(false);
                _selectedPiece = null;
                for (int i = 0; i < boardPositions.Length; i++)
                {
                    boardPositions[i].GetComponent<BoardPosition>().SetHighlight(UnityEngine.Color.clear, false);
                }
            }

        }
    }
    public void FinishRemove()
    {
        _isRemoving = false;
        ClearAllHighlights();
        SwitchTurnOrNextPhase();
    }
    private int FindPieceIndex(GameObject piece)
    {
        for (int i = 0; i < boardPositions.Length; i++)
        {
            if (boardPositions[i].GetComponent<BoardPosition>().GetPiece() == piece)
                return i;
        }
        return -1;
    }

    //is a piece that is getting removed a part of the mill
    public bool IsPartOfMill(int index)
    {
        int owner = boardPositions[index].GetComponent<BoardPosition>().GetPiece().GetComponent<Piece>().GetOwner();

        foreach (var mill in mills)
        {
            if (System.Array.IndexOf(mill, index) >= 0)
            {
                bool fullMill = true;
                foreach (var pos in mill)
                {
                    var bp = boardPositions[pos].GetComponent<BoardPosition>();
                    if (bp.GetPiece() == null || bp.GetPiece().GetComponent<Piece>().GetOwner() != owner)
                    {
                        fullMill = false; 
                        break;
                    }
                }

                if (fullMill) return true; 
            }
        }

        return false;
    }

    //are all the pieces of a player, parts of a mill
    public bool AllOpponentPiecesInMills()
    {
        int opponent = 1 - _currentPlayer;

        foreach (var pos in boardPositions)
        {
            if (pos.GetComponent<BoardPosition>().GetPiece() != null)
            {
                Piece piece = pos.GetComponent<BoardPosition>().GetPiece().GetComponent<Piece>();
                if (piece.GetOwner() == opponent && !IsPartOfMill(pos.GetComponent<BoardPosition>().GetIndex()))
                {
                    return false; // There are some removable piece not in a Mill
                }
            }
        }

        return true; // All pieces are in mills
    }

    //Check if player has only 3 pieces left
    private int CountPlayerPieces(int player)
    {
        int count = 0;
        foreach (var pos in boardPositions)
        {
            if (pos.GetComponent<BoardPosition>().GetPiece() != null && pos.GetComponent<BoardPosition>().GetPiece().GetComponent<Piece>().GetOwner() == player)
            {
                count++;
            }
        }
        return count;
    }
    //Checks if game is over
    private bool IsGameOver()
    {
        if (currentPhase == GamePhase.Placement) 
            return false;
        int pieces = CountPlayerPieces(_currentPlayer);
        if (pieces < 3) 
            return true;

        if (currentPhase == GamePhase.Flying)
            return false; 

        //If still in movement phase, check if any legal move are available
        foreach (var pos in boardPositions)
        {
            int from = 0;
            var bp = pos.GetComponent<BoardPosition>();
            if (bp.GetPiece() != null && bp.GetPiece().GetComponent<Piece>().GetOwner() == _currentPlayer)
            {
                from = bp.GetIndex();
                foreach (int to in availableMoves[from])
                {
                    if (!boardPositions[to].GetComponent<BoardPosition>().IsOccupied)
                        return false; // move exists
                }
            }
        }

        return true; // no legal moves and game is over
    }

    public void ClearAllHighlights()
    {
        foreach (var bp in boardPositions)
        {
            bp.GetComponent<BoardPosition>().SetHighlight(UnityEngine.Color.clear, false);
        }
    }

    public void HighlightRemovablePieces()
    {
        int opponent = 1 - _currentPlayer;
        bool allInMills = AllOpponentPiecesInMills();

        foreach (var bps in boardPositions)
        {
            var bp = bps.GetComponent<BoardPosition>();

            if (bp.GetPiece() != null)
            {
                Piece piece = bp.GetPiece().GetComponent<Piece>();

                if (piece.GetOwner() == opponent)
                {
                    if (!IsPartOfMill(bp.GetIndex()) || allInMills)
                    {
                        bp.SetHighlight(UnityEngine.Color.green, true);
                    }
                }
            }
        }
    }

    //Get & Set
    public int GetCurrentPlayer() => _currentPlayer;
    public GamePhase GetCurrentPhase() => currentPhase;
    public bool IsRemoving() => _isRemoving;
    public bool IsGameOverFlag() => uiManager.IsGameOverFlag();

    public UnityEngine.Color GetPlayer1Color() => playerColorList[p1];
    public UnityEngine.Color GetPlayer2Color() => playerColorList[p2];
    //Hardcoded things as regions
    #region mills
    private int[][] mills = new int[][]
    {
    new int[] {0, 1, 2},
    new int[] {3, 4, 5},
    new int[] {6, 7, 8},
    new int[] {9, 10, 11},
    new int[] {12, 13, 14},
    new int[] {15, 16, 17},
    new int[] {18, 19, 20},
    new int[] {21, 22, 23},

    new int[] {0, 9, 21},
    new int[] {3, 10, 18},
    new int[] {6, 11, 15},
    new int[] {1, 4, 7},
    new int[] {16, 19, 22},
    new int[] {8, 12, 17},
    new int[] {5, 13, 20},
    new int[] {2, 14, 23}
    };
    #endregion

    #region availableMoves
    private Dictionary<int, int[]> availableMoves = new Dictionary<int, int[]>
    {
    { 0, new[] { 1, 9 } },
    { 1, new[] { 0, 2, 4 } },
    { 2, new[] { 1, 14 } },
    { 3, new[] { 4, 10 } },
    { 4, new[] { 1, 3, 5, 7 } },
    { 5, new[] { 4, 13 } },
    { 6, new[] { 7, 11 } },
    { 7, new[] { 4, 6, 8 } },
    { 8, new[] { 7, 12 } },
    { 9, new[] { 0, 10, 21 } },
    {10, new[] { 3, 9, 11, 18 } },
    {11, new[] { 6, 10, 15 } },
    {12, new[] { 8, 13, 17 } },
    {13, new[] { 5, 12, 14, 20 } },
    {14, new[] { 2, 13, 23 } },
    {15, new[] { 11, 16 } },
    {16, new[] { 15, 17, 19 } },
    {17, new[] { 12, 16 } },
    {18, new[] { 10, 19 } },
    {19, new[] { 16, 18, 20, 22 } },
    {20, new[] { 13, 19 } },
    {21, new[] { 9, 22 } },
    {22, new[] { 19, 21, 23 } },
    {23, new[] { 14, 22 } }
};
    #endregion

    #region playerColors
    private UnityEngine.Color[] playerColorList = new UnityEngine.Color[]
    {
        UnityEngine.Color.white,
        UnityEngine.Color.red,
        UnityEngine.Color.yellow,
        UnityEngine.Color.cyan,
        UnityEngine.Color.magenta,

    };
    #endregion
}
