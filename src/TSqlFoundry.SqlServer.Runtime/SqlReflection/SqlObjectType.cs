// Copyright (c) Rodrigo Speller. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace TSqlFoundry.SqlServer.Runtime.SqlReflection
{
    internal enum SqlObjectType
    {
        AssemblyAggregateFunction,
        AssemblyScalarFunction,
        AssemblyStoredProcedure,
        AssemblyTableValuedFunction,
        CheckConstraint,
        DefaultConstraint,
        EdgeContraint,
        ForeignKeyConstraint,
        InlineTableValuedFunction,
        InternalTable,
        PlanGuide,
        PrimaryKeyConstraint,
        ReplicationFilterProcedure,
        Rule,
        ScalarFunction,
        SequenceObject,
        StoredProcedure,
        Synonym,
        SystemBaseTable,
        Table,
        View,
    }
}
