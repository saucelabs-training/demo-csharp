using NUnit.Framework;

[assembly: LevelOfParallelism(10)]
[assembly: Parallelizable(ParallelScope.All)]