using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.Events;
using System.Linq;
using Fusion.Addons.ConnectionManagerAddon;
using System;

namespace VRCase
{
    public class Board : MonoBehaviour
    {
        [SerializeField] private ConnectionManager _connectionManager;
        [SerializeField] private GamePlayRuleService _gamePlayRuleService;
        [SerializeField] private List<Pallet> _pallets;

        public List<BoardCell> BoardCells;

        public UnityEvent<int, BoardCellStatus> OnBoardCellChangedEvent;
        private void Awake()
        {
            _connectionManager.onLocalPlayerJoined.AddListener(OnLocalPlayerJoined);
        }

        /// <summary>
        /// Called when a local player joins the game.
        /// Initiates a coroutine to handle further actions required when the local player joins.
        /// </summary>
        private void OnLocalPlayerJoined()
        {
            StartCoroutine(OnLocalPlayerJoinedRoutine());
        }

        /// <summary>
        /// Coroutine that handles actions required after a local player joins the game.
        /// </summary>
        /// <remarks>
        /// This coroutine waits until the pallets are available in the scene and then updates the board status for each pallet
        /// based on its current box type. It also subscribes to the pallet's box type change event to ensure the board is updated
        /// whenever a pallet's box type changes.
        /// </remarks>
        /// <returns>An IEnumerator for coroutine execution.</returns>
        private IEnumerator OnLocalPlayerJoinedRoutine()
        {
            yield return new WaitUntil(() => _pallets[0].Object != null); // Wait until pallets data is available, as Pallet is a scene object.

            foreach (Pallet pallet in _pallets)
            {
                if (pallet.CurrentBoxType != BoxType.None)
                {
                    UpdateBoardWithPalletStatus(pallet, pallet.CurrentBoxType); // Update board for the new player.
                }

                pallet.OnPalletBoxTypeChangedEvent.AddListener(UpdateBoardWithPalletStatus); // Subscribe to the pallet's box type change event.
            }
        }


        /// <summary>
        /// Updates the status of the board based on the given pallet and box type.
        /// </summary>
        /// <param name="pallet">The pallet whose status needs to be updated on the board.</param>
        /// <param name="boxType">The type of the box that determines how the board cell status should be updated.</param>
        private void UpdateBoardWithPalletStatus(Pallet pallet, BoxType boxType)
        {
            if (boxType == BoxType.None)
            {
                int cellIndex = BoardCells.FindIndex(x => x.palletForRow == pallet && x.BoardCellStatus != BoardCellStatus.None);// Find the last marked cell. 

                if (cellIndex != -1)// if could not find a cell.
                {
                    BoardCell cell = BoardCells[cellIndex];// Get Cell
                    cell.BoardCellStatus = BoardCellStatus.None;// Set Cell Status
                    BoardCells[cellIndex] = cell;// Set Cell

                    OnBoardCellChangedEvent?.Invoke(cellIndex, BoardCellStatus.None);

                    UpdateBoardForNeighbourPalletStatus(pallet);// Update for neighbour pallets status.
                }
            }
            else
            {
                SetCellStatusNoneWithPalletRow(pallet); // reset all column for pallet row

                int cellIndex = BoardCells.FindIndex(x => x.palletForRow == pallet && x.boxTypeForColumn == boxType); // Find cell with row and column.

                BoardCell cell = BoardCells[cellIndex];

                bool isValidate = pallet.IsPalletRuleValidate();

                BoardCellStatus boardCellStatus = isValidate ? BoardCellStatus.True : BoardCellStatus.False;

                if (cell.BoardCellStatus != boardCellStatus) // Check for difference to cell and pallet
                {
                    cell.BoardCellStatus = boardCellStatus; // Set Cell Status
                    BoardCells[cellIndex] = cell; // Set Cell
                    OnBoardCellChangedEvent?.Invoke(cellIndex, boardCellStatus);
                    UpdateBoardForNeighbourPalletStatus(pallet); // Update for neighbour pallets status.
                }
            }
        }

        /// <summary>
        /// Updates the status of the board cells associated with the neighboring pallets of the given pallet.
        /// </summary>
        /// <param name="relatedPallet">The pallet whose neighboring pallets' statuses need to be updated.</param>
        private void UpdateBoardForNeighbourPalletStatus(Pallet relatedPallet)
        {
            foreach (Pallet pallet in relatedPallet.NeighbourPallets)
            {
                if (pallet.CurrentBoxType == BoxType.None) // If neighbour boxType is none, continue. Because rules are not using
                {
                    continue;
                }

                int cellIndex = BoardCells.FindIndex(x => x.palletForRow == pallet && x.BoardCellStatus != BoardCellStatus.None); // Find cell index for neighbour pallet if Cell status is not none

                if (cellIndex == -1) // If could not find a cell
                {
                    continue;
                }

                BoardCell cell = BoardCells[cellIndex];

                bool palletCurrentValidation = pallet.IsPalletRuleValidate(); // Get pallet rule validation

                BoardCellStatus boardCellStatus = palletCurrentValidation ? BoardCellStatus.True : BoardCellStatus.False;

                if (cell.BoardCellStatus != boardCellStatus) // Check for difference between cell and pallet status
                {
                    cell.BoardCellStatus = boardCellStatus; // Set Cell Status
                    BoardCells[cellIndex] = cell; // Set Cell
                    OnBoardCellChangedEvent?.Invoke(cellIndex, boardCellStatus);
                }
            }
        }

        /// <summary>
        /// Resets the status of all board cells in the row corresponding to the given pallet to `None`.
        /// </summary>
        /// <param name="pallet">The pallet whose row's cell statuses need to be reset.</param>
        private void SetCellStatusNoneWithPalletRow(Pallet pallet)
        {
            for (int i = 0; i < BoardCells.Count; i++)
            {
                BoardCell boardCell = BoardCells[i];
                if (boardCell.palletForRow != pallet)
                    continue;
                if (boardCell.BoardCellStatus != BoardCellStatus.None && // If this cell is marked
                    boardCell.palletForRow.CurrentBoxType == BoxType.None) // If the cell and pallet are not in sync
                {
                    boardCell.BoardCellStatus = BoardCellStatus.None; // Unmark this cell
                    BoardCells[i] = boardCell;
                    OnBoardCellChangedEvent?.Invoke(i, BoardCellStatus.None);
                }
            }
        }

    }

}
