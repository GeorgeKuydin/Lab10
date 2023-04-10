using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class Calculator : ICalculator
    {
        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Subtract(double a, double b)
        {
            return a - b;
        }

        public double Multiply(double a, double b)
        {
            return a * b;
        }

        public double Divide(double a, double b)
        {
            return a / b;
        }
    }
    public class ProxyCalculator : Calculator
    {
        private Calculator _calculator = new Calculator();

        public double Add(double a, double b)
        {
            ValidateInput(a, b);
            return _calculator.Add(a, b);
        }

        public double Subtract(double a, double b)
        {
            ValidateInput(a, b);
            return _calculator.Subtract(a, b);
        }

        public double Multiply(double a, double b)
        {
            ValidateInput(a, b);
            return _calculator.Multiply(a, b);
        }

        public double Divide(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException();
            }

            ValidateInput(a, b);
            return _calculator.Divide(a, b);
        }

        private void ValidateInput(double a, double b)
        {
            if (double.IsNaN(a) || double.IsNaN(b))
            {
                throw new ArgumentException("Invalid input: NaN");
            }

            if (double.IsInfinity(a) || double.IsInfinity(b))
            {
                throw new ArgumentException("Invalid input: Infinity");
            }
        }
    }
    public class CalculatorForm : Form
    {
        private ICalculator _calculator = new ProxyCalculator();

        private TextBox _textBoxA;
        private TextBox _textBoxB;
        private Button _buttonAdd;
        private Button _buttonSubtract;
        private Button _buttonMultiply;
        private Button _buttonDivide;
        private Label _labelResult;

        public CalculatorForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // Set up the TextBoxes
            _textBoxA = new TextBox();
            _textBoxA.Location = new Point(10, 10);
            _textBoxA.Size = new Size(100, 20);

            _textBoxB = new TextBox();
            _textBoxB.Location = new Point(10, 40);
            _textBoxB.Size = new Size(100, 20);

            // Set up the Buttons
            _buttonAdd = new Button();
            _buttonAdd.Text = "+";
            _buttonAdd.Location = new Point(10, 70);
            _buttonAdd.Size = new Size(30, 30);
            _buttonAdd.Click += ButtonAdd_Click;
            _buttonSubtract = new Button();
            _buttonSubtract.Text = "-";
            _buttonSubtract.Location = new Point(50, 70);
            _buttonSubtract.Size = new Size(30, 30);
            _buttonSubtract.Click += ButtonSubtract_Click;

            _buttonMultiply = new Button();
            _buttonMultiply.Text = "*";
            _buttonMultiply.Location = new Point(90, 70);
            _buttonMultiply.Size = new Size(30, 30);
            _buttonMultiply.Click += ButtonMultiply_Click;

            _buttonDivide = new Button();
            _buttonDivide.Text = "/";
            _buttonDivide.Location = new Point(130, 70);
            _buttonDivide.Size = new Size(30, 30);
            _buttonDivide.Click += ButtonDivide_Click;

            // Set up the Label
            _labelResult = new Label();
            _labelResult.Location = new Point(10, 110);
            _labelResult.Size = new Size(100, 20);
            _labelResult.Text = "Result:";

            // Add the controls to the form
            this.Controls.Add(_textBoxA);
            this.Controls.Add(_textBoxB);
            this.Controls.Add(_buttonAdd);
            this.Controls.Add(_buttonSubtract);
            this.Controls.Add(_buttonMultiply);
            this.Controls.Add(_buttonDivide);
            this.Controls.Add(_labelResult);
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            Calculate(_calculator.Add);
        }

        private void ButtonSubtract_Click(object sender, EventArgs e)
        {
            Calculate(_calculator.Subtract);
        }

        private void ButtonMultiply_Click(object sender, EventArgs e)
        {
            Calculate(_calculator.Multiply);
        }

        private void ButtonDivide_Click(object sender, EventArgs e)
        {
            Calculate(_calculator.Divide);
        }

        private void Calculate(Func<double, double, double> operation)
        {
            double a;
            if (!double.TryParse(_textBoxA.Text, out a))
            {
                MessageBox.Show("Invalid input for a.");
                return;
            }

            double b;
            if (!double.TryParse(_textBoxB.Text, out b))
            {
                MessageBox.Show("Invalid input for b.");
                return;
            }

            try
            {
                double result = operation(a, b);
                _labelResult.Text = $"Result: {result}";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
