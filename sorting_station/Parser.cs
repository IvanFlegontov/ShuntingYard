using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace sorting_station
{
    class Operator
    {
        public string Name { get; set; }
        public int Priority { get; set; }
    }

    class Parser
    {

        enum State { Start, Ok, Error, ReadNumber, ReadNumberHasDot, WaitNumber, WaitOperator }

        State _state;
        int _bracket;
        StringBuilder _number; 
        Queue<Token> _queue;

        public Parser()
        {
            _state = State.Start;
            _bracket = 0;
            _number = new StringBuilder();
            _queue = new Queue<Token>();
        }


        public IEnumerable<Token> GetToken(string infix)
        {
            Token result;
            foreach (char ch in infix)
            {
                ProcessChar(ch);
                if (_state == State.Error)
                    yield return new TokenError();
                if (_queue.TryDequeue(out result))
                    yield return result;
            }
            ProcessEnd();
            //_ state == State.Start тут code smells, чтобы программа не выдава ошибку, 
            // если в самом начале ввода строки ввести enter (\n)
            if (_state == State.Ok || _state == State.Start)
            {
                if (_queue.TryDequeue(out result))
                    yield return result;
                yield return null;
            }
            else
                yield return new TokenError();
        }
        public Queue<Token> Tokenize(string infix)
        {
            foreach(char ch in infix)
            {
                ProcessChar(ch);
                if (_state == State.Error)
                    return null;
            }
            ProcessEnd();
            if (_state == State.Ok)
                return _queue;
            else
                return null;
        }

        public void ProcessChar(char ch)
        {
            if (char.IsWhiteSpace(ch))
                return;
            switch (_state)
            {
                case State.Start:
                    switch (ch)
                    {
                        case '\r':
                        case '\n':
                            break;
                        case '(':
                            _bracket++;
                            _queue.Enqueue(new TokenParentheses(true));
                            break;
                        case '+':
                        case '-':
                            _queue.Enqueue(new TokenOperator(ch, true));
                            _state = State.WaitNumber;
                            break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            _number.Append(ch);
                            _state = State.ReadNumber;
                            break;
                        default:
                            _state = State.Error;
                            break;
                    }
                    break;
                case State.WaitNumber:
                    switch(ch)
                    {
                        case '(':
                            _bracket++;
                            _queue.Enqueue(new TokenParentheses(true));
                            _state = State.Start;
                            break;
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            _number.Append(ch);
                            _state = State.ReadNumber;
                            break;
                        default:
                            _state = State.Error;
                            break;
                    }
                    break;
                case State.ReadNumber:
                    switch(ch)
                    {
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            _number.Append(ch);
                            break;
                        case '.':
                        case ',':
                            _number.Append(ch);
                            _state = State.ReadNumberHasDot;
                            break;
                        case ')':
                            _queue.Enqueue(new TokenNumber(_number.ToString()));
                            _number.Clear();
                            _queue.Enqueue(new TokenParentheses(false));
                            _bracket--;
                            _state = _bracket >= 0 ? State.WaitOperator : State.Error;
                            break;
                        case '+':
                        case '-':
                        case '*':
                        case '/':
                            _queue.Enqueue(new TokenNumber(_number.ToString()));
                            _number.Clear();
                            _queue.Enqueue(new TokenOperator(ch));
                            _state = State.WaitNumber;
                            break;
                        default:
                            _state = State.Error;
                            break;
                    }
                    break;
                case State.ReadNumberHasDot:
                    switch (ch)
                    {
                        case '0':
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9':
                            _number.Append(ch);
                            break;
                        case ')':
                            _queue.Enqueue(new TokenNumber(_number.ToString()));
                            _number.Clear();
                            _queue.Enqueue(new TokenParentheses(false));
                            _bracket--;
                            _state = _bracket >= 0 ? State.WaitNumber : State.Error;
                            break;
                        case '+':
                        case '-':
                        case '*':
                        case '/':
                            _queue.Enqueue(new TokenNumber(_number.ToString()));
                            _number.Clear();
                            _queue.Enqueue(new TokenOperator(ch));
                            _state = State.WaitNumber;
                            break;
                        default:
                            _state = State.Error;
                            break;
                    }
                    break;
                case State.WaitOperator:
                    switch (ch)
                    {
                        case ')':
                            _queue.Enqueue(new TokenParentheses(false));
                            _bracket--;
                            _state = _bracket >= 0 ? State.WaitOperator : State.Error;
                            break;
                        case '+':
                        case '-':
                        case '*':
                        case '/':
                            _queue.Enqueue(new TokenOperator(ch));
                            _state = State.WaitNumber;
                            break;
                        default:
                            _state = State.Error;
                            break;
                    }
                    break;
                default:
                    _state = State.Error;
                    break;

            }
        }

        public void ProcessEnd()
        {
            switch(_state)
            {
                case State.Start:
                    break;
                case State.WaitNumber:
                    _state = State.Error;
                    break;
                case State.ReadNumber:
                case State.ReadNumberHasDot:
                    _queue.Enqueue(new TokenNumber(_number.ToString()));
                    _number.Clear();
                    _state = _bracket == 0 ? State.Ok : State.Error;
                    break;
                case State.WaitOperator:
                    _state = _bracket == 0 ? State.Ok : State.Error;
                    break;
            }
        }
    }
}
