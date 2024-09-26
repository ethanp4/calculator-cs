﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Assignment1 {
    // this object represents an expression (ie 5+2*3)
    public class Expression 
    {
        private string stringExpr { get; set; }
        private bool isEvaluated { get; set; } = false; //modified 
        private List<string> postFixExpr;
        public Expression(string expr) {
            stringExpr = expr;
            postFixExpr = convertToPostfix(expr); //addwd 
            
        }

        // convert the string to a postfix expression (Reverse Polish Notation)
        // without sorting it by order of operations
        public List<string> convertToPostfix(string expr) {
            var ret = new List<string>();
            var operatorStack = new Stack<char>(); // added: Stack to hold operators

            for (int i = 0; i < expr.Length; i++)
            {
                char ch = expr[i];

                if (char.IsWhiteSpace(ch)) continue; // ignore empty spaces

                if (char.IsDigit(ch))
                { // added: Parse numbers
                    string number = string.Empty;
                    while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.'))
                    {
                        number += expr[i];
                        i++;
                    }
                    i--; // adjust index after loop
                    ret.Add(number);
                }
                else if (ch == '(')
                {
                    operatorStack.Push(ch); // added: Push '(' to stack
                }
                else if (ch == ')')
                {
                    // added: Pop operators until '(' is found
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
                    {
                        ret.Add(operatorStack.Pop().ToString());
                    }
                    operatorStack.Pop(); // Pop the '('
                }
                else if (isOperator(ch))
                {
                    // added: Pop operators with higher or equal precedence
                    while (operatorStack.Count > 0 && Prior(operatorStack.Peek()) >= Prior(ch))
                    {
                        ret.Add(operatorStack.Pop().ToString());
                    }
                    operatorStack.Push(ch); // Push current operator
                }
            }

            // Pop remaining operators
            while (operatorStack.Count > 0)
            {
                ret.Add(operatorStack.Pop().ToString());
            }

            return ret;
        }


        //added:to determine the order of operatord evaluated in an expression
        private int Prior(char op)
        {
            switch (op)
            {
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
        private bool isOperator(char ch) {
            return false;
        }

        // this takes no parameters and sorts the postFixExpr of
        // this object
        public void sortExprByOOP() {

        }

        // evaluates the postfix expression and returns the result
        // regardless of if it is sorted by OOP or not
        public double evaluateExpr() {
            return 420.0;
        }
    }

    // this class represents a calculator "session" and its expressions / state
    public class Calculation {
        // can change this to hold multiple expressions
        // for referring to the previous answer 
        private Expression expr;
        public Calculation() { }

        //can be later extended to add multiple expressions in order
        //then calculate all of them sequentially
        //return value is false if it is not a valid expression
        public bool setExpression(string expr) {
            if (!isExpressionValid(expr)) { return false; }

            return true;
        }

        // checks if the expression can be evaluated in the first place
        //first check if expression is only + - / */x, (), and numbers
        //then expression should make sense ie 2+3 4* and 9(*3*5) are not valid
        public bool isExpressionValid(string expr) {
            var res = Regex.IsMatch(expr, "^[0-9+ ()*-/]+$"); // expression can only be these characters and must be at least 1 long
            //Console.WriteLine(expr + " is " + res);
            string previous = ""; // expression must go number/operator/number/operator
            string current = "";
            expr = Regex.Replace(expr, @"\s+", " "); // get rid of repeating spaces
            expr = expr.Trim();
            int bracketCounter = 0;
            var split = expr.Split(' ');
            for (int i = 0; i < split.Length; i++) {
                previous = current;
                current = split[i];
                // previous and current cannot both be numbers
                if (previous != "") { // check only after first iteration and skip brackets
                    if (float.TryParse(previous, out float _) == float.TryParse(current, out float _)) {
                        Console.WriteLine("previous and current cannot both be numbers");
                        return false;
                    }
                }
                if (current == "(") {
                    bracketCounter++;
                } else if (current == ")") {
                    bracketCounter--;
                }
                if (bracketCounter < 0) {
                    Console.WriteLine("Mismatched brackets");
                    return false;
                }
            }
            Console.WriteLine("it is an expression");
            return true;
        }

        //
        public double evaluateExpressions() {
            if (expr == null) { // if no expressions are set yet
                throw new Exception("no expressions loaded"); //wrap evaluateExpressions call in try catch
            }
            return 0;
        }
    }

    public class Program {
        public static string ProcessCommand(string input) {
            try {
                // TODO Evaluate the expression and return the result
                var c = new Calculation();
                c.setExpression(input);
                return "";
            } catch (Exception e) {
                return "Error evaluating expression: " + e;
            }
        }

        static void Main(string[] args) {
            string input;
            while ((input = Console.ReadLine()) != "exit") {
                Console.WriteLine(ProcessCommand(input));
            }
        }
    }
}
