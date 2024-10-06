using System;
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
        public List<string> convertToPostfix(string expr)
        {
            var ret = new List<string>();
            var operatorStack = new Stack<char>();
            expr = expr.Replace('–', '-'); // Handle non-standard subtraction symbols

            for (int i = 0; i < expr.Length; i++)
            {
                char ch = expr[i];

                if (char.IsWhiteSpace(ch)) continue;

                // Handle numbers and negative numbers
                if (char.IsDigit(ch) || (ch == '-' && (i == 0 || expr[i - 1] == '(' || isOperator(expr[i - 1]))))
                {
                    string number = string.Empty;

                    // Handle negative numbers
                    if (ch == '-')
                    {
                        number += ch;  // Add the '-' sign
                        i++;
                    }

                    // Continue parsing the number after the '-' or directly
                    while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.'))
                    {
                        number += expr[i];
                        i++;
                    }

                    i--; // Adjust index after loop
                    ret.Add(number); // Add the parsed number
                }
                else if (ch == '(')
                {
                    operatorStack.Push(ch);
                }
                else if (ch == ')')
                {
                    while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
                    {
                        ret.Add(operatorStack.Pop().ToString());
                    }
                    operatorStack.Pop(); // Pop the '('
                }
                else if (isOperator(ch))
                {
                    while (operatorStack.Count > 0 && Prior(operatorStack.Peek()) >= Prior(ch))
                    {
                        ret.Add(operatorStack.Pop().ToString());
                    }
                    operatorStack.Push(ch);
                }
            }

            // Pop remaining operators from the stack
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
        private bool isOperator(char ch) 
        {
            return ch == '+' || ch == '-' || ch == '*' || ch == '/';
        }

        public double evaluateExpr()
        {
            var evaluationStack = new Stack<double>(); // ADDED: Stack for evaluation

            foreach (var token in postFixExpr)
            {
                if (double.TryParse(token, out double number))
                {
                    evaluationStack.Push(number); // Push number to stack
                }
                else if (isOperator(token[0]) && token.Length == 1)
                {
                    double b = evaluationStack.Pop();
                    double a = evaluationStack.Pop();

                    switch (token)
                    {
                        case "+":
                            evaluationStack.Push(a + b);
                            break;
                        case "-":
                            evaluationStack.Push(a - b);
                            break;
                        case "*":
                            evaluationStack.Push(a * b);
                            break;
                        case "/":
                            evaluationStack.Push(a / b);
                            break;
                    }
                }
            }

            isEvaluated = true; // Mark as evaluated
            return evaluationStack.Pop(); // Return result
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
       public bool setExpression(string exprStr)
        {
            if (!isExpressionValid(exprStr)) { return false; }

            expr = new Expression(exprStr);
            return true;
        }

        // checks if the expression can be evaluated in the first place
        //first check if expression is only + - / */x, (), and numbers
        //then expression should make sense ie 2+3 4* and 9(*3*5) are not valid
        public bool isExpressionValid(string expr)
        {
            var res = Regex.IsMatch(expr, "^[0-9+ ()*-/]+$"); // expression can only be these characters and must be at least 1 long
            //Console.WriteLine(expr + " is " + res);
            string previous = ""; // expression must go number/operator/number/operator
            string current = "";
            expr = Regex.Replace(expr, @"\s+", " "); // get rid of repeating spaces
            expr = expr.Trim();
            int bracketCounter = 0;
            var split = expr.Split(' ');
            for (int i = 0; i < split.Length; i++)
            {
                previous = current;
                current = split[i];
                // previous and current cannot both be numbers
                if (previous != "")
                { // check only after first iteration and skip brackets
                    if (float.TryParse(previous, out float _) == float.TryParse(current, out float _))
                    {
                        Console.WriteLine("previous and current cannot both be numbers");
                        return false;
                    }
                }
                if (current == "(")
                {
                    bracketCounter++;
                }
                else if (current == ")")
                {
                    bracketCounter--;
                }
                if (bracketCounter < 0)
                {
                    Console.WriteLine("Mismatched brackets");
                    return false;
                }
            }
            Console.WriteLine("it is an expression");
            return true;
        }

        // ADDED: Helper method to check if a character is an operator
        private bool isOperator(char ch)
        {
            return ch == '+' || ch == '-' || ch == '*' || ch == '/';
        }

        //
        public double evaluateExpressions() {
            if (expr == null) 
            { // if no expressions are set yet
                throw new Exception("no expressions loaded"); //wrap evaluateExpressions call in try catch
            }
            return expr.evaluateExpr();
        }
    }

    public class Program
    {
        public static Calculation calculation = new Calculation(); // ADDED: Global variable for calculation

        public static string ProcessCommand(string input)
        {
            try
            {
                if (!calculation.setExpression(input))
                {
                    return "Invalid expression!";
                }
                double result = calculation.evaluateExpressions(); // ADDED: Evaluate expression
                return result.ToString();
            }
            catch (Exception e)
            {
                return "Error evaluating expression: " + e.Message;
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
