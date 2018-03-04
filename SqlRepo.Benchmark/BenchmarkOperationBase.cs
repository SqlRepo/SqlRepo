using System;
using System.Diagnostics;
using System.Threading;

namespace SqlRepo.Benchmark
{
    public abstract class BenchmarkOperationBase : IBenchmarkOperation
    {
        protected BenchmarkOperationBase(IBenchmarkHelpers benchmarkHelpers, Component component)
        {
            this.Component = component;
            this.BenchmarkHelpers = benchmarkHelpers;
        }

        public IBenchmarkHelpers BenchmarkHelpers { get; }

        public Component Component { get; set; }

        public virtual string GetNotes()
        {
            return null;
        }

        public BenchmarkResult Run()
        {
            var notes = this.GetNotes();

            Console.WriteLine($"Running {this.GetType() .Name}");

            if(!string.IsNullOrEmpty(notes))
            {
                Console.WriteLine(notes);
            }

            var result = new BenchmarkResult();
            result.TestName = this.GetType()
                                  .Name;

            this.Setup();
            Thread.Sleep(2000);

            var sw = new Stopwatch();
            sw.Start();

            this.Execute();

            sw.Stop();

            result.TimeTaken = Math.Round(sw.Elapsed.TotalMilliseconds, 2);
            result.Notes = this.GetNotes();
            result.Component = this.Component.ToString();

            return result;
        }

        public abstract void Execute();

        public virtual void Setup()
        {
            //this.BenchmarkHelpers.ClearRecords();
            //this.BenchmarkHelpers.ClearBufferPool();
            //this.BenchmarkHelpers.InsertRecords(250000);
        }
    }
}