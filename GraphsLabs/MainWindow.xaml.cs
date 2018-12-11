using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GraphsLabs
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
		private Graph graph1;
		private Graph graph2;
		private Graph graph3;
		private bool firstSetting;

		public MainWindow()
        {
			InitializeComponent();
		}

		#region Лабораторная 1
		private void SetGraphsButton_Click(object sender, RoutedEventArgs e)
		{
			int dimension = Convert.ToInt32(DimensionTextBox.Text);
			graph1 = new Graph(dimension);
			graph2 = new Graph(dimension);
			graph1.AdjMatrix.Random();
			graph2.AdjMatrix.Random();
			Graph1DataGrid.DataContext = graph1.AdjMatrix;
			Graph2DataGrid.DataContext = graph2.AdjMatrix;
			if (!firstSetting)
			{
				CalculateOutdegreeButton.IsEnabled = true;
				GetG3Button.IsEnabled = true;
				firstSetting = true;
			}
			else
			{
				OutdegreeG1TextBox.Text = string.Empty;
				OutdegreeG2TextBox.Text = string.Empty;
			}
		}

		private void CalculateOutdegreeButton_Click(object sender, RoutedEventArgs e)
		{
			int vertexIndex = Convert.ToInt32(VertexTextBox.Text) - 1;
			OutdegreeG1TextBox.Text = graph1.CalculateOutdegree(vertexIndex).ToString();
			OutdegreeG2TextBox.Text = graph2.CalculateOutdegree(vertexIndex).ToString();
		}

		private void GetG3Button_Click(object sender, RoutedEventArgs e)
		{
			graph3 = Graph.GetUnionGraph(graph1, graph2);
			Graph3DataGrid.DataContext = graph3.AdjMatrix;
		}
		#endregion

		#region Лабораторная 2
		private void SetGraphsButton_Click2(object sender, RoutedEventArgs e)
        {
			int dimension = Convert.ToInt32(DimensionTextBox2.Text); //Достаём размерность матрицы
            Graph graph = new Graph(dimension); //Создаём новый граф через матрицу смежности
			graph.AdjMatrix.Random(); //Заполняем матрицу смежности случайными связями.
            Graph1DataGrid2.DataContext = graph.AdjMatrix; //Заполняем таблицу для матрицы смежности
			graph.IncMatrix = graph.FormIncByAdj(); //Формируем матрицу инцидентности по матрице смежности
			Graph2DataGrid2.DataContext = graph.IncMatrix; //Заполняем таблицу для матрицы инцидентности
			equals_text.Text = graph.EqualsVertices(Graph.MatrixType.Incidence); //Находим вершины с одинаковыми полустепенями исхода и захода по матрице инцидентности
        }
		#endregion

		#region Лабораторная 3
		private void SetGraphsButton3_Click(object sender, RoutedEventArgs e)
		{
			Graph graph = new Graph(Convert.ToInt32(DimensionTextBox3.Text)); //Задаём граф
			graph.AdjMatrix.Random(); //Рандомим матрицу
			Graph1DataGrid3.DataContext = graph.AdjMatrix; //Добавляем данные в датагрид
			IsCompleteGraphTextBlock.Text = graph.IsCompleteGraph() ? "Да" : "Нет"; //Проверяем на полноту графа

			Graph graphTest = new Graph(6)
			{
				AdjMatrix = new AdjacencyMatrix()
			};
			IsCompleteGraphTextBlock.Text += "\n" + graphTest.GetTransitiveClosure(0, Graph.Moving.Direct);
			IsCompleteGraphTextBlock.Text += "\n" + graphTest.GetTransitiveClosure(1, Graph.Moving.Reverse);
			IsCompleteGraphTextBlock.Text += "\n" + graphTest.ReachabilityMatrix().ToString();
			IsCompleteGraphTextBlock.Text += "\n" + graphTest.IsStronglyConnected();
			IsCompleteGraphTextBlock.Text += "\n" + graphTest.CMatrix().ToString();
			IsCompleteGraphTextBlock.Text += "\n" + graphTest.MaxStronglyConnectedSubgraphs().ToString();
		}
		#endregion

		#region Лабораторная 4
		private void RadioButtons4_Checked(object sender, RoutedEventArgs e)
		{
			switch ((sender as RadioButton).Tag)
			{
				case "SeqRadButt4":
					Graph1DataGrid4.Visibility = Visibility.Visible;
					DimensionSlider4.Minimum = 5;
					DimensionSlider4.Maximum = 10;
					DimensionSlider4.Value = 5;
					DimensionSlider4.SmallChange = 1;
					DimensionSlider4.LargeChange = 1;
					DimensionSlider4.TickFrequency = 1;
					break;
				case "ParRadButt4":
					Graph1DataGrid4.Visibility = Visibility.Collapsed;
					DimensionSlider4.Minimum = 100;
					DimensionSlider4.Maximum = 5000;
					DimensionSlider4.Value = 100;
					DimensionSlider4.SmallChange = 100;
					DimensionSlider4.LargeChange = 100;
					DimensionSlider4.TickFrequency = 100;
					break;
			}
		}

		private void SetGraphsButton4_Click(object sender, RoutedEventArgs e)
		{
			int taskVertex = 0; //x1
			Graph graph = new Graph(Convert.ToInt32(DimensionTextBox4.Text)); //Задаём граф
			Stopwatch sw = new Stopwatch();
			graph.AdjMatrix.Random(); //Рандомим матрицу
			if (Graph1DataGrid4.IsVisible)
				Graph1DataGrid4.DataContext = graph.AdjMatrix; //Добавляем данные в датагрид
			//TimesTextBlock1.Text = sw.Elapsed.ToString();

			//using (var streamWriter = new System.IO.StreamWriter("4seq.txt"))
			//{
			//	streamWriter.Write(graph.AdjMatrix.ToString());
			//}
			//using (var proc = new Process() { StartInfo = new ProcessStartInfo("C:\\Windows\\System32\\notepad.exe", "4seq.txt") })
			//	proc.Start();
			sw.Start();
			DirectTransitiveClosureTextBox.Text = graph.GetTransitiveClosure(taskVertex, Graph.Moving.Direct);
			ReverseTransitiveClosureTextBox.Text = graph.GetTransitiveClosure(taskVertex, Graph.Moving.Reverse);
			sw.Stop();
			TimesTextBlock1.Text = sw.ElapsedMilliseconds.ToString() + " ms";
			sw.Restart();
			DirectTransitiveClosureTextBox.Text = graph.GetTransitiveClosureParallel(taskVertex, Graph.Moving.Direct);
			ReverseTransitiveClosureTextBox.Text = graph.GetTransitiveClosureParallel(taskVertex, Graph.Moving.Reverse);
			sw.Stop();
			TimesParallelTextBlock1.Text = sw.ElapsedMilliseconds.ToString() + " ms";
			IsStronglyConnectedGraphTextBlock.Text = graph.IsStronglyConnected() ? "Да" : "Нет";
		}
		#endregion

		#region Лабораторная 5
		private void SetGraphsButton5_Click(object sender, RoutedEventArgs e)
		{
			Graph graph1 = new Graph(Convert.ToInt32(DimensionTextBox5.Text)); //Задаём граф
			graph1.AdjMatrix.Random(); //Рандомим матрицу
			Graph1DataGrid5.DataContext = graph1.AdjMatrix; //Добавляем данные в датагрид

			Graph graph2 = new Graph(Convert.ToInt32(DimensionTextBox5.Text)); //Задаём граф
			graph2.AdjMatrix.Random(); //Рандомим матрицу
			Graph2DataGrid5.DataContext = graph2.AdjMatrix; //Добавляем данные в датагрид

			Graph graph3 = Graph.GetIntersectionGraph(graph1, graph2); //Задаём граф
			Graph3DataGrid5.DataContext = graph3.AdjMatrix; //Добавляем данные в датагрид

			//Graph graphTest = new Graph(6)
			//{
			//	AdjMatrix = new AdjacencyMatrix()
			//};
			Graph4DataGrid5.DataContext = new AdjacencyMatrix(graph3.ReachabilityMatrix());
			Graph5DataGrid5.DataContext = new AdjacencyMatrix(graph3.CounterReachabilityMatrix());
			Graph6DataGrid5.DataContext = new AdjacencyMatrix(graph3.CMatrix());
			Graph7DataGrid5.DataContext = graph3.MaxStronglyConnectedSubgraphs(); //Добавляем данные в датагрид
		}
		#endregion

		private void Button6_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}