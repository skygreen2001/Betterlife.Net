using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Common
{
    public class ExcelBE
    {
        private int _row = 0;
        private int _col = 0;
        private string _text = string.Empty;
        private string _startCell = string.Empty;
        private string _endCell = string.Empty;
        private string _interiorColor = string.Empty;
        private bool _isMerge = false;
        private int _size = 0;
        private string _fontColor = string.Empty;
        private string _format = string.Empty;

        public ExcelBE(int row, int col, string text, string startCell, string endCell, string interiorColor, bool isMerge, int size, string fontColor, string format)
        {
            _row = row;
            _col = col;
            _text = text;
            _startCell = startCell;
            _endCell = endCell;
            _interiorColor = interiorColor;
            _isMerge = isMerge;
            _size = size;
            _fontColor = fontColor;
            _format = format;
        }

        public ExcelBE()
        { }

        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public int Col
        {
            get { return _col; }
            set { _col = value; }
        }

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public string StartCell
        {
            get { return _startCell; }
            set { _startCell = value; }
        }

        public string EndCell
        {
            get { return _endCell; }
            set { _endCell = value; }
        }

        public string InteriorColor
        {
            get { return _interiorColor; }
            set { _interiorColor = value; }
        }

        public bool IsMerge
        {
            get { return _isMerge; }
            set { _isMerge = value; }
        }

        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public string FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        public string Formart
        {
            get { return _format; }
            set { _format = value; }
        }
    }
}
