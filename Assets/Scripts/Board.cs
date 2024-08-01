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
        public List<BoardCell> BoardCells;
        [SerializeField] private List<Pallet> _pallets;
        [SerializeField] private GamePlayRuleService _gamePlayRuleService;
        public UnityEvent<int, BoardCellStatus> OnBoardCellChangedEvent;
        private void Awake()
        {
            _connectionManager.onLocalPlayerJoined.AddListener(OnLocalPlayerJoined);
        }

        private void OnLocalPlayerJoined()
        {
            StartCoroutine(OnLocallPlayerJoinedRoutine());
        }
        private IEnumerator OnLocallPlayerJoinedRoutine()
        {
            yield return new WaitUntil(() => _pallets[0].Object != null);
            foreach (Pallet pallet in _pallets)
            {
                if (pallet.CurrentBoxType != BoxType.None)
                    UpdateBoardWithPalletStatus(pallet, pallet.CurrentBoxType);//Update board for new player

                pallet.OnPalletBoxTypeChangedEvent.AddListener(UpdateBoardWithPalletStatus);//Subscribe to Pallet Event
            }
        }
        private void UpdateBoardWithPalletStatus(Pallet pallet, BoxType boxType)
        {

            Debug.Log("UpdateBoardWithPalletStatus : " + pallet.name + " Box " + boxType);
            if (boxType == BoxType.None)
            {
                int cellIndex = BoardCells.FindIndex(x => x.palletForRow == pallet && x.BoardCellStatus != BoardCellStatus.None);// Find the last marked cell. 
                Debug.Log("Last Marked Cell Index : " + cellIndex);
                if (cellIndex != -1)
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
        private void UpdateBoardForNeighbourPalletStatus(Pallet relatedPallet)
        {
            foreach (Pallet pallet in relatedPallet.NeighbourPallets)
            {

                if (pallet.CurrentBoxType == BoxType.None)// if neighbour boxType is none when continue. Because rules is not using
                {
                    continue;
                }
                Debug.Log("UpdateBoardForNeighbourPalletStatus neighbour : " + pallet.name);
                int cellIndex = BoardCells.FindIndex(x => x.palletForRow == pallet && x.BoardCellStatus != BoardCellStatus.None); // Find cell index for neighbour pallet. if Cell status is none

                if (cellIndex == -1)
                {
                    continue;
                }

                BoardCell cell = BoardCells[cellIndex];

                bool palletCurrentValidition = pallet.IsPalletRuleValidate();

                BoardCellStatus boardCellStatus = palletCurrentValidition ? BoardCellStatus.True : BoardCellStatus.False;

                if (cell.BoardCellStatus != boardCellStatus) // Check for difference to cell and pallet
                {
                    cell.BoardCellStatus = boardCellStatus;// Set Cell Status
                    BoardCells[cellIndex] = cell;// Set Cell 
                    OnBoardCellChangedEvent?.Invoke(cellIndex, boardCellStatus);
                }
            }
        }
        private void SetCellStatusNoneWithPalletRow(Pallet pallet)
        {
            for (int i = 0; i < BoardCells.Count; i++)
            {
                BoardCell boardCell = BoardCells[i];
                if (boardCell.palletForRow != pallet)
                    continue;
                if (boardCell.BoardCellStatus != BoardCellStatus.None && boardCell.palletForRow.CurrentBoxType == BoxType.None)
                {
                    Debug.Log("SetCellStatusNoneWithPalletRow index : " + i);
                    boardCell.BoardCellStatus = BoardCellStatus.None;
                    BoardCells[i] = boardCell;
                    OnBoardCellChangedEvent?.Invoke(i, BoardCellStatus.None);
                }

            }
        }
        [System.Serializable]
        public struct BoardCell
        {
            public BoxType boxTypeForColumn;//column
            public Pallet palletForRow;//row
            public BoardCellStatus BoardCellStatus;
        }
        public enum BoardCellStatus
        {
            None,
            True,
            False
        }
    }

}
