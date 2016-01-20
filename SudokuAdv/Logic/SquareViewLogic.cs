using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SudokuAdv.Logic
{
    public class SquareViewLogic : ViewBase
    {
        #region Properties
        private int _value;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (IsEditable)
                {
                    _value = value;
                    NotifyPropertyChanged("Value");
                    NotifyPropertyChanged("StringValue");
                    UpdateState();
                }
            }
        }

        public string StringValue
        {
            get
            {
                string result = "";
                if (_value > 0)
                    result = _value.ToString();
                return result;
            }
        }

        private bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                UpdateState();
            }
        }

        private bool _isValid = true;
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                UpdateState();
            }
        }

        private bool _isEditable = true;
        public bool IsEditable
        {
            get
            {
                return _isEditable;
            }
            set
            {
                _isEditable = value;
                UpdateState();
            }
        }

        private int _row;
        public int Row
        {
            get
            { return _row; }
            set
            {
                _row = value;
            }
        }

        private int _column;
        public int Column
        {
            get
            { return _column; }
            set
            {
                _column = value;
            }
        }

        private int _currentBoxState = BoxStates.Default;
        public int CurrentBoxState
        {
            get
            {
                return _currentBoxState;
            }
            set
            {
                _currentBoxState = value;
                NotifyPropertyChanged("CurrentBoxState");
            }
        }
        #endregion

        private void UpdateState()
        {
            if (IsEditable)
            {
                if (IsSelected)
                    CurrentBoxState = BoxStates.Selected;
                else if (!IsValid)
                    CurrentBoxState = BoxStates.Invalid;
                else
                    CurrentBoxState = BoxStates.Default;
            }
            else
            {
                CurrentBoxState = BoxStates.UnEditable;
            }
        }
    }
}
