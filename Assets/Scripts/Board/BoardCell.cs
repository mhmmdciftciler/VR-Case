using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VRCase
{
    [System.Serializable]
    public struct BoardCell
    {
        public BoxType boxTypeForColumn;//column
        public Pallet palletForRow;//row
        public BoardCellStatus BoardCellStatus;
    }
}
