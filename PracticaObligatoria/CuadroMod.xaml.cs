using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Código C# asociado a CuadroMod.xaml
    /// </summary>
    public partial class CuadroMod : Window
    {
        private ObservableCollection<Comidas> comidas;

        // Constructor de la clase
        public CuadroMod()
        {
            InitializeComponent();
        }

        // Getter & Setter
        public ObservableCollection<Comidas> Comidas
        {
            get { return comidas; }
            set 
            { 
                comidas = value;
                SetValues();
            }
        }
        
        // Funciones para la actualización de los datos
        private void SetValues()
        {
            Desayuno.Text = Comidas[0].Cal_Comida.ToString();
            Almuerzo.Text = Comidas[1].Cal_Comida.ToString();
            Comida.Text = Comidas[2].Cal_Comida.ToString();
            Merienda.Text = Comidas[3].Cal_Comida.ToString();
            Cena.Text = Comidas[4].Cal_Comida.ToString();
            Otros.Text = Comidas[5].Cal_Comida.ToString();
        }

        private void Cancel_bt_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Accept_bt_Click(object sender, RoutedEventArgs e)
        {
            Comidas[0].Cal_Comida = float.Parse(Desayuno.Text);
            Comidas[1].Cal_Comida = float.Parse(Almuerzo.Text);
            Comidas[2].Cal_Comida = float.Parse(Comida.Text);
            Comidas[3].Cal_Comida = float.Parse(Merienda.Text);
            Comidas[4].Cal_Comida = float.Parse(Cena.Text);
            Comidas[5].Cal_Comida = float.Parse(Otros.Text);
            DialogResult = true;
        }
    }
}
