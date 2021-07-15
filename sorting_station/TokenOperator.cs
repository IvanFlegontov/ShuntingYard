using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace sorting_station
{
    class TokenOperator : Token, IComparable
    {
        public OperatorType Type { get; }
        int _priority;

        public TokenOperator(char ch, bool unary = false)
        {
            switch (ch)
            {
                case '+':
                    _priority = 1;
                    Type = unary ? OperatorType.UnaryPlus : OperatorType.Add;
                    break;
                case '-':
                    _priority = 1;
                    Type = unary ? OperatorType.UnaryMinus : OperatorType.Subtract;
                    break;
                case '*':
                    _priority = 2;
                    Type = OperatorType.Multiply;
                    break;
                case '/':
                    _priority = 2;
                    Type = OperatorType.Divide;
                    break;
            }
        }

        public double Apply(double x, double y)
        {
            double result = 0;
            switch(Type)
            {
                case OperatorType.Add:
                    result = x + y;
                    break;
                case OperatorType.Subtract:
                    result = x - y;
                    break;
                case OperatorType.Multiply:
                    result = x * y;
                    break;
                case OperatorType.Divide:
                    result = x / y;
                    if (double.IsInfinity(result))
                        throw new Exception("Division by 0");
                    break;
                default:
                    throw new Exception("unary operation on two numbers");
            }
            return result;
        }

        public override string ToString()
        {
            return Type.ToString();
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            TokenOperator otherOperator = obj as TokenOperator;
            return _priority >= otherOperator._priority ? 1 : -1;
        }
    }
}
