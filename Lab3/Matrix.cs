using System;

namespace Matrix
{
    static class Matrix
    {
        private static int accupancy = 10;
        private static double delta = Math.Pow(10, -accupancy);
        private static bool IsMatrixStringZero(double[] matrixString)
        {
            int j;

            for (j = 0; j < matrixString.Length; j++)
            {
                if (matrixString[j] >= delta || matrixString[j] <= -delta)
                {
                    break;
                }
            }

            return j == matrixString.Length;
        }

        private static int GetReplaceableMatrixIndex(double[][] matrix, int indexI, int indexJ)
        {
            int i;

            for (i = indexI + 1; i < matrix.Length; i++)
            {
                if (matrix[i][indexJ] >= delta || matrix[i][indexJ] <= -delta)
                {
                    return i;
                }
            }

            return -1;
        }

        private static double[][] DeleteMatrixString(double[][] matrix, int index)
        {
            var newMatrix = new double[matrix.Length - 1][];

            for (int i = 0; i < matrix.Length - 1; i++)
            {
                if (i < index)
                {
                    newMatrix[i] = matrix[i];
                }
                else
                {
                    newMatrix[i] = matrix[i + 1];
                }
            }

            return newMatrix;
        }

        private static double[] GetResultValues(double[][] matrix)
        {
            var result = new double[matrix.Length];

            for (int i = matrix.Length - 1; i >= 0; i--)
            {
                int number = result.Length - 1 - i;

                double value = 0;

                for (int j = 0; j < number; j++)
                {
                    value += result[^(j + 1)] * matrix[i][^(j + 2)];
                }

                value = matrix[i][^1] - value;

                result[i] = value / matrix[i][i];
            }

            return result;
        }

        public static double[] GaussianMethod(double[][] matrix)
        {

            int indexI;
            int indexJ = 0;

            for (indexI = 0; indexI < matrix.Length - 1; indexI++)
            {
                if (-delta < matrix[indexI][indexJ] && matrix[indexI][indexJ] < delta)
                {

                    if (IsMatrixStringZero(matrix[indexI]))
                    {
                        matrix = DeleteMatrixString(matrix, indexI);
                        indexI--;
                        continue;
                    }

                    int index = GetReplaceableMatrixIndex(matrix, indexI, indexJ);

                    if (index != -1)
                    {
                        var temp = matrix[indexI];
                        matrix[indexI] = matrix[index];
                        matrix[index] = temp;
                    }
                    else
                    {
                        throw new ArgumentException("Error the system of equations has no one solution");
                    }
                }

                for (int i = indexI + 1; i < matrix.Length; i++)
                {
                    double factor = matrix[i][indexJ] / matrix[indexI][indexJ];

                    for (int j = indexJ; j < matrix[0].Length; j++)
                    {
                        matrix[i][j] -= factor * matrix[indexI][j];
                    }

                    if (IsMatrixStringZero(matrix[i]))
                    {
                        matrix = DeleteMatrixString(matrix, i);
                        i--;
                    }
                }
                indexJ++;
            }

            int y;

            for (y = 0; y < matrix[^1].Length; y++)
            {
                if (matrix[^1][y] >= delta || matrix[^1][y] <= -delta)
                {
                    break;
                }
            }

            if (y > matrix[^1].Length - 2)
            {
                throw new ArgumentException("Error the system of equations has no solution");
            }

            if (y < matrix[^1].Length - 2)
            {
                throw new ArgumentException("Error the system of equations has no one solution");
            }

            var result = GetResultValues(matrix);

            return result;
        }
    }
}
