﻿using Calculations.Dto;
using System;
using System.Threading.Tasks;

namespace TaxEtoro.Interfaces
{
    internal interface IActionPerformer : IAsyncDisposable
    {
        Task<CalculationResultDto> PerformCalculations(string directory, string fileName);
        Task PresentCalcucaltionResults(CalculationResultDto result);
        void OnAppClose(object sender, EventArgs e);
    }
}