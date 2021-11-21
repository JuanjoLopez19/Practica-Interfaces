using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PracticaObligatoria
{
    // Clase utilizada como modelo de la vista siguiendo el formato M V VM
    public class Model : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Data> MyData { get; }
        public ObservableCollection<Comidas> comidas = null;
        public float cal_total;
        public float media_cal;
        
        // Constructor de la clase
        public Model()
        {
            MyData = new ObservableCollection<Data>();
        }

        // Getter & Setter
        public float Media_Cal
        {
            get {return media_cal; }
            set { media_cal = value; OnPropertyChanged("Media_Cal"); }
        }

        // Funciones para añadir los valores en los atributos de la clase
        public bool Add(Data d)
        {
            foreach (Data aux in MyData)
            {
                if (aux.Fecha.Equals(d.Fecha))
                    return false;
            }
            MyData.Add(d);
            CalcMedia();
            return true;
        }

        public bool Delete(Data d)
        {
            foreach (Data aux in MyData)
            {
                if (aux.Fecha.Equals(d.Fecha))
                {
                    MyData.Remove(d);
                    CalcMedia();
                    return true;
                }
               
            }
            return false;
        }

        public void Draw_diary(ObservableCollection<Comidas> c, float cal)
        {
            comidas = c;
            cal_total = cal;
        }

        // Función auxiliar
        private void CalcMedia()
        {

            if (MyData.Count != 0)
            {
                float aux = 0;
                foreach (Data d in MyData)
                    aux += d.Cal_total;
                Media_Cal = aux / MyData.Count;
            }
            else
                Media_Cal = 0;
        }

        // Implementación de la interfaz para notificar el cambio de las propiedades
        protected void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
