using System;
using ExampleService.Merged;
using System.Collections.Generic;

namespace ExampleService.DataAccess
{
    internal interface ITelemetryAccess : IDisposable
    {
        void Open();

        void BeginTransaction();

        void Commit();

        void TestInsert();

        void TelemetryInsert(DeviceError item);

        void TelemetryInsert(MergedData item);

        int OutputInsert(OutputControl item);

        void OutputUpdate(int id, byte status);

        List<int> CheckConfirmationExpirations();
    }
}