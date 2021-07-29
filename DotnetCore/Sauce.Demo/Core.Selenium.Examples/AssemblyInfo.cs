//Docs on NUnit parallelization: https://github.com/nunit/docs/wiki/Framework-Parallel-Test-Execution

using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]
//Set this value to the Maximum amount of VMs that you have in Sauce Labs
[assembly: LevelOfParallelism(100)]