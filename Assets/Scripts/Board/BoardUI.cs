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

        /// <summary>
        /// Handles the event when a board cell's status changes.
        /// </summary>
        /// <param name="cellIndex">The index of the board cell that changed.</param>
        /// <param name="status">The new status of the board cell.</param>
        /// <remarks>
        /// This method updates the appearance of the cell image based on its new status.
        /// - If the status is `None`, it sets the cell image to null and applies a default color.
        /// - If the status is `True`, it sets the cell image to a specific sprite and applies a color indicating truth.
        /// - If the status is `False`, it sets the cell image to a different sprite and applies a color indicating falsehood.
        /// </remarks>
        private void OnBoardCellChanged(int cellIndex, BoardCellStatus status)
        {
            switch (status)
            {
                case BoardCellStatus.None:
                    _cellImages[cellIndex].sprite = null;
                    _cellImages[cellIndex].color = _nullColor;
                    break;
                case BoardCellStatus.True:
                    _cellImages[cellIndex].sprite = _trueSprite;
                    _cellImages[cellIndex].color = _trueColor;
                    break;
                case BoardCellStatus.False:
                    _cellImages[cellIndex].sprite = _falseSprite;
                    _cellImages[cellIndex].color = _falseColor;
                    break;
            }
        }
    }
}