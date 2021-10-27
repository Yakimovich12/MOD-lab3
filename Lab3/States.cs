using System.Text;

namespace QueuingSystem
{
    public class States
    {
        public byte NumberOfTacts { private set; get; }
        public byte Pipe1State { private set; get; }
        public byte QueueLength { private set; get; }
        public byte Pipe2State { private set; get; }

        public States()
        {
            NumberOfTacts = 2;
            Pipe1State = 0;
            QueueLength = 0;
            Pipe2State = 0;
        }

        public States NextState(byte pi1, byte pi2)
        {
            States newState = new States();

            if (NumberOfTacts == 2)
            {
                newState.NumberOfTacts = 1;
            }
            else
            {
                newState.NumberOfTacts = 2;
            }

            if (Pipe1State == 2 || (Pipe1State == 1 && QueueLength == 1 && pi1 == 1) || (QueueLength == 1 && Pipe2State == 1 && pi2 == 0) || (Pipe1State == 1 && Pipe2State == 1 && pi1 == 1 && pi2 == 0))
            {
                newState.QueueLength = 1;
            }
            else
            {
                newState.QueueLength = 0;
            }

            if ((NumberOfTacts == 1 && (QueueLength == 0 || Pipe1State == 0 || pi2 == 1 || (Pipe1State == 2 && pi2 == 1))) || (Pipe1State == 1 && pi1 == 0))
            {
                newState.Pipe1State = 1;
            }
            else if (pi2 == 0 && (Pipe1State == 2 || (Pipe1State == 1 && QueueLength == 1 && pi1 == 1)))
            {
                newState.Pipe1State = 2;
            }
            else
            {
                newState.Pipe1State = 0;
            }

            if ((Pipe2State == 1 && (pi2 == 0 || QueueLength == 1)) || Pipe1State == 1 && pi1 == 1)
            {
                newState.Pipe2State = 1;
            }
            else
            {
                newState.Pipe2State = 0;
            }

            return newState;
        }

        public bool WillBeExecuted(byte pi1, byte pi2)
        {
            return NumberOfTacts == 1 && (Pipe1State == 0 || (Pipe1State == 1 && pi1 == 1 && pi2 == 1) ||
                (Pipe1State == 2 && pi2 == 1) || (Pipe1State == 1 && QueueLength == 0 && pi1 == 1));
        }

        public bool IsPipe1Locked()
        {
            return this.Pipe1State == 2;
        }

        public bool IsRequest()
        {
            return this.NumberOfTacts == 1;
        }

        public bool HasBeenExecuted(byte pi2)
        {
            return this.Pipe2State == 1 && pi2 == 1;
        }

        public bool IsQueueEmpty()
        {
            return this.QueueLength == 0;
        }

        public int NumberOfApplications()
        {
            int number = this.Pipe2State + this.QueueLength;

            number += this.Pipe1State == 2 ? 1 : this.Pipe1State;

            return number;
        }

        public bool IsPipe1Busy()
        {
            return this.Pipe1State != 0;
        }

        public bool IsPipe2Busy()
        {
            return this.Pipe2State == 1;
        }

        public override string ToString()
        {
            var state = new StringBuilder();

            state.Append(this.NumberOfTacts);
            state.Append(this.Pipe1State);
            state.Append(this.QueueLength);
            state.Append(this.Pipe2State);

            return state.ToString();
        }
    }
}
