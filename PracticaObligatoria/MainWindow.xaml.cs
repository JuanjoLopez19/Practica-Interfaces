using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PracticaObligatoria
{
    /// <summary>
    /// Código C# asociado a MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        private Line ejeX, ejeY;
        private Cuadro cuadro;
        private Extras extras;
        private Model modelo;
        private VentanaGraficos vg;
        private bool flag = false;
        
        // Constructo de la clase 
        public MainWindow()
        {
            InitializeComponent();
            cuadro = new Cuadro();
            extras = new Extras();
            modelo = new Model();
            vg = new VentanaGraficos();
        }

        // Manejadoras de los eventos definidos en el xaml
        private void Registro_Click(object sender, RoutedEventArgs e)
        {
            cuadro.Owner = this;
            cuadro.AddDataHandler += cd_AddData;
            cuadro.ChangeDataHandler += cd_ChangeData;
            cuadro.DeleteDataHandler += cd_DeleteDataHandler;
            cuadro.SwipeHandler += Cuadro_SwipeHandler;
            cuadro.Show();
        }

        private void Extras_Click(object sender, RoutedEventArgs e)
        {
            extras.Owner = this;
            extras.Calorias = modelo.media_cal;
            extras.Show();
        }

        private void Gráficos_Click(object sender, RoutedEventArgs e)
        {
            vg.Owner = this;
            vg.Datos = modelo.MyData;
            vg.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            flag = false;
            Vuelta.IsEnabled = false;
            RedrawRect();
            cuadro.Registro.SelectedIndex = -1;

        }

        private void Main_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!flag)
                RedrawRect();
            else
                DrawDaily(modelo.comidas);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cuadro = null;
            extras = null;
            modelo = null;
            vg = null;
            e.Cancel = false;
        }

        // Funciones que manejan toda la parte del dibujado en el canvas
        private void DrawRect(ObservableCollection<Data> datos)
        {
            float aux = 0;
            double percentage, maxCanvas, range, per2, tam;
            Line r, r_aux;
            double x1 = 40f, x2 = 40f;

            foreach (Data d in datos)
            { 
                if (d.Cal_total > aux)
                    aux = d.Cal_total;
            }

            if (aux != 0)
            {
                if (datos.Count != 0)
                {
                    MaxCal.Visibility = Visibility.Visible;
                    MaxCal.Content = aux;
                    Canvas.SetBottom(MaxCal, 0);
                    Canvas.SetLeft(MaxCal, 10);
                    MaxCal_2.Visibility = Visibility.Visible;
                    MaxCal_2.Content = aux / 2;
                    Canvas.SetBottom(MaxCal_2, Lienzo.ActualHeight / 2);
                    Canvas.SetLeft(MaxCal_2, 10);
                    RotateTransform rt = new RotateTransform();
                    rt.CenterX = 0.5;
                    rt.CenterY = 0.5;
                    rt.Angle += -90;
                    MaxCal_2.RenderTransform = rt;
                }
            }
            else
            {
                MaxCal.Visibility = Visibility.Hidden;
                MaxCal_2.Visibility = Visibility.Hidden;
                bt_grafs.IsEnabled = false;
            }

            foreach (Data d in datos)
            {
                percentage = d.Cal_total / aux;
                maxCanvas = Lienzo.ActualHeight - (Lienzo.ActualHeight * percentage);
                range = Lienzo.ActualHeight - maxCanvas;
                per2 = d.Lista[0].Cal_Comida / d.Cal_total;
                tam = range * per2;
                r = new Line();
                r.X1 = x1;
                r.X2 = x2;
                r.Y1 = Lienzo.ActualHeight - tam;
                r.Y2 = Lienzo.ActualHeight;
                r.StrokeThickness = Lienzo.ActualWidth / 20 - datos.Count();
                r.Stroke = Select_color(d.Lista[0].Comida);
                Lienzo.Children.Add(r);
                for (int i = 1; i < d.Lista.Count(); i++) 
                {
                    per2 = d.Lista[i].Cal_Comida / d.Cal_total;
                    tam = range * per2;
                    r_aux = r;
                    r = new Line();
                    r.X1 = r_aux.X1;
                    r.X2 = r_aux.X2;
                    r.Y1 = r_aux.Y1 - tam;
                    r.Y2 = r_aux.Y1;
                    r.StrokeThickness = Lienzo.ActualWidth / 20 - datos.Count();
                    r.Stroke = Select_color(d.Lista[i].Comida);
                    Lienzo.Children.Add(r);
                }
                TextBlock tb = new TextBlock();
                tb.Text = d.Fecha;
                RotateTransform rt = new RotateTransform();
                rt.CenterX = 0.5;
                rt.CenterY = 0.5;
                rt.Angle += -20;
                tb.RenderTransform = rt;
                Canvas.SetBottom(tb, range);
                Canvas.SetLeft(tb, r.X1 - r.StrokeThickness / 2);
                Lienzo.Children.Add(tb);
                x1 = r.X1 + Lienzo.ActualWidth / datos.Count; 
                x2 = r.X2 + Lienzo.ActualWidth / datos.Count;
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
            ejeX.Y1 = Lienzo.ActualHeight;
            ejeX.Y2 = ejeX.Y1;
            Lienzo.Children.Add(ejeX);


            ejeY.Stroke = Brushes.Black;
            ejeY.StrokeThickness = 2;
            ejeY.X1 = 0;
            ejeY.X2 = 0;
            ejeY.Y1 = 0;
            ejeY.Y2 = Lienzo.ActualHeight;
            Lienzo.Children.Add(ejeY);
        }

        private void DrawDaily(ObservableCollection<Comidas> datos)
        {
            Lienzo.Children.Clear();
            DrawAxis();
            float aux = 0;
            double percentage, maxCanvas, range, per2, tam;
            Line r;
            double x = 40f, auy1 = Lienzo.ActualHeight;

            foreach (Comidas c in datos)
                if (c.Cal_Comida > aux)
                    aux = c.Cal_Comida;

            if (aux != 0)
            {
                if (datos.Count != 0)
                {
                    MaxCal.Content = aux;
                    MaxCal_2.Content = aux / 2;
                }
            }
            else
            {
                MaxCal.Visibility = Visibility.Hidden;
                MaxCal_2.Visibility = Visibility.Hidden;
                bt_grafs.IsEnabled = false;
            }

            foreach (Comidas c in datos)
            {
                percentage = modelo.cal_total / aux ;
                maxCanvas = auy1 - (auy1 * percentage);
                range = auy1 - maxCanvas;
                per2 = c.Cal_Comida / modelo.cal_total; // calorias de la comida / calorias totales del dia
                tam = range * per2;

                r = new Line();
                r.X1 = x;
                r.X2 = x;
                r.Y1 = auy1 - tam;
                r.Y2 = auy1;
                r.Stroke = Select_color(c.Comida);
                r.StrokeThickness = Lienzo.ActualWidth / 20 - datos.Count();
                TextBlock tb = new TextBlock();
                tb.Text = c.Comida;
                Canvas.SetBottom(tb, -15);
                Canvas.SetLeft(tb, r.X1 - r.StrokeThickness / 2);
                Lienzo.Children.Add(tb);

                tb = new TextBlock();
                tb.Text = c.Cal_Comida + " Kcal";
                Canvas.SetBottom(tb, tam + 10);
                Canvas.SetLeft(tb, r.X1 - r.StrokeThickness / 2);
                Lienzo.Children.Add(tb);

                Lienzo.Children.Add(r);
                x = (float)r.X1 + (float)Lienzo.ActualWidth / datos.Count;
            }
        }

        private void RedrawRect()
        {
            Lienzo.Children.Clear();
            titleX.Visibility = Visibility.Visible;
            titleY.Visibility = Visibility.Visible;
            DrawRect(modelo.MyData);
            DrawAxis();
        }

        // Manejadoras de los eventos que se definen para cada instancia de la ventanas auxiliares del programa
        private void Cuadro_SwipeHandler(object sender, SwipeDiagramsEventArgs e)
        {
            flag = e.flag;
            Vuelta.IsEnabled = false;
            RedrawRect();
        }

        void cd_AddData(object sender, AddDataEventArgs e)
        {
            if (!bt_grafs.IsEnabled)
                bt_grafs.IsEnabled = true;
            if (modelo.Add(e.d) && !flag)
                RedrawRect();
        }

        void cd_ChangeData(object sender, ChangeDataEventArgs e)
        {
            modelo.Draw_diary(e.c, e.cal_total);
            flag = true;
            Vuelta.IsEnabled = true;
            DrawDaily(modelo.comidas);
        }

        private void cd_DeleteDataHandler(object sender, DeleteDataEventArgs e)
        {
            if (modelo.Delete(e.d))
            {
                flag = false;
                Vuelta.IsEnabled = false;
                RedrawRect();
            }
            else
                return;
        }

        // Función auxiliares para dibujar las rectas
        private SolidColorBrush Select_color(string meal)
        {
            if (meal.Equals("Desayuno"))
                return new SolidColorBrush(Color.FromRgb(102, 84, 94));
            else if (meal.Equals("Almuerzo"))
                return new SolidColorBrush(Color.FromRgb(163, 145, 147));
            else if (meal.Equals("Comida"))
                return new SolidColorBrush(Color.FromRgb(170, 111, 115));
            else if (meal.Equals("Merienda"))
                return new SolidColorBrush(Color.FromRgb(238, 169, 144));
            else if (meal.Equals("Cena"))
                return new SolidColorBrush(Color.FromRgb(246, 224, 181));
            else if (meal.Equals("Otros"))
                return new SolidColorBrush(Color.FromRgb(84, 102, 92));
            else
                return new SolidColorBrush(Color.FromRgb(0, 0, 0));
        } 
    }

    
}