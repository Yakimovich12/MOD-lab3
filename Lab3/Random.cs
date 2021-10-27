using System;

namespace Random
{
    public class Random
    {
        private byte[] values;
        private System.Random random;

        public Random(double probability)
        {
            if (probability < 0 || probability > 1)
            {
                throw new ArgumentOutOfRangeException($"{probability} can be greater or equal than 0 and less or equal 1");
            }

            string number = probability.ToString();

            int numberOfDigits = number.Contains('.') ? number.Length - 2 : number.Length;

            int massiveSize = (int)Math.Pow(10, numberOfDigits);

            int numberOfValues = (int)(probability * massiveSize);

            values = new byte[massiveSize];

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = i < numberOfValues ? (byte)0 : (byte)1;
            }

            this.random = new System.Random();
        }

        public byte Next()
        {
            int index = random.Next(0, values.Length);

            return values[index];
        }
    }
}


