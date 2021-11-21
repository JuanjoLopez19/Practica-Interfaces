using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Código C# asociado a VentanaGráficos.xaml
    /// </summary>
    public partial class VentanaGraficos : Window
    {
        private ObservableCollection<Data> d;
        private List<float> num_total;
        private float cal_total;
        private List<Formato> estilos;
        private Line ejeX,ejeY;
        private bool flag = true;

        // Constructor de la clase
        public VentanaGraficos()
        {
            InitializeComponent();
            num_total = new List<float>();
            cal_total = 0;
            FactoryEstilos();
            
        }

        // Getter & Setters
        public ObservableCollection<Data> Datos
        {
            get { return d; }
            set 
            { 
                d = value; 
                var l = d.OrderBy(x => DateTime.Parse(x.Fecha)).ToList();
                d.Clear();
                foreach (var aux in l)
                    d.Add(aux);
                CalcValores();
            }
        }

        public List<float> Total
        {
            get { return num_total; }
            set { num_total = value; }
        }


        // Manejadoras de los eventos definidos en el Xaml
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (flag)
                Draw();
            else
                DrawGrafLine();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            byte blue, green, red;
            Slider sl;
            TextBlock tb;
            sl = Slider_red;
            tb = txt_red;
            if (sl == null || tb == null)
                return;
            else
            {
                red = (byte)sl.Value;
                tb.Text = sl.Value.ToString("F");
            }

            sl = Slider_green;
            tb = txt_green;
            if (sl == null || tb == null)
                return;
            else
            {
                green = (byte)sl.Value;
                tb.Text = sl.Value.ToString("F");
            }

            sl = Slider_blue;
            tb = txt_blue;
            if (sl == null || tb == null)
                return;
            else
            {
                blue = (byte)sl.Value;
                tb.Text = sl.Value.ToString("F");
            }
            Color c = Color.FromRgb(red, green, blue);

            switch (Desplegable.Text)
            {
                case "Desayuno":
                    estilos[0].rgb = new SolidColorBrush(c);
                    break;
                case "Almuerzo":
                    estilos[1].rgb = new SolidColorBrush(c);
                    break;
                case "Comida":
                    estilos[2].rgb = new SolidColorBrush(c);
                    break;
                case "Merienda":
                    estilos[3].rgb = new SolidColorBrush(c);
                    break;
                case "Cena":
                    estilos[4].rgb = new SolidColorBrush(c);
                    break;
                case "Otros":
                    estilos[5].rgb = new SolidColorBrush(c);
                    break;
                default:
                    return;

            }
            if (!flag)
                DrawGrafLine();
            else
                Draw();
        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush c;
            if (sender == Des)
            {
                c = estilos[0].rgb;
                Desplegable.Text = "Desayuno";
            }
            else if (sender == Alm)
            {
                c = estilos[1].rgb;
                Desplegable.Text = "Almuerzo";
            }
            else if (sender == Com)
            {
                c = estilos[2].rgb;
                Desplegable.Text = "Comida";
            }
            else if (sender == Mer)
            {
                c = estilos[3].rgb;
                Desplegable.Text = "Merienda";
            }
            else if (sender == Cen)
            {
                c = estilos[4].rgb;
                Desplegable.Text = "Cena";
            }
            else if (sender == Otr)
            {
                c = estilos[5].rgb;
                Desplegable.Text = "Otros";
            }
            else
                return;

            Slider_red.Value = (double)c.Color.R;
            Slider_green.Value = (double)c.Color.G;
            Slider_blue.Value = (double)c.Color.B;
            Desplegable.IsDropDownOpen = false;
        }

        private void bt_change_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                flag = false;
                DrawGrafLine();
                lbl_1.Content = "Gráfico de líneas: evolución temporal de las comidas";
            }
            else
            {
                flag = true;
                Draw();
                lbl_1.Content = "Gráfico de barras: total de calorias por comida";
            }
        }
        

        // Funciones que definen el comportamiento de dibujado de los gráficos 
        private void Draw()
        {
            Lienzo.Children.Clear();
            DrawAxis();
            float aux = 0;
            double percentage, maxCanvas, range, tam;
            Line r;
            double auxX = 40f, auxY = Lienzo.ActualHeight - 10;

            foreach (float d in num_total)
                if (d > aux)
                    aux = d;

           for (int i = 0; i < num_total.Count; i++)
            {
                percentage = num_total[i] / aux;
                maxCanvas = auxY - (auxY * percentage);
                range = auxY - maxCanvas;
                //per2 = num_total[i] / num_total[i];
                tam = range;
                r = new Line();
                r.X1 = auxX;
                r.X2 = auxX;
                r.Y1 = auxY - tam;
                r.Y2 = auxY;
                r.Stroke = estilos[i].rgb;
                r.StrokeThickness = Lienzo.ActualWidth / 20 - num_total.Count();

                TextBlock tb = new TextBlock();
                tb.Text = estilos[i].Tipo;
                Canvas.SetBottom(tb, -10);
                Canvas.SetLeft(tb, r.X1 - r.StrokeThickness / 2);
                Lienzo.Children.Add(tb);

                tb = new TextBlock();
                tb.Text = num_total[i] + " Kcal";
                Canvas.SetBottom(tb, tam + 10);
                Canvas.SetLeft(tb, r.X1 - r.StrokeThickness / 2);
                Lienzo.Children.Add(tb);

                Lienzo.Children.Add(r);
                auxX = (float)r.X1 + (float)Lienzo.ActualWidth / num_total.Count;
            }
        }

        private void DrawAxis()
        {
            ejeX = new Line();
            ejeY = new Line();

            ejeX.Stroke = Brushes.Black;
            ejeX.StrokeThickness = 2;
            ejeX.X1 = 0;
            ejeX.X2 = Lienzo.ActualWidth;
            ejeX.Y1 = Lienzo.ActualHeight - 10;
            ejeX.Y2 = ejeX.Y1;
            Lienzo.Children.Add(ejeX);


            ejeY.Stroke = Brushes.Black;
            ejeY.StrokeThickness = 2;
            ejeY.X1 = 0;
            ejeY.X2 = 0;
            ejeY.Y1 = 0;
            ejeY.Y2 = Lienzo.ActualHeight - 10 ;
            Lienzo.Children.Add(ejeY);
        }

        private void DrawGrafLine()
        {
            Lienzo.Children.Clear();
            DrawAxis();
            double max = 0;
            double div, step;
            double per1, per2;
            Polyline p;
            Point punto;
            TextBlock tb, tb2;
            Ellipse el;
            foreach(Data aux in d)
                foreach(Comidas c in aux.Lista)
                {
                    if (c.Cal_Comida > max)
                        max = c.Cal_Comida;
                }
            div = Lienzo.ActualWidth / d.Count; 
            step = 0;
            for (int i = 0; i < 6; i++) 
            {
                p = new Polyline();
                foreach (Data aux in d)
                { 
                    el = new Ellipse();
                    per1 = aux.Lista[i].Cal_Comida / max; 
                    if (per1 <= 0.05)
                        per1 = 0;
                    per2 = Lienzo.ActualHeight * (1 - per1);
                    if (per2 == Lienzo.ActualHeight)
                        per2 = Lienzo.ActualHeight - 10;
                    punto = new Point(step, per2);

                    el.Width = 10;
                    el.Height = 10;
                    el.Fill = estilos[i].rgb;
                    el.StrokeThickness = 2;
                    Canvas.SetLeft(el, step);
                    Canvas.SetTop(el, per2 - 3);
                    p.Points.Add(punto);

                    tb2 = new TextBlock();
                    tb2.Text = aux.Lista[i].Cal_Comida.ToString() + "Kcal";
                    Canvas.SetLeft(tb2, step);
                    if(per2 == Lienzo.ActualHeight - 10)
                        Canvas.SetTop(tb2, per2 - 20);
                    else
                        Canvas.SetTop(tb2, per2 + 5);
                    step += div;
                    Lienzo.Children.Add(el);
                    Lienzo.Children.Add(tb2);

                }

                p.StrokeThickness = 2;
                p.Stroke = estilos[i].rgb;
                Lienzo.Children.Add(p);
                step = 0;
            }
            foreach(Data aux in d)
            {
                tb = new TextBlock();
                tb.Text = aux.Fecha;
                Canvas.SetBottom(tb, -10);
                Canvas.SetLeft(tb, step);
                Lienzo.Children.Add(tb);
                step += div;
            }
            
        }

        // Funciones auxiliares
        public void FactoryEstilos()
        {
            List<Formato> aux = new List<Formato>();
            Formato f;
            f = new Formato("Desayuno", 237, 85, 85);
            aux.Add(f);
            f = new Formato("Almuerzo", 180, 196, 104);
            aux.Add(f);
            f = new Formato("Comida", 184, 167, 234);
            aux.Add(f);
            f = new Formato("Merienda", 183, 224, 220);
            aux.Add(f);
            f = new Formato("Cena", 255, 128, 0);
            aux.Add(f);
            f = new Formato("Otros", 1, 158, 255);
            aux.Add(f);

            estilos = aux;
        }

        private void CalcValores()
        {
            Total.Clear();
            float aux;
            for (int i = 0; i < 6; i++)
            {
                aux = 0;
                foreach (Data a in d)
                {
                    aux += a.Lista[i].Cal_Comida;
                }
                cal_total += aux;
                Total.Add(aux);
            }
            Total_Cal.Text = "Total: " + cal_total.ToString() + " kcal";
            Draw();
        }



        // Clase auxiliar para almacenar los colores
        public class Formato : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;
            private string tipo_comida;
            private SolidColorBrush color;

            // Constructor de la clase
            public Formato(string s, byte r, byte g, byte b)
            {
                Tipo = s;
                Color c = Color.FromRgb(r, g, b);
                color = new SolidColorBrush(c);
            }

            // Getter & Setters
            public SolidColorBrush rgb
            {
                get { return color; }
                set
                {
                    color = value;
                    OnPropertyChanged("rgb");
                }
            }

            public string Tipo
            {
                get { return tipo_comida; }
                set
                {
                    tipo_comida = value;
                    OnPropertyChanged("Tipo");
                }
            }

            // Función que para implementar la interfaz de cambio
            protected void OnPropertyChanged(string propertyname)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
