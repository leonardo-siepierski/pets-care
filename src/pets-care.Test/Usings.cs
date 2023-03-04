global using Xunit;
global using FluentAssertions;

// dotnet test --collect "Xplat Code Coverage"
// reportgenerator "-reports:D:\Curso Trybe\Projetos empresas\pets-care\src\pets-care.Test\TestResults\11ecbc11-916d-433d-835a-a62ced258561\coverage.cobertura.xml" "-targetdir:./pets-care.Test/coveragereport" -reporttypes:Html