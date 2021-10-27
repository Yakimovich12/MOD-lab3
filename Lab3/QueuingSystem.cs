using MyRandom = Random.Random;

namespace QueuingSystem
{
    class QueuingSystem
    {
        public static double[] EmulateQueuingSystem(int number, double p1, double p2)
        {
            var random1 = new MyRandom(p1);
            var random2 = new MyRandom(p2);

            byte pi1;
            byte pi2;

            States state = new States();
            States newState = null;

            int numberOfRequest = 0;
            int numberOfGrand = 0;
            int numberOfExecutedTask = 0;
            int numberOfLocks = 0;
            int numberOfApInQueue = 0;
            int numberOfApplications = 0;
            int pipe1Busy = 0;
            int pipe2Busy = 0;

            for (int i = 0; i < number; i++)
            {
                pi1 = random1.Next();
                pi2 = random2.Next();

                if (state.IsRequest())
                {
                    numberOfRequest++;
                }

                if (state.WillBeExecuted(pi1, pi2))
                {
                    numberOfGrand++;
                }

                if (state.HasBeenExecuted(pi2))
                {
                    numberOfExecutedTask++;
                }

                newState = state.NextState(pi1, pi2);

                if (newState.IsPipe1Locked())
                {
                    numberOfLocks++;
                }

                if (!newState.IsQueueEmpty())
                {
                    numberOfApInQueue++;
                }

                numberOfApplications += newState.NumberOfApplications();

                if (newState.IsPipe1Busy())
                {
                    pipe1Busy++;
                }

                if (newState.IsPipe2Busy())
                {
                    pipe2Busy++;
                }

                state = newState;
            }

            var parameters = new double[10];

            parameters[0] = (double)numberOfGrand / numberOfRequest;
            parameters[1] = (double)numberOfExecutedTask / number;
            parameters[2] = 1 - (double)numberOfGrand / numberOfRequest;
            parameters[3] = (double)numberOfLocks / number;
            parameters[4] = (double)numberOfApInQueue / number;
            parameters[5] = (double)numberOfApplications / number;
            parameters[6] = (double)numberOfApInQueue / numberOfExecutedTask;
            parameters[7] = (double)numberOfApplications / numberOfExecutedTask;
            parameters[8] = (double)pipe1Busy / number;
            parameters[9] = (double)pipe2Busy / number;

            return parameters;
        }

        public static double[][] GenerateMatrix(double pi1, double pi2)
        {
            var matrix = new double[12][];

            matrix[0] = new double[13] { 1, 1 - pi2, 0, pi1, pi1 * (1 - pi2), 0, 0, 0, -1, 0, 0, 0, 0 };
            matrix[1] = new double[13] { 0, 0, 0, -1, 0, 0, 0, 0, pi1, pi1 * (1 - pi2), 0, 0, 0 };
            matrix[2] = new double[13] { 0, -1, 0, 0, 0, 0, 0, 0, 1 - pi1, (1 - pi1) * (1 - pi2), 0, 0, 0 };
            matrix[3] = new double[13] { 0, pi2, 1 - pi2, 1 - pi1, pi1 * pi2 + (1 - pi1) * (1 - pi2), pi1 * (1 - pi2), 0, 0, 0, -1, 0, 0, 0 };
            matrix[4] = new double[13] { 0, 0, -1, 0, 0, 0, 0, 0, 0, (1 - pi1) * pi2, (1 - pi1) * (1 - pi2), 1 - pi2, 0 };
            matrix[5] = new double[13] { 0, 0, 0, 0, -1, 0, 0, 0, 0, pi1 * pi2, pi1 * (1 - pi2), 0, 0 };
            matrix[6] = new double[13] { 0, 0, pi2, 0, (1 - pi1) * pi2, (1 - pi1) * (1 - pi2) + pi1 * pi2, 1 - pi2, 0, 0, 0, -1, 0, 0 };
            matrix[7] = new double[13] { 0, 0, 0, 0, 0, -1, 0, 0, 0, 0, pi1 * pi2, 0, 0 };
            matrix[8] = new double[13] { 0, 0, 0, 0, 0, 0, -1, 0, 0, 0, (1 - pi1) * pi2, pi2, 0 };
            matrix[9] = new double[13] { 0, 0, 0, 0, 0, (1 - pi1) * pi2, pi2, 0, 0, 0, 0, -1, 0 };
            matrix[10] = new double[13] { -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
            matrix[10] = new double[13] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 };
            matrix[11] = new double[13] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            return matrix;
        }

        public static double GetProbabilityOfFailure(double[] resultProbabilities, double pi1, double pi2)
        {
            double p1xxx = 0;

            for (int i = 0; i < 7; i++)
            {
                p1xxx += resultProbabilities[i];
            }

            return ((resultProbabilities[3] + resultProbabilities[4]) * pi1 + (resultProbabilities[5] +
                resultProbabilities[6]) * pi2 + resultProbabilities[5] * pi1 * (1 - pi2)) / p1xxx;
        }

        public static double GetAbsolutlyThroughtput(double[] resultProbabilities, double pi1, double pi2)
        {
            return 0.5 * (1 - GetProbabilityOfFailure(resultProbabilities, pi1, pi2));
        }

        public static double GetProbabilityOfLocking(double[] resultProbabilities, double pi1, double pi2)
        {
            return resultProbabilities[6] + resultProbabilities[11];
        }

        public static double GetAverageQueueLength(double[] resultProbabilities, double pi1, double pi2)
        {
            return resultProbabilities[2] + resultProbabilities[5] + resultProbabilities[6] + resultProbabilities[10] +
                resultProbabilities[11];
        }

        public static double GetAverageNumberOfApplications(double[] resultProbabilities, double pi1, double pi2)
        {
            double oneApProbability = resultProbabilities[1] + resultProbabilities[3] + resultProbabilities[8];

            double twoApProbabilities = resultProbabilities[2] + resultProbabilities[4] + resultProbabilities[9];

            double threeApProbabilities = 1 - oneApProbability - twoApProbabilities;

            return oneApProbability + 2 * twoApProbabilities + 3 * threeApProbabilities;
        }

        public static double GetPipe1BusyProbability(double[] resultProbability, double pi1, double pi2)
        {
            return 1 - resultProbability[0] - resultProbability[1] - resultProbability[2] - resultProbability[7];
        }

        public static double GetPipe2BusyProbability(double[] resultProbability, double pi1, double pi2)
        {
            return 1 - resultProbability[0] - resultProbability[3] - resultProbability[7] - resultProbability[8];
        }
    }
}

