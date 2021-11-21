using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace PracticaObligatoria
{
    /// <summary>
    /// Código C# asociado a Cuadro.xaml
    /// </summary>
    
    // Clases auxiliares utilizadas para la delegación de eventos
    public class AddDataEventArgs : EventArgs { public Data d; }
    public class ChangeDataEventArgs : EventArgs { public ObservableCollection<Comidas> c; public float cal_total; }
    public class DeleteDataEventArgs : EventArgs { public Data d; }
    public class SwipeDiagramsEventArgs : EventArgs { public bool flag; }

    public partial class Cuadro : Window
    {
        // Manejadores de las clases eventos definidas previamente
        public event EventHandler<AddDataEventArgs> AddDataHandler;
        public event EventHandler<ChangeDataEventArgs> ChangeDataHandler;
        public event EventHandler<DeleteDataEventArgs> DeleteDataHandler;
        public event EventHandler<SwipeDiagramsEventArgs> SwipeHandler;

        private CuadroMod modificador;
        private ObservableCollection<Data> MyData;

        // Constructor de la clase
        public Cuadro()
        {
            InitializeComponent();
            MyData = new ObservableCollection<Data>();
            modificador = null;
            Registro.ItemsSource = MyData;
        }

        // Manejadoras de los eventos
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        private void bt_add_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Comidas> c = new ObservableCollection<Comidas>();
            Comidas aux;
            Data d;
            string fecha;
            AddDataEventArgs args;
            bool flag =true;
            if (Calendario.SelectedDate.HasValue)
            {
                fecha = Calendario.SelectedDate.ToString().Substring(0, 10);
                if (!CheckDate(fecha))
                    flag = false;
                if (flag)
                {
                    if (!Box_Des.isEmpty())
                    {
                        aux = new Comidas("Desayuno", (float)Box_Des.doubleValue);
                        c.Add(aux);
                        if (!Box_Alm.isEmpty())
                        {
                            aux = new Comidas("Almuerzo", (float)Box_Alm.doubleValue);
                            c.Add(aux);
                            if (!Box_Com.isEmpty())
                            {
                                aux = new Comidas("Comida", (float)Box_Com.doubleValue);
                                c.Add(aux);
                                if (!Box_Mer.isEmpty())
                                {
                                    aux = new Comidas("Merienda", (float)Box_Mer.doubleValue);
                                    c.Add(aux);
                                    if (!Box_Cena.isEmpty())
                                    {
                                        aux = new Comidas("Cena", (float)Box_Cena.doubleValue);
                                        c.Add(aux);
                                        if (!Box_Otros.isEmpty())
                                        {
                                            aux = new Comidas("Otros", (float)Box_Otros.doubleValue);
                                            c.Add(aux);
                                            d = new Data(fecha, c);
                                            MyData.Add(d);
                                            args = new AddDataEventArgs();
                                            args.d = d;
                                            OnAddData(args);
                                            SetBlackOutDates();
                                            Output.Text = "Datos introducidos correctamente";
                                        }
                                        else
                                            Output.Text = "Introduce datos en los campos";
                                    }
                                    else
                                        Output.Text = "Introduce datos en los campos";
                                }
                                else
                                    Output.Text = "Introduce datos en los campos";
                            }
                            else
                                Output.Text = "Introduce datos en los campos";
                        }
                        else
                            Output.Text = "Introduce datos en los campos";
                    }
                    else
                        Output.Text = "Introduce datos en los campos";
                }
                else
                    Output.Text = "Selecciona una fecha que no este repetida";
            }
            else
                Output.Text = "Selecciona una fecha válida";

        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Output.Text = "";
            Selected_Date.Text = Calendario.SelectedDate.ToString().Substring(0, 10);
        }

        private void Registro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Registro.SelectedIndex < 0)
            {
                Diario.ItemsSource = null;
                Eliminar_bt.IsEnabled = false;
                Editar_bt.IsEnabled = false;
                SwipeDiagramsEventArgs args = new SwipeDiagramsEventArgs();
                args.flag = false;
                OnSwipe(args);
            }
            else
            {
                Eliminar_bt.IsEnabled = true;
                Editar_bt.IsEnabled = true;
                Data d = MyData[Registro.SelectedIndex];
                Diario.ItemsSource = d.Lista;
                ChangeDataEventArgs args = new ChangeDataEventArgs();
                args.c = d.Lista;
                args.cal_total = d.Cal_total;
                OnChangeData(args);
                
            }
        }

        private void Eliminar_bt_Click(object sender, RoutedEventArgs e)
        {
            int index = Registro.SelectedIndex;
            if (index >= 0)
            {
                DeleteDataEventArgs args = new DeleteDataEventArgs();
                args.d = MyData[index];
                MyData.RemoveAt(index);
                OnDeleteData(args);
                Calendario.BlackoutDates.Clear();
                SetBlackOutDates();
            }
            else
                return;
        }

        private void Editar_bt_Click(object sender, RoutedEventArgs e)
        {
            int index = Registro.SelectedIndex;
            if (index >= 0)
            {
                modificador = new CuadroMod();
                modificador.Owner = this;
                modificador.Comidas = MyData[index].Lista;
                modificador.ShowDialog();
                if (modificador.DialogResult == true)
                {
                    MyData[index].Lista = modificador.Comidas;
                    ChangeDataEventArgs args = new ChangeDataEventArgs();
                    args.c = MyData[index].Lista;
                    args.cal_total = MyData[index].Cal_total;
                    OnChangeData(args);
                }
            }
            else
                return;
        }

        private void Export_bt_Click(object sender, RoutedEventArgs e)
        {
            if (MyData.Count != 0)
            {
                Microsoft.Win32.SaveFileDialog cd_save;
                cd_save = new Microsoft.Win32.SaveFileDialog();
                cd_save.FileName = "Registro";
                cd_save.DefaultExt = ".txt";
                cd_save.Filter = "Text documents (.txt)|*.txt";
                if (cd_save.ShowDialog() == true)
                {
                    StreamWriter sw = File.CreateText(cd_save.FileName);
                    foreach (Data d in MyData)
                    {
                        sw.Write(d.Fecha);
                        foreach (Comidas c in d.Lista)
                        {
                            sw.Write("#" + c.Cal_Comida);
                        }
                        sw.WriteLine("");
                    }
                    sw.Close();
                    ShowPrompt(sender, MyData.Count, MyData.Count);
                }
            }
        }

        private void Import_bt_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Comidas> c;
            Comidas aux;
            Data d;
            string fecha;
            string[] a;
            AddDataEventArgs args;
            int tam = MyData.Count;
            Microsoft.Win32.OpenFileDialog cd_open;
            cd_open = new Microsoft.Win32.OpenFileDialog();
            cd_open.FileName = "Registro";
            cd_open.DefaultExt = ".txt";
            cd_open.Filter = "Text documents (.txt)|*.txt";
            if (cd_open.ShowDialog() == true)
            {
                string[] lines = File.ReadAllLines(cd_open.FileName);
                foreach (string l in lines)
                {
                    a = l.Split('#');
                    if (a.Length == 7)
                    {
                        fecha = a[0];
                        if (CheckDate(fecha))
                        {
                            try
                            {
                                c = new ObservableCollection<Comidas>();
                                aux = new Comidas("Desayuno", float.Parse(a[1]));
                                c.Add(aux);
                                aux = new Comidas("Almuerzo", float.Parse(a[2]));
                                c.Add(aux);
                                aux = new Comidas("Comida", float.Parse(a[3]));
                                c.Add(aux);
                                aux = new Comidas("Merienda", float.Parse(a[4]));
                                c.Add(aux);
                                aux = new Comidas("Cena", float.Parse(a[5]));
                                c.Add(aux);
                                aux = new Comidas("Otros", float.Parse(a[6]));
                                c.Add(aux);
                                d = new Data(fecha, c);
                                MyData.Add(d);
                                args = new AddDataEventArgs();
                                args.d = d;
                                OnAddData(args);

                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("No tiene el formato válido el fichero");
                            }
                        }
                        else
                            Console.WriteLine("La fecha está repetida");
                    }
                }
                ShowPrompt(sender, MyData.Count - tam, MyData.Count);
                SetBlackOutDates();
            }
        }


        // Métodos On creados para el correcto manejo de los eventos definidos por el usuario
        protected virtual void OnAddData(AddDataEventArgs e)
        {
            AddDataHandler?.Invoke(this, e);
        }

        protected virtual void OnChangeData(ChangeDataEventArgs e)
        {
            ChangeDataHandler?.Invoke(this, e);
        }

        protected virtual void OnDeleteData(DeleteDataEventArgs e)
        {
            DeleteDataHandler?.Invoke(this, e);
        }

        protected virtual void OnSwipe(SwipeDiagramsEventArgs e)
        {
            SwipeHandler?.Invoke(this, e);
        }


        // Funciones auxiliares para el correcto funcionamiento de la práctica
        private bool CheckDate(string date)
        {
            foreach(Data d in MyData)
            {
                if (d.Fecha.Equals(date))
                    return false;
            }
            return true;
        }

        private void ShowPrompt(object sender,int tam, int max)
        {
            string msg;
            if (sender == Import_bt)
            {
                if (tam != 0)
                {
                    if (tam == max)
                        msg = "El número de días importados es: " + tam;
                    else
                        msg = "El número de días importados es: " + tam + " había " + (max - tam) + " repetidos";
                }
                else
                    msg = "Todos los datos del archivo son fechas repetidas";
                string title = "Aviso";
                MessageBoxButton bts = MessageBoxButton.OK;
                MessageBoxImage image = MessageBoxImage.Information;
                MessageBox.Show(msg, title, bts, image);
            }
            else if(sender == Export_bt)
            {
                msg = "El número de días exportados es: " + tam;
                string title = "Aviso";
                MessageBoxButton bts = MessageBoxButton.OK;
                MessageBoxImage image = MessageBoxImage.Information;
                MessageBox.Show(msg, title, bts, image);
            }
        }

        private void SetBlackOutDates()
        {
            foreach (Data aux in MyData)
            {
                DateTime z = DateTime.Parse(aux.Fecha);
                try
                {
                    Calendario.BlackoutDates.Add(new CalendarDateRange(z));
                    
                }catch(ArgumentOutOfRangeException)
                {}
             }
        }
    }
}