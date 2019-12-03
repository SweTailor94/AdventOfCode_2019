using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2019
{
    class Day2
    {
        private int[] program = new[]
        {
            1, 0, 0, 3, 1, 1, 2, 3, 1, 3, 4, 3, 1, 5, 0, 3, 2, 9, 1, 19, 1, 19, 6, 23, 2, 6, 23, 27, 2, 27, 9, 31, 1, 5,
            31, 35, 1, 35, 10, 39, 2, 39, 9, 43, 1, 5, 43, 47, 2, 47, 10, 51, 1, 51, 6, 55, 1, 5, 55, 59, 2, 6, 59, 63,
            2, 63, 6, 67, 1, 5, 67, 71, 1, 71, 9, 75, 2, 75, 10, 79, 1, 79, 5, 83, 1, 10, 83, 87, 1, 5, 87, 91, 2, 13,
            91, 95, 1, 95, 10, 99, 2, 99, 13, 103, 1, 103, 5, 107, 1, 107, 13, 111, 2, 111, 9, 115, 1, 6, 115, 119, 2,
            119, 6, 123, 1, 123, 6, 127, 1, 127, 9, 131, 1, 6, 131, 135, 1, 135, 2, 139, 1, 139, 10, 0, 99, 2, 0, 14, 0
        };

        private int[] programTest = new[] {1, 1, 1, 4, 99, 5, 6, 0, 99};

        private int[] memory;

    private int pc;
        private bool running = false;

        public int FindResult(int wanted)
        {
            for(int noun = 0; noun < 100; ++noun)
            for (int verb = 0; verb < 100; ++verb)
            {
                if (wanted == Run(noun, verb))
                {
                    return noun * 100 + verb;
                }
            }
            return -1;
        }

        public int Run(int noun, int verb)
        {
            memory = new int[program.Length];
            Array.Copy(program,memory,program.Length);

            memory[1] = noun;
            memory[2] = verb;

            running = true;
            pc = 0;
            while (running)
            {
                switch (memory[pc])
                {
                    case 1:
                        Add();
                        break;
                    case 2:
                        Mul();
                        break;
                    case 99:
                        running = false;
                        break;
                    default:
                        throw new Exception("something went wrong!");

                }
            }
            return memory[0];
        }

        private void Mul()
        {
            var i1= memory[pc + 1];
            var i2 = memory[pc + 2];
            var i3 = memory[pc + 3];
            memory[i3] = memory[i1] * memory[i2];
            pc += 4;
        }

        private void Add()
        {
            var i1 = memory[pc + 1];
            var i2 = memory[pc + 2];
            var i3 = memory[pc + 3];
            memory[i3] = memory[i1] + memory[i2];
            pc += 4;
        }

    }
}
