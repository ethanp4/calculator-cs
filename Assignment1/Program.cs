using System;
using System.Collections.Generic;

namespace Assignment1
{
    // this object represents an expression (ie 5+2*3)
    public class Expression {
        private string stringExpr { get; set; }
        private bool isEvaluated { get; } = false;
        private List<string> postFixExpr;
        public Expression(string expr) {
            stringExpr = expr;
        }

        // convert the string to a postfix expression
        // without sorting it by order of operations
        public List<string> convertToPostfix(string expr) {
            var ret = new List<string>();

            return ret;
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

    // this class represents a calculator "session" and its
    // expressions / state
    public class Calculation {
        // can change this to hold multiple expressions
        // for referring to the previous answer 
        private Expression expr;
        public Calculation() { }

        //can be later extended to add multiple expressions in order
        //then calculate all of them sequentially
        //return value is false if it is not a valid expression
        public bool setExpression() {

            return false;
        }

        public double evaluateExpressions() {
            if (expr == null) { // if no expressions are set yet
                throw new Exception("no expressions loaded");
            }
            return 0;
        }
    }
  
    public class Program
    {
        public static string ProcessCommand(string input)
        {
            try
            {
                // TODO Evaluate the expression and return the result
                return "";
            }
            catch (Exception e)
            {
                return "Error evaluating expression: " + e;
            }
        }

        static void Main(string[] args)
        {
            string input;
            while ((input = Console.ReadLine()) != "exit")
            {
                Console.WriteLine(ProcessCommand(input));
            }
        }
    }
}
