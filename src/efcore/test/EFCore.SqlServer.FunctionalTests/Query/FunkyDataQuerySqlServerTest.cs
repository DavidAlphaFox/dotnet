﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Microsoft.EntityFrameworkCore.Query;

#nullable disable

public class FunkyDataQuerySqlServerTest : FunkyDataQuerySqlServerBaseTest<FunkyDataQuerySqlServerTest.FunkyDataQuerySqlServerFixture>
{
    public FunkyDataQuerySqlServerTest(FunkyDataQuerySqlServerFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture, testOutputHelper)
    {
    }

    public override async Task String_contains_on_argument_with_wildcard_constant(bool async)
    {
        await base.String_contains_on_argument_with_wildcard_constant(async);

        AssertSql(
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'%\%B%' ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'%a\_%' ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'%\_Ba\_%' ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] NOT LIKE N'%\%B\%a\%r%' ESCAPE N'\' OR [f].[FirstName] IS NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
""");
    }

    public override async Task String_contains_on_argument_with_wildcard_parameter(bool async)
    {
        await base.String_contains_on_argument_with_wildcard_parameter(async);

        AssertSql(
            """
@prm1_contains='%\%B%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm1_contains ESCAPE N'\'
""",
            //
            """
@prm2_contains='%a\_%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm2_contains ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1
""",
            //
            """
@prm4_contains='%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm4_contains ESCAPE N'\'
""",
            //
            """
@prm5_contains='%\_Ba\_%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm5_contains ESCAPE N'\'
""",
            //
            """
@prm6_contains='%\%B\%a\%r%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] NOT LIKE @prm6_contains ESCAPE N'\' OR [f].[FirstName] IS NULL
""",
            //
            """
@prm7_contains='%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] NOT LIKE @prm7_contains ESCAPE N'\' OR [f].[FirstName] IS NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
""");
    }

    public override async Task String_contains_on_argument_with_wildcard_column(bool async)
    {
        await base.String_contains_on_argument_with_wildcard_column(async);

        AssertSql(
            """
SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE [f].[FirstName] IS NOT NULL AND [f0].[LastName] IS NOT NULL AND (CHARINDEX([f0].[LastName], [f].[FirstName]) > 0 OR [f0].[LastName] LIKE N'')
""");
    }

    public override async Task String_contains_on_argument_with_wildcard_column_negated(bool async)
    {
        await base.String_contains_on_argument_with_wildcard_column_negated(async);

        AssertSql(
            """
SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE [f].[FirstName] IS NULL OR [f0].[LastName] IS NULL OR (CHARINDEX([f0].[LastName], [f].[FirstName]) <= 0 AND [f0].[LastName] NOT LIKE N'')
""");
    }

    public override async Task String_starts_with_on_argument_with_wildcard_constant(bool async)
    {
        await base.String_starts_with_on_argument_with_wildcard_constant(async);

        AssertSql(
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'\%B%' ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'\_B%' ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'\_Ba\_%' ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] NOT LIKE N'\%B\%a\%r%' ESCAPE N'\' OR [f].[FirstName] IS NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
""");
    }

    public override async Task String_starts_with_on_argument_with_wildcard_parameter(bool async)
    {
        await base.String_starts_with_on_argument_with_wildcard_parameter(async);

        AssertSql(
            """
@prm1_startswith='\%B%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm1_startswith ESCAPE N'\'
""",
            //
            """
@prm2_startswith='\_B%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm2_startswith ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1
""",
            //
            """
@prm4_startswith='%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm4_startswith ESCAPE N'\'
""",
            //
            """
@prm5_startswith='\_Ba\_%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm5_startswith ESCAPE N'\'
""",
            //
            """
@prm6_startswith='\%B\%a\%r%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] NOT LIKE @prm6_startswith ESCAPE N'\' OR [f].[FirstName] IS NULL
""",
            //
            """
@prm7_startswith='%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] NOT LIKE @prm7_startswith ESCAPE N'\' OR [f].[FirstName] IS NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
""");
    }

    public override async Task String_starts_with_on_argument_with_bracket(bool async)
    {
        await base.String_starts_with_on_argument_with_bracket(async);

        AssertSql(
            """
SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'\[%' ESCAPE N'\'
""",
            //
            """
SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'B\[%' ESCAPE N'\'
""",
            //
            """
SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'B\[\[a^%' ESCAPE N'\'
""",
            //
            """
@prm1_startswith='\[%' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm1_startswith ESCAPE N'\'
""",
            //
            """
@prm2_startswith='B\[%' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm2_startswith ESCAPE N'\'
""",
            //
            """
@prm3_startswith='B\[\[a^%' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm3_startswith ESCAPE N'\'
""",
            //
            """
SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL AND [f].[LastName] IS NOT NULL AND LEFT([f].[FirstName], LEN([f].[LastName])) = [f].[LastName]
""");
    }

    public override async Task String_starts_with_on_argument_with_wildcard_column(bool async)
    {
        await base.String_starts_with_on_argument_with_wildcard_column(async);

        AssertSql(
            """
SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE [f].[FirstName] IS NOT NULL AND [f0].[LastName] IS NOT NULL AND LEFT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]
""");
    }

    public override async Task String_starts_with_on_argument_with_wildcard_column_negated(bool async)
    {
        await base.String_starts_with_on_argument_with_wildcard_column_negated(async);

        AssertSql(
            """
SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE [f].[FirstName] IS NULL OR [f0].[LastName] IS NULL OR LEFT([f].[FirstName], LEN([f0].[LastName])) <> [f0].[LastName]
""");
    }

    public override async Task String_ends_with_on_argument_with_wildcard_constant(bool async)
    {
        await base.String_ends_with_on_argument_with_wildcard_constant(async);

        AssertSql(
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'%\%r' ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'%r\_' ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NOT NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE N'%\_r\_' ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] NOT LIKE N'%a\%r\%' ESCAPE N'\' OR [f].[FirstName] IS NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] IS NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
""");
    }

    public override async Task String_ends_with_on_argument_with_wildcard_parameter(bool async)
    {
        await base.String_ends_with_on_argument_with_wildcard_parameter(async);

        AssertSql(
            """
@prm1_endswith='%\%r' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm1_endswith ESCAPE N'\'
""",
            //
            """
@prm2_endswith='%r\_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm2_endswith ESCAPE N'\'
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE 0 = 1
""",
            //
            """
@prm4_endswith='%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm4_endswith ESCAPE N'\'
""",
            //
            """
@prm5_endswith='%\_r\_' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @prm5_endswith ESCAPE N'\'
""",
            //
            """
@prm6_endswith='%a\%r\%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] NOT LIKE @prm6_endswith ESCAPE N'\' OR [f].[FirstName] IS NULL
""",
            //
            """
@prm7_endswith='%' (Size = 4000)

SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] NOT LIKE @prm7_endswith ESCAPE N'\' OR [f].[FirstName] IS NULL
""",
            //
            """
