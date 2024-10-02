using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assignment1 {
    // this object represents an expression (ie 5+2*3)
    public class Expression {
        public string stringExpr;
        //private bool isEvaluated; //modified 
        private List<string> postFixExpr;
        public Expression(string expr) {
            stringExpr = expr;
            postFixExpr = convertToPostfix(expr); //addwd 
        }

        public List<string> getExpression() {
            return postFixExpr;
        }

        // convert the string to a postfix expression (Reverse Polish Notation)
        // without sorting it by order of operations
        public List<string> convertToPostfix(string expr) {
            var ret = new List<string>();
            var operatorStack = new Stack<char>(); // added: Stack to hold operators
            char prevCh = ' ';
            for (int i = 0; i < expr.Length; i++) {
                char ch = expr[i];

                if (char.IsWhiteSpace(ch)) continue; // ignore empty spaces

                if (char.IsDigit(ch)) { // added: Parse numbers
                    string number = string.Empty;
                    if (prevCh == '-') {
                        number = "-";
                    }
                    while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.')) {
                        number += expr[i];
                        i++;
                    }
                    i--; // adjust index after loop
                    ret.Add(number);
                } else if (ch == '(') {
                    operatorStack.Push(ch); // added: Push '(' to stack
                } else if (ch == ')') {
                    // added: Pop operators until '(' is found
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '(') {
                        ret.Add(operatorStack.Pop().ToString());
                    }
                    operatorStack.Pop(); // Pop the '('
                } else if (isOperator(ch.ToString())) {
                    // added: Pop operators with higher or equal precedence
                    while (operatorStack.Count > 0 && Prior(operatorStack.Peek()) >= Prior(ch)) {
                        ret.Add(operatorStack.Pop().ToString());
                    }
                    operatorStack.Push(ch); // Push current operator
                }
                prevCh = ch;
            }

            // Pop remaining operators
            while (operatorStack.Count > 0) {
                ret.Add(operatorStack.Pop().ToString());
            }

            return ret;
        }


        //added:to determine the order of operatord evaluated in an expression
        private int Prior(char op) {
            switch (op) {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                default:
                    return 0;
            }
        }

        // return true if "ch" is an operator
        private bool isOperator(string ch) {
            return Regex.IsMatch(ch, "^[=+/*]+$");
        }

        // evaluates the postfix expression and returns the result
        // regardless of if it is sorted by OOP or not
        public double evaluateExpr() {
            //Console.WriteLine("evaluating");
            var stack = new Stack<float>();
            for (int i = 0; i < postFixExpr.Count; i++) {
                var e = postFixExpr[i];
                if (isOperator(e)) {
                    float a;
                    float b;
                    switch (e) {
                        case ("*"):
                            stack.Push(stack.Pop() * stack.Pop());
                            break;
                        case ("/"):
                            a = stack.Pop();
                            b = stack.Pop();
                            stack.Push(b / a);
                            break;
                        case ("+"):
                            stack.Push(stack.Pop() + stack.Pop());
                            break;
                        case ("-"):
                            a = stack.Pop();
                            b = stack.Pop();
                            stack.Push(b - a);
                            break;
                    }
                } else {
                    stack.Push(float.Parse(e));
                }
            }

            return stack.Pop();
        }
    }

    // this class represents a calculator "session" and its expressions / state
    public class Calculation {
        private Expression expr;
        public Calculation() { }

        public List<string> getExpression() {
            return expr.getExpression();
        }

        //return value is false if it is not a valid expression
        public bool setExpression(string str) {
            str = Regex.Replace(str, @"\s+", " "); // get rid of repeating spaces
            str = str.Trim();
            if (!Regex.IsMatch(str, "^[0-9+ ()*-/]+$")) { return false; }
            expr = new Expression(str);
            return true;
        }

        public double evaluateExpressions() {
            if (expr == null) { // if no expressions are set yet
                throw new Exception("no expressions loaded"); //wrap evaluateExpressions call in try catch
            }
            return expr.evaluateExpr();
        }
    }

    public class Program {
        public static string ProcessCommand(string input) {
            //try {
                // TODO Evaluate the expression and return the result
                var c = new Calculation();
                if (!c.setExpression(input)) { return "expression invalid"; }
            Console.WriteLine("printing expression");
            foreach (var e in c.getExpression()) {
                Console.Write(e + " ");
            }
            var ret = c.evaluateExpressions();
                return ret.ToString();
            //} catch (Exception e) {
            //    return "Error evaluating expression: " + e;
            //}
        }

        static void Main(string[] args) {
            string input;
            while ((input = Console.ReadLine()) != "exit") {
                Console.WriteLine(ProcessCommand(input));
            }
        }
    }
}
