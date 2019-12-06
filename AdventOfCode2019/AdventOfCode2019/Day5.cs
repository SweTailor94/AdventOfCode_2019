using System;
using System.Reflection.Emit;
using System.Xml;

namespace AdventOfCode2019
{
    class Day5
    {
        private int[] program = new[]
        {
            3,225,1,225,6,6,1100,1,238,225,104,0,101,67,166,224,1001,224,-110,224,4,224,102,8,223,223,1001,224,4,224,1,224,223,223,2,62,66,224,101,
            -406,224,224,4,224,102,8,223,223,101,3,224,224,1,224,223,223,1101,76,51,225,1101,51,29,225,1102,57,14,225,1102,64,48,224,1001,224,-3072,
            224,4,224,102,8,223,223,1001,224,1,224,1,224,223,223,1001,217,90,224,1001,224,-101,224,4,224,1002,223,8,223,1001,224,2,224,1,223,224,223,
            1101,57,55,224,1001,224,-112,224,4,224,102,8,223,223,1001,224,7,224,1,223,224,223,1102,5,62,225,1102,49,68,225,102,40,140,224,101,-2720,
            224,224,4,224,1002,223,8,223,1001,224,4,224,1,223,224,223,1101,92,43,225,1101,93,21,225,1002,170,31,224,101,-651,224,224,4,224,102,8,223,
            223,101,4,224,224,1,223,224,223,1,136,57,224,1001,224,-138,224,4,224,102,8,223,223,101,2,224,224,1,223,224,223,1102,11,85,225,4,223,99,0,
            0,0,677,0,0,0,0,0,0,0,0,0,0,0,1105,0,99999,1105,227,247,1105,1,99999,1005,227,99999,1005,0,256,1105,1,99999,1106,227,99999,1106,0,265,1105,
            1,99999,1006,0,99999,1006,227,274,1105,1,99999,1105,1,280,1105,1,99999,1,225,225,225,1101,294,0,0,105,1,0,1105,1,99999,1106,0,300,1105,1,
            99999,1,225,225,225,1101,314,0,0,106,0,0,1105,1,99999,1107,226,226,224,102,2,223,223,1006,224,329,1001,223,1,223,1007,226,677,224,1002,223,
            2,223,1005,224,344,101,1,223,223,108,677,677,224,1002,223,2,223,1006,224,359,101,1,223,223,1008,226,226,224,1002,223,2,223,1005,224,374,1001,
            223,1,223,108,677,226,224,1002,223,2,223,1006,224,389,101,1,223,223,7,226,226,224,102,2,223,223,1006,224,404,101,1,223,223,7,677,226,224,1002,
            223,2,223,1005,224,419,101,1,223,223,107,226,226,224,102,2,223,223,1006,224,434,1001,223,1,223,1008,677,677,224,1002,223,2,223,1005,224,449,101,
            1,223,223,108,226,226,224,102,2,223,223,1005,224,464,1001,223,1,223,1108,226,677,224,1002,223,2,223,1005,224,479,1001,223,1,223,8,677,226,224,
            102,2,223,223,1006,224,494,1001,223,1,223,1108,677,677,224,102,2,223,223,1006,224,509,1001,223,1,223,1007,226,226,224,1002,223,2,223,1005,224,
            524,1001,223,1,223,7,226,677,224,1002,223,2,223,1005,224,539,1001,223,1,223,8,677,677,224,102,2,223,223,1005,224,554,1001,223,1,223,107,226,677,
            224,1002,223,2,223,1006,224,569,101,1,223,223,1107,226,677,224,102,2,223,223,1005,224,584,1001,223,1,223,1108,677,226,224,102,2,223,223,1006,224,
            599,1001,223,1,223,1008,677,226,224,102,2,223,223,1006,224,614,101,1,223,223,107,677,677,224,102,2,223,223,1006,224,629,1001,223,1,223,1107,677,
            226,224,1002,223,2,223,1005,224,644,101,1,223,223,8,226,677,224,102,2,223,223,1005,224,659,1001,223,1,223,1007,677,677,224,102,2,223,223,1005,224,
            674,1001,223,1,223,4,223,99,226
        };

