using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace sorting_station
{
    class ShuntingYard
    {
        //public string Postfix { get; }

        public double Value { get; private set; }
        Stack<double> _numbers;
        Stack<Token> _operators;
        //StringBuilder _postfix;

        public ShuntingYard()
        {
            Value = 0;
            _numbers = new Stack<double>();
            _operators = new Stack<Token>();
            //_postfix = new StringBuilder();
        }

        public bool Evaluate(string infix)
        {
            var parser = new Parser();
            foreach (var token in parser.GetToken(infix))
            {
                if (!TryEvaluate(token))
                    return false;
            }
            try
            {
                EndEvaluation();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public bool TryEvaluate(Token token)
        {
            try
            {
                ProcessToken(token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }


        private void ProcessToken(Token token)
        {
            switch(token)
            {
                case TokenError error:
                    throw new Exception("error in evaluation");
                case TokenNumber number:
                    _numbers.Push(number.Value);
                    break;
                case TokenOperator op:
                    if (_operators.Count > 0 && _operators.Peek() is TokenOperator op2 && op2.CompareTo(op) == 1)
                    {
                        double y = _numbers.Pop();
                        double x = _numbers.Pop();
                        try
                        {
                            Value = op2.Apply(x, y);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        _numbers.Push(Value);
                        _operators.Pop();
                        _operators.Push(op);
                    }
                    else
                        _operators.Push(op);
                    break;
                case TokenParentheses br:
                    if (br.IsOpening)
                        _operators.Push(br);
                    else
                    {
                        while (_operators.Count > 0 && _operators.Peek() is TokenOperator op)
                        {
                            double y = _numbers.Pop();
                            double x = _numbers.Pop();
                            try
                            {
                                Value = op.Apply(x, y);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            _numbers.Push(Value);
                            _operators.Pop();
                        }

                        if (_operators.Peek() is TokenParentheses brnew && brnew.IsOpening)
                            _operators.Pop();
                        else
                           throw new Exception("error in brackets");
                    }
                    break;
            }
        }

        private void EndEvaluation()
        {
            while (_operators.Count > 0 && _numbers.Count > 1)
            {
                if (_operators.Pop() is TokenOperator op)
                {
                    double y = _numbers.Pop();
                    double x = _numbers.Pop();
                    try
                    {
                        Value = op.Apply(x, y);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    _numbers.Push(Value);
                }
            }

        }
    }
}
