using System;
using System.Collections.Generic;
using System.Text;

namespace sorting_station
{
    class TokenParentheses : Token
    {
        public bool IsOpening { get; }

        public TokenParentheses(bool isOpening)
        {
            IsOpening = isOpening;
        }
        public TokenParentheses(char ch)
        {
            switch(ch)
            {
                case '(':
                    IsOpening = true;
                    break;
                case ')':
                    IsOpening = false;
                    break;
            }
        }

        public override string ToString()
        {
            return IsOpening ? "(" : ")";
        }
    }
}
