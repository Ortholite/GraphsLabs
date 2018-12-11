using System;
using System.Threading;
using System.Threading.Tasks;

namespace GraphsLabs
{
	/// <summary>
	/// Класс, реализующий матрицу.
	/// </summary>
	/// <typeparam name="T">Тип данных элементов матрицы.</typeparam>
	internal class Matrix<T> where T : IConvertible
	{
		/// <summary>
		/// Поле, представляющее данные матрицы.
		/// </summary>
		protected T[,] matrix;

		public T[,] MatrixData { get { return matrix; } }

		/// <summary>
		/// Индексация матрицы.
		/// </summary>
		/// <param name="i">Строка матрицы.</param>
		/// <param name="j">Столбец матрицы.</param>
		/// <returns>Возвращает элемент матрицы по индексу.</returns>
		public T this[int i, int j]
		{
			get => matrix[i, j];
			set => matrix[i, j] = value;
		}

		/// <summary>
		/// Свойство, представляющее длину строки матрицы.
		/// </summary>
		public int Rows { get; private set; }

		/// <summary>
		/// Свойство, представляющее длину столбца матрицы.
		/// </summary>
		public int Columns { get; private set; }

		/// <summary>
		/// Размерность квадратной матрицы.
		/// </summary>
		public int Dimension { get; private set; }

		public Matrix(T[,] matrix)
		{
			this.matrix = matrix;
			Dimension = matrix.Length;
			Rows = Dimension;
			Columns = Dimension;
		}

		/// <summary>
		/// Конструктор, принимающий количество строк и столбцов матрицы.
		/// </summary>
		public Matrix(int rows, int columns)
		{
			Rows = rows;
			Columns = columns;
			if (rows == columns)
				Dimension = rows;
			matrix = new T[rows, columns];
		}

		/// <summary>
		/// Метод заполнения матрицы от 0 до 1.
		/// </summary>
		public void Random() => SetRandomMatrix(min: 0, max: 2);

		/// <summary>
		/// Метод заполнения матрицы случайными числами от min до max.
		/// </summary>
		public void Random(int min, int max) => SetRandomMatrix(min, max);

		private void SetRandomMatrix(int min, int max)
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					if (matrix is int[,])
					{
						int rand = random.Next(min, max);
						matrix[i, j] = (T)Convert.ChangeType(rand, typeof(T));
					}
				}
			}
		}

		public void RandomParallel() => SetRandomMatrixParallel(min: 0, max: 2);

		private void SetRandomMatrixParallel(int min, int max)  //1.4-1.8
		{
			Random random = new Random((int)DateTime.Now.Ticks);
			Parallel.For(0, Rows, i =>
			{
				//for (int j = 0; j < Columns; j++)
				//{
				//	if (matrix is int[,])
				//	{
				//		int rand = random.Next(min, max);
				//		matrix[i, j] = (T)Convert.ChangeType(rand, typeof(T));
				//	}
				//}
				Parallel.For(0, Columns, j =>
				{
					if (matrix is int[,])
					{
						//Thread.Sleep(10); //Так лучше рандомит.
						int rand = random.Next(min, max);
						matrix[i, j] = (T)Convert.ChangeType(rand, typeof(T));
					}
				});
			});
		}

		/// <summary>
		/// Метод транспонирования матрицы.
		/// Возвращает транспонированную матрицу.
		/// </summary>
		public Matrix<T> Transpose()
		{
			Matrix<T> newMatrix = new Matrix<T>(Columns, Rows);
			for (int i = 0; i < Rows; i++)
				for (int j = 0; j < Columns; j++)
					newMatrix.matrix[j, i] = matrix[i, j];
			return newMatrix;
		}

		/// <summary>
		/// Преобразует матрицу в строку.
		/// </summary>
		public override string ToString()
		{
			string str = "";
			for (int i = 0; i < Rows; i++)
			{
				for (int j = 0; j < Columns; j++)
				{
					str += matrix[i, j] + "  ";
				}
				str += "\n";
			}
			return str;
		}
	}
}