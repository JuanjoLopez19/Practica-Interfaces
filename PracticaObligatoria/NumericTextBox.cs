using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;

namespace PracticaObligatoria
{
    public partial class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            PreviewTextInput += new TextCompositionEventHandler(NumericTextBox_PreviewTextInput);
        }
        public int intValue
        {
            get
            {
                return Int32.Parse(this.Text);
            }
        }
        public double doubleValue
        {
            get
            {
                return double.Parse(this.Text);
            }
        }

        void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Set the format of how the numbers are given
            NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;
            // Get the separator, the negative sign and the character itself
            string decimalSeparator = numberFormatInfo.NumberDecimalSeparator;
            string negativeSign = numberFormatInfo.NegativeSign;
            string caracter = e.Text;
            if (char.IsDigit(e.Text[0]))
            {
                // No hacemos nada porque aceptamos los dígitos
            }
            else if (caracter.Equals(decimalSeparator) || caracter.Equals(negativeSign))
            { // No hacemos nada porque aceptamos el punto y el signo
            }
            else if (caracter == "\b")
            {
                // No hacemos nada porque aceptamos el Backspace
            }
            else
            {
                // Nos saltamos el carácter deteniendo el enrutamiento
                Console.WriteLine("I recognised a non handled key " + caracter);
                e.Handled = true;
            }
        }
        public bool isEmpty()
        {
            return Text.Equals("") ? true : false;
        }

        public void clear()
        {
            Text = "";
        }
    }


}