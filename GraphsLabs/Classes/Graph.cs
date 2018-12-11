using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace GraphsLabs
{
	/// <summary>
	/// Класс, реализующий матрицу смежности.
	/// </summary>
	internal class AdjacencyMatrix : Matrix<int>, INotifyPropertyChanged
	{
		public AdjacencyMatrix() :
			base(6, 6)
		{
			matrix = new int[,] { { 0, 1, 1, 0, 0, 0 }, { 0, 1, 0, 0, 1, 0 }, { 0, 0, 0, 0, 0, 0 }, { 0, 0, 1, 0, 0, 0 }, { 1, 0, 0, 1, 0, 0 }, { 1, 0, 0, 0, 1, 1 } };
		}

		/// <summary>
		/// Конструктор матрицы смежности.
		/// </summary>
		/// <param name="dimension">Размерность матрицы.</param>
		public AdjacencyMatrix(int dimension) :
			base(dimension, dimension)
		{
			//Инициализация таблицы DataGrid2D
			Data2D = matrix; //Данные для таблицы
			RowHeaders = new string[dimension]; //Заголовки строк
			ColumnHeaders = new string[dimension]; //Заголовки столбцов
			for (int i = 0; i < dimension; i++) //Инициализация заголовков
			{
				RowHeaders[i] = $"x{i + 1}";
				ColumnHeaders[i] = $"x{i + 1}";
			}
			UpdateData();
		}

		public AdjacencyMatrix(Matrix<int> someMatrix) :
			base(someMatrix.Rows, someMatrix.Columns)
		{
			matrix = someMatrix.MatrixData;
			//Инициализация таблицы DataGrid2D
			Data2D = matrix; //Данные для таблицы
			RowHeaders = new string[Dimension]; //Заголовки строк
			ColumnHeaders = new string[Dimension]; //Заголовки столбцов
			for (int i = 0; i < Dimension; i++) //Инициализация заголовков
			{
				RowHeaders[i] = $"x{i + 1}";
				ColumnHeaders[i] = $"x{i + 1}";
			}
			UpdateData();
		}

		/// <summary>
		/// Событие обновления свойства.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#region Код из пакета Gu.Wpf.DataGrid2D
		private string data;
		public string[] RowHeaders { get; }
		public string[] ColumnHeaders { get; }
		public int[,] Data2D { get; }
		public string Data
		{
			get => data;

			private set
			{
				if (value == data)
				{
					return;
				}
				data = value;
				OnPropertyChanged();
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void UpdateData()
		{
			Data = $"{{{string.Join(", ", Rows_s(Data2D).Select(x => $"{{{string.Join(", ", x)}}}"))}}}";
		}

		private IEnumerable<IEnumerable<T>> Rows_s<T>(T[,] source)
		{
			for (int r = 0; r < source.GetLength(0); r++)
			{
				var row = new List<T>();
				for (int c = 0; c < source.GetLength(1); c++)
				{
					row.Add(source[r, c]);
				}
				yield return row;
			}
		}
		#endregion
	}

	/// <summary>
	/// Класс, реализующий матрицу инцидентности.
	/// </summary>
	internal class IncidenceMatrix : Matrix<int>, INotifyPropertyChanged
	{
		/// <summary>
		/// Конструктор матрицы инцидентности для построения по матрице смежности
		/// </summary>
		/// <param name="rows">Количество вершин (строки матрицы).</param>
		/// <param name="columns">Количество рёбер (столбцы матрицы).</param>
		/// <param name="columnsHeaders">Заголовки рёбер.</param>
		/// <param name="connections">Соединения вершин.</param>
		public IncidenceMatrix(int rows, int columns, string[] columnsHeaders, int[,] connections) :
			base(rows, columns)
		{
			matrix = connections;
			//Инициализация таблицы DataGrid2D
			Data2D = matrix; //Данные для таблицы
			RowHeaders = new string[rows]; //Заголовки строк
			ColumnHeaders = columnsHeaders; //Заголовки столбцов
			for (int i = 0; i < rows; i++) //Инициализация заголовков
				RowHeaders[i] = $"x{i + 1}";
			UpdateData();
		}

		/// <summary>
		/// Событие обновления свойства.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#region Код из пакета Gu.Wpf.DataGrid2D
		private string data;
		public string[] RowHeaders { get; }
		public string[] ColumnHeaders { get; }
		public int[,] Data2D { get; }
		public string Data
		{
			get => data;

			private set
			{
				if (value == data)
				{
					return;
				}
				data = value;
				OnPropertyChanged();
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void UpdateData()
		{
			Data = $"{{{string.Join(", ", Rows_s(Data2D).Select(x => $"{{{string.Join(", ", x)}}}"))}}}";
		}

		private IEnumerable<IEnumerable<T>> Rows_s<T>(T[,] source)
		{
			for (int r = 0; r < source.GetLength(0); r++)
			{
				var row = new List<T>();
				for (int c = 0; c < source.GetLength(1); c++)
				{
					row.Add(source[r, c]);
				}
				yield return row;
			}
		}
		#endregion
	}

	internal class MaxStronglyConnectedSubgraphsMatrix : Matrix<int>, INotifyPropertyChanged
	{
		private int oldIndex;

		public MaxStronglyConnectedSubgraphsMatrix(int dimension) :
			base(dimension, dimension)
		{
			RowHeaders = new string[dimension]; //Заголовки строк
			ColumnHeaders = new string[dimension]; //Заголовки столбцов
		}

		public void Add(Queue<int> vertices)
		{
			int length = oldIndex + vertices.Count;
			for (int i = oldIndex; i < length; i++)
			{
				int vertex = vertices.Dequeue();
				RowHeaders[i] = $"x{vertex + 1}";
				ColumnHeaders[i] = $"x{vertex + 1}";
				for (int j = oldIndex; j < length; j++)
				{
					matrix[i, j] = 1;
				}
			}
			Data2D = matrix;
			oldIndex = length;
			UpdateData();
		}

		/// <summary>
		/// Событие обновления свойства.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#region Код из пакета Gu.Wpf.DataGrid2D
		private string data;
		public string[] RowHeaders { get; private set; }
		public string[] ColumnHeaders { get; private set; }
		public int[,] Data2D { get; private set; }
		public string Data
		{
			get => data;

			private set
			{
				if (value == data)
				{
					return;
				}
				data = value;
				OnPropertyChanged();
			}
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void UpdateData()
		{
			Data = $"{{{string.Join(", ", Rows_s(Data2D).Select(x => $"{{{string.Join(", ", x)}}}"))}}}";
		}

		private IEnumerable<IEnumerable<T>> Rows_s<T>(T[,] source)
		{
			for (int r = 0; r < source.GetLength(0); r++)
			{
				var row = new List<T>();
				for (int c = 0; c < source.GetLength(1); c++)
				{
					row.Add(source[r, c]);
				}
				yield return row;
			}
		}
		#endregion
	}

	/// <summary>
	/// Класс, описывающий граф.
	/// </summary>
	internal partial class Graph
	{
		/// <summary>
		/// Перечислеине для матриц представления графа.
		/// </summary>
		public enum MatrixType
		{
			Adjacency,
			Incidence
		}

		/// <summary>
		/// Представление графа с помощью матрицы смежности.
		/// </summary>
		public AdjacencyMatrix AdjMatrix { get; set; }

		/// <summary>
		/// Представление графа с помощью матрицы инцидентности.
		/// </summary>
		public IncidenceMatrix IncMatrix { get; set; }

		/// <summary>
		/// Задание графа матрицей смежности.
		/// </summary>
		/// <param name="dimension">Размерность матрицы смежности - количество вершин графа.</param>
		public Graph(int dimension)
		{
			AdjMatrix = new AdjacencyMatrix(dimension);
		}

		/// <summary>
		/// Задание графа матрицей инцидентности.
		/// </summary>
		/// <param name="rows">Количество строк матрицы инцидентности - количество вершин графа.</param>
		/// <param name="columns">Количество столбцов матрицы инцидентности - количество рёбер графа</param>
		public Graph(int rows, int columns)
		{
			//IncMatrix = new IncidenceMatrix(rows, columns);
		}

		/// <summary>
		/// Подсчёт полустепени исхода вершины по матрице смежности.
		/// </summary>
		/// <param name="vertexIndex">Индекс вершины в матрице смежности.</param>
		/// <returns>Возвращает полустепень исхода.</returns>
		public int CalculateOutdegree(int vertexIndex)
		{
			int outDegree = 0;
			for (int j = 0; j < AdjMatrix.Dimension; j++)
				if (AdjMatrix[vertexIndex, j] == 1)
					outDegree++;
			return outDegree;
		}

		/// <summary>
		/// Подсчёт полустепени захода вершины по матрице смежности.
		/// </summary>
		/// <param name="vertexIndex">Индекс вершины в матрице смежности.</param>
		/// <returns>Возвращает полустепень захода.</returns>
		public int CalculateIndegree(int vertexIndex)
		{
			int inDegree = 0;
			for (int j = 0; j < AdjMatrix.Dimension; j++)
				if (AdjMatrix[j, vertexIndex] == 1)
					inDegree++;
			return inDegree;
		}

		/// <summary>
		/// Подсчёт количества рёбер графа по матрице.
		/// </summary>
		/// <param name="matrixType">Тип матрицы.</param>
		/// <returns>Возвращает количество рёбер графа.</returns>
		public int EdgesNumber(MatrixType matrixType)
		{
			int edges = 0;
			switch (matrixType)
			{
				case MatrixType.Adjacency:
					for (int i = 0; i < AdjMatrix.Dimension; i++)
						for (int j = 0; j < AdjMatrix.Dimension; j++)
							if (AdjMatrix[i, j] == 1)
								edges++;
					break;
				case MatrixType.Incidence:
					edges = IncMatrix.Columns;
					break;
			}
			return edges;
		}

		/// <summary>
		/// Построение матрицы инцидентности по матрице смежности.
		/// </summary>
		/// <returns>Возвращаеет матрицу инцидентности.</returns>
		public IncidenceMatrix FormIncByAdj()
		{
			int rows = AdjMatrix.Rows;
			int columns = EdgesNumber(MatrixType.Adjacency);
			string[] columnHeaders = new string[columns];
			int[,] connections = new int[rows, columns];
			for (int i = 0, k = 0; i < rows; i++)
				for (int j = 0; j < rows; j++)
					if (AdjMatrix[i, j] == 1)
					{
						columnHeaders[k] = $"x{i + 1} → x{j + 1}";
						if (i != j)
						{
							connections[i, k] = 1;
							connections[j, k] = -1;
						}
						k++;
					}
			return new IncidenceMatrix(rows, columns, columnHeaders, connections);
		}

		/// <summary>
		/// Метод вывода вершин с равным количеством полустепеней исходов и заходов.
		/// </summary>
		/// <param name="matrixType">Тип матрицы.</param>
		/// <returns>Возвращает строку с вершинами.</returns>
		public string EqualsVertices(MatrixType matrixType)
		{
			string res = "";
			switch (matrixType)
			{
				case MatrixType.Adjacency:
					for (int i = 0; i < AdjMatrix.Dimension; i++)
						if (CalculateOutdegree(i) == CalculateIndegree(i))
							res += IncMatrix.RowHeaders[i] + " ";
					break;
				case MatrixType.Incidence:
					{
						int sum = 0;
						for (int i = 0; i < IncMatrix.Rows; i++)
						{
							sum = 0;
							for (int j = 0; j < IncMatrix.Columns; j++)
								sum += IncMatrix[i, j];
							if (sum == 0)
								res += IncMatrix.RowHeaders[i] + " ";
						}
					}
					break;
			}
			return res;
		}

		/// <summary>
		/// Проверка полноты графа.
		/// </summary>
		/// <returns>Возвращает true, когда граф является полным, иначе - false.</returns>
		public bool IsCompleteGraph()
		{
			bool res = false;
			for (int i = 0; i < AdjMatrix.Dimension - 1; i++)
			{
				for (int j = i + 1; j < AdjMatrix.Dimension; j++)
				{
					res = (AdjMatrix[i, j] | AdjMatrix[j, i]) == 1;
					if (!res)
					{
						return res;
					}
				}
			}
			return res;
		}

		/// <summary>
		/// Направление.
		/// </summary>
		public enum Moving
		{
			/// <summary>
			/// Прямое направление.
			/// </summary>
			Direct,

			/// <summary>
			/// Обратное направление.
			/// </summary>
			Reverse
		}

		/// <summary>
		/// Поле для стартовой вершины. Используется в алгоритмах.
		/// </summary>
		private int vertex;

		/// <summary>
		/// Поле для направления. Используется в алгоритмах.
		/// </summary>
		private Moving moving;

		/// <summary>
		/// Метод поиска дочерних вершин родительской вершины.
		/// </summary>
		/// <returns>Возвращает коллекцию дочерних вершин.</returns>
		private IEnumerable<int> Children()
		{
			for (int child = 0; child < AdjMatrix.Dimension; child++)
			{
				switch (moving)
				{
					case Moving.Direct:
						if (AdjMatrix[vertex, child] >= 1)
							yield return child;
						break;
					case Moving.Reverse:
						if (AdjMatrix[child, vertex] >= 1)
							yield return child;
						break;
				}
			}
		}

		/// <summary>
		/// Метод поиска в ширину.
		/// </summary>
		/// <returns>Возвращает массив посещённыйх вершин.</returns>
		private bool[] BreadthFirstSearch()
		{
			bool[] visited = new bool[AdjMatrix.Dimension];     // изначально список посещённых узлов пуст
			Queue<int> queue = new Queue<int>();
			queue.Enqueue(vertex);                         // начиная с узла-источника
			visited[vertex] = true;
			while (queue.Count != 0)
			{
				vertex = queue.Dequeue();                  // извлечь первый элемент в очереди
				foreach (int child in Children())    // все преемники текущего узла, ...
				{
					if (visited[child] == false)                // ... которые ещё не были посещены ...
					{
						queue.Enqueue(child);                   // ... добавить в конец очереди...
						visited[child] = true;                  // ... и пометить как посещённые
					}
				}
			}
			return visited;
		}

		/// <summary>
		/// Метод получения транзитивного замыкания i-вершины.
		/// </summary>
		/// <returns>Возвращает транзитивное замыкание в виде коллекции.</returns>
		private List<int> TransitiveClosure()
		{
			List<int> directTransitiveClosure = new List<int>();
			int index = 0;
			foreach (bool item in BreadthFirstSearch())
			{
				if (item) directTransitiveClosure.Add(index);
				index++;
			}
			return directTransitiveClosure;
		}

		/// <summary>
		/// Метод получения транзитивного замыкания i-вершины.
		/// </summary>
		/// <returns>Возвращает строковое представление транзитивного замыкания.</returns>
		public string GetTransitiveClosure(int vertex, Moving moving)
		{
			this.vertex = vertex;
			this.moving = moving;
			string trClosure = string.Empty;
			foreach (int item in TransitiveClosure())
				trClosure += $" x{item + 1},";
			string sign = string.Empty;
			switch (moving)
			{
				case Moving.Direct:
					sign = "⁺";
					break;
				case Moving.Reverse:
					sign = "⁻";
					break;
			}
			return $"T{sign}(x{vertex + 1}) = {{ {trClosure.Substring(0, trClosure.Length - 1)} }}";
		}

		/// <summary>
		/// Метод, определяющий, является ли граф сильно-связным.
		/// </summary>
		/// <returns>Если граф сильно-связный вовращает true, иначе - false.</returns>
		public bool IsStronglyConnected()
		{
			bool result = false;
			Matrix<int> reachMatrix = ReachabilityMatrix();
			for (int i = 0; i < AdjMatrix.Dimension - 1; i++)
			{
				for (int j = i + 1; j < AdjMatrix.Dimension; j++)
				{
					result = (reachMatrix[i, j] & reachMatrix[j, i]) == 1;
				}
			}
			return result;
		}

		/// <summary>
		/// Метод поиска матрицы достижимости по прямым транзитивным замыканиям.
		/// </summary>
		/// <returns>Возвращает матрицу достижимости.</returns>
		public Matrix<int> ReachabilityMatrix()
		{
			int dimension = AdjMatrix.Dimension;
			Matrix<int> reachabilityMatrix = new Matrix<int>(dimension, dimension);
			for (int vertex = 0; vertex < dimension; vertex++)
				foreach (var item in TransitiveClosure())
					reachabilityMatrix[vertex, item] = 1;
			return reachabilityMatrix;
		}

		/// <summary>
		/// Метод получения матрицы контрдостижимости транспонированием матрицы достижимости.
		/// </summary>
		/// <returns>Возвращает матрицу контрдостижимости.</returns>
		public Matrix<int> CounterReachabilityMatrix() => ReachabilityMatrix().Transpose();

		public Matrix<int> CMatrix()
		{
			int dimension = AdjMatrix.Dimension;
			Matrix<int> RMatrix = ReachabilityMatrix();
			Matrix<int> QMatrix = CounterReachabilityMatrix();
			Matrix<int> result = new Matrix<int>(dimension, dimension);
			for (int i = 0; i < dimension; i++)
			{
				for (int j = 0; j < dimension; j++)
				{
					result[i, j] = RMatrix[i, j] & QMatrix[i, j];
				}
			}
			return result;
		}

		//private bool RowMatchesColumn(Matrix<int> matrix, int rowIndex)
		//{
		//	int dimension = matrix.Dimension;
		//	bool result = false;
		//	for (int j = rowIndex + 1; j < dimension; j++)
		//	{
		//		if (matrix[rowIndex, j] == matrix[j, rowIndex])
		//		{

		//		}
		//	}
		//}

		public MaxStronglyConnectedSubgraphsMatrix MaxStronglyConnectedSubgraphs()
		{
			int dimension = AdjMatrix.Dimension;
			bool[] visited = new bool[dimension];
			Queue<int> vertices = new Queue<int>();
			MaxStronglyConnectedSubgraphsMatrix subgraphsMatrix = new MaxStronglyConnectedSubgraphsMatrix(dimension);
			Matrix<int> cMatrix = CMatrix();
			for (int i = 0; i < dimension; i++)
			{
				if (visited[i]) continue;
				for (int j = i; j < dimension; j++)
				{
					if (cMatrix[i, j] == 1)
					{
						vertices.Enqueue(j);
						visited[j] = true;
					}
				}
				bool rowsColumnsCheck = false;
				foreach (var vertex in vertices)
				{
					for (int j = i; j < dimension; j++)
					{
						if (cMatrix[vertex, j] == 1)
						{
							if (vertices.Contains(j))
							{
								rowsColumnsCheck = true;
							}
							else rowsColumnsCheck = false;
						}
					}
				}
				if (rowsColumnsCheck)
				{
					subgraphsMatrix.Add(vertices);
				}

			}
			return subgraphsMatrix;
		}

		/// <summary>
		/// Операция объединения двух графов.
		/// </summary>
		/// <param name="g1">Первый граф.</param>
		/// <param name="g2">Второй граф.</param>
		/// <returns>Возвращает новый граф, как результат объединения двух графов.</returns>
		public static Graph GetUnionGraph(Graph g1, Graph g2)
		{
			int dimension = g1.AdjMatrix.Dimension;
			Graph g3 = new Graph(dimension);
			for (int i = 0; i < dimension; i++)
				for (int j = 0; j < dimension; j++)
					g3.AdjMatrix[i, j] = g1.AdjMatrix[i, j] | g2.AdjMatrix[i, j];
			return g3;
		}

		/// <summary>
		/// Операция пересечения двух графов.
		/// </summary>
		/// <param name="g1">Первый граф.</param>
		/// <param name="g2">Второй граф.</param>
		/// <returns>Возвращает новый граф, как результат пересечения двух графов.</returns>
		public static Graph GetIntersectionGraph(Graph g1, Graph g2)
		{
			int dimension = g1.AdjMatrix.Dimension;
			Graph g3 = new Graph(dimension);
			for (int i = 0; i < dimension; i++)
				for (int j = 0; j < dimension; j++)
					g3.AdjMatrix[i, j] = g1.AdjMatrix[i, j] & g2.AdjMatrix[i, j];
			return g3;
		}

		private Matrix<int> C = new Matrix<int>(new int[,]
												{{0, 2, 5, 3, 0, 4, 0, 0},
												{4, 0, 0, 8, 0, 15, 0, 0},
												{9, 1, 0, 2, 5, 0, 13, 0},
												{0, 8, 2, 0, 16, 0, 3, 0},
												{0, 0, 5, 16, 0, 0, 0, 4},
												{4, 15, 0, 0, 0, 6, 7, 0},
												{0, 0, 13, 0, 0, 7, 0, 3},
												{0, 0, 0, 0, 4, 0, 0, 0}});

		struct Node
		{
			public int Index { get; set; }
			public int Weight { get; set; }

			public Node(int index, int weight)
			{
				Index = index;
				Weight = weight;
			}
		}

		private void Dijkstra(int startVertex)
		{
			int mark = 0;
			List<int> visitedVertices = new List<int>();
			Node minNode = new Node(0, int.MaxValue);
			foreach (var child in Children())
			{
				if (AdjMatrix[vertex, child] < minNode.Weight)
				{
					minNode.Index = child;
					minNode.Weight = AdjMatrix[vertex, child];
				}
			}

		}
	}
}