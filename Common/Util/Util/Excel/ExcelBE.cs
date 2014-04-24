using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Common
{
    /// <summary>
    /// 工具类：导入导出Excel的基类
    /// http://www.cnblogs.com/springyangwc/archive/2011/08/12/2136498.html
    /// </summary>
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
        private double _columnWidth = 16;
        private double _rowHeight = 16;
        private byte _horizontalAlignmentIndex = 1;
        private string _fontColor = string.Empty;
        private string _fontName = string.Empty;
        private int _fontSize = 12;
        private bool _fontBold = false;
        private string _format = string.Empty;

        public ExcelBE(int row, int col, string text, string startCell, string endCell, string interiorColor, bool isMerge, int size, double columnWidth, double rowHeight, byte horizontalAlignmentIndex, string fontColor, string fontName, int fontSize, bool fontBold, string format)
        {
            _row = row;
            _col = col;
            _text = text;
            _startCell = startCell;
            _endCell = endCell;
            _interiorColor = interiorColor;
            _isMerge = isMerge;
            _size = size;
            _columnWidth = columnWidth;
            _rowHeight = rowHeight;
            _horizontalAlignmentIndex = horizontalAlignmentIndex;
            _fontColor = fontColor;
            _fontName = fontName;
            _fontSize = fontSize;
            _fontBold = fontBold;
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

        public double ColumnWidth
        {
            get { return _columnWidth; }
            set { _columnWidth = value; }
        }

        public double RowHeight
        {
            get { return _rowHeight; }
            set { _rowHeight = value; }
        }

        public byte HorizontalAlignmentIndex
        {
            get { return _horizontalAlignmentIndex; }
            set { _horizontalAlignmentIndex = value; }
        }


        public string FontColor
        {
            get { return _fontColor; }
            set { _fontColor = value; }
        }

        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }

        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        public bool FontBold
        {
            get { return _fontBold; }
            set { _fontBold = value; }
        }

        public string Formart
        {
            get { return _format; }
            set { _format = value; }
        }
    }
}
