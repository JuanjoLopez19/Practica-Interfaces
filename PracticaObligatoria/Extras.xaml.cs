using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace PracticaObligatoria
{
    /// <summary>
    /// Código C# asociado a Extras.xaml
    /// </summary>
    public partial class Extras : Window
    {
        private float calories = 0;

        // Constructor de la clase
        public Extras()
        {
            InitializeComponent(); 
        }

        // Getter & Setter
        public float Calorias
        {
            get { return calories; }
            set
            {
                calories = value;
                if (calories != 0)
                    Cal_media.Content = calories.ToString("F");
                else
                    Cal_media.Content = "No hay datos";
            }
        }

        // Manejadores de los eventos definidos en el xaml
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void Calc_IMC_Click(object sender, RoutedEventArgs e)
        {
            TextBlock tb = Resultado_IMC;
            TextBlock tb1 = Resultado_IMC2;
            double peso;
            double altura;
            double IMC;
            if (!Peso.isEmpty() && !Altura.isEmpty())
            {
                peso = Peso.doubleValue;
                altura = Altura.doubleValue;
                IMC = peso / (altura * altura);
                IMC = Math.Round(IMC, 2);
                tb.Text = IMC.ToString("F");
                if (IMC < 18.5)
                    tb1.Text = "Estás por debajo del peso recomendado";
                else if (IMC >= 18.5 && IMC <= 24.9)
                    tb1.Text = "Estás en el peso normal";
                else if (IMC >= 25.0 && IMC <= 29.9)
                    tb1.Text = "Tienes sobrepeso";
                else if (IMC >= 30.0 && IMC <= 34.9)
                    tb1.Text = "Tienes obesidad Clase I";
                else if (IMC >= 35.0 && IMC <= 39.9)
                    tb1.Text = "Tienes obesidad Clase II";
                else
                    tb1.Text = "Tienes obesidad Clase III";
            }
            else
                tb1.Text = "Datos no válidos";

            
        }

        private void Calc_Meta_Click(object sender, RoutedEventArgs e)
        {
            double TBM, cal;
            Label tb1 = metabolismo;
            Label tb2 = Cal_Man;
            RadioButton r1 = br_H;
            RadioButton r2 = br_M;
            double peso;
            double altura;
            if (!Peso1.isEmpty() && !Altura1.isEmpty())
            {
                peso = Peso1.doubleValue;
                altura = Altura1.doubleValue;
                int edad = Edad.intValue;
                if (r1.IsChecked == true)
                    TBM = (10.0 * peso) + (6.25 * altura) - (5 * edad) + 5;
                else if (r2.IsChecked == true)
                    TBM = (10.0 * peso) + (6.25 * altura) - (5 * edad) - 161;
                else
                {
                    tb1.Content = "Selecciona el género";
                    return;
                }
                cal = TBM * Multiplo();
                cal = Math.Round(cal, 2);
                tb1.Content = TBM;
                tb2.Content = cal;
            }
            else
                tb1.Content = "Datos no válidos"; 
        }

        // Función auxiliar 
        private float Multiplo()
        {
            if (Sedentario.IsChecked == true)
                return 1.20f;
            else if (Ligera.IsChecked == true)
                return 1.375f;
            else if (Moderada.IsChecked == true)
                return 1.55f;
            else if (Intensa.IsChecked == true)
                return 1.725f;
            else if (Muy_Intensa.IsChecked == true)
                return 1.9f;
            else
                return 1f;
        }
    }
}