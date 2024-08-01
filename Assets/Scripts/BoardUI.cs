using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VRCase
{
    public class BoardUI : MonoBehaviour
    {
        [SerializeField] private Board _board;

        [SerializeField] private Image[] _cellImages;
        [SerializeField] private Sprite _trueSprite;
        [SerializeField] private Sprite _falseSprite;
        [SerializeField] private Color _nullColor;
        [SerializeField] private Color _trueColor;
        [SerializeField] private Color _falseColor;
        private void Start()
        {
            _board.OnBoardCellChangedEvent.AddListener(OnBoardCellChanged);
        }

        private void OnBoardCellChanged(int arg0, Board.BoardCellStatus arg1)
        {
            switch (arg1)
            {
                case Board.BoardCellStatus.None:
                    _cellImages[arg0].sprite = null;
                    _cellImages[arg0].color = _nullColor;
                    break;
                case Board.BoardCellStatus.True:
                    _cellImages[arg0].sprite = _trueSprite;
                    _cellImages[arg0].color = _trueColor;
                    break;
                case Board.BoardCellStatus.False:
                    _cellImages[arg0].sprite = _falseSprite;
                    _cellImages[arg0].color = _falseColor;
                    break;
            }
        }
    }
}