        private int[] programTest = new[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 };

        private int[] memory;

        private int pc;
        private bool running = false;

        public int FindResult(int wanted)
        {
            //for (int noun = 0; noun < 100; ++noun)
            //    for (int verb = 0; verb < 100; ++verb)
            //    {
            //        if (wanted == Run(noun, verb))
            //        {
            //            return noun * 100 + verb;
            //        }
            //    }
            return -1;
        }

        public int Run()
        {
            memory = new int[program.Length];
            Array.Copy(program, memory, program.Length);


            running = true;
            pc = 0;
            while (running)
            {
                var op = new OpCode(memory[pc]);
                switch (op.Operation)
                {
                    case 1:
                        Add(op);
                        break;
                    case 2:
                        Mul(op);
                        break;
                    case 3:
                        Input(op);
                        break;
                    case 4:
                        Output(op);
                        break;
                    case 5:
                        JumpTrue(op);
                        break;
                    case 6:
                        JumpFalse(op);
                        break;
                    case 7:
                        Less(op);
                        break;
                    case 8:
                        Equals(op);
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

        private void JumpTrue(OpCode op)
        {
            var i1 = op.Immediate[0] ? pc + 1 : memory[pc + 1];
            var i2 = op.Immediate[1] ? pc + 2 : memory[pc + 2];
            if (memory[i1] != 0) pc = memory[i2];
            else pc += 3;
        }

        private void JumpFalse(OpCode op)
        {
            var i1 = op.Immediate[0] ? pc + 1 : memory[pc + 1];
            var i2 = op.Immediate[1] ? pc + 2 : memory[pc + 2];
            if (memory[i1] == 0) pc = memory[i2];
            else pc += 3;
        }

        private void Less(OpCode op)
        {
            var i1 = op.Immediate[0] ? pc + 1 : memory[pc + 1];
            var i2 = op.Immediate[1] ? pc + 2 : memory[pc + 2];
            var i3 = op.Immediate[2] ? pc + 3 : memory[pc + 3];
            memory[i3] = memory[i1] < memory[i2] ? 1 : 0;
            pc += 4;
        }

        private void Equals(OpCode op)
        {
            var i1 = op.Immediate[0] ? pc + 1 : memory[pc + 1];
            var i2 = op.Immediate[1] ? pc + 2 : memory[pc + 2];
            var i3 = op.Immediate[2] ? pc + 3 : memory[pc + 3];
            memory[i3] = memory[i1] == memory[i2] ? 1 : 0;
            pc += 4;
        }

        private void Mul(OpCode op)
        {
            var i1 = op.Immediate[0] ? pc + 1 : memory[pc + 1];
            var i2 = op.Immediate[1] ? pc + 2 : memory[pc + 2];
            var i3 = op.Immediate[2] ? pc + 3 : memory[pc + 3];
            memory[i3] = memory[i1] * memory[i2];
            pc += 4;
        }

        private void Add(OpCode op)
        {
            var i1 = op.Immediate[0] ? pc + 1 : memory[pc + 1];
            var i2 = op.Immediate[1] ? pc + 2 : memory[pc + 2];
            var i3 = op.Immediate[2] ? pc + 3 : memory[pc + 3];
            memory[i3] = memory[i1] + memory[i2];
            pc += 4;
        }

        private void Input(OpCode op)
        {
            Console.Write("Input value: ");
            var inp = Console.ReadLine();
            int value = int.Parse(inp);
            memory[memory[pc + 1]] = value;
            pc += 2;
        }
        private void Output(OpCode op)
        {
            int value;
            if (op.Immediate[0])
            {
                value = memory[pc + 1];
            }
            else
            {
                value = memory[memory[pc + 1]];
            }

            Console.WriteLine($"Output value: {value}");
            pc += 2;
        }

    }

    public class OpCode
    {
        public OpCode(int o)
        {
            Operation = o % 100;
            o /= 100;
            int i = 0;
            while (o > 0)
            {
                Immediate[i++] = (o % 10) > 0;
                o /= 10;
            }
        }
        public int Operation { get; set; }
        public bool[] Immediate = new bool[3];
    }


}