SELECT [f].[FirstName]
FROM [FunkyCustomers] AS [f]
""");
    }

    public override async Task String_ends_with_on_argument_with_wildcard_column(bool async)
    {
        await base.String_ends_with_on_argument_with_wildcard_column(async);

        AssertSql(
            """
SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE [f].[FirstName] IS NOT NULL AND [f0].[LastName] IS NOT NULL AND RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName]
""");
    }

    public override async Task String_ends_with_on_argument_with_wildcard_column_negated(bool async)
    {
        await base.String_ends_with_on_argument_with_wildcard_column_negated(async);

        AssertSql(
            """
SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE [f].[FirstName] IS NULL OR [f0].[LastName] IS NULL OR RIGHT([f].[FirstName], LEN([f0].[LastName])) <> [f0].[LastName]
""");
    }

    public override async Task String_ends_with_inside_conditional(bool async)
    {
        await base.String_ends_with_inside_conditional(async);

        AssertSql(
            """
SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE CASE
    WHEN [f].[FirstName] IS NOT NULL AND [f0].[LastName] IS NOT NULL AND RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName] THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END = CAST(1 AS bit)
""");
    }

    public override async Task String_ends_with_inside_conditional_negated(bool async)
    {
        await base.String_ends_with_inside_conditional_negated(async);

        AssertSql(
            """
SELECT [f].[FirstName] AS [fn], [f0].[LastName] AS [ln]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE CASE
    WHEN [f].[FirstName] IS NULL OR [f0].[LastName] IS NULL OR RIGHT([f].[FirstName], LEN([f0].[LastName])) <> [f0].[LastName] THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END = CAST(1 AS bit)
""");
    }

    public override async Task String_ends_with_equals_nullable_column(bool async)
    {
        await base.String_ends_with_equals_nullable_column(async);

        AssertSql(
            """
SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool], [f0].[Id], [f0].[FirstName], [f0].[LastName], [f0].[NullableBool]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE CASE
    WHEN [f].[FirstName] IS NOT NULL AND [f0].[LastName] IS NOT NULL AND RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName] THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END = [f].[NullableBool]
""");
    }

    public override async Task String_ends_with_not_equals_nullable_column(bool async)
    {
        await base.String_ends_with_not_equals_nullable_column(async);

        AssertSql(
            """
SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool], [f0].[Id], [f0].[FirstName], [f0].[LastName], [f0].[NullableBool]
FROM [FunkyCustomers] AS [f]
CROSS JOIN [FunkyCustomers] AS [f0]
WHERE CASE
    WHEN [f].[FirstName] IS NOT NULL AND [f0].[LastName] IS NOT NULL AND RIGHT([f].[FirstName], LEN([f0].[LastName])) = [f0].[LastName] THEN CAST(1 AS bit)
    ELSE CAST(0 AS bit)
END <> [f].[NullableBool] OR [f].[NullableBool] IS NULL
""");
    }

    public override async Task String_FirstOrDefault_and_LastOrDefault(bool async)
    {
        await base.String_FirstOrDefault_and_LastOrDefault(async);

        AssertSql(
            """
SELECT SUBSTRING([f].[FirstName], 1, 1) AS [first], SUBSTRING([f].[FirstName], LEN([f].[FirstName]), 1) AS [last]
FROM [FunkyCustomers] AS [f]
ORDER BY [f].[Id]
""");
    }

    public override async Task String_Contains_and_StartsWith_with_same_parameter(bool async)
    {
        await base.String_Contains_and_StartsWith_with_same_parameter(async);

        AssertSql(
            """
@s_contains='%B%' (Size = 4000)
@s_startswith='B%' (Size = 4000)

SELECT [f].[Id], [f].[FirstName], [f].[LastName], [f].[NullableBool]
FROM [FunkyCustomers] AS [f]
WHERE [f].[FirstName] LIKE @s_contains ESCAPE N'\' OR [f].[LastName] LIKE @s_startswith ESCAPE N'\'
""");
    }

    public class FunkyDataQuerySqlServerFixture : FunkyDataQueryFixtureBase, ITestSqlLoggerFactory
    {
        public TestSqlLoggerFactory TestSqlLoggerFactory
            => (TestSqlLoggerFactory)ListLoggerFactory;

        protected override ITestStoreFactory TestStoreFactory
            => SqlServerTestStoreFactory.Instance;

        protected override string StoreName
            => nameof(FunkyDataQuerySqlServerTest);
    }
}
