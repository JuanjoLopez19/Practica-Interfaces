using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PracticaObligatoria
{
    public class Data : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string fecha;
        private ObservableCollection<Comidas> comidas;
        private float num_cal_totales;

        // Constructores de la clase dependiendo de los parámetros que se le pasen
        public Data() 
        { 
            comidas = new ObservableCollection<Comidas>(); 
        }

        public Data(string f, ObservableCollection<Comidas> c)
        {
            fecha = f;
            comidas = c;
            num_cal_totales = Calc_Total();
        }

        // Getter & Setter
        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; OnPropertyChanged("Fecha"); }
        }

        public float Cal_total
        {
            get { return num_cal_totales; }
            set
            {
                num_cal_totales = value;
                OnPropertyChanged("Cal_total");
            }
        }

        public ObservableCollection<Comidas> Lista
        {
            get { return comidas; }
            set { comidas = value; OnPropertyChanged("Lista"); Cal_total = Calc_Total(); }
        }

        // Función auxiliar
        private float Calc_Total()
        {
            float aux = 0f;
            foreach(Comidas c in comidas)
            {
                aux += c.Cal_Comida;
            }
            return aux;
        }

        // Implementación de la interfaz para notificar el cambio de un valor
        protected void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }

    // Clase auxiliar usada para representar cada tipo de comida en un día específico 
    public class Comidas : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string comida;
        private float calorias;

        // Constructor de la clase
        public Comidas(string com, float cal)
        {
            comida = com;
            calorias = cal;
        }

        // Getter & Setter
        public string Comida
        {
            get { return comida; }
            set { comida = value; OnPropertyChanged("Comida"); }
        }

        public float Cal_Comida
        {
            get { return calorias; }
            set { calorias = value; OnPropertyChanged("Cal_Comida"); }
        }

        // Implementación de la interfaz para notificar el cambio de un valor
        protected void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
